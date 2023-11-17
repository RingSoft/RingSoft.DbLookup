using System;
using System.ComponentModel;
using System.Linq;
using RingSoft.DataEntryControls.Engine;
using RingSoft.DbLookup.App.Library.Northwind.LookupModel;
using RingSoft.DbLookup.App.Library.Northwind.Model;
using RingSoft.DbLookup.AutoFill;
using RingSoft.DbLookup.Lookup;
using RingSoft.DbLookup.ModelDefinition;
using RingSoft.DbMaintenance;

namespace RingSoft.DbLookup.App.Library.Northwind.ViewModels
{
    public class OrderDetailsViewModel : DbMaintenanceViewModel<Order_Detail>
    {
        #region Properties

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

        public int ProductId { get; set; }

        private AutoFillSetup _productAutoFillSetup;
        public AutoFillSetup ProductAutoFillSetup
        {
            get => _productAutoFillSetup;
            set
            {
                if (_productAutoFillSetup == value)
                    return;

                _productAutoFillSetup = value;
                OnPropertyChanged(nameof(ProductAutoFillSetup));
            }
        }

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

        private double _price;

        public double Price
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

        private double _extPrice;

        public double ExtPrice
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

        private double _discount;
        public double Discount
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

        #endregion

        internal NorthwindViewModelInput ViewModelInput { get; private set; }

        public UiCommand ProductUiCommand { get; } = new UiCommand();

        private INorthwindLookupContext _lookupContext;
        private bool _productDirty;

        public OrderDetailsViewModel()
        {
            _orderDate = new DateTime(1980,1,1);
            
        }
        protected override void Initialize()
        {
            _lookupContext = RsDbLookupAppGlobals.EfProcessor.NorthwindLookupContext;

            if (LookupAddViewArgs != null && LookupAddViewArgs.InputParameter is NorthwindViewModelInput viewModelInput)
            {
                ViewModelInput = viewModelInput;
            }
            else
            {
                ViewModelInput = new NorthwindViewModelInput();
            }

            ViewModelInput.OrderDetailsViewModels.Add(this);

            FindButtonLookupDefinition = _lookupContext.NorthwindContextConfiguration.OrderDetailsFormLookup.Clone();
            ProductAutoFillSetup = new AutoFillSetup(TableDefinition.GetFieldDefinition(p => p.ProductID))
            {
                AddViewParameter = ViewModelInput
            };

            if (ViewLookupDefinition is LookupDefinition<OrderDetailLookup, Order_Detail> orderDetailLookup)
            {
                var sortColumn = orderDetailLookup.GetColumnDefinition(p => p.Product);
                ChangeSortColumn(sortColumn);
            }

            FindButtonLookupDefinition.FilterDefinition.CopyFrom(ViewLookupDefinition.FilterDefinition);

            ProductUiCommand.LostFocus += ProductUiCommand_LostFocus;

            base.Initialize();
        }

        private void ProductUiCommand_LostFocus(object sender, UiLostFocusArgs e)
        {
            OnKeyControlLeave();
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

        public override void OnNewButton()
        {
            ProductUiCommand.IsEnabled = true;
            base.OnNewButton();
        }

        public override void OnKeyControlLeave()
        {
            if (ProductAutoFillValue != null && ProductAutoFillValue.PrimaryKeyValue.IsValid())
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
            if (_productDirty && ProductAutoFillValue != null && ProductAutoFillValue.PrimaryKeyValue.IsValid())
            {
                var product =
                    _lookupContext.Products.GetEntityFromPrimaryKeyValue(ProductAutoFillValue.PrimaryKeyValue);
                product = RsDbLookupAppGlobals.EfProcessor.NorthwindEfDataProcessor.GetProduct(product.ProductID);
                if (product != null && product.UnitPrice != null)
                {
                    Price = (double)product.UnitPrice;
                    Quantity = 1;
                }

                _productDirty = false;
            }
        }

        protected override Order_Detail PopulatePrimaryKeyControls(Order_Detail newEntity, PrimaryKeyValue primaryKeyValue)
        {
            ProductId = newEntity.ProductID;
            var orderDetail =
                RsDbLookupAppGlobals.EfProcessor.NorthwindEfDataProcessor.GetOrderDetail(newEntity.OrderID,
                    newEntity.ProductID);

            ProductAutoFillValue =
                new AutoFillValue(_lookupContext.Products.GetPrimaryKeyValueFromEntity(orderDetail.Product),
                    orderDetail.Product.ProductName);

            ReadOnlyMode =
                ViewModelInput.OrderDetailsViewModels.Any(
                    a => a != this && a.OrderId == OrderId && a.ProductId == ProductId);

            if (LookupAddViewArgs != null)
            {
                if (LookupAddViewArgs.LookupReadOnlyMode)
                {
                    ReadOnlyMode = true;
                    NewButtonEnabled = false;
                }

            }

            ProductUiCommand.IsEnabled = false;
            return orderDetail;
        }

        protected override void LoadFromEntity(Order_Detail entity)
        {
            LoadFromOrder(entity.Order);
            Quantity = entity.Quantity;
            Price = entity.UnitPrice;
            Discount = (double)entity.Discount;
            _productDirty = false;

            if (ReadOnlyMode)
                ControlsGlobals.UserInterface.ShowMessageBox(
                    "This Order Detail is being modified in another window.  Editing not allowed.", "Editing not allowed",
                    RsMessageBoxIcons.Exclamation);
        }

        protected override Order_Detail GetEntityData()
        {
            var orderDetail = new Order_Detail();
            orderDetail.OrderID = OrderId;
            if (ProductAutoFillValue != null && ProductAutoFillValue.PrimaryKeyValue.IsValid())
            {
                var product =
                    _lookupContext.Products.GetEntityFromPrimaryKeyValue(ProductAutoFillValue.PrimaryKeyValue);
                orderDetail.ProductID = product.ProductID;
            }

            orderDetail.Quantity = Quantity;
            orderDetail.UnitPrice = (float)Price;
            orderDetail.Discount = (float)Discount;
            return orderDetail;
        }

        protected override void ClearData()
        {
            if (LookupAddViewArgs != null && LookupAddViewArgs.ParentWindowPrimaryKeyValue != null)
            {
                var order = _lookupContext.Orders.GetEntityFromPrimaryKeyValue(LookupAddViewArgs
                    .ParentWindowPrimaryKeyValue);
                order = RsDbLookupAppGlobals.EfProcessor.NorthwindEfDataProcessor.GetOrder(order.OrderID, false);
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

        public override void OnWindowClosing(CancelEventArgs e)
        {
            base.OnWindowClosing(e);
            if (!e.Cancel)
                ViewModelInput.OrderDetailsViewModels.Remove(this);
        }

        protected override void SetupPrinterArgs(PrinterSetupArgs printerSetupArgs, int stringFieldIndex = 1, int numericFieldIndex = 1,
            int memoFieldIndex = 1)
        {
            printerSetupArgs.CodeAutoFillSetup = null;
            base.SetupPrinterArgs(printerSetupArgs, stringFieldIndex, numericFieldIndex, memoFieldIndex);
        }
    }
}
