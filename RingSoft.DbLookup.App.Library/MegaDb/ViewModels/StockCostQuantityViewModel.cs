using System;
using RingSoft.DbLookup.App.Library.MegaDb.Model;
using RingSoft.DbLookup.ModelDefinition;
using RingSoft.DbMaintenance;

namespace RingSoft.DbLookup.App.Library.MegaDb.ViewModels
{
    public class StockCostQuantityViewModel : DbMaintenanceViewModel<StockCostQuantity>
    {
        public override TableDefinition<StockCostQuantity> TableDefinition =>
            RsDbLookupAppGlobals.EfProcessor.MegaDbLookupContext.StockCostQuantities;

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

        public StockCostQuantityViewModel()
        {
            PurchaseDate = _newDate;
        }

        protected override void Initialize()
        {
            _lookupContext = RsDbLookupAppGlobals.EfProcessor.MegaDbLookupContext;

            FindButtonLookupDefinition =
                _lookupContext.MegaDbContextConfiguration.StockCostQuantityLookupFiltered.Clone();

            FindButtonLookupDefinition.FilterDefinition.CopyFrom(ViewLookupDefinition.FilterDefinition);

            base.Initialize();
        }

        public override void OnKeyControlLeave()
        {
            var stockCostQuantity =
                RsDbLookupAppGlobals.EfProcessor.MegaDbEfDataProcessor.GetStockCostQuantity(StockNumber, Location,
                    PurchaseDate);

            if (stockCostQuantity != null)
            {
                SelectPrimaryKey(_lookupContext.StockCostQuantities.GetPrimaryKeyValueFromEntity(stockCostQuantity));
            }
        }


        protected override StockCostQuantity PopulatePrimaryKeyControls(StockCostQuantity newEntity, PrimaryKeyValue primaryKeyValue)
        {
            StockNumber = newEntity.StockNumber;
            Location = newEntity.Location;
            PurchaseDate = newEntity.PurchasedDateTime;

            var stockCostQuantity =
                RsDbLookupAppGlobals.EfProcessor.MegaDbEfDataProcessor.GetStockCostQuantity(newEntity.StockNumber,
                    newEntity.Location, newEntity.PurchasedDateTime);

            return stockCostQuantity;
        }

        protected override void LoadFromEntity(StockCostQuantity entity)
        {
            Quantity = entity.Quantity;
            Cost = entity.Cost;
        }

        protected override StockCostQuantity GetEntityData()
        {
            var stockCostQuantity = new StockCostQuantity
            {
                StockNumber =  StockNumber,
                Location = Location,
                PurchasedDateTime = PurchaseDate,
                Quantity = Quantity,
                Cost = Cost
            };


            return stockCostQuantity;
        }

        protected override void ClearData()
        {
            if (LookupAddViewArgs != null && LookupAddViewArgs.ParentWindowPrimaryKeyValue != null &&
                LookupAddViewArgs.ParentWindowPrimaryKeyValue.TableDefinition == _lookupContext.Stocks)
            {
                var stockItem = _lookupContext.Stocks.GetEntityFromPrimaryKeyValue(LookupAddViewArgs
                    .ParentWindowPrimaryKeyValue);
                StockNumber = stockItem.StockNumber;
                Location = stockItem.Location;
            }
            else
            {
                StockNumber = Location = string.Empty;
            }

            PurchaseDate = _newDate;
            Quantity = Cost = 0;
        }

        protected override bool SaveEntity(StockCostQuantity entity)
        {
            return RsDbLookupAppGlobals.EfProcessor.MegaDbEfDataProcessor.SaveStockCostQuantity(entity);
        }

        protected override bool DeleteEntity()
        {
            return RsDbLookupAppGlobals.EfProcessor.MegaDbEfDataProcessor.DeleteStockCostQuantity(
                StockNumber, Location, PurchaseDate);
        }
    }
}
