using System;
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
        public abstract DbMaintenanceViewModelBase ViewModel { get; set; }

        public AutoFillControl KeyAutoFillControl { get; set; }

        public abstract Button SaveButton { get; set; }
        public abstract Button SelectButton { get; set; }
        public abstract Button DeleteButton { get; set; }
        public abstract Button FindButton { get; set; }
        public abstract Button NewButton { get; set;  }
        public abstract Button CloseButton { get; set; }
        public abstract Button NextButton { get; set;  }
        public abstract Button PreviousButton { get; set; }

        public abstract BaseWindow MaintenanceWindow { get; set; }

        public abstract Control MaintenanceButtonsControl { get; set; }

        public IDbMaintenanceView View { get; set; }


        public virtual void SetupControl(IDbMaintenanceView view)
        {
            ViewModel.Processor = this;

            View = view;

            if (PreviousButton == null)
            {
                throw new Exception("Maintenance Buttons Not Created");
            }
            PreviousButton.Command = ViewModel.PreviousCommand;
            NewButton.Command = ViewModel.NewCommand;
            SaveButton.Command = ViewModel.SaveCommand;
            DeleteButton.Command = ViewModel.DeleteCommand;
            FindButton.Command = ViewModel.FindCommand;
            SelectButton.Command = ViewModel.SelectCommand;
            NextButton.Command = ViewModel.NextCommand;
            CloseButton.Click += (_, _) => CloseWindow();

            MaintenanceWindow.ShowInTaskbar = false;
            MaintenanceWindow.EnterToTab = true;
            MaintenanceButtonsControl.Margin = new Thickness(0, 0, 0, 2.5);

            MaintenanceWindow.Loaded += (sender, args) => ViewModel.OnViewLoaded(View);
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
            IDbMaintenanceView view)
        {
            MaintenanceWindow = window;
            MaintenanceButtonsControl = buttonsControl;
            ViewModel = viewModel;
            View = view;
        }

        public virtual void RegisterFormKeyControl(AutoFillControl keyAutoFillControl)
        {
            KeyAutoFillControl = keyAutoFillControl;
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

            keyAutoFillControl.LostFocus += (sender, args) => ViewModel.OnKeyControlLeave();
            KeyAutoFillControl.SetReadOnlyMode(false);
            KeyControlRegistered = true;
        }

        public bool KeyControlRegistered { get; set; }
        public event EventHandler<LookupAddViewArgs> LookupAddView;

        public virtual void InitializeFromLookupData(LookupAddViewArgs e)
        {
            if (ViewModel != null)
            {
                ViewModel.InitializeFromLookupData(e);
                LookupAddView?.Invoke(this, e);
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

            lookupWindow.LookupSelect += (sender, args) =>
            {
                ViewModel.OnRecordSelected(args);
            };
            lookupWindow.Owner = MaintenanceWindow;
            lookupWindow.AddViewParameter = ViewModel?.InputParameter;
            lookupWindow.ApplyNewLookup += (sender, args) =>
                ViewModel.FindButtonLookupDefinition = lookupWindow.LookupDefinition;

            DebugShowFind?.Invoke(this, EventArgs.Empty);
            lookupWindow.ShowDialog();
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

        public virtual bool SetControlReadOnlyMode(Control control, bool readOnlyValue)
        {
            if (control == KeyAutoFillControl)
            {
                return false;
            }

            return true;
        }

        public void CheckAddOnFlyMode()
        {
            
        }
    }
}
