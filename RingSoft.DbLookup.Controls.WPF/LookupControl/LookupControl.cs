using RingSoft.DataEntryControls.Engine;
using RingSoft.DbLookup.Lookup;
using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Data;
using System.Linq;
using System.Media;
using System.Reflection;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Data;
using System.Windows.Input;
using System.Windows.Media;
using RingSoft.DataEntryControls.WPF;
using RingSoft.DbLookup.Controls.WPF.AdvancedFind;
using RingSoft.DbLookup.QueryBuilder;
using RingSoft.DbMaintenance;

// ReSharper disable once CheckNamespace
namespace RingSoft.DbLookup.Controls.WPF
{
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

        public LookupDataBase LookupData { get; private set; }

        public bool LookupWindowReadOnlyMode { get; internal set; }

        public object AddViewParameter { get; internal set; }

        /// <summary>
        /// Occurs when a user wishes to add or view a selected lookup row.  Set Handled property to True to not send this message to the LookupContext.
        /// </summary>
        public event EventHandler<LookupAddViewArgs> LookupView;

        public event EventHandler<LookupColumnWidthChangedArgs> ColumnWidthChanged;

        private bool _controlLoaded;
        private bool _onLoadRan;
        private bool _setupRan;
        private int _originalPageSize;
        private int _currentPageSize;
        private DataTable _dataSource = new DataTable("DataSourceTable");
        GridViewColumnHeader _lastHeaderClicked;
        ListSortDirection _lastDirection = ListSortDirection.Ascending;
        private double _preScrollThumbPosition;

        private RefreshPendingData _refreshPendingData;

        private bool _resettingSearchFor;
        private double _itemHeight;
        private int _designSortIndex = -1;
        private double _designModeHeaderLineHeight;
        private TextBox _designModeSearchForTextBox;

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
            base.OnApplyTemplate();
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
            //if (LookupDefinition.InitialSortColumnDefinition == null)
            //    throw new ArgumentException(
            //        "Lookup definition does not have any visible columns defined or its initial sort column is null.");

            if (LookupData != null)
            {
                ClearLookupControl();
                LookupColumns.Clear();
            }

            if (LookupDefinition != null)
            {

                LookupData = new LookupDataBase(LookupDefinition, this);
                LookupData.LookupDataChanged += LookupData_LookupDataChanged;
                LookupData.DataSourceChanged += LookupData_DataSourceChanged;
                LookupData.LookupView += (sender, args) => LookupView?.Invoke(this, args);
            }

            if (!_setupRan)
            {
                SetupRecordCount();
                if (ListView != null)
                {
                    ListView.PreviewKeyDown += (sender, args) => { OnListViewKeyDown(args); };
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

            _dataSource.Columns.Clear();

            if (LookupColumns.Any())
                MergeLookupDefinition();
            else
                ImportLookupDefinition();

            var sortColumnIndex =
                GetIndexOfVisibleColumnDefinition(LookupDefinition?.InitialSortColumnDefinition);

            InitializeHeader(sortColumnIndex);
            if (LookupDefinition?.InitialSortColumnDefinition != null)
                SetActiveColumn(sortColumnIndex, LookupDefinition.InitialSortColumnDefinition.DataType);

            if (_refreshPendingData != null)
            {
                RefreshData(true, _refreshPendingData.InitialSearchFor, _refreshPendingData.ParentWindowPrimaryKeyValue,
                    true);
                _refreshPendingData = null;
            }

            _setupRan = _controlLoaded = true;
        }

        private void SearchForControl_TextChanged(object sender, EventArgs e)
        {
            if (!_resettingSearchFor)
                LookupData.OnSearchForChange(SearchForHost.SearchText);
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

                lookupColumnBase.DataColumnName = lookupColumnDefinition.SelectSqlAlias;
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
                    double columnWidth = 100;
                    if (ListView != null)
                        columnWidth = GetWidthFromPercent(ListView, column.PercentWidth);

                    var lookupColumn = LookupControlsGlobals.LookupControlColumnFactory.CreateLookupColumn(column);

                    lookupColumn.DataColumnName = column.SelectSqlAlias;
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
            var columnHeader = new GridViewColumnHeader { Content = caption }; // + "\r\nLine2\r\nLine3"};
            columnHeader.Click += GridViewColumnHeaderClickedHandler;

            var gridColumn = new GridViewColumn
            {
                Header = columnHeader,
                Width = width,
                CellTemplate = lookupColumn.GetCellDataTemplate(this, dataColumnName, this.IsDesignMode())
            };
            LookupGridView?.Columns.Add(gridColumn);
            _dataSource.Columns.Add(dataColumnName);

            if (LookupGridView != null && ListView != null)
            {
                columnHeader.SizeChanged += (sender, args) =>
                {
                    if (args.WidthChanged)
                    {
                        var lookupColumnDefinition = LookupDefinition.VisibleColumns.ToList()
                            .ElementAt(LookupGridView.Columns.IndexOf(gridColumn));
                        lookupColumnDefinition.UpdatePercentWidth(args.NewSize.Width / ListView.ActualWidth * 100);
                        ColumnWidthChanged?.Invoke(this, new LookupColumnWidthChangedArgs
                        {
                            ColumnDefinition = lookupColumnDefinition,
                            SizeChangedEventArgs = args
                        });
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
                    if (!column.Header.IsNullOrEmpty())
                    {
                        while (startIndex >= 0)
                        {
                            startIndex = column.Header.IndexOf('\n', startIndex);
                            if (startIndex >= 0)
                            {
                                lineFeedCount++;
                                startIndex++;
                            }
                        }

                        startIndex = 0;
                    }
                }

                height = _designModeHeaderLineHeight * (lineFeedCount + 1);
            }
            else
            {
                foreach (var column in LookupColumns)
                {
                    if (!column.Header.IsNullOrEmpty())
                    {
                        while (startIndex >= 0)
                        {
                            startIndex = column.Header.IndexOf('\n', startIndex);
                            if (startIndex >= 0)
                            {
                                lineFeedCount++;
                                startIndex++;
                            }
                        }

                        startIndex = 0;
                    }
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
            _dataSource.Rows.Clear();
            _dataSource.Columns.Clear();

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
            //Must get page size before clearing the data source.  Otherwise we get exceptions if column PropertyName is invalid.
            _currentPageSize = GetPageSize(false);
            _dataSource.Rows.Clear();

            for (var i = 0; i < _currentPageSize; i++)
            {
                var newDataRow = _dataSource.NewRow();
                foreach (var column in LookupColumns)
                {
                    var cellValue = column.DesignText;
                    newDataRow[column.DataColumnName] = cellValue;
                }

                _dataSource.Rows.Add(newDataRow);
            }

            if (ListView != null)
                ListView.ItemsSource = _dataSource.DefaultView;
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
                LookupData.OnMouseWheelForward(); //See OnPageDown() for quadriplegic debugging.
            else
                LookupData.OnMouseWheelBack(); //See OnPageUp() for quadriplegic debugging.
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

                LookupData.OnColumnClick(columnIndex, resetSortOrder);

                for (int i = 0; i < LookupGridView.Columns.Count; i++)
                {
                    var gridColumn = LookupGridView.Columns[i];
                    if (gridColumn != _lastHeaderClicked.Column)
                    {
                        var columnHeader = gridColumn.Header as GridViewColumnHeader;
                        GridViewSort.RemoveSortGlyph(columnHeader);
                    }
                }

                var columnNumber = 1;
                foreach (var lookupColumnDefinition in LookupData.OrderByList)
                {
                    var orderColumnIndex = GetIndexOfVisibleColumnDefinition(lookupColumnDefinition);
                    var orderGridColumnHeader =
                        LookupGridView.Columns[orderColumnIndex].Header as GridViewColumnHeader;
                    GridViewSort.AddNonPrimarySortGlyph(orderGridColumnHeader, columnNumber);
                    columnNumber++;
                }

                if (resetSortOrder)
                    SetActiveColumn(columnIndex, LookupData.SortColumnDefinition.DataType);

                SearchForHost?.Control.Focus();
            }
        }

        private void SetActiveColumn(int sortColumnIndex, FieldDataTypes datatype)
        {
            if (!LookupColumns.Any())
                return;

            if (SearchForStackPanel != null)
            {
                if (DesignerProperties.GetIsInDesignMode(this))
                {
                    _designModeSearchForTextBox = new TextBox();
                    SearchForStackPanel.Children.Add(_designModeSearchForTextBox);
                }
                else
                {
                    var lookupColumnDefinition = LookupDefinition.VisibleColumns[sortColumnIndex];

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
                    LookupData?.OnChangePageSize();
                }
            }
        }

        private void LookupData_LookupDataChanged(object sender, LookupDataChangedArgs e)
        {
            _dataSource.Rows.Clear();
            foreach (DataRow dataRow in e.OutputTable.Rows)
            {
                var newDataRow = _dataSource.NewRow();
                foreach (var lookupColumn in LookupColumns)
                {
                    var cellValue = dataRow.GetRowValue(lookupColumn.DataColumnName);
                    if (cellValue.IsNullOrEmpty() &&
                        (lookupColumn.KeepNullEmpty || lookupColumn.LookupColumnDefinition.KeepNullEmpty))
                    {
                    }
                    else
                    {
                        cellValue = lookupColumn.LookupColumnDefinition.FormatValue(cellValue);
                    }
                    
                    newDataRow[lookupColumn.DataColumnName] = cellValue;
                }

                _dataSource.Rows.Add(newDataRow);
            }

            if (ListView != null)
                ListView.ItemsSource = _dataSource.DefaultView;

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

            if (ListView != null)
                ListView.SelectedIndex = e.SelectedRowIndex;

            var setupRecordCount = true;
            switch (SearchType)
            {
                case LookupSearchTypes.Equals:
                    if (e.CountingRecords)
                        setupRecordCount = false;
                    break;
                case LookupSearchTypes.Contains:
                    if (!e.SearchForChanging)
                        setupRecordCount = false;
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }
            if (setupRecordCount)
                SetupRecordCount();
        }

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
            if (LookupData == null || ListView == null || !LookupData.LookupDefinition.VisibleColumns.Any())
            {
                _refreshPendingData = new RefreshPendingData(initialSearchFor, parentWindowPrimaryKeyValue);
                return;
            }

            LookupData.ParentWindowPrimaryKeyValue = parentWindowPrimaryKeyValue;

            if (!resetSearchFor && initialSearchFor.IsNullOrEmpty())
            {
                if (SearchForHost != null)
                    initialSearchFor = SearchForHost.SearchText;
            }

            _currentPageSize = GetPageSize();

            if (String.IsNullOrEmpty(initialSearchFor))
                LookupData.GetInitData();
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

                        if (forceRefresh)
                            LookupData.OnSearchForChange(initialSearchFor);
                    }
                    else
                    {
                        SearchText = initialSearchFor;
                        if (searchForSelectAll)
                        {
                            SearchForHost.SelectAll();
                        }
                        LookupData.SelectPrimaryKey(initialSearchForPrimaryKeyValue);
                    }
                }
            }
        }

        private int GetPageSize(bool setOriginalPageSize = true)
        {
            if (ListView == null || LookupGridView == null)
                return 10;

            //var itemHeight = 0.0;
            if (_itemHeight <= 0)
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

        private double GetWidthFromPercent(Control control, double percentWidth)
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
                LookupData.SelectedRowIndex = index;
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
                LookupData.GotoNextRecord();
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
                LookupData.GotoPreviousRecord();
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

            //LookupData.OnMouseWheelForward(); //For debugging purposes only. I'm a quadriplegic and it's very difficult for me to use a mouse wheel.

            //Comment out below code block when debugging mouse wheel.

            var selIndex = ListView.SelectedIndex;
            if (selIndex >= ListView.Items.Count - 1 || !checkSelectedIndex)
                LookupData.GotoNextPage();
            else
                ListView.SelectedIndex = ListView.Items.Count - 1;
        }

        private void OnPageUp(bool checkSelectedIndex = true)
        {
            if (ListView == null)
                return;

            //LookupData.OnMouseWheelBack(); //For debugging purposes only. I'm a quadriplegic and it's very difficult for me to use a mouse wheel.

            //Comment out below code block when debugging mouse wheel.

            var selIndex = ListView.SelectedIndex;
            if (selIndex <= 0 || !checkSelectedIndex)
                LookupData.GotoPreviousPage();
            else
                ListView.SelectedIndex = 0;
        }

        private void OnEnd(bool checkSelectedIndex = true)
        {
            if (ListView == null)
                return;

            var selIndex = ListView.SelectedIndex;

            if (selIndex >= ListView.Items.Count - 1 || !checkSelectedIndex)
                LookupData.GotoBottom();
            else
                ListView.SelectedIndex = ListView.Items.Count - 1;
        }

        private void OnHome(bool checkSelectedIndex = true)
        {
            if (ListView == null)
                return;

            var selIndex = ListView.SelectedIndex;

            if (selIndex <= 0 || !checkSelectedIndex)
                LookupData.GotoTop();
            else
                ListView.SelectedIndex = 0;
        }

        private void OnSearchTypeChanged()
        {
            if (!SearchText.IsNullOrEmpty())
                LookupData.ResetRecordCount();

            if (SearchForHost != null)
            {
                SearchForHost.Control.Focus();
                RefreshData(false, SearchForHost.SearchText);
            }
        }
        private async void GetRecordCountButtonClick()
        {
            ShowRecordCountLabel();

            if (Spinner != null)
                Spinner.Visibility = Visibility.Visible;

            var processComplete = await LookupData.GetRecordCount();

            if (Spinner != null)
                Spinner.Visibility = Visibility.Collapsed;

            if (processComplete)
            {
                if (!GetRecordCountButton.IsVisible)
                    SetupRecordCount();
            }
        }

        private void ShowAdvancedFind()
        {
            //advancedFindWindow.Owner = Window.GetWindow(this);
            var lookupData =
                new LookupDataBase(new LookupDefinitionBase(SystemGlobals.AdvancedFindLookupContext.AdvancedFinds),
                    this);

            var addViewArgs = new LookupAddViewArgs(lookupData, true, LookupFormModes.View, 
                "", Window.GetWindow(this));

            addViewArgs.InputParameter = new AdvancedFindInput
            {
                InputParameter = AddViewParameter,
                LockTable = LookupDefinition.TableDefinition,
                LookupDefinition = LookupDefinition,
                LookupWidth = ActualWidth
            };

            var advancedFindWindow = new AdvancedFindWindow(addViewArgs);
            advancedFindWindow.Owner = Window.GetWindow(this);
            advancedFindWindow.ShowInTaskbar = false;
            
            advancedFindWindow.ShowDialog();
        }

        private void SetupRecordCount()
        {
            if (GetRecordCountButton == null || RecordCountControl == null || RecordCountStackPanel == null)
                return;

            var showRecordCount = false;
            if (LookupData.ScrollPosition == LookupScrollPositions.Disabled)
            {
                showRecordCount = true;
            }
            else if (LookupData.RecordCount > 0)
                showRecordCount = true;

            if (showRecordCount)
            {
                ShowRecordCountLabel();
                var recordsText = LookupData.RecordCount == 1 ? "" : "s";
                RecordCountControl.Text =
                    $@"{LookupData.RecordCount.ToString(GblMethods.GetNumFormat(0, false))} Record{recordsText} Found";
            }
            else
            {
                RecordCountStackPanel.Visibility = Visibility.Hidden;
                GetRecordCountButton.Visibility = Visibility.Visible;
            }
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
                var selectedIndex = ListView.SelectedIndex;
                if (selectedIndex >= 0)
                {
                    LookupData.ViewSelectedRow(selectedIndex, Window.GetWindow(this), AddViewParameter, _readOnlyMode);
                    return true;
                }
            }

            return false;
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
                        break;
                    case LookupCommands.Refresh:
                        this.AddViewParameter = command.AddViewParameter;
                        RefreshData(command.ResetSearchFor, String.Empty, command.ParentWindowPrimaryKeyValue);
                        break;
                    case LookupCommands.AddModify:
                        var addViewParameter = command.AddViewParameter;
                        if (addViewParameter == null)
                            addViewParameter = this.AddViewParameter;

                        var selectedIndex = ListView?.SelectedIndex ?? 0;
                        if (selectedIndex >= 0)
                            LookupData.ViewSelectedRow(selectedIndex, Window.GetWindow(this), addViewParameter);
                        else
                            LookupData.AddNewRow(Window.GetWindow(this), addViewParameter);
                        break;
                    case LookupCommands.Reset:
                        ClearLookupControl();
                        SetupControl();
                        this.AddViewParameter = command.AddViewParameter;
                        _currentPageSize = GetPageSize();
                        RefreshData(true);
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
            if (LookupData != null)
            {
                LookupData.ClearLookupData();
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
    }
}