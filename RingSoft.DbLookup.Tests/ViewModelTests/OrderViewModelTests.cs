using System;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using RingSoft.DbLookup.App.Library;
using RingSoft.DbLookup.App.Library.Northwind.Model;
using RingSoft.DbLookup.App.Library.Northwind.ViewModels;
using RingSoft.DbMaintenance;

namespace RingSoft.DbLookup.Tests.ViewModelTests
{
    [TestClass]
    public class OrderViewModelTests
    {
        public static TestGlobals<OrderViewModel, TestDbMaintenanceView> Globals { get; private set; }

        [ClassInitialize]
        public static void Setup(TestContext testContext)
        {
            Globals = new TestGlobals<OrderViewModel, TestDbMaintenanceView>();
            SystemGlobals.LookupContext = RsDbLookupAppGlobals.EfProcessor.NorthwindLookupContext.LookupContext;
            Globals.PreInitializeEvent += (sender, args) =>
            {
                Globals.ViewModel.GridMode = true;
            };
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
            Globals.ViewModel.GridMode = true;
            
            Globals.ViewModel.SaveCommand.Execute(null);
            Globals.ViewModel.SaveCommand.Execute(null);
        }

        [TestMethod]
        public void TestOrderLoad()
        {
            Globals.ClearData();

            var category = new Category
            {
                CategoryName = "First Category",
            };

            var context = SystemGlobals.DataRepository.GetDataContext();
            context.SaveEntity(category, "Test Category");

            var product = new Product
            {
                ProductName = "First Product",
                CategoryID = category.CategoryId,
            };
            context.SaveEntity(product, "Saving Product");

            var customer = new Customer
            {
                CustomerID = "ALFKI",
                CompanyName = "First Customer",
            };

            context.SaveEntity(customer, "Saving Customer");

            var employee = new Employee
            {
                FirstName = "First",
                LastName = "Employee",
                FullName = "First Employee",
            };

            context.SaveEntity(employee, "Saving Employee");

            var secondEmployee = new Employee
            {
                FirstName = "Second",
                LastName = "Employee",
                FullName = "Second Employee",
                SupervisorId = employee.EmployeeID,
            };

            context.SaveEntity(secondEmployee, "Saving Employee");

            var shipper = new Shipper
            {
                CompanyName = "First Shipper",
            };

            context.SaveEntity(shipper, "Saving Shipper");

            var order = new Order
            {
                CustomerID = customer.CustomerID,
                EmployeeID = secondEmployee.EmployeeID,
                ShipVia = shipper.ShipperID,
                OrderDate = new DateTime(1980, 1, 1),
            };

            context.SaveEntity(order, "Saving Order");

            var detail = new Order_Detail
            {
                OrderID = order.OrderID,
                ProductID = product.ProductID,
                Quantity = 1,
                UnitPrice = 10.00,
            };

            context.SaveEntity(detail, "Saving Order Details");

            var table = context.GetTable<Order>();
            var loadOrder = table.FirstOrDefault(p => p.OrderID == order.OrderID);

            Globals.ViewModel.GridMode = true;
            Globals.ViewModel.OnRecordSelected(loadOrder.GetAutoFillValue().PrimaryKeyValue);
            Globals.ViewModel.SaveCommand.Execute(null);
            var odTable = context.GetTable<Order_Detail>();
            Assert.AreEqual(1, odTable.Count());
            Globals.ViewModel.DeleteCommand.Execute(null);
            odTable = context.GetTable<Order_Detail>();
            Assert.AreEqual(0, odTable.Count());
        }
    }
}
