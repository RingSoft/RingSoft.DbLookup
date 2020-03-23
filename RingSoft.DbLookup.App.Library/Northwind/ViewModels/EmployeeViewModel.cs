﻿using RSDbLookupApp.Library.Northwind.LookupModel;
using RSDbLookupApp.Library.Northwind.Model;
using System;
using RingSoft.DbLookup.AutoFill;
using RingSoft.DbLookup.Lookup;
using RingSoft.DbLookup.ModelDefinition;
using RingSoft.DbLookup.ModelDefinition.FieldDefinitions;
using RingSoft.DbLookup.QueryBuilder;
using RingSoft.DbMaintenance;

namespace RSDbLookupApp.Library.Northwind.ViewModels
{
    public class EmployeeViewModel : DbMaintenanceViewModel<Employee>
    {
        public override TableDefinition<Employee> TableDefinition =>
            RsDbLookupAppGlobals.EfProcessor.NorthwindLookupContext.Employees;

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
        public AutoFillSetup ReportsToAutoFillSetup => _reportsToAutoFillSetup;

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
        public LookupDefinitionBase OrdersLookupDefinition => _ordersLookup;

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

        private readonly DateTime _newDateTime = new DateTime(1980, 1, 1);
        private INorthwindLookupContext _lookupContext;

        protected override void Initialize()
        {
            _lookupContext = RsDbLookupAppGlobals.EfProcessor.NorthwindLookupContext;

            _reportsToAutoFillSetup = new AutoFillSetup(_lookupContext.Employees.GetFieldDefinition(p => p.ReportsTo));
            _ordersLookup = new LookupDefinition<OrderLookup, Order>(_lookupContext.Orders);
            _ordersLookup.AddVisibleColumnDefinition(p => p.OrderDate, "Date", p => p.OrderDate, 30);
            _ordersLookup.Include(p => p.Customer)
                .AddVisibleColumnDefinition(p => p.Customer, "Company Name", p => p.CompanyName, 70);

            base.Initialize();
        }

        protected override void LoadFromEntity(Employee newEntity)
        {
            var employee = RsDbLookupAppGlobals.EfProcessor.NorthwindEfDataProcessor.GetEmployee(newEntity.EmployeeID);

            EmployeeId = employee.EmployeeID;
            FirstName = employee.FirstName;
            LastName = employee.LastName;
            Title = employee.Title;
            _titleOfCourtesy = employee.TitleOfCourtesy;

            if (employee.BirthDate != null)
                BirthDate = employee.BirthDate;
            else
                BirthDate = _newDateTime;

            if (employee.HireDate != null)
                HireDate = employee.HireDate;
            else
                HireDate = _newDateTime;

            Address = employee.Address;
            City = employee.City;
            Region = employee.Region;
            PostalCode = employee.PostalCode;
            Country = employee.Country;
            HomePhone = employee.HomePhone;
            Extension = employee.Extension;
            Notes = employee.Notes;
            PhotoPath = employee.PhotoPath;

            var supervisorPrimaryKey = _lookupContext.Employees.GetPrimaryKeyValueFromEntity(employee.Employee1);
            if (employee.Employee1 != null)
            {
                ReportsTo = new AutoFillValue(supervisorPrimaryKey,
                    $"{employee.Employee1.FirstName} {employee.Employee1.LastName}");
            }
            else
            {
                ReportsTo = new AutoFillValue(supervisorPrimaryKey, string.Empty);
            }

            _ordersLookup.FilterDefinition.ClearFixedFilters();
            _ordersLookup.FilterDefinition.AddFixedFilter(p => p.EmployeeID, Conditions.Equals,
                employee.EmployeeID);
            OrdersLookupCommand = GetLookupCommand(LookupCommands.Refresh,
                _lookupContext.Employees.GetPrimaryKeyValueFromEntity(employee));
        }

        protected override Employee GetEntityData()
        {
            var employee = new Employee
            {
                EmployeeID = EmployeeId,
                FirstName = FirstName,
                LastName = LastName,
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
                HireDate = HireDate
            };

            if (ReportsTo != null && ReportsTo.PrimaryKeyValue.ContainsValidData())
            {
                var supervisor = _lookupContext.Employees.GetEntityFromPrimaryKeyValue(ReportsTo.PrimaryKeyValue);
                employee.ReportsTo = supervisor.EmployeeID;
            }

            return employee;
        }

        protected override AutoFillValue GetAutoFillValueForNullableForeignKeyField(FieldDefinition fieldDefinition)
        {
            if (fieldDefinition == TableDefinition.GetFieldDefinition(p => p.ReportsTo))
                return ReportsTo;

            return base.GetAutoFillValueForNullableForeignKeyField(fieldDefinition);
        }

        protected override void ClearData()
        {
            EmployeeId = 0;
            FirstName = LastName = string.Empty;
            Title = TitleOfCourtesy = Address = City = Region = PostalCode = Country = HomePhone = Extension = Notes= PhotoPath = null;
            BirthDate = HireDate = _newDateTime;
            ReportsTo = null;
            OrdersLookupCommand = GetLookupCommand(LookupCommands.Clear);
        }

        protected override bool SaveEntity(Employee entity)
        {
            return RsDbLookupAppGlobals.EfProcessor.NorthwindEfDataProcessor.SaveEmployee(entity);
        }

        protected override bool DeleteEntity()
        {
            return RsDbLookupAppGlobals.EfProcessor.NorthwindEfDataProcessor.DeleteEmployee(EmployeeId);
        }

        public void OnAddModify()
        {
            if (ExecuteAddModifyCommand() == DbMaintenanceResults.Success)
                OrdersLookupCommand = GetLookupCommand(LookupCommands.AddModify);
        }
    }
}
