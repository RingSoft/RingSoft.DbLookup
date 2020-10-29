using RingSoft.DataEntryControls.WPF;
using RingSoft.DbLookup.AutoFill;
using RingSoft.DbLookup.Lookup;
using RingSoft.DbLookup.ModelDefinition.FieldDefinitions;
using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Data;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;

namespace RingSoft.DbLookup.Controls.WPF
{
    /// <summary>
    /// A control to auto fill a value into a text box and show similar values in a drop down contains box as the user types.
    /// </summary>
    public partial class AutoFillControl : IAutoFillControl
    {
        public ObservableCollection<AutoFillContainsItem> ContainsItems { get; set; } =
            new ObservableCollection<AutoFillContainsItem>();

        public static readonly DependencyProperty SetupProperty =
            DependencyProperty.Register("Setup", typeof(AutoFillSetup), typeof(AutoFillControl),
                new FrameworkPropertyMetadata(SetupChangedCallback));

        /// <summary>
        /// Gets or sets the AutoFillSetup to determine how this control will behave.
        /// </summary>
        /// <value>
        /// The AutoFillSetup.
        /// </value>
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

        /// <summary>
        /// Gets or sets the AutoFillValue.
        /// </summary>
        /// <value>
        /// The value.
        /// </value>
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

        /// <summary>
        /// Gets or sets a value indicating whether this control's value has changed.
        /// </summary>
        /// <value>
        ///   <c>true</c> if this control's value has changed; otherwise, <c>false</c>.
        /// </value>
        public bool IsDirty
        {
            get { return (bool)GetValue(IsDirtyProperty); }
            set { SetValue(IsDirtyProperty, value); }
        }

        public static readonly DependencyProperty TabOutAfterLookupSelectProperty =
            DependencyProperty.Register("TabOutAfterLookupSelect", typeof(bool), typeof(AutoFillControl));

        /// <summary>
        /// Gets or sets a value indicating whether to automatically tab out after the user selects a record in the LookupWindow.
        /// </summary>
        /// <value>
        ///   <c>true</c> if to tab out after lookup select; otherwise, <c>false</c>.
        /// </value>
        public bool TabOutAfterLookupSelect
        {
            get { return (bool)GetValue(TabOutAfterLookupSelectProperty); }
            set { SetValue(TabOutAfterLookupSelectProperty, value); }
        }

        public static readonly DependencyProperty ShowContainsBoxProperty =
            DependencyProperty.Register("ShowContainsBox", typeof(bool), typeof(AutoFillControl));

        /// <summary>
        /// Gets or sets a value indicating whether to show the contains box.
        /// </summary>
        /// <value>
        ///   <c>true</c> if to show the contains box; otherwise, <c>false</c>.
        /// </value>
        public bool ShowContainsBox
        {
            get { return (bool)GetValue(ShowContainsBoxProperty); }
            set { SetValue(ShowContainsBoxProperty, value); }
        }

        public static readonly DependencyProperty ContainsBoxMaxRowsProperty =
            DependencyProperty.Register("ContainsBoxMaxRows", typeof(int), typeof(AutoFillControl));

        /// <summary>
        /// Gets or sets the contains box maximum number of rows.
        /// </summary>
        /// <value>
        /// The contains box maximum number of rows.
        /// </value>
        public int ContainsBoxMaxRows
        {
            get { return (int)GetValue(ContainsBoxMaxRowsProperty); }
            set { SetValue(ContainsBoxMaxRowsProperty, value); }
        }

        public static readonly DependencyProperty DesignTextProperty =
            DependencyProperty.Register("DesignText", typeof(string), typeof(AutoFillControl),
                new FrameworkPropertyMetadata(DesignTextChangedCallback));

        public string DesignText
        {
            get { return (string)GetValue(DesignTextProperty); }
            set { SetValue(DesignTextProperty, value); }
        }

        private static void DesignTextChangedCallback(DependencyObject obj,
            DependencyPropertyChangedEventArgs args)
        {
            var autoFillControl = (AutoFillControl)obj;
            autoFillControl.SetDesignText();
        }

        public static readonly DependencyProperty CharacterCasingProperty =
            DependencyProperty.Register("CharacterCasing", typeof(CharacterCasing), typeof(AutoFillControl),
                new FrameworkPropertyMetadata(CharacterCasingChangedCallback));

        public CharacterCasing CharacterCasing
        {
            get { return (CharacterCasing)GetValue(CharacterCasingProperty); }
            set { SetValue(CharacterCasingProperty, value); }
        }

        private static void CharacterCasingChangedCallback(DependencyObject obj,
            DependencyPropertyChangedEventArgs args)
        {
            var autoFillControl = (AutoFillControl)obj;
            autoFillControl.AutoFillTextBox.CharacterCasing = autoFillControl.CharacterCasing;
        }

        public string EditText
        {
            get => AutoFillTextBox.Text;
            set
            {
                _settingText = true;
                AutoFillTextBox.Text = value;
                _settingText = false;
            } 
        }

        public int SelectionStart
        {
            get => AutoFillTextBox.SelectionStart;
            set => AutoFillTextBox.SelectionStart = value;
        }

        public int SelectionLength
        {
            get => AutoFillTextBox.SelectionLength;
            set => AutoFillTextBox.SelectionLength = value;
        }

        public bool ContainsBoxIsOpen
        {
            get
            {
                var result = false;
                if (Popup != null)
                    result = Popup.IsOpen;

                return result;
            }

        }

        public new Brush Background
        {
            get => AutoFillTextBox.Background;
            set => AutoFillTextBox.Background = value;
        }

        public new Brush Foreground
        {
            get => AutoFillTextBox.Foreground;
            set => AutoFillTextBox.Foreground = value;
        }

        public Brush SelectionBrush
        {
            get => AutoFillTextBox.SelectionBrush;
            set => AutoFillTextBox.SelectionBrush = value;
        }

        public TextAlignment TextAlignment
        {
            get => AutoFillTextBox.TextAlignment;
            set => AutoFillTextBox.TextAlignment = value;
        }

        public event EventHandler ControlDirty;

        private AutoFillData _autoFillData;
        private bool _controlLoaded;
        private bool _onAutoFillDataChanged;
        private bool _onValuePropertySetting;
        private bool _pendingAutoFillValue;
        private bool _setupRan;
        private bool _settingText;

        static AutoFillControl()
        {
            ShowContainsBoxProperty.OverrideMetadata(typeof(AutoFillControl), new PropertyMetadata(true));
            TabOutAfterLookupSelectProperty.OverrideMetadata(typeof(AutoFillControl), new PropertyMetadata(true));
            ContainsBoxMaxRowsProperty.OverrideMetadata(typeof(AutoFillControl), new PropertyMetadata(5));
        }

        public AutoFillControl()
        {
            //InitializeComponent();
            this.LoadViewFromUri("/RingSoft.DbLookup.Controls.WPF;component/AutoFillControl.xaml");

            Loaded += (sender, args) =>
            {
                if (Setup != null && !_controlLoaded)
                {
                    SetupControl();
                }
                _controlLoaded = true;

                if (IsFocused)
                    AutoFillTextBox.Focus();
            };

            SizeChanged += (sender, args) => { ListBox.Width = AutoFillTextBox.ActualWidth; };
            LostFocus += (sender, args) =>
            {
                Popup.IsOpen = false;
            };
            GotFocus += (sender, args) =>
            {
                AutoFillTextBox.Focus();
                AutoFillTextBox.SelectionStart = 0;
                AutoFillTextBox.SelectionLength = AutoFillTextBox.Text.Length;
            };
            ContextMenu = new ContextMenu();
            ContextMenu.AddTextBoxContextMenuItems();
            AutoFillTextBox.ContextMenu = ContextMenu;
        }

        private void SetupControl()
        {
            if (Setup.LookupDefinition == null || Setup.LookupDefinition.InitialSortColumnDefinition == null)
                throw new ArgumentException(
                    "Lookup definition does not have any visible columns defined or its initial sort column is null.");

            if (_autoFillData != null)
                ClearValue();

            _autoFillData = new AutoFillData(this, Setup.LookupDefinition, Setup.Distinct)
            {
                ShowContainsBox = ShowContainsBox,
                ContainsBoxMaxRows = ContainsBoxMaxRows
            };

            if (Setup.LookupDefinition.InitialSortColumnDefinition is LookupFieldColumnDefinition fieldColumn)
            {
                if (fieldColumn.FieldDefinition is StringFieldDefinition stringField)
                    AutoFillTextBox.MaxLength = stringField.MaxLength;
            }


            _autoFillData.AutoFillDataChanged += AutoFillData_AutoFillDataChanged;

            if (!_setupRan)
            {
                AutoFillTextBox.PreviewKeyDown += AutoFillTextBox_PreviewKeyDown;

                AutoFillTextBox.PreviewTextInput += AutoFillTextBox_PreviewTextInput;
                ListBox.SelectionChanged += (sender, args) =>
                {
                    _autoFillData.OnChangeContainsIndex(ListBox.SelectedIndex);
                };
                LookupButton.Click += (sender, args) => ShowLookupWindow();
                AutoFillTextBox.TextChanged += AutoFillTextBox_TextChanged;
            }

            if (_pendingAutoFillValue)
            {
                _onValuePropertySetting = true;
                SetValue(Value.PrimaryKeyValue, Value.Text);
                _onValuePropertySetting = false;
                _pendingAutoFillValue = false;
            }

            _setupRan = true;
        }

        private void AutoFillTextBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (!_settingText)
            {
                _autoFillData.OnTextChanged();
            }
        }

        private void SetDesignText()
        {
            if (DesignerProperties.GetIsInDesignMode(this) && !DesignText.IsNullOrEmpty())
            {
                _settingText = true;
                AutoFillTextBox.Text = DesignText;
                _settingText = false;
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

            if (e.RefreshContainsList)
            {
                var openPopup = false;
                ContainsItems.Clear();
                if (_autoFillData.ShowContainsBox && e.ContainsBoxDataTable != null)
                {
                    foreach (DataRow dataRow in e.ContainsBoxDataTable.Rows)
                    {
                        ContainsItems.Add(_autoFillData.GetAutoFillContainsItem(dataRow));
                        openPopup = true;
                    }
                }

                Popup.IsOpen = openPopup;
            }

            if (!_onValuePropertySetting)
            {
                _onAutoFillDataChanged = true;
                Value = new AutoFillValue(_autoFillData.PrimaryKeyValue, EditText);
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
                if (_autoFillData == null)
                    _pendingAutoFillValue = true;

                SetValue(Value.PrimaryKeyValue, Value.Text);
            }
        }

        private void SetValue(PrimaryKeyValue primaryKeyValue, string text)
        {
            if (_autoFillData == null)
            {
                return;
            }
            _autoFillData.SetValue(primaryKeyValue, text, Popup.IsOpen); //Must set to false otherwise list shows when control is tabbed out.
        }

        private void ClearValue()
        {
            if (_autoFillData == null)
                return;

            _autoFillData.ClearValue();
        }

        private void AutoFillTextBox_PreviewKeyDown(object sender, KeyEventArgs e)
        {
            switch (e.Key)
            {
                case Key.Back:
                    _autoFillData.OnBackspaceKeyDown();
                    e.Handled = true;
                    RaiseDirtyFlag();
                    break;
                case Key.Delete:
                    _autoFillData.OnDeleteKeyDown();
                    e.Handled = true;
                    RaiseDirtyFlag();
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

            if (AutoFillTextBox.MaxLength > 0 && AutoFillTextBox.SelectionLength <= 0 &&
                AutoFillTextBox.Text.Length >= AutoFillTextBox.MaxLength)
            {
                System.Media.SystemSounds.Exclamation.Play();
                return;
            }

            var newText = e.Text;
            switch (CharacterCasing)
            {
                case CharacterCasing.Normal:
                    break;
                case CharacterCasing.Lower:
                    newText = newText.ToLower();
                    break;
                case CharacterCasing.Upper:
                    newText = newText.ToUpper();
                    break;
            }
            if (_autoFillData.OnKeyCharPressed(newText[0]))
                e.Handled = true;
            RaiseDirtyFlag();
        }

        private void ShowLookupWindow()
        {
            var lookupWindow = ControlsGlobals.LookupWindowFactory.CreateLookupWindow(Setup.LookupDefinition,
                Setup.AllowLookupAdd, Setup.AllowLookupView, AutoFillTextBox.Text);
            lookupWindow.Owner = Window.GetWindow(this);
            lookupWindow.RefreshData += (o, args) => _autoFillData.RefreshData(Popup.IsOpen);
            lookupWindow.LookupSelect += LookupForm_LookupSelect;
            lookupWindow.ShowDialog();
        }

        private void LookupForm_LookupSelect(object sender, LookupSelectArgs e)
        {
            var text = e.LookupData.GetSelectedRow()
                .GetRowValue(Setup.LookupDefinition.InitialSortColumnDefinition.SelectSqlAlias);
            SetValue(e.LookupData.SelectedPrimaryKeyValue, text);
            RaiseDirtyFlag();
            if (TabOutAfterLookupSelect)
            {
                Send(Key.Tab);
            }
        }

        private void RaiseDirtyFlag()
        {
            IsDirty = true;
            ControlDirty?.Invoke(this, EventArgs.Empty);
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