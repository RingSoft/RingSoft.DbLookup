// ***********************************************************************
// Assembly         : RingSoft.DbLookup.Controls.WPF
// Author           : petem
// Created          : 12-19-2022
//
// Last Modified By : petem
// Last Modified On : 03-04-2023
// ***********************************************************************
// <copyright file="LookupSearchForDecimalHost.cs" company="Peter Ringering">
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
    /// Class LookupSearchForDecimalHost.
    /// Implements the <see cref="RingSoft.DbLookup.Controls.WPF.LookupSearchForDropDownHost{RingSoft.DataEntryControls.WPF.DecimalEditControl}" />
    /// </summary>
    /// <seealso cref="RingSoft.DbLookup.Controls.WPF.LookupSearchForDropDownHost{RingSoft.DataEntryControls.WPF.DecimalEditControl}" />
    public class LookupSearchForDecimalHost : LookupSearchForDropDownHost<DecimalEditControl>
    {
        /// <summary>
        /// Gets or sets the search text.
        /// </summary>
        /// <value>The search text.</value>
        public override string SearchText
        {
            get => Control.Value.ToString();
            set => Control.Value = value.ToDecimal(Control.Culture);
        }

        /// <summary>
        /// Gets or sets the default width.
        /// </summary>
        /// <value>The default width.</value>
        protected virtual double? DefaultWidth { get; set; } = 140;

        /// <summary>
        /// Constructs the control.
        /// </summary>
        /// <returns>DecimalEditControl.</returns>
        protected override DecimalEditControl ConstructControl()
        {
            return new DecimalEditControl();
        }


        /// <summary>
        /// Initializes the specified control.
        /// </summary>
        /// <param name="control">The control.</param>
        /// <param name="columnDefinition">The column definition.</param>
        /// <exception cref="System.ArgumentOutOfRangeException"></exception>
        protected override void Initialize(DecimalEditControl control, LookupColumnDefinitionBase columnDefinition)
        {
            switch (columnDefinition.ColumnType)
            {
                case LookupColumnTypes.Field:
                    if (columnDefinition is LookupFieldColumnDefinition fieldColumnDefinition)
                    {
                        if (fieldColumnDefinition.FieldDefinition is DecimalFieldDefinition decimalFieldDefinition)
                        {
                            control.CultureId = decimalFieldDefinition.Culture.Name;
                            control.FormatType = decimalFieldDefinition.DecimalFieldType
                                .ConvertDecimalFieldTypeToDecimalEditFormatType();
                            control.Precision = decimalFieldDefinition.DecimalCount;
                            control.NumberFormatString = decimalFieldDefinition.NumberFormatString;
                        }
                    }
                    break;
                case LookupColumnTypes.Formula:
                    if (columnDefinition is LookupFormulaColumnDefinition formulaColumnDefinition)
                    {
                        control.CultureId = formulaColumnDefinition.ColumnCulture.Name;
                        control.FormatType = formulaColumnDefinition.DecimalFieldType
                            .ConvertDecimalFieldTypeToDecimalEditFormatType();
                        control.Precision = formulaColumnDefinition.DecimalCount;
                        control.NumberFormatString = formulaColumnDefinition.NumberFormatString;
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
        protected override void Initialize(DecimalEditControl control)
        {
            control.AllowNullValue = true;
            control.ValueChanged += (sender, args) => OnTextChanged();
            control.TextAlignment = TextAlignment.Left;

            if (DefaultWidth != null)
            {
                Control.HorizontalAlignment = HorizontalAlignment.Left;
                Control.Width = (double)DefaultWidth;
            }
        }
    }
}
