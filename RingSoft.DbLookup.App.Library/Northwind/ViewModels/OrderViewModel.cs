using System;
using RingSoft.DbLookup.App.Library.Northwind.LookupModel;
using RingSoft.DbLookup.App.Library.Northwind.Model;
using RingSoft.DbLookup.AutoFill;
using RingSoft.DbLookup.Lookup;
using RingSoft.DbLookup.ModelDefinition;
using RingSoft.DbLookup.ModelDefinition.FieldDefinitions;
using RingSoft.DbLookup.QueryBuilder;
using RingSoft.DbMaintenance;

namespace RingSoft.DbLookup.App.Library.Northwind.ViewModels
{
    public class OrderViewModel : DbMaintenanceViewModel<Order>
    {
        public override TableDefinition<Order> TableDefinition =>
            RsDbLookupAppGlobals.EfProcessor.NorthwindLookupContext.Orders;

        private int _orderId;
        public int OrderId
        {
            get => _orderId;
            set
            {
                if (value == _orderId) return;
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

        private DateTime _requiredDate;
        public DateTime RequiredDate
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

        private DateTime _shippedDate;

        public DateTime ShippedDate
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
                if (OrderId == 0)
                    return string.Empty;
                return OrderId.ToString();
            }
        }

        private readonly DateTime _newDateTime = DateTime.Today;

        private INorthwindLookupContext _lookupContext;

        private bool _customerDirty;

        public OrderViewModel()
        {
            _orderDate = _requiredDate = _shippedDate = _newDateTime;
        }

        protected override void Initialize()
        {
            _lookupContext = RsDbLookupAppGlobals.EfProcessor.NorthwindLookupContext;

            CustomersAutoFillSetup = new AutoFillSetup(TableDefinition.GetFieldDefinition(p => p.CustomerID));
            EmployeeAutoFillSetup = new AutoFillSetup(TableDefinition.GetFieldDefinition(p => p.EmployeeID));
            ShipViaAutoFillSetup = new AutoFillSetup(TableDefinition.GetFieldDefinition(p => p.ShipVia));

            OrderDetailsLookupDefinition =
                _lookupContext.NorthwindContextConfiguration.OrderDetailsFormLookup.Clone();

            base.Initialize();
        }

        protected override void LoadFromEntity(Order newEntity)
        {
            var order = RsDbLookupAppGlobals.EfProcessor.NorthwindEfDataProcessor.GetOrder(newEntity.OrderID);
            OrderId = order.OrderID;
            Customer = new AutoFillValue(_lookupContext.Customers.GetPrimaryKeyValueFromEntity(order.Customer),
                order.CustomerID);

            if (order.Customer != null)
                CompanyName = order.Customer.CompanyName;

            var employeeName = string.Empty;
            if (order.Employee != null)
                employeeName = GetEmployeeAutoFillValueText(order.Employee);

            Employee = new AutoFillValue(_lookupContext.Employees.GetPrimaryKeyValueFromEntity(order.Employee),
                employeeName);

            if (order.RequiredDate == null)
                RequiredDate = _newDateTime;
            else
            {
                RequiredDate = (DateTime)order.RequiredDate;
            }

            if (order.OrderDate == null)
                OrderDate = _newDateTime;
            else
            {
                OrderDate = (DateTime)order.OrderDate;
            }

            if (order.ShippedDate == null)
                ShippedDate = _newDateTime;
            else
            {
                ShippedDate = (DateTime)order.ShippedDate;
            }

            var shipCompanyName = string.Empty;
            if (order.Shipper != null)
                shipCompanyName = order.Shipper.CompanyName;

            ShipVia = new AutoFillValue(_lookupContext.Shippers.GetPrimaryKeyValueFromEntity(order.Shipper),
                shipCompanyName);

            Freight = order.Freight;
            ShipName = order.ShipName;
            Address = order.ShipAddress;
            City = order.ShipCity;
            Region = order.ShipRegion;
            PostalCode = order.ShipPostalCode;
            Country = order.ShipCountry;

            _orderDetailsLookup.FilterDefinition.ClearFixedFilters();
            _orderDetailsLookup.FilterDefinition.AddFixedFilter(p => p.OrderID, Conditions.Equals, order.OrderID);

            //_orderDetailsLookup.TableFilterDefinition.Include(p => p.Product)
            //    .Include(p => p.Category)
            //    .AddFixedFilter(p => p.CategoryName, Conditions.Contains, "mea");
            OrderDetailsLookupCommand = GetLookupCommand(LookupCommands.Refresh,
                _lookupContext.Orders.GetPrimaryKeyValueFromEntity(order));

            RefreshTotalControls();
            _customerDirty = false;
        }

        private void RefreshTotalControls()
        {
            decimal subTotal = 0;
            decimal totalDiscount = 0;
            decimal freight = 0;
            if (Freight != null)
                freight = (decimal) Freight;

            var orderDetails = RsDbLookupAppGlobals.EfProcessor.NorthwindEfDataProcessor.GetOrderDetails(OrderId);

            foreach (var orderDetail in orderDetails)
            {
                var extendedPrice = orderDetail.Quantity * orderDetail.UnitPrice;
                subTotal += extendedPrice;
                var decimalDiscount = (decimal) orderDetail.Discount;
                totalDiscount += decimalDiscount;
            }

            SubTotal = subTotal;
            TotalDiscount = totalDiscount;
            Total = (SubTotal - TotalDiscount) + freight;
        }
        
        private string GetEmployeeAutoFillValueText(Employee employee)
        {
            return $"{employee.FirstName} {employee.LastName}";
        }

        protected override Order GetEntityData()
        {
            var order = new Order();
            order.OrderID = OrderId;

            if (Customer != null && Customer.PrimaryKeyValue.ContainsValidData())
            {
                var customer = _lookupContext.Customers.GetEntityFromPrimaryKeyValue(Customer.PrimaryKeyValue);
                order.CustomerID = customer.CustomerID;
            }

            if (Employee != null && Employee.PrimaryKeyValue.ContainsValidData())
            {
                var employee = _lookupContext.Employees.GetEntityFromPrimaryKeyValue(Employee.PrimaryKeyValue);
                order.EmployeeID = employee.EmployeeID;
            }

            if (ShipVia != null && ShipVia.PrimaryKeyValue.ContainsValidData())
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
            OrderDate = RequiredDate = ShippedDate = _newDateTime;
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
            return RsDbLookupAppGlobals.EfProcessor.NorthwindEfDataProcessor.SaveOrder(entity);
        }

        protected override bool DeleteEntity()
        {
            return RsDbLookupAppGlobals.EfProcessor.NorthwindEfDataProcessor.DeleteOrder(OrderId);
        }

        public void OnCustomerIdLostFocus()
        {
            if (_customerDirty)
            {
                if (Customer?.PrimaryKeyValue == null || !Customer.PrimaryKeyValue.ContainsValidData())
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
    }
}
