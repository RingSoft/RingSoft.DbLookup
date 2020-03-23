using RingSoft.DbLookup.AutoFill;
using RingSoft.DbLookup.Lookup;
using RingSoft.DbLookup.ModelDefinition;
using RingSoft.DbLookup.QueryBuilder;
using RingSoft.DbMaintenance;
using RSDbLookupApp.Library.MegaDb.LookupModel;
using RSDbLookupApp.Library.MegaDb.Model;

namespace RSDbLookupApp.Library.MegaDb.ViewModels
{
    public class ManufacturerViewModel : DbMaintenanceViewModel<Manufacturer>
    {
        public override TableDefinition<Manufacturer> TableDefinition =>
            RsDbLookupAppGlobals.EfProcessor.MegaDbLookupContext.Manufacturers;

        private int _manufacturerId;
        public int ManufacturerId
        {
            get => _manufacturerId;
            set
            {
                if (_manufacturerId == value)
                    return;
                _manufacturerId = value;
                OnPropertyChanged(nameof(ManufacturerId));
            }
        }

        private LookupDefinition<ItemLookup, Item> _itemsLookup;
        public LookupDefinitionBase ItemsLookupDefinition => _itemsLookup;

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

            _itemsLookup =
                new LookupDefinition<ItemLookup, Item>(RsDbLookupAppGlobals.EfProcessor.MegaDbLookupContext.Items);

            _itemsLookup.AddVisibleColumnDefinition(p => p.Name, "Item", p => p.Name, 50);
            _itemsLookup.Include(p => p.Location)
                .AddVisibleColumnDefinition(p => p.Location, "Location", p => p.Name, 50);

            base.Initialize();
        }

        protected override void LoadFromEntity(Manufacturer newEntity)
        {
            var manufacturer = RsDbLookupAppGlobals.EfProcessor.MegaDbEfDataProcessor.GetManufacturer(newEntity.Id);
            ManufacturerId = manufacturer.Id;
            KeyAutoFillValue = new AutoFillValue(_lookupContext.Manufacturers.GetPrimaryKeyValueFromEntity(manufacturer),
                manufacturer.Name);

            _itemsLookup.FilterDefinition.ClearFixedFilters();
            _itemsLookup.FilterDefinition.AddFixedFilter(p => p.ManufacturerId, Conditions.Equals, manufacturer.Id);
            ItemsLookupCommand = GetLookupCommand(LookupCommands.Refresh);
        }

        protected override Manufacturer GetEntityData()
        {
            var manufacturer = new Manufacturer();
            manufacturer.Id = ManufacturerId;
            manufacturer.Name = KeyAutoFillValue.Text;

            return manufacturer;
        }

        protected override void ClearData()
        {
            ManufacturerId = 0;
            ItemsLookupCommand = GetLookupCommand(LookupCommands.Clear);
        }

        protected override bool SaveEntity(Manufacturer entity)
        {
            return RsDbLookupAppGlobals.EfProcessor.MegaDbEfDataProcessor.SaveManufacturer(entity);
        }

        protected override bool DeleteEntity()
        {
            return RsDbLookupAppGlobals.EfProcessor.MegaDbEfDataProcessor.DeleteManufacturer(ManufacturerId);
        }

        public void OnAddModify()
        {
            if (ExecuteAddModifyCommand() == DbMaintenanceResults.Success)
                ItemsLookupCommand = GetLookupCommand(LookupCommands.AddModify);
        }
    }
}
