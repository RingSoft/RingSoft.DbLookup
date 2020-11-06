using System;
using System.Windows;
using RingSoft.DataEntryControls.Engine;
using RingSoft.DataEntryControls.WPF;
using RingSoft.DbLookup.Lookup;
using RingSoft.DbLookup.ModelDefinition.FieldDefinitions;

// ReSharper disable once CheckNamespace
namespace RingSoft.DbLookup.Controls.WPF
{
    public class LookupSearchForDecimalHost : LookupSearchForDropDownHost<DecimalEditControl>
    {
        public override string SearchText
        {
            get => Control.Value.ToString();
            set => Control.Value = value.ToDecimal(Control.Culture);
        }
        protected override DecimalEditControl ConstructControl()
        {
            return new DecimalEditControl();
        }

        protected override void Initialize(DecimalEditControl control, LookupColumnDefinitionBase columnDefinition)
        {
            control.AllowNullValue = true;
            control.ValueChanged += (sender, args) => OnTextChanged();
            control.TextAlignment = TextAlignment.Left;

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
    }
}
