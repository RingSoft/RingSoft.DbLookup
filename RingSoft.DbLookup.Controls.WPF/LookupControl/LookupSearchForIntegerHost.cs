using System;
using System.Windows;
using RingSoft.DataEntryControls.Engine;
using RingSoft.DataEntryControls.WPF;
using RingSoft.DbLookup.Lookup;
using RingSoft.DbLookup.ModelDefinition.FieldDefinitions;

// ReSharper disable once CheckNamespace
namespace RingSoft.DbLookup.Controls.WPF
{
    public class LookupSearchForIntegerHost : LookupSearchForDropDownHost<IntegerEditControl>
    {
        public override string SearchText
        {
            get => Control.Value.ToString();
            set => Control.Value = value.ToInt(Control.Culture);
        }

        protected virtual double? DefaultWidth { get; set; } = 100;

        protected override IntegerEditControl ConstructControl()
        {
            return new IntegerEditControl();
        }

        protected override void Initialize(IntegerEditControl control, LookupColumnDefinitionBase columnDefinition)
        {
            control.AllowNullValue = true;
            control.ValueChanged += (sender, args) => OnTextChanged();
            control.TextAlignment = TextAlignment.Left;

            switch (columnDefinition.ColumnType)
            {
                case LookupColumnTypes.Field:
                    if (columnDefinition is LookupFieldColumnDefinition fieldColumnDefinition)
                    {
                        if (fieldColumnDefinition.FieldDefinition is IntegerFieldDefinition integerFieldDefinition)
                        {
                            control.CultureId = integerFieldDefinition.Culture.Name;
                            control.NumberFormatString = integerFieldDefinition.NumberFormatString;
                        }
                    }
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

            if (DefaultWidth != null)
            {
                Control.HorizontalAlignment = HorizontalAlignment.Left;
                Control.Width = (double)DefaultWidth;
            }
        }
    }
}
