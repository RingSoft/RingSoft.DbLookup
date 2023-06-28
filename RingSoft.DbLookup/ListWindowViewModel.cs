using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices.ComTypes;
using MySqlX.XDevAPI.Relational;
using Org.BouncyCastle.Utilities;
using RingSoft.DataEntryControls.Engine;
using RingSoft.DbLookup.Lookup;
using RingSoft.DbLookup.QueryBuilder;

namespace RingSoft.DbLookup
{
    public enum ScrollPositions
    {
        Top = 0,
        Middle = 1,
        Bottom = 2,
    }
    public interface IListWindowView
    {
        void CloseWindow();

        void InitializeListView();

        void FillListView();
    }

    public class ListWindowViewModel : INotifyPropertyChanged
    {
        private string _searchText;

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

        public ListControlSetup ControlSetup { get; private set; }

        public ListControlDataSource DataSource { get; private set; }

        public ListControlDataSourceRow SelectedItem { get; private set; }

        public int SelectedIndex { get; private set; }

        public IListWindowView View { get; private set; }

        public RelayCommand SelectCommand { get; private set; }

        public RelayCommand CloseCommand { get; private set; }

        public bool DialogResult { get; private set; }

        public IReadOnlyList<ListControlColumn> OrderByList => _orderByList.AsReadOnly();

        private int _pageSize;
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

        private bool _dontSelectRowOnSearchChaged;

        public DataTable OutputTable { get; private set; } = new DataTable();

        public IReadOnlyList<ListControlDataSourceRow> OrderedList => _orderedList.AsReadOnly();

        public IReadOnlyList<ListControlDataSourceRow> CurrentPage => _currentPage.AsReadOnly();

        public ScrollPositions ScrollPosition { get; private set; }

        private List<ListControlDataSourceRow> _orderedList;
        private List<ListControlDataSourceRow> _ascList;
        private List<ListControlColumn> _orderByList = new List<ListControlColumn>();
        private List<ListControlDataSourceRow> _currentPage = new List<ListControlDataSourceRow>();
        private string _initSearchText;

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

        public void ChangeOrderByType(OrderByTypes orderByType, ListControlColumn column)
        {
            ControlSetup.OrderByType = orderByType;
            GetInitData(column);
            SelectedItem = _orderedList[0];
            SelectedIndex = 0;
        }

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

        public void GoHome()
        {
            _currentPage = GetTopPage();
            SetSelectedIndex(0);
            OutputData(_currentPage);
        }

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
                                                                           searchForText.ToDecimal().ToDouble());
                            break;
                        case OrderByTypes.Descending:
                            selectedRow = _ascList.FirstOrDefault(p => p.DataCells[sortColumnIndex].NumericValue >=
                                                                           searchForText.ToDecimal().ToDouble());
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

        private int GetTopSize()
        {
            var pageSize = (double)PageSize;
            var topSize = (int)(Math.Floor(pageSize / 2)) - 1;
            return topSize;
        }

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

        private List<ListControlDataSourceRow> GetBottomPage()
        {
            List<ListControlDataSourceRow> topPage;
            topPage = _orderedList.GetRange(_orderedList.Count - PageSize, PageSize);
            _currentPage.Clear();
            _currentPage.AddRange(topPage);
            return topPage;
        }

        public void SetSelectedIndex(int index)
        {
            SelectedIndex = index;
            SelectedItem = _currentPage[index];
        }

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
        public event PropertyChangedEventHandler PropertyChanged;

        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        protected bool SetField<T>(ref T field, T value, [CallerMemberName] string propertyName = null)
        {
            if (EqualityComparer<T>.Default.Equals(field, value)) return false;
            field = value;
            OnPropertyChanged(propertyName);
            return true;
        }
    }
}
