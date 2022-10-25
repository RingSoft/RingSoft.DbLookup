using System;
using System.Linq;
using RingSoft.DbLookup;
using RingSoft.DbLookup.AutoFill;
using RingSoft.DbLookup.ModelDefinition;
using RingSoft.DbLookup.RecordLocking;

namespace RingSoft.DbMaintenance
{
    public interface IRecordLockingView : IDbMaintenanceView
    {
        void SetupView();
    }
    public class RecordLockingViewModel : DbMaintenanceViewModel<RecordLock>
    {
        public override TableDefinition<RecordLock> TableDefinition { get; }

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

        protected override void Initialize()
        {
            if (!(base.View is IRecordLockingView))
            {
                throw new Exception($"View must be of type {nameof(IRecordLockingView)}");
            }
            base.Initialize();
        }

        protected override RecordLock PopulatePrimaryKeyControls(RecordLock newEntity, PrimaryKeyValue primaryKeyValue)
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

                PrimaryKeyDescription = tableDefinition.Description;

                PrimaryKeyAutoFillSetup = new AutoFillSetup(tableDefinition.LookupDefinition);
                PrimaryKeyAutoFillValue =
                    TableDefinition.Context.OnAutoFillTextRequest(TableDefinition, recordLock.PrimaryKey);

                var userAutoFill = TableDefinition.Context.GetUserAutoFill(recordLock.User);

                if (userAutoFill != null)
                {
                    UserAutoFillSetup = userAutoFill.AutoFillSetup;
                    UserAutoFillValue = userAutoFill.AutoFillValue;
                    UserName = string.Empty;
                }
                else
                {
                    UserName = recordLock.User;
                }
            }
            View.SetupView();
            return recordLock;
        }

        protected override void LoadFromEntity(RecordLock entity)
        {
            
        }

        protected override RecordLock GetEntityData()
        {
            throw new System.NotImplementedException();
        }

        protected override void ClearData()
        {
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
    }
}
