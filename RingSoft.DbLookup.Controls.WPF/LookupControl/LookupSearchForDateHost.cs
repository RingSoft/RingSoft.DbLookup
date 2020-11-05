using System;
using RingSoft.DataEntryControls.Engine;
using RingSoft.DataEntryControls.WPF;
using RingSoft.DbLookup.Lookup;
using RingSoft.DbLookup.ModelDefinition.FieldDefinitions;
using RingSoft.DbLookup.QueryBuilder;

// ReSharper disable once CheckNamespace
namespace RingSoft.DbLookup.Controls.WPF
{
    class LookupSearchForDateHost : LookupSearchForHost<DateEditControl>
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

        public override void SelectAll()
        {
            Control.TextBox.SelectAll();
        }

        protected override DateEditControl ConstructControl()
        {
            return new DateEditControl();
        }

        protected override void Initialize(DateEditControl control, LookupColumnDefinitionBase columnDefinition)
        {
            Control.AllowNullValue = true;

            if (columnDefinition is LookupFieldColumnDefinition fieldColumnDefinition)
            {
                if (fieldColumnDefinition.FieldDefinition is DateFieldDefinition dateFieldDefinition)
                {
                    Control.DateFormatType = dateFieldDefinition.DateType.ConvertDbDateTypeToDateFormatType();
                    Control.CultureId = dateFieldDefinition.Culture.Name;
                    Control.DisplayFormat = dateFieldDefinition.DateFormatString;
                }
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

        public override void SetFocusToControl()
        {
            Control.Focus();
            base.SetFocusToControl();
        }
    }
}
