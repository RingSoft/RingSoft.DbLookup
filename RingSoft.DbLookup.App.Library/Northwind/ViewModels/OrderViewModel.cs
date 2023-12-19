using RingSoft.DataEntryControls.Engine;
using RingSoft.DbLookup.App.Library.Northwind.LookupModel;
using RingSoft.DbLookup.App.Library.Northwind.Model;
using RingSoft.DbLookup.AutoFill;
using RingSoft.DbLookup.Lookup;
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
        void ShowAdvancedFind();
    }
    public class OrderInput
    {
        public bool GridMode { get; set; }

        public int ProductId { get; set; }

        public bool FromProductOrders { get; set; }
    }
    public class OrderViewModel : DbMaintenanceViewModel<Order>
    {
        #region Properties

        
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

        private double? _freight;
        public double? Freight
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

        private LookupDataSourceChanged _orderDetailsDataSourceChanged;

        public LookupDataSourceChanged OrderDetailsDataSourceChanged
        {
            get => _orderDetailsDataSourceChanged;
            set
            {
                if (_orderDetailsDataSourceChanged == value)
                    return;

                _orderDetailsDataSourceChanged = value;
                OnPropertyChanged(nameof(OrderDetailsDataSourceChanged), false);
                RefreshTotalControls();
            }
        }

        private double _subTotal;

        public double SubTotal
        {
            get => _subTotal;
            set
            {
                if (_subTotal == value)
                    return;

                _subTotal = value;
                OnPropertyChanged(nameof(SubTotal), false);
            }
        }

        private double _totalDiscount;
        public double TotalDiscount
        {
            get => _totalDiscount;
            set
            {
                if (_totalDiscount == value)
                    return;

                _totalDiscount = value;
                OnPropertyChanged(nameof(TotalDiscount), false);
            }
        }

        private double _total;

        public double Total
        {
            get => _total;
            set
            {
                if (_total == value)
                    return;

                _total = value;
                OnPropertyChanged(nameof(Total), false);
            }
        }

        //protected override string FindButtonInitialSearchFor
        //{
        //    get
        //    {
        //        if (MaintenanceMode == DbMaintenanceModes.AddMode)
        //            return string.Empty;

        //        return OrderDate.ToShortDateString();
        //    }
        //}

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
        #endregion

        public bool GridMode { get; set; }

        public bool SetInitialFocusToGrid { get; internal set; }

        public int GotoProductId { get; private set; }

        public NorthwindViewModelInput ViewModelInput { get; private set; }

        public UiCommand CustomerUiCommand { get; } = new UiCommand();

        public UiCommand EmployeeUiCommand { get; } = new UiCommand();

        public UiCommand ShipViaUiCommand { get; } = new UiCommand();

        public RelayCommand AddModifyCommand { get; }

        public RelayCommand ShowAdvFindCommand { get; }

        public AutoFillValue DefaultCustomerAutoFillValue { get; private set; }

        public string DefaultCustomerName { get; private set; }

        public AutoFillValue DefaultEmployeeaAutoFillValue { get; private set; }

        public new IOrderView View { get; private set; }


        private readonly DateTime _newDateTime = DateTime.Today;
        private INorthwindLookupContext _lookupContext;
        private bool _customerDirty;
        
        public OrderViewModel()
        {
            _orderDate = _newDateTime;
            _requiredDate = _shippedDate = null;
            //TablesToDelete.Add(RsDbLookupAppGlobals.EfProcessor.NorthwindLookupContext.OrderDetails);

            CustomerUiCommand.LostFocus += CustomerUiCommand_LostFocus;

            AddModifyCommand = new RelayCommand((() =>
            {
                OnAddModify();
            }));

            ShowAdvFindCommand = new RelayCommand((() =>
            {
                if (View != null)
                {
                    View.ShowAdvancedFind();
                }
            }));
        }

        private async void CustomerUiCommand_LostFocus(object sender, UiLostFocusArgs e)
        {
            if (Customer != null && !Customer.Text.IsNullOrEmpty() && !Customer.PrimaryKeyValue.IsValid())
            {
                var message = $"Customer {Customer.Text} was not found.  Do you wish to add it?";
                var caption = "Invalid Customer";
                var mbResult = await ControlsGlobals.UserInterface.ShowYesNoMessageBox(message, caption);
                switch (mbResult)
                {
                    case MessageBoxButtonsResult.Yes:
                        var aofResult = RsDbLookupAppGlobals
                            .EfProcessor
                            .NorthwindLookupContext
                            .NorthwindContextConfiguration
                            .CustomerIdLookup
                            .ShowAddOnTheFlyWindow(Customer.Text);
                        if (aofResult.NewPrimaryKeyValue.IsValid())
                        {
                            var customer = RsDbLookupAppGlobals
                                .EfProcessor
                                .NorthwindLookupContext
                                .Customers
                                .GetEntityFromPrimaryKeyValue(aofResult.NewPrimaryKeyValue);
                            Customer = customer.FillOutProperties(false)
                                .GetAutoFillValue();
                        }
                        else
                        {
                            e.ContinueFocusChange = false;
                            return;
                        }
                        break;
                    case MessageBoxButtonsResult.No:
                        e.ContinueFocusChange = false;
                        return;
                }
            }
            OnCustomerIdLostFocus();
        }

        protected override void Initialize()
        {
            if (base.View is IOrderView orderView)
            {
                View = orderView;
            }
            _lookupContext = RsDbLookupAppGlobals.EfProcessor.NorthwindLookupContext;

            if (LookupAddViewArgs != null && LookupAddViewArgs.InputParameter is NorthwindViewModelInput viewModelInput)
            {
                ViewModelInput = viewModelInput;
            }
            else
            {
                ViewModelInput = new NorthwindViewModelInput();
            }

            if (LookupAddViewArgs != null && LookupAddViewArgs.ParentWindowPrimaryKeyValue != null)
            {
                var table = LookupAddViewArgs.ParentWindowPrimaryKeyValue.TableDefinition;
                if (table == _lookupContext.Customers)
                {
                    var customer =
                        _lookupContext.Customers.GetEntityFromPrimaryKeyValue(LookupAddViewArgs
                            .ParentWindowPrimaryKeyValue).FillOutProperties(false);
                    DefaultCustomerAutoFillValue = customer.GetAutoFillValue();
                    DefaultCustomerName = customer.CompanyName;
                }
                else if (table == _lookupContext.Employees)
                {
                    if (_lookupContext.Employees != null)
                    {
                        var employee =
                            _lookupContext.Employees
                                .GetEntityFromPrimaryKeyValue(LookupAddViewArgs
                                .ParentWindowPrimaryKeyValue);
                        DefaultEmployeeaAutoFillValue = employee.FillOutProperties(false).GetAutoFillValue();
                    }
                }
            }

            InputParameter = ViewModelInput;

            ViewModelInput.OrderViewModels.Add(this);
            ViewModelInput.OrderInput ??= new OrderInput {GridMode = GridMode};

            CustomersAutoFillSetup = new AutoFillSetup(TableDefinition.GetFieldDefinition(p => p.CustomerID))
            {
                AddViewParameter = ViewModelInput,
            };
            EmployeeAutoFillSetup = new AutoFillSetup(TableDefinition.GetFieldDefinition(p => p.EmployeeID))
                {AddViewParameter = ViewModelInput};

            ShipViaAutoFillSetup = new AutoFillSetup(TableDefinition.GetFieldDefinition(p => p.ShipVia));

            OrderDetailsLookupDefinition =
                _lookupContext.NorthwindContextConfiguration.OrderDetailsFormLookup.Clone();

            DetailsGridManager = new OrderDetailsGridManager(this);
            if (GridMode)
            {
                RegisterGrid(DetailsGridManager);
            }
            else
            {
                RegisterLookup(OrderDetailsLookupDefinition, ViewModelInput);
            }

            base.Initialize();
        }

        public override void OnNewButton()
        {
            base.OnNewButton();
            CustomerUiCommand.SetFocus();
        }

        protected override void PopulatePrimaryKeyControls(Order newEntity, PrimaryKeyValue primaryKeyValue)
        {
            OrderId = newEntity.OrderID;

            ReadOnlyMode = ViewModelInput.OrderViewModels.Any(a => a != this && a.OrderId == newEntity.OrderID);
        }

        protected override void LoadFromEntity(Order entity)
        {
            Customer = entity.Customer.GetAutoFillValue();
            CompanyName = entity.Customer.CompanyName;
            Employee = entity.Employee.GetAutoFillValue();
            RequiredDate = entity.RequiredDate;
            if (entity.OrderDate == null)
                OrderDate = _newDateTime;
            else
            {
                OrderDate = (DateTime)entity.OrderDate;
            }

            ShippedDate = entity.ShippedDate;
            ShipVia = entity.Shipper.GetAutoFillValue();
            Freight = entity.Freight;
            ShipName = entity.ShipName;
            Address = entity.ShipAddress;
            City = entity.ShipCity;
            Region = entity.ShipRegion;
            PostalCode = entity.ShipPostalCode;
            Country = entity.ShipCountry;

            RefreshTotalControls();
            _customerDirty = false;

            if (ReadOnlyMode)
            {
                ControlsGlobals.UserInterface.ShowMessageBox(
                    "This Order is being modified in another window.  Editing not allowed.", "Editing not allowed",
                    RsMessageBoxIcons.Exclamation);
                Processor?.Activate();
            }
        }

        public void RefreshTotalControls()
        {
            double subTotal = 0;
            double totalDiscount = 0;
            double freight = 0;
            if (Freight != null)
                freight = (double) Freight;

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
                var context = SystemGlobals.DataRepository.GetDataContext();
                var table = context.GetTable<Order_Detail>();
                var orderDetails = table
                    .Where(p => p.OrderID == OrderId);
                foreach (var orderDetail in orderDetails)
                {
                    var extendedPrice = orderDetail.Quantity * orderDetail.UnitPrice;
                    subTotal += extendedPrice;
                    var decimalDiscount = (double) orderDetail.Discount;
                    totalDiscount += decimalDiscount;
                }
            }

            SubTotal = subTotal;
            TotalDiscount = totalDiscount;
            Total = (SubTotal - TotalDiscount) + freight;
        }
        
        protected override Order GetEntityData()
        {
            var order = new Order
            {
                OrderID = OrderId,
                CustomerID = Customer.GetEntity<Customer>().CustomerID,
                EmployeeID = Employee.GetEntity<Employee>().EmployeeID,
                ShipVia = ShipVia.GetEntity<Shipper>().ShipperID,
                OrderDate = OrderDate,
                RequiredDate = RequiredDate,
                ShippedDate = ShippedDate,
                Freight = Freight.GetValueOrDefault(),
                ShipName = ShipName,
                ShipAddress = Address,
                ShipCity = City,
                ShipRegion = Region,
                ShipPostalCode = PostalCode,
                ShipCountry = Country
            };
            order.OrderName = $"{GblMethods.FormatDateValue(OrderDate, DbDateTypes.DateOnly)} {order.CustomerID}";
            return order;
        }

        protected override void ClearData()
        {
            OrderId = 0;
            Customer = DefaultCustomerAutoFillValue;
            Employee = DefaultEmployeeaAutoFillValue;
            ShipVia = null;
            OrderDate = _newDateTime;
            RequiredDate = ShippedDate = null;
            CompanyName = DefaultCustomerName;
            SubTotal = TotalDiscount = Total = 0;
            Freight = 0;
            ShipName = Address = City = Region = PostalCode = Country = null;

            _customerDirty = false;
        }

        private void OnCustomerIdLostFocus()
        {
            if (_customerDirty)
            {
                if (Customer?.PrimaryKeyValue == null || !Customer.PrimaryKeyValue.IsValid())
                {
                    CompanyName = string.Empty;
                }
                else
                {
                    var customer = _lookupContext.Customers.GetEntityFromPrimaryKeyValue(Customer.PrimaryKeyValue);
                    customer = customer.FillOutProperties(false);

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
            {
                OrderDetailsLookupDefinition.SetCommand(GetLookupCommand(LookupCommands.AddModify));
            }
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

        protected override void SetupPrinterArgs(PrinterSetupArgs printerSetupArgs, int stringFieldIndex = 1, 
            int numericFieldIndex = 1, int memoFieldIndex = 1)
        {
            if (MaintenanceMode == DbMaintenanceModes.EditMode)
            {
                var order = new Order()
                {
                    OrderID = OrderId
                };
                var orderSetup = new AutoFillSetup(FindButtonLookupDefinition);
                printerSetupArgs.CodeAutoFillValue = orderSetup.GetAutoFillValueForIdValue(OrderId);
            }
            base.SetupPrinterArgs(printerSetupArgs, stringFieldIndex, numericFieldIndex, memoFieldIndex);
        }
    }
}
