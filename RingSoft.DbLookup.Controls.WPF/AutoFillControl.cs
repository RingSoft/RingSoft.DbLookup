using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Data;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Data;
using System.Windows.Input;
using System.Windows.Media;
using RingSoft.DataEntryControls.Engine;
using RingSoft.DataEntryControls.WPF;
using RingSoft.DbLookup.AutoFill;
using RingSoft.DbLookup.Lookup;
using RingSoft.DbLookup.ModelDefinition.FieldDefinitions;

namespace RingSoft.DbLookup.Controls.WPF
{
    /// <summary>
    /// Follow steps 1a or 1b and then 2 to use this custom control in a XAML file.
    ///
    /// Step 1a) Using this custom control in a XAML file that exists in the current project.
    /// Add this XmlNamespace attribute to the root element of the markup file where it is 
    /// to be used:
    ///
    ///     xmlns:MyNamespace="clr-namespace:RingSoft.DbLookup.Controls.WPF"
    ///
    ///
    /// Step 1b) Using this custom control in a XAML file that exists in a different project.
    /// Add this XmlNamespace attribute to the root element of the markup file where it is 
    /// to be used:
    ///
    ///     xmlns:MyNamespace="clr-namespace:RingSoft.DbLookup.Controls.WPF;assembly=RingSoft.DbLookup.Controls.WPF"
    ///
    /// You will also need to add a project reference from the project where the XAML file lives
    /// to this project and Rebuild to avoid compilation errors:
    ///
    ///     Right click on the target project in the Solution Explorer and
    ///     "Add Reference"->"Projects"->[Browse to and select this project]
    ///
    ///
    /// Step 2)
    /// Go ahead and use your control in the XAML file.
    ///
    ///     <MyNamespace:NewAutoFillControl/>
    ///
    /// </summary>
    [TemplatePart(Name = "TextBox", Type = typeof(TextBox))]
    [TemplatePart(Name = "Button", Type = typeof(Button))]
    [TemplatePart(Name = "Popup", Type = typeof(Popup))]
    [TemplatePart(Name = "ListBox", Type = typeof(ListBox))]
    public class AutoFillControl : Control, IAutoFillControl
    {
        public TextBox TextBox { get; set; }

        public Button Button { get; set; }

        public Popup Popup { get; set; }

        public ListBox ListBox { get; set; }

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

            if (autoFillControl.TextBox != null)
                autoFillControl.TextBox.CharacterCasing = autoFillControl.CharacterCasing;
        }

        public static readonly DependencyProperty TextAlignmentProperty =
            DependencyProperty.Register(nameof(TextAlignment), typeof(TextAlignment), typeof(AutoFillControl),
                new FrameworkPropertyMetadata(TextAlignmentChangedCallback));

        public TextAlignment TextAlignment
        {
            get { return (TextAlignment)GetValue(TextAlignmentProperty); }
            set { SetValue(TextAlignmentProperty, value); }
        }

        private static void TextAlignmentChangedCallback(DependencyObject obj,
            DependencyPropertyChangedEventArgs args)
        {
            var autoFillControl = (AutoFillControl)obj;
            if (autoFillControl.TextBox != null)
                autoFillControl.TextBox.TextAlignment = autoFillControl.TextAlignment;
        }

        private static void BorderThicknessChangedCallback(DependencyObject obj,
    DependencyPropertyChangedEventArgs args)
        {
            var autoFillControl = (AutoFillControl)obj;
            if (autoFillControl.TextBox != null)
            {
                autoFillControl.TextBox.BorderThickness = autoFillControl.BorderThickness;
            }
        }

        private static void BackgroundChangedCallback(DependencyObject obj,
            DependencyPropertyChangedEventArgs args)
        {
            var autoFillControl = (AutoFillControl)obj;
            if (autoFillControl.TextBox != null)
            {
                autoFillControl.TextBox.Background = autoFillControl.Background;
            }
        }

        private static void ForegroundChangedCallback(DependencyObject obj,
            DependencyPropertyChangedEventArgs args)
        {
            var autoFillControl = (AutoFillControl)obj;
            if (autoFillControl.TextBox != null)
            {
                autoFillControl.TextBox.Foreground = autoFillControl.Foreground;
            }
        }

        public static readonly DependencyProperty SelectionBrushProperty =
            DependencyProperty.Register(nameof(SelectionBrush), typeof(Brush), typeof(AutoFillControl),
                new FrameworkPropertyMetadata(SelectionBrushChangedCallback));

        public Brush SelectionBrush
        {
            get { return (Brush)GetValue(SelectionBrushProperty); }
            set { SetValue(SelectionBrushProperty, value); }
        }

        private static void SelectionBrushChangedCallback(DependencyObject obj,
            DependencyPropertyChangedEventArgs args)
        {
            var autoFillControl = (AutoFillControl)obj;
            if (autoFillControl.TextBox != null)
                autoFillControl.TextBox.SelectionBrush = autoFillControl.SelectionBrush;
        }

        public string EditText
        {
            get
            {
                if (TextBox != null)
                    return TextBox.Text;

                return string.Empty;
            }
            set
            {
                if (TextBox != null)
                {
                    _settingText = true;
                    TextBox.Text = value;
                    _settingText = false;
                }
            }
        }

        public int SelectionStart
        {
            get
            {
                if (TextBox != null)
                    return TextBox.SelectionStart;

                return 0;
            }
            set
            {
                if (TextBox != null)
                    TextBox.SelectionStart = value;
            }
        }

        public int SelectionLength
        {
            get
            {
                if (TextBox != null)
                    return TextBox.SelectionLength;

                return 0;
            }
            set
            {
                if (TextBox != null)
                    TextBox.SelectionLength = value;
            }
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
            DefaultStyleKeyProperty.OverrideMetadata(typeof(AutoFillControl),
                new FrameworkPropertyMetadata(typeof(AutoFillControl)));

            IsTabStopProperty.OverrideMetadata(typeof(AutoFillControl), new FrameworkPropertyMetadata(false));

            KeyboardNavigation.TabNavigationProperty.OverrideMetadata(typeof(AutoFillControl),
                new FrameworkPropertyMetadata(KeyboardNavigationMode.Local));

            ShowContainsBoxProperty.OverrideMetadata(typeof(AutoFillControl), new PropertyMetadata(true));
            TabOutAfterLookupSelectProperty.OverrideMetadata(typeof(AutoFillControl), new PropertyMetadata(true));
            ContainsBoxMaxRowsProperty.OverrideMetadata(typeof(AutoFillControl), new PropertyMetadata(5));

            BorderThicknessProperty.OverrideMetadata(typeof(AutoFillControl),
                new FrameworkPropertyMetadata(new Thickness(1), BorderThicknessChangedCallback));

            BackgroundProperty.OverrideMetadata(typeof(AutoFillControl),
                new FrameworkPropertyMetadata(BackgroundChangedCallback));

            ForegroundProperty.OverrideMetadata(typeof(AutoFillControl),
                new FrameworkPropertyMetadata(ForegroundChangedCallback));

        }

        public AutoFillControl()
        {
            Loaded += (sender, args) => OnLoad();

            SizeChanged += (sender, args) =>
            {
                if (TextBox != null && ListBox != null)
                    ListBox.Width = TextBox.ActualWidth;
            };
            LostFocus += (sender, args) =>
            {
                if (Popup != null)
                    Popup.IsOpen = false;
            };
            GotFocus += (sender, args) =>
            {
                if (TextBox != null)
                {
                    TextBox.Focus();
                    TextBox.SelectionStart = 0;
                    TextBox.SelectionLength = TextBox.Text.Length;
                }
            };
            ContextMenu = new ContextMenu();
            ContextMenu.AddTextBoxContextMenuItems();

        }

        public override void OnApplyTemplate()
        {
            TextBox = GetTemplateChild(nameof(TextBox)) as TextBox;
            Button = GetTemplateChild(nameof(Button)) as Button;
            Popup = GetTemplateChild(nameof(Popup)) as Popup;
            ListBox = GetTemplateChild(nameof(ListBox)) as ListBox;

            if (TextBox != null)
                TextBox.ContextMenu = ContextMenu;

            base.OnApplyTemplate();
        }

        private void OnLoad()
        {
            if (Setup != null && !_controlLoaded)
            {
                SetupControl();
            }
            _controlLoaded = true;

            if (IsFocused)
                TextBox?.Focus();

            CreateContainsTemplate();
        }

        private void CreateContainsTemplate()
        {
            if (ListBox == null)
                return;

            var dataTemplate = new DataTemplate();

            var stackPanelFactory = new FrameworkElementFactory(typeof(StackPanel));
            stackPanelFactory.SetValue(StackPanel.OrientationProperty, Orientation.Horizontal);

            var textBlockFactory = new FrameworkElementFactory(typeof(TextBlock));
            textBlockFactory.SetBinding(TextBlock.TextProperty, new Binding(nameof(AutoFillContainsItem.PrefixText)));
            stackPanelFactory.AppendChild(textBlockFactory);

            textBlockFactory = new FrameworkElementFactory(typeof(TextBlock));
            textBlockFactory.SetBinding(TextBlock.TextProperty, new Binding(nameof(AutoFillContainsItem.ContainsText)));
            textBlockFactory.SetValue(TextBlock.FontWeightProperty, FontWeights.Bold);
            stackPanelFactory.AppendChild(textBlockFactory);

            textBlockFactory = new FrameworkElementFactory(typeof(TextBlock));
            textBlockFactory.SetBinding(TextBlock.TextProperty, new Binding(nameof(AutoFillContainsItem.SuffixText)));
            stackPanelFactory.AppendChild(textBlockFactory);

            dataTemplate.VisualTree = stackPanelFactory;
            ListBox.ItemTemplate = dataTemplate;
            ListBox.ItemsSource = ContainsItems;
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
                if (fieldColumn.FieldDefinition is StringFieldDefinition stringField && TextBox != null)
                    TextBox.MaxLength = stringField.MaxLength;
            }


            _autoFillData.AutoFillDataChanged += AutoFillData_AutoFillDataChanged;

            if (!_setupRan)
            {
                if (TextBox != null)
                {
                    TextBox.PreviewKeyDown += TextBox_PreviewKeyDown;
                    TextBox.PreviewTextInput += TextBox_PreviewTextInput;
                    TextBox.TextChanged += TextBox_TextChanged;
                }

                if (ListBox != null)
                {
                    ListBox.SelectionChanged += (sender, args) =>
                    {
                        _autoFillData.OnChangeContainsIndex(ListBox.SelectedIndex);
                    };
                }

                if (Button != null)
                    Button.Click += (sender, args) => ShowLookupWindow();
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

        private void TextBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (!_settingText)
            {
                _autoFillData.OnTextChanged();
            }
        }

        private void SetDesignText()
        {
            if (DesignerProperties.GetIsInDesignMode(this) && !DesignText.IsNullOrEmpty() && TextBox != null)
            {
                _settingText = true;
                TextBox.Text = DesignText;
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

                if (Popup != null)
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

            var isOpen = false;
            if (Popup != null)
                isOpen = Popup.IsOpen;

            _autoFillData.SetValue(primaryKeyValue, text, isOpen); //Must set to false otherwise list shows when control is tabbed out.
        }

        private void ClearValue()
        {
            if (_autoFillData == null)
                return;

            _autoFillData.ClearValue();
        }

        private void TextBox_PreviewKeyDown(object sender, KeyEventArgs e)
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
                    if (Popup != null && Popup.IsOpen)
                    {
                        Popup.IsOpen = false;
                        e.Handled = true;
                    }

                    break;
                case Key.Down:
                    if (Popup != null && Popup.IsOpen && ListBox != null && ListBox.SelectedIndex < ContainsItems.Count - 1)
                    {
                        ListBox.SelectedIndex++;
                    }
                    break;
                case Key.Up:
                    if (Popup != null && Popup.IsOpen && ListBox != null && ListBox.SelectedIndex > 0)
                        ListBox.SelectedIndex--;
                    break;
                case Key.F5:
                    ShowLookupWindow();
                    break;
            }

        }

        private void TextBox_PreviewTextInput(object sender, TextCompositionEventArgs e)
        {
            if (Keyboard.IsKeyDown(Key.LeftCtrl) || Keyboard.IsKeyDown(Key.RightCtrl))
                return;

            if (Keyboard.IsKeyDown(Key.LeftAlt) || Keyboard.IsKeyDown(Key.RightAlt))
                return;

            if (TextBox.MaxLength > 0 && TextBox.SelectionLength <= 0 &&
                TextBox.Text.Length >= TextBox.MaxLength)
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
            var initialText = string.Empty;
            if (TextBox != null)
                initialText = TextBox.Text;

            var lookupWindow = LookupControlsGlobals.LookupWindowFactory.CreateLookupWindow(Setup.LookupDefinition,
                Setup.AllowLookupAdd, Setup.AllowLookupView, initialText);
            lookupWindow.AddViewParameter = Setup.AddViewParameter;

            lookupWindow.Owner = Window.GetWindow(this);
            lookupWindow.RefreshData += (o, args) =>
            {
                var popupIsOpen = false;
                if (Popup != null)
                    popupIsOpen = Popup.IsOpen;

                _autoFillData.RefreshData(popupIsOpen);
            };
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
