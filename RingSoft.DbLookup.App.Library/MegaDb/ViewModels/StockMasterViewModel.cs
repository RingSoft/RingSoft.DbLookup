using System.Linq;
using RingSoft.DataEntryControls.Engine;
using RingSoft.DbLookup.App.Library.MegaDb.LookupModel;
using RingSoft.DbLookup.App.Library.MegaDb.Model;
using RingSoft.DbLookup.AutoFill;
using RingSoft.DbLookup.Lookup;
using RingSoft.DbLookup.ModelDefinition;
using RingSoft.DbLookup.QueryBuilder;
using RingSoft.DbMaintenance;

namespace RingSoft.DbLookup.App.Library.MegaDb.ViewModels
{
    public interface IStockMasterView
    {
        void ShowAdvancedFind();
    }
    public class StockMasterViewModel : DbMaintenanceViewModel<StockMaster>
    {
        #region Properties

        private int _id;

        public int Id
        {
            get => _id;
            set
            {
                if (_id == value)
                {
                    return;
                }
                _id = value;
                OnPropertyChanged();
            }
        }

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

        private double _price;

        public double Price
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

        #endregion

        public UiCommand StockUiCommand { get; } = new UiCommand();

        public UiCommand LocationUiCommand { get; } = new UiCommand();

        public UiCommand PriceUiCommand { get; } = new UiCommand();

        public RelayCommand AdvFindCommand { get; }

        public RelayCommand AddViewCommand { get; }

        public IStockMasterView View { get; private set; }

        private IMegaDbLookupContext _lookupContext;

        public StockMasterViewModel()
        {
            TablesToDelete.Add(RsDbLookupAppGlobals.EfProcessor.MegaDbLookupContext.StocksTable);
            TablesToDelete.Add(RsDbLookupAppGlobals.EfProcessor.MegaDbLookupContext.MliLocationsTable);
            StockUiCommand.LostFocus += (sender, args) =>
            {
                StockLocationLeave();
            };

            LocationUiCommand.LostFocus += (sender, args) =>
            {
                StockLocationLeave();
            };

            AdvFindCommand = new RelayCommand((() =>
            {
                View.ShowAdvancedFind();
            }));

            AddViewCommand = new RelayCommand((() =>
            {
                OnAddModify();
            }));
        }

        protected override void Initialize()
        {
            _lookupContext = RsDbLookupAppGlobals.EfProcessor.MegaDbLookupContext;

            if (base.View is IStockMasterView stockMasterView)
            {
                View = stockMasterView;
            }

            StockNumberAutoFillSetup = new AutoFillSetup(
                TableDefinition.GetFieldDefinition(p => p.StockId))
            {
                AllowLookupAdd = false,
                AllowLookupView = false
            };


            LocationAutoFillSetup = new AutoFillSetup(
                TableDefinition.GetFieldDefinition(p => p.MliLocationId))
            {
                AllowLookupAdd = false,
                AllowLookupView = false
            };

            StockCostQuantityLookupDefinition =
                _lookupContext.MegaDbContextConfiguration.StockCostQuantityLookupFiltered.Clone();

            base.Initialize();
        }

        public override void OnNewButton()
        {
            base.OnNewButton();
            StockUiCommand.IsEnabled = true;
            LocationUiCommand.IsEnabled = true;

            StockUiCommand.SetFocus();
        }

        protected override StockMaster PopulatePrimaryKeyControls(StockMaster newEntity, PrimaryKeyValue primaryKeyValue)
        {
            Id = newEntity.Id;

            StockCostQuantityLookupDefinition.FilterDefinition.ClearFixedFilters();
            StockCostQuantityLookupDefinition
                .FilterDefinition
                .AddFixedFilter(p => p.StockMasterId, Conditions.Equals, newEntity.Id);

            StockCostQuantityCommand = GetLookupCommand(LookupCommands.Refresh, primaryKeyValue);

            if (StockUiCommand.IsFocused || LocationUiCommand.IsFocused)
            {
                PriceUiCommand.SetFocus();
            }

            StockUiCommand.IsEnabled = false;
            LocationUiCommand.IsEnabled = false;

            return base.PopulatePrimaryKeyControls(newEntity, primaryKeyValue);
        }

        protected override void LoadFromEntity(StockMaster entity)
        {
            StockNumberAutoFillValue = entity.Stock.GetAutoFillValue();
            LocationAutoFillValue = entity.MliLocation.GetAutoFillValue();
            Price = entity.Price;
        }

        protected override StockMaster GetEntityData()
        {
            var stockId = StockNumberAutoFillValue.GetEntity<StocksTable>().Id;
            var locationId = LocationAutoFillValue.GetEntity<MliLocationsTable>().Id;
            var context = SystemGlobals.DataRepository.GetDataContext();

            if (stockId == 0 && !StockNumberAutoFillValue.Text.IsNullOrEmpty())
            {
                var stock = new StocksTable
                {
                    Name = StockNumberAutoFillValue.Text,
                };
                context.SaveEntity(stock, "Adding Stock Item");
                stockId = stock.Id;
                StockNumberAutoFillValue = stock.GetAutoFillValue();
            }

            if (locationId == 0 && !LocationAutoFillValue.Text.IsNullOrEmpty())
            {
                var location = new MliLocationsTable
                {
                    Name = LocationAutoFillValue.Text,
                };
                context.SaveEntity(location, "Adding Location");
                locationId = location.Id;
                LocationAutoFillValue = location.GetAutoFillValue();
            }

            var entity = new StockMaster
            {
                Id = Id,
                StockId = stockId,
                MliLocationId = locationId,
                Price = Price,
            };
            return entity;
        }

        protected override void ClearData()
        {
            Id = 0;
            StockNumberAutoFillValue = null;
            LocationAutoFillValue = null;
            Price = 0;
            StockCostQuantityCommand = GetLookupCommand(LookupCommands.Clear);
        }

        protected override bool SaveEntity(StockMaster entity)
        {
            var context = SystemGlobals.DataRepository.GetDataContext();
            var result = context.SaveEntity(entity, "Saving Stock Master");
            return result;
        }

        protected override bool DeleteEntity()
        {
            var context = SystemGlobals.DataRepository.GetDataContext();
            var table = context.GetTable<StockMaster>();
            var stockMaster = table
                .FirstOrDefault(p => p.Id == Id);

            var result = true;
            if (stockMaster != null)
            {
                result = context.DeleteEntity(stockMaster, "Deleting Stock Master");
            }

            if (result)
            {
                if (!table.Any(p => p.StockId == stockMaster.StockId))
                {
                    var stocksTable = context.GetTable<StocksTable>();
                    var stockRecord = stocksTable
                        .FirstOrDefault(p => p.Id == stockMaster.StockId);
                    if (stockRecord != null)
                    {
                        result = context.DeleteEntity(stockRecord, "Deleting Stock");
                    }
                }
            }

            if (result)
            {
                if (!table.Any(p => p.MliLocationId == stockMaster.MliLocationId))
                {
                    var locationsTable = context.GetTable<MliLocationsTable>();
                    var locationRecord = locationsTable
                        .FirstOrDefault(p => p.Id == stockMaster.MliLocationId);
                    if (locationRecord != null)
                    {
                        result = context.DeleteEntity(locationRecord, "Deleting Location");
                    }
                }
            }

            return result;
        }

        private void OnAddModify()
        {
            if (ExecuteAddModifyCommand() == DbMaintenanceResults.Success)
                StockCostQuantityCommand = GetLookupCommand(LookupCommands.AddModify);
        }

        private void StockLocationLeave()
        {
            if (StockNumberAutoFillValue.IsValid() && LocationAutoFillValue.IsValid())
            {
                var context = SystemGlobals.DataRepository.GetDataContext();
                var table = context.GetTable<StockMaster>();
                var stockMasterRecord = table.FirstOrDefault(
                    p => p.StockId == StockNumberAutoFillValue.GetEntity<StocksTable>().Id
                         && p.MliLocationId == LocationAutoFillValue.GetEntity<MliLocationsTable>().Id);

                if (stockMasterRecord != null
                    && stockMasterRecord.Id != Id)
                {
                    KeyAutoFillValue = stockMasterRecord.GetAutoFillValue();
                    KeyValueDirty = true;
                    OnKeyControlLeave();
                }
            }
        }
    }
}
