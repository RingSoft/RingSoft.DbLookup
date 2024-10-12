using RingSoft.DataEntryControls.Engine;
using RingSoft.DbLookup.App.Library.Northwind.LookupModel;
using RingSoft.DbLookup.App.Library.Northwind.Model;
using RingSoft.DbLookup.Lookup;
using System.Collections.Generic;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using RingSoft.DbLookup.App.Library.MegaDb.LookupModel;
using RingSoft.DbLookup.App.Library.MegaDb.Model;

namespace RingSoft.DbLookup.App.Library.ViewModels
{
    public class MegaDbMainViewModel : INotifyPropertyChanged
    {
        private LookupDefinition<ItemLookup, Item> _itemLookupDefinition;

        public LookupDefinition<ItemLookup, Item> ItemLookupDefinition
        {
            get { return _itemLookupDefinition; }
            set
            {
                if (_itemLookupDefinition == value)
                {
                    return;
                }
                _itemLookupDefinition = value;
                OnPropertyChanged();
            }
        }

        private LookupDefinition<StockMasterLookup, StockMaster> _stockLookupDefinition;

        public LookupDefinition<StockMasterLookup, StockMaster> StockLookupDefinition
        {
            get { return _stockLookupDefinition; }
            set
            {
                if (_stockLookupDefinition == value)
                {
                    return;
                }
                _stockLookupDefinition = value;
                OnPropertyChanged();
            }
        }


        public UiCommand LookupUiCommand { get; }

        public MegaDbMainViewModel()
        {
            ItemLookupDefinition = RsDbLookupAppGlobals.EfProcessor.MegaDbLookupContext
                .MegaDbContextConfiguration.ItemsLookup.Clone();

            StockLookupDefinition = RsDbLookupAppGlobals.EfProcessor.MegaDbLookupContext
                .MegaDbContextConfiguration.StockMasterLookup.Clone();

            LookupUiCommand = new UiCommand();
        }

        public void Initialize()
        {
            ItemLookupDefinition.SetCommand(new LookupCommand(LookupCommands.Refresh));
            StockLookupDefinition.SetCommand(new LookupCommand(LookupCommands.Refresh));
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
