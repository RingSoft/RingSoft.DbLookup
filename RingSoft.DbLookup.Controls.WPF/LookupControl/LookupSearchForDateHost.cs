using RingSoft.DataEntryControls.Engine;
using RingSoft.DataEntryControls.WPF;
using RingSoft.DbLookup.Lookup;
using RingSoft.DbLookup.ModelDefinition.FieldDefinitions;
using System;
using System.Windows;

// ReSharper disable once CheckNamespace
namespace RingSoft.DbLookup.Controls.WPF
{
    public class LookupSearchForDateHost : LookupSearchForDropDownHost<DateEditControl>
    {
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

        protected virtual double? DefaultDateOnlyWidth { get; set; } = 100;

        protected virtual double? DefaultDateTimeWidth { get; set; } = 175;

        public bool ConvertToLocalTime { get; private set; }

        private DateTime? _currentValue;

        protected override DateEditControl ConstructControl()
        {
            return new DateEditControl();
        }

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
