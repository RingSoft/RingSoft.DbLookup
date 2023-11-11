using System;
using System.Collections.Generic;
using System.Media;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Input;
using RingSoft.DataEntryControls.WPF;
using RingSoft.DbLookup.Lookup;
using RingSoft.DbLookup.ModelDefinition.FieldDefinitions;
using RingSoft.DbMaintenance;

namespace RingSoft.DbLookup.Controls.WPF
{
    public abstract class DbMaintenanceWindowProcessor : IDbMaintenanceProcessor
    {
        public DbMaintenanceViewModelBase ViewModel { get; private set; }

        public  AutoFillControl KeyAutoFillControl { get; private set; }

        public abstract Button SaveButton { get; }
        public VmUiControl SaveButtonUiControl { get; private set; }
        public abstract Button SelectButton { get; }
        public VmUiControl SelectButtonUiControl { get; private set; }
        public abstract Button DeleteButton { get; }
        public VmUiControl DeleteButtonUiControl { get; private set; }
        public abstract Button FindButton { get; }
        public VmUiControl FindButtonUiControl { get; private set; }
        public abstract Button NewButton { get; }
        public VmUiControl NewButtonUiControl { get; private set; }
        public abstract Button CloseButton { get; }
        public abstract Button NextButton { get; }
        public VmUiControl NextButtonUiControl { get; private set; }
        public abstract Button PreviousButton { get; }
        public VmUiControl PreviousButtonUiControl { get; private set; }
        public abstract Button PrintButton { get; }
        public VmUiControl PrintButtonUiControl { get; private set; }
        public BaseWindow MaintenanceWindow { get; private set; }
        public Control MaintenanceButtonsControl { get; private set; }
        public VmUiControl MaintenanceButtonsUiControl { get; private set; }
        public IDbMaintenanceView View { get; private set; }
        public DbMaintenanceStatusBar StatusBar { get; private set; }
        public VmUiControl StatusBarUiControl { get; private set; }

        private VmUiControl _keyAutoFillControlUiControl;
        private bool _registerKeyControl;
        private LookupAddViewArgs _lookupAddViewArgs;

        protected virtual void SetupControl()
        {
            if (PreviousButton != null)
            {
                PreviousButton.Command = ViewModel.PreviousCommand;
                PreviousButtonUiControl = new VmUiControl(PreviousButton, ViewModel.PreviousUiCommand);
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

            if (FindButton != null)
            {
                FindButton.Command = ViewModel.FindCommand;
                FindButtonUiControl = new VmUiControl(FindButton, ViewModel.FindUiCommand);
            }

            if (SelectButton != null)
            {
                SelectButton.Command = ViewModel.SelectCommand;
                SelectButtonUiControl = new VmUiControl(SelectButton, ViewModel.SelectUiCommand);
            }

            if (NextButton != null)
            {
                NextButton.Command = ViewModel.NextCommand;
                NextButtonUiControl = new VmUiControl(NextButton, ViewModel.NextUiCommand);
            }

            if (PrintButton != null)
            {
                PrintButton.Command = ViewModel.PrintCommand;
                PrintButtonUiControl = new VmUiControl(PrintButton, ViewModel.PrintUiCommand);
            }

            if (CloseButton != null)
            {
                CloseButton.Click += (_, _) => CloseWindow();
            }

            MaintenanceWindow.ShowInTaskbar = false;
            MaintenanceWindow.EnterToTab = true;
            MaintenanceButtonsControl.Margin = new Thickness(0, 0, 0, 2.5);

            MaintenanceWindow.Loaded += (sender, args) =>
            {
                ViewModel.OnViewLoaded(View);
            };
            MaintenanceWindow.PreviewKeyDown += DbMaintenanceWindow_PreviewKeyDown;
            //MaintenanceWindow.Closing += (sender, args) => ViewModel.OnWindowClosing(args);
        }

        public virtual void CloseWindow()
        {
            MaintenanceWindow.Close();
        }

        protected virtual void DbMaintenanceWindow_PreviewKeyDown(object sender, KeyEventArgs e)
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

        public virtual void Initialize(BaseWindow window, Control buttonsControl, DbMaintenanceViewModelBase viewModel,
            IDbMaintenanceView view, DbMaintenanceStatusBar statusBar = null)
        {
            MaintenanceWindow = window;
            MaintenanceButtonsControl = buttonsControl;
            ViewModel = viewModel;
            ViewModel.Processor  = this;
            View = view;
            SetupControl();
            SetupStatusBar(viewModel, statusBar);
            MaintenanceButtonsUiControl =
                new VmUiControl(MaintenanceButtonsControl, ViewModel.MaintenanceButtonsUiCommand);
            if (statusBar != null)
            {
                StatusBarUiControl = new VmUiControl(statusBar, ViewModel.StatusBarUiCommand);
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

        public void SetupStatusBar(DbMaintenanceViewModelBase viewModel, DbMaintenanceStatusBar statusBar)
        {
            if (statusBar == null)
            {
                return;
            }
            StatusBar = statusBar;
            BindingOperations.SetBinding(statusBar, DbMaintenanceStatusBar.LastSavedDateProperty, new Binding
            {
                Source = viewModel,
                Path = new PropertyPath(nameof(ViewModel.LastSavedDate)),
                Mode = BindingMode.TwoWay
            });

        }

        public void SetSaveStatus(string message, AlertLevels alertLevel)
        {
            if (StatusBar != null)
            {
                StatusBar.SetSaveStatus(message, alertLevel);
            }
        }

        public List<DbAutoFillMap> GetAutoFills()
        {
            if (View is Window window)
            {
                return LookupControlsGlobals.GetAutoFills(window);
            }
            return null;
        }

        public void HandleAutoFillValFail(DbAutoFillMap autoFillMap)
        {
            if (View is Window window)
            {
                LookupControlsGlobals.HandleValFail(window, autoFillMap);
            }
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

        public bool KeyControlRegistered { get; set; }
        public event EventHandler<LookupAddViewArgs> LookupAddView;

        public virtual void InitializeFromLookupData(LookupAddViewArgs e)
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

        public virtual void OnValidationFail(FieldDefinition fieldDefinition, string text, string caption)
        {
            MessageBox.Show(text, caption, MessageBoxButton.OK, MessageBoxImage.Exclamation);
        }

        public virtual void OnRecordSelected()
        {
            if (FocusManager.GetFocusedElement(MaintenanceWindow) is TextBox textBox)
            {
                var lookupControl = textBox.GetParentOfType<LookupControl>();
                if (lookupControl == null)
                    textBox.SelectAll();
            }
        }

        public event EventHandler DebugShowFind;

        public virtual void ShowFindLookupWindow(LookupDefinitionBase lookupDefinition, bool allowAdd, bool allowView,
            string initialSearchFor, PrimaryKeyValue initialSearchForPrimaryKey)
        {
            var lookupWindow = LookupControlsGlobals.LookupWindowFactory.CreateLookupWindow(lookupDefinition,
                allowAdd, allowView, initialSearchFor, initialSearchForPrimaryKey);

            //lookupWindow.InitialSearchForPrimaryKeyValue = initialSearchForPrimaryKey;

            lookupWindow.LookupSelect += (sender, args) =>
            {
                ViewModel.OnRecordSelected(args);
            };
            lookupWindow.Owner = MaintenanceWindow;
            lookupWindow.AddViewParameter = ViewModel?.InputParameter;
            lookupWindow.ApplyNewLookup += (sender, args) =>
                ViewModel.FindButtonLookupDefinition = lookupWindow.LookupDefinition;

            DebugShowFind?.Invoke(this, EventArgs.Empty);
            bool isAltDown = IsMaintenanceKeyDown(MaintenanceKey.Alt);
            lookupWindow.Closed += (sender, args) =>
            {
                MaintenanceWindow.Activate();
                if (!isAltDown)
                {
                    View.ResetViewForNewRecord();
                }
            };
            lookupWindow.Show();
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

        public virtual void SetReadOnlyMode(bool readOnlyValue)
        {
            
        }

        public virtual void OnRecordSelect(LookupSelectArgs args)
        {
            OnRecordSelected();
        }

        public virtual void OnReadOnlyModeSet(bool readOnlyValue)
        {
            if (MaintenanceWindow != null && MaintenanceButtonsControl != null)
            {
                if (readOnlyValue)
                {
                    var focusedElement = FocusManager.GetFocusedElement(MaintenanceWindow);
                    if (focusedElement == null || !focusedElement.IsEnabled)
                        NextButton.Focus();
                }
                else if (MaintenanceButtonsControl.IsKeyboardFocusWithin)
                {
                    WPFControlsGlobals.SendKey(Key.Tab);
                }
            }
        }

        public void Activate()
        {
            if (MaintenanceWindow != null)
            {
                MaintenanceWindow.Activate();
                MaintenanceWindow.Focus();
                WPFControlsGlobals.SendKey(Key.Tab);
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

        public void CheckAddOnFlyMode()
        {
            
        }

        public bool CheckDeleteTables(DeleteTables deleteTables)
        {
            var deleteWindow = new DeleteRecordWindow(deleteTables);
            deleteWindow.Owner = MaintenanceWindow;
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
            filterWindow.Owner = MaintenanceWindow;
            filterWindow.ShowInTaskbar = false;
            filterWindow.ShowDialog();
        }

        public virtual void SetWindowReadOnlyMode()
        {
            SaveButton.IsEnabled = DeleteButton.IsEnabled = NewButton.IsEnabled = false;
            SelectButton.IsEnabled = false;
        }

        public bool ShowRecordLockWindow(PrimaryKeyValue lockKey, string message, object inputParameter)
        {
            var recordLockInputParameter = new RecordLockingInputParameter
            {
                InputParameter = inputParameter,
                RecordLockMessage = message
            };

            var lookupDefinition = SystemGlobals.AdvancedFindLookupContext.RecordLockingLookup.Clone();

            lookupDefinition.ShowAddOnTheFlyWindow(string.Empty, MaintenanceWindow, null, recordLockInputParameter,
                lockKey);
            
            return recordLockInputParameter.ContinueSave;
        }

        public bool IsMaintenanceKeyDown(MaintenanceKey key)
        {
            switch (key)
            {
                case MaintenanceKey.Alt:
                    return Keyboard.IsKeyDown(Key.LeftAlt) || Keyboard.IsKeyDown(Key.RightAlt);
                case MaintenanceKey.Ctrl:
                    return Keyboard.IsKeyDown(Key.LeftCtrl) || Keyboard.IsKeyDown(Key.RightCtrl);
                default:
                    throw new ArgumentOutOfRangeException(nameof(key), key, null);
            }
        }
    }
}
