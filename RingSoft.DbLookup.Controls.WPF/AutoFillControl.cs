using System;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Data;
using System.Linq;
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
    public class ContainsSource : ObservableCollection<AutoFillContainsItem>
    {
        public void UpdateSource()
        {
            OnCollectionChanged(new NotifyCollectionChangedEventArgs(NotifyCollectionChangedAction.Reset));
        }

    }
    public class LookupShownArgs
    {
        public LookupWindow LookupWindow { get; set; }
    }
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
    public class AutoFillControl : Control, IAutoFillControl, IReadOnlyControl
    {
        public TextBox TextBox { get; set; }

        public Button Button { get; set; }

        public Popup Popup { get; set; }

        public ListBox ListBox { get; set; }

        public ContainsSource ContainsItems { get; }

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
            get { return (AutoFillSetup) GetValue(SetupProperty); }
            set { SetValue(SetupProperty, value); }
        }

        private static void SetupChangedCallback(DependencyObject obj,
            DependencyPropertyChangedEventArgs args)
        {
            var autoFillControl = (AutoFillControl) obj;
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
            get { return (AutoFillValue) GetValue(ValueProperty); }
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
            autoFillControl.CheckButton();
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
            get { return (bool) GetValue(IsDirtyProperty); }
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
            get { return (bool) GetValue(TabOutAfterLookupSelectProperty); }
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
            get { return (bool) GetValue(ShowContainsBoxProperty); }
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
            get { return (int) GetValue(ContainsBoxMaxRowsProperty); }
            set { SetValue(ContainsBoxMaxRowsProperty, value); }
        }

        public static readonly DependencyProperty DesignTextProperty =
            DependencyProperty.Register("DesignText", typeof(string), typeof(AutoFillControl),
                new FrameworkPropertyMetadata(DesignTextChangedCallback));

        public string DesignText
        {
            get { return (string) GetValue(DesignTextProperty); }
            set { SetValue(DesignTextProperty, value); }
        }

        private static void DesignTextChangedCallback(DependencyObject obj,
            DependencyPropertyChangedEventArgs args)
        {
            var autoFillControl = (AutoFillControl) obj;
            autoFillControl.SetDesignText();
        }

        public static readonly DependencyProperty CharacterCasingProperty =
            DependencyProperty.Register("CharacterCasing", typeof(CharacterCasing), typeof(AutoFillControl),
                new FrameworkPropertyMetadata(CharacterCasingChangedCallback));

        public CharacterCasing CharacterCasing
        {
            get { return (CharacterCasing) GetValue(CharacterCasingProperty); }
            set { SetValue(CharacterCasingProperty, value); }
        }

        private static void CharacterCasingChangedCallback(DependencyObject obj,
            DependencyPropertyChangedEventArgs args)
        {
            var autoFillControl = (AutoFillControl) obj;

            if (autoFillControl.TextBox != null)
                autoFillControl.TextBox.CharacterCasing = autoFillControl.CharacterCasing;
        }

        public static readonly DependencyProperty TextAlignmentProperty =
            DependencyProperty.Register(nameof(TextAlignment), typeof(TextAlignment), typeof(AutoFillControl),
                new FrameworkPropertyMetadata(TextAlignmentChangedCallback));

        public TextAlignment TextAlignment
        {
            get { return (TextAlignment) GetValue(TextAlignmentProperty); }
            set { SetValue(TextAlignmentProperty, value); }
        }

        private static void TextAlignmentChangedCallback(DependencyObject obj,
            DependencyPropertyChangedEventArgs args)
        {
            var autoFillControl = (AutoFillControl) obj;
            if (autoFillControl.TextBox != null)
                autoFillControl.TextBox.TextAlignment = autoFillControl.TextAlignment;
        }

        private static void BorderThicknessChangedCallback(DependencyObject obj,
            DependencyPropertyChangedEventArgs args)
        {
            var autoFillControl = (AutoFillControl) obj;
            if (autoFillControl.TextBox != null)
            {
                autoFillControl.TextBox.BorderThickness = autoFillControl.BorderThickness;
            }
        }

        private static void BackgroundChangedCallback(DependencyObject obj,
            DependencyPropertyChangedEventArgs args)
        {
            var autoFillControl = (AutoFillControl) obj;
            if (autoFillControl.TextBox != null)
            {
                autoFillControl.TextBox.Background = autoFillControl.Background;
            }
        }

        private static void HeightChangedCallback(DependencyObject obj,
            DependencyPropertyChangedEventArgs args)
        {
            var autoFillControl = (AutoFillControl)obj;
            if (autoFillControl.TextBox != null)
            {
                var height = autoFillControl.Height;
                if (height > autoFillControl.ActualHeight)
                {
                    height = autoFillControl.ActualHeight;
                }
                autoFillControl.TextBox.Height = height;
                if (autoFillControl.Button != null)
                {
                    autoFillControl.Button.Height = height;
                }
            }
        }


        private static void ForegroundChangedCallback(DependencyObject obj,
            DependencyPropertyChangedEventArgs args)
        {
            var autoFillControl = (AutoFillControl) obj;
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
            get { return (Brush) GetValue(SelectionBrushProperty); }
            set { SetValue(SelectionBrushProperty, value); }
        }

        private static void SelectionBrushChangedCallback(DependencyObject obj,
            DependencyPropertyChangedEventArgs args)
        {
            var autoFillControl = (AutoFillControl) obj;
            if (autoFillControl.TextBox != null)
                autoFillControl.TextBox.SelectionBrush = autoFillControl.SelectionBrush;
        }

        public static new readonly DependencyProperty RsIsTabStopProperty =
            DependencyProperty.Register(nameof(RsIsTabStop), typeof(bool), typeof(AutoFillControl),
                new FrameworkPropertyMetadata(true, RsIsTabStopChangedCallback));

        public new bool RsIsTabStop
        {
            get { return (bool)GetValue(RsIsTabStopProperty); }
            set { SetValue(RsIsTabStopProperty, value); }
        }

        private static void RsIsTabStopChangedCallback(DependencyObject obj,
            DependencyPropertyChangedEventArgs args)
        {
            var autoFillControl = (AutoFillControl)obj;
            if (autoFillControl.TextBox != null)
            {
                autoFillControl.TextBox.IsTabStop = autoFillControl.RsIsTabStop;
            }
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

        public void RefreshValue(AutoFillValue autoFillValue)
        {
            if (Value.PrimaryKeyValue.IsEqualTo(autoFillValue.PrimaryKeyValue))
            {
                AutoFillDataMaui.SetValue(autoFillValue.PrimaryKeyValue, autoFillValue.Text, false);
                Value = autoFillValue;
            }
        }

        public void OnSelect()
        {
            if (TabOutAfterLookupSelect)
            {
                Send(Key.Tab);
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

        private bool _readOnlyMode;

        public bool ReadOnlyMode
        {
            get => _readOnlyMode;
            set
            {
                _readOnlyMode = value;
                SetReadOnlyMode(_readOnlyMode);
            }
        }

        public AutoFillDataMauiBase AutoFillDataMaui { get; private set; }


        public event EventHandler ControlDirty;
        public event EventHandler LookupSelect;
        public event EventHandler<LookupShownArgs> LookupShown;
        public event EventHandler AutoFillLostFocus;

        //private AutoFillData _autoFillData;
        private bool _controlLoaded;
        private bool _onAutoFillDataChanged;
        private bool _onValuePropertySetting;
        private bool _pendingAutoFillData;
        private bool _setupRan;
        private bool _settingText;
        private AutoFillValue _pendingAutoFillValue;
        private bool _pendingSendTab = false;

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

            HeightProperty.OverrideMetadata(typeof(AutoFillControl),
                new FrameworkPropertyMetadata(HeightChangedCallback));

        }

        public AutoFillControl()
        {
            ContainsItems = new ContainsSource();
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
                AutoFillLostFocus?.Invoke(this, EventArgs.Empty);
            };
            GotFocus += AutoFillControl_GotFocus;
            ContextMenu = new ContextMenu();
            ContextMenu.AddTextBoxContextMenuItems();
            MouseEnter += (sender, args) =>
            {
                if (_readOnlyMode)
                {
                    Mouse.OverrideCursor = Cursors.Arrow;
                }
                else
                {
                    Mouse.OverrideCursor = null;
                }
            };
        }

        private void AutoFillControl_GotFocus(object sender, RoutedEventArgs e)
        {
            if (ReadOnlyMode)
            {
                if (Button != null)
                {
                        Button.Focus();
                }
            }
            else
            {
                if (TextBox != null)
                {
                    TextBox.Focus();
                    TextBox.SelectionStart = 0;
                    TextBox.SelectionLength = TextBox.Text.Length;
                }
            }
        }


        public override void OnApplyTemplate()
        {
            TextBox = GetTemplateChild(nameof(TextBox)) as TextBox;
            Button = GetTemplateChild(nameof(Button)) as Button;
            Popup = GetTemplateChild(nameof(Popup)) as Popup;
            ListBox = GetTemplateChild(nameof(ListBox)) as ListBox;

            if (TextBox != null)
                TextBox.ContextMenu = ContextMenu;

            SetDesignText();

            CheckButton();

            if (_controlLoaded && Setup != null)
            {
                _setupRan = false;
                CheckButton();
                Setup.SetDirty = false;
                SetupControl();
                CreateContainsTemplate();

                if (_pendingAutoFillValue != null)
                {
                    Value = _pendingAutoFillValue;
                }
                Setup.SetDirty = true;
            }

            if (TextBox != null)
            {
                if (Foreground != null)
                    TextBox.Foreground = Foreground;

                if (Background != null)
                    TextBox.Background = Background;

                if (SelectionBrush != null)
                    TextBox.SelectionBrush = SelectionBrush;

                TextBox.IsTabStop = RsIsTabStop;
            }

            SetReadOnlyMode(_readOnlyMode);
            Button.KeyDown += Button_KeyDown;

            base.OnApplyTemplate();
        }

        private void CheckButton()
        {
            if (Setup != null && _readOnlyMode && Button != null)
            {
                if (Setup.LookupDefinition.TableDefinition.CanViewTable)
                {
                    if (Button != null)
                    {
                        Button.Visibility = Visibility.Visible;
                    }
                }
                else
                {
                    if (Button != null)
                    {
                        Button.Visibility = Visibility.Collapsed;
                    }
                }
            }
        }

        private void Button_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.F5)
            {
                ShowLookupWindow();
                e.Handled = true;
            }
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
            var window = Window.GetWindow(this);
        }

        public new bool Focus()
        {
            base.Focus();
            return IsKeyboardFocusWithin;
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

            var style = new Style(typeof(ListBoxItem));
            var eventSetter = new EventSetter(ListBoxItem.PreviewMouseLeftButtonDownEvent,
                new MouseButtonEventHandler(ListBox_PreviewMouseLeftButtonDown));
            style.Setters.Add(eventSetter);
            ListBox.ItemContainerStyle = style;
        }

        private void ListBox_PreviewMouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            ListBoxItem listBoxItem = sender as ListBoxItem;
            ListBox.SelectedItem = listBoxItem?.DataContext;
        }

        private void SetupControl()
        {
            if (Setup.LookupDefinition == null || Setup.LookupDefinition.InitialSortColumnDefinition == null)
                throw new ArgumentException(
                    "Lookup definition does not have any visible columns defined or its initial sort column is null.");

            CheckButton();
            if (AutoFillDataMaui != null)
                ClearValue();

            //_autoFillData = new AutoFillData(this, Setup.LookupDefinition, Setup.Distinct)
            //{
            //    ShowContainsBox = ShowContainsBox,
            //    ContainsBoxMaxRows = ContainsBoxMaxRows
            //};
            AutoFillDataMaui = Setup.LookupDefinition.TableDefinition.LookupDefinition.GetAutoFillDataMaui(Setup, this);

            if (Setup.LookupDefinition.InitialSortColumnDefinition is LookupFieldColumnDefinition fieldColumn)
            {
                if (fieldColumn.FieldDefinition is StringFieldDefinition stringField && TextBox != null)
                    TextBox.MaxLength = stringField.MaxLength;
            }

            AutoFillDataMaui.OutputDataChanged += AutoFillDataMaui_OutputDataChanged;

            //_autoFillData.AutoFillDataChanged += AutoFillData_AutoFillDataChanged;

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
                        //_autoFillData.OnChangeContainsIndex(ListBox.SelectedIndex);
                    };
                }

                if (Button != null)
                    Button.Click += (sender, args) => ShowLookupWindow();
            }

            if (_pendingAutoFillData)
            {
                _onValuePropertySetting = true;
                if (Value != null) SetValue(Value.PrimaryKeyValue, Value.Text);
                _onValuePropertySetting = false;
                _pendingAutoFillData = false;
            }

            _setupRan = true;
        }

        private void AutoFillDataMaui_OutputDataChanged(object sender, AutoFillOutputData e)
        {
            var openPopup = false;
            ContainsItems.Clear();

            if (e.ContainsData != null)
            {
                foreach (var contItem in e.ContainsData)
                {
                    ContainsItems.Add(AutoFillDataMaui.GetAutoFillContainsItem(contItem, e.BeginText));
                    openPopup = true;
                }
            }

            if (Popup != null)
                Popup.IsOpen = openPopup;

            if (openPopup)
            {
                ListBox.ItemsSource = ContainsItems;
                ContainsItems.UpdateSource();
            }

            //if (e.AutoFillValue != null)
            {
                _onAutoFillDataChanged = true;
                Value = e.AutoFillValue;
                EditText = Value?.Text;
                _onAutoFillDataChanged = false;
            }
        }

        private void TextBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (!_settingText)
            {
                //_autoFillData.OnTextChanged();
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
                //if (_autoFillData.ShowContainsBox && e.ContainsBoxDataTable != null)
                //{
                //    foreach (DataRow dataRow in e.ContainsBoxDataTable.Rows)
                //    {
                //        ContainsItems.Add(_autoFillData.GetAutoFillContainsItem(dataRow));
                //        openPopup = true;
                //    }
                //}

                if (Popup != null)
                    Popup.IsOpen = openPopup;
            }

            if (!_onValuePropertySetting)
            {
                _onAutoFillDataChanged = true;
                //var newData = new AutoFillValue(_autoFillData.PrimaryKeyValue, EditText);
                //Value = newData;
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
                if (AutoFillDataMaui == null)
                    _pendingAutoFillData = true;

                if (TextBox == null)
                    _pendingAutoFillValue = Value;
                else
                    SetValue(Value.PrimaryKeyValue, Value.Text);
            }
        }

        private void SetValue(PrimaryKeyValue primaryKeyValue, string text)
        {
            if (AutoFillDataMaui == null)
            {
                return;
            }

            var isOpen = false;
            if (Popup != null)
                isOpen = Popup.IsOpen;

            AutoFillDataMaui.SetValue(primaryKeyValue, text,
                isOpen); //Must set to false otherwise list shows when control is tabbed out.
        }

        private void ClearValue()
        {
            TextBox.Text = string.Empty;
            Popup.IsOpen = false;
        }

        private void TextBox_PreviewKeyDown(object sender, KeyEventArgs e)
        {
            if (_readOnlyMode && e.Key != Key.F5)
                return;
            
            switch (e.Key)
            {
                case Key.Back:
                    AutoFillDataMaui.OnBackspaceKeyDown();
                    e.Handled = true;
                    RaiseDirtyFlag();
                    break;
                case Key.Delete:
                    AutoFillDataMaui.OnDeleteKeyDown();
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
                    if (Popup != null && Popup.IsOpen && ListBox != null &&
                        ListBox.SelectedIndex < ContainsItems.Count - 1)
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
            if (_readOnlyMode)
                return;

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

            if (e.Text == "\t")
            {
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

            AutoFillDataMaui.OnKeyCharPressed(newText[0]);
            e.Handled = true;
            RaiseDirtyFlag();
        }

        public virtual void ShowLookupWindow()
        {
            if (Button == null || Button.Visibility != Visibility.Visible)
            {
                return;
            }
            var initialText = string.Empty;
            if (TextBox != null)
                initialText = TextBox.Text;

            if (Setup.LookupDefinition.InitialOrderByColumn != Setup.LookupDefinition.InitialSortColumnDefinition &&
                !initialText.IsNullOrEmpty() && Value.PrimaryKeyValue.IsValid)
            {
                initialText = Setup.LookupDefinition.InitialOrderByColumn.GetTextForColumn(Value.PrimaryKeyValue);
            }
            PrimaryKeyValue readOnlyPrimaryKeyValue = null;
            if (_readOnlyMode && Value != null)
            {
                readOnlyPrimaryKeyValue = Value.PrimaryKeyValue;
            }
            var lookupWindow = LookupControlsGlobals.LookupWindowFactory.CreateLookupWindow(
                Setup.LookupDefinition
                , Setup.AllowLookupAdd
                , Setup.AllowLookupView
                , initialText
                , Value?.PrimaryKeyValue
                , this
                , readOnlyPrimaryKeyValue);
            lookupWindow.AddViewParameter = Setup.AddViewParameter;

            if (Popup != null)
                Popup.IsOpen = false;

            lookupWindow.Owner = Window.GetWindow(this);
            lookupWindow.RefreshData += (o, args) =>
            {
                if (Value != null)
                {
                    var value = Value.PrimaryKeyValue.KeyValueFields[0].Value;
                    Value =
                        Setup.LookupDefinition.TableDefinition.Context.OnAutoFillTextRequest(
                            Setup.LookupDefinition.TableDefinition, value);

                    if (Value != null)
                    {
                        EditText = Value.Text;
                    }
                }

                //_autoFillData.RefreshData(popupIsOpen);
            };
            lookupWindow.LookupSelect += LookupForm_LookupSelect;
            lookupWindow.SetReadOnlyMode(_readOnlyMode);
            lookupWindow.ApplyNewLookup += (sender, args) => Setup.LookupDefinition = lookupWindow.LookupDefinition;
            LookupShown?.Invoke(this, new LookupShownArgs(){LookupWindow = lookupWindow});
            lookupWindow.Closed += (sender, args) =>
            {
                Window.GetWindow(this).Activate();
            };
            lookupWindow.Show();
        }

        private void LookupForm_LookupSelect(object sender, LookupSelectArgs e)
        { 
            var text = e.LookupData.GetSelectedText();
            SetValue(e.LookupData.SelectedPrimaryKeyValue, text);
            RaiseDirtyFlag();
            LookupSelect?.Invoke(this, new EventArgs());

            AutoFillDataMaui.OnLookupSelect(e.LookupData.GetSelectedPrimaryKeyValue());
            OnSelect();
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
        
        public void SetReadOnlyMode(bool readOnlyValue)
        {
            _readOnlyMode = readOnlyValue;

            if (TextBox != null)
            {
                TextBox.IsReadOnly = readOnlyValue;
                TextBox.Focusable = !readOnlyValue;
            }

            if (!readOnlyValue && TextBox != null)
            {
                if (Button != null && Button.IsFocused)
                {
                    TextBox.Focus();
                }
            }

            //if (IsFocused)
            //{
            //    AutoFillControl_GotFocus(this, new RoutedEventArgs());
            //}
        }

        protected override void OnKeyDown(KeyEventArgs e)
        {
            if (e.Key == Key.Tab)
            {
                var predictionElement = TextBox.PredictFocus(FocusNavigationDirection.Up);
                {
                    if (predictionElement == TextBox)
                    {
                        Popup.IsOpen = false;
                        AutoFillLostFocus?.Invoke(this, EventArgs.Empty);
                    }
                }
            }
        }
    }
}
