// ***********************************************************************
// Assembly         : RingSoft.DbLookup
// Author           : petem
// Created          : 12-19-2022
//
// Last Modified By : petem
// Last Modified On : 07-07-2023
// ***********************************************************************
// <copyright file="AdvancedFindRefreshViewModel.cs" company="Peter Ringering">
//     Copyright (c) . All rights reserved.
// </copyright>
// <summary></summary>
// ***********************************************************************
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
    /// <summary>
    /// Enum RefreshRate
    /// </summary>
    public enum RefreshRate
    {
        /// <summary>
        /// The none
        /// </summary>
        None = 3,
        /// <summary>
        /// The hours
        /// </summary>
        Hours = 0,
        /// <summary>
        /// The minutes
        /// </summary>
        Minutes = 1,
    }

    /// <summary>
    /// Class AdvancedFindRefreshViewModel.
    /// Implements the <see cref="INotifyPropertyChanged" />
    /// </summary>
    /// <seealso cref="INotifyPropertyChanged" />
    public class AdvancedFindRefreshViewModel : INotifyPropertyChanged
    {
        /// <summary>
        /// The advanced find
        /// </summary>
        private string _advancedFind;

        /// <summary>
        /// Gets or sets the advanced find.
        /// </summary>
        /// <value>The advanced find.</value>
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

        /// <summary>
        /// The refresh rate setup
        /// </summary>
        private TextComboBoxControlSetup _refreshRateSetup;

        /// <summary>
        /// Gets or sets the refresh rate setup.
        /// </summary>
        /// <value>The refresh rate setup.</value>
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

        /// <summary>
        /// The refresh rate item
        /// </summary>
        private TextComboBoxItem _refreshRateItem;

        /// <summary>
        /// Gets or sets the refresh rate item.
        /// </summary>
        /// <value>The refresh rate item.</value>
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

        /// <summary>
        /// Gets or sets the refresh rate.
        /// </summary>
        /// <value>The refresh rate.</value>
        public RefreshRate RefreshRate
        {
            get => (RefreshRate) RefreshRateItem.NumericValue;
            set
            {
                var item = RefreshRateSetup.Items
                    .FirstOrDefault(p => p.NumericValue == (int)value);
                if (item != null)
                {
                    RefreshRateItem = item;
                }
            }
        }

        /// <summary>
        /// The refresh condition setup
        /// </summary>
        private TextComboBoxControlSetup _refreshConditionSetup;

        /// <summary>
        /// The refresh value
        /// </summary>
        private int _refreshValue;

        /// <summary>
        /// Gets or sets the refresh value.
        /// </summary>
        /// <value>The refresh value.</value>
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


        /// <summary>
        /// Gets or sets the refresh condition setup.
        /// </summary>
        /// <value>The refresh condition setup.</value>
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

        /// <summary>
        /// The refresh condition item
        /// </summary>
        private TextComboBoxItem _refreshConditionItem;

        /// <summary>
        /// Gets or sets the refresh condition item.
        /// </summary>
        /// <value>The refresh condition item.</value>
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

        /// <summary>
        /// Gets or sets the refresh condition.
        /// </summary>
        /// <value>The refresh condition.</value>
        public Conditions RefreshCondition
        {
            get => (Conditions)RefreshConditionItem.NumericValue;
            set => RefreshConditionItem = RefreshConditionSetup.GetItem((int)value);
        }

        /// <summary>
        /// The yellow alert
        /// </summary>
        private int _yellowAlert;

        /// <summary>
        /// Gets or sets the yellow alert.
        /// </summary>
        /// <value>The yellow alert.</value>
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

        /// <summary>
        /// The red alert
        /// </summary>
        private int _redAlert;

        /// <summary>
        /// Gets or sets the red alert.
        /// </summary>
        /// <value>The red alert.</value>
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

        /// <summary>
        /// The disable
        /// </summary>
        private bool _disable;

        /// <summary>
        /// Gets or sets a value indicating whether this <see cref="AdvancedFindRefreshViewModel"/> is disable.
        /// </summary>
        /// <value><c>true</c> if disable; otherwise, <c>false</c>.</value>
        public bool Disable
        {
            get => _disable;
            set
            {
                if (_disable == value)
                {
                    return;
                }
                _disable = value;
                OnPropertyChanged();
            }
        }


        /// <summary>
        /// Gets or sets the properties.
        /// </summary>
        /// <value>The properties.</value>
        public AdvancedFind Properties { get; set; }

        /// <summary>
        /// Initializes the specified advanced find model.
        /// </summary>
        /// <param name="advancedFindModel">The advanced find model.</param>
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

            if (advancedFindModel.Disabled.HasValue)
            {
                Disable = advancedFindModel.Disabled.Value;
            }
            
        }

        /// <summary>
        /// Refreshes the properties.
        /// </summary>
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
            Properties.Disabled = Disable;
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
    }
}
