﻿using RingSoft.DbLookup;
using RingSoft.DbLookup.AutoFill;
using RingSoft.DbLookup.ModelDefinition;
using RingSoft.DbMaintenance;
using RSDbLookupApp.Library.MegaDb.Model;

namespace RSDbLookupApp.Library.MegaDb.ViewModels
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
        public AutoFillSetup LocationAutoFillSetup => _locationAutoFillSetup;

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
        public AutoFillSetup ManufacturerAutoFillSetup => _manufacturerSetup;

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

        private IMegaDbLookupContext _lookupContext;

        protected override void Initialize()
        {
            _lookupContext = RsDbLookupAppGlobals.EfProcessor.MegaDbLookupContext;

            _locationAutoFillSetup = new AutoFillSetup(TableDefinition.GetFieldDefinition(p => p.LocationId));
            _manufacturerSetup = new AutoFillSetup(TableDefinition.GetFieldDefinition(p => p.ManufacturerId));
            base.Initialize();
        }

        protected override void LoadFromEntity(Item newEntity)
        {
            var item = RsDbLookupAppGlobals.EfProcessor.MegaDbEfDataProcessor.GetItem(newEntity.Id);
            ItemId = item.Id;
            KeyAutoFillValue = new AutoFillValue(_lookupContext.Items.GetPrimaryKeyValueFromEntity(item),
                item.Name);
            LocationAutoFillValue =
                new AutoFillValue(_lookupContext.Locations.GetPrimaryKeyValueFromEntity(item.Location),
                    item.Location.Name);
            ManufacturerAutoFillValue =
                new AutoFillValue(_lookupContext.Manufacturers.GetPrimaryKeyValueFromEntity(item.Manufacturer),
                    item.Manufacturer.Name);
        }

        protected override Item GetEntityData()
        {
            var item = new Item();

            item.Id = ItemId;

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
    }
}
