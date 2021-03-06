﻿using RingSoft.DataEntryControls.Engine;
using RingSoft.DbLookup.App.Library.Northwind.LookupModel;
using RingSoft.DbLookup.App.Library.Northwind.Model;
using RingSoft.DbLookup.AutoFill;
using RingSoft.DbLookup.Lookup;
using RingSoft.DbLookup.ModelDefinition;
using RingSoft.DbLookup.ModelDefinition.FieldDefinitions;
using RingSoft.DbLookup.QueryBuilder;
using RingSoft.DbLookup.TableProcessing;
using RingSoft.DbMaintenance;
using System;
using System.ComponentModel;
using System.Linq;
using System.Text;

namespace RingSoft.DbLookup.App.Library.Northwind.ViewModels
{
    public interface IOrderView : IDbMaintenanceView
    {
        object OwnerWindow { get; }

        void SetFocusToGrid(OrderDetailsGridRow row, int columnId);
    }

    public class OrderInput
    {
        public bool GridMode { get; set; }

        public int ProductId { get; set; }
    }
    public class OrderViewModel : DbMaintenanceViewModel<Order>
    {
        public IOrderView OrderView { get; private set; }

        public override TableDefinition<Order> TableDefinition =>
            RsDbLookupAppGlobals.EfProcessor.NorthwindLookupContext.Orders;

        private int _orderId;
        public int OrderId
        {
            get => _orderId;
            set
            {
                if (value == _orderId) 
                    return;

                _orderId = value;
                OnPropertyChanged(nameof(OrderId));
            }
        }

        private AutoFillSetup _customerAutoFillSetup;

        public AutoFillSetup CustomersAutoFillSetup
        {
            get => _customerAutoFillSetup;
            set
            {
                if (_customerAutoFillSetup == value)
                    return;

                _customerAutoFillSetup = value;
                OnPropertyChanged(nameof(CustomersAutoFillSetup), false);
            }
        }

        private AutoFillValue _customer;
        public AutoFillValue Customer
        {
            get => _customer;
            set
            {
                if (_customer == value)
                    return;

                _customer = value;
                _customerDirty = true;
                OnPropertyChanged(nameof(Customer));
            }
        }

        private string _companyName;

        public string CompanyName
        {
            get => _companyName;
            set
            {
                if (_companyName == value)
                    return;

                _companyName = value;
                OnPropertyChanged(nameof(CompanyName));
            }
        }

        private AutoFillSetup _employeeAutoFillSetup;
        public AutoFillSetup EmployeeAutoFillSetup
        {
            get => _employeeAutoFillSetup;
            set
            {
                if (_employeeAutoFillSetup == value)
                    return;

                _employeeAutoFillSetup = value;
                OnPropertyChanged(nameof(EmployeeAutoFillSetup), false);
            }
        }

        private AutoFillValue _employee;
        public AutoFillValue Employee
        {
            get => _employee;
            set
            {
                if (_employee == value)
                    return;

                _employee = value;
                OnPropertyChanged(nameof(Employee));
            }
        }

        private AutoFillSetup _shipViaAutoFillSetup;

        public AutoFillSetup ShipViaAutoFillSetup
        {
            get => _shipViaAutoFillSetup;
            set
            {
                if (_shipViaAutoFillSetup == value)
                    return;

                _shipViaAutoFillSetup = value;
                OnPropertyChanged(nameof(ShipViaAutoFillSetup), false);
            }
        }

        private AutoFillValue _shipVia;
        public AutoFillValue ShipVia
        {
            get => _shipVia;
            set
            {
                if (_shipVia == value)
                    return;

                _shipVia = value;
                OnPropertyChanged(nameof(ShipVia));
            }
        }

        private DateTime? _requiredDate;
        public DateTime? RequiredDate
        {
            get => _requiredDate;
            set
            {
                if (_requiredDate == value)
                    return;
                _requiredDate = value;
                OnPropertyChanged(nameof(RequiredDate));
            }
        }

        private DateTime _orderDate;
        public DateTime OrderDate
        {
            get => _orderDate;
            set
            {
                if (_orderDate == value)
                    return;
                _orderDate = value;
                OnPropertyChanged(nameof(OrderDate));
            }
        }

        private DateTime? _shippedDate;

        public DateTime? ShippedDate
        {
            get => _shippedDate;
            set
            {
                if (_shippedDate == value)
                    return;

                _shippedDate = value;
                OnPropertyChanged(nameof(ShippedDate));
            }
        }

        private decimal? _freight;
        public decimal? Freight
        {
            get => _freight;
            set
            {
                if (_freight == value)
                    return;

                _freight = value;
                OnPropertyChanged(nameof(Freight));
                if (!ChangingEntity)
                    RefreshTotalControls();
            }
        }

        [CanBeNull] private string _shipName;
        [CanBeNull]
        public string ShipName
        {
            get => _shipName;
            set
            {
                if (_shipName == value)
                    return;

                _shipName = value;
                OnPropertyChanged(nameof(ShipName));
            }
        }

        [CanBeNull] private string _address;

        [CanBeNull]
        public string Address
        {
            get => _address;
            set
            {
                if (_address == value)
                    return;

                _address = value;
                OnPropertyChanged(nameof(Address));
            }
        }

        [CanBeNull] private string _city;

        [CanBeNull]
        public string City
        {
            get => _city;
            set
            {
                if (_city == value)
                    return;

                _city = value;
                OnPropertyChanged(nameof(City));
            }
        }

        [CanBeNull] private string _region;
        [CanBeNull]
        public string Region
        {
            get => _region;
            set
            {
                if (_region == value)
                    return;

                _region = value;
                OnPropertyChanged(nameof(Region));
            }
        }

        [CanBeNull] private string _postalCode;

        [CanBeNull]
        public string PostalCode
        {
            get => _postalCode;
            set
            {
                if (_postalCode == value)
                    return;

                _postalCode = value;
                OnPropertyChanged(nameof(PostalCode));
            }
        }

        [CanBeNull] private string _country;

        [CanBeNull]
        public string Country
        {
            get => _country;
            set
            {
                if (_country == value)
                    return;

                _country = value;
                OnPropertyChanged(nameof(Country));
            }
        }

        private LookupDefinition<OrderDetailLookup, Order_Detail> _orderDetailsLookup;

        public LookupDefinition<OrderDetailLookup, Order_Detail> OrderDetailsLookupDefinition
        {
            get => _orderDetailsLookup;
            set
            {
                if (_orderDetailsLookup == value)
                    return;

                _orderDetailsLookup = value;
                OnPropertyChanged(nameof(OrderDetailsLookupDefinition), false);
            }
        }

        private LookupCommand _orderDetailsLookupCommand;

        public LookupCommand OrderDetailsLookupCommand
        {
            get => _orderDetailsLookupCommand;
            set
            {
                if (_orderDetailsLookupCommand == value)
                    return;

                _orderDetailsLookupCommand = value;
                OnPropertyChanged(nameof(OrderDetailsLookupCommand), false);
            }
        }

        private LookupDataSourceChanged _orderDetailsDataSourceChanged;

        public LookupDataSourceChanged OrderDetailsDataSourceChanged
        {
            get => _orderDetailsDataSourceChanged;
            set
            {
                if (_orderDetailsDataSourceChanged == value)
                    return;

                _orderDetailsDataSourceChanged = value;
                OnPropertyChanged(nameof(OrderDetailsDataSourceChanged));
                RefreshTotalControls();
            }
        }

        private decimal _subTotal;

        public decimal SubTotal
        {
            get => _subTotal;
            set
            {
                if (_subTotal == value)
                    return;

                _subTotal = value;
                OnPropertyChanged(nameof(SubTotal));
            }
        }

        private decimal _totalDiscount;
        public decimal TotalDiscount
        {
            get => _totalDiscount;
            set
            {
                if (_totalDiscount == value)
                    return;

                _totalDiscount = value;
                OnPropertyChanged(nameof(TotalDiscount));
            }
        }

        private decimal _total;

        public decimal Total
        {
            get => _total;
            set
            {
                if (_total == value)
                    return;

                _total = value;
                OnPropertyChanged(nameof(Total));
            }
        }

        protected override string FindButtonInitialSearchFor
        {
            get
            {
                if (MaintenanceMode == DbMaintenanceModes.AddMode)
                    return string.Empty;

                return OrderDate.ToShortDateString();
            }
        }

        private OrderDetailsGridManager _detailsGridManager;

        public OrderDetailsGridManager DetailsGridManager
        {
            get => _detailsGridManager;
            set
            {
                if (_detailsGridManager == value)
                    return;

                _detailsGridManager = value;
                OnPropertyChanged(nameof(DetailsGridManager));
            }
        }

        public bool GridMode { get; set; }

        public bool SetInitialFocusToGrid { get; internal set; }

        public int GotoProductId { get; private set; }

        internal NorthwindViewModelInput ViewModelInput { get; private set; }

        private readonly DateTime _newDateTime = DateTime.Today;

        private INorthwindLookupContext _lookupContext;

        private bool _customerDirty;
        
        public OrderViewModel()
        {
            _orderDate = _newDateTime;
            _requiredDate = _shippedDate = null;
        }

        protected override void Initialize()
        {
            _lookupContext = RsDbLookupAppGlobals.EfProcessor.NorthwindLookupContext;

            OrderView = View as IOrderView ??
                             throw new ArgumentException(
                                 $"ViewModel requires an {nameof(IOrderView)} interface.");

            if (LookupAddViewArgs != null && LookupAddViewArgs.InputParameter is NorthwindViewModelInput viewModelInput)
            {
                ViewModelInput = viewModelInput;
            }
            else
            {
                ViewModelInput = new NorthwindViewModelInput();
            }
            ViewModelInput.OrderViewModels.Add(this);
            ViewModelInput.OrderInput ??= new OrderInput {GridMode = GridMode};

            CustomersAutoFillSetup = new AutoFillSetup(TableDefinition.GetFieldDefinition(p => p.CustomerID))
            {
                AddViewParameter = ViewModelInput,
                //AllowLookupAdd = false
            };
            EmployeeAutoFillSetup = new AutoFillSetup(TableDefinition.GetFieldDefinition(p => p.EmployeeID))
                {AddViewParameter = ViewModelInput};
            ShipViaAutoFillSetup = new AutoFillSetup(TableDefinition.GetFieldDefinition(p => p.ShipVia));

            OrderDetailsLookupDefinition =
                _lookupContext.NorthwindContextConfiguration.OrderDetailsFormLookup.Clone();

            DetailsGridManager = new OrderDetailsGridManager(this);

            base.Initialize();
        }

        protected override Order PopulatePrimaryKeyControls(Order newEntity, PrimaryKeyValue primaryKeyValue)
        {
            var order = RsDbLookupAppGlobals.EfProcessor.NorthwindEfDataProcessor.GetOrder(newEntity.OrderID, GridMode);
            OrderId = order.OrderID;

            _orderDetailsLookup.FilterDefinition.ClearFixedFilters();
            _orderDetailsLookup.FilterDefinition.AddFixedFilter(p => p.OrderID, Conditions.Equals, order.OrderID);

            //_orderDetailsLookup.TableFilterDefinition.Include(p => p.Product)
            //    .Include(p => p.Category)
            //    .AddFixedFilter(p => p.CategoryName, Conditions.Contains, "mea");
            OrderDetailsLookupCommand = GetLookupCommand(LookupCommands.Refresh, primaryKeyValue, ViewModelInput);

            ReadOnlyMode = ViewModelInput.OrderViewModels.Any(a => a != this && a.OrderId == OrderId);

            return order;
        }

        protected override void LoadFromEntity(Order entity)
        {
            Customer = new AutoFillValue(_lookupContext.Customers.GetPrimaryKeyValueFromEntity(entity.Customer),
                entity.CustomerID);

            if (entity.Customer != null)
                CompanyName = entity.Customer.CompanyName;

            var employeeName = string.Empty;
            if (entity.Employee != null)
                employeeName = GetEmployeeAutoFillValueText(entity.Employee);

            Employee = new AutoFillValue(_lookupContext.Employees.GetPrimaryKeyValueFromEntity(entity.Employee),
                employeeName);

            RequiredDate = entity.RequiredDate;
            if (entity.OrderDate == null)
                OrderDate = _newDateTime;
            else
            {
                OrderDate = (DateTime)entity.OrderDate;
            }

            ShippedDate = entity.ShippedDate;

            var shipCompanyName = string.Empty;
            if (entity.Shipper != null)
                shipCompanyName = entity.Shipper.CompanyName;

            ShipVia = new AutoFillValue(_lookupContext.Shippers.GetPrimaryKeyValueFromEntity(entity.Shipper),
                shipCompanyName);

            Freight = entity.Freight;
            ShipName = entity.ShipName;
            Address = entity.ShipAddress;
            City = entity.ShipCity;
            Region = entity.ShipRegion;
            PostalCode = entity.ShipPostalCode;
            Country = entity.ShipCountry;

            DetailsGridManager.LoadGrid(entity.Order_Details);

            RefreshTotalControls();
            _customerDirty = false;

            if (ReadOnlyMode)
                ControlsGlobals.UserInterface.ShowMessageBox(
                    "This Order is being modified in another window.  Editing not allowed.", "Editing not allowed",
                    RsMessageBoxIcons.Exclamation);
        }

        public void RefreshTotalControls()
        {
            decimal subTotal = 0;
            decimal totalDiscount = 0;
            decimal freight = 0;
            if (Freight != null)
                freight = (decimal) Freight;

            if (GridMode)
            {
                var productsRows = DetailsGridManager.Rows.OfType<OrderDetailsGridRow>();
                foreach (var productRow in productsRows)
                {
                    subTotal += productRow.ExtendedPrice;
                    totalDiscount += productRow.Discount;
                }
            }
            else
            {
                var orderDetails = RsDbLookupAppGlobals.EfProcessor.NorthwindEfDataProcessor.GetOrderDetails(OrderId);

                foreach (var orderDetail in orderDetails)
                {
                    var extendedPrice = orderDetail.Quantity * orderDetail.UnitPrice;
                    subTotal += extendedPrice;
                    var decimalDiscount = (decimal) orderDetail.Discount;
                    totalDiscount += decimalDiscount;
                }
            }

            SubTotal = subTotal;
            TotalDiscount = totalDiscount;
            Total = (SubTotal - TotalDiscount) + freight;
        }
        
        private string GetEmployeeAutoFillValueText(Employee employee)
        {
            return $"{employee.FirstName} {employee.LastName}";
        }

        protected override bool ValidateEntity(Order entity)
        {
            var result = base.ValidateEntity(entity);

            if (result)
                result = DetailsGridManager.ValidateGrid();

            return result;
        }

        protected override Order GetEntityData()
        {
            var order = new Order();
            order.OrderID = OrderId;

            if (Customer.IsValid())
            {
                var customer = _lookupContext.Customers.GetEntityFromPrimaryKeyValue(Customer.PrimaryKeyValue);
                order.CustomerID = customer.CustomerID;
            }

            if (Employee.IsValid())
            {
                var employee = _lookupContext.Employees.GetEntityFromPrimaryKeyValue(Employee.PrimaryKeyValue);
                order.EmployeeID = employee.EmployeeID;
            }

            if (ShipVia.IsValid())
            {
                var shipVia = _lookupContext.Shippers.GetEntityFromPrimaryKeyValue(ShipVia.PrimaryKeyValue);
                order.ShipVia = shipVia.ShipperID;
            }

            order.OrderDate = OrderDate;
            order.RequiredDate = RequiredDate;
            order.ShippedDate = ShippedDate;
            order.Freight = Freight;
            order.ShipName = ShipName;
            order.ShipAddress = Address;
            order.ShipCity = City;
            order.ShipRegion = Region;
            order.ShipPostalCode = PostalCode;
            order.ShipCountry = Country;

            return order;
        }

        protected override void ClearData()
        {
            OrderId = 0;
            Customer = Employee = ShipVia = null;
            OrderDate = _newDateTime;
            RequiredDate = ShippedDate = null;
            CompanyName = string.Empty;
            SubTotal = TotalDiscount = Total = 0;
            Freight = 0;
            ShipName = Address = City = Region = PostalCode = Country = null;
            OrderDetailsLookupCommand = GetLookupCommand(LookupCommands.Clear);

            if (LookupAddViewArgs != null && LookupAddViewArgs.ParentWindowPrimaryKeyValue != null)
            {
                var table = LookupAddViewArgs.ParentWindowPrimaryKeyValue.TableDefinition;
                if (table == _lookupContext.Customers)
                {
                    var customer =
                        _lookupContext.Customers.GetEntityFromPrimaryKeyValue(LookupAddViewArgs
                            .ParentWindowPrimaryKeyValue);
                    customer =
                        RsDbLookupAppGlobals.EfProcessor.NorthwindEfDataProcessor.GetCustomer(customer.CustomerID);
                    Customer = new AutoFillValue(LookupAddViewArgs.ParentWindowPrimaryKeyValue, customer.CustomerID);
                    CompanyName = customer.CompanyName;
                }
                else if (table == _lookupContext.Employees)
                {
                    if (_lookupContext.Employees != null)
                    {
                        var employee =
                            _lookupContext.Employees.GetEntityFromPrimaryKeyValue(LookupAddViewArgs
                                .ParentWindowPrimaryKeyValue);
                        employee =
                            RsDbLookupAppGlobals.EfProcessor.NorthwindEfDataProcessor.GetEmployee(employee.EmployeeID);
                        Employee = new AutoFillValue(LookupAddViewArgs.ParentWindowPrimaryKeyValue,
                            GetEmployeeAutoFillValueText(employee));
                    }
                }
            }

            _customerDirty = false;
            DetailsGridManager.SetupForNewRecord();
        }

        protected override AutoFillValue GetAutoFillValueForNullableForeignKeyField(FieldDefinition fieldDefinition)
        {
            if (fieldDefinition == TableDefinition.GetFieldDefinition(p => p.CustomerID))
                return Customer;

            if (fieldDefinition == TableDefinition.GetFieldDefinition(p => p.EmployeeID))
                return Employee;

            if (fieldDefinition == TableDefinition.GetFieldDefinition(p => p.ShipVia))
                return ShipVia;

            return base.GetAutoFillValueForNullableForeignKeyField(fieldDefinition);
        }

        protected override bool SaveEntity(Order entity)
        {
            var orderDetails = DetailsGridManager.GetEntityList();
            return RsDbLookupAppGlobals.EfProcessor.NorthwindEfDataProcessor.SaveOrder(entity, orderDetails);
        }

        protected override bool DeleteEntity()
        {
            return RsDbLookupAppGlobals.EfProcessor.NorthwindEfDataProcessor.DeleteOrder(OrderId);
        }

        public void OnCustomerIdLostFocus()
        {
            if (_customerDirty)
            {
                if (Customer?.PrimaryKeyValue == null || !Customer.PrimaryKeyValue.IsValid)
                {
                    CompanyName = string.Empty;
                }
                else
                {
                    var customer = _lookupContext.Customers.GetEntityFromPrimaryKeyValue(Customer.PrimaryKeyValue);
                    customer =
                        RsDbLookupAppGlobals.EfProcessor.NorthwindEfDataProcessor.GetCustomer(customer.CustomerID);

                    if (customer != null)
                    {
                        CompanyName = ShipName = customer.CompanyName;
                        Address = customer.Address;
                        City = customer.City;
                        Region = customer.Region;
                        PostalCode = customer.PostalCode;
                        Country = customer.Country;
                    }
                }
                _customerDirty = false;
            }
        }

        public void OnAddModify()
        {
            if (ExecuteAddModifyCommand() == DbMaintenanceResults.Success)
                OrderDetailsLookupCommand = GetLookupCommand(LookupCommands.AddModify);
        }

        public bool ValidateCustomer()
        {
            if (Customer != null && !string.IsNullOrEmpty(Customer.Text) &&
                !Customer.PrimaryKeyValue.IsValid)
            {
                var message = "Invalid Customer!";
                ControlsGlobals.UserInterface.ShowMessageBox(message, "Validation Fail", RsMessageBoxIcons.Exclamation);
                return false;
            }

            return true;
        }

        protected override TableFilterDefinitionBase GetAddViewFilter()
        {
            _lookupContext ??= RsDbLookupAppGlobals.EfProcessor.NorthwindLookupContext;

            if (LookupAddViewArgs.LookupData.LookupDefinition.TableDefinition == _lookupContext.OrderDetails)
            {
                SetInitialFocusToGrid = true;
                var orderDetail =
                    _lookupContext.OrderDetails.GetEntityFromPrimaryKeyValue(LookupAddViewArgs.LookupData
                        .SelectedPrimaryKeyValue);

                GotoProductId = orderDetail.ProductID;

                var sqlStringBuilder = new StringBuilder();
                var sqlGen = _lookupContext.DataProcessor.SqlGenerator;

                sqlStringBuilder.AppendLine(
                    $"{sqlGen.FormatSqlObject(_lookupContext.Orders.TableName)}.{sqlGen.FormatSqlObject(_lookupContext.Orders.GetFieldDefinition(p => p.OrderID).FieldName)} IN");

                sqlStringBuilder.AppendLine("(");

                var query = new SelectQuery(_lookupContext.OrderDetails.TableName);
                query.AddSelectColumn(_lookupContext.OrderDetails.GetFieldDefinition(p => p.OrderID).FieldName);
                query.AddWhereItem(_lookupContext.OrderDetails.GetFieldDefinition(p => p.ProductID).FieldName,
                    Conditions.Equals, GotoProductId);
                sqlStringBuilder.AppendLine(_lookupContext.DataProcessor.SqlGenerator.GenerateSelectStatement(query));

                sqlStringBuilder.AppendLine(")");

                var tableFilterDefinition = new TableFilterDefinition<Order>(TableDefinition);
                var sql = sqlStringBuilder.ToString();
                tableFilterDefinition.AddFixedFilter(sql);
                
                return tableFilterDefinition;
            }

            return base.GetAddViewFilter();
        }

        protected override PrimaryKeyValue GetAddViewPrimaryKeyValue(PrimaryKeyValue addViewPrimaryKeyValue)
        {
            if (addViewPrimaryKeyValue.TableDefinition ==
                RsDbLookupAppGlobals.EfProcessor.NorthwindLookupContext.OrderDetails)
            {
                var orderDetail =
                    _lookupContext.OrderDetails.GetEntityFromPrimaryKeyValue(addViewPrimaryKeyValue);
                
                var order = new Order{OrderID = orderDetail.OrderID};
                return TableDefinition.GetPrimaryKeyValueFromEntity(order);
            }

            return base.GetAddViewPrimaryKeyValue(addViewPrimaryKeyValue);
        }

        public override void OnWindowClosing(CancelEventArgs e)
        {
            base.OnWindowClosing(e);
            if (!e.Cancel)
                ViewModelInput.OrderViewModels.Remove(this);
        }
    }
}
