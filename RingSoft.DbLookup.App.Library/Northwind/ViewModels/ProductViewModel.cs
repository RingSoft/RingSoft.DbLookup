using RingSoft.DbLookup.App.Library.Northwind.Model;
using RingSoft.DbLookup.AutoFill;
using RingSoft.DbLookup.ModelDefinition;
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

        private decimal? _unitPrice;

        public decimal? UnitPrice
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

        private decimal? _unitsInStock;

        public decimal? UnitsInStock
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

        private decimal? _unitsOnOrder;

        public decimal? UnitsOnOrder
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

        private decimal? _reorderLevel;

        public decimal? ReorderLevel
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

        private INorthwindLookupContext _lookupContext;

        protected override void Initialize()
        {
            _lookupContext = RsDbLookupAppGlobals.EfProcessor.NorthwindLookupContext;
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

            base.Initialize();
        }

        protected override Product PopulatePrimaryKeyControls(Product newEntity, PrimaryKeyValue primaryKeyValue)
        {
            ProductId = newEntity.ProductID;
            var product = RsDbLookupAppGlobals.EfProcessor.NorthwindEfDataProcessor.GetProduct(ProductId);

            KeyAutoFillValue = new AutoFillValue(primaryKeyValue, product.ProductName);
            return product;
        }

        protected override void LoadFromEntity(Product entity)
        {
            throw new System.NotImplementedException();
        }

        protected override Product GetEntityData()
        {
            throw new System.NotImplementedException();
        }

        protected override void ClearData()
        {
            ProductId = 0;
            QuantityPerUnit = string.Empty;

            CategoryAutoFillValue = SupplierAutoFillValue = null;
            UnitPrice = UnitsInStock = UnitsOnOrder = ReorderLevel = null;
            Discontinued = false;
        }

        protected override bool SaveEntity(Product entity)
        {
            throw new System.NotImplementedException();
        }

        protected override bool DeleteEntity()
        {
            throw new System.NotImplementedException();
        }
    }
}
