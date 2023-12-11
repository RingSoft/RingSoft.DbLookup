// ***********************************************************************
// Assembly         : RingSoft.DbLookup
// Author           : petem
// Created          : 03-04-2023
//
// Last Modified By : petem
// Last Modified On : 07-24-2023
// ***********************************************************************
// <copyright file="ListWindowViewModel.cs" company="Peter Ringering">
//     Copyright (c) . All rights reserved.
// </copyright>
// <summary></summary>
// ***********************************************************************
using RingSoft.DataEntryControls.Engine;
using RingSoft.DbLookup.QueryBuilder;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Linq;
using System.Runtime.CompilerServices;

namespace RingSoft.DbLookup
{
    /// <summary>
    /// Enum ScrollPositions
    /// </summary>
    public enum ScrollPositions
    {
        /// <summary>
        /// The top
        /// </summary>
        Top = 0,
        /// <summary>
        /// The middle
        /// </summary>
        Middle = 1,
        /// <summary>
        /// The bottom
        /// </summary>
        Bottom = 2,
    }
    /// <summary>
    /// Interface IListWindowView
    /// </summary>
    public interface IListWindowView
    {
        /// <summary>
        /// Closes the window.
        /// </summary>
        void CloseWindow();

        /// <summary>
        /// Initializes the ListView.
        /// </summary>
        void InitializeListView();

        /// <summary>
        /// Fills the ListView.
        /// </summary>
        void FillListView();
    }

    /// <summary>
    /// Class ListWindowViewModel.
    /// Implements the <see cref="INotifyPropertyChanged" />
    /// </summary>
    /// <seealso cref="INotifyPropertyChanged" />
    public class ListWindowViewModel : INotifyPropertyChanged
    {
        /// <summary>
        /// The search text
        /// </summary>
        private string _searchText;

        /// <summary>
        /// Gets or sets the search text.
        /// </summary>
        /// <value>The search text.</value>
        public string SearchText
        {
            get => _searchText;
            set
            {
                if (_searchText == value)
                {
                    return;
                }
                _searchText = value;
                if (!_dontSelectRowOnSearchChaged)
                {
                    OnSearchForChanged(value);
                }
            }
        }

        /// <summary>
        /// Gets the control setup.
        /// </summary>
        /// <value>The control setup.</value>
        public ListControlSetup ControlSetup { get; private set; }

        /// <summary>
        /// Gets the data source.
        /// </summary>
        /// <value>The data source.</value>
        public ListControlDataSource DataSource { get; private set; }

        /// <summary>
        /// Gets the selected item.
        /// </summary>
        /// <value>The selected item.</value>
        public ListControlDataSourceRow SelectedItem { get; private set; }

        /// <summary>
        /// Gets the index of the selected.
        /// </summary>
        /// <value>The index of the selected.</value>
        public int SelectedIndex { get; private set; }

        /// <summary>
        /// Gets the view.
        /// </summary>
        /// <value>The view.</value>
        public IListWindowView View { get; private set; }

        /// <summary>
        /// Gets the select command.
        /// </summary>
        /// <value>The select command.</value>
        public RelayCommand SelectCommand { get; private set; }

        /// <summary>
        /// Gets the close command.
        /// </summary>
        /// <value>The close command.</value>
        public RelayCommand CloseCommand { get; private set; }

        /// <summary>
        /// Gets a value indicating whether [dialog result].
        /// </summary>
        /// <value><c>true</c> if [dialog result]; otherwise, <c>false</c>.</value>
        public bool DialogResult { get; private set; }

        /// <summary>
        /// Gets the order by list.
        /// </summary>
        /// <value>The order by list.</value>
        public IReadOnlyList<ListControlColumn> OrderByList => _orderByList.AsReadOnly();

        /// <summary>
        /// The page size
        /// </summary>
        private int _pageSize;
        /// <summary>
        /// Gets or sets the size of the page.
        /// </summary>
        /// <value>The size of the page.</value>
        public int PageSize
        {
            get => _pageSize;
            set
            {
                if (_pageSize != value)
                {
                    _pageSize = value;
                    OnPageSizeChanged(_pageSize);
                }
            }
        }

        /// <summary>
        /// The dont select row on search chaged
        /// </summary>
        private bool _dontSelectRowOnSearchChaged;

        /// <summary>
        /// Gets the output table.
        /// </summary>
        /// <value>The output table.</value>
        public DataTable OutputTable { get; private set; } = new DataTable();

        /// <summary>
        /// Gets the ordered list.
        /// </summary>
        /// <value>The ordered list.</value>
        public IReadOnlyList<ListControlDataSourceRow> OrderedList => _orderedList.AsReadOnly();

        /// <summary>
        /// Gets the current page.
        /// </summary>
        /// <value>The current page.</value>
        public IReadOnlyList<ListControlDataSourceRow> CurrentPage => _currentPage.AsReadOnly();

        /// <summary>
        /// Gets the scroll position.
        /// </summary>
        /// <value>The scroll position.</value>
        public ScrollPositions ScrollPosition { get; private set; }

        /// <summary>
        /// The ordered list
        /// </summary>
        private List<ListControlDataSourceRow> _orderedList;
        /// <summary>
        /// The asc list
        /// </summary>
        private List<ListControlDataSourceRow> _ascList;
        /// <summary>
        /// The order by list
        /// </summary>
        private List<ListControlColumn> _orderByList = new List<ListControlColumn>();
        /// <summary>
        /// The current page
        /// </summary>
        private List<ListControlDataSourceRow> _currentPage = new List<ListControlDataSourceRow>();
        /// <summary>
        /// The initialize search text
        /// </summary>
        private string _initSearchText;

        /// <summary>
        /// Initializes a new instance of the <see cref="ListWindowViewModel"/> class.
        /// </summary>
        public ListWindowViewModel()
        {
            SelectCommand = new RelayCommand(() =>
            {
                DialogResult = true;
                View.CloseWindow();
            });
            CloseCommand = new RelayCommand(() =>
            {
                DialogResult = false;
                View.CloseWindow();
            });
        }

        /// <summary>
        /// Initializes the specified setup.
        /// </summary>
        /// <param name="setup">The setup.</param>
        /// <param name="dataSource">The data source.</param>
        /// <param name="view">The view.</param>
        public void Initialize(ListControlSetup setup, ListControlDataSource dataSource, IListWindowView view)
        {
            ControlSetup = setup;
            ControlSetup.OrderByType = OrderByTypes.Ascending;
            ControlSetup.SortColumn = ControlSetup.ColumnList[0];
            DataSource = dataSource;
            View = view;
            view.InitializeListView();
            foreach (var listControlColumn in ControlSetup.ColumnList)
            {
                OutputTable.Columns.Add($"COLUMN{listControlColumn.ColumnId}");
            }
            GetInitData(setup.SortColumn);
        }

        /// <summary>
        /// Selects the row.
        /// </summary>
        /// <param name="selectedRow">The selected row.</param>
        /// <param name="pageSize">Size of the page.</param>
        public void SelectRow(ListControlDataSourceRow selectedRow, int pageSize)
        {
            PageSize = pageSize;
            
            SelectRow(selectedRow);
            SetSelectedIndex(_currentPage.IndexOf(selectedRow));
            _dontSelectRowOnSearchChaged = true;
            SearchText = selectedRow.DataCells[0].TextValue;
            _initSearchText = SearchText;
            _dontSelectRowOnSearchChaged = false;
        }

        /// <summary>
        /// Changes the type of the order by.
        /// </summary>
        /// <param name="orderByType">Type of the order by.</param>
        /// <param name="column">The column.</param>
        public void ChangeOrderByType(OrderByTypes orderByType, ListControlColumn column)
        {
            ControlSetup.OrderByType = orderByType;
            GetInitData(column);
            SelectedItem = _orderedList[0];
            SelectedIndex = 0;
        }

        /// <summary>
        /// Gets the initialize data.
        /// </summary>
        /// <param name="sortColumn">The sort column.</param>
        /// <exception cref="System.ArgumentOutOfRangeException"></exception>
        public void GetInitData(ListControlColumn sortColumn)
        {
            ControlSetup.SortColumn = sortColumn;
            var sortColumnIndex = ControlSetup.GetIndexOfColumn(ControlSetup.SortColumn);

            _ascList = DataSource.Items
                .OrderBy(p => p.DataCells[sortColumnIndex].GetSortValue()).ToList();


            switch (ControlSetup.OrderByType)
            {
                case OrderByTypes.Ascending:
                    _orderedList = DataSource.Items
                        .OrderBy(p => p.DataCells[sortColumnIndex].GetSortValue()).ToList();
                    break;
                case OrderByTypes.Descending:
                    _orderedList = DataSource.Items
                        .OrderByDescending(p => p.DataCells[sortColumnIndex].GetSortValue()).ToList();
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }
            if (PageSize > 0)
            {
                var size = PageSize;
                if (size > _orderedList.Count)
                {
                    size = _orderedList.Count;
                }
                var rows = _orderedList.GetRange(0, size);
                _currentPage.Clear();
                _currentPage.AddRange(rows);
                OutputData(rows);
            }
        }

        /// <summary>
        /// Outputs the data.
        /// </summary>
        /// <param name="rows">The rows.</param>
        private void OutputData(List<ListControlDataSourceRow> rows)
        {
            var topRow = rows[0];
            var bottomRow = rows[rows.Count - 1];
            if (_orderedList.IndexOf(topRow) == 0)
            {
                ScrollPosition = ScrollPositions.Top;
            }
            else if (_orderedList.IndexOf(bottomRow) == _orderedList.Count - 1)
            {
                ScrollPosition = ScrollPositions.Bottom;
            }
            else
            {
                ScrollPosition = ScrollPositions.Middle;
            }
            OutputTable.Rows.Clear();
            
            foreach (var listControlDataSourceRow in rows)
            {
                var dataRow = OutputTable.NewRow();
                var columnIndex = 0;
                foreach (var listControlColumn in listControlDataSourceRow.Columns)
                {
                    var columnName = listControlColumn.ColumnName;
                    if (OutputTable.Columns.Contains(columnName))
                    {
                        var cell = listControlDataSourceRow.DataCells[columnIndex].GetSortValue();
                        dataRow[columnName] = cell;
                    }

                    columnIndex++;
                }
                OutputTable.Rows.Add(dataRow);
            }

            View.FillListView();
        }
        /// <summary>
        /// Called when [page size changed].
        /// </summary>
        /// <param name="pageSize">Size of the page.</param>
        private void OnPageSizeChanged(int pageSize)
        {
            if (_initSearchText.IsNullOrEmpty())
            {
                GetInitData(ControlSetup.SortColumn);
            }
            else if (SelectedItem != null)
            {
                //SearchText = _initSearchText;
                //_initSearchText = string.Empty;
                SelectRow(SelectedItem);
            }
        }

        /// <summary>
        /// Goes the home.
        /// </summary>
        public void GoHome()
        {
            _currentPage = GetTopPage();
            SetSelectedIndex(0);
            OutputData(_currentPage);
        }

        /// <summary>
        /// Goes the end.
        /// </summary>
        public void GoEnd()
        {
            if (ScrollPosition != ScrollPositions.Bottom 
                && SelectedIndex == _currentPage.Count - 1
                && PageSize == _currentPage.Count)
            {
                _currentPage = GetBottomPage();
                SetSelectedIndex(_currentPage.Count - 1);
                OutputData(_currentPage);
            }
        }

        /// <summary>
        /// Called when [search for changed].
        /// </summary>
        /// <param name="searchForText">The search for text.</param>
        /// <exception cref="System.ArgumentOutOfRangeException"></exception>
        private void OnSearchForChanged(string searchForText)
        {
            if (searchForText.IsNullOrEmpty())
            {
                _currentPage = GetTopPage();
                SetSelectedIndex(0);
                OutputData(_currentPage);
                return;
            }

            ListControlDataSourceRow selectedRow = null;
            var sortColumnIndex = ControlSetup.GetIndexOfColumn(ControlSetup.SortColumn);
            switch (ControlSetup.SortColumn.DataType)
            {
                case FieldDataTypes.String:
                    switch (ControlSetup.OrderByType)
                    {
                        case OrderByTypes.Ascending:
                            selectedRow = _orderedList.FirstOrDefault(p => p.DataCells[sortColumnIndex].TextValue
                                .CompareTo(searchForText) >= 0);
                            break;
                        case OrderByTypes.Descending:
                            selectedRow = _ascList.FirstOrDefault(p => p.DataCells[sortColumnIndex].TextValue
                                .CompareTo(searchForText) >= 0);
                            break;
                        default:
                            throw new ArgumentOutOfRangeException();
                    }
                    break;
                case FieldDataTypes.Integer:
                case FieldDataTypes.Decimal:
                    switch (ControlSetup.OrderByType)
                    {
                        case OrderByTypes.Ascending:
                            selectedRow = _orderedList.FirstOrDefault(p => p.DataCells[sortColumnIndex].NumericValue >=
                                                                           searchForText.ToDecimal());
                            break;
                        case OrderByTypes.Descending:
                            selectedRow = _ascList.FirstOrDefault(p => p.DataCells[sortColumnIndex].NumericValue >=
                                                                           searchForText.ToDecimal());
                            break;
                        default:
                            throw new ArgumentOutOfRangeException();
                    }
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }

            SelectRow(selectedRow);
        }

        /// <summary>
        /// Selects the row.
        /// </summary>
        /// <param name="selectedRow">The selected row.</param>
        private void SelectRow(ListControlDataSourceRow selectedRow)
        {
            var topPage = new List<ListControlDataSourceRow>();
            var bottomPage = new List<ListControlDataSourceRow>();

            var topSize = GetTopSize();
            var bottomSize = PageSize - topSize;

            var selectedIndex = _orderedList.IndexOf(selectedRow);

            if (_orderedList.Count < PageSize)
            {
                _currentPage = _orderedList;
                SetSelectedIndex(selectedIndex);
                OutputData(_currentPage);
                return;
            }

            if (selectedIndex >= 0)
            {
                if (selectedIndex == 0)
                {
                    topPage = GetTopPage();
                }
                else
                {
                    if (selectedIndex < topSize - 1)
                    {
                        topSize = selectedIndex;
                        bottomSize = PageSize - topSize;
                    }

                    if (bottomSize + selectedIndex > _orderedList.Count)
                    {
                        topPage = GetBottomPage();
                    }
                    else
                    {
                        if (selectedIndex - topSize < 0)
                        {
                            topPage = _orderedList.GetRange(0, selectedIndex);
                        }
                        else
                        {
                            topPage = _orderedList.GetRange(selectedIndex - topSize, topSize);
                        }

                        if (selectedIndex + bottomSize > _orderedList.Count)
                        {
                            bottomPage = _orderedList.GetRange(selectedIndex, bottomSize - selectedIndex);
                        }
                        else
                        {
                            bottomPage = _orderedList.GetRange(selectedIndex, bottomSize);
                        }
                    }
                }
            }
            else
            {
                bottomPage = _orderedList.GetRange(_orderedList.Count - PageSize, PageSize);
            }

            _currentPage.Clear();
            _currentPage.AddRange(topPage);
            _currentPage.AddRange(bottomPage);
            SelectedItem = selectedRow;
            SelectedIndex = _currentPage.IndexOf(selectedRow);
            OutputData(_currentPage);
        }

        /// <summary>
        /// Gets the size of the top.
        /// </summary>
        /// <returns>System.Int32.</returns>
        private int GetTopSize()
        {
            var pageSize = (double)PageSize;
            var topSize = (int)(Math.Floor(pageSize / 2)) - 1;
            return topSize;
        }

        /// <summary>
        /// Gets the top page.
        /// </summary>
        /// <returns>List&lt;ListControlDataSourceRow&gt;.</returns>
        private List<ListControlDataSourceRow> GetTopPage()
        {
            List<ListControlDataSourceRow> topPage;
            if (_orderedList.Count < PageSize)
            {
                _currentPage = _orderedList;
                return _orderedList;
            }

            topPage = _orderedList.GetRange(0, PageSize);
            _currentPage.Clear();
            _currentPage.AddRange(topPage);
            return topPage;
        }

        /// <summary>
        /// Gets the bottom page.
        /// </summary>
        /// <returns>List&lt;ListControlDataSourceRow&gt;.</returns>
        private List<ListControlDataSourceRow> GetBottomPage()
        {
            List<ListControlDataSourceRow> topPage;
            topPage = _orderedList.GetRange(_orderedList.Count - PageSize, PageSize);
            _currentPage.Clear();
            _currentPage.AddRange(topPage);
            return topPage;
        }

        /// <summary>
        /// Sets the index of the selected.
        /// </summary>
        /// <param name="index">The index.</param>
        public void SetSelectedIndex(int index)
        {
            SelectedIndex = index;
            SelectedItem = _currentPage[index];
        }

        /// <summary>
        /// Called when [mouse wheel forward].
        /// </summary>
        public void OnMouseWheelForward()
        {
            var newSelectedRowIndex = 3;

            if (ScrollPosition == ScrollPositions.Bottom)
            {
                return;
            }
            SetSelectedIndex(newSelectedRowIndex);
            GetNextPage(newSelectedRowIndex);
        }

        /// <summary>
        /// Called when [mouse wheel back].
        /// </summary>
        public void OnMouseWheelBack()
        {
            var newSelectedRowIndex = _currentPage.Count - 3;
            if (newSelectedRowIndex < 0)
            {
                _currentPage = GetTopPage();
                SetSelectedIndex(0);
                OutputData(_currentPage);
                return;
            }
            GetPreviousPage(newSelectedRowIndex);
        }

        /// <summary>
        /// Gets the next page.
        /// </summary>
        /// <param name="startingIndex">Index of the starting.</param>
        public void GetNextPage(int startingIndex)
        {
            if (_orderedList.Count < PageSize)
            {
                return;
            }

            var index = _orderedList.IndexOf(SelectedItem);
            List<ListControlDataSourceRow> nextPage = null;
            if (index + PageSize > _orderedList.Count && startingIndex > 0)
            {
                nextPage = GetBottomPage();
            }
            else
            {
                var lastRow = _currentPage[startingIndex];
                if (lastRow != _orderedList.Last())
                {
                    var lastRowIndex = _orderedList.IndexOf(lastRow);
                    if (startingIndex == 0 && lastRowIndex + PageSize >= _orderedList.Count)
                    {
                        nextPage = GetBottomPage();
                    }

                    if (nextPage == null)
                    {
                        nextPage = _orderedList.GetRange(lastRowIndex + 1, PageSize);
                    }
                }
            }

            _currentPage = nextPage;
            SelectedIndex = nextPage.Count - 1;
            SelectedItem = nextPage[SelectedIndex];
            OutputData(nextPage);

        }

        /// <summary>
        /// Gets the previous page.
        /// </summary>
        /// <param name="startingIndex">Index of the starting.</param>
        public void GetPreviousPage(int startingIndex)
        {
            if (_orderedList.Count < PageSize)
            {
                return;
            }

            var upArrow = startingIndex == _currentPage.Count - 1;
            var index = _orderedList.IndexOf(SelectedItem);
            List<ListControlDataSourceRow> previousPage = null;
            var topSize = GetTopSize();
            if (index < topSize && !upArrow)
            {
                previousPage = GetTopPage();
            }
            else
            {
                var firstRow = _currentPage[startingIndex];
                if (firstRow  != _orderedList.First())
                {
                    var firstRowIndex = _orderedList.IndexOf(firstRow);
                    if (firstRowIndex - PageSize < 0)
                    {
                        previousPage = GetTopPage();
                    }
                    else
                    {
                        previousPage = _orderedList.GetRange(firstRowIndex - PageSize, PageSize);
                    }
                }
                else
                {
                    previousPage = GetTopPage();
                }
            }

            _currentPage = previousPage;
            SelectedIndex = 0;
            SelectedItem = previousPage[SelectedIndex];
            OutputData(previousPage);

        }
        /// <summary>
        /// Occurs when a property value changes.
        /// </summary>
        public event PropertyChangedEventHandler PropertyChanged;

        /// <summary>
        /// Called when [property changed].
        /// </summary>
        /// <param name="propertyName">Name of the property.</param>
        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        /// <summary>
        /// Sets the field.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="field">The field.</param>
        /// <param name="value">The value.</param>
        /// <param name="propertyName">Name of the property.</param>
        /// <returns><c>true</c> if XXXX, <c>false</c> otherwise.</returns>
        protected bool SetField<T>(ref T field, T value, [CallerMemberName] string propertyName = null)
        {
            if (EqualityComparer<T>.Default.Equals(field, value)) return false;
            field = value;
            OnPropertyChanged(propertyName);
            return true;
        }
    }
}
