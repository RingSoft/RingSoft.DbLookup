using System.Collections.Generic;
using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace RingSoft.DbLookup
{
    public interface IListWindowView
    {
        void CloseWindow();

        void InitializeListView();
    }

    public class ListWindowViewModel : INotifyPropertyChanged
    {
        public ListControlSetup ControlSetup { get; private set; }

        public ListControlDataSource DataSource { get; private set; }

        public IListWindowView View { get; private set; }

        public IReadOnlyList<ListControlColumn> OrderByList => _orderByList.AsReadOnly();

        private List<ListControlColumn> _orderByList = new List<ListControlColumn>();

        public void Initialize(ListControlSetup setup, ListControlDataSource dataSource, IListWindowView view)
        {
            ControlSetup = setup;
            DataSource = dataSource;
            View = view;
            view.InitializeListView();
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
