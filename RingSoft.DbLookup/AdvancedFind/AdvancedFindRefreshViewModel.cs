using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using RingSoft.DataEntryControls.Engine;
using RingSoft.DbLookup.QueryBuilder;

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
            get => (RefreshRate) RefreshRateItem.NumericValue;
            set => RefreshRateItem = RefreshRateSetup.GetItem((int) value);
        }

        private TextComboBoxControlSetup _refreshConditionSetup;

        private int _refreshValue;

        public int RefreshValue
        {
            get => _refreshValue;
            set
            {
                if (_refreshValue == value)
                {
                    return;
                }
                _refreshValue = value;
                OnPropertyChanged();
            }
        }


        public TextComboBoxControlSetup RefreshConditionSetup
        {
            get => _refreshConditionSetup;
            set
            {
                if (_refreshConditionSetup == value)
                {
                    return;
                }
                _refreshConditionSetup = value;
                OnPropertyChanged();
            }
        }

        private TextComboBoxItem _refreshConditionItem;

        public TextComboBoxItem RefreshConditionItem
        {
            get => _refreshConditionItem;
            set
            {
                if (_refreshConditionItem == value)
                {
                    return;
                }
                _refreshConditionItem = value;
                OnPropertyChanged();
            }
        }

        public Conditions RefreshCondition
        {
            get => (Conditions)RefreshConditionItem.NumericValue;
            set => RefreshConditionItem = RefreshConditionSetup.GetItem((int)value);
        }

        private int _yellowAlert;

        public int YellowAlert
        {
            get => _yellowAlert;
            set
            {
                if (_yellowAlert == value)
                {
                    return;
                }
                _yellowAlert = value;
                OnPropertyChanged();
            }
        }

        private int _redAlert;

        public int RedAlert
        {
            get => _redAlert;
            set
            {
                if (_redAlert == value)
                {
                    return;
                }
                _redAlert = value;
                OnPropertyChanged();
            }
        }

        public AdvancedFind Properties { get; set; }

        public void Initialize(AdvancedFind advancedFindModel)
        {
            Properties = advancedFindModel;
            RefreshRateSetup = new TextComboBoxControlSetup();
            RefreshRateSetup.LoadFromEnum<RefreshRate>();

            RefreshConditionSetup = new TextComboBoxControlSetup();
            RefreshConditionSetup.LoadFromEnum<Conditions>();

            RefreshConditionSetup.Items.Remove(
                RefreshConditionSetup.Items.FirstOrDefault(p =>
                    p.NumericValue == (int)Conditions.Contains));

            RefreshConditionSetup.Items.Remove(
                RefreshConditionSetup.Items.FirstOrDefault(p =>
                    p.NumericValue == (int)Conditions.NotContains));

            RefreshConditionSetup.Items.Remove(
                RefreshConditionSetup.Items.FirstOrDefault(p =>
                    p.NumericValue == (int)Conditions.EndsWith));

            RefreshConditionSetup.Items.Remove(
                RefreshConditionSetup.Items.FirstOrDefault(p =>
                    p.NumericValue == (int)Conditions.BeginsWith));

            RefreshConditionSetup.Items.Remove(
                RefreshConditionSetup.Items.FirstOrDefault(p =>
                    p.NumericValue == (int)Conditions.EqualsNull));

            RefreshConditionSetup.Items.Remove(
                RefreshConditionSetup.Items.FirstOrDefault(p =>
                    p.NumericValue == (int)Conditions.NotEqualsNull));

            AdvancedFind = advancedFindModel.Name;
            if (advancedFindModel.RefreshRate.HasValue)
            {
                RefreshRate = (DbLookup.AdvancedFind.RefreshRate) advancedFindModel.RefreshRate;
            }

            if (advancedFindModel.RefreshValue.HasValue)
            {
                RefreshValue = advancedFindModel.RefreshValue.Value;
            }

            if (advancedFindModel.RefreshCondition.HasValue)
            {
                RefreshCondition = (Conditions) advancedFindModel.RefreshCondition.Value;
            }

            if (advancedFindModel.YellowAlert.HasValue)
            {
                YellowAlert = advancedFindModel.YellowAlert.Value;
            }

            if (advancedFindModel.RedAlert.HasValue)
            {
                RedAlert = advancedFindModel.RedAlert.Value;
            }
        }

        public void RefreshProperties()
        {
            if (RefreshRateItem != null)
            {
                Properties.RefreshRate = (byte)RefreshRate;
            }

            if (RefreshConditionItem != null)
            {
                Properties.RefreshCondition = (byte)RefreshCondition;
            }
            
            Properties.RefreshValue = (int)RefreshValue;
            Properties.YellowAlert = (int)YellowAlert;
            Properties.RedAlert = (int)RedAlert;
        }

        public event PropertyChangedEventHandler PropertyChanged;

        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
