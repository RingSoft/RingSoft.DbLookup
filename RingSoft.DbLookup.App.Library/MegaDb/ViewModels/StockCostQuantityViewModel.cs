using System;
using System.Linq;
using RingSoft.DbLookup.App.Library.MegaDb.Model;
using RingSoft.DbLookup.ModelDefinition;
using RingSoft.DbMaintenance;

namespace RingSoft.DbLookup.App.Library.MegaDb.ViewModels
{
    public class StockCostQuantityViewModel : DbMaintenanceViewModel<StockCostQuantity>
    {
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

        private IMegaDbLookupContext _lookupContext;
        private DateTime _newDate = DateTime.Today;
        private StockMaster _parentStock;

        public StockCostQuantityViewModel()
        {
            _lookupContext = RsDbLookupAppGlobals.EfProcessor.MegaDbLookupContext;
        }

        protected override void Initialize()
        {
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

                    if (_parentStock != null)
                    {
                        StockNumber = _parentStock.Stock.Name;
                        Location = _parentStock.MliLocation.Name;
                    }
                }
            }
            base.Initialize();
        }

        protected override StockCostQuantity PopulatePrimaryKeyControls(StockCostQuantity newEntity, PrimaryKeyValue primaryKeyValue)
        {
            PurchaseDate = newEntity.PurchasedDateTime;

            return base.PopulatePrimaryKeyControls(newEntity, primaryKeyValue);
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

        protected override bool SaveEntity(StockCostQuantity entity)
        {
            var context = SystemGlobals.DataRepository.GetDataContext();
            var table = context.GetTable<StockCostQuantity>();
            var costQuantity = table
                .FirstOrDefault(p => p.StockMasterId == entity.StockMasterId
                                     && p.PurchasedDateTime == entity.PurchasedDateTime);
            if (costQuantity == null)
            {
                return context.AddSaveEntity(entity, "Saving Cost/Quantity");
            }

            context = SystemGlobals.DataRepository.GetDataContext();
            return context.SaveEntity(entity, "Saving Cost/Quantity");
        }

        protected override bool DeleteEntity()
        {
            throw new NotImplementedException();
        }


    }
}
