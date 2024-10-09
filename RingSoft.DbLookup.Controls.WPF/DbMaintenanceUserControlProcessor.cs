using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;
using RingSoft.DataEntryControls.Engine;
using RingSoft.DataEntryControls.WPF;
using RingSoft.DbLookup.Lookup;
using RingSoft.DbLookup.ModelDefinition.FieldDefinitions;
using RingSoft.DbMaintenance;

namespace RingSoft.DbLookup.Controls.WPF
{
    public abstract class DbMaintenanceUserControlProcessor : IDbMaintenanceDataProcessor
    {
        /// <summary>
        /// Gets the find button.
        /// </summary>
        /// <value>The find button.</value>
        public abstract Button FindButton { get; }
        /// <summary>
        /// Gets the find button UI control.
        /// </summary>
        /// <value>The find button UI control.</value>
        public VmUiControl FindButtonUiControl { get; private set; }

        public DbMaintenanceViewModelBase ViewModel { get; }

        public Control ButtonsControl { get; }

        public DbMaintenanceUserControl UserControl { get; }

        public DbMaintenanceStatusBar StatusBar { get; }

        public bool DeleteChildrenResult { get; set; }
        public bool PreDeleteResult { get; set; }
        public bool KeyControlRegistered { get; set; }
        public event EventHandler<LookupAddViewArgs> LookupAddView;

        public DbMaintenanceUserControlProcessor(
            DbMaintenanceViewModelBase viewModel
            , Control buttonsControl
            , DbMaintenanceUserControl userControl
            , DbMaintenanceStatusBar statusBar)
        {
            ViewModel = viewModel;
            ButtonsControl = buttonsControl;
            UserControl = userControl;
            StatusBar = statusBar;
        }

        public virtual void Initialize()
        {
            SetupControl();
        }

        protected virtual void SetupControl()
        {
            if (FindButton != null)
            {
                FindButton.Command = ViewModel.FindCommand;
                FindButtonUiControl = new VmUiControl(FindButton, ViewModel.FindUiCommand);
            }
        }

        public void InitializeFromLookupData(LookupAddViewArgs e)
        {
            
        }

        public void OnValidationFail(FieldDefinition fieldDefinition, string text, string caption)
        {
            
        }

        public void OnRecordSelected()
        {
            
        }

        public void ShowFindLookupWindow(LookupDefinitionBase lookupDefinition, bool allowAdd, bool allowView, string initialSearchFor,
            PrimaryKeyValue initialSearchForPrimaryKey)
        {
            var lookupWindow = LookupControlsGlobals.LookupWindowFactory.CreateLookupWindow(lookupDefinition,
                allowAdd, allowView, initialSearchFor, initialSearchForPrimaryKey);

            //lookupWindow.InitialSearchForPrimaryKeyValue = initialSearchForPrimaryKey;

            lookupWindow.LookupSelect += (sender, args) =>
            {
                ViewModel.OnRecordSelected(args);
            };
            var baseWindow = Window.GetWindow(UserControl);
            lookupWindow.Owner = baseWindow;
            lookupWindow.AddViewParameter = ViewModel?.InputParameter;
            lookupWindow.ApplyNewLookup += (sender, args) =>
                ViewModel.FindButtonLookupDefinition = lookupWindow.LookupDefinition;

            bool isAltDown = IsMaintenanceKeyDown(MaintenanceKey.Alt);
            lookupWindow.Closed += (sender, args) =>
            {
                baseWindow.Focus();
                UserControl.Focus();
                if (!isAltDown)
                {
                    UserControl.ResetViewForNewRecord();
                }
            };
            lookupWindow.Show();
        }

        public void CloseWindow()
        {
            
        }

        public MessageButtons ShowYesNoCancelMessage(string text, string caption, bool playSound = false)
        {
            throw new NotImplementedException();
        }

        public bool ShowYesNoMessage(string text, string caption, bool playSound = false)
        {
            throw new NotImplementedException();
        }

        public void ShowRecordSavedMessage()
        {
            
        }

        public void OnReadOnlyModeSet(bool readOnlyValue)
        {
            
        }

        public bool IsMaintenanceKeyDown(MaintenanceKey key)
        {
            return false;
        }

        public void Activate()
        {
            
        }

        public void SetWindowReadOnlyMode()
        {
            
        }

        public bool ShowRecordLockWindow(PrimaryKeyValue lockKey, string message, object inputParameter)
        {
            return true;
        }

        public bool CheckDeleteTables(DeleteTables deleteTables)
        {
            return true;
        }

        public void PrintOutput(PrinterSetupArgs printerSetupArgs)
        {
            
        }

        public void SetSaveStatus(string message, AlertLevels alertLevel)
        {
            
        }

        public List<DbAutoFillMap> GetAutoFills()
        {
            throw new NotImplementedException();
        }

        public void HandleAutoFillValFail(DbAutoFillMap autoFillMap)
        {
            
        }

        public ITwoTierProcessingProcedure GetDeleteProcedure(DeleteTables deleteTables)
        {
            throw new NotImplementedException();
        }

        public void GetPreDeleteProcedure(List<FieldDefinition> fields, DeleteTables deleteTables)
        {
            
        }

        public void SetWindowReadOnlyMode(bool readOnlyMode)
        {
            
        }
    }
}
