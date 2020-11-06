using RingSoft.DataEntryControls.Engine;
using RingSoft.DataEntryControls.WPF;
using RingSoft.DbLookup.Lookup;
using RingSoft.DbLookup.ModelDefinition.FieldDefinitions;
using System;
using System.Windows.Input;

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

        private DateTime? _currentValue;

        protected override DateEditControl ConstructControl()
        {
            return new DateEditControl();
        }

        protected override void Initialize(DateEditControl control, LookupColumnDefinitionBase columnDefinition)
        {
            Control.AllowNullValue = true;
            Control.PlayValidationSoundOnLostFocus = false;

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
                        }
                    }
                    break;
                case LookupColumnTypes.Formula:
                    if (columnDefinition is LookupFormulaColumnDefinition formulaColumnDefinition)
                    {
                        control.CultureId = formulaColumnDefinition.ColumnCulture.Name;
                        control.DisplayFormat = formulaColumnDefinition.DateFormatString;
                    }
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }

            Control.ValueChanged += (sender, args) =>
            {
                if (Control.Value != _currentValue)
                {
                    OnTextChanged();
                    _currentValue = Control.Value;
                }
            };
        }

        public override bool CanProcessSearchForKey(Key key)
        {
            if (Control.IsPopupOpen())
            {
                switch (key)
                {
                    case Key.Left:
                    case Key.Right:
                    case Key.Up:
                    case Key.Down:
                        return false;
                }
            }

            return base.CanProcessSearchForKey(key);
        }
    }
}
