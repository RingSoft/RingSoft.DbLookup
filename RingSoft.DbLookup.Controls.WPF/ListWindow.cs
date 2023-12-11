// ***********************************************************************
// Assembly         : RingSoft.DbLookup.Controls.WPF
// Author           : petem
// Created          : 03-04-2023
//
// Last Modified By : petem
// Last Modified On : 07-01-2023
// ***********************************************************************
// <copyright file="ListWindow.cs" company="Peter Ringering">
//     Copyright (c) . All rights reserved.
// </copyright>
// <summary></summary>
// ***********************************************************************
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Data;
using System.Data.Common;
using System.Drawing.Printing;
using System.Linq;
using System.Media;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using RingSoft.DataEntryControls.Engine;
using RingSoft.DataEntryControls.WPF;
using RingSoft.DbLookup.Lookup;
using RingSoft.DbLookup.QueryBuilder;

namespace RingSoft.DbLookup.Controls.WPF
{
    /// <summary>
    /// Class ListWindow.
    /// Implements the <see cref="BaseWindow" />
    /// Implements the <see cref="RingSoft.DbLookup.IListWindowView" />
    /// </summary>
    /// <seealso cref="BaseWindow" />
    /// <seealso cref="RingSoft.DbLookup.IListWindowView" />
    /// <font color="red">Badly formed XML comment.</font>
    public class ListWindow : BaseWindow, IListWindowView
    {
        /// <summary>
        /// Gets the border.
        /// </summary>
        /// <value>The border.</value>
        public Border Border { get; private set; }

        /// <summary>
        /// Gets the view model.
        /// </summary>
        /// <value>The view model.</value>
        public ListWindowViewModel ViewModel { get; private set; }

        /// <summary>
        /// Gets or sets the search for stack panel.
        /// </summary>
        /// <value>The search for stack panel.</value>
        public StackPanel SearchForStackPanel { get; set; }

        /// <summary>
        /// Gets or sets the search for label.
        /// </summary>
        /// <value>The search for label.</value>
        public Label SearchForLabel { get; set; }

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
        /// The lookup search for host
        /// </summary>
        private LookupSearchForHost _lookupSearchForHost;
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
        /// Gets the lookup columns.
        /// </summary>
        /// <value>The lookup columns.</value>
        public ObservableCollection<LookupColumnBase> LookupColumns { get; }

        /// <summary>
        /// The data source
        /// </summary>
        private DataTable _dataSource = new DataTable("DataSourceTable");
        /// <summary>
        /// The last direction
        /// </summary>
        private ListSortDirection _lastDirection = ListSortDirection.Ascending;
        /// <summary>
        /// The last header clicked
        /// </summary>
        private GridViewColumnHeader _lastHeaderClicked;
        /// <summary>
        /// The sort column
        /// </summary>
        private ListControlColumn _sortColumn;
        /// <summary>
        /// The item height
        /// </summary>
        private double _itemHeight;
        /// <summary>
        /// The old size
        /// </summary>
        private Size _oldSize;
        /// <summary>
        /// The pre scroll thumb position
        /// </summary>
        private double _preScrollThumbPosition;

        /// <summary>
        /// Initializes static members of the <see cref="ListWindow"/> class.
        /// </summary>
        static ListWindow()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(ListWindow), new FrameworkPropertyMetadata(typeof(ListWindow)));
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="ListWindow"/> class.
        /// </summary>
        /// <param name="setup">The setup.</param>
        /// <param name="dataSource">The data source.</param>
        /// <param name="selectedRow">The selected row.</param>
        public ListWindow(ListControlSetup setup, ListControlDataSource dataSource, ListControlDataSourceRow selectedRow)
        {
            var loaded = false;
            var initPageSize = 0;
            var sizeChangedRan = false;
            Loaded += (sender, args) =>
            {
                ViewModel.Initialize(setup, dataSource, this);
                if (initPageSize > 0 && selectedRow != null)
                {
                    ViewModel.SelectRow(selectedRow, initPageSize);
                    selectedRow = null;
                    SearchForHost.SearchText = ViewModel.SearchText;
                    SearchForHost.SelectAll();
                }
                _oldSize = new Size(Width, Height);
                loaded = true;
            };
            SizeChanged += (sender, args) =>
            {
                if (ListView != null && loaded)
                {
                    var widthDif = Width - _oldSize.Width;
                    var heightDif = Height - _oldSize.Height;
                    ListView.Width = ListView.ActualWidth + widthDif;
                    ListView.Height = ListView.ActualHeight + heightDif;
                }

                _oldSize = args.NewSize;
                if (selectedRow != null)
                {
                    initPageSize = GetPageSize();
                }

            };
        }

        /// <summary>
        /// When overridden in a derived class, is invoked whenever application code or internal processes call <see cref="M:System.Windows.FrameworkElement.ApplyTemplate" />.
        /// </summary>
        public override void OnApplyTemplate()
        {
            Border = GetTemplateChild(nameof(Border)) as Border;
            ViewModel = Border.TryFindResource("ViewModel") as ListWindowViewModel;

            SearchForLabel = GetTemplateChild(nameof(SearchForLabel)) as Label;
            SearchForStackPanel = GetTemplateChild(nameof(SearchForStackPanel)) as StackPanel;
            ListView = GetTemplateChild(nameof(ListView)) as ListView;
            LookupGridView = GetTemplateChild(nameof(LookupGridView)) as GridView;
            ScrollBar = GetTemplateChild(nameof(ScrollBar)) as ScrollBar;

            ScrollBar.Scroll += ScrollBar_Scroll;
            ScrollBar.PreviewMouseDown += (sender, args) => { _preScrollThumbPosition = ScrollBar.Value; };
            ListView.SizeChanged += (sender, args) => LookupControlSizeChanged();
            ListView.PreviewKeyDown += (sender, args) => { OnListViewKeyDown(args); };
            ListView.SelectionChanged += ListView_SelectionChanged;
            ListView.MouseDoubleClick += (sender, args) => { OnEnter(); };
            ListView.PreviewMouseWheel += ListView_PreviewMouseWheel;


            base.OnApplyTemplate();
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
                ViewModel.OnMouseWheelForward(); //See OnPageDown() for quadriplegic debugging.
            else
                ViewModel.OnMouseWheelBack(); //See OnPageUp() for quadriplegic debugging.
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
                ViewModel.SetSelectedIndex(index);
            }
        }


        /// <summary>
        /// Lookups the control size changed.
        /// </summary>
        private void LookupControlSizeChanged()
        {
            var pageSize = GetPageSize();
            ViewModel.PageSize = pageSize;
        }
        /// <summary>
        /// Closes the window.
        /// </summary>
        public void CloseWindow()
        {
            Close();
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
        /// Handles the TextChanged event of the SearchForControl control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="EventArgs"/> instance containing the event data.</param>
        private void SearchForControl_TextChanged(object sender, EventArgs e)
        {
            ViewModel.SearchText = SearchForHost.SearchText;
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
                    OnColumnClick((int)sortColumnIndex, false);
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
                {
                    ViewModel.GetNextPage(0);
                    ListView.SelectedIndex = ViewModel.SelectedIndex;

                }
            }
            else
            {
                ListView.SelectedIndex = selIndex + 1;
                ViewModel.SetSelectedIndex(ListView.SelectedIndex);
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
                {
                    ViewModel.GetPreviousPage(ViewModel.CurrentPage.Count - 1);
                    ListView.SelectedIndex = ViewModel.SelectedIndex;
                }
            }
            else
            {
                ListView.SelectedIndex = selIndex - 1;
                ViewModel.SetSelectedIndex(ListView.SelectedIndex);

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

            //ViewModel.OnMouseWheelForward(); //For debugging purposes only. I'm a quadriplegic and it's very difficult for me to use a mouse wheel.

            //Comment out below code block when debugging mouse wheel.

            var selIndex = ListView.SelectedIndex;
            if (selIndex >= ListView.Items.Count - 1 || !checkSelectedIndex)
            {
                ViewModel.GetNextPage(ViewModel.PageSize - 1);
                ListView.SelectedIndex = ViewModel.SelectedIndex;
            }
            else
            {
                ListView.SelectedIndex = ListView.Items.Count - 1;
                ViewModel.SetSelectedIndex(ListView.SelectedIndex);
            }
        }

        /// <summary>
        /// Called when [page up].
        /// </summary>
        /// <param name="checkSelectedIndex">if set to <c>true</c> [check selected index].</param>
        private void OnPageUp(bool checkSelectedIndex = true)
        {
            if (ListView == null)
                return;

            //ViewModel.OnMouseWheelBack(); //For debugging purposes only. I'm a quadriplegic and it's very difficult for me to use a mouse wheel.

            //Comment out below code block when debugging mouse wheel.

            var selIndex = ListView.SelectedIndex;
            if (selIndex <= 0 || !checkSelectedIndex)
            {
                ViewModel.GetPreviousPage(0);
                ListView.SelectedIndex = ViewModel.SelectedIndex;
            }
            else
            {
                ListView.SelectedIndex = 0;
                ViewModel.SetSelectedIndex(ListView.SelectedIndex);
            }
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
            {
                ViewModel.GoEnd();
            }
            else
            {
                ListView.SelectedIndex = ListView.Items.Count - 1;
                ViewModel.SetSelectedIndex(ListView.SelectedIndex);
            }
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
            {
                ViewModel.GoHome();
            }
            else
                ListView.SelectedIndex = 0;
        }

        /// <summary>
        /// Called when [enter].
        /// </summary>
        /// <returns><c>true</c> if XXXX, <c>false</c> otherwise.</returns>
        private bool OnEnter()
        {
            ViewModel.SelectCommand.Execute(null);
            return true;
        }

        /// <summary>
        /// Initializes the ListView.
        /// </summary>
        public void InitializeListView()
        {
            LookupGridView?.Columns.Clear();
            foreach (var column in ViewModel.ControlSetup.ColumnList)
            {
                double columnWidth = 100;
                if (ListView != null)
                    columnWidth = LookupControl.GetWidthFromPercent(ListView, column.PercentWidth);

                AddColumnToGrid(column, columnWidth);

            }

            InitializeHeader(0);
            SetActiveColumn(0, FieldDataTypes.String);
        }

        /// <summary>
        /// Fills the ListView.
        /// </summary>
        /// <exception cref="System.ArgumentOutOfRangeException"></exception>
        public void FillListView()
        {
            ListView.ItemsSource = ViewModel.OutputTable.DefaultView;
            ListView.SelectedIndex = ViewModel.SelectedIndex;
            switch (ViewModel.ScrollPosition)
            {
                case ScrollPositions.Top:
                    ScrollBar.Value = 0;
                    break;
                case ScrollPositions.Middle:
                    ScrollBar.Value = 50;
                    break;
                case ScrollPositions.Bottom:
                    ScrollBar.Value = 100;
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }
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
        /// Adds the column to grid.
        /// </summary>
        /// <param name="column">The column.</param>
        /// <param name="width">The width.</param>
        private void AddColumnToGrid(ListControlColumn column, double width)
        {
            var gridColumn = AddGridViewColumn(column.Caption, width, $"COLUMN{column.ColumnId}");
        }


        /// <summary>
        /// Adds the grid view column.
        /// </summary>
        /// <param name="caption">The caption.</param>
        /// <param name="width">The width.</param>
        /// <param name="dataColumnName">Name of the data column.</param>
        /// <returns>GridViewColumn.</returns>
        private GridViewColumn AddGridViewColumn(string caption, double width, string dataColumnName)
        {
            var columnHeader = new GridViewColumnHeader { Content = caption }; // + "\r\nLine2\r\nLine3"};
            columnHeader.Click += GridViewColumnHeaderClickedHandler;

            if (width < 0)
            {
                width = 20;
            }
            var template = new DataTemplate();
            var factory = new FrameworkElementFactory(typeof(TextBlock));
            factory.SetBinding(TextBlock.TextProperty, new Binding(dataColumnName));
            template.VisualTree = factory;

            var gridColumn = new GridViewColumn
            {
                Header = columnHeader,
                Width = width,
                CellTemplate = template
            };
            LookupGridView?.Columns.Add(gridColumn);

            return gridColumn;
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
        /// <exception cref="System.ArgumentOutOfRangeException"></exception>
        private void OnColumnClick(int columnIndex, bool mouseClick)
        {
            if (LookupGridView == null)
                return;

            if (columnIndex > LookupGridView.Columns.Count - 1)
            {
                SystemSounds.Exclamation.Play();
                return;
            }

            var orderByType = ViewModel.ControlSetup.OrderByType;
            _sortColumn = ViewModel.ControlSetup.ColumnList[columnIndex];
            if (_sortColumn == ViewModel.ControlSetup.SortColumn)
            {
                switch (orderByType)
                {
                    case OrderByTypes.Ascending:
                        ViewModel.ChangeOrderByType(OrderByTypes.Descending, _sortColumn);
                        break;
                    case OrderByTypes.Descending:
                        ViewModel.ChangeOrderByType(OrderByTypes.Ascending, _sortColumn);
                        break;
                    default:
                        throw new ArgumentOutOfRangeException();
                }
            }
            else
            {
                ViewModel.ChangeOrderByType(OrderByTypes.Ascending, _sortColumn);
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
                foreach (var lookupColumn in ViewModel.OrderByList)
                {
                    var orderColumnIndex = GetIndexOfVisibleColumnDefinition(lookupColumn);
                    var orderGridColumnHeader =
                        LookupGridView.Columns[orderColumnIndex].Header as GridViewColumnHeader;
                    GridViewSort.AddNonPrimarySortGlyph(orderGridColumnHeader, columnNumber);
                    columnNumber++;
                }

                if (resetSortOrder)
                    SetActiveColumn(columnIndex, _sortColumn.DataType);

                SearchForHost?.Control.Focus();
            }
        }
        /// <summary>
        /// Gets the index of visible column definition.
        /// </summary>
        /// <param name="listControlColumn">The list control column.</param>
        /// <returns>System.Int32.</returns>
        private int GetIndexOfVisibleColumnDefinition(ListControlColumn listControlColumn)
        {
            var lookupColumn = ViewModel.ControlSetup.ColumnList.FirstOrDefault(listControlColumn);
            return ViewModel.ControlSetup.GetIndexOfColumn(lookupColumn);
        }
        /// <summary>
        /// Sets the active column.
        /// </summary>
        /// <param name="sortColumnIndex">Index of the sort column.</param>
        /// <param name="dataType">Type of the data.</param>
        private void SetActiveColumn(int sortColumnIndex, FieldDataTypes dataType)
        {
            if (SearchForStackPanel != null)
            {
                if (SearchForHost != null)
                {
                    SearchForHost = null;
                    SearchForStackPanel.Children.Clear();
                }
                SearchForHost =
                    LookupControlsGlobals.LookupControlSearchForFactory.CreateSearchForHost(dataType);

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

            var column = ViewModel.ControlSetup.ColumnList[sortColumnIndex];

            if (!column.Caption.IsNullOrEmpty())
            {
                var headerText = column.Caption.Replace('\n', ' ').Replace("\r", "");
                if (SearchForLabel != null)
                    SearchForLabel.Content = $@"Search For {headerText}";
            }

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
        /// Gets the height of the header.
        /// </summary>
        /// <param name="header">The header.</param>
        /// <returns>System.Double.</returns>
        private double GetHeaderHeight(GridViewHeaderRowPresenter header)
        {
            var height = header.ActualHeight;
            var lineFeedCount = 0;
            var startIndex = 0;

            foreach (var column in ViewModel.ControlSetup.ColumnList)
            {
                var columnLineCount = 0;
                if (!column.Caption.IsNullOrEmpty())
                {
                    while (startIndex >= 0)
                    {
                        startIndex = column.Caption.IndexOf('\n', startIndex);
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

            return height;
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

            ListView.UpdateLayout();
            var binding = new Binding(TextBlock.TextProperty.Name);
            binding.RelativeSource = new RelativeSource(RelativeSourceMode.TemplatedParent);
            binding.Path = new PropertyPath(nameof(GridViewColumnHeader.Content));
            factory.SetBinding(TextBlock.TextProperty, binding);

            template.VisualTree = factory;

            return template;
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

            GridViewSort.ApplySort(_lastDirection, ListView, _lastHeaderClicked);
        }

        /// <summary>
        /// Gets the size of the page.
        /// </summary>
        /// <returns>System.Int32.</returns>
        private int GetPageSize()
        {
            if (ListView == null || LookupGridView == null)
                return 10;

            ListView.UpdateLayout();
            if (_itemHeight <= 0 || ListView.Items.Count == 0)
            {
                //ListView.ItemsSource = null;
                var addBlankRow = ListView.Items.Count <= 0;
                if (addBlankRow)
                {
                    ListView.Items.Add("text");
                }
                ListView.UpdateLayout();
                var item = ListView.Items.GetItemAt(0);

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

    }
}
