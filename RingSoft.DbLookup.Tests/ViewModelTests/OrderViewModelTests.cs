using Microsoft.VisualStudio.TestTools.UnitTesting;
using RingSoft.DbLookup.App.Library.Northwind.Model;
using RingSoft.DbLookup.App.Library.Northwind.ViewModels;
using RingSoft.DbMaintenance;

namespace RingSoft.DbLookup.Tests.ViewModelTests
{
    [TestClass]
    public class OrderViewModelTests
    {
        public static TestGlobals<OrderViewModel, TestOrderView> Globals { get; } =
            new TestGlobals<OrderViewModel, TestOrderView>();

        [ClassInitialize]
        public static void Setup(TestContext testContext)
        {
            Globals.Initialize();
        }

        [TestMethod]
        public void TestOrderSave()
        {
            Globals.ClearData();
            Assert.AreEqual(false, Globals.ViewModel.RecordDirty);

            var customer = new Customer
            {
                CustomerID = "ALFKI",
                CompanyName = "ALFKI Customer"
            };
            Globals.ViewModel.Customer = customer.GetAutoFillValue();

            var employee = new Employee
            {
                EmployeeID = 1,
                FullName = "John Doe",
                FirstName = "John",
                LastName = "Doe",
            };
            Globals.ViewModel.Employee = employee.GetAutoFillValue();

            var shipVia = new Shipper
            {
                ShipperID = 1,
                CompanyName = "FedEx",
            };
            Globals.ViewModel.ShipVia = shipVia.GetAutoFillValue();

            Globals.ViewModel.SaveCommand.Execute(null);
            Globals.ViewModel.SaveCommand.Execute(null);
        }
    }
}
