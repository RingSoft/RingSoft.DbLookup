using RingSoft.DbLookup.App.Library.MegaDb.LookupModel;
using RingSoft.DbLookup.App.Library.MegaDb.Model;
using RingSoft.DbLookup.AutoFill;
using RingSoft.DbLookup.Lookup;
using RingSoft.DbLookup.ModelDefinition;
using RingSoft.DbLookup.QueryBuilder;
using RingSoft.DbMaintenance;

namespace RingSoft.DbLookup.App.Library.MegaDb.ViewModels
{
    public class LocationViewModel : DbMaintenanceViewModel<Location>
    {
        public override TableDefinition<Location> TableDefinition =>
            RsDbLookupAppGlobals.EfProcessor.MegaDbLookupContext.Locations;

        private int _locationId;
        public int LocationId
        {
            get => _locationId;
            set
            {
                if (_locationId == value)
                    return;
                _locationId = value;
                OnPropertyChanged(nameof(LocationId));
            }
        }

        private LookupDefinition<ItemLookup, Item> _itemsLookup;
        public LookupDefinition<ItemLookup, Item> ItemsLookupDefinition
        {
            get => _itemsLookup;
            set
            {
                if (_itemsLookup == value)
                    return;

                _itemsLookup = value;
                OnPropertyChanged(nameof(ItemsLookupDefinition));
            }
        }

        private LookupCommand _itemsLookupCommand;

        public LookupCommand ItemsLookupCommand
        {
            get => _itemsLookupCommand;
            set
            {
                if (_itemsLookupCommand == value)
                    return;

                _itemsLookupCommand = value;
                OnPropertyChanged(nameof(ItemsLookupCommand), false);
            }
        }

        private IMegaDbLookupContext _lookupContext;

        protected override void Initialize()
        {
            _lookupContext = RsDbLookupAppGlobals.EfProcessor.MegaDbLookupContext;

            var itemsLookup =
                new LookupDefinition<ItemLookup, Item>(RsDbLookupAppGlobals.EfProcessor.MegaDbLookupContext.Items);

            itemsLookup.AddVisibleColumnDefinition(p => p.Name, "Item", p => p.Name, 50);
            itemsLookup.Include(p => p.Manufacturer)
                .AddVisibleColumnDefinition(p => p.Manufacturer, "Manufacturer", p => p.Name, 50);

            ItemsLookupDefinition = itemsLookup;

            base.Initialize();
        }

        protected override void LoadFromEntity(Location newEntity)
        {
            var location = RsDbLookupAppGlobals.EfProcessor.MegaDbEfDataProcessor.GetLocation(newEntity.Id);
            LocationId = location.Id;
            KeyAutoFillValue = new AutoFillValue(_lookupContext.Locations.GetPrimaryKeyValueFromEntity(location),
                location.Name);

            _itemsLookup.FilterDefinition.ClearFixedFilters();
            _itemsLookup.FilterDefinition.AddFixedFilter(p => p.LocationId, Conditions.Equals, location.Id);
            ItemsLookupCommand = GetLookupCommand(LookupCommands.Refresh);
        }

        protected override Location GetEntityData()
        {
            var location = new Location();
            location.Id = LocationId;
            location.Name = KeyAutoFillValue.Text;

            return location;
        }

        protected override void ClearData()
        {
            LocationId = 0;
            ItemsLookupCommand = GetLookupCommand(LookupCommands.Clear);
        }

        protected override bool SaveEntity(Location entity)
        {
            return RsDbLookupAppGlobals.EfProcessor.MegaDbEfDataProcessor.SaveLocation(entity);
        }

        protected override bool DeleteEntity()
        {
            return RsDbLookupAppGlobals.EfProcessor.MegaDbEfDataProcessor.DeleteLocation(LocationId);
        }

        public void OnAddModify()
        {
            if (ExecuteAddModifyCommand() == DbMaintenanceResults.Success)
                ItemsLookupCommand = GetLookupCommand(LookupCommands.AddModify);
        }
    }
}
