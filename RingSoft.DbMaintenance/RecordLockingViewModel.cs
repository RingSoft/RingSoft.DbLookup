using System;
using System.Linq;
using RingSoft.DataEntryControls.Engine;
using RingSoft.DbLookup;
using RingSoft.DbLookup.AutoFill;
using RingSoft.DbLookup.ModelDefinition;
using RingSoft.DbLookup.RecordLocking;

namespace RingSoft.DbMaintenance
{
    public interface IRecordLockingView : IDbMaintenanceView
    {
        void SetupView();

        void CloseWindow();
    }

    public class RecordLockingInputParameter
    {
        public object InputParameter { get; set; }

        public string RecordLockMessage { get; set; }

        public bool ContinueSave { get; set; }
    }

    public class RecordLockingViewModel : DbMaintenanceViewModel<RecordLock>
    {
        public override TableDefinition<RecordLock> TableDefinition =>
            SystemGlobals.AdvancedFindLookupContext.RecordLocks;

        private string _message;

        public string Message
        {
            get => _message;
            set
            {
                if (_message == value)
                {
                    return;
                }
                _message = value;
                OnPropertyChanged();
            }
        }


        private DateTime _lockDate;

        public DateTime LockDate
        {
            get => _lockDate;
            set
            {
                if (_lockDate == value)
                {
                    return;
                }
                _lockDate = value;
                OnPropertyChanged();
            }
        }


        private string _primaryKeyDescription;

        public string PrimaryKeyDescription
        {
            get => _primaryKeyDescription;
            set
            {
                if (_primaryKeyDescription == value)
                {
                    return;
                }
                _primaryKeyDescription = value;
                OnPropertyChanged();
            }
        }


        private AutoFillSetup _primaryKeyAutoFillSetup;

        public AutoFillSetup PrimaryKeyAutoFillSetup
        {
            get => _primaryKeyAutoFillSetup;
            set
            {
                if (_primaryKeyAutoFillSetup == value)
                {
                    return;
                }
                _primaryKeyAutoFillSetup = value;
                OnPropertyChanged();
            }
        }


        private AutoFillValue  _primaryKeyAutoFillValue;

        public AutoFillValue PrimaryKeyAutoFillValue
        {
            get => _primaryKeyAutoFillValue;
            set
            {
                if (_primaryKeyAutoFillValue == value)
                {
                    return;
                }
                _primaryKeyAutoFillValue = value;
                OnPropertyChanged();
            }
        }

        private string _userName;

        public string UserName
        {
            get => _userName;
            set
            {
                if (_userName == value)
                {
                    return;
                }
                _userName = value;
                OnPropertyChanged();
            }
        }

        private AutoFillSetup _userAutoFillSetup;

        public AutoFillSetup UserAutoFillSetup
        {
            get => _userAutoFillSetup;
            set
            {
                if (_userAutoFillSetup == value)
                {
                    return;
                }
                _userAutoFillSetup = value;
                OnPropertyChanged();
            }
        }

        private AutoFillValue _userAutoFillValue;

        public AutoFillValue UserAutoFillValue
        {
            get => _userAutoFillValue;
            set
            {
                if (_userAutoFillValue == value)
                {
                    return;
                }
                _userAutoFillValue = value;
                OnPropertyChanged();
            }
        }

        public new IRecordLockingView View { get; set; }

        public RelayCommand ContinueSaveCommand { get; set; }

        public RelayCommand CancelSaveCommand { get; set; }

        public RecordLockingInputParameter RecordLockingParameter { get; set; }

        public RecordLockingViewModel()
        {
            ContinueSaveCommand = new RelayCommand(ContinueSave);

            CancelSaveCommand = new RelayCommand(CancelSave);
        }
        protected override void Initialize()
        {
            if (!(base.View is IRecordLockingView))
            {
                throw new Exception($"View must be of type {nameof(IRecordLockingView)}");
            }

            if (InputParameter is RecordLockingInputParameter recordLockingInput)
            {
                RecordLockingParameter = recordLockingInput;
                if (!RecordLockingParameter.RecordLockMessage.IsNullOrEmpty())
                {
                    PreviousCommand.IsEnabled = NextCommand.IsEnabled = false;
                }
            }
            else
            {
                RecordLockingParameter = new RecordLockingInputParameter();
            }

            Message = RecordLockingParameter.RecordLockMessage;
            Processor.SetWindowReadOnlyMode();
            base.Initialize();
        }

        protected override void PopulatePrimaryKeyControls(RecordLock newEntity, PrimaryKeyValue primaryKeyValue)
        {
            if (SystemGlobals.AdvancedFindDbProcessor == null)
            {
                throw new ApplicationException(
                    $"{nameof(SystemGlobals)}.{nameof(SystemGlobals.AdvancedFindDbProcessor)} not set.");
            }
            var recordLock = SystemGlobals.AdvancedFindDbProcessor.GetRecordLock(newEntity.Table, newEntity.PrimaryKey);

            if (recordLock != null)
            {
                var tableDefinition =
                    TableDefinition.Context.TableDefinitions.FirstOrDefault(p => p.TableName == recordLock.Table);

                PrimaryKeyDescription =$"{tableDefinition.Description} Value";

                PrimaryKeyAutoFillSetup = new AutoFillSetup(tableDefinition.LookupDefinition);
                PrimaryKeyAutoFillSetup.AddViewParameter = RecordLockingParameter.InputParameter;
                PrimaryKeyAutoFillValue =
                    TableDefinition.Context.OnAutoFillTextRequest(tableDefinition, recordLock.PrimaryKey);

                var userAutoFill = TableDefinition.Context.GetUserAutoFill(recordLock.User);

                if (userAutoFill != null)
                {
                    UserAutoFillSetup = userAutoFill.AutoFillSetup;
                    UserAutoFillSetup.AddViewParameter = RecordLockingParameter.InputParameter;
                    UserAutoFillValue = userAutoFill.AutoFillValue;
                    UserName = string.Empty;
                }
                else
                {
                    UserName = recordLock.User;
                }
            }
            View.SetupView();
        }

        protected override void LoadFromEntity(RecordLock entity)
        {
            LockDate = entity.LockDateTime.ToLocalTime();
        }

        protected override RecordLock GetEntityData()
        {
            throw new System.NotImplementedException();
        }

        protected override void ClearData()
        {
            LockDate = DateTime.MinValue;
            UserAutoFillValue = null;
            UserName = string.Empty;
            PrimaryKeyAutoFillValue = null;
        }

        protected override bool SaveEntity(RecordLock entity)
        {
            throw new System.NotImplementedException();
        }

        protected override bool DeleteEntity()
        {
            throw new System.NotImplementedException();
        }

        private void ContinueSave()
        {
            RecordLockingParameter.ContinueSave = true;
            View.CloseWindow();
        }

        private void CancelSave()
        {
            RecordLockingParameter.ContinueSave = false;
            View.CloseWindow();
        }
    }
}
