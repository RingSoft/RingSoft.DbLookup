using System.ComponentModel;
using System.Linq;
using RingSoft.DataEntryControls.Engine;
using RingSoft.DbLookup.App.Library.Northwind.LookupModel;
using RingSoft.DbLookup.App.Library.Northwind.Model;
using RingSoft.DbLookup.AutoFill;
using RingSoft.DbLookup.Lookup;
using RingSoft.DbLookup.ModelDefinition;
using RingSoft.DbLookup.QueryBuilder;
using RingSoft.DbMaintenance;

namespace RingSoft.DbLookup.App.Library.Northwind.ViewModels
{
    public class ProductViewModel : DbMaintenanceViewModel<Product>
    {
        public override TableDefinition<Product> TableDefinition =>
            RsDbLookupAppGlobals.EfProcessor.NorthwindLookupContext.Products;

        private int _productId;

        public int ProductId
        {
            get => _productId;
            set
            {
                if (_productId == value)
                    return;

                _productId = value;
                OnPropertyChanged(nameof(ProductId));
            }
        }

        private AutoFillSetup _supplierAutoFillSetup;

        public AutoFillSetup SupplierAutoFillSetup
        {
            get => _supplierAutoFillSetup;
            set
            {
                if (_supplierAutoFillSetup == value)
                    return;

                _supplierAutoFillSetup = value;
                OnPropertyChanged(nameof(SupplierAutoFillSetup));
            }
        }

        private AutoFillValue _supplierAutoFillValue;

        public AutoFillValue SupplierAutoFillValue
        {
            get => _supplierAutoFillValue;
            set
            {
                if (_supplierAutoFillValue == value)
                    return;

                _supplierAutoFillValue = value;
                OnPropertyChanged(nameof(SupplierAutoFillValue));
            }
        }

        private AutoFillSetup _categoryAutoFillSetup;

        public AutoFillSetup CategoryAutoFillSetup
        {
            get => _categoryAutoFillSetup;
            set
            {
                if (_categoryAutoFillSetup == value)
                    return;

                _categoryAutoFillSetup = value;
                OnPropertyChanged(nameof(CategoryAutoFillSetup));
            }
        }

        private AutoFillValue _categoryAutoFillValue;

        public AutoFillValue CategoryAutoFillValue
        {
            get => _categoryAutoFillValue;
            set
            {
                if (_supplierAutoFillValue == value)
                    return;

                _categoryAutoFillValue = value;
                OnPropertyChanged(nameof(CategoryAutoFillValue));
            }
        }

        private string _quantityPerUnit;

        public string QuantityPerUnit
        {
            get => _quantityPerUnit;
            set
            {
                if (_quantityPerUnit == value)
                    return;

                _quantityPerUnit = value;
                OnPropertyChanged(nameof(QuantityPerUnit));
            }
        }

        private double? _unitPrice;

        public double? UnitPrice
        {
            get => _unitPrice;
            set
            {
                if (_unitPrice == value)
                    return;

                _unitPrice = value;
                OnPropertyChanged(nameof(UnitPrice));
            }
        }

        private short? _unitsInStock;

        public short? UnitsInStock
        {
            get => _unitsInStock;
            set
            {
                if (_unitsInStock == value)
                    return;

                _unitsInStock = value;
                OnPropertyChanged(nameof(UnitsInStock));
            }
        }

        private short? _unitsOnOrder;

        public short? UnitsOnOrder
        {
            get => _unitsOnOrder;
            set
            {
                if (_unitsOnOrder == value)
                    return;

                _unitsOnOrder = value;
                OnPropertyChanged(nameof(UnitsOnOrder));
            }
        }

        private short? _reorderLevel;

        public short? ReorderLevel
        {
            get => _reorderLevel;
            set
            {
                if (_reorderLevel == value)
                    return;

                _reorderLevel = value;
                OnPropertyChanged(nameof(ReorderLevel));
            }
        }

        private bool _discontinued;

        public bool Discontinued
        {
            get => _discontinued;
            set
            {
                if (_discontinued == value)
                    return;

                _discontinued = value;

                OnPropertyChanged(nameof(Discontinued));
            }
        }

        private LookupDefinition<OrderDetailLookup, Order_Detail> _orderDetailsLookup;

        public LookupDefinition<OrderDetailLookup, Order_Detail> OrderDetailsLookupDefinition
        {
            get => _orderDetailsLookup;
            set
            {
                if (_orderDetailsLookup == value)
                    return;

                _orderDetailsLookup = value;
                OnPropertyChanged(nameof(OrderDetailsLookupDefinition), false);
            }
        }

        private LookupCommand _orderDetailsLookupCommand;

        public LookupCommand OrderDetailsLookupCommand
        {
            get => _orderDetailsLookupCommand;
            set
            {
                if (_orderDetailsLookupCommand == value)
                    return;

                _orderDetailsLookupCommand = value;
                OnPropertyChanged(nameof(OrderDetailsLookupCommand), false);
            }
        }

        internal NorthwindViewModelInput ViewModelInput { get; private set; }

        public bool GridMode { get; private set; }

        private INorthwindLookupContext _lookupContext;

        protected override void Initialize()
        {
            _lookupContext = RsDbLookupAppGlobals.EfProcessor.NorthwindLookupContext;

            if (LookupAddViewArgs != null && LookupAddViewArgs.InputParameter is NorthwindViewModelInput viewModelInput)
            {
                ViewModelInput = viewModelInput;
                if (viewModelInput.OrderInput != null)
                {
                    GridMode = viewModelInput.OrderInput.GridMode;
                }
            }
            else
            {
                ViewModelInput = new NorthwindViewModelInput();
            }

            ViewModelInput.ProductViewModels.Add(this);

            SupplierAutoFillSetup = new AutoFillSetup(TableDefinition.GetFieldDefinition(p => p.SupplierID))
            {
                AllowLookupAdd = false,
                AllowLookupView = false
            };
            CategoryAutoFillSetup = new AutoFillSetup(TableDefinition.GetFieldDefinition(p => p.CategoryID))
            {
                AllowLookupAdd = false,
                AllowLookupView = false
            };

            var orderDetailsLookupDefinition =
                new LookupDefinition<OrderDetailLookup, Order_Detail>(_lookupContext.OrderDetails);
            orderDetailsLookupDefinition.Include(p => p.Order)
                .AddVisibleColumnDefinition(p => p.Order, p => p.OrderName);
            orderDetailsLookupDefinition.Include(p => p.Order)
                .Include(p => p.Customer)
                .AddVisibleColumnDefinition(p => p.Customer, p => p.CompanyName);
            orderDetailsLookupDefinition.AddVisibleColumnDefinition(p => p.Quantity, p => p.Quantity);
            orderDetailsLookupDefinition.AddVisibleColumnDefinition(p => p.UnitPrice, p => p.UnitPrice);
            OrderDetailsLookupDefinition = orderDetailsLookupDefinition;

            base.Initialize();
        }

        protected override Product PopulatePrimaryKeyControls(Product newEntity, PrimaryKeyValue primaryKeyValue)
        {
            ProductId = newEntity.ProductID;
            var product = RsDbLookupAppGlobals.EfProcessor.NorthwindEfDataProcessor.GetProduct(ProductId);

            KeyAutoFillValue = new AutoFillValue(primaryKeyValue, product.ProductName);

            _orderDetailsLookup.FilterDefinition.ClearFixedFilters();
            _orderDetailsLookup.FilterDefinition.AddFixedFilter(p => p.ProductID, Conditions.Equals, ProductId);

            var orderInput = new OrderInput
            {
                GridMode = GridMode,
                ProductId = ProductId
            };

            if (!GridMode)
            {
                orderInput.FromProductOrders = true;
            }

            ViewModelInput.OrderInput = orderInput;
            OrderDetailsLookupCommand = GetLookupCommand(LookupCommands.Refresh, primaryKeyValue, ViewModelInput);

            ReadOnlyMode =
                ViewModelInput.ProductViewModels.Any(
                    a => a != this && a.ProductId == ProductId);

            return product;
        }

        protected override void LoadFromEntity(Product entity)
        {
            PrimaryKeyValue primaryKey;
            if (entity.Supplier != null)
            {
                primaryKey = _lookupContext.Suppliers.GetPrimaryKeyValueFromEntity(entity.Supplier);
                SupplierAutoFillValue = new AutoFillValue(primaryKey, entity.Supplier.CompanyName);
            }
            else
            {
                SupplierAutoFillValue = null;
            }

            if (entity.Category != null)
            {
                primaryKey = _lookupContext.Categories.GetPrimaryKeyValueFromEntity(entity.Category);
                CategoryAutoFillValue = new AutoFillValue(primaryKey, entity.Category.CategoryName);
            }
            else
            {
                CategoryAutoFillValue = null;
            }

            QuantityPerUnit = entity.QuantityPerUnit;
            UnitPrice = entity.UnitPrice;
            UnitsInStock = entity.UnitsInStock;
            UnitsOnOrder = entity.UnitsOnOrder;
            ReorderLevel = entity.ReorderLevel;
            Discontinued = entity.Discontinued;

            if (ReadOnlyMode)
                ControlsGlobals.UserInterface.ShowMessageBox(
                    "This Product is being modified in another window.  Editing not allowed.", "Editing not allowed",
                    RsMessageBoxIcons.Exclamation);
        }

        protected override Product GetEntityData()
        {
            var product = new Product
            {
                ProductID = ProductId,
                ProductName = KeyAutoFillValue.Text,
                QuantityPerUnit = QuantityPerUnit,
                UnitPrice = (float?)UnitPrice,
                UnitsInStock = UnitsInStock,
                UnitsOnOrder = UnitsOnOrder,
                ReorderLevel = ReorderLevel,
                Discontinued = Discontinued
            };

            if (CategoryAutoFillValue != null)
            {
                product.CategoryID = _lookupContext.Categories
                    .GetEntityFromPrimaryKeyValue(CategoryAutoFillValue.PrimaryKeyValue).CategoryId;
            }

            if (SupplierAutoFillValue != null)
            {
                product.SupplierID = _lookupContext.Suppliers
                    .GetEntityFromPrimaryKeyValue(SupplierAutoFillValue.PrimaryKeyValue).SupplierID;
            }

            return product;
        }

        protected override void ClearData()
        {
            ProductId = 0;
            QuantityPerUnit = string.Empty;

            CategoryAutoFillValue = null;
            SupplierAutoFillValue = null;
            UnitPrice = null;
            UnitsInStock = UnitsOnOrder = ReorderLevel = null;
            Discontinued = false;
            OrderDetailsLookupCommand = GetLookupCommand(LookupCommands.Clear);
        }

        protected override bool SaveEntity(Product entity)
        {
            return RsDbLookupAppGlobals.EfProcessor.NorthwindEfDataProcessor.SaveProduct(entity);
        }

        protected override bool DeleteEntity()
        {
            return RsDbLookupAppGlobals.EfProcessor.NorthwindEfDataProcessor.DeleteProduct(ProductId);
        }

        public override void OnWindowClosing(CancelEventArgs e)
        {
            base.OnWindowClosing(e);
            if (!e.Cancel)
                ViewModelInput.ProductViewModels.Remove(this);
        }
    }
}
