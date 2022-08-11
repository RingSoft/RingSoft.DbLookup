using System.ComponentModel;
using System.Linq;
using RingSoft.DbLookup.App.Library.MegaDb.Model;
using RingSoft.DbLookup.AutoFill;
using RingSoft.DbLookup.ModelDefinition;
using RingSoft.DbMaintenance;
using RingSoft.DataEntryControls.Engine;

namespace RingSoft.DbLookup.App.Library.MegaDb.ViewModels
{
    public class ItemViewModel : DbMaintenanceViewModel<Item>
    {
        public override TableDefinition<Item> TableDefinition =>
            RsDbLookupAppGlobals.EfProcessor.MegaDbLookupContext.Items;

        private int _itemId;
        public int ItemId
        {
            get => _itemId;
            set
            {
                if (_itemId == value)
                    return;
                _itemId = value;
                OnPropertyChanged(nameof(ItemId));
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

        private AutoFillSetup _manufacturerSetup;

        public AutoFillSetup ManufacturerAutoFillSetup
        {
            get => _manufacturerSetup;
            set
            {
                if (_manufacturerSetup == value)
                    return;

                _manufacturerSetup = value;
                OnPropertyChanged(nameof(ManufacturerAutoFillSetup));
            }
        }

        private AutoFillValue _manufacturerAutoFillValue;
        public AutoFillValue ManufacturerAutoFillValue
        {
            get => _manufacturerAutoFillValue;
            set
            {
                if (_manufacturerAutoFillValue == value)
                    return;

                _manufacturerAutoFillValue = value;
                OnPropertyChanged(nameof(ManufacturerAutoFillValue));
            }
        }

        private byte _iconType;

        public byte IconType
        {
            get => _iconType;
            set
            {
                if (_iconType == value)
                    return;

                _iconType = value;
                OnPropertyChanged(nameof(IconType));
            }
        }


        private IMegaDbLookupContext _lookupContext;
        private MegaDbViewModelInput _viewModelInput;

        protected override void Initialize()
        {
            _lookupContext = RsDbLookupAppGlobals.EfProcessor.MegaDbLookupContext;
            if (LookupAddViewArgs != null && LookupAddViewArgs.InputParameter is MegaDbViewModelInput viewModelInput)
                _viewModelInput = viewModelInput;
            else
                _viewModelInput = new MegaDbViewModelInput();

            _viewModelInput.ItemViewModels.Add(this);

            LocationAutoFillSetup = new AutoFillSetup(TableDefinition.GetFieldDefinition(p => p.LocationId))
            {
                AddViewParameter = _viewModelInput
            };
            ManufacturerAutoFillSetup = new AutoFillSetup(TableDefinition.GetFieldDefinition(p => p.ManufacturerId))
            {
                AddViewParameter = _viewModelInput
            };
            base.Initialize();
        }

        protected override Item PopulatePrimaryKeyControls(Item newEntity, PrimaryKeyValue primaryKeyValue)
        {
            var item = RsDbLookupAppGlobals.EfProcessor.MegaDbEfDataProcessor.GetItem(newEntity.Id);
            ItemId = item.Id;
            KeyAutoFillValue = new AutoFillValue(primaryKeyValue, item.Name);

            ReadOnlyMode = _viewModelInput.ItemViewModels.Any(a => a != this && a.ItemId == ItemId);

            return item;
        }

        protected override void LoadFromEntity(Item entity)
        {
            LocationAutoFillValue =
                new AutoFillValue(_lookupContext.Locations.GetPrimaryKeyValueFromEntity(entity.Location),
                    entity.Location.Name);
            ManufacturerAutoFillValue =
                new AutoFillValue(_lookupContext.Manufacturers.GetPrimaryKeyValueFromEntity(entity.Manufacturer),
                    entity.Manufacturer.Name);

            IconType = entity.IconType;

            if (ReadOnlyMode)
                ControlsGlobals.UserInterface.ShowMessageBox(
                    "This Item is being modified in another window.  Editing not allowed.", "Editing not allowed",
                    RsMessageBoxIcons.Exclamation);
        }

        protected override Item GetEntityData()
        {
            var item = new Item {Id = ItemId, IconType = IconType};


            if (KeyAutoFillValue != null)
                item.Name = KeyAutoFillValue.Text;

            if (LocationAutoFillValue != null)
            {
                var location =
                    _lookupContext.Locations.GetEntityFromPrimaryKeyValue(LocationAutoFillValue.PrimaryKeyValue);
                item.LocationId = location.Id;
            }

            if (ManufacturerAutoFillValue != null)
            {
                var manufacturer =
                    _lookupContext.Manufacturers.GetEntityFromPrimaryKeyValue(ManufacturerAutoFillValue
                        .PrimaryKeyValue);
                item.ManufacturerId = manufacturer.Id;
            }

            return item;
        }

        protected override void ClearData()
        {
            ItemId = 0;
            IconType = 0;
            LocationAutoFillValue = ManufacturerAutoFillValue = null;
            if (LookupAddViewArgs != null && LookupAddViewArgs.ParentWindowPrimaryKeyValue != null)
            {
                if (LookupAddViewArgs.ParentWindowPrimaryKeyValue.TableDefinition == _lookupContext.Locations)
                    SetNewLocationValue(LookupAddViewArgs.ParentWindowPrimaryKeyValue);
                else if (LookupAddViewArgs.ParentWindowPrimaryKeyValue.TableDefinition == _lookupContext.Manufacturers)
                    SetNewManufacturerValue(LookupAddViewArgs.ParentWindowPrimaryKeyValue);
            }
        }

        private void SetNewLocationValue(PrimaryKeyValue primaryKeyValue)
        {
            var location = _lookupContext.Locations.GetEntityFromPrimaryKeyValue(primaryKeyValue);
            location = RsDbLookupAppGlobals.EfProcessor.MegaDbEfDataProcessor.GetLocation(location.Id);
            LocationAutoFillValue = new AutoFillValue(primaryKeyValue, location.Name);
        }

        private void SetNewManufacturerValue(PrimaryKeyValue primaryKeyValue)
        {
            var manufacturer = _lookupContext.Manufacturers.GetEntityFromPrimaryKeyValue(primaryKeyValue);
            manufacturer = RsDbLookupAppGlobals.EfProcessor.MegaDbEfDataProcessor.GetManufacturer(manufacturer.Id);
            ManufacturerAutoFillValue = new AutoFillValue(primaryKeyValue, manufacturer.Name);
        }

        protected override bool SaveEntity(Item entity)
        {
            return RsDbLookupAppGlobals.EfProcessor.MegaDbEfDataProcessor.SaveItem(entity);
        }

        protected override bool DeleteEntity()
        {
            return RsDbLookupAppGlobals.EfProcessor.MegaDbEfDataProcessor.DeleteItem(ItemId);
        }

        public bool LocationLostFocusValidation(object ownerWindow)
        {
            if (LocationAutoFillValue != null && !LocationAutoFillValue.PrimaryKeyValue.IsValid &&
                !LocationAutoFillValue.Text.IsNullOrEmpty())
            {
                var message =
                    $"'{LocationAutoFillValue.Text}' was not found in the database.  Would you like to add it?";
                if (!Processor.ShowYesNoMessage(message, "Invalid Location"))
                    return false;

                var newRecord = _lookupContext.MegaDbContextConfiguration.LocationsLookup.ShowAddOnTheFlyWindow(
                    LocationAutoFillValue.Text, ownerWindow);

                if (!newRecord.NewPrimaryKeyValue.IsValid)
                    return false;

                LocationAutoFillValue =
                    new AutoFillValue(newRecord.NewPrimaryKeyValue, newRecord.NewLookupEntity.Name);
            }
            return true;
        }

        public bool ManufacturerLostFocusValidation(object ownerWindow)
        {
            if (ManufacturerAutoFillValue != null && !ManufacturerAutoFillValue.PrimaryKeyValue.IsValid &&
                !ManufacturerAutoFillValue.Text.IsNullOrEmpty())
            {
                var message =
                    $"'{ManufacturerAutoFillValue.Text}' was not found in the database.  Would you like to add it?";
                if (!Processor.ShowYesNoMessage(message, "Invalid Manufacturer"))
                    return false;

                var newRecord =
                    _lookupContext.MegaDbContextConfiguration.ManufacturersLookup.ShowAddOnTheFlyWindow(
                        ManufacturerAutoFillValue.Text, ownerWindow);

                if (!newRecord.NewPrimaryKeyValue.IsValid)
                    return false;

                ManufacturerAutoFillValue =
                    new AutoFillValue(newRecord.NewPrimaryKeyValue, newRecord.NewLookupEntity.Name);
            }
            return true;
        }

        public override void OnWindowClosing(CancelEventArgs e)
        {
            base.OnWindowClosing(e);
            if (!e.Cancel)
                _viewModelInput.ItemViewModels.Remove(this);
        }
    }
}
