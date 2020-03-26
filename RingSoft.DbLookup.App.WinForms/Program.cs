using System;
using System.Globalization;
using System.Linq;
using System.Threading;
using System.Windows.Forms;
using RingSoft.DbLookup.App.Library;
using RingSoft.DbLookup.App.Library.Ef6;
using RingSoft.DbLookup.App.Library.EfCore;
using RingSoft.DbLookup.App.Library.EfCore.DevLogix;
using RingSoft.DbLookup.GetDataProcessor;
using RingSoft.DbLookup.Lookup;
using RingSoft.DbLookup.QueryBuilder;

namespace RingSoft.DbLookup.App.WinForms
{
    static class Program
    {
        public static bool ShowDevLogix { get; private set; }
        public static DevLogixLookupContextEfCore DevLogixLookupContext => _devLogixLookupContext;

        private static DevLogixLookupContextEfCore _devLogixLookupContext;

        private static MainForm _mainForm;
        private static AppSplashForm _splashForm;

        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main(string[] args)
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);

            if (args.Contains("-devLogix"))
                ShowDevLogix = true;

            _mainForm = new MainForm();
            DbDataProcessor.SqlErrorViewer = _mainForm;

            if (ShowDevLogix)
            {
                _devLogixLookupContext = new DevLogixLookupContextEfCore();
                //_devLogixLookupContext.DataProcessorType = DataProcessorTypes.MySql;
                //_devLogixLookupContext.DevLogixConfiguration.ConfigureLookups();

                //Application.Run(new DevLogixTestForm());
            }
            else
            {
                _mainForm.Load += (sender, eventArgs) =>
                {
                    if (_splashForm != null && !_splashForm.Disposing && !_splashForm.IsDisposed)
                        _splashForm.Invoke(new Action(() => _splashForm.Close()));
                    _mainForm.TopMost = true;
                    _mainForm.Activate();
                    _mainForm.TopMost = false;
                };

                _splashForm = new AppSplashForm();
                var splashThread = new Thread(new ThreadStart(
                    () => Application.Run(_splashForm)));
                splashThread.SetApartmentState(ApartmentState.STA);
                splashThread.Start();

                RsDbLookupAppGlobals.AppStartProgress += (sender, progressArgs) =>
                {
                    _splashForm.SetProgress(progressArgs.ProgressText);
                };

                ChangeEntityFrameworkVersion(RegistrySettings.GetEntityFrameworkVersion());

                RsDbLookupAppGlobals.EfProcessor.NorthwindLookupContext.LookupAddView += NorthwindLookupContext_LookupView;
                RsDbLookupAppGlobals.EfProcessor.MegaDbLookupContext.LookupAddView += MegaDbLookupContextOnLookupView;
                Application.Run(_mainForm);
            }
        }

        public static void ChangeEntityFrameworkVersion(EntityFrameworkVersions newVersion)
        {
            switch (newVersion)
            {
                case EntityFrameworkVersions.EntityFrameworkCore3:
                    RsDbLookupAppGlobals.EfProcessor = new EfProcessorCore();
                    break;
                case EntityFrameworkVersions.EntityFramework6:
                    RsDbLookupAppGlobals.EfProcessor = new EfProcessor6();
                    break;
                default:
                    throw new ArgumentOutOfRangeException(nameof(newVersion), newVersion, null);
            }
        }

        private static void MegaDbLookupContextOnLookupView(object sender, LookupAddViewArgs e)
        {
            //if (e.LookupData.LookupDefinition.TableDefinition == RsDbLookupAppGlobals.EfProcessor.MegaDbLookupContext.Items)
            //{
            //    var itemForm = new ItemForm();
            //    itemForm.InitializeFromLookupData(e);
            //    itemForm.ShowDialog();
            //}
            //else if (e.LookupData.LookupDefinition.TableDefinition == RsDbLookupAppGlobals.EfProcessor.MegaDbLookupContext.Locations)
            //{
            //    var locationsForm = new LocationForm();
            //    locationsForm.InitializeFromLookupData(e);
            //    locationsForm.ShowDialog();
            //}
            //else if (e.LookupData.LookupDefinition.TableDefinition == RsDbLookupAppGlobals.EfProcessor.MegaDbLookupContext.Manufacturers)
            //{
            //    var manufacturersForm = new ManufacturerForm();
            //    manufacturersForm.InitializeFromLookupData(e);
            //    manufacturersForm.ShowDialog();
            //}
            //else if (e.LookupData.LookupDefinition.TableDefinition ==
            //         RsDbLookupAppGlobals.EfProcessor.MegaDbLookupContext.Stocks)
            //{
            //    var stockMasterForm = new StockMasterForm();
            //    stockMasterForm.InitializeFromLookupData(e);
            //    stockMasterForm.ShowDialog();
            //}
            //else if (e.LookupData.LookupDefinition.TableDefinition ==
            //         RsDbLookupAppGlobals.EfProcessor.MegaDbLookupContext.StockCostQuantities)
            //{
            //    var stockCostQuantityForm = new StockCostQuantityForm();
            //    stockCostQuantityForm.InitializeFromLookupData(e);
            //    stockCostQuantityForm.ShowDialog();
            //}
        }

        private static void NorthwindLookupContext_LookupView(object sender, LookupAddViewArgs e)
        {
            //if (e.LookupData.LookupDefinition.TableDefinition == RsDbLookupAppGlobals.EfProcessor.NorthwindLookupContext.Customers)
            //{
            //    var customerForm = new CustomerForm();
            //    customerForm.InitializeFromLookupData(e);
            //    customerForm.ShowDialog();
            //}
            //else if (e.LookupData.LookupDefinition.TableDefinition == RsDbLookupAppGlobals.EfProcessor.NorthwindLookupContext.Orders)
            //{
            //    var orderForm = new OrdersForm();
            //    orderForm.InitializeFromLookupData(e);
            //    orderForm.ShowDialog();
            //}
            //else if (e.LookupData.LookupDefinition.TableDefinition == RsDbLookupAppGlobals.EfProcessor.NorthwindLookupContext.Employees)
            //{
            //    var employeeForm = new EmployeeForm();
            //    employeeForm.InitializeFromLookupData(e);
            //    employeeForm.ShowDialog();
            //}
            //else if (e.LookupData.LookupDefinition.TableDefinition ==
            //         RsDbLookupAppGlobals.EfProcessor.NorthwindLookupContext.OrderDetails)
            //{
            //    var orderDetailsForm = new OrderDetailsForm();
            //    orderDetailsForm.InitializeFromLookupData(e);
            //    orderDetailsForm.ShowDialog();
            //}
        }

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
