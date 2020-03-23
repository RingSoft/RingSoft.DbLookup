using RSDbLookupApp.Library.Northwind.LookupModel;
using RSDbLookupApp.Library.Northwind.Model;
using System;
using RingSoft.DbLookup.AutoFill;
using RingSoft.DbLookup.Lookup;
using RingSoft.DbLookup.ModelDefinition;
using RingSoft.DbMaintenance;

namespace RSDbLookupApp.Library.Northwind.ViewModels
{
    public class OrderDetailsViewModel : DbMaintenanceViewModel<Order_Detail>
    {
        public override TableDefinition<Order_Detail> TableDefinition =>
            RsDbLookupAppGlobals.EfProcessor.NorthwindLookupContext.OrderDetails;

        private int _orderId;

        public int OrderId
        {
            get => _orderId;
            set
            {
                if (_orderId == value)
                    return;

                _orderId = value;
                OnPropertyChanged(nameof(OrderId));
            }
        }

        private string _customer;
        public string Customer
        {
            get => _customer;
            set
            {
                if (_customer == value)
                    return;

                _customer = value;
                OnPropertyChanged(nameof(Customer));
            }
        }

        private DateTime _orderDate;

        public DateTime OrderDate
        {
            get => _orderDate;
            set
            {
                if (_orderDate == value)
                    return;

                _orderDate = value;
                OnPropertyChanged(nameof(OrderDate));
            }
        }

        private AutoFillSetup _productAutoFillSetup;
        public AutoFillSetup ProductAutoFillSetup => _productAutoFillSetup;

        private AutoFillValue _productAutoFillValue;
        public AutoFillValue ProductAutoFillValue
        {
            get => _productAutoFillValue;
            set
            {
                if (_productAutoFillValue == value)
                    return;

                _productAutoFillValue = value;
                _productDirty = true;
                OnPropertyChanged(nameof(ProductAutoFillValue));
            }
        }

        private short _quantity;
        public short Quantity
        {
            get => _quantity;
            set
            {
                if (_quantity == value)
                    return;

                _quantity = value;
                UpdateExtPrice();
                OnPropertyChanged(nameof(Quantity));
            }
        }

        private decimal _price;

        public decimal Price
        {
            get => _price;
            set
            {
                if (_price == value)
                    return;

                _price = value;
                UpdateExtPrice();
                OnPropertyChanged(nameof(Price));
            }
        }

        private decimal _extPrice;

        public decimal ExtPrice
        {
            get => _extPrice;
            set
            {
                if (_extPrice == value)
                    return;

                _extPrice = value;
                OnPropertyChanged(nameof(ExtPrice));
            }
        }

        private decimal _discount;
        public decimal Discount
        {
            get => _discount;
            set
            {
                if (_discount == value)
                    return;

                _discount = value;
                OnPropertyChanged(nameof(Discount));
            }
        }

        private INorthwindLookupContext _lookupContext;
        private bool _productDirty;

        protected override void Initialize()
        {
            _lookupContext = RsDbLookupAppGlobals.EfProcessor.NorthwindLookupContext;

            FindButtonLookupDefinition = _lookupContext.NorthwindContextConfiguration.OrderDetailsFormLookup.Clone();
            _productAutoFillSetup = new AutoFillSetup(TableDefinition.GetFieldDefinition(p => p.ProductID));

            if (ViewLookupDefinition is LookupDefinition<OrderDetailLookup, Order_Detail> orderDetailLookup)
            {
                var sortColumn = orderDetailLookup.GetColumnDefinition(p => p.Product);
                ChangeSortColumn(sortColumn);
            }

            FindButtonLookupDefinition.FilterDefinition.CopyFrom(ViewLookupDefinition.FilterDefinition);

            base.Initialize();
        }

        private void UpdateExtPrice()
        {
            ExtPrice = Quantity * Price;
        }

        protected override string FindButtonInitialSearchFor
        {
            get
            {
                if (ProductAutoFillValue != null)
                    return ProductAutoFillValue.Text;

                return string.Empty;
            }
        }

        public override void OnKeyControlLeave()
        {
            if (ProductAutoFillValue != null && ProductAutoFillValue.PrimaryKeyValue.ContainsValidData())
            {
                var product =
                    _lookupContext.Products.GetEntityFromPrimaryKeyValue(ProductAutoFillValue.PrimaryKeyValue);
                var orderDetail =
                    RsDbLookupAppGlobals.EfProcessor.NorthwindEfDataProcessor.GetOrderDetail(OrderId,
                        product.ProductID);

                if (orderDetail != null)
                {
                    SelectPrimaryKey(_lookupContext.OrderDetails.GetPrimaryKeyValueFromEntity(orderDetail));
                }
                else
                {
                    LoadProductData();
                }
            }
        }

        private void LoadProductData()
        {
            if (_productDirty && ProductAutoFillValue != null && ProductAutoFillValue.PrimaryKeyValue.ContainsValidData())
            {
                var product =
                    _lookupContext.Products.GetEntityFromPrimaryKeyValue(ProductAutoFillValue.PrimaryKeyValue);
                product = RsDbLookupAppGlobals.EfProcessor.NorthwindEfDataProcessor.GetProduct(product.ProductID);
                if (product != null && product.UnitPrice != null)
                {
                    Price = (decimal)product.UnitPrice;
                    Quantity = 1;
                }

                _productDirty = false;
            }
        }
        protected override void LoadFromEntity(Order_Detail newEntity)
        {
            var orderDetail =
                RsDbLookupAppGlobals.EfProcessor.NorthwindEfDataProcessor.GetOrderDetail(newEntity.OrderID,
                    newEntity.ProductID);

            ProductAutoFillValue =
                new AutoFillValue(_lookupContext.Products.GetPrimaryKeyValueFromEntity(orderDetail.Product),
                    orderDetail.Product.ProductName);
            Quantity = orderDetail.Quantity;
            Price = orderDetail.UnitPrice;
            Discount = (decimal) orderDetail.Discount;
            _productDirty = false;
        }

        protected override Order_Detail GetEntityData()
        {
            var orderDetail = new Order_Detail();
            orderDetail.OrderID = OrderId;
            if (ProductAutoFillValue != null && ProductAutoFillValue.PrimaryKeyValue.ContainsValidData())
            {
                var product =
                    _lookupContext.Products.GetEntityFromPrimaryKeyValue(ProductAutoFillValue.PrimaryKeyValue);
                orderDetail.ProductID = product.ProductID;
            }

            orderDetail.Quantity = Quantity;
            orderDetail.UnitPrice = Price;
            orderDetail.Discount = (float)Discount;
            return orderDetail;
        }

        protected override void ClearData()
        {
            if (LookupAddViewArgs != null && LookupAddViewArgs.ParentWindowPrimaryKeyValue != null)
            {
                var order = _lookupContext.Orders.GetEntityFromPrimaryKeyValue(LookupAddViewArgs
                    .ParentWindowPrimaryKeyValue);
                order = RsDbLookupAppGlobals.EfProcessor.NorthwindEfDataProcessor.GetOrder(order.OrderID);
                LoadFromOrder(order);
            }

            ProductAutoFillValue = null;
            Quantity = 0;
            Price = 0;
            Discount = 0;
            _productDirty = false;
        }

        private void LoadFromOrder(Order order)
        {
            OrderId = order.OrderID;
            if (order.Customer == null)
                Customer = string.Empty;
            else
                Customer = order.Customer.CompanyName;
            if (order.OrderDate == null)
                OrderDate = new DateTime(1980, 1, 1);
            else
                OrderDate = (DateTime)order.OrderDate;
        }
        protected override bool SaveEntity(Order_Detail entity)
        {
            return RsDbLookupAppGlobals.EfProcessor.NorthwindEfDataProcessor.SaveOrderDetail(entity);
        }

        protected override bool DeleteEntity()
        {
            var product = _lookupContext.Products.GetEntityFromPrimaryKeyValue(ProductAutoFillValue.PrimaryKeyValue);
            return RsDbLookupAppGlobals.EfProcessor.NorthwindEfDataProcessor.DeleteOrderDetail(OrderId,
                product.ProductID);
        }
    }
}
