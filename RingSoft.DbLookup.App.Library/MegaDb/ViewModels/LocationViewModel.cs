using System.ComponentModel;
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

        private MegaDbViewModelInput _viewModelInput;

        protected override void Initialize()
        {
            if (LookupAddViewArgs != null && LookupAddViewArgs.InputParameter is MegaDbViewModelInput viewModelInput)
                _viewModelInput = viewModelInput;
            else
                _viewModelInput = new MegaDbViewModelInput();

            _viewModelInput.LocationViewModels.Add(this);

            var itemsLookup =
                new LookupDefinition<ItemLookup, Item>(RsDbLookupAppGlobals.EfProcessor.MegaDbLookupContext.Items);

            itemsLookup.AddVisibleColumnDefinition(p => p.Name, p => p.Name);
            itemsLookup.Include(p => p.Manufacturer)
                .AddVisibleColumnDefinition(p => p.Manufacturer, p => p.Name);
            itemsLookup.AddVisibleColumnDefinition(p => p.IconType, p => p.IconType);

            ItemsLookupDefinition = itemsLookup;

            base.Initialize();
        }

        protected override Location PopulatePrimaryKeyControls(Location newEntity, PrimaryKeyValue primaryKeyValue)
        {
            var location = RsDbLookupAppGlobals.EfProcessor.MegaDbEfDataProcessor.GetLocation(newEntity.Id);
            LocationId = location.Id;
            KeyAutoFillValue = new AutoFillValue(primaryKeyValue, location.Name);

            ReadOnlyMode = _viewModelInput.LocationViewModels.Any(a => a != this && a.LocationId == LocationId);

            _itemsLookup.FilterDefinition.ClearFixedFilters();
            _itemsLookup.FilterDefinition.AddFixedFilter(p => p.LocationId, Conditions.Equals, location.Id);
            ItemsLookupCommand = GetLookupCommand(LookupCommands.Refresh, null, _viewModelInput);

            return location;
        }

        protected override void LoadFromEntity(Location entity)
        {
            if (ReadOnlyMode)
                ControlsGlobals.UserInterface.ShowMessageBox(
                    "This Location is being modified in another window.  Editing not allowed.", "Editing not allowed",
                    RsMessageBoxIcons.Exclamation);
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
            //var test = LookupAddViewArgs;
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

        public override void OnWindowClosing(CancelEventArgs e)
        {
            base.OnWindowClosing(e);
            if (!e.Cancel)
                _viewModelInput.LocationViewModels.Remove(this);
        }
    }
}
