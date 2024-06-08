// ***********************************************************************
// Assembly         : RingSoft.DbLookup.Controls.WPF
// Author           : petem
// Created          : 12-19-2022
//
// Last Modified By : petem
// Last Modified On : 12-08-2023
// ***********************************************************************
// <copyright file="LookupControl.cs" company="Peter Ringering">
//     Copyright (c) 2023. All rights reserved.
// </copyright>
// <summary></summary>
// ***********************************************************************
using RingSoft.DataEntryControls.Engine;
using RingSoft.DataEntryControls.WPF;
using RingSoft.DbLookup.Controls.WPF.AdvancedFind;
using RingSoft.DbLookup.Lookup;
using RingSoft.DbLookup.ModelDefinition.FieldDefinitions;
using RingSoft.DbLookup.QueryBuilder;
using RingSoft.DbMaintenance;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Linq;
using System.Media;
using System.Reflection;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Data;
using System.Windows.Input;
using System.Windows.Media;

// ReSharper disable once CheckNamespace
namespace RingSoft.DbLookup.Controls.WPF
{
    /// <summary>
    /// Enum AdvancedFindModes
    /// </summary>
    public enum AdvancedFindModes
    {
        /// <summary>
        /// The enabled
        /// </summary>
        Enabled = 1,
        /// <summary>
        /// The disabled
        /// </summary>
        Disabled = 2,
        /// <summary>
        /// The done
        /// </summary>
        Done = 3
    }

    /// <summary>
    /// Class DataSource.
    /// Implements the <see cref="System.Collections.ObjectModel.ObservableCollection{RingSoft.DbLookup.Controls.WPF.DataItem}" />
    /// </summary>
    /// <seealso cref="System.Collections.ObjectModel.ObservableCollection{RingSoft.DbLookup.Controls.WPF.DataItem}" />
    public class DataSource : ObservableCollection<DataItem>
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
    /// Class DataColumn.
    /// </summary>
    public class DataColumn
    {
        /// <summary>
        /// Gets the index of the column.
        /// </summary>
        /// <value>The index of the column.</value>
        public int ColumnIndex { get; }

        /// <summary>
        /// Gets the name of the column.
        /// </summary>
        /// <value>The name of the column.</value>
        public string ColumnName { get; }

        /// <summary>
        /// Initializes a new instance of the <see cref="DataColumn"/> class.
        /// </summary>
        /// <param name="columnIndex">Index of the column.</param>
        /// <param name="columnName">Name of the column.</param>
        public DataColumn(int columnIndex, string columnName)
        {
            ColumnIndex = columnIndex;
            ColumnName = columnName;
        }
    }

    /// <summary>
    /// Class DataColumnMaps.
    /// </summary>
    public class DataColumnMaps
    {
        /// <summary>
        /// Gets the column maps.
        /// </summary>
        /// <value>The column maps.</value>
        public List<DataColumn> ColumnMaps { get; } = new List<DataColumn>();

        /// <summary>
        /// Adds the column.
        /// </summary>
        /// <param name="columnName">Name of the column.</param>
        public void AddColumn(string columnName)
        {
            var columnMap = new DataColumn(ColumnMaps.Count, columnName);
            ColumnMaps.Add(columnMap);
        }

        /// <summary>
        /// Gets the name of the visible column.
        /// </summary>
        /// <param name="dataColumnName">Name of the data column.</param>
        /// <returns>System.String.</returns>
        public string GetVisibleColumnName(string dataColumnName)
        {
            var map = ColumnMaps.FirstOrDefault(p => p.ColumnName == dataColumnName);
            var visibleColumnName = $"Column{map.ColumnIndex}";
            return visibleColumnName;
        }

        /// <summary>
        /// Clears the columns.
        /// </summary>
        public void ClearColumns()
        {
            ColumnMaps.Clear();
        }
}

    /// <summary>
    /// Class DataItem.
    /// </summary>
    public class DataItem
    {
        /// <summary>
        /// Gets or sets the column0.
        /// </summary>
        /// <value>The column0.</value>
        public string Column0 { get; set; }
        /// <summary>
        /// Gets or sets the column1.
        /// </summary>
        /// <value>The column1.</value>
        public string Column1 { get; set; }
        /// <summary>
        /// Gets or sets the column2.
        /// </summary>
        /// <value>The column2.</value>
        public string Column2 { get; set; }
        /// <summary>
        /// Gets or sets the column3.
        /// </summary>
        /// <value>The column3.</value>
        public string Column3 { get; set; }
        /// <summary>
        /// Gets or sets the column4.
        /// </summary>
        /// <value>The column4.</value>
        public string Column4 { get; set; }
        /// <summary>
        /// Gets or sets the column5.
        /// </summary>
        /// <value>The column5.</value>
        public string Column5 { get; set; }
        /// <summary>
        /// Gets or sets the column6.
        /// </summary>
        /// <value>The column6.</value>
        public string Column6 { get; set; }
        /// <summary>
        /// Gets or sets the column7.
        /// </summary>
        /// <value>The column7.</value>
        public string Column7 { get; set; }
        /// <summary>
        /// Gets or sets the column8.
        /// </summary>
        /// <value>The column8.</value>
        public string Column8 { get; set; }
        /// <summary>
        /// Gets or sets the column9.
        /// </summary>
        /// <value>The column9.</value>
        public string Column9 { get; set; }

        /// <summary>
        /// Gets the column maps.
        /// </summary>
        /// <value>The column maps.</value>
        public DataColumnMaps ColumnMaps { get; }

        /// <summary>
        /// Initializes a new instance of the <see cref="DataItem"/> class.
        /// </summary>
        /// <param name="columnMaps">The column maps.</param>
        public DataItem(DataColumnMaps columnMaps)
        {
            ColumnMaps = columnMaps;
        }

        /// <summary>
        /// Gets the column value.
        /// </summary>
        /// <param name="columnName">Name of the column.</param>
        /// <returns>System.String.</returns>
        public string GetColumnValue(string columnName)
        {
            var column = ColumnMaps.ColumnMaps.FirstOrDefault(p => p.ColumnName == columnName);
            var index = ColumnMaps.ColumnMaps.IndexOf(column);
            switch (index)
            {
                case 0:
                    return Column0;
                case 1:
                    return Column1;
                case 2:
                    return Column2;
                case 3: return Column3;
                case 4: return Column4;
                case 5: return Column5;
                case 6: return Column6;
                case 7: return Column7;
                case 8: return Column8;
                case 9: return Column9;

            }

            return string.Empty;
        }

        /// <summary>
        /// Sets the column value.
        /// </summary>
        /// <param name="columnName">Name of the column.</param>
        /// <param name="value">The value.</param>
        public void SetColumnValue(string columnName, string value)
        {
            var column = ColumnMaps.ColumnMaps.FirstOrDefault(p => p.ColumnName == columnName);
            var index = ColumnMaps.ColumnMaps.IndexOf(column);
            switch (index)
            {
                case 0:
                    Column0 = value;
                    break;
                case 1:
                    Column1 = value;
                    break;
                case 2: Column2 = value;
                    break;
                case 3: Column3 = value;
                    break;
                case 4: Column4 = value;
                    break;
                case 5: Column5 = value;
                    break;
                case 6: Column6 = value;
                    break;
                case 7: Column7 = value;
                    break;
                case 8: Column8 = value;
                    break;
                case 9: Column9 = value;
                    break;
            }
        }
    }
    /// <summary>
    /// Class LookupColumnWidthChangedArgs.
    /// </summary>
    /// <font color="red">Badly formed XML comment.</font>
    public class LookupColumnWidthChangedArgs
    {
        /// <summary>
        /// Gets or sets the column definition.
        /// </summary>
        /// <value>The column definition.</value>
        public LookupColumnDefinitionBase ColumnDefinition { get; set; }
        /// <summary>
        /// Gets or sets the size changed event arguments.
        /// </summary>
        /// <value>The size changed event arguments.</value>
        public SizeChangedEventArgs SizeChangedEventArgs { get; set; }
    }

    /// <summary>
    /// A control that displays paged data to the user.
    /// Implements the <see cref="Control" />
    /// Implements the <see cref="ILookupControl" />
    /// Implements the <see cref="IReadOnlyControl" />
    /// </summary>
    /// <seealso cref="Control" />
    /// <seealso cref="ILookupControl" />
    /// <seealso cref="IReadOnlyControl" />
    [TemplatePart(Name = "SearchForLabel", Type = typeof(Label))]
    [TemplatePart(Name = "EqualsRadioButton", Type = typeof(RadioButton))]
    [TemplatePart(Name = "ContainsRadioButton", Type = typeof(RadioButton))]
    [TemplatePart(Name = "SearchForStackPanel", Type = typeof(StackPanel))]
    [TemplatePart(Name = "ListView", Type = typeof(ListView))]
    [TemplatePart(Name = "LookupGridView", Type = typeof(GridView))]
    [TemplatePart(Name = "ScrollBar", Type = typeof(ScrollBar))]
    [TemplatePart(Name = "RecordCountStackPanel", Type = typeof(StackPanel))]
    [TemplatePart(Name = "RecordCountControl", Type = typeof(StringReadOnlyBox))]
    [TemplatePart(Name = "Spinner", Type = typeof(Control))]
    [TemplatePart(Name = "AdvancedFindButton", Type = typeof(Button))]
    [TemplatePart(Name = "ListTextBox", Type = typeof(StringReadOnlyBox))]
    public class LookupControl : Control, ILookupControl, IReadOnlyControl
    {
        /// <summary>
        /// Class RefreshPendingData.
        /// </summary>
        private class RefreshPendingData
        {
            /// <summary>
            /// Gets the initial search for.
            /// </summary>
            /// <value>The initial search for.</value>
            public string InitialSearchFor { get; }

            /// <summary>
            /// Gets the parent window primary key value.
            /// </summary>
            /// <value>The parent window primary key value.</value>
            public PrimaryKeyValue ParentWindowPrimaryKeyValue { get; }

            /// <summary>
            /// Initializes a new instance of the <see cref="RefreshPendingData"/> class.
            /// </summary>
            /// <param name="initialSearchFor">The initial search for.</param>
            /// <param name="parentWindowPrimaryKeyValue">The parent window primary key value.</param>
            public RefreshPendingData(string initialSearchFor, PrimaryKeyValue parentWindowPrimaryKeyValue)
            {
                InitialSearchFor = initialSearchFor;
                ParentWindowPrimaryKeyValue = parentWindowPrimaryKeyValue;
            }
        }

        /// <summary>
        /// Gets or sets the search for label.
        /// </summary>
        /// <value>The search for label.</value>
        public Label SearchForLabel { get; set; }

        /// <summary>
        /// Gets or sets the list text box.
        /// </summary>
        /// <value>The list text box.</value>
        public StringReadOnlyBox ListTextBox { get; set; }

        /// <summary>
        /// Gets or sets the equals RadioButton.
        /// </summary>
        /// <value>The equals RadioButton.</value>
        public RadioButton EqualsRadioButton { get; set; }

        /// <summary>
        /// Gets or sets the contains RadioButton.
        /// </summary>
        /// <value>The contains RadioButton.</value>
        public RadioButton ContainsRadioButton { get; set; }

        /// <summary>
        /// Gets or sets the search for stack panel.
        /// </summary>
        /// <value>The search for stack panel.</value>
        public StackPanel SearchForStackPanel { get; set; }

        /// <summary>
        /// Gets or sets the ListView.
        /// </summary>
        /// <value>The ListView.</value>
        public ListView ListView { get; set; }

        /// <summary>
        /// Gets or sets the lookup grid view.
        /// </summary>
        /// <value>The lookup grid view.</value>
        public GridView LookupGridView { get; set; }

        /// <summary>
        /// Gets or sets the scroll bar.
        /// </summary>
        /// <value>The scroll bar.</value>
        public ScrollBar ScrollBar { get; set; }

        /// <summary>
        /// Gets or sets the get record count button.
        /// </summary>
        /// <value>The get record count button.</value>
        public Button GetRecordCountButton { get; set; }

        /// <summary>
        /// Gets or sets the record count stack panel.
        /// </summary>
        /// <value>The record count stack panel.</value>
        public StackPanel RecordCountStackPanel { get; set; }

        /// <summary>
        /// Gets or sets the record count control.
        /// </summary>
        /// <value>The record count control.</value>
        public StringReadOnlyBox RecordCountControl { get; set; }

        /// <summary>
        /// Gets or sets the spinner.
        /// </summary>
        /// <value>The spinner.</value>
        public Control Spinner { get; set; }

        /// <summary>
        /// Gets or sets the advanced find button.
        /// </summary>
        /// <value>The advanced find button.</value>
        public Button AdvancedFindButton { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether [show record count props].
        /// </summary>
        /// <value><c>true</c> if [show record count props]; otherwise, <c>false</c>.</value>
        public bool ShowRecordCountProps { get; set; }

        //--------------------------------------------------------------

        /// <summary>
        /// The lookup search for host
        /// </summary>
        private LookupSearchForHost _lookupSearchForHost;
        /// <summary>
        /// The read only mode
        /// </summary>
        private bool _readOnlyMode;

        /// <summary>
        /// Gets or sets the search for host.
        /// </summary>
        /// <value>The search for host.</value>
        public LookupSearchForHost SearchForHost
        {
            get => _lookupSearchForHost;
            set
            {
                if (_lookupSearchForHost != null)
                {
                    SearchForHost.PreviewKeyDown -= SearchForControl_PreviewKeyDown;
                    SearchForHost.TextChanged -= SearchForControl_TextChanged;
                    SearchForHost.Control.PreviewLostKeyboardFocus -= Control_PreviewLostKeyboardFocus;
                }

                _lookupSearchForHost = value;

                if (_lookupSearchForHost != null)
                {
                    SearchForHost.PreviewKeyDown += SearchForControl_PreviewKeyDown;
                    SearchForHost.TextChanged += SearchForControl_TextChanged;
                    SearchForHost.Control.PreviewLostKeyboardFocus += Control_PreviewLostKeyboardFocus;
                }
            }
        }

        /// <summary>
        /// Gets the number of rows on a page.
        /// </summary>
        /// <value>The number of rows on the page.</value>
        public int PageSize => _currentPageSize;

        /// <summary>
        /// Gets the type of the search.
        /// </summary>
        /// <value>The type of the search.</value>
        public LookupSearchTypes SearchType
        {
            get
            {
                if (EqualsRadioButton == null)
                    return LookupSearchTypes.Equals;

                if (EqualsRadioButton.IsChecked != null && (bool)EqualsRadioButton.IsChecked)
                    return LookupSearchTypes.Equals;

                return LookupSearchTypes.Contains;
            }
        }

        /// <summary>
        /// Gets or sets the search text.
        /// </summary>
        /// <value>The search text.</value>
        public string SearchText
        {
            get
            {
                if (SearchForHost == null)
                    return String.Empty;

                return SearchForHost.SearchText;
            }
            set
            {
                _resettingSearchFor = true;
                if (SearchForHost != null)
                    SearchForHost.SearchText = value;
                _resettingSearchFor = false;
            }
        }

        /// <summary>
        /// Gets the index of the selected.
        /// </summary>
        /// <value>The index of the selected.</value>
        public int SelectedIndex
        {
            get
            {
                if (ListView.SelectedIndex >= 0)
                {
                    return ListView.SelectedIndex;
                }
                return -1;
            }
        }

        /// <summary>
        /// Sets the index of the lookup.
        /// </summary>
        /// <param name="index">The index.</param>
        public void SetLookupIndex(int index)
        {
            ListView.SelectedIndex = index;
        }

        /// <summary>
        /// The lookup definition property
        /// </summary>
        public static readonly DependencyProperty LookupDefinitionProperty =
            DependencyProperty.Register("LookupDefinition", typeof(LookupDefinitionBase), typeof(LookupControl),
                new FrameworkPropertyMetadata(LookupDefinitionChangedCallback));

        /// <summary>
        /// Gets or sets the lookup definition.
        /// </summary>
        /// <value>The lookup definition.</value>
        public LookupDefinitionBase LookupDefinition
        {
            get { return (LookupDefinitionBase)GetValue(LookupDefinitionProperty); }
            set { SetValue(LookupDefinitionProperty, value); }
        }

        /// <summary>
        /// Lookups the definition changed callback.
        /// </summary>
        /// <param name="obj">The object.</param>
        /// <param name="args">The <see cref="DependencyPropertyChangedEventArgs"/> instance containing the event data.</param>
        private static void LookupDefinitionChangedCallback(DependencyObject obj,
            DependencyPropertyChangedEventArgs args)
        {
            var lookupControl = (LookupControl)obj;
            if (lookupControl.LookupDefinition != null)
            {
                lookupControl.LookupDefinition.CommandChanged += (sender, args) =>
                {
                    lookupControl._commandToExecute = args.NewCommand;
                    lookupControl.ExecuteCommand();
                };
            }

            if (lookupControl._controlLoaded)
                lookupControl.SetupControl();
        }

        /// <summary>
        /// The command property
        /// </summary>
        public static readonly DependencyProperty CommandProperty =
            DependencyProperty.Register("Command", typeof(LookupCommand), typeof(LookupControl),
                new FrameworkPropertyMetadata(CommandChangedCallback));

        /// <summary>
        /// Gets or sets the LookupCommand which is used by view models to tell this control to either refresh, clear etc.
        /// </summary>
        /// <value>The command.</value>
        public LookupCommand Command
        {
            get { return (LookupCommand)GetValue(CommandProperty); }
            set { SetValue(CommandProperty, value); }
        }

        /// <summary>
        /// Commands the changed callback.
        /// </summary>
        /// <param name="obj">The object.</param>
        /// <param name="args">The <see cref="DependencyPropertyChangedEventArgs"/> instance containing the event data.</param>
        private static void CommandChangedCallback(DependencyObject obj,
            DependencyPropertyChangedEventArgs args)
        {
            var lookupControl = (LookupControl)obj;
            lookupControl._commandToExecute = lookupControl.Command;
            lookupControl.ExecuteCommand();
        }

        /// <summary>
        /// The data source changed property
        /// </summary>
        public static readonly DependencyProperty DataSourceChangedProperty =
            DependencyProperty.Register("DataSourceChanged", typeof(LookupDataSourceChanged), typeof(LookupControl));

        /// <summary>
        /// Gets or sets the data source changed.
        /// </summary>
        /// <value>The data source changed.</value>
        public LookupDataSourceChanged DataSourceChanged
        {
            get { return (LookupDataSourceChanged)GetValue(DataSourceChangedProperty); }
            set { SetValue(DataSourceChangedProperty, value); }
        }

        /// <summary>
        /// The design text property
        /// </summary>
        public static readonly DependencyProperty DesignTextProperty =
            DependencyProperty.Register("DesignText", typeof(string), typeof(LookupControl),
                new FrameworkPropertyMetadata(DesignTextChangedCallback));

        /// <summary>
        /// Gets or sets the design text.
        /// </summary>
        /// <value>The design text.</value>
        public string DesignText
        {
            get { return (string)GetValue(DesignTextProperty); }
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
            var lookupControl = (LookupControl)obj;
            lookupControl.SetDesignText();
        }

        /// <summary>
        /// Gets a list of LookupColumns which allow adding new columns.
        /// </summary>
        /// <value>The lookup columns.</value>
        public ObservableCollection<LookupColumnBase> LookupColumns { get; }

        //public LookupDataBase LookupData { get; private set; }

        /// <summary>
        /// Gets the lookup data maui.
        /// </summary>
        /// <value>The lookup data maui.</value>
        public LookupDataMauiBase LookupDataMaui { get; private set; }

        /// <summary>
        /// Gets a value indicating whether [lookup window read only mode].
        /// </summary>
        /// <value><c>true</c> if [lookup window read only mode]; otherwise, <c>false</c>.</value>
        public bool LookupWindowReadOnlyMode { get; internal set; }

        /// <summary>
        /// Gets the add view parameter.
        /// </summary>
        /// <value>The add view parameter.</value>
        public object AddViewParameter { get; internal set; }

        /// <summary>
        /// Gets or sets a value indicating whether [show advanced find button].
        /// </summary>
        /// <value><c>true</c> if [show advanced find button]; otherwise, <c>false</c>.</value>
        public bool ShowAdvancedFindButton { get; set; } = true;

        /// <summary>
        /// Gets or sets a value indicating whether [force refresh on activate].
        /// </summary>
        /// <value><c>true</c> if [force refresh on activate]; otherwise, <c>false</c>.</value>
        public bool ForceRefreshOnActivate { get; set; } = true;

        /// <summary>
        /// Gets or sets the record count wait.
        /// </summary>
        /// <value>The record count wait.</value>
        public int RecordCountWait { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether [show record count wait].
        /// </summary>
        /// <value><c>true</c> if [show record count wait]; otherwise, <c>false</c>.</value>
        public bool ShowRecordCountWait { get; set; }

        /// <summary>
        /// Gets the lookup window.
        /// </summary>
        /// <value>The lookup window.</value>
        public ILookupWindow LookupWindow { get; private set; }

        /// <summary>
        /// Gets or sets a value indicating whether [hide user double click row message].
        /// </summary>
        /// <value><c>true</c> if [hide user double click row message]; otherwise, <c>false</c>.</value>
        public bool HideUserDoubleClickRowMessage { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether [allow on enter].
        /// </summary>
        /// <value><c>true</c> if [allow on enter]; otherwise, <c>false</c>.</value>
        public bool AllowOnEnter { get; set; } = true;

        /// <summary>
        /// Occurs when a user wishes to add or view a selected lookup row.  Set Handled property to True to not send this message to the LookupContext.
        /// </summary>
        public event EventHandler<LookupAddViewArgs> LookupView;

        /// <summary>
        /// Occurs when [column width changed].
        /// </summary>
        public event EventHandler<LookupColumnWidthChangedArgs> ColumnWidthChanged;

        /// <summary>
        /// Occurs when [selected index changed].
        /// </summary>
        public event EventHandler SelectedIndexChanged;

        /// <summary>
        /// The control loaded
        /// </summary>
        private bool _controlLoaded;
        /// <summary>
        /// The on load ran
        /// </summary>
        private bool _onLoadRan;
        /// <summary>
        /// The setup ran
        /// </summary>
        private bool _setupRan;
        /// <summary>
        /// The original page size
        /// </summary>
        private int _originalPageSize;
        /// <summary>
        /// The current page size
        /// </summary>
        private int _currentPageSize;
        //private DataTable _dataSource = new DataTable("DataSourceTable");
        /// <summary>
        /// The column maps
        /// </summary>
        private DataColumnMaps _columnMaps = new DataColumnMaps();
        /// <summary>
        /// The data source
        /// </summary>
        private DataSource _dataSource = new DataSource();

        /// <summary>
        /// The last header clicked
        /// </summary>
        GridViewColumnHeader _lastHeaderClicked;
        /// <summary>
        /// The last direction
        /// </summary>
        ListSortDirection _lastDirection = ListSortDirection.Ascending;
        /// <summary>
        /// The pre scroll thumb position
        /// </summary>
        private double _preScrollThumbPosition;
        /// <summary>
        /// The selected index
        /// </summary>
        private int _selectedIndex;

        /// <summary>
        /// The refresh pending data
        /// </summary>
        private RefreshPendingData _refreshPendingData;

        /// <summary>
        /// The resetting search for
        /// </summary>
        private bool _resettingSearchFor;
        /// <summary>
        /// The item height
        /// </summary>
        private double _itemHeight;
        /// <summary>
        /// The design sort index
        /// </summary>
        private int _designSortIndex = -1;
        /// <summary>
        /// The design mode header line height
        /// </summary>
        private double _designModeHeaderLineHeight;
        /// <summary>
        /// The design mode search for text box
        /// </summary>
        private TextBox _designModeSearchForTextBox;
        /// <summary>
        /// The advanced find mode
        /// </summary>
        private AdvancedFindModes _advancedFindMode = AdvancedFindModes.Done;
        /// <summary>
        /// The command to execute
        /// </summary>
        private LookupCommand _commandToExecute;

        /// <summary>
        /// Initializes static members of the <see cref="LookupControl"/> class.
        /// </summary>
        static LookupControl()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(LookupControl), new FrameworkPropertyMetadata(typeof(LookupControl)));

            IsTabStopProperty.OverrideMetadata(typeof(LookupControl), new FrameworkPropertyMetadata(false));

            KeyboardNavigation.TabNavigationProperty.OverrideMetadata(typeof(LookupControl),
                new FrameworkPropertyMetadata(KeyboardNavigationMode.Local));
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="LookupControl"/> class.
        /// </summary>
        public LookupControl()
        {
            var d = DependencyPropertyDescriptor.FromProperty(IsVisibleProperty, typeof(LookupControl));
            d.AddValueChanged(this, OnIsVisiblePropertyChanged);

            LookupColumns = new ObservableCollection<LookupColumnBase>();

            LookupColumns.CollectionChanged += (sender, args) => OnLookupColumnsChanged();

            Loaded += (sender, args) => OnLoad();
        }

        /// <summary>
        /// Handles the <see cref="E:IsVisiblePropertyChanged" /> event.
        /// </summary>
        /// <param name="sender">The sender.</param>
        /// <param name="e">The <see cref="EventArgs"/> instance containing the event data.</param>
        private void OnIsVisiblePropertyChanged(object sender, EventArgs e)
        {
            LoadOnIsVisible();
        }

        /// <summary>
        /// When overridden in a derived class, is invoked whenever application code or internal processes call <see cref="M:System.Windows.FrameworkElement.ApplyTemplate" />.
        /// </summary>
        /// <exception cref="System.ArgumentOutOfRangeException"></exception>
        public override void OnApplyTemplate()
        {
            SearchForLabel = GetTemplateChild(nameof(SearchForLabel)) as Label;
            EqualsRadioButton = GetTemplateChild(nameof(EqualsRadioButton)) as RadioButton;
            ContainsRadioButton = GetTemplateChild(nameof(ContainsRadioButton)) as RadioButton;
            SearchForStackPanel = GetTemplateChild(nameof(SearchForStackPanel)) as StackPanel;
            ListView = GetTemplateChild(nameof(ListView)) as ListView;
            LookupGridView = GetTemplateChild(nameof(LookupGridView)) as GridView;
            ScrollBar = GetTemplateChild(nameof(ScrollBar)) as ScrollBar;
            GetRecordCountButton = GetTemplateChild(nameof(GetRecordCountButton)) as Button;
            RecordCountStackPanel = GetTemplateChild(nameof(RecordCountStackPanel)) as StackPanel;
            RecordCountControl = GetTemplateChild(nameof(RecordCountControl)) as StringReadOnlyBox;
            Spinner = GetTemplateChild(nameof(Spinner)) as Control;
            AdvancedFindButton = GetTemplateChild(nameof(AdvancedFindButton)) as Button;
            ListTextBox = GetTemplateChild(nameof(ListTextBox)) as StringReadOnlyBox;

            ListTextBox.Visibility = Visibility.Collapsed;

            if (LookupDefinition != null && !LookupDefinition.TableDefinition.CanViewTable)
            {
                ShowAdvancedFindButton = false;
            }

            if (!ShowAdvancedFindButton)
            {
                if (AdvancedFindButton != null) 
                    AdvancedFindButton.Visibility = Visibility.Collapsed;
            }

            switch (_advancedFindMode)
            {
                case AdvancedFindModes.Enabled:
                    AdvancedFindButton.IsEnabled = true;
                    break;
                case AdvancedFindModes.Disabled:
                    AdvancedFindButton.IsEnabled = false;
                    break;
                case AdvancedFindModes.Done:
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }
            _advancedFindMode = AdvancedFindModes.Done;
            base.OnApplyTemplate();
        }

        /// <summary>
        /// Handles the SelectionChanged1 event of the ListView control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="SelectionChangedEventArgs"/> instance containing the event data.</param>
        /// <exception cref="System.NotImplementedException"></exception>
        private void ListView_SelectionChanged1(object sender, SelectionChangedEventArgs e)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Called when [load].
        /// </summary>
        private void OnLoad()
        {
            OnLookupColumnsChanged();
            SetDesignText();
            if (_designSortIndex >= 0 && DesignerProperties.GetIsInDesignMode(this))
            {
                InitializeHeader(_designSortIndex);
                DesignerFillGrid();
            }

            //This runs twice when control is in a tab page.  Once when the control is loaded and again when tab page is loaded.
            _onLoadRan = true;
            LoadOnIsVisible();
        }

        /// <summary>
        /// Loads the on is visible.
        /// </summary>
        private void LoadOnIsVisible()
        {
            if (DesignerProperties.GetIsInDesignMode(this))
            {
                return;
            }
            //All 4 of these variables must be checked to ensure the following scenarios work:
            //* Northwind Customers Window Orders tab page works.
            //* SimpleDemo Order Details lookup when initial is collapsed and then is expanded.
            if (IsVisible && _onLoadRan && !_controlLoaded && ListView != null)
            {
                SizeChanged += (sender, args) => LookupControlSizeChanged();

                if (LookupDefinition != null)
                    SetupControl();
                _controlLoaded = true;
            }
        }

        /// <summary>
        /// Invoked whenever an unhandled <see cref="E:System.Windows.UIElement.GotFocus" /> event reaches this element in its route.
        /// </summary>
        /// <param name="e">The <see cref="T:System.Windows.RoutedEventArgs" /> that contains the event data.</param>
        protected override void OnGotFocus(RoutedEventArgs e)
        {
            SearchForHost?.Control.Focus();

            base.OnGotFocus(e);
        }

        /// <summary>
        /// Setups the control.
        /// </summary>
        private void SetupControl()
        {
            if (_commandToExecute != null && (_commandToExecute.ClearColumns || LookupDefinition == null))
                return;

            //if (LookupDefinition.InitialSortColumnDefinition == null)
            //    throw new ArgumentException(
            //        "Lookup definition does not have any visible columns defined or its initial sort column is null.");

            var refreshPendingData = _refreshPendingData;
            ClearLookupControl();
            
            if (LookupDataMaui != null)
            {
                //ClearLookupControl();
                LookupColumns.Clear();
            }
            
            if (LookupDefinition != null)
            {
                LookupDataMaui =
                    LookupDefinition.TableDefinition.LookupDefinition.GetLookupDataMaui(LookupDefinition, false);
                LookupDataMaui.SetParentControls(this, LookupWindow);

                //LookupData = new LookupDataBase(LookupDefinition, this);

                LookupDataMaui.LookupDataChanged += LookupDataMaui_LookupDataChanged;
                //LookupData.LookupDataChanged += LookupData_LookupDataChanged;
                //LookupData.DataSourceChanged += LookupData_DataSourceChanged;
                LookupDataMaui.LookupView += (sender, args) => LookupView?.Invoke(this, args);
            }

            if (!_setupRan && LookupDataMaui != null)
            {
                //SetupRecordCount(LookupDataMaui.GetRecordCount());
                if (ListView != null)
                {
                    ListView.PreviewKeyDown += ListView_PreviewKeyDown;
                    ListView.SelectionChanged += ListView_SelectionChanged;
                    ListView.MouseDoubleClick += (sender, args) => { OnEnter(); };
                    ListView.PreviewMouseWheel += ListView_PreviewMouseWheel;
                }

                if (ContainsRadioButton != null)
                    ContainsRadioButton.Click += (sender, args) => { OnSearchTypeChanged(); };

                if (EqualsRadioButton != null)
                    EqualsRadioButton.Click += (sender, args) => { OnSearchTypeChanged(); };

                if (GetRecordCountButton != null)
                    GetRecordCountButton.Click += (sender, args) => { GetRecordCountButtonClick(); };

                if (AdvancedFindButton != null)
                    AdvancedFindButton.Click += (sender, args) => { ShowAdvancedFind(); };

                if (ScrollBar != null)
                {
                    ScrollBar.Scroll += ScrollBar_Scroll;
                    ScrollBar.PreviewMouseDown += (sender, args) => { _preScrollThumbPosition = ScrollBar.Value; };
                }
            }

            LookupGridView?.Columns.Clear();

            _columnMaps.ClearColumns();
            _dataSource.Clear();


            if (LookupColumns.Any())
                MergeLookupDefinition();
            else
                ImportLookupDefinition();

            if (LookupDefinition != null)
            {
                if (!LookupDefinition.VisibleColumns.Any())
                {
                    SetActiveColumn(0, FieldDataTypes.String);
                    _setupRan = true;
                    return;
                }
            }
            else
            {
                SetActiveColumn(0, FieldDataTypes.String);
            }

            //LookupDefinition.InitialSortColumnDefinition = LookupDefinition.VisibleColumns[0];
            var sortColumnIndex =
                GetIndexOfVisibleColumnDefinition(LookupDefinition?.InitialOrderByColumn);

            if (sortColumnIndex < 0)
                sortColumnIndex = 0;
            InitializeHeader(sortColumnIndex);
            if (LookupDefinition?.InitialOrderByColumn != null)
                SetActiveColumn(sortColumnIndex, LookupDefinition.InitialOrderByColumn.DataType);

            if (refreshPendingData != null)
            {
                RefreshData(true, refreshPendingData.InitialSearchFor, refreshPendingData.ParentWindowPrimaryKeyValue,
                    true);
                refreshPendingData = null;
            }

            _setupRan = _controlLoaded = true;
        }

        /// <summary>
        /// Handles the PreviewKeyDown event of the ListView control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="KeyEventArgs"/> instance containing the event data.</param>
        private void ListView_PreviewKeyDown(object sender, KeyEventArgs e)
        {
            OnListViewKeyDown(e);
        }

        /// <summary>
        /// Handles the TextChanged event of the SearchForControl control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="EventArgs"/> instance containing the event data.</param>
        private void SearchForControl_TextChanged(object sender, EventArgs e)
        {
            if (!_resettingSearchFor)
                LookupDataMaui.OnSearchForChange(SearchForHost.SearchText);
        }

        /// <summary>
        /// Handles the PreviewKeyDown event of the SearchForControl control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="KeyEventArgs"/> instance containing the event data.</param>
        private void SearchForControl_PreviewKeyDown(object sender, KeyEventArgs e)
        {
            OnListViewKeyDown(e);
        }

        /// <summary>
        /// Handles the PreviewLostKeyboardFocus event of the Control control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="KeyboardFocusChangedEventArgs"/> instance containing the event data.</param>
        private void Control_PreviewLostKeyboardFocus(object sender, KeyboardFocusChangedEventArgs e)
        {
            if (ReferenceEquals(e.NewFocus, ListView))
                e.Handled = true;
        }

        /// <summary>
        /// Merges the lookup definition.
        /// </summary>
        /// <exception cref="System.Exception">No Lookup Column was added to Columns collection for initial sort column Property '{LookupDefinition.InitialSortColumnDefinition.PropertyName}'.</exception>
        /// <exception cref="System.Exception">No visible Lookup Column Definition was found for Property '{lookupColumnBase.PropertyName}'.</exception>
        private void MergeLookupDefinition()
        {
            if (LookupColumns.FirstOrDefault(
                    f => f.PropertyName
                         == LookupDefinition
                             .InitialSortColumnDefinition
                             .PropertyName) == null
                 && LookupWindow != null)
                throw new Exception($"No Lookup Column was added to Columns collection for initial sort column Property '{LookupDefinition.InitialSortColumnDefinition.PropertyName}'.");

            foreach (var lookupColumnBase in LookupColumns)
            {
                var lookupColumnDefinition =
                    LookupDefinition.VisibleColumns.FirstOrDefault(f => f.PropertyName == lookupColumnBase.PropertyName);
                if (lookupColumnDefinition == null)
                    throw new Exception($"No visible Lookup Column Definition was found for Property '{lookupColumnBase.PropertyName}'.");

                _columnMaps.AddColumn(lookupColumnDefinition.SelectSqlAlias);
                lookupColumnBase.DataColumnName = _columnMaps.GetVisibleColumnName(lookupColumnDefinition.SelectSqlAlias);

                lookupColumnBase.LookupColumnDefinition = lookupColumnDefinition;

                if (lookupColumnBase is LookupColumn lookupColumn)
                    if (!lookupColumn.TextAlignmentChanged)
                    {
                        lookupColumn.TextAlignment = lookupColumnDefinition.HorizontalAlignment;
                    }

                lookupColumnDefinition.UpdateCaption(lookupColumnBase.Header);

                AddColumnToGrid(lookupColumnBase);
            }
        }

        /// <summary>
        /// Imports the lookup definition.
        /// </summary>
        private void ImportLookupDefinition()
        {
            if (LookupDefinition!= null)
            {
                foreach (var column in LookupDefinition.VisibleColumns)
                {
                    var percentWidth = column.PercentWidth;
                    if (LookupDefinition.VisibleColumns.Count == 1 
                        && column.AdjustColumnWidth)
                    {
                        percentWidth = 99;
                    }
                    double columnWidth = 100;
                    if (ListView != null)
                        columnWidth = GetWidthFromPercent(ListView, percentWidth);

                    if (column is LookupFieldColumnDefinition lookupFieldColumn)
                    {
                        if (lookupFieldColumn.FieldDefinition is IntegerFieldDefinition integerFieldDefinition)
                        {
                            //lookupFieldColumn.HasContentTemplateId(integerFieldDefinition.ContentTemplateId);
                        }
                    }

                    var lookupColumn = LookupControlsGlobals.LookupControlColumnFactory.CreateLookupColumn(column);

                    _columnMaps.AddColumn(column.SelectSqlAlias);
                    lookupColumn.DataColumnName = _columnMaps.GetVisibleColumnName(column.SelectSqlAlias);
                    
                    lookupColumn.Header = column.Caption;
                    lookupColumn.LookupColumnDefinition = column;
                    lookupColumn.PropertyName = column.PropertyName;
                    lookupColumn.Width = columnWidth;

                    LookupColumns.Add(lookupColumn);
                    AddColumnToGrid(lookupColumn);
                }
            }
        }

        /// <summary>
        /// Gets the index of visible column definition.
        /// </summary>
        /// <param name="lookupColumnDefinition">The lookup column definition.</param>
        /// <returns>System.Int32.</returns>
        private int GetIndexOfVisibleColumnDefinition(LookupColumnDefinitionBase lookupColumnDefinition)
        {
            var lookupColumn = LookupColumns.FirstOrDefault(f => f.LookupColumnDefinition == lookupColumnDefinition);
            return LookupColumns.IndexOf(lookupColumn);
        }

        /// <summary>
        /// Adds the grid view column.
        /// </summary>
        /// <param name="caption">The caption.</param>
        /// <param name="width">The width.</param>
        /// <param name="dataColumnName">Name of the data column.</param>
        /// <param name="lookupColumn">The lookup column.</param>
        /// <returns>GridViewColumn.</returns>
        private GridViewColumn AddGridViewColumn(string caption, double width, string dataColumnName, LookupColumnBase lookupColumn)
        {
            var enable = true;
            var columnHeader = new GridViewColumnHeader { Content = caption }; // + "\r\nLine2\r\nLine3"};
            if (lookupColumn.LookupColumnDefinition is LookupFormulaColumnDefinition)
            {
                enable = false;
            }

            if (enable)
            {
                if (lookupColumn.LookupColumnDefinition is LookupFieldColumnDefinition fieldColumn)
                {
                    enable = LookupControlsGlobals.LookupWindowFactory.CanDisplayField(fieldColumn.FieldDefinition);
                }
            }

            if (enable)
            {
                columnHeader.Click += GridViewColumnHeaderClickedHandler;
            }
            else
            {
                columnHeader.Foreground = new SolidColorBrush(Colors.Black);
                columnHeader.IsEnabled = false;
            }

            if (width < 0)
            {
                width = 20;
            }

            var gridColumn = new GridViewColumn
            {
                Header = columnHeader,
                Width = width,
                CellTemplate = lookupColumn.GetCellDataTemplate(this, dataColumnName, false),
            };
            LookupGridView?.Columns.Add(gridColumn);
            

            if (LookupGridView != null && ListView != null)
            {
                columnHeader.SizeChanged += (sender, args) =>
                {
             
                    if (args.WidthChanged)
                    {
                        if (!DesignerProperties.GetIsInDesignMode(this))
                        {
                            var index = LookupGridView.Columns.IndexOf(gridColumn);
                            if (index != -1 && index < LookupGridView.Columns.Count)
                            {
                                //var lookupColumnDefinition = LookupDefinition.VisibleColumns.ToList()
                                //    .ElementAt(index);
                                var lookupColumnDefinition = LookupColumns[index]
                                    .LookupColumnDefinition;

                                lookupColumnDefinition?.UpdatePercentWidth(
                                    Math.Ceiling((args.NewSize.Width / ListView.ActualWidth) * 100));
                                ColumnWidthChanged?.Invoke(this, new LookupColumnWidthChangedArgs
                                {
                                    ColumnDefinition = lookupColumnDefinition,
                                    SizeChangedEventArgs = args
                                });
                            }
                        }
                    }
                };
            }
            return gridColumn;
        }

        /// <summary>
        /// Initializes the header.
        /// </summary>
        /// <param name="sortColumnIndex">Index of the sort column.</param>
        private void InitializeHeader(int sortColumnIndex)
        {
            if (ListView == null || LookupGridView == null)
                return;

            GridViewHeaderRowPresenter header = (GridViewHeaderRowPresenter)LookupGridView.GetType()
                .GetProperty("HeaderRowPresenter", BindingFlags.NonPublic | BindingFlags.Instance)
                ?.GetValue(LookupGridView);

            if (header != null && LookupGridView.Columns.Any())
            {
                CalculateDesignHeaderLineHeight(header);
                header.UpdateLayout();

                var column = LookupGridView.Columns[sortColumnIndex];
                var columnHeader = (GridViewColumnHeader)column.Header;
                var glyphSize = GridViewSort.GetGlyphSize(columnHeader, ListSortDirection.Ascending, ListView);
                
                Style style = new Style();
                style.TargetType = typeof(GridViewColumnHeader);
                var height = GetHeaderHeight(header);
                style.Setters.Add(new Setter(GridViewColumnHeader.HeightProperty, height + glyphSize.Height + 5));
                style.Setters.Add(new Setter(GridViewColumnHeader.VerticalContentAlignmentProperty, VerticalAlignment.Bottom));

                LookupGridView.ColumnHeaderContainerStyle = style;
                LookupGridView.ColumnHeaderTemplate = GetColumnHeaderDataTemplate();

                header.UpdateLayout();
            }

            ResetColumnHeaderSort(sortColumnIndex);
        }

        /// <summary>
        /// Gets the column header data template.
        /// </summary>
        /// <returns>DataTemplate.</returns>
        private DataTemplate GetColumnHeaderDataTemplate()
        {
            var template = new DataTemplate();

            var factory = new FrameworkElementFactory(typeof(TextBlock));
            factory.SetValue(TextBlock.TextAlignmentProperty, TextAlignment.Center);

            var binding = new Binding(TextBlock.TextProperty.Name);
            binding.RelativeSource = new RelativeSource(RelativeSourceMode.TemplatedParent);
            binding.Path = new PropertyPath(nameof(GridViewColumnHeader.Content));
            factory.SetBinding(TextBlock.TextProperty, binding);

            template.VisualTree = factory;

            return template;
        }

        /// <summary>
        /// Gets the height of the header.
        /// </summary>
        /// <param name="header">The header.</param>
        /// <returns>System.Double.</returns>
        private double GetHeaderHeight(GridViewHeaderRowPresenter header)
        {
            var height = header.ActualHeight;
            var lineFeedCount = 0;
            var startIndex = 0;

            if (DesignerProperties.GetIsInDesignMode(this))
            {
                foreach (var column in LookupColumns)
                {
                    var columnLineCount = 0;
                    if (!column.Header.IsNullOrEmpty())
                    {
                        while (startIndex >= 0)
                        {
                            startIndex = column.Header.IndexOf('\n', startIndex);
                            if (startIndex >= 0)
                            {
                                columnLineCount++;
                                startIndex++;
                            }
                        }

                        startIndex = 0;
                        if (lineFeedCount < columnLineCount)
                            lineFeedCount = columnLineCount;

                    }
                }

                height = _designModeHeaderLineHeight * (lineFeedCount + 1);
            }
            else
            {
                foreach (var column in LookupColumns)
                {
                    var columnLineCount = 0;
                    if (!column.Header.IsNullOrEmpty())
                    {
                        while (startIndex >= 0)
                        {
                            startIndex = column.Header.IndexOf('\n', startIndex);
                            if (startIndex >= 0)
                            {
                                columnLineCount++;
                                startIndex++;
                            }
                        }

                        startIndex = 0;
                    }

                    if (lineFeedCount < columnLineCount)
                        lineFeedCount = columnLineCount;
                }

                height = 20 * (lineFeedCount + 1);

            }
            return height;
        }

        /// <summary>
        /// Calculates the height of the design header line.
        /// </summary>
        /// <param name="header">The header.</param>
        private void CalculateDesignHeaderLineHeight(GridViewHeaderRowPresenter header)
        {
            if (_designModeHeaderLineHeight > 0 || !DesignerProperties.GetIsInDesignMode(this))
                return;

            if (LookupGridView == null)
                return;

            if (!LookupGridView.Columns.Any())
                return;

            foreach (var column in LookupGridView.Columns)
            {
                var columnHeader = (GridViewColumnHeader)column.Header;
                columnHeader.Content = "WWWWW\r\nWWW";
            }
            header.UpdateLayout();
            _designModeHeaderLineHeight = header.ActualHeight / 2;

            var index = 0;
            foreach (var column in LookupGridView.Columns)
            {
                var columnHeader = (GridViewColumnHeader)column.Header;
                var lookupColumn = LookupColumns[index];
                columnHeader.Content = lookupColumn.Header;
                index++;
            }
        }

        /// <summary>
        /// Resets the column header sort.
        /// </summary>
        /// <param name="sortColumnIndex">Index of the sort column.</param>
        private void ResetColumnHeaderSort(int sortColumnIndex)
        {
            if (ListView == null || LookupGridView == null)
                return;

            if (!LookupGridView.Columns.Any())
                return;

            var sortColumn = LookupGridView.Columns[sortColumnIndex];
            _lastHeaderClicked = sortColumn.Header as GridViewColumnHeader;

            _lastDirection = ListSortDirection.Ascending;
            if (LookupDefinition != null && LookupDefinition.InitialOrderByType == OrderByTypes.Descending)
                _lastDirection = ListSortDirection.Descending;

            GridViewSort.ApplySort(_lastDirection, ListView, _lastHeaderClicked);
        }

        /// <summary>
        /// Called when [lookup columns changed].
        /// </summary>
        private void OnLookupColumnsChanged()
        {
            if (!DesignerProperties.GetIsInDesignMode(this))
                return;

            LookupGridView?.Columns.Clear();
            _dataSource.Clear();

            var index = 1;

            foreach (var column in LookupColumns)
            {
                column.DataColumnName = $"Column{index}";
                AddColumnToGrid(column);
                column.PropertyChanged -= Column_PropertyChanged;
                column.PropertyChanged += Column_PropertyChanged;
                index++;
            }

            if (LookupColumns.Any())
                _designSortIndex = 0;

            InitializeHeader(_designSortIndex);
            SetActiveColumn(_designSortIndex, FieldDataTypes.String);
            DesignerFillGrid();
        }

        /// <summary>
        /// Adds the column to grid.
        /// </summary>
        /// <param name="column">The column.</param>
        private void AddColumnToGrid(LookupColumnBase column)
        {
            var gridColumn = AddGridViewColumn(column.Header, column.Width, column.DataColumnName, column);

            ((INotifyPropertyChanged)gridColumn).PropertyChanged += (sender, args) =>
            {
                if (args.PropertyName == "ActualWidth")
                {
                    LookupControlSizeChanged();
                }
            };
        }

        /// <summary>
        /// Designers the fill grid.
        /// </summary>
        private void DesignerFillGrid()
        {
            //This is to buggy.  It's causing Visual Studio to hang.
            if (DesignerProperties.GetIsInDesignMode(this))
            {
                return;
            }
            //Must get page size before clearing the data source.  Otherwise we get exceptions if column PropertyName is invalid.
            _currentPageSize = GetPageSize(false);
            _dataSource.Clear();

            for (var i = 0; i < _currentPageSize; i++)
            {
                var dataItem = new DataItem(_columnMaps);
                //var newDataRow = _dataSource.NewRow();
                foreach (var column in LookupColumns)
                {
                    var cellValue = column.DesignText;
                    //newDataRow[column.DataColumnName] = cellValue;
                    dataItem.SetColumnValue(column.DataColumnName, cellValue);
                }

                _dataSource.Add(dataItem);
            }

            if (ListView != null)
                ListView.ItemsSource = _dataSource;
        }

        /// <summary>
        /// Handles the PropertyChanged event of the Column control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="PropertyChangedEventArgs"/> instance containing the event data.</param>
        private void Column_PropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            if (LookupGridView == null)
                return;

            if (!DesignerProperties.GetIsInDesignMode(this) && !LookupGridView.Columns.Any())
                return;

            var lookupColumn = (LookupColumnBase)sender;
            var columnIndex = LookupColumns.IndexOf(lookupColumn);
            var gridColumn = LookupGridView.Columns[columnIndex];

            if (e.PropertyName == nameof(LookupColumnBase.Header))
            {
                var columnHeader = (GridViewColumnHeader)gridColumn.Header;
                columnHeader.Content = lookupColumn.Header;

                InitializeHeader(_designSortIndex);

                DesignerFillGrid();

                if (columnIndex == _designSortIndex && !lookupColumn.Header.IsNullOrEmpty())
                    SetActiveColumn(_designSortIndex, FieldDataTypes.String);
            }
            else if (e.PropertyName == nameof(LookupColumnBase.DesignText))
            {
                DesignerFillGrid();
            }
            else if (e.PropertyName == nameof(LookupColumnBase.Width))
            {
                gridColumn.Width = lookupColumn.Width;
            }
            else if (e.PropertyName == nameof(LookupColumn.TextAlignment))
            {
                OnLookupColumnsChanged();
            }
        }

        /// <summary>
        /// Handles the PreviewMouseWheel event of the ListView control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="MouseWheelEventArgs"/> instance containing the event data.</param>
        private void ListView_PreviewMouseWheel(object sender, MouseWheelEventArgs e)
        {
            e.Handled = true;
            if (e.Delta > 0)
                LookupDataMaui.OnMouseWheelForward(); //See OnPageDown() for quadriplegic debugging.
            else
                LookupDataMaui.OnMouseWheelBack(); //See OnPageUp() for quadriplegic debugging.
        }

        /// <summary>
        /// Handles the Scroll event of the ScrollBar control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="ScrollEventArgs"/> instance containing the event data.</param>
        private void ScrollBar_Scroll(object sender, ScrollEventArgs e)
        {
            e.Handled = true;
            switch (e.ScrollEventType)
            {
                case ScrollEventType.SmallDecrement:
                    OnUpArrow();
                    break;
                case ScrollEventType.SmallIncrement:
                    OnDownArrow();
                    break;
                case ScrollEventType.LargeDecrement:
                    OnPageUp(false);
                    break;
                case ScrollEventType.LargeIncrement:
                    OnPageDown(false);
                    break;
                case ScrollEventType.EndScroll:
                    var newValue = (int)Math.Ceiling(e.NewValue);
                    var scrollBarMinimum = (int)Math.Ceiling(ScrollBar.Minimum);

                    if (newValue == scrollBarMinimum)
                        OnHome(false);
                    else if (e.NewValue + ScrollBar.LargeChange >= ScrollBar.Maximum)
                        OnEnd(false);
                    else
                        ScrollBar.Value = _preScrollThumbPosition;
                    break;
            }
        }

        /// <summary>
        /// Handles the DataSourceChanged event of the LookupData control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="EventArgs"/> instance containing the event data.</param>
        private void LookupData_DataSourceChanged(object sender, EventArgs e)
        {
            DataSourceChanged = new LookupDataSourceChanged();
        }


        /// <summary>
        /// Grids the view column header clicked handler.
        /// </summary>
        /// <param name="sender">The sender.</param>
        /// <param name="e">The <see cref="RoutedEventArgs"/> instance containing the event data.</param>
        private void GridViewColumnHeaderClickedHandler(object sender, RoutedEventArgs e)
        {
            var headerClicked = e.OriginalSource as GridViewColumnHeader;

            if (headerClicked != null)
            {
                var columnIndex = LookupGridView.Columns.IndexOf(headerClicked.Column);
                if (columnIndex != -1)
                    OnColumnClick(columnIndex, true);
            }
        }

        /// <summary>
        /// Called when [column click].
        /// </summary>
        /// <param name="columnIndex">Index of the column.</param>
        /// <param name="mouseClick">if set to <c>true</c> [mouse click].</param>
        private void OnColumnClick(int columnIndex, bool mouseClick)
        {
            if (LookupGridView == null)
                return;

            if (columnIndex > LookupGridView.Columns.Count - 1)
            {
                SystemSounds.Exclamation.Play();
                return;
            }

            var sortColumn1 = LookupColumns[columnIndex];
            if (sortColumn1.LookupColumnDefinition is LookupFormulaColumnDefinition)
            {
                SystemSounds.Exclamation.Play();
                return;
            }

            if (sortColumn1.LookupColumnDefinition is LookupFieldColumnDefinition fieldColumn1)
            {
                if (!LookupControlsGlobals.LookupWindowFactory.CanDisplayField(fieldColumn1.FieldDefinition))
                {
                    SystemSounds.Exclamation.Play();
                    return;
                }
            }


            var headerClicked = LookupGridView.Columns[columnIndex].Header as GridViewColumnHeader;
            if (headerClicked == null)
                return;

            ListSortDirection direction = _lastDirection;

            if (headerClicked.Role != GridViewColumnHeaderRole.Padding)
            {
                var controlKeyPressed = Keyboard.IsKeyDown(Key.LeftCtrl) || Keyboard.IsKeyDown(Key.RightCtrl);
                var resetSortOrder = !controlKeyPressed || _lastHeaderClicked == headerClicked;
                if (!mouseClick)
                    resetSortOrder = true;

                if (resetSortOrder)
                {
                    if (headerClicked != _lastHeaderClicked)
                    {
                        direction = ListSortDirection.Ascending;
                    }
                    else
                    {
                        if (_lastDirection == ListSortDirection.Ascending)
                        {
                            direction = ListSortDirection.Descending;
                        }
                        else
                        {
                            direction = ListSortDirection.Ascending;
                        }
                    }
                }

                if (resetSortOrder)
                {
                    GridViewSort.ApplySort(direction, ListView, headerClicked);
                    _lastHeaderClicked = headerClicked;
                    _lastDirection = direction;
                }

                if (SearchForHost != null && resetSortOrder)
                    SearchForHost.SearchText = String.Empty;

                var sortColumn = LookupColumns[columnIndex];
                if (sortColumn.LookupColumnDefinition is LookupFormulaColumnDefinition)
                {
                    SystemSounds.Exclamation.Play();
                    return;
                }
                var sortColumnIndex = columnIndex;
                if (sortColumn != null && sortColumn.LookupColumnDefinition != null)
                {
                    var indexOfColumn = LookupDefinition.VisibleColumns.ToList()
                        .IndexOf(sortColumn.LookupColumnDefinition);
                    if (indexOfColumn != -1)
                    {
                        sortColumnIndex = indexOfColumn;
                    }
                }

                if (sortColumn.LookupColumnDefinition is LookupFieldColumnDefinition fieldColumn)
                {
                    LookupDataMaui.OnColumnClick(fieldColumn, resetSortOrder);
                }

                for (int i = 0; i < LookupGridView.Columns.Count; i++)
                {
                    var gridColumn = LookupGridView.Columns[i];
                    if (gridColumn != _lastHeaderClicked.Column)
                    {
                        var columnHeader = gridColumn.Header as GridViewColumnHeader;
                        GridViewSort.RemoveSortGlyph(columnHeader);
                    }
                }

                var first = true;
                var columnNumber = 1;
                foreach (var lookupColumnDefinition in LookupDataMaui.OrderByList)
                {
                    if (LookupDefinition.InitialOrderByField != null)
                    {
                        if (LookupDefinition.InitialOrderByField == lookupColumnDefinition.FieldDefinition)
                        {
                            continue;
                        }
                    }
                    if (first)
                    {
                        first = false;
                    }
                    else
                    {
                        var orderColumnIndex = GetIndexOfVisibleColumnDefinition(lookupColumnDefinition);
                        if (orderColumnIndex != -1)
                        {
                            var orderGridColumnHeader =
                                LookupGridView.Columns[orderColumnIndex].Header as GridViewColumnHeader;
                            GridViewSort.AddNonPrimarySortGlyph(orderGridColumnHeader, columnNumber);
                            columnNumber++;
                        }
                    }
                }

                if (resetSortOrder)
                {
                    var activeColumn = LookupDataMaui.OrderByList.FirstOrDefault();
                    if (LookupDefinition.InitialOrderByField != null)
                    {
                        if (LookupDefinition.InitialOrderByField == activeColumn.FieldDefinition)
                        {
                            activeColumn = LookupDataMaui.OrderByList[1];
                        }
                    }
                    SetActiveColumn(columnIndex, activeColumn.DataType);
                }

                SearchForHost?.Control.Focus();
            }
        }

        /// <summary>
        /// Sets the active column.
        /// </summary>
        /// <param name="sortColumnIndex">Index of the sort column.</param>
        /// <param name="datatype">The datatype.</param>
        private void SetActiveColumn(int sortColumnIndex, FieldDataTypes datatype)
        {
            if (!LookupColumns.Any())
            {
                SearchForStackPanel.Children.Clear();
                SearchForLabel.Content = "Search For";
                UpdateLayout();
                return;
            }

            if (SearchForStackPanel != null)
            {
                if (DesignerProperties.GetIsInDesignMode(this))
                {
                    _designModeSearchForTextBox = new TextBox();
                    SearchForStackPanel.Children.Add(_designModeSearchForTextBox);
                }
                else
                {
                    //var lookupColumnDefinition = LookupDefinition.VisibleColumns[sortColumnIndex];
                    var lookupColumnDefinition = LookupColumns[sortColumnIndex]
                        .LookupColumnDefinition;

                    if (SearchForHost != null)
                    {
                        SearchForHost = null;
                        SearchForStackPanel.Children.Clear();
                    }
                    SearchForHost =
                        LookupControlsGlobals.LookupControlSearchForFactory.CreateSearchForHost(lookupColumnDefinition);
                    
                    SearchForStackPanel.Children.Add(SearchForHost.Control);
                    SearchForStackPanel.UpdateLayout();

                    SearchForHost.Control.Loaded += (sender, args) =>
                    {
                        SearchForStackPanel.Height = SearchForHost.Control.ActualHeight;
                        if (IsKeyboardFocusWithin)
                        {
                            SearchForHost.Control.Focus();
                            SearchForHost.SetFocusToControl();
                        }
                    };
                }
            }

            var column = LookupColumns[sortColumnIndex];

            if (!column.Header.IsNullOrEmpty())
            {
                var headerText = column.Header.Replace('\n', ' ').Replace("\r", "");
                if (SearchForLabel != null)
                    SearchForLabel.Content = $@"Search For {headerText}";
            }

            if (datatype == FieldDataTypes.String)
            {
                if (ContainsRadioButton != null)
                    ContainsRadioButton.IsEnabled = true;
            }
            else
            {
                if (ContainsRadioButton != null)
                    ContainsRadioButton.IsEnabled = false;
                if (EqualsRadioButton != null)
                    EqualsRadioButton.IsChecked = true;
            }
        }

        /// <summary>
        /// Lookups the control size changed.
        /// </summary>
        private void LookupControlSizeChanged()
        {
            var originalPageSize = _originalPageSize;
            var newPageSize = _currentPageSize = GetPageSize();

            if (originalPageSize != newPageSize && originalPageSize > 0)
            {
                if (DesignerProperties.GetIsInDesignMode(this))
                {
                    DesignerFillGrid();
                }
                else
                {
                    LookupDataMaui.OnSizeChanged();
                }
            }
        }

        /// <summary>
        /// Lookups the data maui lookup data changed.
        /// </summary>
        /// <param name="sender">The sender.</param>
        /// <param name="e">The e.</param>
        /// <exception cref="System.ArgumentOutOfRangeException"></exception>
        private void LookupDataMaui_LookupDataChanged(object sender, LookupDataMauiOutput e)
        {
            _dataSource.Clear();
            
            if (sender is LookupDataMauiBase lookupDataMaui)
            {
                DataSourceChanged = new LookupDataSourceChanged();
                for (int row = 0; row < lookupDataMaui.RowCount; row++)
                {
                    var dataItem = new DataItem(_columnMaps);
                    foreach (var lookupColumn in LookupColumns)
                    {
                        //var dbValue = lookupDataMaui.GetDatabaseRowValue(row, lookupColumn.LookupColumnDefinition);
                        //if (dbValue != null)
                        //{
                        //    if (lookupColumn.SetValue(dbValue))
                        //    {
                        //        dataItem.SetColumnValue(lookupColumn.LookupColumnDefinition.SelectSqlAlias, dbValue);
                        //        continue;
                        //    }
                        //}
                        var displayVal = string.Empty;
                        if (lookupColumn.LookupColumnDefinition is LookupFieldColumnDefinition fieldColumn)
                        {
                            if (fieldColumn.FieldDefinition is IntegerFieldDefinition integerField)
                            {
                                if (integerField.ContentTemplateId > 0)
                                {
                                    var dbValue = lookupDataMaui
                                        .GetDatabaseRowValue(row, lookupColumn.LookupColumnDefinition);
                                    dataItem.SetColumnValue(lookupColumn.LookupColumnDefinition.SelectSqlAlias, dbValue);
                                    continue;
                                }
                            }
                            if (!LookupControlsGlobals.LookupWindowFactory.CanDisplayField(fieldColumn.FieldDefinition))
                            {
                                displayVal = "Can't Display Value";
                            }
                        }
                        var value = lookupDataMaui.GetFormattedRowValue(row, lookupColumn.LookupColumnDefinition);
                        if (displayVal.IsNullOrEmpty())
                        {
                            displayVal = value;
                        }
                        dataItem.SetColumnValue(lookupColumn.LookupColumnDefinition.SelectSqlAlias, displayVal);
                        displayVal = string.Empty;
                    }
                    _dataSource.Add(dataItem);
                }

                if (_dataSource.Any())
                {
                    if (LookupWindow == null && !HideUserDoubleClickRowMessage)
                    {
                        if (LookupDefinition.TableDefinition.CanViewTable)
                        {
                            ListTextBox.Visibility = Visibility.Visible;
                        }
                    }
                    var showRecordCountButton = _dataSource.Count == PageSize;
                    if (showRecordCountButton)
                    {
                        ShowRecordCount(0, false);
                    }
                    else
                    {
                        var recordCount = lookupDataMaui.GetRecordCount();
                        ShowRecordCount(recordCount, true);
                    }
                }
                else
                {
                    ShowRecordCount(0, false);
                }
                SelectedIndexChanged?.Invoke(this, EventArgs.Empty);
            }

            if (ListView != null && ListView.ItemsSource == null)
                ListView.ItemsSource = _dataSource;

            AdvancedFindButton.IsEnabled = _dataSource.Any();

            if (ScrollBar != null)
            {
                ScrollBar.IsEnabled = true;
                switch (e.ScrollPosition)
                {
                    case LookupScrollPositions.Disabled:
                        ScrollBar.IsEnabled = false;
                        break;
                    case LookupScrollPositions.Top:
                        ScrollBar.Value = ScrollBar.Minimum;
                        break;
                    case LookupScrollPositions.Middle:
                        SetScrollThumbToMiddle();
                        break;
                    case LookupScrollPositions.Bottom:
                        ScrollBar.Value = ScrollBar.Maximum;
                        break;
                    default:
                        throw new ArgumentOutOfRangeException();
                }
            }
        }


        //private void LookupData_LookupDataChanged(object sender, LookupDataChangedArgs e)
        //{
        //    _dataSource.Rows.Clear();
        //    foreach (DataRow dataRow in e.OutputTable.Rows)
        //    {
        //        var newDataRow = _dataSource.NewRow();
        //        foreach (var lookupColumn in LookupColumns)
        //        {
        //            var cellValue = dataRow.GetRowValue(lookupColumn.DataColumnName);
        //            if (cellValue.IsNullOrEmpty() &&
        //                (lookupColumn.KeepNullEmpty || lookupColumn.LookupColumnDefinition.KeepNullEmpty))
        //            {
        //            }
        //            else
        //            {
        //                var formattedValue = cellValue.ToString();
        //                if (lookupColumn.LookupColumnDefinition.SearchForHostId.HasValue)
        //                {
        //                    var newValue = LookupControlsGlobals.LookupControlSearchForFactory.FormatValue(
        //                        lookupColumn.LookupColumnDefinition.SearchForHostId.Value, cellValue.ToString());

        //                    if (newValue == formattedValue)
        //                    {
        //                        cellValue = lookupColumn.LookupColumnDefinition.FormatValue(cellValue);
        //                    }
        //                    else
        //                    {
        //                        cellValue = newValue;
        //                    }
        //                }
        //                else
        //                {
        //                    cellValue = lookupColumn.LookupColumnDefinition.FormatValue(cellValue);
        //                }
        //            }

        //            newDataRow[lookupColumn.DataColumnName] = cellValue;
        //        }

        //        _dataSource.Rows.Add(newDataRow);
        //    }

        //    if (ListView != null)
        //        ListView.ItemsSource = _dataSource.DefaultView;

        //    if (ScrollBar != null)
        //    {
        //        ScrollBar.IsEnabled = true;
        //        switch (e.ScrollPosition)
        //        {
        //            case LookupScrollPositions.Disabled:
        //                ScrollBar.IsEnabled = false;
        //                break;
        //            case LookupScrollPositions.Top:
        //                ScrollBar.Value = ScrollBar.Minimum;
        //                break;
        //            case LookupScrollPositions.Middle:
        //                SetScrollThumbToMiddle();
        //                break;
        //            case LookupScrollPositions.Bottom:
        //                ScrollBar.Value = ScrollBar.Maximum;
        //                break;
        //            default:
        //                throw new ArgumentOutOfRangeException();
        //        }
        //    }

        //    if (ListView != null)
        //    {
        //        ListView.SelectedIndex = e.SelectedRowIndex;
        //        var window = Window.GetWindow(this);
        //        var focusedElement = FocusManager.GetFocusedElement(window);
        //        if (focusedElement != null && focusedElement is Control focusedControl)
        //        {
        //            if (focusedElement is ListViewItem listViewItem)
        //            {
        //                ListView.Focus();
        //            }
        //        }
        //    }

        //    var setupRecordCount = true;
        //    switch (SearchType)
        //    {
        //        case LookupSearchTypes.Equals:
        //            if (e.CountingRecords)
        //                setupRecordCount = false;
        //            break;
        //        case LookupSearchTypes.Contains:
        //            if (!e.SearchForChanging)
        //                setupRecordCount = false;
        //            break;
        //        default:
        //            throw new ArgumentOutOfRangeException();
        //    }

        //    if (ShowRecordCountWait || ShowRecordCountProps)
        //    {
        //        GetRecordCountWait();
        //    }

        //    if (setupRecordCount)
        //    {
        //    }
        //    if (ShowRecordCountProps)
        //    {
        //        SetupRecordCount(LookupData.GetRecordCountWait());
        //    }
        //    else
        //    {
        //        if (setupRecordCount )
        //        {
        //            SetupRecordCount(LookupData.RecordCount);
        //        }

        //    }

        //}

        /// <summary>
        /// Sets the scroll thumb to middle.
        /// </summary>
        private void SetScrollThumbToMiddle()
        {
            if (ScrollBar != null)
            {
                double middleValue = Math.Floor((ScrollBar.Maximum - ScrollBar.Minimum) / 2);
                ScrollBar.Value = (int) middleValue - 5;
            }
        }

        /// <summary>
        /// Refreshes the data based on changes in the LookupDefinition.
        /// </summary>
        /// <param name="resetSearchFor">If set to true then reset the Search For TextBox.</param>
        /// <param name="initialSearchFor">The new Search For value.</param>
        /// <param name="parentWindowPrimaryKeyValue">The parent window's PrimaryKeyValue.</param>
        /// <param name="searchForSelectAll">Select all text in the Search For TextBox.</param>
        /// <param name="initialSearchForPrimaryKeyValue">The initial search for primary key value.</param>
        public void RefreshData(bool resetSearchFor, string initialSearchFor = "",
            PrimaryKeyValue parentWindowPrimaryKeyValue = null, bool searchForSelectAll = false,
            PrimaryKeyValue initialSearchForPrimaryKeyValue = null)
        {
            _currentPageSize = GetPageSize();
            
            if (LookupDataMaui == null || ListView == null || !LookupDataMaui.LookupDefinition.VisibleColumns.Any())
            {
                _refreshPendingData = new RefreshPendingData(initialSearchFor, parentWindowPrimaryKeyValue);
                return;
            }

            if (initialSearchForPrimaryKeyValue == null)
            {
                LookupDataMaui.GetInitData();
            }

            LookupDataMaui.ParentWindowPrimaryKeyValue = parentWindowPrimaryKeyValue;

            if (!resetSearchFor && initialSearchFor.IsNullOrEmpty())
            {
                if (SearchForHost != null)
                    initialSearchFor = SearchForHost.SearchText;
            }

            _currentPageSize = GetPageSize();

            if (String.IsNullOrEmpty(initialSearchFor) &&
                (initialSearchForPrimaryKeyValue == null || !initialSearchForPrimaryKeyValue.IsValid()))
            {
                //LookupData.GetInitData();
                LookupDataMaui.GetInitData();
            }
            else
            {
                if (SearchForHost != null)
                {
                    if (initialSearchForPrimaryKeyValue == null || !initialSearchForPrimaryKeyValue.IsValid())
                    {
                        var forceRefresh = SearchForHost.SearchText == initialSearchFor;

                        SearchForHost.SearchText =
                            initialSearchFor; //This automatically triggers LookupData.OnSearchForChange.  Only if the text value has changed.

                        if (searchForSelectAll)
                        {
                            SearchForHost.SelectAll();
                        }

                        //if (forceRefresh)
                            LookupDataMaui.OnSearchForChange(initialSearchFor, true);
                    }
                    else
                    {
                        SearchText = initialSearchFor;
                        if (searchForSelectAll)
                        {
                            SearchForHost.SelectAll();
                        }
                        LookupDataMaui.SelectPrimaryKey(initialSearchForPrimaryKeyValue);
                        if (!initialSearchFor.IsNullOrEmpty() && initialSearchForPrimaryKeyValue == null)
                        {
                            LookupDataMaui.OnSearchForChange(initialSearchFor);
                        }
                    }
                }
            }
        }

        /// <summary>
        /// Gets the size of the page.
        /// </summary>
        /// <param name="setOriginalPageSize">if set to <c>true</c> [set original page size].</param>
        /// <returns>System.Int32.</returns>
        private int GetPageSize(bool setOriginalPageSize = true)
        {
            if (ListView == null || LookupGridView == null)
                return 10;

            //var itemHeight = 0.0;
            if (_itemHeight <= 0 || ListView.Items.Count == 0)
            {
                ListView.ItemsSource = null;
                var addBlankRow = ListView.Items.Count <= 0;
                if (addBlankRow)
                {
                    ListView.Items.Add("text");
                }

                var item = ListView.Items.GetItemAt(0);
                ListView.UpdateLayout();

                var containerItem = ListView.ItemContainerGenerator.ContainerFromItem(item);

                if (containerItem is ListViewItem listViewItem)
                {
                    _itemHeight = listViewItem.ActualHeight;
                }

                if (addBlankRow)
                    ListView.Items.Clear();
            }

            ListView.UpdateLayout();
            var totalHeight = ListView.ActualHeight;
            GridViewHeaderRowPresenter header = (GridViewHeaderRowPresenter)LookupGridView.GetType()
                .GetProperty("HeaderRowPresenter", BindingFlags.NonPublic | BindingFlags.Instance)
                ?.GetValue(LookupGridView);

            if (header != null)
                totalHeight -= header.ActualHeight;

            double items = 10;
            if (_itemHeight > 0)
                items = totalHeight / _itemHeight;

            var pageSize = (int)(Math.Floor(items)) - 1;

            var scrollViewer = FindVisualChild<ScrollViewer>(ListView);

            if (scrollViewer != null && scrollViewer.ComputedHorizontalScrollBarVisibility == Visibility.Visible)
                pageSize -= 1;

            if (setOriginalPageSize)
                _originalPageSize = pageSize;
            return pageSize;

        }

        /// <summary>
        /// Finds the visual child.
        /// </summary>
        /// <typeparam name="TChildItem">The type of the t child item.</typeparam>
        /// <param name="obj">The object.</param>
        /// <returns>TChildItem.</returns>
        private TChildItem FindVisualChild<TChildItem>(DependencyObject obj)
            where TChildItem : DependencyObject

        {
            for (int i = 0; i < VisualTreeHelper.GetChildrenCount(obj); i++)
            {
                DependencyObject child = VisualTreeHelper.GetChild(obj, i);
                if (child is TChildItem)
                    return (TChildItem)child;
                else
                {
                    TChildItem childOfChild = FindVisualChild<TChildItem>(child);
                    if (childOfChild != null)
                        return childOfChild;
                }
            }
            return null;
        }

        /// <summary>
        /// Gets the width from percent.
        /// </summary>
        /// <param name="control">The control.</param>
        /// <param name="percentWidth">Width of the percent.</param>
        /// <returns>System.Double.</returns>
        public static double GetWidthFromPercent(Control control, double percentWidth)
        {
            double width = 0;
            if (percentWidth > 0)
            {
                var controlWidth = control.ActualWidth;
                width = Math.Floor(controlWidth * (percentWidth / 100)) - 5;
            }
            return width;
        }

        /// <summary>
        /// Handles the SelectionChanged event of the ListView control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="SelectionChangedEventArgs"/> instance containing the event data.</param>
        /// <exception cref="System.InvalidOperationException"></exception>
        private void ListView_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (e.AddedItems.Count > 0)
            {
                var index = ListView.Items.IndexOf(e.AddedItems[0] ?? throw new InvalidOperationException());
                SelectedIndexChanged?.Invoke(this, EventArgs.Empty);
                //LookupData.SelectedRowIndex = index;
            }
        }

        /// <summary>
        /// Handles the <see cref="E:ListViewKeyDown" /> event.
        /// </summary>
        /// <param name="e">The <see cref="KeyEventArgs"/> instance containing the event data.</param>
        private void OnListViewKeyDown(KeyEventArgs e)
        {
            if (SearchForHost != null && SearchForHost.Control.IsKeyboardFocusWithin)
            {
                if (!SearchForHost.CanProcessSearchForKey(e.Key))
                {
                    return;
                }
            }

            if (Keyboard.IsKeyDown(Key.LeftCtrl) || Keyboard.IsKeyDown(Key.RightCtrl))
            {
                int? sortColumnIndex = null;
                switch (e.Key)
                {
                    case Key.D1:
                    case Key.NumPad1:
                        sortColumnIndex = 0;
                        break;
                    case Key.D2:
                    case Key.NumPad2:
                        sortColumnIndex = 1;
                        break;
                    case Key.D3:
                    case Key.NumPad3:
                        sortColumnIndex = 2;
                        break;
                    case Key.D4:
                    case Key.NumPad4:
                        sortColumnIndex = 3;
                        break;
                    case Key.D5:
                    case Key.NumPad5:
                        sortColumnIndex = 4;
                        break;
                    case Key.D6:
                    case Key.NumPad6:
                        sortColumnIndex = 5;
                        break;
                    case Key.D7:
                    case Key.NumPad7:
                        sortColumnIndex = 6;
                        break;
                    case Key.D8:
                    case Key.NumPad8:
                        sortColumnIndex = 7;
                        break;
                    case Key.D9:
                    case Key.NumPad9:
                        sortColumnIndex = 8;
                        break;
                    case Key.D0:
                    case Key.NumPad0:
                        sortColumnIndex = 9;
                        break;
                }

                if (sortColumnIndex != null)
                {
                    OnColumnClick((int) sortColumnIndex, false);
                    e.Handled = true;
                    return;
                }
            }

            switch (e.Key)
            {
                case Key.Down:
                    OnDownArrow();
                    e.Handled = true;
                    break;
                case Key.Up:
                    OnUpArrow();
                    e.Handled = true;
                    break;
                case Key.PageDown:
                    OnPageDown();
                    e.Handled = true;
                    break;
                case Key.PageUp:
                    OnPageUp();
                    e.Handled = true;
                    break;
                case Key.End:
                    OnEnd();
                    e.Handled = true;
                    break;
                case Key.Home:
                    OnHome();
                    e.Handled = true;
                    break;
                case Key.Enter:
                    if (OnEnter())
                        e.Handled = true;
                    break;
            }
        }

        /// <summary>
        /// Called when [down arrow].
        /// </summary>
        protected void OnDownArrow()
        {
            if (ListView == null)
                return;

            var selIndex = ListView.SelectedIndex;
            if (selIndex >= ListView.Items.Count - 1)
            {
                //LookupData.GotoNextRecord();
                LookupDataMaui.GotoNextRecord();
            }
            else
            {
                ListView.SelectedIndex = selIndex + 1;
            }
        }

        /// <summary>
        /// Called when [up arrow].
        /// </summary>
        protected void OnUpArrow()
        {
            if (ListView == null)
                return;

            var selIndex = ListView.SelectedIndex;
            if (selIndex <= 0)
            {
                LookupDataMaui.GotoPreviousRecord();
            }
            else
            {
                ListView.SelectedIndex = selIndex - 1;
            }
        }

        /// <summary>
        /// Called when [page down].
        /// </summary>
        /// <param name="checkSelectedIndex">if set to <c>true</c> [check selected index].</param>
        private void OnPageDown(bool checkSelectedIndex = true)
        {
            if (ListView == null)
                return;

            //LookupDataMaui.OnMouseWheelForward(); //For debugging purposes only. I'm a quadriplegic and it's very difficult for me to use a mouse wheel.

            //Comment out below code block when debugging mouse wheel.

            var selIndex = ListView.SelectedIndex;
            if (selIndex >= ListView.Items.Count - 1 || !checkSelectedIndex)
                LookupDataMaui.GotoNextPage();
            else
                ListView.SelectedIndex = ListView.Items.Count - 1;
        }

        /// <summary>
        /// Called when [page up].
        /// </summary>
        /// <param name="checkSelectedIndex">if set to <c>true</c> [check selected index].</param>
        private void OnPageUp(bool checkSelectedIndex = true)
        {
            if (ListView == null)
                return;

            //LookupDataMaui.OnMouseWheelBack(); //For debugging purposes only. I'm a quadriplegic and it's very difficult for me to use a mouse wheel.

            //Comment out below code block when debugging mouse wheel.

            var selIndex = ListView.SelectedIndex;
            if (selIndex <= 0 || !checkSelectedIndex)
                LookupDataMaui.GotoPreviousPage();
            else
                ListView.SelectedIndex = 0;
        }

        /// <summary>
        /// Called when [end].
        /// </summary>
        /// <param name="checkSelectedIndex">if set to <c>true</c> [check selected index].</param>
        private void OnEnd(bool checkSelectedIndex = true)
        {
            if (ListView == null)
                return;

            var selIndex = ListView.SelectedIndex;

            if (selIndex >= ListView.Items.Count - 1 || !checkSelectedIndex)
                LookupDataMaui.GotoBottom();
            else
                ListView.SelectedIndex = ListView.Items.Count - 1;
        }

        /// <summary>
        /// Called when [home].
        /// </summary>
        /// <param name="checkSelectedIndex">if set to <c>true</c> [check selected index].</param>
        private void OnHome(bool checkSelectedIndex = true)
        {
            if (ListView == null)
                return;

            var selIndex = ListView.SelectedIndex;

            if (selIndex <= 0 || !checkSelectedIndex)
                LookupDataMaui.GotoTop();
            else
                ListView.SelectedIndex = 0;
        }

        /// <summary>
        /// Called when [search type changed].
        /// </summary>
        private void OnSearchTypeChanged()
        {
            //if (!SearchText.IsNullOrEmpty())
            //    LookupData.ResetRecordCount();

            if (SearchForHost != null)
            {
                SearchForHost.Control.Focus();
                RefreshData(false, SearchForHost.SearchText);
            }
        }

        /// <summary>
        /// Gets the record count wait.
        /// </summary>
        /// <returns>System.Int32.</returns>
        public int GetRecordCountWait()
        {
            if (LookupDataMaui != null)
            {
                var result = LookupDataMaui.GetRecordCount();

                ShowRecordCountLabel();
                var recordsText = result == 1 ? "" : "s";
                RecordCountControl.Text =
                    $@"{result.ToString(GblMethods.GetNumFormat(0, false))} Record{recordsText} Found";
                RecordCountWait = result;
                return result;
            }
            ShowRecordCountLabel();
            return 0;
        }

        /// <summary>
        /// Gets the record count button click.
        /// </summary>
        public async void GetRecordCountButtonClick()
        {
            ShowRecordCountLabel();

            if (Spinner != null)
                Spinner.Visibility = Visibility.Visible;

            var recordCount = LookupDataMaui.GetRecordCount();

            if (Spinner != null)
                Spinner.Visibility = Visibility.Collapsed;

            ShowRecordCount(recordCount, true);
        }

        /// <summary>
        /// Shows the advanced find.
        /// </summary>
        private void ShowAdvancedFind()
        {
            //advancedFindWindow.Owner = Window.GetWindow(this);
            //var lookupData =
            //    new LookupDataBase(new LookupDefinitionBase(SystemGlobals.AdvancedFindLookupContext.AdvancedFinds),
            //        this);

            var lookupData =
                SystemGlobals.AdvancedFindLookupContext.AdvancedFindLookup.GetLookupDataMaui(LookupDefinition, true);

            var addViewArgs = new LookupAddViewArgs(lookupData, true, LookupFormModes.View, 
                "", Window.GetWindow(this));

            addViewArgs.InputParameter = new AdvancedFindInput
            {
                InputParameter = AddViewParameter,
                LockTable = LookupDefinition.TableDefinition,
                LookupDefinition = LookupDefinition,
                LookupWidth = ActualWidth,
            };

            var advancedFindWindow = new AdvancedFindWindow(addViewArgs);
            advancedFindWindow.Owner = Window.GetWindow(this);
            advancedFindWindow.ShowInTaskbar = false;
            
            advancedFindWindow.ShowDialog();
            if (advancedFindWindow.ApplyToLookupDefinition)
            {
                var lookupWindow = this.GetParentOfType<LookupWindow>();
                if (lookupWindow != null)
                {
                    lookupWindow.ApplyNewLookupDefinition(advancedFindWindow.ViewModel.LookupDefinition);
                }
                LookupDefinition = advancedFindWindow.ViewModel.LookupDefinition;
                _commandToExecute = new LookupCommand(LookupCommands.Reset);
                if (lookupWindow != null)
                {
                    lookupWindow.Reload();
                }
            }
        }

        /// <summary>
        /// Setups the record count.
        /// </summary>
        /// <param name="recordCount">The record count.</param>
        private void SetupRecordCount(int recordCount)
        {
            if (GetRecordCountButton == null || RecordCountControl == null || RecordCountStackPanel == null)
                return;

            var showRecordCount = ShowRecordCountProps;
            if (LookupDataMaui?.ScrollPosition == LookupScrollPositions.Disabled)
            {
                showRecordCount = true;
            }
            else if (_commandToExecute != null && _commandToExecute.ClearColumns)
            {
                showRecordCount = true;
            }
            //else if (recordCount > 0)
            //    showRecordCount = true;

            ShowRecordCount(recordCount, showRecordCount);
        }

        /// <summary>
        /// Shows the record count.
        /// </summary>
        /// <param name="recordCount">The record count.</param>
        /// <param name="showRecordCount">if set to <c>true</c> [show record count].</param>
        public void ShowRecordCount(int recordCount, bool showRecordCount)
        {
            Dispatcher.Invoke(() =>
            {
                if (!showRecordCount)
                {
                    showRecordCount = ShowRecordCountProps;
                    if (showRecordCount && recordCount == 0)
                    {
                        recordCount = LookupDataMaui.GetRecordCount();
                    }
                }

                if (showRecordCount)
                {
                    ShowRecordCountLabel();
                    var recordsText = recordCount == 1 ? "" : "s";
                    RecordCountControl.Text =
                        $@"{recordCount.ToString(GblMethods.GetNumFormat(0, false))} Record{recordsText} Found";
                }
                else
                {
                    RecordCountStackPanel.Visibility = Visibility.Hidden;
                    GetRecordCountButton.Visibility = Visibility.Visible;
                }
            });
        }

        /// <summary>
        /// Shows the record count label.
        /// </summary>
        private void ShowRecordCountLabel()
        {
            if (GetRecordCountButton == null || RecordCountControl == null || RecordCountStackPanel == null)
                return;

            GetRecordCountButton.Visibility = Visibility.Hidden;
            RecordCountStackPanel.Visibility = Visibility.Visible;
            RecordCountControl.Text = @"Counting Records";
        }

        /// <summary>
        /// Called when [enter].
        /// </summary>
        /// <returns><c>true</c> if XXXX, <c>false</c> otherwise.</returns>
        private bool OnEnter()
        {
            if (ListView == null || LookupDefinition.ReadOnlyMode || !AllowOnEnter)
            {
                SystemSounds.Exclamation.Play();
            }
            else
            {
                _selectedIndex = ListView.SelectedIndex;
                if (_selectedIndex >= 0)
                {
                    var ownerWindow = Window.GetWindow(this);
                    //03/16/2023
                    //ownerWindow.Activated += OwnerWindow_Activated;
                    LookupDataMaui.ViewSelectedRow(ownerWindow, AddViewParameter, _readOnlyMode);
                    return true;
                }
            }

            return false;
        }

        /// <summary>
        /// Handles the Activated event of the OwnerWindow control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="EventArgs"/> instance containing the event data.</param>
        private void OwnerWindow_Activated(object sender, EventArgs e)
        {
            var ownerWindow = Window.GetWindow(this);
            ownerWindow.Activated -= OwnerWindow_Activated;

            if (!ForceRefreshOnActivate)
            {
                return;
            }
            //Peter Ringering - 09/25/2022 - E-273;
            //RefreshData(false, "", LookupData.ParentWindowPrimaryKeyValue);
            if (SearchForHost != null && SearchForHost.SearchText.IsNullOrEmpty())
            {
                ListView.SelectedIndex = _selectedIndex;
            }
            else
            {
                if (SearchForHost != null) LookupDataMaui.OnSearchForChange(SearchForHost.SearchText);
            }

            if (SearchForHost != null) SearchForHost.Control.Focus();
        }

        /// <summary>
        /// Executes the command.
        /// </summary>
        /// <exception cref="System.ArgumentOutOfRangeException"></exception>
        private void ExecuteCommand()
        {
            var command = _commandToExecute;
            if (command != null)
            {
                switch (command.Command)
                {
                    case LookupCommands.Clear:
                        ClearLookupControl();
                        LookupDefinition.FilterDefinition.ClearFixedFilters();
                        if (AdvancedFindButton != null) 
                            AdvancedFindButton.IsEnabled = false;
                        else
                        {
                            _advancedFindMode = AdvancedFindModes.Disabled;
                        }
                        break;
                    case LookupCommands.Refresh:
                        this.AddViewParameter = command.AddViewParameter;
                        RefreshData(command.ResetSearchFor, String.Empty, command.ParentWindowPrimaryKeyValue);
                        if (AdvancedFindButton != null) 
                            AdvancedFindButton.IsEnabled = true;
                        else
                        {
                            _advancedFindMode = AdvancedFindModes.Enabled;
                        }
                        break;
                    case LookupCommands.AddModify:
                        if (!_setupRan)
                        {
                            SetupControl();
                        }
                        var addViewParameter = command.AddViewParameter;
                        if (addViewParameter == null)
                            addViewParameter = this.AddViewParameter;

                        var selectedIndex = ListView?.SelectedIndex ?? 0;
                        if (selectedIndex >= 0)
                            LookupDataMaui.ViewSelectedRow(Window.GetWindow(this), addViewParameter);
                        else
                            LookupDataMaui.AddNewRow(Window.GetWindow(this), addViewParameter);
                        RefreshData(command
                            .ResetSearchFor, String.Empty, LookupDataMaui.ParentWindowPrimaryKeyValue);
                        break;
                    case LookupCommands.Reset:
                        ClearLookupControl();
                        if (!command.ClearColumns)
                        {
                            SetupControl();
                            if (LookupDefinition != null && LookupDefinition.VisibleColumns.Any())
                            {
                                _resettingSearchFor = true;
                                this.AddViewParameter = command.AddViewParameter;
                                _currentPageSize = GetPageSize();
                                RefreshData(true);
                                _resettingSearchFor = false;
                                if (AdvancedFindButton != null)
                                    AdvancedFindButton.IsEnabled = true;
                                else
                                {
                                    _advancedFindMode = AdvancedFindModes.Enabled;
                                }
                            }
                        }
                        else
                        {
                            ResetColumnHeaderSort(0);
                            ClearLookupControl();
                            SearchForHost = null;
                            SearchForStackPanel.Children.Clear();
                            LookupColumns.Clear();
                            LookupGridView?.Columns.Clear();
                            _columnMaps.ClearColumns();
                            _dataSource.Clear();
                            SetupRecordCount(0);
                            UpdateLayout();
                            SetActiveColumn(-1, FieldDataTypes.String);
                        }

                        //LookupData.GetInitData();
                        break;
                    default:
                        throw new ArgumentOutOfRangeException();
                }
            }
        }

        /// <summary>
        /// Clears all the data in the list view.
        /// </summary>
        public void ClearLookupControl()
        {
            _refreshPendingData = null;
            if (LookupDataMaui != null)
            {
                ShowRecordCountWait = false;
                ShowRecordCountProps = false;
                LookupDataMaui.ClearData();
            }
            
            if (EqualsRadioButton != null)
                EqualsRadioButton.IsChecked = true;

            if (SearchForHost != null)
                SearchForHost.SearchText = String.Empty;

            if (ListTextBox != null)
            {
                ListTextBox.Visibility = Visibility.Collapsed;
            }

            if (GetRecordCountButton != null)
            {
                GetRecordCountButton.Visibility = Visibility.Collapsed;
            }

            if (RecordCountStackPanel != null)
            {
                RecordCountStackPanel.Visibility = Visibility.Collapsed;
            }

            if (AdvancedFindButton != null)
            {
                AdvancedFindButton.IsEnabled = false;
            }
        }

        /// <summary>
        /// Sets the design text.
        /// </summary>
        private void SetDesignText()
        {
            if (DesignerProperties.GetIsInDesignMode(this) && !DesignText.IsNullOrEmpty())
            {
                if (_designModeSearchForTextBox != null)
                    _designModeSearchForTextBox.Text = DesignText;
            }
        }

        /// <summary>
        /// Sets the read only mode.
        /// </summary>
        /// <param name="readOnlyValue">if set to <c>true</c> [read only value].</param>
        public void SetReadOnlyMode(bool readOnlyValue)
        {
            _readOnlyMode = readOnlyValue;
        }

        /// <summary>
        /// Sets the lookup window.
        /// </summary>
        /// <param name="lookupWindow">The lookup window.</param>
        public void SetLookupWindow(ILookupWindow lookupWindow)
        {
            LookupWindow = lookupWindow;
            LookupDataMaui?.SetParentControls(this, lookupWindow);
        }


    }
}