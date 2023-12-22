using RingSoft.DataEntryControls.Engine;
using RingSoft.DbLookup.App.Library.Northwind.LookupModel;
using RingSoft.DbLookup.App.Library.Northwind.Model;
using RingSoft.DbLookup.AutoFill;
using RingSoft.DbLookup.Lookup;
using RingSoft.DbMaintenance;
using System;
using System.ComponentModel;
using System.Linq;

namespace RingSoft.DbLookup.App.Library.Northwind.ViewModels
{
    public class EmployeeViewModel : DbMaintenanceViewModel<Employee>
    {
        #region Properties

        private int _employeeId;
        public int EmployeeId
        {
            get => _employeeId;
            set
            {
                if (_employeeId == value)
                    return;
                _employeeId = value;
                OnPropertyChanged(nameof(EmployeeId));
            }
        }

        private string _firstName;
        public string FirstName
        {
            get => _firstName;
            set
            {
                if (_firstName == value)
                    return;
                _firstName = value;
                OnPropertyChanged(nameof(FirstName));
            }
        }

        private string _lastName;
        public string LastName
        {
            get => _lastName;
            set
            {
                if (_lastName == value)
                    return;
                _lastName = value;
                OnPropertyChanged(nameof(LastName));
            }
        }

        [CanBeNull] private string _title;
        [CanBeNull]
        public string Title
        {
            get => _title;
            set
            {
                if (_title == value)
                    return;
                _title = value;
                OnPropertyChanged(nameof(Title));
            }
        }

        [CanBeNull] private string _titleOfCourtesy;

        public string TitleOfCourtesy
        {
            get => _titleOfCourtesy;
            set
            {
                if (_titleOfCourtesy == value)
                    return;

                _titleOfCourtesy = value;
                OnPropertyChanged(nameof(TitleOfCourtesy));
            }
        }

        private DateTime? _birthDate;
        public DateTime? BirthDate
        {
            get => _birthDate;
            set
            {
                if (_birthDate == value)
                    return;

                _birthDate = value;
                OnPropertyChanged(nameof(BirthDate));
            }
        }

        private DateTime? _hireDate;

        public DateTime? HireDate
        {
            get => _hireDate;
            set
            {
                if (_hireDate == value)
                    return;

                _hireDate = value;
                OnPropertyChanged(nameof(HireDate));
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

        [CanBeNull] private string _homePhone;

        [CanBeNull]
        public string HomePhone
        {
            get => _homePhone;
            set
            {
                if (_homePhone == value)
                    return;

                _homePhone = value;
                OnPropertyChanged(nameof(HomePhone));
            }
        }

        [CanBeNull] private string _extension;

        [CanBeNull]
        public string Extension
        {
            get => _extension;
            set
            {
                if (_extension == value)
                    return;

                _extension = value;
                OnPropertyChanged(nameof(Extension));
            }
        }

        [CanBeNull] private string _notes;

        [CanBeNull]
        public string Notes
        {
            get => _notes;
            set
            {
                if (_notes == value)
                    return;

                _notes = value;
                OnPropertyChanged(nameof(Notes));
            }
        }

        private AutoFillSetup _reportsToAutoFillSetup;
        public AutoFillSetup ReportsToAutoFillSetup
        {
            get => _reportsToAutoFillSetup;
            set
            {
                if (_reportsToAutoFillSetup == value)
                    return;

                _reportsToAutoFillSetup = value;
                OnPropertyChanged(nameof(ReportsToAutoFillSetup));
            }
        }

        private AutoFillValue _reportsTo;
        public AutoFillValue ReportsTo
        {
            get => _reportsTo;
            set
            {
                if (_reportsTo == value)
                    return;

                _reportsTo = value;
                OnPropertyChanged(nameof(ReportsTo));
            }
        }

        [CanBeNull] private string _photoPath;
        [CanBeNull]
        public string PhotoPath
        {
            get => _photoPath;
            set
            {
                if (_photoPath == value)
                    return;

                _photoPath = value;
                OnPropertyChanged(nameof(PhotoPath));
            }
        }

        protected override string FindButtonInitialSearchFor => $"{_firstName} {_lastName}";

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

        #endregion

        internal NorthwindViewModelInput ViewModelInput { get; private set; }

        public RelayCommand AddModifyCommand { get; }

        public UiCommand FirstNameUiCommand { get; } = new UiCommand();

        public UiCommand LastNameUiCommand { get; } = new UiCommand();

        private INorthwindLookupContext _lookupContext;

        public EmployeeViewModel()
        {
            AddModifyCommand = new RelayCommand(OnAddModify);
            MapFieldToUiCommand(FirstNameUiCommand
            , TableDefinition.GetFieldDefinition(p => p.FirstName));

            MapFieldToUiCommand(LastNameUiCommand
                , TableDefinition.GetFieldDefinition(p => p.LastName));
        }

        protected override void Initialize()
        {
            _lookupContext = RsDbLookupAppGlobals.EfProcessor.NorthwindLookupContext;

            if (LookupAddViewArgs != null && LookupAddViewArgs.InputParameter is NorthwindViewModelInput viewModelInput)
            {
                ViewModelInput = viewModelInput;
            }
            else
            {
                ViewModelInput = new NorthwindViewModelInput();
            }

            ViewModelInput.EmployeeViewModels.Add(this);
            ReportsToAutoFillSetup = new AutoFillSetup(_lookupContext.Employees.GetFieldDefinition(p => p.SupervisorId))
            {
                AddViewParameter = ViewModelInput
            };

            var ordersLookup = new LookupDefinition<OrderLookup, Order>(_lookupContext.Orders);
            ordersLookup.AddVisibleColumnDefinition(p => p.Order
                , p => p.OrderName);
            ordersLookup.Include(p => p.Customer)
                .AddVisibleColumnDefinition(p => p.Customer
                    , p => p.CompanyName);

            OrdersLookupDefinition = ordersLookup;
            RegisterLookup(OrdersLookupDefinition, ViewModelInput);

            base.Initialize();
        }

        public override void OnNewButton()
        {
            base.OnNewButton();
            FirstNameUiCommand.SetFocus();
        }

        protected override void PopulatePrimaryKeyControls(Employee newEntity, PrimaryKeyValue primaryKeyValue)
        {
            EmployeeId = newEntity.EmployeeID;

            ReadOnlyMode = ViewModelInput.EmployeeViewModels.Any(a => a != this && a.EmployeeId == EmployeeId);
        }

        protected override void LoadFromEntity(Employee entity)
        {
            FirstName = entity.FirstName;
            LastName = entity.LastName;
            Title = entity.Title;
            TitleOfCourtesy = entity.TitleOfCourtesy;
            BirthDate = entity.BirthDate;
            HireDate = entity.HireDate;
            Address = entity.Address;
            City = entity.City;
            Region = entity.Region;
            PostalCode = entity.PostalCode;
            Country = entity.Country;
            HomePhone = entity.HomePhone;
            Extension = entity.Extension;
            Notes = entity.Notes;
            PhotoPath = entity.PhotoPath;
            ReportsTo = entity.Supervisor.GetAutoFillValue();

            if (ReadOnlyMode)
                ControlsGlobals.UserInterface.ShowMessageBox(
                    "This Employee is being modified in another window.  Editing not allowed.", "Editing not allowed",
                    RsMessageBoxIcons.Exclamation);
        }

        protected override Employee GetEntityData()
        {
            var employee = new Employee
            {
                EmployeeID = EmployeeId,
                FirstName = FirstName,
                LastName = LastName,
                FullName = $"{FirstName} {LastName}",
                Title = Title,
                TitleOfCourtesy = TitleOfCourtesy,
                Address = Address,
                City = City,
                Region = Region,
                PostalCode = PostalCode,
                Country = Country,
                HomePhone = HomePhone,
                Extension = Extension,
                Notes = Notes,
                PhotoPath = PhotoPath,
                BirthDate = BirthDate,
                HireDate = HireDate,
                SupervisorId = ReportsTo.GetEntity<Employee>().EmployeeID
            };

            return employee;
        }

        protected override void ClearData()
        {
            EmployeeId = 0;
            FirstName = LastName = string.Empty;
            Title = null;
            TitleOfCourtesy = Address = City = Region = PostalCode = Country = HomePhone = Extension = Notes= PhotoPath = null;
            BirthDate = HireDate = null;
            ReportsTo = null;
        }

        private void OnAddModify()
        {
            if (ExecuteAddModifyCommand() == DbMaintenanceResults.Success)
            {
                OrdersLookupDefinition.SetCommand(GetLookupCommand(LookupCommands.AddModify));
            }
        }

        public override void OnWindowClosing(CancelEventArgs e)
        {
            base.OnWindowClosing(e);
            if (!e.Cancel)
                ViewModelInput.EmployeeViewModels.Remove(this);
        }
    }
}
