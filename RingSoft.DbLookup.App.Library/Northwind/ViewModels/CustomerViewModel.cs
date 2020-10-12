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
        public override TableDefinition<Customer> TableDefinition =>
            RsDbLookupAppGlobals.EfProcessor.NorthwindLookupContext.Customers;

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

        private INorthwindLookupContext _lookupContext;

        protected override void Initialize()
        {
            _lookupContext = RsDbLookupAppGlobals.EfProcessor.NorthwindLookupContext;

            var ordersLookup = new LookupDefinition<OrderLookup, Order>(_lookupContext.Orders);
            ordersLookup.AddVisibleColumnDefinition(p => p.OrderDate, p => p.OrderDate);
            ordersLookup.Include(p => p.Employee);
            ordersLookup.AddVisibleColumnDefinition(p => p.Employee, 
                _lookupContext.NorthwindContextConfiguration.GetOrdersEmployeeNameFormula());

            OrdersLookupDefinition = ordersLookup;

            base.Initialize();
        }

        protected override Customer PopulatePrimaryKeyControls(Customer newEntity, PrimaryKeyValue primaryKeyValue)
        {
            var customer = RsDbLookupAppGlobals.EfProcessor.NorthwindEfDataProcessor.GetCustomer(newEntity.CustomerID);
            KeyAutoFillValue = new AutoFillValue(primaryKeyValue, customer.CustomerID);

            OrdersLookupCommand = GetLookupCommand(LookupCommands.Refresh);
            return customer;
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

            _ordersLookup.FilterDefinition.ClearFixedFilters();
            _ordersLookup.FilterDefinition.AddFixedFilter(p => p.CustomerID, Conditions.Equals,
                entity.CustomerID);
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
            CompanyName = string.Empty;
            Address = City = ContactName = ContactTitle = Country = Fax = Phone = PostalCode = Region = null;
            OrdersLookupCommand = GetLookupCommand(LookupCommands.Clear);
        }

        protected override bool SaveEntity(Customer entity)
        {
            return RsDbLookupAppGlobals.EfProcessor.NorthwindEfDataProcessor.SaveCustomer(entity);
        }

        protected override bool DeleteEntity()
        {
            var customer = RsDbLookupAppGlobals.EfProcessor.NorthwindEfDataProcessor.GetCustomer(KeyAutoFillValue.Text);
            if (customer == null)
            {
                var message = $"Customer ID {KeyAutoFillValue.Text} was not found";
                var fieldDefinition =
                    RsDbLookupAppGlobals.EfProcessor.NorthwindLookupContext.Customers.GetFieldDefinition(p =>
                        p.CustomerID);

                View.OnValidationFail(fieldDefinition, message, "Delete Validation Fail");
                return false;
            }
            return RsDbLookupAppGlobals.EfProcessor.NorthwindEfDataProcessor.DeleteCustomer(KeyAutoFillValue.Text);
        }

        public void OnAddModify()
        {
            if (ExecuteAddModifyCommand() == DbMaintenanceResults.Success)
                OrdersLookupCommand = GetLookupCommand(LookupCommands.AddModify);
        }
    }
}
