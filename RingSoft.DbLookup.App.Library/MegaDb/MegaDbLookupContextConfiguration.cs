using System;
using RingSoft.DbLookup.App.Library.LibLookupContext;
using RingSoft.DbLookup.App.Library.MegaDb.LookupModel;
using RingSoft.DbLookup.App.Library.MegaDb.Model;
using RingSoft.DbLookup.GetDataProcessor;
using RingSoft.DbLookup.Lookup;
using RingSoft.DbLookup.ModelDefinition.FieldDefinitions;

namespace RingSoft.DbLookup.App.Library.MegaDb
{
    public class MegaDbLookupContextConfiguration : AppLookupContextConfiguration, ILookupUserInterface
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
            Reinitialize();
        }

        public override void Reinitialize(RegistrySettings registrySettings)
        {
            switch (registrySettings.MegaDbPlatformType)
            {
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
                .AddVisibleColumnDefinition(p => p.Location, "Location", p => p.Name, 28);
            ItemsLookup.Include(p => p.Manufacturer)
                .AddVisibleColumnDefinition(p => p.Manufacturer, "Manufacturer", p => p.Name, 28);

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
            StockCostQuantityLookup.AddVisibleColumnDefinition(p => p.Cost, "Cost", p => p.Cost, 15);

            _lookupContext.StockCostQuantities.HasLookupDefinition(StockCostQuantityLookup);

            StockCostQuantityLookupFiltered =
                new LookupDefinition<StockCostQuantityLookup, StockCostQuantity>(_lookupContext.StockCostQuantities);
            StockCostQuantityLookupFiltered.AddVisibleColumnDefinition(p => p.PurchasedDate, "Purchase Date",
                p => p.PurchasedDateTime, 50);
            StockCostQuantityLookupFiltered.AddVisibleColumnDefinition(p => p.Quantity, "Quantity", p => p.Quantity, 25);
            StockCostQuantityLookupFiltered.AddVisibleColumnDefinition(p => p.Cost, "Cost", p => p.Cost, 25);
        }

        public override bool TestConnection()
        {
            var itemsLookupData = new LookupDataBase(ItemsLookup, this);
            var result = itemsLookupData.GetInitData();
            return result.ResultCode == GetDataResultCodes.Success;
        }

        public override bool TestConnection(RegistrySettings registrySettings)
        {
            Reinitialize(registrySettings);
            return TestConnection();
        }

        public void InitializeModel()
        {
            _lookupContext.Items.HasDescription("Items");
            _lookupContext.Locations.HasDescription("Locations");
            _lookupContext.Manufacturers.HasDescription("Manufacturers");

            _lookupContext.Stocks.HasDescription("Stocks").HasRecordDescription("Stock Item");
            _lookupContext.Stocks.GetFieldDefinition(p => p.StockNumber).HasDescription("Stock Number");
            _lookupContext.Stocks.GetFieldDefinition(p => p.Price).HasDecimalFieldType(DecimalFieldTypes.Currency);

            _lookupContext.StockCostQuantities.HasDescription("Stock Purchases");
            _lookupContext.StockCostQuantities.GetFieldDefinition(p => p.Quantity).HasDecimalCount(2);
            _lookupContext.StockCostQuantities.GetFieldDefinition(p => p.Cost)
                .HasDecimalFieldType(DecimalFieldTypes.Currency);
        }

        public int PageSize { get; } = 15;
        public LookupSearchTypes SearchType { get; } = LookupSearchTypes.Equals;
        public string SearchText { get; set; } = string.Empty;
    }
}
