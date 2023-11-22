using System.ComponentModel;
using System.Linq;
using RingSoft.DataEntryControls.Engine;
using RingSoft.DbLookup.App.Library.Northwind.LookupModel;
using RingSoft.DbLookup.App.Library.Northwind.Model;
using RingSoft.DbLookup.AutoFill;
using RingSoft.DbLookup.Lookup;
using RingSoft.DbLookup.ModelDefinition;
using RingSoft.DbLookup.QueryBuilder;
using RingSoft.DbMaintenance;

namespace RingSoft.DbLookup.App.Library.Northwind.ViewModels
{
    public class CustomerViewModel : DbMaintenanceViewModel<Customer>
    {

        #region Properties

        public string  CustomerId { get; set; }

        private string _companyName;
        public string CompanyName
        {
            get => _companyName;
            set
            {
                if (_companyName == value) return;
                _companyName = value;
                OnPropertyChanged(nameof(CompanyName));
            }
        }

        [CanBeNull] private string _contactName;
        [CanBeNull]
        public string ContactName
        {
            get => _contactName;
            set
            {
                if (_contactName == value) return;
                _contactName = value;
                OnPropertyChanged(nameof(ContactName));
            }
        }

        [CanBeNull] private string _contactTitle;
        [CanBeNull]
        public string ContactTitle
        {
            get => _contactTitle;
            set
            {
                if (_contactTitle == value) return;
                _contactTitle = value;
                OnPropertyChanged(nameof(ContactTitle));
            }
        }

        private string _address;
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

        [CanBeNull] private string _phone;

        [CanBeNull]
        public string Phone
        {
            get => _phone;
            set
            {
                if (_phone == value)
                    return;

                _phone = value;
                OnPropertyChanged(nameof(Phone));
            }
        }

        [CanBeNull] private string _fax;

        [CanBeNull]
        public string Fax
        {
            get => _fax;
            set
            {
                if (_fax == value)
                    return;

                _fax = value;
                OnPropertyChanged(nameof(Fax));
            }
        }

        private LookupDefinition<OrderLookup, Order> _ordersLookup;

        public LookupDefinition<OrderLookup, Order> OrdersLookupDefinition
        {
            get => _ordersLookup;
            set
            {
                if (_ordersLookup == value)
                    return;

                _ordersLookup = value;
                OnPropertyChanged(nameof(OrdersLookupDefinition));
            }
        }

        private LookupCommand _ordersLookupCommand;

        public LookupCommand OrdersLookupCommand
        {
            get => _ordersLookupCommand;
            set
            {
                if (_ordersLookupCommand == value)
                    return;

                _ordersLookupCommand = value;
                OnPropertyChanged(nameof(OrdersLookupCommand), false);
            }
        }

        #endregion

        internal NorthwindViewModelInput ViewModelInput { get; private set; }

        public RelayCommand AddModifyCommand { get; private set; }

        public UiCommand CompanyNameUiCommand { get; } = new UiCommand();

        private INorthwindLookupContext _lookupContext;

        public CustomerViewModel()
        {
            AddModifyCommand = new RelayCommand(OnAddModify);
        }

        protected override void Initialize()
        {
            MapFieldToUiCommand(CompanyNameUiCommand
                , TableDefinition.GetFieldDefinition(p => p.CompanyName));

            _lookupContext = RsDbLookupAppGlobals.EfProcessor.NorthwindLookupContext;

            if (LookupAddViewArgs != null && LookupAddViewArgs.InputParameter is NorthwindViewModelInput viewModelInput)
            {
                ViewModelInput = viewModelInput;
            }
            else
            {
                ViewModelInput = new NorthwindViewModelInput();
            }
            ViewModelInput.CustomerViewModels.Add(this);

            var ordersLookup = new LookupDefinition<OrderLookup, Order>(_lookupContext.Orders);
            ordersLookup.AddVisibleColumnDefinition(p => p.Order
                , p => p.OrderName);
            var join = ordersLookup.Include(p => p.Employee);
            join.AddVisibleColumnDefinition(p => p.Employee, "Employee",
                p => p.FullName, 20);

            OrdersLookupDefinition = ordersLookup;

            base.Initialize();
        }

        protected override Customer PopulatePrimaryKeyControls(Customer newEntity, PrimaryKeyValue primaryKeyValue)
        {
            KeyAutoFillUiCommand.IsEnabled = false;
            CustomerId = newEntity.CustomerID;

            _ordersLookup.FilterDefinition.ClearFixedFilters();
            _ordersLookup.FilterDefinition.AddFixedFilter(p => p.CustomerID, Conditions.Equals,
                newEntity.CustomerID);

            ReadOnlyMode = ViewModelInput.CustomerViewModels.Any(a => a != this && a.CustomerId == CustomerId);
            OrdersLookupCommand = GetLookupCommand(LookupCommands.Refresh, primaryKeyValue, ViewModelInput);
            return base.PopulatePrimaryKeyControls(newEntity, primaryKeyValue);
        }

        protected override void LoadFromEntity(Customer entity)
        {
            CompanyName = entity.CompanyName;
            ContactName = entity.ContactName;
            ContactTitle = entity.ContactTitle;
            Address = entity.Address;
            City = entity.City;
            Region = entity.Region;
            PostalCode = entity.PostalCode;
            Country = entity.Country;
            Phone = entity.Phone;
            Fax = entity.Fax;

            if (ReadOnlyMode)
                ControlsGlobals.UserInterface.ShowMessageBox(
                    "This Customer is being modified in another window.  Editing not allowed.", "Editing not allowed",
                    RsMessageBoxIcons.Exclamation);
        }

        protected override Customer GetEntityData()
        {
            var customer = new Customer
            {
                CustomerID = KeyAutoFillValue.Text,
                Address = Address,
                City = City,
                CompanyName = CompanyName,
                ContactName = ContactName,
                ContactTitle = ContactTitle,
                Country = Country,
                Fax = Fax,
                Phone = Phone,
                PostalCode = PostalCode,
                Region = Region
            };
            return customer;
        }

        protected override void ClearData()
        {
            CustomerId = CompanyName = string.Empty;
            Address = City = ContactName = ContactTitle = Country = Fax = Phone = PostalCode = Region = null;
            OrdersLookupCommand = GetLookupCommand(LookupCommands.Clear);
        }

        protected override bool SaveEntity(Customer entity)
        {
            var context = SystemGlobals.DataRepository.GetDataContext();
            var table = context.GetTable<Customer>();
            var existCustomer = table
                .FirstOrDefault(p => p.CustomerID == entity.CustomerID);
            context = SystemGlobals.DataRepository.GetDataContext();
            if (existCustomer == null)
            {
                return context.AddSaveEntity(entity, "Saving Customer");
            }

            return context.SaveEntity(entity, "Saving Customer");
        }

        protected override bool DeleteEntity()
        {
            var context = SystemGlobals.DataRepository.GetDataContext();
            var table = context.GetTable<Customer>();
            var existCustomer = table
                .FirstOrDefault(p => p.CustomerID == CustomerId);
            context = SystemGlobals.DataRepository.GetDataContext();
            if (existCustomer != null)
            {
                return context.DeleteEntity(existCustomer, "Deleting Customer");
            }

            return true;
        }

        private void OnAddModify()
        {
            if (ExecuteAddModifyCommand() == DbMaintenanceResults.Success)
                OrdersLookupCommand = GetLookupCommand(LookupCommands.AddModify);
        }

        public override void OnWindowClosing(CancelEventArgs e)
        {
            base.OnWindowClosing(e);
            if (!e.Cancel)
                ViewModelInput.CustomerViewModels.Remove(this);
        }
    }
}
