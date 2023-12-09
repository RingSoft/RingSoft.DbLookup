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
        #region Properties

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

        #endregion

        public RelayCommand AddModifyCommand { get; }

        public LocationViewModel()
        {
            AddModifyCommand = new RelayCommand((() =>
            {
                OnAddModify();
            }));
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
            RegisterLookup(ItemsLookupDefinition, _viewModelInput);

            base.Initialize();
        }

        protected override void PopulatePrimaryKeyControls(Location newEntity, PrimaryKeyValue primaryKeyValue)
        {
            LocationId = newEntity.Id;

            ReadOnlyMode = _viewModelInput.LocationViewModels.Any(a => a != this && a.LocationId == LocationId);
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
            var location = new Location
            {
                Id = LocationId,
                Name = KeyAutoFillValue.Text
            };

            return location;
        }

        protected override void ClearData()
        {
            LocationId = 0;
            ItemsLookupCommand = GetLookupCommand(LookupCommands.Clear);
        }

        private void OnAddModify()
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
