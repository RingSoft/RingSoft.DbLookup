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

        protected override void LoadFromEntity(StockMaster entity)
        {
            
        }

        protected override StockMaster GetEntityData()
        {
            throw new System.NotImplementedException();
        }

        protected override void ClearData()
        {
            
        }

        protected override bool SaveEntity(StockMaster entity)
        {
            throw new System.NotImplementedException();
        }

        protected override bool DeleteEntity()
        {
            throw new System.NotImplementedException();
        }


        public void OnAddModify()
        {
            if (ExecuteAddModifyCommand() == DbMaintenanceResults.Success)
                StockCostQuantityCommand = GetLookupCommand(LookupCommands.AddModify);
        }
    }
}
