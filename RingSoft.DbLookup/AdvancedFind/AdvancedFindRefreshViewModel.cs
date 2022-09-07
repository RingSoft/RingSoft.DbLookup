using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Text;
using RingSoft.DataEntryControls.Engine;

namespace RingSoft.DbLookup.AdvancedFind
{
    public enum RefreshRate
    {
        Hours = 0,
        Minutes = 1,
        Seconds = 2,
    }

    public class AdvancedFindRefreshViewModel : INotifyPropertyChanged
    {
        private string _advancedFind;

        public string AdvancedFind
        {
            get => _advancedFind;
            set
            {
                if (_advancedFind == value)
                {
                    return;
                }

                _advancedFind = value;
                OnPropertyChanged();
            }
        }

        private TextComboBoxControlSetup _refreshRateSetup;

        public TextComboBoxControlSetup RefreshRateSetup
        {
            get => _refreshRateSetup;
            set
            {
                if (_refreshRateSetup == value)
                {
                    return;
                }

                _refreshRateSetup = value;
                OnPropertyChanged();
            }

        }

        private TextComboBoxItem _refreshRateItem;

        public TextComboBoxItem RefreshRateItem
        {
            get => _refreshRateItem;
            set
            {
                if (_refreshRateItem == value)
                {
                    return;
                }
                _refreshRateItem = value;
                OnPropertyChanged();
            }
        }

        public RefreshRate RefreshRate
        {
            get => (DbLookup.AdvancedFind.RefreshRate) RefreshRateItem.NumericValue;
            set => RefreshRateItem = RefreshRateSetup.GetItem((int) value);
        }

        public void Initialize(AdvancedFind advancedFindModel)
        {
            RefreshRateSetup = new TextComboBoxControlSetup();
            RefreshRateSetup.LoadFromEnum<RefreshRate>();

            AdvancedFind = advancedFindModel.Name;
        }

        public event PropertyChangedEventHandler PropertyChanged;

        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
