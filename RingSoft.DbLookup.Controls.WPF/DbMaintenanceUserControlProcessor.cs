using System;
using System.Collections.Generic;
using System.Media;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
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
        /// <summary>
        /// Gets the next button.
        /// </summary>
        /// <value>The next button.</value>
        public abstract Button NextButton { get; }
        /// <summary>
        /// Gets the next button UI control.
        /// </summary>
        /// <value>The next button UI control.</value>
        public VmUiControl NextButtonUiControl { get; private set; }
        /// <summary>
        /// Gets the previous button.
        /// </summary>
        /// <value>The previous button.</value>
        public abstract Button PreviousButton { get; }
        /// <summary>
        /// Gets the previous button UI control.
        /// </summary>
        /// <value>The previous button UI control.</value>
        public VmUiControl PreviousButtonUiControl { get; private set; }
        /// <summary>
        /// Gets the save button.
        /// </summary>
        /// <value>The save button.</value>
        public abstract Button SaveButton { get; }
        /// <summary>
        /// Gets the save button UI control.
        /// </summary>
        /// <value>The save button UI control.</value>
        public VmUiControl SaveButtonUiControl { get; private set; }
        /// <summary>
        /// Gets the select button.
        /// </summary>
        /// <value>The select button.</value>
        public abstract Button SelectButton { get; }
        /// <summary>
        /// Gets the select button UI control.
        /// </summary>
        /// <value>The select button UI control.</value>
        public VmUiControl SelectButtonUiControl { get; private set; }
        /// <summary>
        /// Gets the delete button.
        /// </summary>
        /// <value>The delete button.</value>
        public abstract Button DeleteButton { get; }
        /// <summary>
        /// Gets the delete button UI control.
        /// </summary>
        /// <value>The delete button UI control.</value>
        public VmUiControl DeleteButtonUiControl { get; private set; }

        /// <summary>
        /// Creates new button.
        /// </summary>
        /// <value>The new button.</value>
        public abstract Button NewButton { get; }
        /// <summary>
        /// Creates new buttonuicontrol.
        /// </summary>
        /// <value>The new button UI control.</value>
        public VmUiControl NewButtonUiControl { get; private set; }

        /// <summary>
        /// Gets the close button.
        /// </summary>
        /// <value>The close button.</value>
        public abstract Button CloseButton { get; }


        public DbMaintenanceViewModelBase ViewModel { get; }

        public Control ButtonsControl { get; }

        public DbMaintenanceUserControl UserControl { get; }

        public DbMaintenanceStatusBar StatusBar { get; }
        public IUserControlHost Host { get; }

        public bool DeleteChildrenResult { get; set; }
        public bool PreDeleteResult { get; set; }
        public bool KeyControlRegistered { get; set; }
        public event EventHandler<LookupAddViewArgs> LookupAddView;

        private LookupAddViewArgs _lookupAddViewArgs;

        public DbMaintenanceUserControlProcessor(
            DbMaintenanceViewModelBase viewModel
            , Control buttonsControl
            , DbMaintenanceUserControl userControl
            , DbMaintenanceStatusBar statusBar
            , IUserControlHost host)
        {
            Host = host;
            ViewModel = viewModel;
            ButtonsControl = buttonsControl;
            UserControl = userControl;
            StatusBar = statusBar;
        }

        public virtual void Initialize()
        {
            SetupControl();
            UserControl.PreviewKeyDown += UserControl_PreviewKeyDown;

            if (_lookupAddViewArgs != null)
            {
                InitializeFromLookupData(_lookupAddViewArgs);
                _lookupAddViewArgs = null;
            }
        }

        private void UserControl_PreviewKeyDown(object sender, System.Windows.Input.KeyEventArgs e)
        {
            if (Keyboard.IsKeyDown(Key.LeftCtrl) || Keyboard.IsKeyDown(Key.RightCtrl))
            {
                switch (e.Key)
                {
                    case Key.Left:
                        ViewModel.OnGotoPreviousButton();
                        e.Handled = true;
                        break;
                    case Key.Right:
                        ViewModel.OnGotoNextButton();
                        e.Handled = true;
                        break;
                }
            }
        }

        protected virtual void SetupControl()
        {
            if (FindButton != null)
            {
                FindButton.Command = ViewModel.FindCommand;
                FindButtonUiControl = new VmUiControl(FindButton, ViewModel.FindUiCommand);
            }

            if (PreviousButton != null)
            {
                PreviousButton.Command = ViewModel.PreviousCommand;
                PreviousButtonUiControl = new VmUiControl(PreviousButton, ViewModel.PreviousUiCommand);
            }

            if (NextButton != null)
            {
                NextButton.Command = ViewModel.NextCommand;
                NextButtonUiControl = new VmUiControl(NextButton, ViewModel.NextUiCommand);
            }

            if (NewButton != null)
            {
                NewButton.Command = ViewModel.NewCommand;
                NewButtonUiControl = new VmUiControl(NewButton, ViewModel.NewUiCommand);
            }

            if (SaveButton != null)
            {
                SaveButton.Command = ViewModel.SaveCommand;
                SaveButtonUiControl = new VmUiControl(SaveButton, ViewModel.SaveUiCommand);
            }

            if (DeleteButton != null)
            {
                DeleteButton.Command = ViewModel.DeleteCommand;
                DeleteButtonUiControl = new VmUiControl(DeleteButton, ViewModel.DeleteUiCommand);
            }

            if (SelectButton != null)
            {
                SelectButton.Command = ViewModel.SelectCommand;
                SelectButtonUiControl = new VmUiControl(SelectButton, ViewModel.SelectUiCommand);
            }

            if (CloseButton != null)
            {
                CloseButton.Click += (_, _) => CloseWindow();
            }

        }

        public void InitializeFromLookupData(LookupAddViewArgs e)
        {
            if (ViewModel == null)
            {
                _lookupAddViewArgs = e;
                return;
            }
            ViewModel.InitializeFromLookupData(e);
            LookupAddView?.Invoke(this, e);
            if (e.LookupReadOnlyMode)
            {
                SelectButton.Visibility = Visibility.Collapsed;
            }

        }

        public void OnValidationFail(FieldDefinition fieldDefinition, string text, string caption)
        {
            MessageBox.Show(text, caption, MessageBoxButton.OK, MessageBoxImage.Exclamation);
        }

        public void OnRecordSelected()
        {
            if (FocusManager.GetFocusedElement(UserControl) is TextBox textBox)
            {
                var lookupControl = textBox.GetParentOfType<LookupControl>();
                if (lookupControl == null)
                    textBox.SelectAll();
            }
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
            Host.CloseHost();
        }

        public MessageButtons ShowYesNoCancelMessage(string text, string caption, bool playSound = false)
        {
            if (playSound)
                SystemSounds.Exclamation.Play();

            var result = MessageBox.Show(text, caption, MessageBoxButton.YesNoCancel, MessageBoxImage.Question);
            switch (result)
            {
                case MessageBoxResult.Yes:
                    return MessageButtons.Yes;
                case MessageBoxResult.No:
                    return MessageButtons.No;
            }

            return MessageButtons.Cancel;
        }

        public bool ShowYesNoMessage(string text, string caption, bool playSound = false)
        {
            if (playSound)
                SystemSounds.Exclamation.Play();

            if (MessageBox.Show(text, caption, MessageBoxButton.YesNo, MessageBoxImage.Question) ==
                MessageBoxResult.Yes)
                return true;

            return false;
        }

        public virtual void ShowRecordSavedMessage()
        {
            MessageBox.Show("Record Saved!", "Record Saved", MessageBoxButton.OK, MessageBoxImage.Information);
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
            if (UserControl != null)
            {
                UserControl.Focus();
                WPFControlsGlobals.SendKey(Key.Tab);
            }

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
            return LookupControlsGlobals.GetAutoFills(UserControl);
        }

        public void HandleAutoFillValFail(DbAutoFillMap autoFillMap)
        {
            
        }

        public ITwoTierProcessingProcedure GetDeleteProcedure(DeleteTables deleteTables)
        {
            var delProcedure = new DeleteProcedure (Window.GetWindow(UserControl)
                , "Deleting Table Data"
                , ViewModel
                , deleteTables);
            delProcedure.Start();
            return delProcedure;
        }

        public void GetPreDeleteProcedure(List<FieldDefinition> fields, DeleteTables deleteTables)
        {
            var delProcedure = new PreDeleteProcedure(Window.GetWindow(UserControl)
                , "Gathering Tables To Delete"
                , ViewModel
                , deleteTables
                , fields
                , this);

            delProcedure.Start();
        }

        public void SetWindowReadOnlyMode(bool readOnlyMode)
        {
            
        }
    }
}
