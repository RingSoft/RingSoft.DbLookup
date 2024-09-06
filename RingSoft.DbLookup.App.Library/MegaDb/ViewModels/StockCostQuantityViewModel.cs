using RingSoft.DataEntryControls.Engine;
using RingSoft.DbLookup.App.Library.MegaDb.Model;
using RingSoft.DbMaintenance;
using System;
using System.Linq;

namespace RingSoft.DbLookup.App.Library.MegaDb.ViewModels
{
    public class StockCostQuantityViewModel : DbMaintenanceViewModel<StockCostQuantity>
    {
        #region Properties

        private string _stockNumber;

        public string StockNumber
        {
            get => _stockNumber;
            set
            {
                if (_stockNumber == value)
                    return;

                _stockNumber = value;
                OnPropertyChanged(nameof(StockNumber));
            }
        }

        private string _location;

        public string Location
        {
            get => _location;
            set
            {
                if (_location == value)
                    return;

                _location = value;
                OnPropertyChanged(nameof(Location));
            }
        }

        private DateTime _purchaseDate;
        public DateTime PurchaseDate
        {
            get => _purchaseDate;
            set
            {
                if (_purchaseDate == value)
                    return;

                _purchaseDate = value;
                OnPropertyChanged(nameof(PurchaseDate));
            }
        }

        private double _quantity;

        public double Quantity
        {
            get => _quantity;
            set
            {
                if (_quantity == value)
                    return;

                _quantity = value;
                OnPropertyChanged(nameof(Quantity));
            }
        }

        private double _cost;

        public double Cost
        {
            get => _cost;
            set
            {
                if (_cost == value)
                    return;

                _cost = value;
                OnPropertyChanged(nameof(Cost));
            }
        }

        #endregion

        public UiCommand PurchaseDateUiCommand { get; } = new UiCommand();

        public UiCommand QuantityUiCommand { get; } = new UiCommand();

        private IMegaDbLookupContext _lookupContext;
        private DateTime _newDate = DateTime.Today;
        private StockMaster _parentStock;

        public StockCostQuantityViewModel()
        {
            _lookupContext = RsDbLookupAppGlobals.EfProcessor.MegaDbLookupContext;

            PurchaseDateUiCommand.LostFocus += PurchaseDateUiCommand_LostFocus;
        }

        private void PurchaseDateUiCommand_LostFocus(object sender, UiLostFocusArgs e)
        {
            if (_parentStock == null)
            {
                return;
            }
            var context = SystemGlobals.DataRepository.GetDataContext();
            var table = context.GetTable<StockCostQuantity>();
            var stockCq = table
                .FirstOrDefault(p => p.StockMasterId == _parentStock.Id
                && p.PurchasedDateTime == PurchaseDate);

            if (stockCq != null)
            {
                var primaryKey = RsDbLookupAppGlobals
                    .EfProcessor
                    .MegaDbLookupContext
                    .StockCostQuantities
                    .GetPrimaryKeyValueFromEntity(stockCq);
                KeyValueDirty = true;
                SelectPrimaryKey(primaryKey);
            }
        }

        protected override void Initialize()
        {
            if (LookupAddViewArgs != null 
                && LookupAddViewArgs.ParentWindowPrimaryKeyValue == null
                && LookupAddViewArgs.SelectedPrimaryKeyValue != null)
            {
                var costQty = TableDefinition.GetEntityFromPrimaryKeyValue(
                    LookupAddViewArgs.SelectedPrimaryKeyValue);
                if (costQty != null)
                {
                    _parentStock = RsDbLookupAppGlobals
                        .EfProcessor
                        .MegaDbLookupContext
                        .StockMasters
                        .GetEntity() as StockMaster;
                    _parentStock.Id = costQty.StockMasterId;
                    _parentStock = _parentStock.FillOutProperties(true);
                }
            }
            if (LookupAddViewArgs != null && LookupAddViewArgs.ParentWindowPrimaryKeyValue != null)
            {
                if (LookupAddViewArgs.ParentWindowPrimaryKeyValue.TableDefinition == _lookupContext.StockMasters)
                {
                    var stockMaster = _lookupContext
                        .StockMasters
                        .GetEntityFromPrimaryKeyValue(LookupAddViewArgs.ParentWindowPrimaryKeyValue);

                    if (stockMaster != null)
                    {
                        _parentStock = stockMaster.FillOutProperties(true);
                    }
                }
            }
            if (_parentStock != null)
            {
                StockNumber = _parentStock.Stock.Name;
                Location = _parentStock.MliLocation.Name;
            }
            base.Initialize();
        }

        public override void OnNewButton()
        {
            base.OnNewButton();
            PurchaseDateUiCommand.IsReadOnly = false;
            PurchaseDateUiCommand.SetFocus();
        }

        protected override void PopulatePrimaryKeyControls(StockCostQuantity newEntity, PrimaryKeyValue primaryKeyValue)
        {
            PurchaseDate = newEntity.PurchasedDateTime;

            if (PurchaseDateUiCommand.IsFocused)
            {
                QuantityUiCommand.SetFocus();
            }

            PurchaseDateUiCommand.IsReadOnly = true;
        }

        protected override void LoadFromEntity(StockCostQuantity entity)
        {
            Quantity = entity.Quantity;
            Cost = entity.Cost;
        }

        protected override StockCostQuantity GetEntityData()
        {
            var result = new StockCostQuantity
            {
                StockMasterId = _parentStock.Id,
                PurchasedDateTime = PurchaseDate,
                Quantity = Quantity,
                Cost = Cost,
            };
            return result;
        }

        protected override void ClearData()
        {
            PurchaseDate = DateTime.Today;
            Quantity = 1;
            Cost = 0;
        }
    }
}
