using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Input;
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

        protected void Initialize()
        {
            ShowInTaskbar = false;
            MaintenanceButtonsControl.Margin = new Thickness(0, 0, 0, 2.5);

            MaintenanceButtonsControl.PreviousButton.Click += (sender, args) => ViewModel.OnGotoPreviousButton();
            MaintenanceButtonsControl.NewButton.Click += (sender, args) => ViewModel.OnNewButton();
            MaintenanceButtonsControl.SaveButton.Click += (sender, args) => ViewModel.OnSaveButton();
            MaintenanceButtonsControl.DeleteButton.Click += (sender, args) => ViewModel.OnDeleteButton();
            MaintenanceButtonsControl.FindButton.Click += (sender, args) => ViewModel.OnFindButton();
            MaintenanceButtonsControl.SelectButton.Click += (sender, args) => ViewModel.OnSelectButton();
            MaintenanceButtonsControl.CloseButton.Click += (sender, args) => CloseWindow();
            MaintenanceButtonsControl.NextButton.Click += (sender, args) => ViewModel.OnGotoNextButton();

            BindingOperations.SetBinding(MaintenanceButtonsControl.DeleteButton, Button.IsEnabledProperty, new Binding
            {
                Source = ViewModel,
                Path = new PropertyPath(nameof(ViewModel.DeleteButtonEnabled))
            });

            BindingOperations.SetBinding(MaintenanceButtonsControl.SelectButton, Button.IsEnabledProperty, new Binding
            {
                Source = ViewModel,
                Path = new PropertyPath(nameof(ViewModel.SelectButtonEnabled))
            });

            Loaded += (sender, args) => ViewModel.OnViewLoaded(this);
            PreviewKeyDown += DbMaintenanceWindow_PreviewKeyDown;

            Closing += (sender, args) => ViewModel.OnWindowClosing(args);
        }

        protected void RegisterFormKeyControl(AutoFillControl keyAutoFillControl)
        {
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

        public void ShowFindLookupWindow(LookupDefinitionBase lookupDefinition, bool allowAdd, bool allowView, string initialSearchFor)
        {
            var lookupWindow =
                new LookupWindow(lookupDefinition, allowAdd, allowView, initialSearchFor);

            lookupWindow.LookupSelect += (sender, args) =>
            {
                LookupFormReturn?.Invoke(this, args);
            };
            lookupWindow.ShowDialog();
        }

        public event EventHandler<LookupSelectArgs> LookupFormReturn;
        public void CloseWindow()
        {
            Close();
        }

        public MessageButtons ShowYesNoCancelMessage(string text, string caption)
        {
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

        public bool ShowYesNoMessage(string text, string caption)
        {
            if (MessageBox.Show(text, caption, MessageBoxButton.YesNo, MessageBoxImage.Question) ==
                MessageBoxResult.Yes)
                return true;

            return false;
        }

        public void ShowRecordSavedMessage()
        {
            MessageBox.Show("Record Saved!", "Record Saved", MessageBoxButton.OK, MessageBoxImage.Information);
        }
    }
}
