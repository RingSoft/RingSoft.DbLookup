using System;
using System.Collections.Generic;
using System.Media;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Data;
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
        /// Gets the print button.
        /// </summary>
        /// <value>The print button.</value>
        public abstract Button PrintButton { get; }
        /// <summary>
        /// Gets the print button UI control.
        /// </summary>
        /// <value>The print button UI control.</value>
        public VmUiControl PrintButtonUiControl { get; private set; }

        /// <summary>
        /// Gets the close button.
        /// </summary>
        /// <value>The close button.</value>
        public abstract Button CloseButton { get; }

        public AutoFillControl KeyAutoFillControl { get; private set; }


        public DbMaintenanceViewModelBase ViewModel { get; }

        public Control ButtonsControl { get; }

        public DbMaintenanceUserControl UserControl { get; }

        public DbMaintenanceStatusBar StatusBar { get; }
        public IUserControlHost Host { get; }

        public bool DeleteChildrenResult { get; set; }
        public bool PreDeleteResult { get; set; }
        public bool KeyControlRegistered { get; set; }
        public HotKeyProcessor HotKeyProcessor { get; }
        public RelayCommand CloseCommand { get; }

        private VmUiControl _keyAutoFillControlUiControl;
        public event EventHandler<LookupAddViewArgs> LookupAddView;

        private LookupAddViewArgs _lookupAddViewArgs;
        private bool _registerKeyControl;

        public DbMaintenanceUserControlProcessor(
            DbMaintenanceViewModelBase viewModel
            , Control buttonsControl
            , DbMaintenanceUserControl userControl
            , DbMaintenanceStatusBar statusBar
            , IUserControlHost host)
        {
            HotKeyProcessor = new HotKeyProcessor();
            Host = host;
            ViewModel = viewModel;
            ButtonsControl = buttonsControl;
            UserControl = userControl;
            StatusBar = statusBar;
            CloseCommand = new RelayCommand((() =>
            {
                CloseWindow();
            }));
        }

        public virtual void Initialize()
        {
            SetupControl();
            UserControl.PreviewKeyDown += UserControl_PreviewKeyDown;
            UserControl.PreviewKeyUp += UserControl_PreviewKeyUp;

            if (StatusBar != null)
            {
                SetupStatusBar(ViewModel, StatusBar);
            }

            if (_registerKeyControl && KeyAutoFillControl != null)
            {
                RegisterFormKeyControl(KeyAutoFillControl);
            }

            if (_lookupAddViewArgs != null)
            {
                InitializeFromLookupData(_lookupAddViewArgs);
                _lookupAddViewArgs = null;
            }
        }

        private void UserControl_PreviewKeyUp(object sender, KeyEventArgs e)
        {
            HotKeyProcessor.OnKeyUp(e);
        }

        public void SetupStatusBar(DbMaintenanceViewModelBase viewModel, DbMaintenanceStatusBar statusBar)
        {
            if (statusBar == null)
            {
                return;
            }
            BindingOperations.SetBinding(statusBar, DbMaintenanceStatusBar.LastSavedDateProperty, new Binding
            {
                Source = viewModel,
                Path = new PropertyPath(nameof(ViewModel.LastSavedDate)),
                Mode = BindingMode.TwoWay
            });

        }

        private void UserControl_PreviewKeyDown(object sender, System.Windows.Input.KeyEventArgs e)
        {
            HotKeyProcessor.OnKeyPressed(e);
        }

        protected virtual void SetupControl()
        {
            if (FindButton != null)
            {
                FindButton.Command = ViewModel.FindCommand;
                FindButtonUiControl = new VmUiControl(FindButton, ViewModel.FindUiCommand);
                LookupControlsGlobals.SetupFindHotKey(HotKeyProcessor, ViewModel.FindCommand);
            }

            if (PreviousButton != null)
            {
                PreviousButton.Command = ViewModel.PreviousCommand;
                PreviousButtonUiControl = new VmUiControl(PreviousButton, ViewModel.PreviousUiCommand);
                LookupControlsGlobals.SetupPreviousHotKey(HotKeyProcessor, ViewModel.PreviousCommand);
            }

            if (NextButton != null)
            {
                NextButton.Command = ViewModel.NextCommand;
                NextButtonUiControl = new VmUiControl(NextButton, ViewModel.NextUiCommand);
                LookupControlsGlobals.SetupNextHotKey(HotKeyProcessor, ViewModel.NextCommand);
            }

            if (NewButton != null)
            {
                NewButton.Command = ViewModel.NewCommand;
                NewButtonUiControl = new VmUiControl(NewButton, ViewModel.NewUiCommand);
                LookupControlsGlobals.SetupNewHotKey(HotKeyProcessor, ViewModel.NewCommand);
            }

            if (SaveButton != null)
            {
                SaveButton.Command = ViewModel.SaveCommand;
                SaveButtonUiControl = new VmUiControl(SaveButton, ViewModel.SaveUiCommand);
                LookupControlsGlobals.SetupSaveHotKey(HotKeyProcessor, ViewModel.SaveCommand);
            }

            if (DeleteButton != null)
            {
                DeleteButton.Command = ViewModel.DeleteCommand;
                DeleteButtonUiControl = new VmUiControl(DeleteButton, ViewModel.DeleteUiCommand);
                LookupControlsGlobals.SetupDeleteHotKey(HotKeyProcessor, ViewModel.DeleteCommand);
            }

            if (SelectButton != null)
            {
                SelectButton.Command = ViewModel.SelectCommand;
                SelectButtonUiControl = new VmUiControl(SelectButton, ViewModel.SelectUiCommand);
                LookupControlsGlobals.SetupSelectHotKey(HotKeyProcessor, ViewModel.SelectCommand);
            }

            if (PrintButton != null)
            {
                PrintButton.Command = ViewModel.PrintCommand;
                PrintButtonUiControl = new VmUiControl(PrintButton, ViewModel.PrintUiCommand);
                LookupControlsGlobals.SetupPrintHotKey(HotKeyProcessor, ViewModel.PrintCommand);
            }

            if (CloseButton != null)
            {
                CloseButton.Command = CloseCommand;
                LookupControlsGlobals.SetupCloseHotKey(HotKeyProcessor, CloseCommand);
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
            //lookupWindow.Owner = baseWindow;
            lookupWindow.AddViewParameter = ViewModel?.InputParameter;
            lookupWindow.ApplyNewLookup += (sender, args) =>
                ViewModel.FindButtonLookupDefinition = lookupWindow.LookupDefinition;

            bool isAltDown = IsMaintenanceKeyDown(MaintenanceKey.Alt);

            //Peter Ringering - 11/23/2024 10:30:59 AM - E-71
            lookupWindow.Closed += (sender, args) =>
            {
                baseWindow.Focus();
                UserControl.Focus();
                if (!isAltDown)
                {
                    UserControl.ResetViewForNewRecord();
                }
            };
            //lookupWindow.Show();
            lookupWindow.ShowDialog();
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

        public virtual void OnReadOnlyModeSet(bool readOnlyValue)
        {
            if (UserControl != null && ButtonsControl != null)
            {
                if (readOnlyValue)
                {
                    var focusedElement = FocusManager.GetFocusedElement(UserControl);
                    if (focusedElement == null || !focusedElement.IsEnabled)
                        NextButton.Focus();
                }
                else if (ButtonsControl.IsKeyboardFocusWithin)
                {
                    WPFControlsGlobals.SendKey(Key.Tab);
                }
            }
        }

        public virtual bool SetControlReadOnlyMode(Control control, bool readOnlyValue)
        {
            if (control == KeyAutoFillControl)
            {
                return false;
            }
            else if (control == StatusBar)
            {
                return false;
            }
            return true;
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

        public virtual void SetWindowReadOnlyMode()
        {
            SelectButton.IsEnabled = false;
            ViewModel.SaveButtonEnabled = false;
            ViewModel.DeleteButtonEnabled = false;
            ViewModel.NewButtonEnabled = false;
        }


        public virtual void RegisterFormKeyControl(AutoFillControl keyAutoFillControl)
        {
            KeyAutoFillControl = keyAutoFillControl;
            if (ViewModel == null)
            {
                _registerKeyControl = true;
                return;
            }
            BindingOperations.SetBinding(keyAutoFillControl, AutoFillControl.IsDirtyProperty, new Binding
            {
                Source = ViewModel,
                Path = new PropertyPath(nameof(ViewModel.KeyValueDirty)),
                Mode = BindingMode.TwoWay
            });

            BindingOperations.SetBinding(keyAutoFillControl, AutoFillControl.SetupProperty, new Binding
            {
                Source = ViewModel,
                Path = new PropertyPath(nameof(ViewModel.KeyAutoFillSetup))
            });

            BindingOperations.SetBinding(keyAutoFillControl, AutoFillControl.ValueProperty, new Binding
            {
                Source = ViewModel,
                Path = new PropertyPath(nameof(ViewModel.KeyAutoFillValue)),
                Mode = BindingMode.TwoWay
            });

            _keyAutoFillControlUiControl = new VmUiControl(keyAutoFillControl, ViewModel.KeyAutoFillUiCommand);
            keyAutoFillControl.UiCommand = _keyAutoFillControlUiControl.Command;

            keyAutoFillControl.AutoFillLostFocus += (sender, args) => ViewModel.OnKeyControlLeave();
            KeyAutoFillControl.SetReadOnlyMode(false);
            KeyControlRegistered = true;
        }

        public bool ShowRecordLockWindow(PrimaryKeyValue lockKey, string message, object inputParameter)
        {
            var recordLockInputParameter = new RecordLockingInputParameter
            {
                InputParameter = inputParameter,
                RecordLockMessage = message
            };

            var lookupDefinition = SystemGlobals.AdvancedFindLookupContext.RecordLockingLookup.Clone();

            lookupDefinition.ShowAddOnTheFlyWindow(string.Empty, Window.GetWindow(UserControl), null, recordLockInputParameter,
                lockKey);

            return recordLockInputParameter.ContinueSave;
        }

        public bool CheckDeleteTables(DeleteTables deleteTables)
        {
            var deleteWindow = new DeleteRecordWindow(deleteTables);
            deleteWindow.Owner = Window.GetWindow(UserControl);
            deleteWindow.ShowInTaskbar = false;
            deleteWindow.ShowDialog();
            if (deleteWindow.DialogResult.HasValue)
            {
                return deleteWindow.DialogResult.Value;
            }
            return false;
        }

        public void PrintOutput(PrinterSetupArgs printerSetupArgs)
        {
            var filterWindow = new GenericReportFilterWindow(printerSetupArgs);
            filterWindow.Owner = Window.GetWindow(UserControl);
            filterWindow.ShowInTaskbar = false;
            filterWindow.ShowDialog();
        }

        public void SetSaveStatus(string message, AlertLevels alertLevel)
        {
            if (StatusBar != null)
            {
                if (StatusBar.StatusTextBox == null)
                {
                    StatusBar.Loaded += (sender, args) =>
                    {
                        StatusBar.SetSaveStatus(message, alertLevel);
                    };
                    return;
                }
                StatusBar.SetSaveStatus(message, alertLevel);
            }
        }

        public List<DbAutoFillMap> GetAutoFills()
        {
            return LookupControlsGlobals.GetAutoFills(UserControl);
        }

        public void HandleAutoFillValFail(DbAutoFillMap autoFillMap)
        {
            LookupControlsGlobals.HandleValFail(UserControl, autoFillMap);
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
            UserControl.SetReadOnlyMode(readOnlyMode);
        }
    }
}
