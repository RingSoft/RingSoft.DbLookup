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
            switch (columnDefinition.ColumnType)
            {
                case LookupColumnTypes.Field:
                    if (columnDefinition is LookupFieldColumnDefinition fieldColumnDefinition)
                    {
                        if (fieldColumnDefinition.FieldDefinition is IntegerFieldDefinition integerFieldDefinition)
                        {
                            control.CultureId = integerFieldDefinition.Culture.Name;
                            control.NumberFormatString = integerFieldDefinition.NumberFormatString;
                            if (integerFieldDefinition.TableDefinition.PrimaryKeyFields.Contains(integerFieldDefinition)
                            )
                                control.DataEntryMode = DataEntryModes.ValidateOnly;
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

        }

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
