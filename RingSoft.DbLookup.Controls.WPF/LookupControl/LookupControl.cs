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
    public enum AdvancedFindModes
    {
        Enabled = 1,
        Disabled = 2,
        Done = 3
    }

    public class DataSource : ObservableCollection<DataItem>
    {
        public void UpdateSource()
        {
            OnCollectionChanged(new NotifyCollectionChangedEventArgs(NotifyCollectionChangedAction.Reset));
        }
    }

    public class DataColumn
    {
        public int ColumnIndex { get; }

        public string ColumnName { get; }

        public DataColumn(int columnIndex, string columnName)
        {
            ColumnIndex = columnIndex;
            ColumnName = columnName;
        }
    }

    public class DataColumnMaps
    {
        public List<DataColumn> ColumnMaps { get; } = new List<DataColumn>();

        public void AddColumn(string columnName)
        {
            var columnMap = new DataColumn(ColumnMaps.Count, columnName);
            ColumnMaps.Add(columnMap);
        }

        public string GetVisibleColumnName(string dataColumnName)
        {
            var map = ColumnMaps.FirstOrDefault(p => p.ColumnName == dataColumnName);
            var visibleColumnName = $"Column{map.ColumnIndex}";
            return visibleColumnName;
        }

        public void ClearColumns()
        {
            ColumnMaps.Clear();
        }
}

    public class DataItem
    {
        public string Column0 { get; set; }
        public string Column1 { get; set; }
        public string Column2 { get; set; }
        public string Column3 { get; set; }
        public string Column4 { get; set; }
        public string Column5 { get; set; }
        public string Column6 { get; set; }
        public string Column7 { get; set; }
        public string Column8 { get; set; }
        public string Column9 { get; set; }

        public DataColumnMaps ColumnMaps { get; }

        public DataItem(DataColumnMaps columnMaps)
        {
            ColumnMaps = columnMaps;
        }
        
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
    /// Follow steps 1a or 1b and then 2 to use this custom control in a XAML file.
    ///
    /// Step 1a) Using this custom control in a XAML file that exists in the current project.
    /// Add this XmlNamespace attribute to the root element of the markup file where it is 
    /// to be used:
    ///
    ///     xmlns:MyNamespace="clr-namespace:RingSoft.DbLookup.Controls.WPF.LookupControl"
    ///
    ///
    /// Step 1b) Using this custom control in a XAML file that exists in a different project.
    /// Add this XmlNamespace attribute to the root element of the markup file where it is 
    /// to be used:
    ///
    ///     xmlns:MyNamespace="clr-namespace:RingSoft.DbLookup.Controls.WPF.LookupControl;assembly=RingSoft.DbLookup.Controls.WPF.LookupControl"
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
    ///     <MyNamespace:NewLookupControl/>
    ///
    /// </summary>
    public class LookupColumnWidthChangedArgs
    {
        public LookupColumnDefinitionBase ColumnDefinition { get; set; }
        public SizeChangedEventArgs SizeChangedEventArgs { get; set; }
    }

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
    public class LookupControl : Control, ILookupControl, IReadOnlyControl
    {
        private class RefreshPendingData
        {
            public string InitialSearchFor { get; }

            public PrimaryKeyValue ParentWindowPrimaryKeyValue { get; }

            public RefreshPendingData(string initialSearchFor, PrimaryKeyValue parentWindowPrimaryKeyValue)
            {
                InitialSearchFor = initialSearchFor;
                ParentWindowPrimaryKeyValue = parentWindowPrimaryKeyValue;
            }
        }

        public Label SearchForLabel { get; set; }

        public RadioButton EqualsRadioButton { get; set; }

        public RadioButton ContainsRadioButton { get; set; }

        public StackPanel SearchForStackPanel { get; set; }

        public ListView ListView { get; set; }

        public GridView LookupGridView { get; set; }

        public ScrollBar ScrollBar { get; set; }

        public Button GetRecordCountButton { get; set; }

        public StackPanel RecordCountStackPanel { get; set; }

        public StringReadOnlyBox RecordCountControl { get; set; }

        public Control Spinner { get; set; }

        public Button AdvancedFindButton { get; set; }

        public bool ShowRecordCountProps { get; set; }

        //--------------------------------------------------------------

        private LookupSearchForHost _lookupSearchForHost;
        private bool _readOnlyMode;

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

        public int PageSize => _currentPageSize;

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

        public void SetLookupIndex(int index)
        {
            ListView.SelectedIndex = index;
        }

        public static readonly DependencyProperty LookupDefinitionProperty =
            DependencyProperty.Register("LookupDefinition", typeof(LookupDefinitionBase), typeof(LookupControl),
                new FrameworkPropertyMetadata(LookupDefinitionChangedCallback));

        /// <summary>
        /// Gets or sets the lookup definition.
        /// </summary>
        /// <value>
        /// The lookup definition.
        /// </value>
        public LookupDefinitionBase LookupDefinition
        {
            get { return (LookupDefinitionBase)GetValue(LookupDefinitionProperty); }
            set { SetValue(LookupDefinitionProperty, value); }
        }

        private static void LookupDefinitionChangedCallback(DependencyObject obj,
            DependencyPropertyChangedEventArgs args)
        {
            var lookupControl = (LookupControl)obj;
            if (lookupControl._controlLoaded)
                lookupControl.SetupControl();
        }

        public static readonly DependencyProperty CommandProperty =
            DependencyProperty.Register("Command", typeof(LookupCommand), typeof(LookupControl),
                new FrameworkPropertyMetadata(CommandChangedCallback));

        /// <summary>
        /// Gets or sets the LookupCommand which is used by view models to tell this control to either refresh, clear etc.
        /// </summary>
        /// <value>
        /// The command.
        /// </value>
        public LookupCommand Command
        {
            get { return (LookupCommand)GetValue(CommandProperty); }
            set { SetValue(CommandProperty, value); }
        }

        private static void CommandChangedCallback(DependencyObject obj,
            DependencyPropertyChangedEventArgs args)
        {
            var lookupControl = (LookupControl)obj;
            lookupControl.ExecuteCommand();
        }

        public static readonly DependencyProperty DataSourceChangedProperty =
            DependencyProperty.Register("DataSourceChanged", typeof(LookupDataSourceChanged), typeof(LookupControl));

        public LookupDataSourceChanged DataSourceChanged
        {
            get { return (LookupDataSourceChanged)GetValue(DataSourceChangedProperty); }
            set { SetValue(DataSourceChangedProperty, value); }
        }

        public static readonly DependencyProperty DesignTextProperty =
            DependencyProperty.Register("DesignText", typeof(string), typeof(LookupControl),
                new FrameworkPropertyMetadata(DesignTextChangedCallback));

        public string DesignText
        {
            get { return (string)GetValue(DesignTextProperty); }
            set { SetValue(DesignTextProperty, value); }
        }

        private static void DesignTextChangedCallback(DependencyObject obj,
            DependencyPropertyChangedEventArgs args)
        {
            var lookupControl = (LookupControl)obj;
            lookupControl.SetDesignText();
        }

        /// <summary>
        /// Gets a list of LookupColumns which allow adding new columns.
        /// </summary>
        /// <value>
        /// The lookup columns.
        /// </value>
        public ObservableCollection<LookupColumnBase> LookupColumns { get; }

        //public LookupDataBase LookupData { get; private set; }

        public LookupDataMauiBase LookupDataMaui { get; private set; }

        public bool LookupWindowReadOnlyMode { get; internal set; }

        public object AddViewParameter { get; internal set; }

        public bool ShowAdvancedFindButton { get; set; } = true;

        public bool ForceRefreshOnActivate { get; set; } = true;

        public int RecordCountWait { get; set; }

        public bool ShowRecordCountWait { get; set; }

        public ILookupWindow LookupWindow { get; private set; }

        /// <summary>
        /// Occurs when a user wishes to add or view a selected lookup row.  Set Handled property to True to not send this message to the LookupContext.
        /// </summary>
        public event EventHandler<LookupAddViewArgs> LookupView;

        public event EventHandler<LookupColumnWidthChangedArgs> ColumnWidthChanged;

        public event EventHandler SelectedIndexChanged;

        private bool _controlLoaded;
        private bool _onLoadRan;
        private bool _setupRan;
        private int _originalPageSize;
        private int _currentPageSize;
        //private DataTable _dataSource = new DataTable("DataSourceTable");
        private DataColumnMaps _columnMaps = new DataColumnMaps();
        private DataSource _dataSource = new DataSource();

        GridViewColumnHeader _lastHeaderClicked;
        ListSortDirection _lastDirection = ListSortDirection.Ascending;
        private double _preScrollThumbPosition;
        private int _selectedIndex;

        private RefreshPendingData _refreshPendingData;

        private bool _resettingSearchFor;
        private double _itemHeight;
        private int _designSortIndex = -1;
        private double _designModeHeaderLineHeight;
        private TextBox _designModeSearchForTextBox;
        private AdvancedFindModes _advancedFindMode = AdvancedFindModes.Done;

        static LookupControl()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(LookupControl), new FrameworkPropertyMetadata(typeof(LookupControl)));

            IsTabStopProperty.OverrideMetadata(typeof(LookupControl), new FrameworkPropertyMetadata(false));

            KeyboardNavigation.TabNavigationProperty.OverrideMetadata(typeof(LookupControl),
                new FrameworkPropertyMetadata(KeyboardNavigationMode.Local));
        }

        public LookupControl()
        {
            var d = DependencyPropertyDescriptor.FromProperty(IsVisibleProperty, typeof(LookupControl));
            d.AddValueChanged(this, OnIsVisiblePropertyChanged);

            LookupColumns = new ObservableCollection<LookupColumnBase>();

            LookupColumns.CollectionChanged += (sender, args) => OnLookupColumnsChanged();

            Loaded += (sender, args) => OnLoad();
        }

        private void OnIsVisiblePropertyChanged(object sender, EventArgs e)
        {
            LoadOnIsVisible();
        }

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

        private void ListView_SelectionChanged1(object sender, SelectionChangedEventArgs e)
        {
            throw new NotImplementedException();
        }

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

        protected override void OnGotFocus(RoutedEventArgs e)
        {
            SearchForHost?.Control.Focus();

            base.OnGotFocus(e);
        }

        private void SetupControl()
        {
            if (Command != null && (Command.ClearColumns || LookupDefinition == null))
                return;

            //if (LookupDefinition.InitialSortColumnDefinition == null)
            //    throw new ArgumentException(
            //        "Lookup definition does not have any visible columns defined or its initial sort column is null.");

            if (LookupDataMaui != null)
            {
                ClearLookupControl();
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

            if (_refreshPendingData != null)
            {
                RefreshData(true, _refreshPendingData.InitialSearchFor, _refreshPendingData.ParentWindowPrimaryKeyValue,
                    true);
                _refreshPendingData = null;
            }

            _setupRan = _controlLoaded = true;
        }

        private void ListView_PreviewKeyDown(object sender, KeyEventArgs e)
        {
            OnListViewKeyDown(e);
        }

        private void SearchForControl_TextChanged(object sender, EventArgs e)
        {
            if (!_resettingSearchFor)
                LookupDataMaui.OnSearchForChange(SearchForHost.SearchText);
        }

        private void SearchForControl_PreviewKeyDown(object sender, KeyEventArgs e)
        {
            OnListViewKeyDown(e);
        }

        private void Control_PreviewLostKeyboardFocus(object sender, KeyboardFocusChangedEventArgs e)
        {
            if (ReferenceEquals(e.NewFocus, ListView))
                e.Handled = true;
        }

        private void MergeLookupDefinition()
        {
            if (LookupColumns.FirstOrDefault(f => f.PropertyName == LookupDefinition.InitialSortColumnDefinition.PropertyName) == null)
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
                        lookupColumn.TextAlignment = lookupColumnDefinition.HorizontalAlignment;

                lookupColumnDefinition.UpdateCaption(lookupColumnBase.Header);

                AddColumnToGrid(lookupColumnBase);
            }
        }

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

        private int GetIndexOfVisibleColumnDefinition(LookupColumnDefinitionBase lookupColumnDefinition)
        {
            var lookupColumn = LookupColumns.FirstOrDefault(f => f.LookupColumnDefinition == lookupColumnDefinition);
            return LookupColumns.IndexOf(lookupColumn);
        }

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

        private void ListView_PreviewMouseWheel(object sender, MouseWheelEventArgs e)
        {
            e.Handled = true;
            if (e.Delta > 0)
                LookupDataMaui.OnMouseWheelForward(); //See OnPageDown() for quadriplegic debugging.
            else
                LookupDataMaui.OnMouseWheelBack(); //See OnPageUp() for quadriplegic debugging.
        }

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

        private void LookupData_DataSourceChanged(object sender, EventArgs e)
        {
            DataSourceChanged = new LookupDataSourceChanged();
        }


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

        private void OnColumnClick(int columnIndex, bool mouseClick)
        {
            if (LookupGridView == null)
                return;

            if (columnIndex > LookupGridView.Columns.Count - 1)
            {
                SystemSounds.Exclamation.Play();
                return;
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
                    if (first)
                    {
                        first = false;
                    }
                    else
                    {
                        var orderColumnIndex = GetIndexOfVisibleColumnDefinition(lookupColumnDefinition);
                        var orderGridColumnHeader =
                            LookupGridView.Columns[orderColumnIndex].Header as GridViewColumnHeader;
                        GridViewSort.AddNonPrimarySortGlyph(orderGridColumnHeader, columnNumber);
                        columnNumber++;
                    }
                }

                //if (resetSortOrder)
                //    SetActiveColumn(columnIndex, LookupData.SortColumnDefinition.DataType);

                SearchForHost?.Control.Focus();
            }
        }

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
                    //LookupData?.OnChangePageSize();
                }
            }
        }

        private void LookupDataMaui_LookupDataChanged(object sender, LookupDataMauiOutput e)
        {
            _dataSource.Clear();
            
            if (sender is LookupDataMauiBase lookupDataMaui)
            {
                for (int row = 0; row < lookupDataMaui.RowCount; row++)
                {
                    var dataItem = new DataItem(_columnMaps);
                    foreach (var lookupColumn in LookupColumns)
                    {
                        var joinProperty = lookupColumn.LookupColumnDefinition.GetPropertyJoinName();
                        var value = lookupDataMaui.GetFormattedRowValue(row, lookupColumn.LookupColumnDefinition);
                        dataItem.SetColumnValue(lookupColumn.LookupColumnDefinition.SelectSqlAlias, value);
                    }
                    _dataSource.Add(dataItem);
                }

                if (_dataSource.Any())
                {

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

            LookupDataMaui.GetInitData();

            LookupDataMaui.ParentWindowPrimaryKeyValue = parentWindowPrimaryKeyValue;

            if (!resetSearchFor && initialSearchFor.IsNullOrEmpty())
            {
                if (SearchForHost != null)
                    initialSearchFor = SearchForHost.SearchText;
            }

            _currentPageSize = GetPageSize();

            if (String.IsNullOrEmpty(initialSearchFor) &&
                (initialSearchForPrimaryKeyValue == null || !initialSearchForPrimaryKeyValue.IsValid))
            {
                //LookupData.GetInitData();
                LookupDataMaui.GetInitData();
            }
            else
            {
                if (SearchForHost != null)
                {
                    if (initialSearchForPrimaryKeyValue == null || !initialSearchForPrimaryKeyValue.IsValid)
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
                        if (!initialSearchFor.IsNullOrEmpty())
                        {
                            LookupDataMaui.OnSearchForChange(initialSearchFor);
                        }
                    }
                }
            }
        }

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

        private void ListView_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (e.AddedItems.Count > 0)
            {
                var index = ListView.Items.IndexOf(e.AddedItems[0] ?? throw new InvalidOperationException());
                SelectedIndexChanged?.Invoke(this, EventArgs.Empty);
                //LookupData.SelectedRowIndex = index;
            }
        }

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

        private void ShowAdvancedFind()
        {
            //advancedFindWindow.Owner = Window.GetWindow(this);
            //var lookupData =
            //    new LookupDataBase(new LookupDefinitionBase(SystemGlobals.AdvancedFindLookupContext.AdvancedFinds),
            //        this);

            var lookupData =
                SystemGlobals.AdvancedFindLookupContext.AdvancedFindLookup.GetLookupDataMaui(LookupDefinition, true);

            var addViewArgs = new LookupAddViewArgs(lookupData, true, LookupFormModes.View, 
                "", Window.GetWindow(this))
            {
            };

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
                Command = new LookupCommand(LookupCommands.Reset);
                if (lookupWindow != null)
                {
                    lookupWindow.Reload();
                }
            }
        }

        private void SetupRecordCount(int recordCount)
        {
            if (GetRecordCountButton == null || RecordCountControl == null || RecordCountStackPanel == null)
                return;

            var showRecordCount = ShowRecordCountProps;
            if (LookupDataMaui?.ScrollPosition == LookupScrollPositions.Disabled)
            {
                showRecordCount = true;
            }
            else if (Command != null && Command.ClearColumns)
            {
                showRecordCount = true;
            }
            //else if (recordCount > 0)
            //    showRecordCount = true;

            ShowRecordCount(recordCount, showRecordCount);
        }

        public void ShowRecordCount(int recordCount, bool showRecordCount)
        {
            Dispatcher.Invoke(() =>
            {
                if (!showRecordCount)
                {
                    showRecordCount = ShowRecordCountProps;
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

        private void ShowRecordCountLabel()
        {
            if (GetRecordCountButton == null || RecordCountControl == null || RecordCountStackPanel == null)
                return;

            GetRecordCountButton.Visibility = Visibility.Hidden;
            RecordCountStackPanel.Visibility = Visibility.Visible;
            RecordCountControl.Text = @"Counting Records";
        }

        private bool OnEnter()
        {
            if (ListView == null || LookupDefinition.ReadOnlyMode)
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

        private void ExecuteCommand()
        {
            var command = Command;
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
                        RefreshData(command.ResetSearchFor, String.Empty, LookupDataMaui.ParentWindowPrimaryKeyValue);
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
        }

        private void SetDesignText()
        {
            if (DesignerProperties.GetIsInDesignMode(this) && !DesignText.IsNullOrEmpty())
            {
                if (_designModeSearchForTextBox != null)
                    _designModeSearchForTextBox.Text = DesignText;
            }
        }

        public void SetReadOnlyMode(bool readOnlyValue)
        {
            _readOnlyMode = readOnlyValue;
        }

        public void SetLookupWindow(ILookupWindow lookupWindow)
        {
            LookupWindow = lookupWindow;
            LookupDataMaui?.SetParentControls(this, lookupWindow);
        }


    }
}