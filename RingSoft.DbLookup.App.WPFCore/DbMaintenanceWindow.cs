using System;
using System.Media;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Input;
using RingSoft.DataEntryControls.WPF;
using RingSoft.DbLookup.Controls.WPF;
using RingSoft.DbLookup.Lookup;
using RingSoft.DbLookup.ModelDefinition.FieldDefinitions;
using RingSoft.DbMaintenance;

namespace RingSoft.DbLookup.App.WPFCore
{
    public abstract class DbMaintenanceWindow : BaseWindow, IDbMaintenanceView
    {
        public abstract DbMaintenanceViewModelBase ViewModel { get; }

        public abstract DbMaintenanceButtonsControl MaintenanceButtonsControl { get; }

        public AutoFillControl KeyAutoFillControl { get; private set; }


        public event EventHandler<LookupSelectArgs> LookupFormReturn;

        public DbMaintenanceWindow()
        {
            EnterToTab = true;
        }
        protected void Initialize()
        {
            ShowInTaskbar = false;
            MaintenanceButtonsControl.Margin = new Thickness(0, 0, 0, 2.5);

            MaintenanceButtonsControl.PreviousButton.Command = ViewModel.PreviousCommand;
            MaintenanceButtonsControl.NewButton.Command = ViewModel.NewCommand;
            MaintenanceButtonsControl.SaveButton.Command = ViewModel.SaveCommand;
            MaintenanceButtonsControl.DeleteButton.Command = ViewModel.DeleteCommand;
            MaintenanceButtonsControl.FindButton.Command = ViewModel.FindCommand;
            MaintenanceButtonsControl.SelectButton.Command = ViewModel.SelectCommand;
            MaintenanceButtonsControl.NextButton.Command = ViewModel.NextCommand;
            MaintenanceButtonsControl.CloseButton.Click += (_, _) => CloseWindow();

            Loaded += (sender, args) => ViewModel.OnViewLoaded(this);
            PreviewKeyDown += DbMaintenanceWindow_PreviewKeyDown;

            Closing += (sender, args) => ViewModel.OnWindowClosing(args);
        }

        protected void RegisterFormKeyControl(AutoFillControl keyAutoFillControl)
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
        }

        private void DbMaintenanceWindow_PreviewKeyDown(object sender, KeyEventArgs e)
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

        public void InitializeFromLookupData(LookupAddViewArgs e)
        {
            ViewModel.InitializeFromLookupData(e);
        }

        public virtual void OnValidationFail(FieldDefinition fieldDefinition, string text, string caption)
        {
            MessageBox.Show(text, caption, MessageBoxButton.OK, MessageBoxImage.Exclamation);
        }

        public virtual void ResetViewForNewRecord()
        {
        }

        public void OnRecordSelected()
        {
            if (FocusManager.GetFocusedElement(this) is TextBox textBox)
            {
                var lookupControl = textBox.GetParentOfType<LookupControl>();
                if (lookupControl == null)
                    textBox.SelectAll();
            }
        }

        public void ShowFindLookupWindow(LookupDefinitionBase lookupDefinition, bool allowAdd, bool allowView,
            string initialSearchFor, PrimaryKeyValue initialSearchForPrimaryKey)
        {
            var lookupWindow = LookupControlsGlobals.LookupWindowFactory.CreateLookupWindow(lookupDefinition,
                allowAdd, allowView, initialSearchFor, initialSearchForPrimaryKey);

            lookupWindow.LookupSelect += (sender, args) =>
            {
                LookupFormReturn?.Invoke(this, args);
            };
            lookupWindow.Owner = this;
            lookupWindow.ShowDialog();
        }

        public void CloseWindow()
        {
            Close();
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

        public void ShowRecordSavedMessage()
        {
            MessageBox.Show("Record Saved!", "Record Saved", MessageBoxButton.OK, MessageBoxImage.Information);
        }

        protected override void OnReadOnlyModeSet(bool readOnlyValue)
        {
            if (readOnlyValue)
            {
                var focusedElement = FocusManager.GetFocusedElement(this);
                if (focusedElement == null || !focusedElement.IsEnabled)
                    MaintenanceButtonsControl.NextButton.Focus();
            }
            else if (MaintenanceButtonsControl.IsKeyboardFocusWithin)
            {
                WPFControlsGlobals.SendKey(Key.Tab);
            }

        }

        public override void SetControlReadOnlyMode(Control control, bool readOnlyValue)
        {
            if (control == KeyAutoFillControl)
            {
                return;
            }
            base.SetControlReadOnlyMode(control, readOnlyValue);
        }
    }
}
