using System;
using System.ComponentModel;
using System.Data;
using System.Reflection;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Input;
using System.Windows.Media;
using RingSoft.DbLookup.Controls.WPF;
using RingSoft.DbLookup.Lookup;

namespace RingSoft.DbLookup.Controls.WPF
{
    /// <summary>
    /// Interaction logic for LookupControl.xaml
    /// </summary>
    public partial class LookupControl : ILookupUserInterface
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

        public LookupDataBase LookupData { get; private set; }

        private bool _controlLoaded;
        private int _originalPageSize;
        private int _currentPageSize;
        private DataTable _dataSource = new DataTable("DataSourceTable");
        GridViewColumnHeader _lastHeaderClicked;
        ListSortDirection _lastDirection = ListSortDirection.Ascending;

        private RefreshPendingData _refreshPendingData;

        private bool _resettingSearchFor;
        private double _itemHeight;

        public LookupControl()
        {
            InitializeComponent();
            Loaded += (sender, args) =>
            {
                if (IsVisible)
                {
                    if (LookupDefinition != null && !_controlLoaded)
                        SetupControl();
                    _controlLoaded = true;
                }
            };
            SearchForTextBox.GotFocus += (sender, args) =>
            {
                SearchForTextBox.SelectionStart = 0;
                SearchForTextBox.SelectionLength = SearchForTextBox.Text.Length;
            };
        }

        private void SetupControl()
        {
            LookupData = new LookupDataBase(LookupDefinition, this);
            LookupData.LookupDataChanged += LookupData_LookupDataChanged;

            if (!_controlLoaded)
            {
                SizeChanged += (sender, args) => LookupControlSizeChanged();
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
            }

            LookupGridView.Columns.Clear();
            var columnCount = LookupDefinition.VisibleColumns.Count;
            foreach (var column in LookupDefinition.VisibleColumns)
            {
                var columnWdth = GetWidthFromPercent(ListView, column.PercentWidth);
                var gridColumn = new GridViewColumn
                {
                    Header = new GridViewColumnHeader
                    {
                        Content = column.Caption// + "\r\nLine2\r\nLine3"
                    },
                    Width = columnWdth,
                    CellTemplate = GetCellDataTemplate(column)
                };

                ((INotifyPropertyChanged)gridColumn).PropertyChanged += (sender, args) =>
                {
                    if (args.PropertyName == "ActualWidth")
                    {
                        LookupControlSizeChanged();
                    }
                };
                LookupGridView.Columns.Add(gridColumn);
                _dataSource.Columns.Add(column.SelectSqlAlias);
            }

            GridViewHeaderRowPresenter header = (GridViewHeaderRowPresenter)LookupGridView.GetType()
                .GetProperty("HeaderRowPresenter", BindingFlags.NonPublic | BindingFlags.Instance)
                ?.GetValue(LookupGridView);

            if (header != null)
            {
                header.UpdateLayout();
                var column =
                    LookupGridView.Columns[
                        LookupDefinition.GetIndexOfVisibleColumn(LookupDefinition.InitialSortColumnDefinition)];
                var columnHeader = column.Header as GridViewColumnHeader;
                var glyphSize = GridViewSort.GetGlyphSize(columnHeader, ListSortDirection.Ascending, ListView);
                Style style = new Style();
                style.TargetType = typeof(GridViewColumnHeader);
                style.Setters.Add(new Setter(GridViewColumnHeader.HeightProperty, header.ActualHeight + glyphSize.Height + 5));
                style.Setters.Add(new Setter(GridViewColumnHeader.VerticalContentAlignmentProperty, VerticalAlignment.Bottom));

                LookupGridView.ColumnHeaderContainerStyle = style;
            }

            var sortColumnIndex = LookupData.LookupDefinition.GetIndexOfVisibleColumn(LookupData.SortColumnDefinition);
            var sortColumn = LookupGridView.Columns[sortColumnIndex];
            _lastHeaderClicked = sortColumn.Header as GridViewColumnHeader;
            _lastDirection = ListSortDirection.Ascending;
            GridViewSort.ApplySort(_lastDirection, ListView, _lastHeaderClicked);
            SetActiveColumn();

            if (_refreshPendingData != null)
            {
                RefreshData(true, _refreshPendingData.InitialSearchFor, _refreshPendingData.ParentWindowPrimaryKeyValue);
                _refreshPendingData = null;
            }
        }

        private static DataTemplate GetCellDataTemplate(LookupColumnBase column)
        {
            var template = new DataTemplate();
            var factory = new FrameworkElementFactory(typeof(TextBlock));
            switch (column.HorizontalAlignment)
            {
                case LookupColumnAlignmentTypes.Left:
                    factory.SetValue(TextBlock.TextAlignmentProperty, TextAlignment.Left);
                    break;
                case LookupColumnAlignmentTypes.Center:
                    factory.SetValue(TextBlock.TextAlignmentProperty, TextAlignment.Center);
                    break;
                case LookupColumnAlignmentTypes.Right:
                    factory.SetValue(TextBlock.TextAlignmentProperty, TextAlignment.Right);
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }
            factory.SetBinding(TextBlock.TextProperty, new Binding(column.SelectSqlAlias));
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
                        var orderColumnIndex = LookupDefinition.GetIndexOfVisibleColumn(lookupColumnDefinition);
                        var orderGridColumnHeader =
                            LookupGridView.Columns[orderColumnIndex].Header as GridViewColumnHeader;
                        GridViewSort.AddNonPrimarySortGlyph(orderGridColumnHeader, columnNumber);
                        columnNumber++;
                    }

                    SetActiveColumn();
                    SearchForTextBox.Focus();
                }
            }
        }

        private void SetActiveColumn()
        {
            SearchForLabel.Content = $@"Search For {LookupData.SortColumnDefinition.Caption}";
            if (LookupData.SortColumnDefinition.DataType == FieldDataTypes.String)
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
                LookupData.OnChangePageSize();
            }
        }

        private void LookupData_LookupDataChanged(object sender, EventArgs e)
        {
            _dataSource.Rows.Clear();
            foreach (DataRow dataRow in LookupData.LookupResultsDataTable.Rows)
            {
                var newDataRow = _dataSource.NewRow();
                foreach (var lookupDefinitionColumn in LookupData.LookupDefinition.VisibleColumns)
                {
                    var cellValue = dataRow.GetRowValue(lookupDefinitionColumn.SelectSqlAlias);
                    cellValue = lookupDefinitionColumn.FormatValue(cellValue);
                    newDataRow[lookupDefinitionColumn.SelectSqlAlias] = cellValue;
                }

                _dataSource.Rows.Add(newDataRow);
            }

            ListView.ItemsSource = _dataSource.DefaultView;

            ScrollBar.IsEnabled = true;
            switch (LookupData.ScrollPosition)
            {
                case LookupScrollPositions.Disabled:
                    ScrollBar.IsEnabled = false;
                    break;
                case LookupScrollPositions.Top:
                    ScrollBar.Value = ScrollBar.Minimum;
                    break;
                case LookupScrollPositions.Middle:
                    double middleValue = Math.Floor((ScrollBar.Maximum - ScrollBar.Minimum) / 2);
                    ScrollBar.Value = (int)middleValue - 5;
                    break;
                case LookupScrollPositions.Bottom:
                    ScrollBar.Value = ScrollBar.Maximum;
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }
            ListView.SelectedIndex = LookupData.SelectedRowIndex;
            SetupRecordCount();
        }

        public void RefreshData(bool resetSearchFor, string initialSearchFor = "", PrimaryKeyValue parentWindowPrimaryKeyValue = null)
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
                LookupData.GetInitData(true);
            else
            {
                var forceRefresh = SearchForTextBox.Text == initialSearchFor;
                SearchForTextBox.Text = initialSearchFor; //This automatically triggers LookupData.OnSearchForChange.  Only if the text value has changed.
                if (forceRefresh)
                    LookupData.OnSearchForChange(initialSearchFor);
            }
        }

        private int GetPageSize()
        {
            //var itemHeight = 0.0;
            if (_itemHeight <= 0)
            {
                var addBlankRow = ListView.Items.Count <= 0;
                if (addBlankRow)
                    ListView.Items.Add("text");

                var item = ListView.Items.GetItemAt(0);
                ListView.UpdateLayout();
                if (ListView.ItemContainerGenerator.ContainerFromItem(item) is ListViewItem listViewItem)
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
            if (scrollViewer.ComputedHorizontalScrollBarVisibility == Visibility.Visible)
                pageSize -= 1;

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

        public double GetWidthFromPercent(Control control, double percentWidth)
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
                var index = ListView.Items.IndexOf(e.AddedItems[0]);
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
            var selIndex = ListView.SelectedIndex;
            if (selIndex >= ListView.Items.Count - 1 || !checkSelectedIndex)
                LookupData.GotoNextPage();

            ListView.SelectedIndex = ListView.Items.Count - 1;
        }

        private void OnPageUp(bool checkSelectedIndex = true)
        {
            var selIndex = ListView.SelectedIndex;
            if (selIndex <= 0 || !checkSelectedIndex)
                LookupData.GotoPreviousPage();

            ListView.SelectedIndex = 0;
        }

        private void OnEnd(bool checkSelectedIndex = true)
        {
            var selIndex = ListView.SelectedIndex;
            if (selIndex >= ListView.Items.Count - 1 || !checkSelectedIndex)
                LookupData.GotoBottom();

            ListView.SelectedIndex = ListView.Items.Count - 1;
        }

        private void OnHome(bool checkSelectedIndex = true)
        {
            var selIndex = ListView.SelectedIndex;
            if (selIndex <= 0 || !checkSelectedIndex)
                LookupData.GotoTop();

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

        public void SetupRecordCount()
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
                LookupData.ViewSelectedRow(selectedIndex);
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
                            LookupData.ViewSelectedRow(selectedIndex);
                        else
                            LookupData.AddNewRow();
                        break;
                    default:
                        throw new ArgumentOutOfRangeException();
                }
            }
        }
        /// <summary>
        /// Clears the lookup control.
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
