// ***********************************************************************
// Assembly         : RingSoft.DbLookup.Controls.WPF
// Author           : petem
// Created          : 07-02-2023
//
// Last Modified By : petem
// Last Modified On : 07-02-2023
// ***********************************************************************
// <copyright file="LookupSearchForEnumHost.cs" company="Peter Ringering">
//     Copyright (c) 2023. All rights reserved.
// </copyright>
// <summary></summary>
// ***********************************************************************
using RingSoft.DataEntryControls.WPF;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using RingSoft.DataEntryControls.Engine;
using RingSoft.DbLookup.Lookup;
using RingSoft.DbLookup.ModelDefinition.FieldDefinitions;
using System.Windows.Controls;

namespace RingSoft.DbLookup.Controls.WPF
{
    /// <summary>
    /// Class LookupSearchForEnumHost.
    /// Implements the <see cref="RingSoft.DbLookup.Controls.WPF.LookupSearchForHost{RingSoft.DataEntryControls.WPF.TextComboBoxControl}" />
    /// </summary>
    /// <seealso cref="RingSoft.DbLookup.Controls.WPF.LookupSearchForHost{RingSoft.DataEntryControls.WPF.TextComboBoxControl}" />
    internal class LookupSearchForEnumHost : LookupSearchForHost<TextComboBoxControl>
    {
        /// <summary>
        /// Gets or sets the search text.
        /// </summary>
        /// <value>The search text.</value>
        public override string SearchText
        {
            get => Control.SelectedItem.NumericValue.ToString();
            set
            {
                var numVal = value.ToInt();
                var item = Setup.Items.FirstOrDefault(
                    p => p.NumericValue == numVal);
                Control.SelectedItem = item;
            } 
        }
        /// <summary>
        /// Gets or sets the setup.
        /// </summary>
        /// <value>The setup.</value>
        public TextComboBoxControlSetup Setup { get; set; }

        /// <summary>
        /// Gets or sets the default width.
        /// </summary>
        /// <value>The default width.</value>
        public double DefaultWidth { get; set; } = 100;

        /// <summary>
        /// Selects all.
        /// </summary>
        public override void SelectAll()
        {
            
        }

        public override void SetValue(string value)
        {
            var valueInt = value.ToInt();
            var item = Setup.Items.FirstOrDefault(
                p => p.NumericValue == valueInt);

            Control.SelectedItem = item;
        }

        /// <summary>
        /// Constructs the control.
        /// </summary>
        /// <returns>TextComboBoxControl.</returns>
        protected override TextComboBoxControl ConstructControl()
        {
            return new TextComboBoxControl();
        }

        /// <summary>
        /// Initializes the specified control.
        /// </summary>
        /// <param name="control">The control.</param>
        /// <param name="columnDefinition">The column definition.</param>
        protected override void Initialize(TextComboBoxControl control, LookupColumnDefinitionBase columnDefinition)
        {
            Setup = new TextComboBoxControlSetup();
            //    if (columnDefinition is LookupFieldColumnDefinition fieldColumn)
            //    {
            //        if (fieldColumn.FieldDefinition is IntegerFieldDefinition integerField)
            //        {
            //            Setup.LoadFromEnum(integerField.EnumTranslation);
            //        }

            //        if (fieldColumn.FieldDefinition is BoolFieldDefinition boolFieldDefinition)
            //        {
            //            Setup.LoadFromEnum(boolFieldDefinition.EnumField);
            //        }
            //    }

            control.SelectionChanged += (sender, args) =>
            {
                OnTextChanged();
            };
            //control.Setup = Setup;
            control.HorizontalAlignment = HorizontalAlignment.Left;
            control.Width = DefaultWidth;
        }

        internal override void Initialize(FieldDefinition fieldDefinition)
        {
            if (Setup == null)
            {
                Setup = new TextComboBoxControlSetup();
            }
            if (fieldDefinition is IntegerFieldDefinition integerField)
            {
                Setup.LoadFromEnum(integerField.EnumTranslation);
            }

            if (fieldDefinition is BoolFieldDefinition boolFieldDefinition)
            {
                Setup.LoadFromEnum(boolFieldDefinition.EnumField);
            }
            Control.Setup = Setup;
        }

        /// <summary>
        /// Initializes the specified control.
        /// </summary>
        /// <param name="control">The control.</param>
        protected override void Initialize(TextComboBoxControl control)
        {
            
        }
    }
}
