using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using RingSoft.DataEntryControls.Engine;
using RingSoft.DbLookup.QueryBuilder;

namespace RingSoft.DbLookup
{
    public interface IListControlView
    {
        ListControlDataSourceRow ShowLookupWindow();

        void SelectDataRow(ListControlDataSourceRow selectedRow);
    }
    public class ListControlColumn
    {
        public int ColumnId { get; internal set; }
        public string Caption { get; internal set; }
        public double PercentWidth { get; internal set; }
        public FieldDataTypes DataType { get; internal set; }
        public string ColumnName => $"COLUMN{ColumnId}";
    }

    public class ListControlSetup
    {
        public ListControlColumn SortColumn { get; set; }

        public OrderByTypes OrderByType { get; set; }

        public IReadOnlyList<ListControlColumn> ColumnList => _columns.AsReadOnly();

        private List<ListControlColumn> _columns = new List<ListControlColumn>();

        public ListControlColumn AddColumn(int columnId, string caption, FieldDataTypes dataType, double percentWidth)
        {
            var column = new ListControlColumn
            {
                ColumnId = columnId,
                Caption = caption,
                DataType = dataType,
                PercentWidth = percentWidth,
            };
            _columns.Add(column);
            if (SortColumn == null)
            {
                SortColumn = column;
            }
            return column;
        }

        public int GetIndexOfColumn(ListControlColumn listControlColumn)
        {
            return _columns.IndexOf(listControlColumn);
        }


    }

    public class ListControlDataCell
    {
        public string TextValue { get; set; }

        public decimal NumericValue { get; set; }

        public ListControlColumn Column { get; internal set; }

        public object GetSortValue()
        {
            switch (Column.DataType)
            {
                case FieldDataTypes.String:
                    return TextValue;
                case FieldDataTypes.Integer:
                case FieldDataTypes.Decimal:
                    return NumericValue;
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }

        public override string ToString()
        {
            switch (Column.DataType)
            {
                case FieldDataTypes.String:
                    return TextValue;
                case FieldDataTypes.Integer:
                case FieldDataTypes.Decimal:
                    return NumericValue.ToString();
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }
    }
    public class ListControlDataSourceRow
    {
        public IReadOnlyList<ListControlColumn> Columns => _columns.AsReadOnly();

        public IReadOnlyList<ListControlDataCell> DataCells => _dataCells.AsReadOnly();

        private List<ListControlColumn> _columns = new List<ListControlColumn>();
        private List<ListControlDataCell> _dataCells = new List<ListControlDataCell>();
        public string GetCellItem(int columnIndex)
        {
            return DataCells[columnIndex].TextValue;
        }

        public void AddColumn(ListControlColumn column, string value)
        {
            _columns.Add(column);
            var cell = new ListControlDataCell();
            switch (column.DataType)
            {
                case FieldDataTypes.String:
                    cell.TextValue = value;
                    break;
                case FieldDataTypes.Integer:
                case FieldDataTypes.Decimal:
                    cell.NumericValue = value.ToDecimal();
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }
            cell.Column = column;
            _dataCells.Add(cell);
        }

        public override string ToString()
        {
            return DataCells[0].TextValue;
        }
    }

    public class ListControlDataSource
    {
        public IReadOnlyList<ListControlDataSourceRow> Items => _dataSourceItems.AsReadOnly();

        private List<ListControlDataSourceRow> _dataSourceItems = new List<ListControlDataSourceRow>();

        public void AddRow(ListControlDataSourceRow item)
        {
            _dataSourceItems.Add(item);
        }
    }
    public class ListControlViewModel : INotifyPropertyChanged
    {
        private string _text;

        public string Text
        {
            get => _text;
            set
            {
                if (_text == value)
                {
                    return;
                }
                _text = value;
                OnPropertyChanged();
            }
        }

        public ListControlDataSource DataSource { get; set; }

        public ListControlSetup Setup { get; set; }

        private ListControlDataSourceRow _selectedDataRow;

        public ListControlDataSourceRow SelectedDataRow
        {
            get => _selectedDataRow;
            set
            {
                if (_selectedDataRow == value)
                {
                    return;
                }
                _selectedDataRow = value;
                View.SelectDataRow(_selectedDataRow);
            }
        }

        public IListControlView View { get; private set; }

        public RelayCommand ShowLookupCommand { get; private set; }

        public ListControlViewModel()
        {
            ShowLookupCommand = new RelayCommand(() =>
            {
                var selectedRow = View.ShowLookupWindow();
                if (selectedRow != null)
                {
                    SelectedDataRow = selectedRow;
                }
            });
        }

        public void Initialize(IListControlView view)
        {
            View = view;
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
