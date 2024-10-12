using System.Collections.Generic;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using RingSoft.DataEntryControls.Engine;
using RingSoft.DbLookup.App.Library.Northwind.LookupModel;
using RingSoft.DbLookup.App.Library.Northwind.Model;
using RingSoft.DbLookup.Lookup;

namespace RingSoft.DbLookup.App.Library.ViewModels
{
    public class NorthwindWindowViewModel : INotifyPropertyChanged
    {
        private LookupDefinition<OrderLookup, Order> _orderLookupDefinition;

        public LookupDefinition<OrderLookup, Order> OrderLookupDefinition
        {
            get { return _orderLookupDefinition; }
            set
            {
                if (_orderLookupDefinition == value)
                {
                    return;
                }
                _orderLookupDefinition = value;
                OnPropertyChanged();
            }
        }

        public UiCommand LookupUiCommand { get; }

        public NorthwindWindowViewModel()
        {
            OrderLookupDefinition = RsDbLookupAppGlobals.EfProcessor.NorthwindLookupContext
                .NorthwindContextConfiguration.OrderDateLookup.Clone();

            LookupUiCommand = new UiCommand();
        }

        public void Initialize()
        {
            OrderLookupDefinition.SetCommand(new LookupCommand(LookupCommands.Refresh));
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
