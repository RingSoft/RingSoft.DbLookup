using System;
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

        protected override void LoadFromEntity(StockCostQuantity entity)
        {
            
        }

        protected override StockCostQuantity GetEntityData()
        {
            throw new NotImplementedException();
        }

        protected override void ClearData()
        {
            
        }

        protected override bool SaveEntity(StockCostQuantity entity)
        {
            throw new NotImplementedException();
        }

        protected override bool DeleteEntity()
        {
            throw new NotImplementedException();
        }


    }
}
