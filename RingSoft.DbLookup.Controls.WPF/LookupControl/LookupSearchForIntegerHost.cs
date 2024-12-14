// ***********************************************************************
// Assembly         : RingSoft.DbLookup.Controls.WPF
// Author           : petem
// Created          : 12-19-2022
//
// Last Modified By : petem
// Last Modified On : 03-04-2023
// ***********************************************************************
// <copyright file="LookupSearchForIntegerHost.cs" company="Peter Ringering">
//     Copyright (c) 2023. All rights reserved.
// </copyright>
// <summary></summary>
// ***********************************************************************
using System;
using System.Linq;
using System.Windows;
using RingSoft.DataEntryControls.Engine;
using RingSoft.DataEntryControls.WPF;
using RingSoft.DbLookup.Lookup;
using RingSoft.DbLookup.ModelDefinition.FieldDefinitions;

// ReSharper disable once CheckNamespace
namespace RingSoft.DbLookup.Controls.WPF
{
    /// <summary>
    /// Class LookupSearchForIntegerHost.
    /// Implements the <see cref="RingSoft.DbLookup.Controls.WPF.LookupSearchForDropDownHost{RingSoft.DataEntryControls.WPF.IntegerEditControl}" />
    /// </summary>
    /// <seealso cref="RingSoft.DbLookup.Controls.WPF.LookupSearchForDropDownHost{RingSoft.DataEntryControls.WPF.IntegerEditControl}" />
    public class LookupSearchForIntegerHost : LookupSearchForDropDownHost<IntegerEditControl>
    {
        /// <summary>
        /// Gets or sets the search text.
        /// </summary>
        /// <value>The search text.</value>
        public override string SearchText
        {
            get => Control.Value.ToString();
            set => Control.Value = value.ToInt(Control.Culture);
        }

        internal override void Initialize(FieldDefinition fieldDefinition)
        {
            if (fieldDefinition is IntegerFieldDefinition integerFieldDefinition)
            {
                Control.CultureId = integerFieldDefinition.Culture.Name;
                Control.NumberFormatString = integerFieldDefinition.NumberFormatString;
                if (integerFieldDefinition.TableDefinition.PrimaryKeyFields.Contains(integerFieldDefinition)
                )
                    Control.DataEntryMode = DataEntryModes.ValidateOnly;
            }

        }

        /// <summary>
        /// Gets or sets the default width.
        /// </summary>
        /// <value>The default width.</value>
        protected virtual double? DefaultWidth { get; set; } = 100;

        /// <summary>
        /// Constructs the control.
        /// </summary>
        /// <returns>TControl.</returns>
        protected override IntegerEditControl ConstructControl()
        {
            return new IntegerEditControl();
        }

        /// <summary>
        /// Initializes the specified control.
        /// </summary>
        /// <param name="control">The control.</param>
        /// <param name="columnDefinition">The column definition.</param>
        /// <exception cref="System.ArgumentOutOfRangeException"></exception>
        protected override void Initialize(IntegerEditControl control, LookupColumnDefinitionBase columnDefinition)
        {
            switch (columnDefinition.ColumnType)
            {
                case LookupColumnTypes.Field:
                    //if (columnDefinition is LookupFieldColumnDefinition fieldColumnDefinition)
                    //{
                    //    if (fieldColumnDefinition.FieldDefinition is IntegerFieldDefinition integerFieldDefinition)
                    //    {
                    //        control.CultureId = integerFieldDefinition.Culture.Name;
                    //        control.NumberFormatString = integerFieldDefinition.NumberFormatString;
                    //        if (integerFieldDefinition.TableDefinition.PrimaryKeyFields.Contains(integerFieldDefinition)
                    //        )
                    //            control.DataEntryMode = DataEntryModes.ValidateOnly;
                    //    }
                    //}
                    break;
                case LookupColumnTypes.Formula:
                    if (columnDefinition is LookupFormulaColumnDefinition formulaColumnDefinition)
                    {
                        control.CultureId = formulaColumnDefinition.ColumnCulture.Name;
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
        protected override void Initialize(IntegerEditControl control)
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
