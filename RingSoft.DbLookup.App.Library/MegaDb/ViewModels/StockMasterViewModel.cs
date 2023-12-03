using System.Linq;
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

        private IMegaDbLookupContext _lookupContext;

        protected override void Initialize()
        {
            _lookupContext = RsDbLookupAppGlobals.EfProcessor.MegaDbLookupContext;

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

        protected override StockMaster PopulatePrimaryKeyControls(StockMaster newEntity, PrimaryKeyValue primaryKeyValue)
        {
            Id = newEntity.Id;
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
            var entity = new StockMaster
            {
                Id = Id,
                StockId = StockNumberAutoFillValue.GetEntity<StocksTable>().Id,
                MliLocationId = LocationAutoFillValue.GetEntity<MliLocationsTable>().Id,
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
            if (stockMaster != null)
            {
                return context.DeleteEntity(stockMaster, "Deleting Stock Master");
            }

            return true;
        }


        public void OnAddModify()
        {
            if (ExecuteAddModifyCommand() == DbMaintenanceResults.Success)
                StockCostQuantityCommand = GetLookupCommand(LookupCommands.AddModify);
        }
    }
}
