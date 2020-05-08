using System;
using RingSoft.DbLookup.AutoFill;
using RingSoft.DbLookup.Lookup;
using System.Collections.ObjectModel;
using System.Data;
using System.Windows;
using System.Windows.Input;

namespace RingSoft.DbLookup.Controls.WPF
{
    /// <summary>
    /// Interaction logic for AutoFillControl.xaml
    /// </summary>
    public partial class AutoFillControl
    {
        public ObservableCollection<AutoFillContainsItem> ContainsItems { get; set; } =
            new ObservableCollection<AutoFillContainsItem>();

        public static readonly DependencyProperty SetupProperty =
            DependencyProperty.Register("Setup", typeof(AutoFillSetup), typeof(AutoFillControl),
                new FrameworkPropertyMetadata(SetupChangedCallback));

        public AutoFillSetup Setup
        {
            get { return (AutoFillSetup)GetValue(SetupProperty); }
            set { SetValue(SetupProperty, value); }
        }

        private static void SetupChangedCallback(DependencyObject obj,
            DependencyPropertyChangedEventArgs args)
        {
            var autoFillControl = (AutoFillControl)obj;
            if (autoFillControl._controlLoaded)
                autoFillControl.SetupControl();
        }

        public static readonly DependencyProperty ValueProperty =
            DependencyProperty.Register("Value", typeof(AutoFillValue), typeof(AutoFillControl),
                new FrameworkPropertyMetadata(ValueChangedCallback));

        public AutoFillValue Value
        {
            get { return (AutoFillValue)GetValue(ValueProperty); }
            set { SetValue(ValueProperty, value); }
        }

        private static void ValueChangedCallback(DependencyObject obj,
            DependencyPropertyChangedEventArgs args)
        {
            var autoFillControl = (AutoFillControl)obj;
            if (!autoFillControl._onAutoFillDataChanged)
            {
                autoFillControl._onValuePropertySetting = true;
                autoFillControl.SetValue();
                autoFillControl._onValuePropertySetting = false;
            }
        }

        public static readonly DependencyProperty IsDirtyProperty =
            DependencyProperty.Register("IsDirty", typeof(bool), typeof(AutoFillControl));
        //, new FrameworkPropertyMetadata(IsDirtyChangedCallback));

        //Uncomment to debug data binding.
        //private static void IsDirtyChangedCallback(DependencyObject obj,
        //    DependencyPropertyChangedEventArgs args)
        //{
        //}

        public bool IsDirty
        {
            get { return (bool)GetValue(IsDirtyProperty); }
            set { SetValue(IsDirtyProperty, value); }
        }

        public static readonly DependencyProperty TabOutAfterLookupSelectProperty =
            DependencyProperty.Register("TabOutAfterLookupSelect", typeof(bool), typeof(AutoFillControl));
        //, new FrameworkPropertyMetadata(TabOutAfterLookupSelectChangedCallback));

        //Uncomment to debug data binding.
        //private static void TabOutAfterLookupSelectChangedCallback(DependencyObject obj,
        //    DependencyPropertyChangedEventArgs args)
        //{
        //}

        /// <summary>
        /// Gets or sets a value indicating whether to automatically tab out after lookup select.
        /// </summary>
        /// <value>
        ///   <c>true</c> if to tab out after lookup select; otherwise, <c>false</c>.
        /// </value>
        public bool TabOutAfterLookupSelect
        {
            get { return (bool)GetValue(TabOutAfterLookupSelectProperty); }
            set { SetValue(TabOutAfterLookupSelectProperty, value); }
        }

        /// <summary>
        /// Gets the AutoFill data.
        /// </summary>
        /// <value>
        /// The auto fill data.
        /// </value>
        public AutoFillData AutoFillData { get; private set; }


        private bool _controlLoaded;
        private bool _onAutoFillDataChanged;
        private bool _onValuePropertySetting;
        private bool _pendingAutoFillValue;

        public AutoFillControl()
        {
            TabOutAfterLookupSelect = true;

            //InitializeComponent();
            this.LoadViewFromUri("/RingSoft.DbLookup.Controls.WPF;component/AutoFillControl.xaml");

            Loaded += (sender, args) =>
            {
                if (Setup != null)
                {
                    SetupControl();
                    _controlLoaded = true;
                }

                if (IsFocused)
                    AutoFillTextBox.Focus();
            };

            SizeChanged += (sender, args) => { ListBox.Width = AutoFillTextBox.ActualWidth; };
            LostFocus += (sender, args) => { Popup.IsOpen = false; };
            GotFocus += (sender, args) =>
            {
                AutoFillTextBox.Focus();
                AutoFillTextBox.SelectionStart = 0;
                AutoFillTextBox.SelectionLength = AutoFillTextBox.Text.Length;
            };

        }

        private void SetupControl()
        {
            if (Setup.LookupDefinition == null || Setup.LookupDefinition.InitialSortColumnDefinition == null)
                throw new ArgumentException(
                    "Lookup definition does not have any visible columns defined or its initial sort column is null.");

            if (AutoFillData != null)
                ClearValue();

            AutoFillData = new AutoFillData(Setup.LookupDefinition, Setup.Distinct)
            { ShowContainsBox = Setup.ShowContainsBox };


            AutoFillData.AutoFillDataChanged += AutoFillData_AutoFillDataChanged;

            if (!_controlLoaded)
            {
                AutoFillTextBox.PreviewKeyDown += AutoFillTextBox_PreviewKeyDown;

                AutoFillTextBox.PreviewTextInput += AutoFillTextBox_PreviewTextInput;
                ListBox.SelectionChanged += (sender, args) =>
                {
                    AutoFillData.OnChangeContainsIndex(ListBox.SelectedIndex);
                };
                LookupButton.Click += (sender, args) => ShowLookupWindow();
            }

            if (_pendingAutoFillValue)
            {
                _onValuePropertySetting = true;
                SetValue(Value.PrimaryKeyValue, Value.Text);
                _onValuePropertySetting = false;
                _pendingAutoFillValue = false;
            }
        }


        private void AutoFillData_AutoFillDataChanged(object sender, AutoFillDataChangedArgs e)
        {
            //Unit Test
            //var startText = AutoFillText.Text;
            //if (!string.IsNullOrEmpty(startText))
            //{
            //    Clipboard.SetText(AutoFillText.Text);
            //}
            //var preSelStart = AutoFillText.SelectionStart;
            //var preSelLen = AutoFillText.SelectionLength;
            //MessageBox.Show($"PreText = {startText}");

            AutoFillTextBox.Text = e.TextResult;
            AutoFillTextBox.SelectionStart = e.CursorStartIndex;
            AutoFillTextBox.SelectionLength = e.TextSelectLength;

            if (e.RefreshContainsList)
            {
                var openPopup = false;
                ContainsItems.Clear();
                if (AutoFillData.ShowContainsBox && e.ContainsBoxDataTable != null)
                {
                    foreach (DataRow dataRow in e.ContainsBoxDataTable.Rows)
                    {
                        ContainsItems.Add(AutoFillData.GetAutoFillContainsItem(dataRow));
                        openPopup = true;
                    }
                }

                Popup.IsOpen = openPopup;
            }

            if (!_onValuePropertySetting)
            {
                _onAutoFillDataChanged = true;
                Value = new AutoFillValue(AutoFillData.PrimaryKeyValue, e.TextResult);
                _onAutoFillDataChanged = false;
            }

            //Unit Test
            //if (!string.IsNullOrEmpty(AutoFillText.Text))
            //    Clipboard.SetText(AutoFillText.Text);
            //MessageBox.Show(
            //    $"PreSelStart = {preSelStart.ToString()}, PreSelLen = {preSelLen.ToString()}\r\nPostSelStart = {AutoFillText.SelectionStart}, PostSelLen = {AutoFillText.SelectionLength}",
            //    "Test", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        private void SetValue()
        {
            if (Value == null)
            {
                ClearValue();
            }
            else
            {
                if (AutoFillData == null)
                    _pendingAutoFillValue = true;

                SetValue(Value.PrimaryKeyValue, Value.Text);
            }
        }

        private void SetValue(PrimaryKeyValue primaryKeyValue, string text)
        {
            if (AutoFillData == null)
            {
                return;
            }
            AutoFillData.SetValue(primaryKeyValue, text, Popup.IsOpen); //Must set to false otherwise list shows when control is tabbed out.
        }

        /// <summary>
        /// Clears the value.
        /// </summary>
        public void ClearValue()
        {
            if (AutoFillData == null)
                return;

            AutoFillData.ClearValue();
        }

        private void AutoFillTextBox_PreviewKeyDown(object sender, KeyEventArgs e)
        {
            switch (e.Key)
            {
                case Key.Back:
                    AutoFillData.OnBackspaceKeyDown(AutoFillTextBox.Text, AutoFillTextBox.SelectionStart,
                        AutoFillTextBox.SelectionLength);
                    e.Handled = true;
                    IsDirty = true;
                    break;
                case Key.Delete:
                    AutoFillData.OnDeleteKeyDown(AutoFillTextBox.Text, AutoFillTextBox.SelectionStart,
                        AutoFillTextBox.SelectionLength);
                    e.Handled = true;
                    IsDirty = true;
                    break;
                case Key.Escape:
                    if (Popup.IsOpen)
                    {
                        Popup.IsOpen = false;
                        e.Handled = true;
                    }

                    break;
                case Key.Down:
                    if (Popup.IsOpen && ListBox.SelectedIndex < ContainsItems.Count - 1)
                    {
                        ListBox.SelectedIndex++;
                    }
                    break;
                case Key.Up:
                    if (Popup.IsOpen && ListBox.SelectedIndex > 0)
                        ListBox.SelectedIndex--;
                    break;
                case Key.F5:
                    ShowLookupWindow();
                    break;
            }

        }

        private void AutoFillTextBox_PreviewTextInput(object sender, TextCompositionEventArgs e)
        {
            if (Keyboard.IsKeyDown(Key.LeftCtrl) || Keyboard.IsKeyDown(Key.RightCtrl))
                return;

            if (Keyboard.IsKeyDown(Key.LeftAlt) || Keyboard.IsKeyDown(Key.RightAlt))
                return;

            if (AutoFillData.OnKeyCharPressed(e.Text[0], AutoFillTextBox.Text, AutoFillTextBox.SelectionStart, AutoFillTextBox.SelectionLength))
                e.Handled = true;
            IsDirty = true;
        }

        private void ShowLookupWindow()
        {
            var lookupWindow = new LookupWindow(Setup.LookupDefinition, Setup.AllowLookupAdd, Setup.AllowLookupView, AutoFillTextBox.Text);
            lookupWindow.RefreshData += (o, args) => AutoFillData.RefreshData(Popup.IsOpen);
            lookupWindow.LookupSelect += LookupForm_LookupSelect;
            lookupWindow.ShowDialog();
        }

        private void LookupForm_LookupSelect(object sender, LookupSelectArgs e)
        {
            var text = e.LookupData.GetSelectedRow()
                .GetRowValue(Setup.LookupDefinition.InitialSortColumnDefinition.SelectSqlAlias);
            SetValue(e.LookupData.PrimaryKeyValue, text);
            IsDirty = true;
            if (TabOutAfterLookupSelect)
            {
                Send(Key.Tab);
            }
        }
        public static void Send(Key key)
        {
            if (Keyboard.PrimaryDevice != null)
            {
                if (Keyboard.PrimaryDevice.ActiveSource != null)
                {
                    var e = new KeyEventArgs(Keyboard.PrimaryDevice, Keyboard.PrimaryDevice.ActiveSource, 0, key)
                    {
                        RoutedEvent = Keyboard.KeyDownEvent
                    };
                    InputManager.Current.ProcessInput(e);
                }
            }
        }
    }
}