using RingSoft.DbLookup.Lookup;
using RingSoft.DataEntryControls.Engine;
using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Data;
using System.Linq;
using System.Reflection;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Data;
using System.Windows.Input;
using System.Windows.Media;

namespace RingSoft.DbLookup.Controls.WPF
{
    /// <summary>
    /// Interaction logic for LookupControl.xaml
    /// </summary>
    public partial class LookupControl : ILookupControl
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
        public int PageSize => _currentPageSize;

        public LookupSearchTypes SearchType
        {
            get
            {
                if (EqualsRadioButton.IsChecked != null && (bool)EqualsRadioButton.IsChecked)
                    return LookupSearchTypes.Equals;

                return LookupSearchTypes.Contains;
            }
        }

        public string SearchText
        {
            get => SearchForTextBox.Text;
            set
            {
                _resettingSearchFor = true;
                SearchForTextBox.Text = value;
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

        /// <summary>
        /// Gets a list of LookupColumns which allow adding new columns.
        /// </summary>
        /// <value>
        /// The lookup columns.
        /// </value>
        public ObservableCollection<LookupColumn> LookupColumns { get; }

        public LookupDataBase LookupData { get; private set; }

        /// <summary>
        /// Occurs when a user wishes to add or view a selected lookup row.  Set Handled property to True to not send this message to the LookupContext.
        /// </summary>
        public event EventHandler<LookupAddViewArgs> LookupView;

        private bool _controlLoaded;
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

        public LookupControl()
        {
            LookupColumns = new ObservableCollection<LookupColumn>();

            //InitializeComponent();
            this.LoadViewFromUri("/RingSoft.DbLookup.Controls.WPF;component/LookupControl.xaml");

            LookupColumns.CollectionChanged += (sender, args) => OnLookupColumnsChanged();

            Loaded += (sender, args) => OnLoad();
            SearchForTextBox.GotFocus += (sender, args) => SearchForTextBox_GotFocus();
        }

        private void SearchForTextBox_GotFocus()
        {
            SearchForTextBox.SelectionStart = 0;
            SearchForTextBox.SelectionLength = SearchForTextBox.Text.Length;
        }

        private void OnLoad()
        {
            if (_designSortIndex >= 0 && DesignerProperties.GetIsInDesignMode(this))
            {
                InitializeHeader(_designSortIndex);
                DesignerFillGrid();
            }

            if (IsVisible)
            {
                SizeChanged += (sender, args) => LookupControlSizeChanged();

                if (LookupDefinition != null && !_controlLoaded)
                    SetupControl();
                _controlLoaded = true;
            }
        }

        private void SetupControl()
        {
            if (LookupDefinition.InitialSortColumnDefinition == null)
                throw new ArgumentException(
                    "Lookup definition does not have any visible columns defined or its initial sort column is null.");

            if (LookupData != null)
            {
                ClearLookupControl();
                LookupColumns.Clear();
            }

            LookupData = new LookupDataBase(LookupDefinition, this);
            LookupData.LookupDataChanged += LookupData_LookupDataChanged;
            LookupData.DataSourceChanged += LookupData_DataSourceChanged;
            LookupData.LookupView += (sender, args) => LookupView?.Invoke(this, args);

            if (!_setupRan)
            {
                SearchForTextBox.PreviewKeyDown += (sender, args) => { OnListViewKeyDown(args); };
                SearchForTextBox.TextChanged += (sender, args) =>
                {
                    if (!_resettingSearchFor)
                        LookupData.OnSearchForChange(SearchForTextBox.Text);
                };
                ListView.PreviewKeyDown += (sender, args) => { OnListViewKeyDown(args); };
                ListView.SelectionChanged += ListView_SelectionChanged;

                ContainsRadioButton.Click += (sender, args) => { OnSearchTypeChanged(); };
                EqualsRadioButton.Click += (sender, args) => { OnSearchTypeChanged(); };

                GetRecordCountButton.Click += (sender, args) => { GetRecordCountButtonClick(); };
                ListView.MouseDoubleClick += (sender, args) => { OnEnter(); };
                ScrollBar.Scroll += ScrollBar_Scroll;
                ScrollBar.PreviewMouseDown += (sender, args) => { _preScrollThumbPosition = ScrollBar.Value; };
                ListView.PreviewMouseWheel += ListView_PreviewMouseWheel;
            }

            LookupGridView.Columns.Clear();
            _dataSource.Columns.Clear();

            if (LookupColumns.Any())
                MergeLookupDefinition();
            else
                ImportLookupDefinition();

            var sortColumnIndex =
                GetIndexOfVisibleColumnDefinition(LookupDefinition.InitialSortColumnDefinition);

            InitializeHeader(sortColumnIndex);
            SetActiveColumn(sortColumnIndex, LookupDefinition.InitialSortColumnDefinition.DataType);

            if (_refreshPendingData != null)
            {
                RefreshData(true, _refreshPendingData.InitialSearchFor, _refreshPendingData.ParentWindowPrimaryKeyValue,
                    true);
                _refreshPendingData = null;
            }

            _setupRan = _controlLoaded = true;
        }

        private void MergeLookupDefinition()
        {
            if (LookupColumns.FirstOrDefault(f => f.PropertyName == LookupDefinition.InitialSortColumnDefinition.PropertyName) == null)
                throw new Exception($"No Lookup Column was added to Columns collection for initial sort column Property '{LookupDefinition.InitialSortColumnDefinition.PropertyName}'.");

            foreach (var lookupColumn in LookupColumns)
            {
                var lookupColumnDefinition =
                    LookupDefinition.VisibleColumns.FirstOrDefault(f => f.PropertyName == lookupColumn.PropertyName);
                if (lookupColumnDefinition == null)
                    throw new Exception($"No visible Lookup Column Definition was found for Property '{lookupColumn.PropertyName}'.");

                lookupColumn.DataColumnName = lookupColumnDefinition.SelectSqlAlias;
                lookupColumn.LookupColumnDefinition = lookupColumnDefinition;
                if (!lookupColumn.TextAlignmentChanged)
                    lookupColumn.TextAlignment = lookupColumnDefinition.HorizontalAlignment;

                AddColumnToGrid(lookupColumn);
            }
        }

        private void ImportLookupDefinition()
        {
            foreach (var column in LookupDefinition.VisibleColumns)
            {
                var columnWidth = GetWidthFromPercent(ListView, column.PercentWidth);
                var lookupColumn = new LookupColumn
                {
                    DataColumnName = column.SelectSqlAlias,
                    Header = column.Caption,
                    LookupColumnDefinition = column,
                    PropertyName = column.PropertyName,
                    TextAlignment = column.HorizontalAlignment,
                    Width = columnWidth
                };
                LookupColumns.Add(lookupColumn);
                AddColumnToGrid(lookupColumn);
            }
        }

        private int GetIndexOfVisibleColumnDefinition(LookupColumnDefinitionBase lookupColumnDefinition)
        {
            var lookupColumn = LookupColumns.FirstOrDefault(f => f.LookupColumnDefinition == lookupColumnDefinition);
            return LookupColumns.IndexOf(lookupColumn);
        }

        private GridViewColumn AddGridViewColumn(string caption, double width, string dataColumnName,
            TextAlignment textAlignment)
        {
            var gridColumn = new GridViewColumn
            {
                Header = new GridViewColumnHeader
                {
                    Content = caption // + "\r\nLine2\r\nLine3"
                },
                Width = width,
                CellTemplate = GetCellDataTemplate(textAlignment, dataColumnName)
            };
            LookupGridView.Columns.Add(gridColumn);
            _dataSource.Columns.Add(dataColumnName);

            return gridColumn;
        }

        private void InitializeHeader(int sortColumnIndex)
        {
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
                style.Setters.Add(new Setter(GridViewColumnHeader.HeightProperty, GetHeaderHeight(header) + glyphSize.Height + 5));
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
            if (DesignerProperties.GetIsInDesignMode(this))
            {
                var lineFeedCount = 0;
                var startIndex = 0;
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
            return height;
        }

        private void CalculateDesignHeaderLineHeight(GridViewHeaderRowPresenter header)
        {
            if (_designModeHeaderLineHeight > 0 || !DesignerProperties.GetIsInDesignMode(this))
                return;

            if (!LookupGridView.Columns.Any())
                return;

            foreach (var column in LookupGridView.Columns)
            {
                var columnHeader = (GridViewColumnHeader) column.Header;
                columnHeader.Content = "WWWWW\r\nWWW";
            }
            header.UpdateLayout();
            _designModeHeaderLineHeight = header.ActualHeight / 2;

            var index = 0;
            foreach (var column in LookupGridView.Columns)
            {
                var columnHeader = (GridViewColumnHeader) column.Header;
                var lookupColumn = LookupColumns[index];
                columnHeader.Content = lookupColumn.Header;
                index++;
            }
        }

        private void ResetColumnHeaderSort(int sortColumnIndex)
        {
            if (!LookupGridView.Columns.Any())
                return;

            var sortColumn = LookupGridView.Columns[sortColumnIndex];
            _lastHeaderClicked = sortColumn.Header as GridViewColumnHeader;
            _lastDirection = ListSortDirection.Ascending;
            GridViewSort.ApplySort(_lastDirection, ListView, _lastHeaderClicked);
        }

        private void OnLookupColumnsChanged()
        {
            if (!DesignerProperties.GetIsInDesignMode(this))
                return;

            LookupGridView.Columns.Clear();
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

        private void AddColumnToGrid(LookupColumn column)
        {
            TextAlignment gridTextAlignment;
            switch (column.TextAlignment)
            {
                case LookupColumnAlignmentTypes.Left:
                    gridTextAlignment = TextAlignment.Left;
                    break;
                case LookupColumnAlignmentTypes.Center:
                    gridTextAlignment = TextAlignment.Center;
                    break;
                case LookupColumnAlignmentTypes.Right:
                    gridTextAlignment = TextAlignment.Right;
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }
            var gridColumn = AddGridViewColumn(column.Header, column.Width, column.DataColumnName,
                gridTextAlignment);

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

            ListView.ItemsSource = _dataSource.DefaultView;
        }

        private void Column_PropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            if (!DesignerProperties.GetIsInDesignMode(this) && !LookupGridView.Columns.Any())
                return;

            var lookupColumn = (LookupColumn) sender;
            var columnIndex = LookupColumns.IndexOf(lookupColumn);
            var gridColumn = LookupGridView.Columns[columnIndex];

            if (e.PropertyName == nameof(LookupColumn.Header))
            {
                var columnHeader = (GridViewColumnHeader)gridColumn.Header;
                columnHeader.Content = lookupColumn.Header;

                InitializeHeader(_designSortIndex);

                DesignerFillGrid();

                if (columnIndex == _designSortIndex && !lookupColumn.Header.IsNullOrEmpty())
                    SetActiveColumn(_designSortIndex, FieldDataTypes.String);
            }
            else if (e.PropertyName == nameof(LookupColumn.DesignText))
            {
                DesignerFillGrid();
            }
            else if (e.PropertyName == nameof(LookupColumn.Width))
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

        private DataTemplate GetCellDataTemplate(TextAlignment textAlignment,
            string dataColumnName)
        {
            var template = new DataTemplate();
            var factory = new FrameworkElementFactory(typeof(TextBlock));
            factory.SetValue(TextBlock.TextAlignmentProperty, textAlignment);

            //factory.SetValue(TextBlock.FontFamilyProperty, SearchForLabel.FontFamily);
            //factory.SetValue(TextBlock.FontSizeProperty, SearchForLabel.FontSize);
            //factory.SetValue(TextBlock.FontWeightProperty, SearchForLabel.FontWeight);
            factory.SetBinding(TextBlock.TextProperty, new Binding(dataColumnName));
            template.VisualTree = factory;

            return template;
        }

        private void GridViewColumnHeaderClickedHandler(object sender, RoutedEventArgs e)
        {
            var headerClicked = e.OriginalSource as GridViewColumnHeader;
            ListSortDirection direction = _lastDirection;

            if (headerClicked != null)
            {
                if (headerClicked.Role != GridViewColumnHeaderRole.Padding)
                {
                    var controlKeyPressed = Keyboard.IsKeyDown(Key.LeftCtrl) || Keyboard.IsKeyDown(Key.RightCtrl);
                    var resetSortOrder = !controlKeyPressed || _lastHeaderClicked == headerClicked;
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

                    SearchForTextBox.Text = string.Empty;
                    var columnIndex = LookupGridView.Columns.IndexOf(headerClicked.Column);

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

                    SetActiveColumn(columnIndex, LookupData.SortColumnDefinition.DataType);
                    SearchForTextBox.Focus();
                }
            }
        }

        private void SetActiveColumn(int sortColumnIndex, FieldDataTypes datatype)
        {
            if (!LookupColumns.Any())
                return;

            var column = LookupColumns[sortColumnIndex];

            if (!column.Header.IsNullOrEmpty())
            {
                var headerText = column.Header.Replace('\n', ' ');
                SearchForLabel.Content = $@"Search For {headerText}";
            }

            if (datatype == FieldDataTypes.String)
            {
                ContainsRadioButton.IsEnabled = true;
            }
            else
            {
                ContainsRadioButton.IsEnabled = false;
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
                    cellValue = lookupColumn.LookupColumnDefinition.FormatValue(cellValue);
                    newDataRow[lookupColumn.DataColumnName] = cellValue;
                }

                _dataSource.Rows.Add(newDataRow);
            }

            ListView.ItemsSource = _dataSource.DefaultView;

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
            double middleValue = Math.Floor((ScrollBar.Maximum - ScrollBar.Minimum) / 2);
            ScrollBar.Value = (int)middleValue - 5;
        }

        /// <summary>
        /// Refreshes the data based on changes in the LookupDefinition.
        /// </summary>
        /// <param name="resetSearchFor">If set to true then reset the Search For TextBox.</param>
        /// <param name="initialSearchFor">The new Search For value.</param>
        /// <param name="parentWindowPrimaryKeyValue">The parent window's PrimaryKeyValue.</param>
        /// <param name="searchForSelectAll">Select all text in the Search For TextBox.</param>
        public void RefreshData(bool resetSearchFor, string initialSearchFor = "",
            PrimaryKeyValue parentWindowPrimaryKeyValue = null, bool searchForSelectAll = false)
        {
            if (LookupData == null)
            {
                _refreshPendingData = new RefreshPendingData(initialSearchFor, parentWindowPrimaryKeyValue);
                return;
            }

            LookupData.ParentWindowPrimaryKeyValue = parentWindowPrimaryKeyValue;

            if (!resetSearchFor && initialSearchFor.IsNullOrEmpty())
                initialSearchFor = SearchForTextBox.Text;

            _currentPageSize = GetPageSize();

            if (string.IsNullOrEmpty(initialSearchFor))
                LookupData.GetInitData();
            else
            {
                var forceRefresh = SearchForTextBox.Text == initialSearchFor;
                SearchForTextBox.Text = initialSearchFor; //This automatically triggers LookupData.OnSearchForChange.  Only if the text value has changed.

                if (searchForSelectAll)
                    SearchForTextBox_GotFocus();

                if (forceRefresh)
                    LookupData.OnSearchForChange(initialSearchFor);
            }
        }

        private int GetPageSize(bool setOriginalPageSize = true)
        {
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
                    OnEnter();
                    e.Handled = true;
                    break;
            }
        }

        protected void OnDownArrow()
        {
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
            var selIndex = ListView.SelectedIndex;

            if (selIndex >= ListView.Items.Count - 1 || !checkSelectedIndex)
                LookupData.GotoBottom();
            else
                ListView.SelectedIndex = ListView.Items.Count - 1;
        }

        private void OnHome(bool checkSelectedIndex = true)
        {
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

            SearchForTextBox.Focus();
            RefreshData(false, SearchForTextBox.Text);
        }
        private async void GetRecordCountButtonClick()
        {
            ShowRecordCountLabel();
            Spinner.Visibility = Visibility.Visible;
            var processComplete = await LookupData.GetRecordCount();
            Spinner.Visibility = Visibility.Collapsed;
            if (processComplete)
            {
                if (!GetRecordCountButton.IsVisible)
                    SetupRecordCount();
            }
        }

        private void SetupRecordCount()
        {
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
                RecordCountLabel.Content =
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
            GetRecordCountButton.Visibility = Visibility.Hidden;
            RecordCountStackPanel.Visibility = Visibility.Visible;
            RecordCountLabel.Content = @"Counting Records";
        }

        private void OnEnter()
        {
            var selectedIndex = ListView.SelectedIndex;
            if (selectedIndex >= 0)
            {
                object inputParameter = null;
                if (Command != null)
                    inputParameter = Command.AddViewParameter;

                LookupData.ViewSelectedRow(selectedIndex, Window.GetWindow(this), inputParameter);
            }
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
                        RefreshData(command.ResetSearchFor, string.Empty, command.ParentWindowPrimaryKeyValue);
                        break;
                    case LookupCommands.AddModify:
                        var selectedIndex = ListView.SelectedIndex;
                        if (selectedIndex >= 0)
                            LookupData.ViewSelectedRow(selectedIndex, Window.GetWindow(this), command.AddViewParameter);
                        else
                            LookupData.AddNewRow(Window.GetWindow(this), command.AddViewParameter);
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

            EqualsRadioButton.IsChecked = true;
            SearchForTextBox.Text = string.Empty;
        }
    }
}
