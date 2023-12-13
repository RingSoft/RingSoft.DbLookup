// ***********************************************************************
// Assembly         : RingSoft.DbMaintenance
// Author           : petem
// Created          : 12-19-2022
//
// Last Modified By : petem
// Last Modified On : 12-08-2023
// ***********************************************************************
// <copyright file="RecordLockingViewModel.cs" company="Peter Ringering">
//     Copyright (c) . All rights reserved.
// </copyright>
// <summary></summary>
// ***********************************************************************
using System;
using System.Linq;
using RingSoft.DataEntryControls.Engine;
using RingSoft.DbLookup;
using RingSoft.DbLookup.AutoFill;
using RingSoft.DbLookup.ModelDefinition;
using RingSoft.DbLookup.RecordLocking;

namespace RingSoft.DbMaintenance
{
    /// <summary>
    /// Interface IRecordLockingView
    /// Extends the <see cref="RingSoft.DbMaintenance.IDbMaintenanceView" />
    /// </summary>
    /// <seealso cref="RingSoft.DbMaintenance.IDbMaintenanceView" />
    public interface IRecordLockingView : IDbMaintenanceView
    {
        /// <summary>
        /// Setups the view.
        /// </summary>
        void SetupView();

        /// <summary>
        /// Closes the window.
        /// </summary>
        void CloseWindow();
    }

    /// <summary>
    /// Class RecordLockingInputParameter.
    /// </summary>
    public class RecordLockingInputParameter
    {
        /// <summary>
        /// Gets or sets the input parameter.
        /// </summary>
        /// <value>The input parameter.</value>
        public object InputParameter { get; set; }

        /// <summary>
        /// Gets or sets the record lock message.
        /// </summary>
        /// <value>The record lock message.</value>
        public string RecordLockMessage { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether [continue save].
        /// </summary>
        /// <value><c>true</c> if [continue save]; otherwise, <c>false</c>.</value>
        public bool ContinueSave { get; set; }
    }

    /// <summary>
    /// Class RecordLockingViewModel.
    /// Implements the <see cref="RingSoft.DbMaintenance.DbMaintenanceViewModel{RingSoft.DbLookup.RecordLocking.RecordLock}" />
    /// </summary>
    /// <seealso cref="RingSoft.DbMaintenance.DbMaintenanceViewModel{RingSoft.DbLookup.RecordLocking.RecordLock}" />
    public class RecordLockingViewModel : DbMaintenanceViewModel<RecordLock>
    {
        /// <summary>
        /// Gets the table definition.
        /// </summary>
        /// <value>The table definition.</value>
        public override TableDefinition<RecordLock> TableDefinition =>
            SystemGlobals.AdvancedFindLookupContext.RecordLocks;

        /// <summary>
        /// The message
        /// </summary>
        private string _message;

        /// <summary>
        /// Gets or sets the message.
        /// </summary>
        /// <value>The message.</value>
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


        /// <summary>
        /// The lock date
        /// </summary>
        private DateTime _lockDate;

        /// <summary>
        /// Gets or sets the lock date.
        /// </summary>
        /// <value>The lock date.</value>
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


        /// <summary>
        /// The primary key description
        /// </summary>
        private string _primaryKeyDescription;

        /// <summary>
        /// Gets or sets the primary key description.
        /// </summary>
        /// <value>The primary key description.</value>
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


        /// <summary>
        /// The primary key automatic fill setup
        /// </summary>
        private AutoFillSetup _primaryKeyAutoFillSetup;

        /// <summary>
        /// Gets or sets the primary key automatic fill setup.
        /// </summary>
        /// <value>The primary key automatic fill setup.</value>
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


        /// <summary>
        /// The primary key automatic fill value
        /// </summary>
        private AutoFillValue  _primaryKeyAutoFillValue;

        /// <summary>
        /// Gets or sets the primary key automatic fill value.
        /// </summary>
        /// <value>The primary key automatic fill value.</value>
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

        /// <summary>
        /// The user name
        /// </summary>
        private string _userName;

        /// <summary>
        /// Gets or sets the name of the user.
        /// </summary>
        /// <value>The name of the user.</value>
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

        /// <summary>
        /// The user automatic fill setup
        /// </summary>
        private AutoFillSetup _userAutoFillSetup;

        /// <summary>
        /// Gets or sets the user automatic fill setup.
        /// </summary>
        /// <value>The user automatic fill setup.</value>
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

        /// <summary>
        /// The user automatic fill value
        /// </summary>
        private AutoFillValue _userAutoFillValue;

        /// <summary>
        /// Gets or sets the user automatic fill value.
        /// </summary>
        /// <value>The user automatic fill value.</value>
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

        /// <summary>
        /// Gets or sets the view.
        /// </summary>
        /// <value>The view.</value>
        public new IRecordLockingView View { get; set; }

        /// <summary>
        /// Gets or sets the continue save command.
        /// </summary>
        /// <value>The continue save command.</value>
        public RelayCommand ContinueSaveCommand { get; set; }

        /// <summary>
        /// Gets or sets the cancel save command.
        /// </summary>
        /// <value>The cancel save command.</value>
        public RelayCommand CancelSaveCommand { get; set; }

        /// <summary>
        /// Gets or sets the record locking parameter.
        /// </summary>
        /// <value>The record locking parameter.</value>
        public RecordLockingInputParameter RecordLockingParameter { get; set; }

        /// <summary>
        /// Initializes a new instance of the <see cref="RecordLockingViewModel"/> class.
        /// </summary>
        public RecordLockingViewModel()
        {
            ContinueSaveCommand = new RelayCommand(ContinueSave);

            CancelSaveCommand = new RelayCommand(CancelSave);
        }
        /// <summary>
        /// Initializes this instance.  Executed after the view is loaded.
        /// </summary>
        /// <exception cref="System.Exception">View must be of type {nameof(IRecordLockingView)}</exception>
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
            ReadOnlyMode = true;
            base.Initialize();
        }

        /// <summary>
        /// Populates the primary key controls.  This is executed during record save and retrieval operations.
        /// </summary>
        /// <param name="newEntity">The entity containing just the primary key values.</param>
        /// <param name="primaryKeyValue">The primary key value.</param>
        /// <returns>An entity populated from the database.</returns>
        /// <exception cref="System.ApplicationException"></exception>
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

        /// <summary>
        /// Loads this view model from the entity generated from PopulatePrimaryKeyControls.  This is executed only during record retrieval operations.
        /// </summary>
        /// <param name="entity">The entity that was loaded from the database by PopulatePrimaryKeyControls.</param>
        protected override void LoadFromEntity(RecordLock entity)
        {
            LockDate = entity.LockDateTime.ToLocalTime();
        }

        /// <summary>
        /// Gets the entity data.
        /// </summary>
        /// <returns>TEntity.</returns>
        /// <exception cref="System.NotImplementedException"></exception>
        protected override RecordLock GetEntityData()
        {
            throw new System.NotImplementedException();
        }

        /// <summary>
        /// Clears the data.
        /// </summary>
        protected override void ClearData()
        {
            LockDate = DateTime.MinValue;
            UserAutoFillValue = null;
            UserName = string.Empty;
            PrimaryKeyAutoFillValue = null;
        }

        /// <summary>
        /// Saves the entity.
        /// </summary>
        /// <param name="entity">The entity.</param>
        /// <returns><c>true</c> if XXXX, <c>false</c> otherwise.</returns>
        /// <exception cref="System.NotImplementedException"></exception>
        protected override bool SaveEntity(RecordLock entity)
        {
            throw new System.NotImplementedException();
        }

        /// <summary>
        /// Deletes the entity.
        /// </summary>
        /// <returns><c>true</c> if XXXX, <c>false</c> otherwise.</returns>
        /// <exception cref="System.NotImplementedException"></exception>
        protected override bool DeleteEntity()
        {
            throw new System.NotImplementedException();
        }

        /// <summary>
        /// Continues the save.
        /// </summary>
        private void ContinueSave()
        {
            RecordLockingParameter.ContinueSave = true;
            View.CloseWindow();
        }

        /// <summary>
        /// Cancels the save.
        /// </summary>
        private void CancelSave()
        {
            RecordLockingParameter.ContinueSave = false;
            View.CloseWindow();
        }
    }
}
