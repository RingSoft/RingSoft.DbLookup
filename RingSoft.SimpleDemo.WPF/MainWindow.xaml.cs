using RingSoft.DbLookup.AutoFill;
using RingSoft.DbLookup.Controls.WPF;
using RingSoft.DbLookup.Lookup;
using RingSoft.DbLookup.QueryBuilder;
using RingSoft.SimpleDemo.WPF.Northwind.LookupModel;
using RingSoft.SimpleDemo.WPF.Northwind.Model;
using RingSoft.SimpleDemo.WPF.Properties;
using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace RingSoft.SimpleDemo.WPF
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : INotifyPropertyChanged
    {
        private int _orderId;

        public int OrderId
        {
            get => _orderId;
            set
            {
                if (_orderId == value)
                    return;

                _orderId = value;
                OnPropertyChanged(nameof(OrderId));
            }
        }

        private AutoFillSetup _customerAutoFillSetup;

        public AutoFillSetup CustomerAutoFillSetup
        {
            get => _customerAutoFillSetup;
            set
            {
                if (_customerAutoFillSetup == value)
                    return;

                _customerAutoFillSetup = value;
                OnPropertyChanged();
            }
        }

        private AutoFillValue _customerAutoFillValue;

        public AutoFillValue CustomerAutoFillValue
        {
            get => _customerAutoFillValue;
            set
            {
                if (_customerAutoFillValue == value)
                    return;

                _customerAutoFillValue = value;
                OnPropertyChanged(nameof(CustomerAutoFillValue));
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
                OnPropertyChanged(nameof(EmployeeAutoFillSetup));
            }
        }

        private AutoFillValue _employeeAutoFillValue;

        public AutoFillValue EmployeeAutoFillValue
        {
            get => _employeeAutoFillValue;
            set
            {
                if (_employeeAutoFillValue == value)
                    return;

                _employeeAutoFillValue = value;
                OnPropertyChanged(nameof(EmployeeAutoFillValue));
            }
        }

        private LookupDefinition<OrderDetailLookup, Order_Detail> _orderDetailsLookupDefinition;

        public LookupDefinition<OrderDetailLookup, Order_Detail> OrderDetailsLookupDefinition
        {
            get => _orderDetailsLookupDefinition;
            set
            {
                if (_orderDetailsLookupDefinition == value)
                    return;

                _orderDetailsLookupDefinition = value;
                OnPropertyChanged(nameof(OrderDetailsLookupDefinition));
            }
        }

        private LookupCommand _orderDetailsCommand;

        public LookupCommand OrderDetailsCommand
        {
            get => _orderDetailsCommand;
            set
            {
                if (_orderDetailsCommand == value)
                    return;

                _orderDetailsCommand = value;
                OnPropertyChanged(nameof(OrderDetailsCommand));
            }
        }

        public MainWindow()
        {
            InitializeComponent();
            CustomerAutoFillSetup = new AutoFillSetup(App.LookupContext.Orders.GetFieldDefinition(p => p.CustomerID))
            {
                AllowLookupAdd =  false,
                AllowLookupView = false
            };
            EmployeeAutoFillSetup =
                new AutoFillSetup(App.LookupContext.Orders.GetFieldDefinition(p => p.EmployeeID))
                {
                    AllowLookupAdd = false,
                    AllowLookupView = false
                };

            CustomerControl.LostFocus += (sender, args) => UpdateCompanyName();
            CloseButton.Click += (sender, args) => Close();

            OrdersLookupButton.Click += OrdersLookupButton_Click;

            var extendedPriceFormula = "([Order Details].[Quantity] * 1.0) * [Order Details].[UnitPrice]";
            var orderDetailsLookupDefinition = new LookupDefinition<OrderDetailLookup, Order_Detail>(App.LookupContext.OrderDetails);
            orderDetailsLookupDefinition.Include(p => p.Product)
                .AddVisibleColumnDefinition(p => p.Product, p => p.ProductName);
            orderDetailsLookupDefinition.AddVisibleColumnDefinition(p => p.Quantity, p => p.Quantity);
            orderDetailsLookupDefinition.AddVisibleColumnDefinition(p => p.UnitPrice, p => p.UnitPrice);
            orderDetailsLookupDefinition.AddVisibleColumnDefinition(p => p.ExtendedPrice, extendedPriceFormula)
                .HasNumberFormatString("c").HasHorizontalAlignmentType(LookupColumnAlignmentTypes.Right);
            orderDetailsLookupDefinition.AddVisibleColumnDefinition(p => p.Discount, p => p.Discount);

            OrderDetailsLookupDefinition = orderDetailsLookupDefinition;
        }

        private void OrdersLookupButton_Click(object sender, System.Windows.RoutedEventArgs e)
        {
            var lookupWindow = new LookupWindow(App.LookupContext.OrdersLookup, false, false,
                OrderId > 0 ? OrderId.ToString() : string.Empty);
            lookupWindow.LookupSelect += (o, args) =>
            {
                var order = App.LookupContext.Orders.GetEntityFromPrimaryKeyValue(args.LookupData.SelectedPrimaryKeyValue);
                order = App.EfDataProcessor.GetOrder(order.OrderID);
                OrderId = order.OrderID;
                CustomerAutoFillValue =
                    new AutoFillValue(App.LookupContext.Customers.GetPrimaryKeyValueFromEntity(order.Customer),
                        order.CustomerID);
                CompanyName = order.Customer.CompanyName;

                var employeeText = $"{order.Employee.FirstName} {order.Employee.LastName}";
                EmployeeAutoFillValue =
                    new AutoFillValue(App.LookupContext.Employees.GetPrimaryKeyValueFromEntity(order.Employee),
                        employeeText);

                OrderDetailsLookupDefinition.FilterDefinition.ClearFixedFilters();
                OrderDetailsLookupDefinition.FilterDefinition.AddFixedFilter(p => p.OrderID, Conditions.Equals,
                    order.OrderID);
                OrderDetailsCommand = new LookupCommand(LookupCommands.Refresh);
            };
            
            lookupWindow.ShowDialog();
        }

        private void UpdateCompanyName()
        {
            if (CustomerAutoFillValue.PrimaryKeyValue.ContainsValidData())
            {
                var customer =
                    App.LookupContext.Customers.GetEntityFromPrimaryKeyValue(CustomerAutoFillValue.PrimaryKeyValue);
                customer = App.EfDataProcessor.GetCustomer(customer.CustomerID);
                CompanyName = customer.CompanyName;
            }
            else
            {
                CompanyName = string.Empty;
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;

        [NotifyPropertyChangedInvocator]
        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
