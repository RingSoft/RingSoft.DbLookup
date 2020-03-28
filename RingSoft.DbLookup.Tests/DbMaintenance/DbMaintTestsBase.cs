using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using RingSoft.DbLookup.App.Library;
using RingSoft.DbLookup.App.Library.LookupContext;
using RingSoft.DbLookup.App.Library.MegaDb.Model;
using RingSoft.DbLookup.App.Library.MegaDb.ViewModels;
using RingSoft.DbLookup.App.Library.Northwind.Model;
using RingSoft.DbLookup.App.Library.Northwind.ViewModels;
using RingSoft.DbLookup.AutoFill;
using RingSoft.DbLookup.Lookup;
using RingSoft.DbLookup.ModelDefinition.FieldDefinitions;
using RingSoft.DbMaintenance;

namespace RingSoft.DbLookup.Tests.DbMaintenance
{
    public abstract class DbMaintTestsBase : IDbMaintenanceView
    {
        protected static void SetupConfigurations()
        {
            var nwConfiguration =
                RsDbLookupAppGlobals.EfProcessor.NorthwindLookupContext.NorthwindContextConfiguration;
            nwConfiguration.DataProcessorType = DataProcessorTypes.SqlServer;
            nwConfiguration.SqlServerDataProcessor.Database = "Northwind_Tests";
            nwConfiguration.ConfigureLookups();

            var megaConfiguration = RsDbLookupAppGlobals.EfProcessor.MegaDbLookupContext.MegaDbContextConfiguration;
            megaConfiguration.DataProcessorType = DataProcessorTypes.SqlServer;
            megaConfiguration.SqlServerDataProcessor.Database = "MegaDb_Tests";
        }

        public void OnValidationFail(FieldDefinition fieldDefinition, string text, string caption)
        {
            
        }

        public void ResetViewForNewRecord()
        {
            
        }

        void IDbMaintenanceView.ShowFindLookupForm(LookupDefinitionBase lookupDefinition, bool allowAdd, bool allowView, string initialSearchFor)
        {
        }

        public event EventHandler<LookupSelectArgs> LookupFormReturn;

        public void CloseWindow()
        {
            
        }

        public MessageButtons ShowYesNoCancelMessage(string text, string caption)
        {
            return MessageButtons.Yes;
        }

        public void ShowInformationMessage(string text, string caption)
        {
            
        }

        public bool ShowYesNoMessage(string text, string caption)
        {
            return true;
        }

        public void ShowRecordSavedMessage()
        {
            
        }

        public void TestOrderViewModel()
        {
            var lookupContext = RsDbLookupAppGlobals.EfProcessor.NorthwindLookupContext;
            var viewModel = new OrderViewModel();
            viewModel.OnViewLoaded(this);

            viewModel.OnGotoNextButton();
            Assert.AreEqual(10248, viewModel.OrderId, "Goto First Record");

            viewModel.OnGotoPreviousButton();
            Assert.AreEqual(11077, viewModel.OrderId, "Goto Last Record");

            viewModel.OnNewButton();
            Assert.AreEqual(0, viewModel.OrderId, "New Record");

            viewModel.Customer = new AutoFillValue(new PrimaryKeyValue(lookupContext.Customers), "ABCDE");
            var result = viewModel.OnSaveButton();
            Assert.AreEqual(DbMaintenanceResults.ValidationError, result, "OnSaveButton validate Customer");

            var customer = new Customer {CustomerID = "VINET" };
            viewModel.Customer =
                new AutoFillValue(lookupContext.Customers.GetPrimaryKeyValueFromEntity(customer), "VINET");
            viewModel.Employee = new AutoFillValue(new PrimaryKeyValue(lookupContext.Employees), "ABCDE");
            result = viewModel.OnSaveButton();
            Assert.AreEqual(DbMaintenanceResults.ValidationError, result, "OnSaveButton validate Employee");

            var employee = new Employee {EmployeeID = 5};
            viewModel.Employee = new AutoFillValue(lookupContext.Employees.GetPrimaryKeyValueFromEntity(employee),
                "Steven Buchanan");
            viewModel.ShipVia = new AutoFillValue(new PrimaryKeyValue(lookupContext.Shippers), "ABCDE");
            result = viewModel.OnSaveButton();
            Assert.AreEqual(DbMaintenanceResults.ValidationError, result, "OnSaveButton validate ShipVia");

            var shipper = new Shipper{ShipperID = 3};
            viewModel.ShipVia = new AutoFillValue(lookupContext.Shippers.GetPrimaryKeyValueFromEntity(shipper),
                "Federal Shipping");
            result = viewModel.OnSaveButton();
            Assert.AreEqual(DbMaintenanceResults.Success, result, "Save New Record");
            Assert.IsTrue(viewModel.OrderId != 0, "Save reload record");

            result = viewModel.OnDeleteButton();
            Assert.AreEqual(DbMaintenanceResults.Success, result, "Delete Record");
        }

        public void TestCustomerViewModel()
        {
            var lookupContext = RsDbLookupAppGlobals.EfProcessor.NorthwindLookupContext;
            var viewModel = new CustomerViewModel();
            viewModel.OnViewLoaded(this);

            viewModel.OnGotoNextButton();
            var customer =
                lookupContext.Customers.GetEntityFromPrimaryKeyValue(viewModel.KeyAutoFillValue.PrimaryKeyValue);
            Assert.AreEqual("ALFKI", customer.CustomerID, "Goto First Record");

            viewModel.OnGotoPreviousButton();
            customer =
                lookupContext.Customers.GetEntityFromPrimaryKeyValue(viewModel.KeyAutoFillValue.PrimaryKeyValue);
            Assert.AreEqual("WOLZA", customer.CustomerID, "Goto Last Record");

            viewModel.OnNewButton();
            Assert.AreEqual(string.Empty, viewModel.KeyAutoFillValue.Text, "New Record");

            var result = viewModel.OnSaveButton();
            Assert.AreEqual(DbMaintenanceResults.ValidationError, result, "OnSaveButton validate Customer Id");

            var guid = Guid.NewGuid().ToString().LeftStr(5);
            viewModel.KeyAutoFillValue = new AutoFillValue(new PrimaryKeyValue(lookupContext.Customers), guid);

            result = viewModel.OnSaveButton();
            Assert.AreEqual(DbMaintenanceResults.ValidationError, result, "OnSaveButton validate Company Name");

            viewModel.CompanyName = Guid.NewGuid().ToString();
            result = viewModel.OnSaveButton();
            Assert.AreEqual(DbMaintenanceResults.Success, result, "Save New Record");

            result = viewModel.OnDeleteButton();
            Assert.AreEqual(DbMaintenanceResults.Success, result, "Delete Record");
        }

        public void TestEmployeeViewModel()
        {
            var viewModel = new EmployeeViewModel();
            viewModel.OnViewLoaded(this);

            viewModel.OnGotoNextButton();
            Assert.AreEqual(2, viewModel.EmployeeId, "Goto First Record");

            viewModel.OnGotoPreviousButton();
            Assert.AreEqual(5, viewModel.EmployeeId, "Goto Last Record");

            viewModel.OnNewButton();
            Assert.AreEqual(string.Empty, viewModel.FirstName, "New Record");
            Assert.AreEqual(string.Empty, viewModel.LastName, "New Record");

            var result = viewModel.OnSaveButton();
            Assert.AreEqual(DbMaintenanceResults.ValidationError, result, "OnSaveButton validate First Name");

            var guid = Guid.NewGuid().ToString();
            viewModel.FirstName = guid.LeftStr(10);
            result = viewModel.OnSaveButton();
            Assert.AreEqual(DbMaintenanceResults.ValidationError, result, "OnSaveButton validate Last Name");

            viewModel.LastName = Guid.NewGuid().ToString().LeftStr(10);
            result = viewModel.OnSaveButton();
            Assert.AreEqual(DbMaintenanceResults.Success, result, "Save New Record");

            result = viewModel.OnDeleteButton();
            Assert.AreEqual(DbMaintenanceResults.Success, result, "Delete Record");
        }

        public void TestItemViewModel()
        {
            //Generate exactly 10,000 records into Items table for this to work.
            var viewModel = new ItemViewModel();
            viewModel.OnViewLoaded(this);

            viewModel.OnGotoNextButton();
            Assert.AreEqual(1, viewModel.ItemId, "Goto First Record");

            viewModel.OnGotoNextButton();
            Assert.AreEqual(1230, viewModel.ItemId, "Goto Next Record");

            viewModel.OnNewButton();
            Assert.AreEqual(0, viewModel.ItemId, "New Record");

            viewModel.OnGotoNextButton();
            viewModel.OnGotoPreviousButton();
            Assert.AreEqual(1231, viewModel.ItemId, "Goto Previous Record");

            viewModel.OnNewButton();
            var result = viewModel.OnSaveButton();
            Assert.AreEqual(DbMaintenanceResults.ValidationError, result, "OnSave validate Name");

            var lookupContext = RsDbLookupAppGlobals.EfProcessor.MegaDbLookupContext;
            var guid = Guid.NewGuid().ToString();
            viewModel.KeyAutoFillValue = new AutoFillValue(new PrimaryKeyValue(viewModel.TableDefinition), guid);
            result = viewModel.OnSaveButton();
            Assert.AreEqual(DbMaintenanceResults.ValidationError, result, "OnSave validate Location");

            var location = new Location {Id = 9};
            viewModel.LocationAutoFillValue =
                new AutoFillValue(lookupContext.Locations.GetPrimaryKeyValueFromEntity(location), "");
            result = viewModel.OnSaveButton();
            Assert.AreEqual(DbMaintenanceResults.ValidationError, result, "OnSave validate Manufacturer");

            var manufacturer = new Manufacturer {Id = 8};
            viewModel.ManufacturerAutoFillValue =
                new AutoFillValue(lookupContext.Manufacturers.GetPrimaryKeyValueFromEntity(manufacturer), string.Empty);

            result = viewModel.OnSaveButton();
            Assert.AreEqual(DbMaintenanceResults.Success, result, "Save New Record");

            result = viewModel.OnDeleteButton();
            Assert.AreEqual(DbMaintenanceResults.Success, result, "Delete Record");
        }
    }
}
