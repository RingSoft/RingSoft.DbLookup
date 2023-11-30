﻿using RingSoft.DbLookup.App.Library.LibLookupContext;
using RingSoft.DbLookup.App.Library.MegaDb.LookupModel;
using RingSoft.DbLookup.App.Library.MegaDb.Model;
using RingSoft.DbLookup.DataProcessor;
using RingSoft.DbLookup.Lookup;
using RingSoft.DbLookup.ModelDefinition.FieldDefinitions;
using System;
using System.Linq;
using RingSoft.DataEntryControls.Engine;
using RingSoft.DbLookup.AutoFill;

namespace RingSoft.DbLookup.App.Library.MegaDb
{
    public class MegaDbLookupContextConfiguration : AppLookupContextConfiguration, ILookupControl
    {
        public LookupDefinition<ItemLookup, Item> ItemsLookup { get; private set; }

        public LookupDefinition<LocationLookup, Location> LocationsLookup { get; private set; }

        public LookupDefinition<ManufacturerLookup, Manufacturer> ManufacturersLookup { get; private set; }

        public LookupDefinition<StockMasterLookup, StockMaster> StockMasterLookup { get; set; }

        public LookupDefinition<StockCostQuantityLookup, StockCostQuantity> StockCostQuantityLookup { get; set; }

        public LookupDefinition<StockCostQuantityLookup, StockCostQuantity> StockCostQuantityLookupFiltered
        {
            get;
            set;
        }

        private IMegaDbLookupContext _lookupContext;
        public MegaDbLookupContextConfiguration(IMegaDbLookupContext lookupContext)
        {
            _lookupContext = lookupContext;
            _lookupContext.GetAutoFillText += _lookupContext_GetAutoFillText;
            Reinitialize();
        }

        private void _lookupContext_GetAutoFillText(object sender, TableDefinitionValue e)
        {
            if (e.LookupDefinition == null)
            {
                return;
            }

            if (e.TableDefinition == _lookupContext.Stocks)
            {
                var primaryKey = new PrimaryKeyValue(e.TableDefinition);
                primaryKey.LoadFromPrimaryString(e.PrimaryKeyString);
                var stockMaster = _lookupContext
                    .Stocks
                    .GetEntityFromPrimaryKeyValue(primaryKey);
                var lookupColumn = e.LookupDefinition.InitialOrderByColumn;
                if (lookupColumn is LookupFieldColumnDefinition fieldColumn)
                {
                    if (fieldColumn.FieldDefinition ==
                        _lookupContext
                            .Stocks
                            .GetFieldDefinition(p => p.StockNumber))
                    {
                        e.ReturnValue = new AutoFillValue(primaryKey, stockMaster.StockNumber);
                    }
                    else if (fieldColumn.FieldDefinition ==
                             _lookupContext
                                 .Stocks
                                 .GetFieldDefinition(p => p.Location))
                    {
                        e.ReturnValue = new AutoFillValue(primaryKey, stockMaster.Location);
                    }
                }
            }
        }

        public override void Reinitialize(RegistrySettings registrySettings)
        {
            switch (registrySettings.MegaDbPlatformType)
            {
                case MegaDbPlatforms.None:
                case MegaDbPlatforms.SqlServer:
                    DataProcessorType = DataProcessorTypes.SqlServer;
                    break;
                case MegaDbPlatforms.MySql:
                    DataProcessorType = DataProcessorTypes.MySql;
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }

            SqlServerDataProcessor.Database = registrySettings.SqlServerMegaDbName;
            MySqlDataProcessor.Database = registrySettings.MySqlMegaDbName;

            base.Reinitialize(registrySettings);
        }

        public void ConfigureLookups()
        {
            ItemsLookup = new LookupDefinition<ItemLookup, Item>(_lookupContext.Items);
            ItemsLookup.AddVisibleColumnDefinition(p => p.Name, "Item Name", p => p.Name, 24);
            ItemsLookup.AddVisibleColumnDefinition(p => p.ItemId, "Item Id", p => p.Id, 20);
            ItemsLookup.Include(p => p.Location)
                .AddVisibleColumnDefinition(p => p.Location, "Location", p => p.Name, 20);
            ItemsLookup.Include(p => p.Manufacturer)
                .AddVisibleColumnDefinition(p => p.Manufacturer, "Manufacturer", 
                    p => p.Name, 20);
            ItemsLookup.AddVisibleColumnDefinition(p => p.IconType, "Icon",
                p => p.IconType, 16);

            _lookupContext.Items.HasLookupDefinition(ItemsLookup);

            LocationsLookup = new LookupDefinition<LocationLookup, Location>(_lookupContext.Locations);
            LocationsLookup.AddVisibleColumnDefinition(p => p.Name, "Name", p => p.Name, 99);

            _lookupContext.Locations.HasLookupDefinition(LocationsLookup);

            ManufacturersLookup = new LookupDefinition<ManufacturerLookup, Manufacturer>(_lookupContext.Manufacturers);
            ManufacturersLookup.AddVisibleColumnDefinition(p => p.Name, "Name", p => p.Name, 99);

            _lookupContext.Manufacturers.HasLookupDefinition(ManufacturersLookup);

            StockMasterLookup = new LookupDefinition<StockMasterLookup, StockMaster>(_lookupContext.Stocks);
            StockMasterLookup.AddVisibleColumnDefinition(p => p.StockNumber, "Stock Number", p => p.StockNumber, 40);
            StockMasterLookup.AddVisibleColumnDefinition(p => p.Location, "Location", p => p.Location, 40);
            StockMasterLookup.AddVisibleColumnDefinition(p => p.Price, "Price", p => p.Price, 20);

            _lookupContext.Stocks.HasLookupDefinition(StockMasterLookup);

            StockCostQuantityLookup = new LookupDefinition<StockCostQuantityLookup, StockCostQuantity>(_lookupContext.StockCostQuantities);
            StockCostQuantityLookup.AddVisibleColumnDefinition(p => p.StockNumber, "Stock Number", p => p.StockNumber, 25);
            StockCostQuantityLookup.AddVisibleColumnDefinition(p => p.Location, "Location", p => p.Location, 25);
            StockCostQuantityLookup.AddVisibleColumnDefinition(p => p.PurchasedDate, "Purchased Date",
                p => p.PurchasedDateTime, 20);
            StockCostQuantityLookup.AddVisibleColumnDefinition(p => p.Quantity, "Quantity", p => p.Quantity, 15);
            StockCostQuantityLookup.AddVisibleColumnDefinition(p => p.Cost, "Cost", 
                p => p.Cost, 15).DoShowNegativeValuesInRed();

            _lookupContext.StockCostQuantities.HasLookupDefinition(StockCostQuantityLookup);

            StockCostQuantityLookupFiltered =
                new LookupDefinition<StockCostQuantityLookup, StockCostQuantity>(_lookupContext.StockCostQuantities);
            StockCostQuantityLookupFiltered.AddVisibleColumnDefinition(p => p.PurchasedDate, "Purchase Date",
                p => p.PurchasedDateTime, 50);
            StockCostQuantityLookupFiltered.AddVisibleColumnDefinition(p => p.Quantity, "Quantity", p => p.Quantity, 25);
            StockCostQuantityLookupFiltered.AddVisibleColumnDefinition(p => p.Cost, 
                "Cost", p => p.Cost, 25).DoShowNegativeValuesInRed().DoShowPositiveValuesInGreen();
        }

        public override bool TestConnection()
        {
            try
            {
                RsDbLookupAppGlobals.EfProcessor.MegaDbEfDataProcessor.SetDataContext();
                var context = SystemGlobals.DataRepository.GetDataContext();
                var table = context.GetTable<Item>();
                var item = table
                    .FirstOrDefault(p => p.Id == 1);
                //RsDbLookupAppGlobals.EfProcessor.MegaDbEfDataProcessor.GetItem(1);
                DataProcessor.IsValid = true;
            }
            catch (Exception e)
            {
                DataProcessor.IsValid = false;
                Console.WriteLine(e);
                ControlsGlobals.UserInterface.ShowMessageBox(e.Message, "Error!", RsMessageBoxIcons.Error);
                return false;
            }
            return true;
        }

        public override bool TestConnection(RegistrySettings registrySettings)
        {
            Reinitialize(registrySettings);
            return TestConnection();
        }

        public void InitializeModel()
        {
            _lookupContext.Items.HasDescription("Items");
            _lookupContext.Items.GetFieldDefinition(p => p.IconType).IsEnum<ItemIcons>()
                .HasContentTemplateId(RsDbLookupAppGlobals.IconTypeTemplateId);

            _lookupContext.Locations.HasDescription("Locations");
            _lookupContext.Manufacturers.HasDescription("Manufacturers");

            _lookupContext.Stocks.HasDescription("Stocks").HasRecordDescription("Stock Item");
            _lookupContext.Stocks.GetFieldDefinition(p => p.StockNumber).HasDescription("Stock Number");
            _lookupContext.Stocks.GetFieldDefinition(p => p.Price).HasDecimalFieldType(DecimalFieldTypes.Currency);

            _lookupContext.StockCostQuantities.HasDescription("Stock Purchases");
            _lookupContext.StockCostQuantities.GetFieldDefinition(p => p.Quantity).HasDecimalCount(2);
            _lookupContext.StockCostQuantities.GetFieldDefinition(p => p.Cost)
                .HasDecimalFieldType(DecimalFieldTypes.Currency).DoShowNegativeValuesInRed().DoShowPositiveValuesInGreen();
        }

        public int PageSize { get; } = 15;
        public LookupSearchTypes SearchType { get; } = LookupSearchTypes.Equals;
        public string SearchText { get; set; } = string.Empty;
        public int SelectedIndex => 0;
        public void SetLookupIndex(int index)
        {
            
        }
    }
}
