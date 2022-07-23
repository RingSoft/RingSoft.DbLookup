using Microsoft.EntityFrameworkCore;
using RingSoft.DbLookup.DataProcessor;
using RingSoft.DbLookup.EfCore;
using RingSoft.DbLookup.ModelDefinition;
using RingSoft.DbLookup.ModelDefinition.FieldDefinitions;
using RingSoft.SimpleDemo.WPF.Northwind.LookupModel;
using RingSoft.SimpleDemo.WPF.Northwind.Model;
using System;
using System.IO;
using System.Reflection;

namespace RingSoft.SimpleDemo.WPF.Northwind
{
    public class NorthwindLookupContext : LookupContext
    {
        protected override DbContext DbContext { get; }
        public override DbDataProcessor DataProcessor { get; }

        public TableDefinition<Category> Categories { get; set; }
        public TableDefinition<Customer> Customers { get; set; }
        public TableDefinition<Employee> Employees { get; set; }
        public TableDefinition<Order> Orders { get; set; }
        public TableDefinition<Order_Detail> OrderDetails { get; set; }
        public TableDefinition<Product> Products { get; set; }

        public DemoLookupDefinition<OrderLookup, Order> OrdersLookup { get; private set; }
        public DemoLookupDefinition<CustomerLookup, Customer> CustomerIdLookup { get; private set; }
        public DemoLookupDefinition<EmployeeLookup, Employee> EmployeesLookup { get; private set; }

        public NorthwindLookupContext()
        {
            DbContext = new NorthwindDbContext(this);

            var codeBase = Assembly.GetExecutingAssembly().CodeBase;
            var uri = new UriBuilder(codeBase);
            var path = Uri.UnescapeDataString(uri.Path);
            var appPath = Path.GetDirectoryName(path);

            DataProcessor = new SqliteDataProcessor()
            {
                FilePath = $"{appPath}\\Northwind\\",
                FileName = "Northwind.sqlite"
            };
            Initialize();
        }

        protected override void SetupModel()
        {
            Orders.HasDescription("Orders");
            Customers.HasDescription("Customers");

            OrderDetails.HasDescription("Order Details");
            OrderDetails.GetFieldDefinition(p => p.ProductID).HasDescription("Product");
            OrderDetails.GetFieldDefinition(p => p.UnitPrice)
                .HasDecimalFieldType(DecimalFieldTypes.Currency);
            OrderDetails.GetFieldDefinition(p => p.Discount)
                .HasDecimalFieldType(DecimalFieldTypes.Currency);

            Employees.HasDescription("Employees");
            Employees.GetFieldDefinition(p => p.ReportsTo).HasDescription("Supervisor");
        }

        protected override void InitializeLookupDefinitions()
        {
            var employeeNameFormula = "[Employees].[FirstName] || ' ' || [Employees].[LastName]";
            var employeeSupervisorFormula = "[Employees_Employees_ReportsTo].[FirstName] || ' ' || [Employees_Employees_ReportsTo].[LastName]";
            var orderEmployeeNameFormula = "[Orders_Employees_EmployeeID].[FirstName] || ' ' || [Orders_Employees_EmployeeID].[LastName]";
            
            OrdersLookup = new DemoLookupDefinition<OrderLookup, Order>(Orders);
            OrdersLookup.AddVisibleColumnDefinition(p => p.OrderId, "Order ID", p => p.OrderID, 15);
            OrdersLookup.AddVisibleColumnDefinition(p => p.OrderDate, "Date", p => p.OrderDate, 20);
            OrdersLookup.Include(p => p.Customer)
                .AddVisibleColumnDefinition(p => p.Customer, "Customer", p => p.CompanyName, 40);
            OrdersLookup.Include(p => p.Employee);
            OrdersLookup.AddVisibleColumnDefinition(p => p.Employee, "Employee", orderEmployeeNameFormula, 25);
            OrdersLookup.TopHeader = "Use this window to select an Order.";

            OrdersLookup = OrdersLookup.Clone();
            Orders.HasLookupDefinition(OrdersLookup);

            CustomerIdLookup = new DemoLookupDefinition<CustomerLookup, Customer>(Customers);
            CustomerIdLookup.AddVisibleColumnDefinition(p => p.CustomerId, "Customer Id", p => p.CustomerID, 20);
            CustomerIdLookup.AddVisibleColumnDefinition(p => p.CompanyName, "Company Name", p => p.CompanyName, 40);
            CustomerIdLookup.AddVisibleColumnDefinition(p => p.ContactName, "Contact", p => p.ContactName, 40);
            CustomerIdLookup.TopHeader = "Use this window to select a Customer.";

            var formula = "SELECT Customers.CustomerId, Customers.CompanyName, Customers.ContactName FROM Customers";
            CustomerIdLookup.HasFromFormula(formula);

            CustomerIdLookup = CustomerIdLookup.Clone();
            Customers.HasLookupDefinition(CustomerIdLookup);

            EmployeesLookup = new DemoLookupDefinition<EmployeeLookup, Employee>(Employees);
            EmployeesLookup.AddVisibleColumnDefinition(p => p.Name, "Name", employeeNameFormula, 40);
            EmployeesLookup.AddVisibleColumnDefinition(p => p.Title, "Title", p => p.Title, 20);
            EmployeesLookup.Include(p => p.Employee1);
            EmployeesLookup.AddVisibleColumnDefinition(p => p.Supervisor, "Supervisor", employeeSupervisorFormula, 40);
            EmployeesLookup.TopHeader = "Use this window to select an Employee.";

            EmployeesLookup = EmployeesLookup.Clone();
            Employees.HasLookupDefinition(EmployeesLookup);
        }
    }
}
