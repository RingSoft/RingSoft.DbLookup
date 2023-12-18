using System;
using System.IO;
using System.Linq;
using RingSoft.DataEntryControls.Engine;
using RingSoft.DbLookup.App.Library.LibLookupContext;
using RingSoft.DbLookup.App.Library.MegaDb.Model;
using RingSoft.DbLookup.App.Library.Northwind.LookupModel;
using RingSoft.DbLookup.App.Library.Northwind.Model;
using RingSoft.DbLookup.DataProcessor;
using RingSoft.DbLookup.Lookup;
using RingSoft.DbLookup.ModelDefinition.FieldDefinitions;
using RingSoft.DbLookup.QueryBuilder;

namespace RingSoft.DbLookup.App.Library.Northwind
{
    public class ExtendedPriceFormula : ILookupFormula
    {
        public int Id => 1;

        public string GetDatabaseValue(object entity)
        {
            if (entity is Order_Detail orderDetail)
            {
                var extendedPrice = orderDetail.Quantity * orderDetail.UnitPrice;
                return Math.Round(extendedPrice, 2).ToString();
            }

            return string.Empty;
        }
    }

    public class NorthwindLookupContextConfiguration : AppLookupContextConfiguration, ILookupControl
    {
        public LookupDefinition<OrderLookup, Order> OrdersLookup { get; private set; }

        public LookupDefinition<OrderDetailLookup, Order_Detail> OrderDetailsLookup { get; private set; }

        public LookupDefinition<OrderDetailLookup, Order_Detail> OrderDetailsFormLookup { get; private set; }

        public LookupDefinition<ProductLookup, Product> ProductsLookup { get; private set; }

        public LookupDefinition<CustomerLookup, Customer> CustomerIdLookup { get; private set; }

        public LookupDefinition<CustomerLookup, Customer> CustomerNameLookup { get; private set; }

        public LookupDefinition<EmployeeLookup, Employee> EmployeesLookup { get; private set; }

        public LookupDefinition<EmployeeTerritoryLookup, EmployeeTerritory> EmployeeTerritoryLookup { get; private set; }

        public LookupDefinition<ShipperLookup, Shipper> ShippersLookup { get; private set; }

        public LookupDefinition<SupplierLookup, Supplier> SuppliersLookup { get; private set; }

        public LookupDefinition<CategoryLookup, Category> CategoriesLookup { get; private set; }

        private INorthwindLookupContext _lookupContext;
        
        public NorthwindLookupContextConfiguration(INorthwindLookupContext lookupContext)
        {
            _lookupContext = lookupContext;
            //_lookupContext.CanViewTableEvent += (sender, args) =>
            //{
            //    if (args.TableDefinition == _lookupContext.Orders)
            //    {
            //        args.AllowView = false;
            //    }
            //};
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

            //if (_lookupContext.Initialized)
            //    ConfigureLookups();
        }

        public string GetOrdersEmployeeNameFormula()
        {
            var orderEmployeeNameFormula = "[{Alias}].[FirstName] + ' ' + [{Alias}].[LastName]";
            switch (DataProcessorType)
            {
                case DataProcessorTypes.Sqlite:
                    orderEmployeeNameFormula = "[{Alias}].[FirstName] || ' ' || [{Alias}].[LastName]";
                    break;
                case DataProcessorTypes.SqlServer:
                    break;
                case DataProcessorTypes.MySql:
                    orderEmployeeNameFormula = "CONCAT(`{Alias}`.`FirstName`, ' ', `{Alias}`.`LastName`)";
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }

            return orderEmployeeNameFormula;
        }

        public string GetOrderFormula()
        {
            var result = string.Empty;
            switch (DataProcessorType)
            {
                case DataProcessorTypes.Sqlite:
                    result = "strftime('%m/%d/%Y', [{Alias}].[OrderDate]) || ' - ' || [{Alias}].[CustomerId]";
                    break;
                case DataProcessorTypes.SqlServer:
                    result = "FORMAT ([{Alias}].[OrderDate], 'dd/MM/yyyy ') + ' - ' + [{Alias}].[CustomerId]";
                    break;
                case DataProcessorTypes.MySql:
                    result = "CONCAT(`{Alias}`.`OrderDate`, ' - ', `{Alias}`.`CustomerId`)";
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }

            return result;
        }

        public void ConfigureLookups()
        {
            OrdersLookup = new LookupDefinition<OrderLookup, Order>(_lookupContext.Orders);
            OrdersLookup.AddVisibleColumnDefinition(p => p.Order
                , "Order", p => p.OrderName, 20);

            var orderInclude = OrdersLookup
                .Include(p => p.Customer);
            orderInclude.AddVisibleColumnDefinition(p => p.Customer
                , "Customer"
                , p => p.CompanyName, 50);

            var join = OrdersLookup
                .Include(p => p.Employee);
            join.AddVisibleColumnDefinition(p => p.Employee
                , "Employee"
                , p => p.FullName, 30);

            _lookupContext.Orders.HasLookupDefinition(OrdersLookup);

            OrderDetailsLookup = new LookupDefinition<OrderDetailLookup, Order_Detail>(_lookupContext.OrderDetails);
            OrderDetailsLookup.Include(p => p.Order)
                .AddVisibleColumnDefinition(p => p.Order, "Order"
                    , p => p.OrderName, 20);
            var productInclude =  OrderDetailsLookup.Include(p => p.Product);

            productInclude.AddVisibleColumnDefinition(p => p.Product, "Product", p => p.ProductName, 40);

            var categoryInclude = productInclude.Include(p => p.Category);
                
            categoryInclude.AddVisibleColumnDefinition(p => p.CategoryName, "Category", p => p.CategoryName, 20);
            OrderDetailsLookup.AddVisibleColumnDefinition(p => p.Quantity, "Quantity", p => p.Quantity, 10);
            OrderDetailsLookup.AddVisibleColumnDefinition(p => p.UnitPrice, "Price", p => p.UnitPrice, 10);

            _lookupContext.OrderDetails.HasLookupDefinition(OrderDetailsLookup);

            OrderDetailsFormLookup = new LookupDefinition<OrderDetailLookup, Order_Detail>(_lookupContext.OrderDetails);
            OrderDetailsFormLookup.Include(p => p.Product)
                .AddVisibleColumnDefinition(p => p.Product, "Product", p => p.ProductName, 40);
            OrderDetailsFormLookup.AddVisibleColumnDefinition(p => p.Quantity, "Quantity", p => p.Quantity, 15);
            OrderDetailsFormLookup.AddVisibleColumnDefinition(p => p.UnitPrice, "Price", p => p.UnitPrice, 15);
            OrderDetailsFormLookup.AddVisibleColumnDefinition(p => p.ExtendedPrice, "Extended\r\nPrice"
                    , new ExtendedPriceFormula(), 15, "")
                .HasDecimalFieldType(DecimalFieldTypes.Currency)
                .HasHorizontalAlignmentType(LookupColumnAlignmentTypes.Right)
                .DoShowNegativeValuesInRed().HasDescription("Extended Price");
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

            //var formula = "SELECT Customers.CustomerId, Customers.CompanyName, Customers.ContactName FROM Customers WHERE Customers.CustomerId = 'ALFKI'";
            //CustomerIdLookup.HasFromFormula(formula);

            //DbDataProcessor.ShowSqlStatementWindow();

            _lookupContext.Customers.HasLookupDefinition(CustomerIdLookup);

            CustomerNameLookup = new LookupDefinition<CustomerLookup, Customer>(_lookupContext.Customers);
            CustomerNameLookup.AddVisibleColumnDefinition(p => p.CompanyName, "Company Name", p => p.CompanyName, 60);
            CustomerNameLookup.AddVisibleColumnDefinition(p => p.ContactName, "Contact", p => p.ContactName, 40);

            EmployeesLookup = new LookupDefinition<EmployeeLookup, Employee>(_lookupContext.Employees);
            EmployeesLookup
                .AddVisibleColumnDefinition(p => p.Name
                    , "Name"
                    , p => p.FullName, 40);
            EmployeesLookup
                .AddVisibleColumnDefinition(p => p.Title
                    , "Title"
                    , p => p.Title, 20);
            var employeeJoin = EmployeesLookup
                .Include(p => p.Supervisor);
            employeeJoin.AddVisibleColumnDefinition(p => p.Supervisor
                , "Supervisor"
                , p => p.FullName, 40);

            _lookupContext.Employees.HasLookupDefinition(EmployeesLookup);

            EmployeeTerritoryLookup = new LookupDefinition<EmployeeTerritoryLookup, EmployeeTerritory>
                (_lookupContext.EmployeeTerritories);
            EmployeeTerritoryLookup.Include(p => p.Employee)
                .AddVisibleColumnDefinition(p => p.Employee
                    , "Employee"
                    , p => p.FullName, 50);
            EmployeeTerritoryLookup.Include(p => p.Territory)
                .AddVisibleColumnDefinition(p => p.Territory
                    , "Territory"
                    , p => p.TerritoryDescription, 50);
            
            _lookupContext.EmployeeTerritories.HasLookupDefinition(EmployeeTerritoryLookup);

            ShippersLookup = new LookupDefinition<ShipperLookup, Shipper>(_lookupContext.Shippers);
            ShippersLookup.AddVisibleColumnDefinition(p => p.CompanyName, "Company Name", p => p.CompanyName, 75);
            ShippersLookup.AddVisibleColumnDefinition(p => p.Phone, "Phone", p => p.Phone, 25);
            ShippersLookup.AllowAddOnTheFly = false;

            _lookupContext.Shippers.HasLookupDefinition(ShippersLookup);

            SuppliersLookup = new LookupDefinition<SupplierLookup, Supplier>(_lookupContext.Suppliers);
            SuppliersLookup.AddVisibleColumnDefinition(p => p.CompanyName, "Company Name", p => p.CompanyName, 60);
            SuppliersLookup.AddVisibleColumnDefinition(p => p.ContactName, "Contact", p => p.ContactName, 40);
            SuppliersLookup.AllowAddOnTheFly = false;
            _lookupContext.Suppliers.HasLookupDefinition(SuppliersLookup);

            CategoriesLookup = new LookupDefinition<CategoryLookup, Category>(_lookupContext.Categories);
            CategoriesLookup.AddVisibleColumnDefinition(p => p.CategoryName, "Category Name", p => p.CategoryName, 40);
            CategoriesLookup.AddVisibleColumnDefinition(p => p.Description, "Description", p => p.Description, 60);
            CategoriesLookup.AllowAddOnTheFly = false;
            _lookupContext.Categories.HasLookupDefinition(CategoriesLookup);

        }

        public override bool TestConnection()
        {
            try
            {
                RsDbLookupAppGlobals.EfProcessor.NorthwindEfDataProcessor.SetDataContext();
                var context = SystemGlobals.DataRepository.GetDataContext();
                var table = context.GetTable<Product>();
                var product = table
                    .FirstOrDefault(p => p.ProductID == 1);
                DataProcessor.IsValid = true;
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                ControlsGlobals.UserInterface.ShowMessageBox(e.Message, "Error!", RsMessageBoxIcons.Error);
                DataProcessor.IsValid = false;
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
            _lookupContext.OrderDetails.GetFieldDefinition(p => p.UnitPrice)
                .HasDecimalFieldType(DecimalFieldTypes.Currency)
                .DoShowNegativeValuesInRed();

            _lookupContext.OrderDetails.GetFieldDefinition(p => p.Discount)
                .HasDecimalFieldType(DecimalFieldTypes.Currency);

            _lookupContext.Orders.GetFieldDefinition(p => p.Freight)
                .HasDecimalFieldType(DecimalFieldTypes.Currency);

            _lookupContext.Employees.GetFieldDefinition(p => p.SupervisorId).HasDescription("Supervisor");
                //.DoesAllowRecursion(false);

            _lookupContext.Employees.GetFieldDefinition(p => p.Notes).IsMemo();

            _lookupContext.Products.GetFieldDefinition(p => p.UnitPrice)
                .HasDecimalFieldType(DecimalFieldTypes.Currency);

            //var chunk = _lookupContext.OrderDetails.GetChunk(100);
            //chunk = _lookupContext.OrderDetails.GetChunk(100, chunk.BottomPrimaryKey);

            //chunk = _lookupContext.Orders.GetChunk(100);
            //chunk = _lookupContext.Orders.GetChunk(100, chunk.BottomPrimaryKey);

            //chunk = _lookupContext.Employees.GetChunk(100);
            //if (chunk.Chunk.Rows.Count >= 100)
            //{
            //    chunk = _lookupContext.Employees.GetChunk(100, chunk.BottomPrimaryKey);
            //}
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
