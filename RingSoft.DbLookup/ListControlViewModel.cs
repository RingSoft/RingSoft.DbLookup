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
        ListControlDataSourceItem ShowLookupWindow();
    }
    public class ListControlColumn
    {
        public int ColumnId { get; internal set; }
        public string Caption { get; internal set; }
        public double PercentWidth { get; internal set; }
        public FieldDataTypes DataType { get; internal set; }
    }

    public class ListControlSetup
    {
        public ListControlColumn InitialSortColumn { get; set; }

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
            if (InitialSortColumn == null)
            {
                InitialSortColumn = column;
            }
            return column;
        }

        public int GetIndexOfColumn(ListControlColumn listControlColumn)
        {
            var lookupColumn = _columns.FirstOrDefault(listControlColumn);
            return _columns.IndexOf(lookupColumn);
        }


    }

    public class ListControlDataSourceItem
    {
        public ListControlColumn Column { get; internal set; }

        public string DataItem { get; internal set; }
    }

    public class ListControlDataSource
    {
        public IReadOnlyList<ListControlDataSourceItem> Items => _dataSourceItems.AsReadOnly();

        private List<ListControlDataSourceItem> _dataSourceItems = new List<ListControlDataSourceItem>();

        public ListControlDataSourceItem AddDataItem(ListControlColumn column, string dataValue)
        {
            var dataItem = new ListControlDataSourceItem
            {
                Column = column,
                DataItem = dataValue,
            };
            _dataSourceItems.Add(dataItem);
            return dataItem;
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

        public ListControlDataSourceItem SelectedDataItem { get; set; }

        public IListControlView View { get; private set; }

        public RelayCommand ShowLookupCommand { get; private set; }

        public ListControlViewModel()
        {
            ShowLookupCommand = new RelayCommand(() =>
            {
                SelectedDataItem = View.ShowLookupWindow();
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
