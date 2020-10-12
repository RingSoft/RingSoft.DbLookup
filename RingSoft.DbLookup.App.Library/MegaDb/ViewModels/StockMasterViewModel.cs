using RingSoft.DbLookup.App.Library.MegaDb.LookupModel;
using RingSoft.DbLookup.App.Library.MegaDb.Model;
using RingSoft.DbLookup.AutoFill;
using RingSoft.DbLookup.Lookup;
using RingSoft.DbLookup.ModelDefinition;
using RingSoft.DbLookup.QueryBuilder;
using RingSoft.DbMaintenance;

namespace RingSoft.DbLookup.App.Library.MegaDb.ViewModels
{
    public class StockMasterViewModel : DbMaintenanceViewModel<StockMaster>
    {
        public override TableDefinition<StockMaster> TableDefinition =>
            RsDbLookupAppGlobals.EfProcessor.MegaDbLookupContext.Stocks;

        private AutoFillSetup _stockNumberAutoFillSetup;

        public AutoFillSetup StockNumberAutoFillSetup
        {
            get => _stockNumberAutoFillSetup;
            set
            {
                if (_stockNumberAutoFillSetup == value)
                    return;

                _stockNumberAutoFillSetup = value;
                OnPropertyChanged(nameof(StockNumberAutoFillSetup));
            }
        }

        private AutoFillValue _stockNumberAutoFillValue;

        public AutoFillValue StockNumberAutoFillValue
        {
            get => _stockNumberAutoFillValue;
            set
            {
                if (_stockNumberAutoFillValue == value)
                    return;

                _stockNumberAutoFillValue = value;
                OnPropertyChanged(nameof(StockNumberAutoFillValue));
            }
        }

        private AutoFillSetup _locationAutoFillSetup;

        public AutoFillSetup LocationAutoFillSetup
        {
            get => _locationAutoFillSetup;
            set
            {
                if (_locationAutoFillSetup == value)
                    return;

                _locationAutoFillSetup = value;
                OnPropertyChanged(nameof(LocationAutoFillSetup));
            }
        }

        private AutoFillValue _locationAutoFillValue;

        public AutoFillValue LocationAutoFillValue
        {
            get => _locationAutoFillValue;
            set
            {
                if (_locationAutoFillValue == value)
                    return;

                _locationAutoFillValue = value;
                OnPropertyChanged(nameof(LocationAutoFillValue));
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
                OnPropertyChanged(nameof(Price));
            }
        }

        private LookupDefinition<StockCostQuantityLookup, StockCostQuantity> _stockCostQuantityLookupDefinition;

        public LookupDefinition<StockCostQuantityLookup, StockCostQuantity> StockCostQuantityLookupDefinition
        {
            get => _stockCostQuantityLookupDefinition;
            set
            {
                if (_stockCostQuantityLookupDefinition == value)
                    return;

                _stockCostQuantityLookupDefinition = value;
                OnPropertyChanged(nameof(StockCostQuantityLookupDefinition));
            }
        }

        private LookupCommand _stockCostQuantityLookupCommand;

        public LookupCommand StockCostQuantityCommand
        {
            get => _stockCostQuantityLookupCommand;
            set
            {
                if (_stockCostQuantityLookupCommand == value)
                    return;

                _stockCostQuantityLookupCommand = value;
                OnPropertyChanged(nameof(StockCostQuantityCommand), false);
            }
        }

        private IMegaDbLookupContext _lookupContext;

        protected override void Initialize()
        {
            _lookupContext = RsDbLookupAppGlobals.EfProcessor.MegaDbLookupContext;

            var stockLookupDefinition = new LookupDefinition<StockMasterLookup, StockMaster>(_lookupContext.Stocks);
            stockLookupDefinition.AddVisibleColumnDefinition(p => p.StockNumber, "Stock Number", p => p.StockNumber,
                99).IsDistinct();

            StockNumberAutoFillSetup = new AutoFillSetup(stockLookupDefinition)
            {
                AllowLookupAdd = false,
                AllowLookupView = false
            };

            var locationLookupDefinition = new LookupDefinition<StockMasterLookup, StockMaster>(_lookupContext.Stocks);
            locationLookupDefinition.Title = "Stock Locations";
            locationLookupDefinition.AddVisibleColumnDefinition(p => p.Location, "Location", p => p.Location, 99)
                .IsDistinct();

            LocationAutoFillSetup = new AutoFillSetup(locationLookupDefinition)
            {
                AllowLookupAdd = false,
                AllowLookupView = false
            };

            StockCostQuantityLookupDefinition =
                _lookupContext.MegaDbContextConfiguration.StockCostQuantityLookupFiltered.Clone();

            base.Initialize();
        }

        public override void OnKeyControlLeave()
        {
            if (StockNumberAutoFillValue != null && LocationAutoFillValue != null)
            {
                var stockItem =
                    RsDbLookupAppGlobals.EfProcessor.MegaDbEfDataProcessor.GetStockItem(StockNumberAutoFillValue.Text,
                        LocationAutoFillValue.Text);
                if (stockItem != null)
                    SelectPrimaryKey(_lookupContext.Stocks.GetPrimaryKeyValueFromEntity(stockItem));
            }
        }

        protected override StockMaster PopulatePrimaryKeyControls(StockMaster newEntity, PrimaryKeyValue primaryKeyValue)
        {
            StockNumberAutoFillValue = new AutoFillValue(primaryKeyValue, newEntity.StockNumber);
            LocationAutoFillValue = new AutoFillValue(primaryKeyValue, newEntity.Location);

            var stockItem =
                RsDbLookupAppGlobals.EfProcessor.MegaDbEfDataProcessor.GetStockItem(newEntity.StockNumber,
                    newEntity.Location);

            StockCostQuantityCommand = GetLookupCommand(LookupCommands.Refresh, primaryKeyValue);

            return stockItem;
        }

        protected override void LoadFromEntity(StockMaster entity)
        {
            Price = entity.Price;

            _stockCostQuantityLookupDefinition.FilterDefinition.ClearFixedFilters();
            _stockCostQuantityLookupDefinition.FilterDefinition.AddFixedFilter(p => p.StockNumber, Conditions.Equals,
                StockNumberAutoFillValue.Text);
            _stockCostQuantityLookupDefinition.FilterDefinition.AddFixedFilter(p => p.Location, Conditions.Equals,
                LocationAutoFillValue.Text);
        }

        protected override StockMaster GetEntityData()
        {
            var stockItem = new StockMaster();

            if (StockNumberAutoFillValue != null)
                stockItem.StockNumber = StockNumberAutoFillValue.Text;

            if (LocationAutoFillValue != null)
                stockItem.Location = LocationAutoFillValue.Text;

            stockItem.Price = Price;

            return stockItem;
        }

        protected override void ClearData()
        {
            StockNumberAutoFillValue = LocationAutoFillValue = null;
            Price = 0;
            StockCostQuantityCommand = GetLookupCommand(LookupCommands.Clear);
        }

        protected override bool SaveEntity(StockMaster entity)
        {
            return RsDbLookupAppGlobals.EfProcessor.MegaDbEfDataProcessor.SaveStockItem(entity);
        }

        protected override bool DeleteEntity()
        {
            return RsDbLookupAppGlobals.EfProcessor.MegaDbEfDataProcessor.DeleteStockItem(
                StockNumberAutoFillValue.Text, LocationAutoFillValue.Text);
        }

        public void OnAddModify()
        {
            if (ExecuteAddModifyCommand() == DbMaintenanceResults.Success)
                StockCostQuantityCommand = GetLookupCommand(LookupCommands.AddModify);
        }
    }
}
