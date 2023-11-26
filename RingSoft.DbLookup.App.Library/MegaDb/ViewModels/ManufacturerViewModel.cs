using RingSoft.DataEntryControls.Engine;
using RingSoft.DbLookup.App.Library.MegaDb.LookupModel;
using RingSoft.DbLookup.App.Library.MegaDb.Model;
using RingSoft.DbLookup.AutoFill;
using RingSoft.DbLookup.Lookup;
using RingSoft.DbLookup.QueryBuilder;
using RingSoft.DbMaintenance;
using System.ComponentModel;
using System.Linq;

namespace RingSoft.DbLookup.App.Library.MegaDb.ViewModels
{
    public class ManufacturerViewModel : DbMaintenanceViewModel<Manufacturer>
    {

        #region Properties

        
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

        private IMegaDbLookupContext _lookupContext;
        private MegaDbViewModelInput _viewModelInput;

        protected override void Initialize()
        {
            _lookupContext = RsDbLookupAppGlobals.EfProcessor.MegaDbLookupContext;

            if (LookupAddViewArgs != null && LookupAddViewArgs.InputParameter is MegaDbViewModelInput viewModelInput)
                _viewModelInput = viewModelInput;
            else
                _viewModelInput = new MegaDbViewModelInput();

            _viewModelInput.ManufacturerViewModels.Add(this);

            var itemsLookup =
                new LookupDefinition<ItemLookup, Item>(RsDbLookupAppGlobals.EfProcessor.MegaDbLookupContext.Items);

            itemsLookup.AddVisibleColumnDefinition(p => p.Name, p => p.Name);
            itemsLookup.Include(p => p.Location)
                .AddVisibleColumnDefinition(p => p.Location, p => p.Name);
            itemsLookup.AddVisibleColumnDefinition(p => p.IconType, p => p.IconType);

            ItemsLookupDefinition = itemsLookup;

            base.Initialize();
        }

        protected override Manufacturer PopulatePrimaryKeyControls(Manufacturer newEntity, PrimaryKeyValue primaryKeyValue)
        {
            var manufacturer = RsDbLookupAppGlobals.EfProcessor.MegaDbEfDataProcessor.GetManufacturer(newEntity.Id);
            ManufacturerId = manufacturer.Id;
            KeyAutoFillValue = new AutoFillValue(_lookupContext.Manufacturers.GetPrimaryKeyValueFromEntity(manufacturer),
                manufacturer.Name);

            ReadOnlyMode =
                _viewModelInput.ManufacturerViewModels.Any(a => a != this && a.ManufacturerId == ManufacturerId);

            _itemsLookup.FilterDefinition.ClearFixedFilters();
            _itemsLookup.FilterDefinition.AddFixedFilter(p => p.ManufacturerId, Conditions.Equals, manufacturer.Id);

            ItemsLookupCommand = GetLookupCommand(LookupCommands.Refresh, null, _viewModelInput);

            return manufacturer;
        }

        protected override void LoadFromEntity(Manufacturer entity)
        {
            if (ReadOnlyMode)
                ControlsGlobals.UserInterface.ShowMessageBox(
                    "This Manufacturer is being modified in another window.  Editing not allowed.", "Editing not allowed",
                    RsMessageBoxIcons.Exclamation);
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

        public override void OnWindowClosing(CancelEventArgs e)
        {
            base.OnWindowClosing(e);
            if (!e.Cancel)
                _viewModelInput.ManufacturerViewModels.Remove(this);
        }
    }
}
