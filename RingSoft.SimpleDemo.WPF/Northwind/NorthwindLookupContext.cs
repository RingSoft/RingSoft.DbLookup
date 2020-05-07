using Microsoft.EntityFrameworkCore;
using RingSoft.DbLookup.EfCore;
using RingSoft.DbLookup.GetDataProcessor;
using RingSoft.DbLookup.Lookup;
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
        public TableDefinition<Shipper> Shippers { get; set; }

        public LookupDefinition<OrderLookup, Order> OrdersLookup { get; private set; }

        public LookupDefinition<OrderDetailLookup, Order_Detail> OrderDetailsLookup { get; private set; }

        public LookupDefinition<OrderDetailLookup, Order_Detail> OrderDetailsFormLookup { get; private set; }

        public LookupDefinition<ProductLookup, Product> ProductsLookup { get; private set; }

        public LookupDefinition<CustomerLookup, Customer> CustomerIdLookup { get; private set; }

        public LookupDefinition<CustomerLookup, Customer> CustomerNameLookup { get; private set; }

        public LookupDefinition<EmployeeLookup, Employee> EmployeesLookup { get; private set; }

        public LookupDefinition<ShipperLookup, Shipper> ShippersLookup { get; private set; }

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
            var extendedPriceFormula = "[Order Details].[Quantity] * [Order Details].[UnitPrice]";


            OrdersLookup = new LookupDefinition<OrderLookup, Order>(Orders);
            OrdersLookup.AddVisibleColumnDefinition(p => p.OrderId, "Order ID", p => p.OrderID, 15);
            OrdersLookup.AddVisibleColumnDefinition(p => p.OrderDate, "Date", p => p.OrderDate, 20);
            OrdersLookup.Include(p => p.Customer)
                .AddVisibleColumnDefinition(p => p.Customer, "Customer", p => p.CompanyName, 40);
            OrdersLookup.Include(p => p.Employee);
            OrdersLookup.AddVisibleColumnDefinition(p => p.Employee, "Employee", orderEmployeeNameFormula, 25);

            Orders.HasLookupDefinition(OrdersLookup);

            OrderDetailsLookup = new LookupDefinition<OrderDetailLookup, Order_Detail>(OrderDetails);
            OrderDetailsLookup.Include(p => p.Order)
                .AddVisibleColumnDefinition(p => p.OrderDate, "Order Date", p => p.OrderDate, 20);
            OrderDetailsLookup.Include(p => p.Product)
                .AddVisibleColumnDefinition(p => p.Product, "Product", p => p.ProductName, 40);
            OrderDetailsLookup.Include(p => p.Product).Include(p => p.Category)
                .AddVisibleColumnDefinition(p => p.CategoryName, "Category", p => p.CategoryName, 20);
            OrderDetailsLookup.AddVisibleColumnDefinition(p => p.Quantity, "Quantity", p => p.Quantity, 10);
            OrderDetailsLookup.AddVisibleColumnDefinition(p => p.UnitPrice, "Price", p => p.UnitPrice, 10);

            OrderDetails.HasLookupDefinition(OrderDetailsLookup);

            OrderDetailsFormLookup = new LookupDefinition<OrderDetailLookup, Order_Detail>(OrderDetails);
            OrderDetailsFormLookup.Include(p => p.Product)
                .AddVisibleColumnDefinition(p => p.Product, "Product", p => p.ProductName, 40);
            OrderDetailsFormLookup.AddVisibleColumnDefinition(p => p.Quantity, "Quantity", p => p.Quantity, 15);
            OrderDetailsFormLookup.AddVisibleColumnDefinition(p => p.UnitPrice, "Price", p => p.UnitPrice, 15);
            OrderDetailsFormLookup.AddVisibleColumnDefinition(p => p.ExtendedPrice, "Ext. Price", extendedPriceFormula, 15)
                .HasNumberFormatString("c").HasHorizontalAlignmentType(LookupColumnAlignmentTypes.Right);
            OrderDetailsFormLookup.AddVisibleColumnDefinition(p => p.Discount, "Discount", p => p.Discount, 15);

            ProductsLookup = new LookupDefinition<ProductLookup, Product>(Products);
            ProductsLookup.AddVisibleColumnDefinition(p => p.ProductName, "Name", p => p.ProductName, 40);
            ProductsLookup.Include(p => p.Category)
                .AddVisibleColumnDefinition(p => p.Category, "Category", p => p.CategoryName, 20);
            ProductsLookup.AddVisibleColumnDefinition(p => p.UnitsInStock, "Quantity On Hand", p => p.UnitsInStock, 20);
            ProductsLookup.AddVisibleColumnDefinition(p => p.UnitPrice, "Price", p => p.UnitPrice, 20);

            Products.HasLookupDefinition(ProductsLookup);

            CustomerIdLookup = new LookupDefinition<CustomerLookup, Customer>(Customers);
            CustomerIdLookup.AddVisibleColumnDefinition(p => p.CustomerId, "Customer Id", p => p.CustomerID, 20);
            CustomerIdLookup.AddVisibleColumnDefinition(p => p.CompanyName, "Company Name", p => p.CompanyName, 40);
            CustomerIdLookup.AddVisibleColumnDefinition(p => p.ContactName, "Contact", p => p.ContactName, 40);

            Customers.HasLookupDefinition(CustomerIdLookup);

            CustomerNameLookup = new LookupDefinition<CustomerLookup, Customer>(Customers);
            CustomerNameLookup.AddVisibleColumnDefinition(p => p.CompanyName, "Company Name", p => p.CompanyName, 60);
            CustomerNameLookup.AddVisibleColumnDefinition(p => p.ContactName, "Contact", p => p.ContactName, 40);

            EmployeesLookup = new LookupDefinition<EmployeeLookup, Employee>(Employees);
            EmployeesLookup.AddVisibleColumnDefinition(p => p.Name, "Name", employeeNameFormula, 40);
            EmployeesLookup.AddVisibleColumnDefinition(p => p.Title, "Title", p => p.Title, 20);
            EmployeesLookup.Include(p => p.Employee1);
            EmployeesLookup.AddVisibleColumnDefinition(p => p.Supervisor, "Supervisor", employeeSupervisorFormula, 40);

            Employees.HasLookupDefinition(EmployeesLookup);

            ShippersLookup = new LookupDefinition<ShipperLookup, Shipper>(Shippers);
            ShippersLookup.AddVisibleColumnDefinition(p => p.CompanyName, "Company Name", p => p.CompanyName, 75);
            ShippersLookup.AddVisibleColumnDefinition(p => p.Phone, "Phone", p => p.Phone, 25);

            Shippers.HasLookupDefinition(ShippersLookup);

        }
    }
}
