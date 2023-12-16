// ***********************************************************************
// Assembly         : RingSoft.DbLookup.Controls.WPF
// Author           : petem
// Created          : 12-19-2022
//
// Last Modified By : petem
// Last Modified On : 03-04-2023
// ***********************************************************************
// <copyright file="LookupSearchForDateHost.cs" company="Peter Ringering">
//     Copyright (c) 2023. All rights reserved.
// </copyright>
// <summary></summary>
// ***********************************************************************
using RingSoft.DataEntryControls.Engine;
using RingSoft.DataEntryControls.WPF;
using RingSoft.DbLookup.Lookup;
using RingSoft.DbLookup.ModelDefinition.FieldDefinitions;
using System;
using System.Windows;

// ReSharper disable once CheckNamespace
namespace RingSoft.DbLookup.Controls.WPF
{
    /// <summary>
    /// Class LookupSearchForDateHost.
    /// Implements the <see cref="RingSoft.DbLookup.Controls.WPF.LookupSearchForDropDownHost{RingSoft.DataEntryControls.WPF.DateEditControl}" />
    /// </summary>
    /// <seealso cref="RingSoft.DbLookup.Controls.WPF.LookupSearchForDropDownHost{RingSoft.DataEntryControls.WPF.DateEditControl}" />
    public class LookupSearchForDateHost : LookupSearchForDropDownHost<DateEditControl>
    {
        /// <summary>
        /// Gets or sets the search text.
        /// </summary>
        /// <value>The search text.</value>
        public override string SearchText
        {
            get
            {
                var result = string.Empty;
                if (Control.Value != null)
                {
                    var dateValue = (DateTime) Control.Value;
                    if (ConvertToLocalTime)
                    {
                        dateValue = dateValue.ToUniversalTime();
                    }
                    result = dateValue.ToString(Control.Culture);
                }

                return result;
            }
            set
            {
                if (value.IsNullOrEmpty())
                    Control.Value = null;
                else
                {
                    Control.Value = DateTime.Parse(value, Control.Culture);
                }
            }
        }

        /// <summary>
        /// Gets or sets the default width of the date only.
        /// </summary>
        /// <value>The default width of the date only.</value>
        protected virtual double? DefaultDateOnlyWidth { get; set; } = 100;

        /// <summary>
        /// Gets or sets the default width of the date time.
        /// </summary>
        /// <value>The default width of the date time.</value>
        protected virtual double? DefaultDateTimeWidth { get; set; } = 175;

        /// <summary>
        /// Gets a value indicating whether [convert to local time].
        /// </summary>
        /// <value><c>true</c> if [convert to local time]; otherwise, <c>false</c>.</value>
        public bool ConvertToLocalTime { get; private set; }

        /// <summary>
        /// The current value
        /// </summary>
        private DateTime? _currentValue;

        /// <summary>
        /// Constructs the control.
        /// </summary>
        /// <returns>DateEditControl.</returns>
        protected override DateEditControl ConstructControl()
        {
            return new DateEditControl();
        }

        /// <summary>
        /// Initializes the specified control.
        /// </summary>
        /// <param name="control">The control.</param>
        /// <param name="columnDefinition">The column definition.</param>
        /// <exception cref="System.ArgumentOutOfRangeException"></exception>
        protected override void Initialize(DateEditControl control, LookupColumnDefinitionBase columnDefinition)
        {
            switch (columnDefinition.ColumnType)
            {
                case LookupColumnTypes.Field:
                    if (columnDefinition is LookupFieldColumnDefinition fieldColumnDefinition)
                    {
                        if (fieldColumnDefinition.FieldDefinition is DateFieldDefinition dateFieldDefinition)
                        {
                            Control.DateFormatType = dateFieldDefinition.DateType.ConvertDbDateTypeToDateFormatType();
                            Control.CultureId = dateFieldDefinition.Culture.Name;
                            Control.DisplayFormat = dateFieldDefinition.DateFormatString;
                            ConvertToLocalTime = dateFieldDefinition.ConvertToLocalTime;
                        }
                    }
                    break;
                case LookupColumnTypes.Formula:
                    if (columnDefinition is LookupFormulaColumnDefinition formulaColumnDefinition)
                    {
                        control.DateFormatType =
                            formulaColumnDefinition.DateType.ConvertDbDateTypeToDateFormatType();
                        control.CultureId = formulaColumnDefinition.ColumnCulture.Name;
                        control.DisplayFormat = formulaColumnDefinition.DateFormatString;
                        ConvertToLocalTime = formulaColumnDefinition.ConvertToLocalTime;
                    }
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }

        }

        /// <summary>
        /// Initializes the specified control.
        /// </summary>
        /// <param name="control">The control.</param>
        /// <exception cref="System.ArgumentOutOfRangeException"></exception>
        protected override void Initialize(DateEditControl control)
        {
            Control.AllowNullValue = true;
            Control.PlayValidationSoundOnLostFocus = false;

            Control.ValueChanged += (sender, args) =>
            {
                if (Control.Value != _currentValue)
                {
                    OnTextChanged();
                    _currentValue = Control.Value;
                }
            };

            switch (Control.DateFormatType)
            {
                case DateFormatTypes.DateOnly:
                    Control.EntryFormat = "d";
                    if (DefaultDateOnlyWidth != null)
                    {
                        Control.HorizontalAlignment = HorizontalAlignment.Left;
                        Control.Width = (double)DefaultDateOnlyWidth;
                    }

                    break;
                case DateFormatTypes.DateTime:
                    Control.EntryFormat = "G";
                    if (DefaultDateTimeWidth != null)
                    {
                        Control.HorizontalAlignment = HorizontalAlignment.Left;
                        Control.Width = (double)DefaultDateTimeWidth;
                    }

                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }
    }
}
