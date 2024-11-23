// ***********************************************************************
// Assembly         : RingSoft.DbLookup.Controls.WPF
// Author           : petem
// Created          : 12-19-2022
//
// Last Modified By : petem
// Last Modified On : 10-29-2023
// ***********************************************************************
// <copyright file="AutoFillControl.cs" company="Peter Ringering">
//     Copyright (c) 2023. All rights reserved.
// </copyright>
// <summary></summary>
// ***********************************************************************
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
    /// <summary>
    /// Class ContainsSource.
    /// Implements the <see cref="System.Collections.ObjectModel.ObservableCollection{RingSoft.DbLookup.AutoFill.AutoFillContainsItem}" />
    /// </summary>
    /// <seealso cref="System.Collections.ObjectModel.ObservableCollection{RingSoft.DbLookup.AutoFill.AutoFillContainsItem}" />
    public class ContainsSource : ObservableCollection<AutoFillContainsItem>
    {
        /// <summary>
        /// Updates the source.
        /// </summary>
        public void UpdateSource()
        {
            OnCollectionChanged(new NotifyCollectionChangedEventArgs(NotifyCollectionChangedAction.Reset));
        }

    }
    /// <summary>
    /// Class LookupShownArgs.
    /// </summary>
    public class LookupShownArgs
    {
        /// <summary>
        /// Gets or sets the lookup window.
        /// </summary>
        /// <value>The lookup window.</value>
        public LookupWindow LookupWindow { get; set; }
    }
    /// <summary>
    /// A textbox control that allows a user to link tables.  Displays a popup of related values to the user
    /// when they type into it.  Displays a lookup window to the user when the find button is clicked.
    /// Implements the <see cref="Control" />
    /// Implements the <see cref="IAutoFillControl" />
    /// Implements the <see cref="IReadOnlyControl" />
    /// </summary>
    /// <seealso cref="Control" />
    /// <seealso cref="IAutoFillControl" />
    /// <seealso cref="IReadOnlyControl" />
    /// <font color="red">Badly formed XML comment.</font>
    [TemplatePart(Name = "TextBox", Type = typeof(TextBox))]
    [TemplatePart(Name = "Button", Type = typeof(Button))]
    [TemplatePart(Name = "Popup", Type = typeof(Popup))]
    [TemplatePart(Name = "ListBox", Type = typeof(ListBox))]
    public class AutoFillControl : Control, IAutoFillControl, IReadOnlyControl
    {
        /// <summary>
        /// Gets or sets the text box.
        /// </summary>
        /// <value>The text box.</value>
        public TextBox TextBox { get; set; }

        /// <summary>
        /// Gets or sets the button.
        /// </summary>
        /// <value>The button.</value>
        public Button Button { get; set; }

        /// <summary>
        /// Gets or sets the popup.
        /// </summary>
        /// <value>The popup.</value>
        public Popup Popup { get; set; }

        /// <summary>
        /// Gets or sets the ListBox.
        /// </summary>
        /// <value>The ListBox.</value>
        public ListBox ListBox { get; set; }

        /// <summary>
        /// Gets the contains items.
        /// </summary>
        /// <value>The contains items.</value>
        public ContainsSource ContainsItems { get; }

        /// <summary>
        /// The setup property
        /// </summary>
        public static readonly DependencyProperty SetupProperty =
            DependencyProperty.Register("Setup", typeof(AutoFillSetup), typeof(AutoFillControl),
                new FrameworkPropertyMetadata(SetupChangedCallback));

        /// <summary>
        /// Gets or sets the AutoFillSetup to determine how this control will behave.
        /// </summary>
        /// <value>The AutoFillSetup.</value>
        public AutoFillSetup Setup
        {
            get { return (AutoFillSetup) GetValue(SetupProperty); }
            set { SetValue(SetupProperty, value); }
        }

        /// <summary>
        /// Setups the changed callback.
        /// </summary>
        /// <param name="obj">The object.</param>
        /// <param name="args">The <see cref="DependencyPropertyChangedEventArgs"/> instance containing the event data.</param>
        private static void SetupChangedCallback(DependencyObject obj,
            DependencyPropertyChangedEventArgs args)
        {
            var autoFillControl = (AutoFillControl) obj;
            if (autoFillControl._controlLoaded)
                autoFillControl.SetupControl();
        }

        /// <summary>
        /// The value property
        /// </summary>
        public static readonly DependencyProperty ValueProperty =
            DependencyProperty.Register("Value", typeof(AutoFillValue), typeof(AutoFillControl),
                new FrameworkPropertyMetadata(ValueChangedCallback));

        /// <summary>
        /// Gets or sets the AutoFillValue.
        /// </summary>
        /// <value>The value.</value>
        public AutoFillValue Value
        {
            get { return (AutoFillValue) GetValue(ValueProperty); }
            set { SetValue(ValueProperty, value); }
        }

        /// <summary>
        /// Values the changed callback.
        /// </summary>
        /// <param name="obj">The object.</param>
        /// <param name="args">The <see cref="DependencyPropertyChangedEventArgs"/> instance containing the event data.</param>
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

        /// <summary>
        /// The is dirty property
        /// </summary>
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
        /// <value><c>true</c> if this control's value has changed; otherwise, <c>false</c>.</value>
        public bool IsDirty
        {
            get { return (bool) GetValue(IsDirtyProperty); }
            set { SetValue(IsDirtyProperty, value); }
        }

        /// <summary>
        /// The tab out after lookup select property
        /// </summary>
        public static readonly DependencyProperty TabOutAfterLookupSelectProperty =
            DependencyProperty.Register("TabOutAfterLookupSelect", typeof(bool), typeof(AutoFillControl));

        /// <summary>
        /// Gets or sets a value indicating whether to automatically tab out after the user selects a record in the LookupWindow.
        /// </summary>
        /// <value><c>true</c> if to tab out after lookup select; otherwise, <c>false</c>.</value>
        public bool TabOutAfterLookupSelect
        {
            get { return (bool) GetValue(TabOutAfterLookupSelectProperty); }
            set { SetValue(TabOutAfterLookupSelectProperty, value); }
        }

        /// <summary>
        /// The show contains box property
        /// </summary>
        public static readonly DependencyProperty ShowContainsBoxProperty =
            DependencyProperty.Register("ShowContainsBox", typeof(bool), typeof(AutoFillControl));

        /// <summary>
        /// Gets or sets a value indicating whether to show the contains box.
        /// </summary>
        /// <value><c>true</c> if to show the contains box; otherwise, <c>false</c>.</value>
        public bool ShowContainsBox
        {
            get { return (bool) GetValue(ShowContainsBoxProperty); }
            set { SetValue(ShowContainsBoxProperty, value); }
        }

        /// <summary>
        /// The contains box maximum rows property
        /// </summary>
        public static readonly DependencyProperty ContainsBoxMaxRowsProperty =
            DependencyProperty.Register("ContainsBoxMaxRows", typeof(int), typeof(AutoFillControl));

        /// <summary>
        /// Gets or sets the contains box maximum number of rows.
        /// </summary>
        /// <value>The contains box maximum number of rows.</value>
        public int ContainsBoxMaxRows
        {
            get { return (int) GetValue(ContainsBoxMaxRowsProperty); }
            set { SetValue(ContainsBoxMaxRowsProperty, value); }
        }

        /// <summary>
        /// The design text property
        /// </summary>
        public static readonly DependencyProperty DesignTextProperty =
            DependencyProperty.Register("DesignText", typeof(string), typeof(AutoFillControl),
                new FrameworkPropertyMetadata(DesignTextChangedCallback));

        /// <summary>
        /// Gets or sets the design text.
        /// </summary>
        /// <value>The design text.</value>
        public string DesignText
        {
            get { return (string) GetValue(DesignTextProperty); }
            set { SetValue(DesignTextProperty, value); }
        }

        /// <summary>
        /// Designs the text changed callback.
        /// </summary>
        /// <param name="obj">The object.</param>
        /// <param name="args">The <see cref="DependencyPropertyChangedEventArgs"/> instance containing the event data.</param>
        private static void DesignTextChangedCallback(DependencyObject obj,
            DependencyPropertyChangedEventArgs args)
        {
            var autoFillControl = (AutoFillControl) obj;
            autoFillControl.SetDesignText();
        }

        /// <summary>
        /// The character casing property
        /// </summary>
        public static readonly DependencyProperty CharacterCasingProperty =
            DependencyProperty.Register("CharacterCasing", typeof(CharacterCasing), typeof(AutoFillControl),
                new FrameworkPropertyMetadata(CharacterCasingChangedCallback));

        /// <summary>
        /// Gets or sets the character casing.
        /// </summary>
        /// <value>The character casing.</value>
        public CharacterCasing CharacterCasing
        {
            get { return (CharacterCasing) GetValue(CharacterCasingProperty); }
            set { SetValue(CharacterCasingProperty, value); }
        }

        /// <summary>
        /// Characters the casing changed callback.
        /// </summary>
        /// <param name="obj">The object.</param>
        /// <param name="args">The <see cref="DependencyPropertyChangedEventArgs"/> instance containing the event data.</param>
        private static void CharacterCasingChangedCallback(DependencyObject obj,
            DependencyPropertyChangedEventArgs args)
        {
            var autoFillControl = (AutoFillControl) obj;

            if (autoFillControl.TextBox != null)
                autoFillControl.TextBox.CharacterCasing = autoFillControl.CharacterCasing;
        }

        /// <summary>
        /// The text alignment property
        /// </summary>
        public static readonly DependencyProperty TextAlignmentProperty =
            DependencyProperty.Register(nameof(TextAlignment), typeof(TextAlignment), typeof(AutoFillControl),
                new FrameworkPropertyMetadata(TextAlignmentChangedCallback));

        /// <summary>
        /// Gets or sets the text alignment.
        /// </summary>
        /// <value>The text alignment.</value>
        public TextAlignment TextAlignment
        {
            get { return (TextAlignment) GetValue(TextAlignmentProperty); }
            set { SetValue(TextAlignmentProperty, value); }
        }

        /// <summary>
        /// Texts the alignment changed callback.
        /// </summary>
        /// <param name="obj">The object.</param>
        /// <param name="args">The <see cref="DependencyPropertyChangedEventArgs"/> instance containing the event data.</param>
        private static void TextAlignmentChangedCallback(DependencyObject obj,
            DependencyPropertyChangedEventArgs args)
        {
            var autoFillControl = (AutoFillControl) obj;
            if (autoFillControl.TextBox != null)
                autoFillControl.TextBox.TextAlignment = autoFillControl.TextAlignment;
        }

        /// <summary>
        /// Borders the thickness changed callback.
        /// </summary>
        /// <param name="obj">The object.</param>
        /// <param name="args">The <see cref="DependencyPropertyChangedEventArgs"/> instance containing the event data.</param>
        private static void BorderThicknessChangedCallback(DependencyObject obj,
            DependencyPropertyChangedEventArgs args)
        {
            var autoFillControl = (AutoFillControl) obj;
            if (autoFillControl.TextBox != null)
            {
                autoFillControl.TextBox.BorderThickness = autoFillControl.BorderThickness;
            }
        }

        /// <summary>
        /// Backgrounds the changed callback.
        /// </summary>
        /// <param name="obj">The object.</param>
        /// <param name="args">The <see cref="DependencyPropertyChangedEventArgs"/> instance containing the event data.</param>
        private static void BackgroundChangedCallback(DependencyObject obj,
            DependencyPropertyChangedEventArgs args)
        {
            var autoFillControl = (AutoFillControl) obj;
            if (autoFillControl.TextBox != null)
            {
                autoFillControl.TextBox.Background = autoFillControl.Background;
            }
        }

        /// <summary>
        /// Heights the changed callback.
        /// </summary>
        /// <param name="obj">The object.</param>
        /// <param name="args">The <see cref="DependencyPropertyChangedEventArgs"/> instance containing the event data.</param>
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


        /// <summary>
        /// Foregrounds the changed callback.
        /// </summary>
        /// <param name="obj">The object.</param>
        /// <param name="args">The <see cref="DependencyPropertyChangedEventArgs"/> instance containing the event data.</param>
        private static void ForegroundChangedCallback(DependencyObject obj,
            DependencyPropertyChangedEventArgs args)
        {
            var autoFillControl = (AutoFillControl) obj;
            if (autoFillControl.TextBox != null)
            {
                autoFillControl.TextBox.Foreground = autoFillControl.Foreground;
            }
        }

        /// <summary>
        /// The selection brush property
        /// </summary>
        public static readonly DependencyProperty SelectionBrushProperty =
            DependencyProperty.Register(nameof(SelectionBrush), typeof(Brush), typeof(AutoFillControl),
                new FrameworkPropertyMetadata(SelectionBrushChangedCallback));

        /// <summary>
        /// Gets or sets the selection brush.
        /// </summary>
        /// <value>The selection brush.</value>
        public Brush SelectionBrush
        {
            get { return (Brush) GetValue(SelectionBrushProperty); }
            set { SetValue(SelectionBrushProperty, value); }
        }

        /// <summary>
        /// Selections the brush changed callback.
        /// </summary>
        /// <param name="obj">The object.</param>
        /// <param name="args">The <see cref="DependencyPropertyChangedEventArgs"/> instance containing the event data.</param>
        private static void SelectionBrushChangedCallback(DependencyObject obj,
            DependencyPropertyChangedEventArgs args)
        {
            var autoFillControl = (AutoFillControl) obj;
            if (autoFillControl.TextBox != null)
                autoFillControl.TextBox.SelectionBrush = autoFillControl.SelectionBrush;
        }

        /// <summary>
        /// The rs is tab stop property
        /// </summary>
        public static new readonly DependencyProperty RsIsTabStopProperty =
            DependencyProperty.Register(nameof(RsIsTabStop), typeof(bool), typeof(AutoFillControl),
                new FrameworkPropertyMetadata(true, RsIsTabStopChangedCallback));

        /// <summary>
        /// Gets or sets a value indicating whether [rs is tab stop].
        /// </summary>
        /// <value><c>true</c> if [rs is tab stop]; otherwise, <c>false</c>.</value>
        public new bool RsIsTabStop
        {
            get { return (bool)GetValue(RsIsTabStopProperty); }
            set { SetValue(RsIsTabStopProperty, value); }
        }

        /// <summary>
        /// Rses the is tab stop changed callback.
        /// </summary>
        /// <param name="obj">The object.</param>
        /// <param name="args">The <see cref="DependencyPropertyChangedEventArgs"/> instance containing the event data.</param>
        private static void RsIsTabStopChangedCallback(DependencyObject obj,
            DependencyPropertyChangedEventArgs args)
        {
            var autoFillControl = (AutoFillControl)obj;
            if (autoFillControl.TextBox != null)
            {
                autoFillControl.TextBox.IsTabStop = autoFillControl.RsIsTabStop;
            }
        }

        /// <summary>
        /// The UI command property
        /// </summary>
        public static readonly DependencyProperty UiCommandProperty =
            DependencyProperty.Register(nameof(UiCommand), typeof(UiCommand), typeof(AutoFillControl),
                new FrameworkPropertyMetadata(UiCommandChangedCallback));

        /// <summary>
        /// Gets or sets the UI command.
        /// </summary>
        /// <value>The UI command.</value>
        public UiCommand UiCommand
        {
            get { return (UiCommand)GetValue(UiCommandProperty); }
            set { SetValue(UiCommandProperty, value); }
        }

        /// <summary>
        /// UIs the command changed callback.
        /// </summary>
        /// <param name="obj">The object.</param>
        /// <param name="args">The <see cref="DependencyPropertyChangedEventArgs"/> instance containing the event data.</param>
        private static void UiCommandChangedCallback(DependencyObject obj,
            DependencyPropertyChangedEventArgs args)
        {
            var autoFillControl = (AutoFillControl)obj;
            if (autoFillControl._vmUiControl == null)
            {
                autoFillControl._vmUiControl = WPFControlsGlobals.VmUiFactory.CreateUiControl(
                    autoFillControl, autoFillControl.UiCommand);
                if (autoFillControl.UiLabel != null)
                {
                    autoFillControl._vmUiControl.SetLabel(autoFillControl.UiLabel);
                }
            }
        }

        /// <summary>
        /// The UI label property
        /// </summary>
        public static readonly DependencyProperty UiLabelProperty =
            DependencyProperty.Register(nameof(UiLabel), typeof(Label), typeof(AutoFillControl),
                new FrameworkPropertyMetadata(UiLabelChangedCallback));

        /// <summary>
        /// Gets or sets the UI label.
        /// </summary>
        /// <value>The UI label.</value>
        public Label UiLabel
        {
            get { return (Label)GetValue(UiLabelProperty); }
            set { SetValue(UiLabelProperty, value); }
        }

        /// <summary>
        /// UIs the label changed callback.
        /// </summary>
        /// <param name="obj">The object.</param>
        /// <param name="args">The <see cref="DependencyPropertyChangedEventArgs"/> instance containing the event data.</param>
        private static void UiLabelChangedCallback(DependencyObject obj,
            DependencyPropertyChangedEventArgs args)
        {
            var autoFillControl = (AutoFillControl)obj;
            if (autoFillControl._vmUiControl != null)
                autoFillControl._vmUiControl.SetLabel(autoFillControl.UiLabel);
        }

        /// <summary>
        /// Gets or sets the edit text.
        /// </summary>
        /// <value>The edit text.</value>
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

        /// <summary>
        /// Gets or sets the selection start.
        /// </summary>
        /// <value>The selection start.</value>
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

        /// <summary>
        /// Gets or sets the length of the selection.
        /// </summary>
        /// <value>The length of the selection.</value>
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

        /// <summary>
        /// Refreshes the value.
        /// </summary>
        /// <param name="token">The token.</param>
        public void RefreshValue(LookupCallBackToken token)
        {
            if (token.RefreshMode == AutoFillRefreshModes.DbDelete)
            {
                if (Value != null && Value.PrimaryKeyValue.IsEqualTo(token.DeletedPrimaryKeyValue))
                {
                    AutoFillDataMaui.SetValue(null, string.Empty, false);
                    Value = null;
                    RaiseDirtyFlag();
                }

                return;
            }
            var process = token.NewAutoFillValue != null;
            if (process)
            {
                if (token.RefreshMode == AutoFillRefreshModes.PkRefresh)
                {
                    process = Value != null && Value.PrimaryKeyValue.IsEqualTo(token.NewAutoFillValue.PrimaryKeyValue);
                }
            }
            if (process)
            {
                AutoFillDataMaui.SetValue(token.NewAutoFillValue.PrimaryKeyValue, token.NewAutoFillValue.Text, false);
                Value = token.NewAutoFillValue;
                RaiseDirtyFlag();
            }
        }

        /// <summary>
        /// Called when [select].
        /// </summary>
        public void OnSelect()
        {
            RaiseDirtyFlag();
            if (TabOutAfterLookupSelect)
            {
                Send(Key.Tab);
            }
        }

        /// <summary>
        /// Gets a value indicating whether [contains box is open].
        /// </summary>
        /// <value><c>true</c> if [contains box is open]; otherwise, <c>false</c>.</value>
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

        /// <summary>
        /// The read only mode
        /// </summary>
        private bool _readOnlyMode;

        /// <summary>
        /// Gets or sets a value indicating whether [read only mode].
        /// </summary>
        /// <value><c>true</c> if [read only mode]; otherwise, <c>false</c>.</value>
        public bool ReadOnlyMode
        {
            get => _readOnlyMode;
            set
            {
                _readOnlyMode = value;
                SetReadOnlyMode(_readOnlyMode);
            }
        }

        /// <summary>
        /// Gets the automatic fill data maui.
        /// </summary>
        /// <value>The automatic fill data maui.</value>
        public AutoFillDataMauiBase AutoFillDataMaui { get; private set; }

        /// <summary>
        /// Gets or sets a value indicating whether [allow advanced find].
        /// </summary>
        /// <value><c>true</c> if [allow advanced find]; otherwise, <c>false</c>.</value>
        public bool AllowAdvancedFind { get; set; } = true;


        /// <summary>
        /// Occurs when [control dirty].
        /// </summary>
        public event EventHandler ControlDirty;
        /// <summary>
        /// Occurs when [lookup select].
        /// </summary>
        public event EventHandler LookupSelect;
        /// <summary>
        /// Occurs when [lookup shown].
        /// </summary>
        public event EventHandler<LookupShownArgs> LookupShown;
        /// <summary>
        /// Occurs when [automatic fill lost focus].
        /// </summary>
        public event EventHandler AutoFillLostFocus;

        //private AutoFillData _autoFillData;
        /// <summary>
        /// The control loaded
        /// </summary>
        private bool _controlLoaded;
        /// <summary>
        /// The on automatic fill data changed
        /// </summary>
        private bool _onAutoFillDataChanged;
        /// <summary>
        /// The on value property setting
        /// </summary>
        private bool _onValuePropertySetting;
        /// <summary>
        /// The pending automatic fill data
        /// </summary>
        private bool _pendingAutoFillData;
        /// <summary>
        /// The setup ran
        /// </summary>
        private bool _setupRan;
        /// <summary>
        /// The setting text
        /// </summary>
        private bool _settingText;
        /// <summary>
        /// The pending automatic fill value
        /// </summary>
        private AutoFillValue _pendingAutoFillValue;
        /// <summary>
        /// The pending send tab
        /// </summary>
        private bool _pendingSendTab = false;
        /// <summary>
        /// The vm UI control
        /// </summary>
        private VmUiControl _vmUiControl;

        /// <summary>
        /// Initializes static members of the <see cref="AutoFillControl"/> class.
        /// </summary>
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

        /// <summary>
        /// Initializes a new instance of the <see cref="AutoFillControl"/> class.
        /// </summary>
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

        /// <summary>
        /// Handles the GotFocus event of the AutoFillControl control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="RoutedEventArgs"/> instance containing the event data.</param>
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


        /// <summary>
        /// When overridden in a derived class, is invoked whenever application code or internal processes call <see cref="M:System.Windows.FrameworkElement.ApplyTemplate" />.
        /// </summary>
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
                    //Value = _pendingAutoFillValue;
                    if (TextBox != null && Value != null)
                    {
                        TextBox.Text = Value.Text;
                    }
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

        /// <summary>
        /// Checks the button.
        /// </summary>
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

        /// <summary>
        /// Handles the KeyDown event of the Button control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="KeyEventArgs"/> instance containing the event data.</param>
        private void Button_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.F5)
            {
                ShowLookupWindow();
                e.Handled = true;
            }
        }

        /// <summary>
        /// Called when [load].
        /// </summary>
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

        /// <summary>
        /// Focuses this instance.
        /// </summary>
        /// <returns><see langword="true" /> if keyboard focus and logical focus were set to this element; <see langword="false" /> if only logical focus was set to this element, or if the call to this method did not force the focus to change.</returns>
        public new bool Focus()
        {
            base.Focus();
            return IsKeyboardFocusWithin;
        }

        /// <summary>
        /// Creates the contains template.
        /// </summary>
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

        /// <summary>
        /// Handles the PreviewMouseLeftButtonDown event of the ListBox control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="MouseButtonEventArgs"/> instance containing the event data.</param>
        private void ListBox_PreviewMouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            ListBoxItem listBoxItem = sender as ListBoxItem;
            ListBox.SelectedItem = listBoxItem?.DataContext;
        }

        /// <summary>
        /// Setups the control.
        /// </summary>
        /// <exception cref="System.ArgumentException">Lookup definition does not have any visible columns defined or its initial sort column is null.</exception>
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
                        if (ListBox.SelectedItem is AutoFillContainsItem autoFillContainsItem)
                        {
                            Value = AutoFillDataMaui.OnListBoxChange(autoFillContainsItem);
                        }
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

        /// <summary>
        /// Automatics the fill data maui output data changed.
        /// </summary>
        /// <param name="sender">The sender.</param>
        /// <param name="e">The e.</param>
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

        /// <summary>
        /// Handles the TextChanged event of the TextBox control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="TextChangedEventArgs"/> instance containing the event data.</param>
        private void TextBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (!_settingText)
            {
                Value = AutoFillDataMaui.OnPaste(TextBox.Text);
                TextBox.SelectionStart = TextBox.Text.Length;
                TextBox.SelectionLength = 0;
                RaiseDirtyFlag();
                {
                    
                }
            }
        }

        /// <summary>
        /// Sets the design text.
        /// </summary>
        private void SetDesignText()
        {
            if (DesignerProperties.GetIsInDesignMode(this) && !DesignText.IsNullOrEmpty() && TextBox != null)
            {
                _settingText = true;
                TextBox.Text = DesignText;
                _settingText = false;
            }
        }

        /// <summary>
        /// Automatics the fill data automatic fill data changed.
        /// </summary>
        /// <param name="sender">The sender.</param>
        /// <param name="e">The e.</param>
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

        /// <summary>
        /// Sets the value.
        /// </summary>
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

        /// <summary>
        /// Sets the value.
        /// </summary>
        /// <param name="primaryKeyValue">The primary key value.</param>
        /// <param name="text">The text.</param>
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

        /// <summary>
        /// Clears the value.
        /// </summary>
        private void ClearValue()
        {
            if (TextBox != null)
            {
                TextBox.Text = string.Empty;
            }

            if (Popup != null)
            {
                Popup.IsOpen = false;
            }
        }

        /// <summary>
        /// Handles the PreviewKeyDown event of the TextBox control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="KeyEventArgs"/> instance containing the event data.</param>
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
                    break;case Key.Space:
                    AutoFillDataMaui.OnKeyCharPressed(' ');
                    e.Handled = true;
                    break;
            }

        }

        /// <summary>
        /// Handles the PreviewTextInput event of the TextBox control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="TextCompositionEventArgs"/> instance containing the event data.</param>
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

        /// <summary>
        /// Shows the lookup window.
        /// </summary>
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
                !initialText.IsNullOrEmpty() && Value.PrimaryKeyValue.IsValid())
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
                , Setup.AllowLookupAdd && !ReadOnlyMode
                , Setup.AllowLookupView
                , initialText
                , Value?.PrimaryKeyValue
                , this
                , readOnlyPrimaryKeyValue);
            lookupWindow.AddViewParameter = Setup.AddViewParameter;
            lookupWindow.AllowAdvancedFind = AllowAdvancedFind;

            if (Popup != null)
                Popup.IsOpen = false;

            //Peter Ringering - 11/23/2024 10:16:20 AM - E-71
            //lookupWindow.Owner = Window.GetWindow(this);

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

            //Peter Ringering - 11/23/2024 10:16:20 AM - E-71
            //lookupWindow.Closed += (sender, args) =>
            //{
            //    Window.GetWindow(this).Activate();
            //};
            //lookupWindow.Show();
            lookupWindow.ShowDialog();
        }

        /// <summary>
        /// Lookups the form lookup select.
        /// </summary>
        /// <param name="sender">The sender.</param>
        /// <param name="e">The e.</param>
        private void LookupForm_LookupSelect(object sender, LookupSelectArgs e)
        { 
            var text = e.LookupData.GetSelectedText();
            SetValue(e.LookupData.SelectedPrimaryKeyValue, text);
            LookupSelect?.Invoke(this, new EventArgs());

            AutoFillDataMaui.OnLookupSelect(e.LookupData.GetSelectedPrimaryKeyValue());
            OnSelect();
        }

        /// <summary>
        /// Raises the dirty flag.
        /// </summary>
        private void RaiseDirtyFlag()
        {
            IsDirty = true;
            ControlDirty?.Invoke(this, EventArgs.Empty);
        }

        /// <summary>
        /// Sends the specified key.
        /// </summary>
        /// <param name="key">The key.</param>
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

        /// <summary>
        /// Sets the read only mode.
        /// </summary>
        /// <param name="readOnlyValue">if set to <c>true</c> [read only value].</param>
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

        /// <summary>
        /// Handles the <see cref="E:KeyDown" /> event.
        /// </summary>
        /// <param name="e">The <see cref="KeyEventArgs"/> instance containing the event data.</param>
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
