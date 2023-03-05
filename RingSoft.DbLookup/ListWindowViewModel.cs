﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices.ComTypes;
using MySqlX.XDevAPI.Relational;
using Org.BouncyCastle.Utilities;
using RingSoft.DataEntryControls.Engine;

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
                OnSearchForChanged(value);
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

        public DataTable OutputTable { get; private set; } = new DataTable();

        public IReadOnlyList<ListControlDataSourceRow> OrderedList => _orderedList.AsReadOnly();

        public IReadOnlyList<ListControlDataSourceRow> CurrentPage => _currentPage.AsReadOnly();

        public ScrollPositions ScrollPosition { get; private set; }

        private List<ListControlDataSourceRow> _orderedList;
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

        public void Initialize(ListControlSetup setup, ListControlDataSource dataSource, IListWindowView view
        , ListControlDataSourceRow selectedRow)
        {
            ControlSetup = setup;
            DataSource = dataSource;
            View = view;
            view.InitializeListView();
            foreach (var listControlColumn in ControlSetup.ColumnList)
            {
                OutputTable.Columns.Add($"COLUMN{listControlColumn.ColumnId}");
            }
            GetInitData(setup.SortColumn);
            if (selectedRow != null)
            {
                if (PageSize == 0)
                {
                    _initSearchText = selectedRow.GetCellItem(0);
                }
            }
        }

        public void GetInitData(ListControlColumn sortColumn)
        {
            ControlSetup.SortColumn = sortColumn;
            var sortColumnIndex = ControlSetup.GetIndexOfColumn(ControlSetup.SortColumn);
            _orderedList = DataSource.Items.OrderBy(p => p.DataCells[sortColumnIndex].GetSortValue()).ToList();
            if (PageSize > 0)
            {
                var rows = _orderedList.GetRange(0, PageSize);
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
            else
            {
                SearchText = _initSearchText;
                _initSearchText = string.Empty;
            }
        }

        private void OnSearchForChanged(string searchForText)
        {
            var topPage = new List<ListControlDataSourceRow>();
            var bottomPage = new List<ListControlDataSourceRow>();

            var pageSize = (double)PageSize;
            var topSize = GetTopSize();
            var bottomSize = PageSize - topSize;

            ListControlDataSourceRow selectedRow = null;
            var sortColumnIndex = ControlSetup.GetIndexOfColumn(ControlSetup.SortColumn);
            switch (ControlSetup.SortColumn.DataType)
            {
                case FieldDataTypes.String:
                    selectedRow = _orderedList.FirstOrDefault(p => p.DataCells[sortColumnIndex].TextValue
                        .CompareTo(searchForText) >= 0);
                    break;
                case FieldDataTypes.Integer:
                case FieldDataTypes.Decimal:
                    selectedRow = _orderedList.FirstOrDefault(p => p.DataCells[sortColumnIndex].NumericValue >=
                                                                   searchForText.ToDecimal());

                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }

            var selectedIndex = _orderedList.IndexOf(selectedRow);
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

        public void GetNextPage(int startingIndex)
        {
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
