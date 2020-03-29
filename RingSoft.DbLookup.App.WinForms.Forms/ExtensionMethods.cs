using System;
using System.Globalization;
using System.Windows.Forms;
using RingSoft.DbLookup.QueryBuilder;

namespace RingSoft.DbLookup.App.WinForms.Forms
{
    public static class ExtensionMethods
    {
        public static void AddCheckedBinding<T>(this RadioButton radio, object dataSource, string dataMember, T trueValue)
        {
            var binding = new Binding(nameof(RadioButton.Checked), dataSource, dataMember, true, DataSourceUpdateMode.OnPropertyChanged);
            binding.Parse += (s, a) => { if ((bool)a.Value) a.Value = trueValue; };
            binding.Format += (s, a) => a.Value = ((T)a.Value).Equals(trueValue);
            radio.DataBindings.Add(binding);
        }

        public static void BindControlToDateFormat(this Control control, object dataSource, string dataMember,
            DbDateTypes dateType)
        {
            Binding b = new Binding
            (nameof(control.Text), dataSource, dataMember, true,
                DataSourceUpdateMode.OnPropertyChanged);
            // Add the delegates to the event.
            b.Format += (sender, args) =>
            {
                if (args.DesiredType == typeof(string))
                {
                    if (args.Value is DateTime dateValue)
                        args.Value = dateValue.FormatDateValue(dateType);
                }
            };
            b.Parse += (sender, args) =>
            {
                if (args.DesiredType == typeof(DateTime))
                {
                    args.Value = DateTime.Parse(args.Value.ToString());
                }
            };
            control.DataBindings.Add(b);
        }

        public static void BindTextBoxToIntFormat(this TextBox textBox, object dataSource, string dataMember)
        {
            // Creates the binding first.
            Binding b = new Binding
            (nameof(textBox.Text), dataSource, dataMember, true,
                DataSourceUpdateMode.OnPropertyChanged);
            // Add the delegates to the event.
            b.Format += IntToFormattedString;
            b.Parse += FormattedStringToInt;
            textBox.DataBindings.Add(b);
        }

        private static void IntToFormattedString(object sender, ConvertEventArgs cevent)
        {
            // The method converts only to string type. Test this using the DesiredType.
            if (cevent.DesiredType != typeof(string)) return;

            // Use the ToString method to format the value as number text.
            var intValue = int.Parse(cevent.Value.ToString());
            cevent.Value = intValue.ToString(GblMethods.GetNumFormat(0, false));
        }

        private static void FormattedStringToInt(object sender, ConvertEventArgs cevent)
        {
            // The method converts back to int type only. 
            if (cevent.DesiredType != typeof(int)) return;

            // Converts the string back to int using the static Parse method.
            var text = GblMethods.NumTextToString(cevent.Value.ToString());
            if (int.TryParse(text, out var intResult))
                cevent.Value = intResult;
        }

        public static void BindControlToDecimalFormat(this Control control, object dataSource, string dataMember, int decimalCount = 2, bool isCurrency = true)
        {
            // Creates the binding first.
            Binding b = new Binding
            (nameof(control.Text), dataSource, dataMember, true,
                DataSourceUpdateMode.OnPropertyChanged);
            // Add the delegates to the event.
            b.Format += (sender, args) =>
            {
                // The method converts only to string type. Test this using the DesiredType.
                if (args.DesiredType != typeof(string)) return;

                // Use the ToString method to format the value as number text.
                var textValue = string.Empty;
                if (args.Value is decimal value)
                    textValue = value.ToString(CultureInfo.InvariantCulture);
                else if (args.Value is double doubleValue)
                    textValue = doubleValue.ToString(CultureInfo.InvariantCulture);

                args.Value = GblMethods.FormatValue(FieldDataTypes.Decimal, textValue,
                    GblMethods.GetNumFormat(decimalCount, isCurrency));
            };
            b.Parse += FormattedCurrencyStringToDouble;
            control.DataBindings.Add(b);
        }

        private static void FormattedCurrencyStringToDouble(object sender, ConvertEventArgs cevent)
        {
            if (cevent.DesiredType == typeof(decimal) || cevent.DesiredType == typeof(double))
            {

                // Converts the string back to int using the static Parse method.
                var text = GblMethods.NumTextToString(cevent.Value.ToString());
                if (cevent.DesiredType == typeof(decimal))
                    cevent.Value = decimal.Parse(text);
                else
                    cevent.Value = double.Parse(text);
            }
        }
    }
}
