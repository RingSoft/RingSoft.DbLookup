using System;
using System.IO;
using RingSoft.DbLookup.App.Library.LookupContext;
using RingSoft.DbLookup.App.Library.Northwind.LookupModel;
using RingSoft.DbLookup.App.Library.Northwind.Model;
using RingSoft.DbLookup.GetDataProcessor;
using RingSoft.DbLookup.Lookup;
using RingSoft.DbLookup.ModelDefinition.FieldDefinitions;

namespace RingSoft.DbLookup.App.Library.Northwind
{
    public class NorthwindLookupContextConfiguration : AppLookupContextConfiguration, ILookupUserInterface
    {
        public LookupDefinition<OrderLookup, Order> OrdersLookup { get; private set; }

        public LookupDefinition<OrderDetailLookup, Order_Detail> OrderDetailsLookup { get; private set; }

        public LookupDefinition<OrderDetailLookup, Order_Detail> OrderDetailsFormLookup { get; private set; }

        public LookupDefinition<ProductLookup, Product> ProductsLookup { get; private set; }

        public LookupDefinition<CustomerLookup, Customer> CustomerIdLookup { get; private set; }

        public LookupDefinition<CustomerLookup, Customer> CustomerNameLookup { get; private set; }

        public LookupDefinition<EmployeeLookup, Employee> EmployeesLookup { get; private set; }

        public LookupDefinition<ShipperLookup, Shipper> ShippersLookup { get; private set; }

        private INorthwindLookupContext _lookupContext;
        public NorthwindLookupContextConfiguration(INorthwindLookupContext lookupContext)
        {
            _lookupContext = lookupContext;
            Reinitialize();
        }

        public override void Reinitialize(RegistrySettings registrySettings)
        {
            switch (registrySettings.NorthwindPlatformType)
            {
                case NorthwindDbPlatforms.SqlServer:
                    DataProcessorType = DataProcessorTypes.SqlServer;
                    break;
                case NorthwindDbPlatforms.MySql:
                    DataProcessorType = DataProcessorTypes.MySql;
                    break;
                case NorthwindDbPlatforms.Sqlite:
                    DataProcessorType = DataProcessorTypes.Sqlite;
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }

            SqlServerDataProcessor.Database = registrySettings.SqlServerNorthwindDbName;
            MySqlDataProcessor.Database = registrySettings.MySqlNorthwindDbName;
            SqliteDataProcessor.FilePath = Path.GetDirectoryName(registrySettings.NorthwindSqliteFileName);
            SqliteDataProcessor.FileName = Path.GetFileName(registrySettings.NorthwindSqliteFileName);

            base.Reinitialize(registrySettings);

            if (_lookupContext.Initialized)
                ConfigureLookups();
        }

        public string GetOrdersEmployeeNameFormula()
        {
            var orderEmployeeNameFormula = "[Orders_Employees_EmployeeID].[FirstName] + ' ' + [Orders_Employees_EmployeeID].[LastName]";
            switch (DataProcessorType)
            {
                case DataProcessorTypes.Sqlite:
                    orderEmployeeNameFormula = "[Orders_Employees_EmployeeID].[FirstName] || ' ' || [Orders_Employees_EmployeeID].[LastName]";
                    break;
                case DataProcessorTypes.SqlServer:
                    break;
                case DataProcessorTypes.MySql:
                    orderEmployeeNameFormula = "CONCAT(`Orders_Employees_EmployeeID`.`FirstName`, ' ', `Orders_Employees_EmployeeID`.`LastName`)";
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }

            return orderEmployeeNameFormula;
        }

        public void ConfigureLookups()
        {
            var employeeNameFormula = "[Employees].[FirstName] + ' ' + [Employees].[LastName]";
            var employeeSupervisorFormula = "[Employees_Employees_ReportsTo].[FirstName] + ' ' + [Employees_Employees_ReportsTo].[LastName]";
            var orderEmployeeNameFormula = GetOrdersEmployeeNameFormula();
            var extendedPriceFormula = "[Order Details].[Quantity] * [Order Details].[UnitPrice]";

            switch (DataProcessorType)
            {
                case DataProcessorTypes.Sqlite:
                    employeeNameFormula = "[Employees].[FirstName] || ' ' || [Employees].[LastName]";
                    employeeSupervisorFormula = "[Employees_Employees_ReportsTo].[FirstName] || ' ' || [Employees_Employees_ReportsTo].[LastName]";
                    break;
                case DataProcessorTypes.SqlServer:
                    break;
                case DataProcessorTypes.MySql:
                    employeeNameFormula = "CONCAT(`Employees`.`FirstName`, ' ', `Employees`.`LastName`)";
                    employeeSupervisorFormula = "CONCAT(`Employees_Employees_ReportsTo`.`FirstName`, ' ', `Employees_Employees_ReportsTo`.`LastName`)";
                    extendedPriceFormula = "`order details`.`Quantity` * `order details`.`UnitPrice`";
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }

            OrdersLookup = new LookupDefinition<OrderLookup, Order>(_lookupContext.Orders);
            OrdersLookup.AddVisibleColumnDefinition(p => p.OrderId, "Order ID", p => p.OrderID, 15);
            OrdersLookup.AddVisibleColumnDefinition(p => p.OrderDate, "Date", p => p.OrderDate, 20);
            OrdersLookup.Include(p => p.Customer)
                .AddVisibleColumnDefinition(p => p.Customer, "Customer", p => p.CompanyName, 40);
            OrdersLookup.Include(p => p.Employee);
            OrdersLookup.AddVisibleColumnDefinition(p => p.Employee, "Employee", orderEmployeeNameFormula, 25);

            _lookupContext.Orders.HasLookupDefinition(OrdersLookup);

            OrderDetailsLookup = new LookupDefinition<OrderDetailLookup, Order_Detail>(_lookupContext.OrderDetails);
            OrderDetailsLookup.Include(p => p.Order)
                .AddVisibleColumnDefinition(p => p.OrderDate, "Order Date", p => p.OrderDate, 20);
            OrderDetailsLookup.Include(p => p.Product)
                .AddVisibleColumnDefinition(p => p.Product, "Product", p => p.ProductName, 40);
            OrderDetailsLookup.Include(p => p.Product).Include(p => p.Category)
                .AddVisibleColumnDefinition(p => p.CategoryName, "Category", p => p.CategoryName, 20);
            OrderDetailsLookup.AddVisibleColumnDefinition(p => p.Quantity, "Quantity", p => p.Quantity, 10);
            OrderDetailsLookup.AddVisibleColumnDefinition(p => p.UnitPrice, "Price", p => p.UnitPrice, 10);

            _lookupContext.OrderDetails.HasLookupDefinition(OrderDetailsLookup);

            OrderDetailsFormLookup = new LookupDefinition<OrderDetailLookup, Order_Detail>(_lookupContext.OrderDetails);
            OrderDetailsFormLookup.Include(p => p.Product)
                .AddVisibleColumnDefinition(p => p.Product, "Product", p => p.ProductName, 40);
            OrderDetailsFormLookup.AddVisibleColumnDefinition(p => p.Quantity, "Quantity", p => p.Quantity, 15);
            OrderDetailsFormLookup.AddVisibleColumnDefinition(p => p.UnitPrice, "Price", p => p.UnitPrice, 15);
            OrderDetailsFormLookup.AddVisibleColumnDefinition(p => p.ExtendedPrice, "Ext. Price", extendedPriceFormula, 15)
                .HasNumberFormatString("c").HasHorizontalAlignmentType(LookupColumnAlignmentTypes.Right);
            OrderDetailsFormLookup.AddVisibleColumnDefinition(p => p.Discount, "Discount", p => p.Discount, 15);
            
            ProductsLookup = new LookupDefinition<ProductLookup, Product>(_lookupContext.Products);
            ProductsLookup.AddVisibleColumnDefinition(p => p.ProductName, "Name", p => p.ProductName, 40);
            ProductsLookup.Include(p => p.Category)
                .AddVisibleColumnDefinition(p => p.Category, "Category", p => p.CategoryName, 20);
            ProductsLookup.AddVisibleColumnDefinition(p => p.UnitsInStock, "Quantity On Hand", p => p.UnitsInStock, 20);
            ProductsLookup.AddVisibleColumnDefinition(p => p.UnitPrice, "Price", p => p.UnitPrice, 20);

            _lookupContext.Products.HasLookupDefinition(ProductsLookup);

            CustomerIdLookup = new LookupDefinition<CustomerLookup, Customer>(_lookupContext.Customers);
            CustomerIdLookup.AddVisibleColumnDefinition(p => p.CustomerId, "Customer Id", p => p.CustomerID, 20);
            CustomerIdLookup.AddVisibleColumnDefinition(p => p.CompanyName, "Company Name", p => p.CompanyName, 40);
            CustomerIdLookup.AddVisibleColumnDefinition(p => p.ContactName, "Contact", p => p.ContactName, 40);

            _lookupContext.Customers.HasLookupDefinition(CustomerIdLookup);

            CustomerNameLookup = new LookupDefinition<CustomerLookup, Customer>(_lookupContext.Customers);
            CustomerNameLookup.AddVisibleColumnDefinition(p => p.CompanyName, "Company Name", p => p.CompanyName, 60);
            CustomerNameLookup.AddVisibleColumnDefinition(p => p.ContactName, "Contact", p => p.ContactName, 40);

            EmployeesLookup = new LookupDefinition<EmployeeLookup, Employee>(_lookupContext.Employees);
            EmployeesLookup.AddVisibleColumnDefinition(p => p.Name, "Name", employeeNameFormula, 40);
            EmployeesLookup.AddVisibleColumnDefinition(p => p.Title, "Title", p => p.Title, 20);
            EmployeesLookup.Include(p => p.Employee1);
            EmployeesLookup.AddVisibleColumnDefinition(p => p.Supervisor, "Supervisor", employeeSupervisorFormula, 40);

            _lookupContext.Employees.HasLookupDefinition(EmployeesLookup);

            ShippersLookup = new LookupDefinition<ShipperLookup, Shipper>(_lookupContext.Shippers);
            ShippersLookup.AddVisibleColumnDefinition(p => p.CompanyName, "Company Name", p => p.CompanyName, 75);
            ShippersLookup.AddVisibleColumnDefinition(p => p.Phone, "Phone", p => p.Phone, 25);

            _lookupContext.Shippers.HasLookupDefinition(ShippersLookup);
        }

        public override bool TestConnection()
        {
            var ordersLookupData = new LookupDataBase(OrdersLookup, this);
            var result = ordersLookupData.GetInitData();
            return result.ResultCode == GetDataResultCodes.Success;
        }

        public override bool TestConnection(RegistrySettings registrySettings)
        {
            Reinitialize(registrySettings);
            return TestConnection();
        }
        
        public void InitializeModel()
        {
            _lookupContext.Orders.HasDescription("Orders");
            _lookupContext.Customers.HasDescription("Customers");

            _lookupContext.OrderDetails.HasDescription("Order Details");
            _lookupContext.OrderDetails.GetFieldDefinition(p => p.ProductID).HasDescription("Product");
            _lookupContext.OrderDetails.GetFieldDefinition(p => p.UnitPrice)
                .HasDecimalFieldType(DecimalFieldTypes.Currency);
            _lookupContext.OrderDetails.GetFieldDefinition(p => p.Discount)
                .HasDecimalFieldType(DecimalFieldTypes.Currency);

            _lookupContext.Employees.HasDescription("Employees");
            _lookupContext.Employees.GetFieldDefinition(p => p.ReportsTo).HasDescription("Supervisor");
        }

        public int PageSize { get; } = 15;
        public LookupSearchTypes SearchType { get; } = LookupSearchTypes.Equals;
        public string SearchText { get; set; } = string.Empty;
    }
}
