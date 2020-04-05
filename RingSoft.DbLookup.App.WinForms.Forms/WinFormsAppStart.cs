using RingSoft.DbLookup.App.Library;
using RingSoft.DbLookup.App.Library.Ef6;
using RingSoft.DbLookup.App.Library.EfCore;
using RingSoft.DbLookup.Controls.WinForms;
using RingSoft.DbLookup.Lookup;
using System;
using System.Windows.Forms;
using RingSoft.DbLookup.App.Library.EfCore.DevLogix;
using RingSoft.DbLookup.App.WinForms.Forms.DevLogix;
using RingSoft.DbLookup.App.WinForms.Forms.MegaDb;
using RingSoft.DbLookup.App.WinForms.Forms.Northwind;

namespace RingSoft.DbLookup.App.WinForms.Forms
{
    public class WinFormsAppStart : AppStart
    {
        public static DevLogixLookupContextEfCore DevLogixLookupContext => _devLogixLookupContext;

        private static DevLogixLookupContextEfCore _devLogixLookupContext;

        public override IAppSplashWindow AppSplashWindow => _splashForm;

        private MainForm _mainForm;
        private AppSplashForm _splashForm;

        public override void StartApp(string[] args)
        {
            ControlsGlobals.InitUi();
            _mainForm = new MainForm();

            base.StartApp(args);
        }

        protected override void InitializeSplash()
        {
            _splashForm = new AppSplashForm();

            _mainForm.Done += (sender, eventArgs) =>
            {
                OnMainWindowShown();
                _splashForm = null;
            };
        }

        protected override void ShowSplash()
        {
            Application.Run(_splashForm);
        }

        protected override void ShowDevLogix()
        {
            _devLogixLookupContext = new DevLogixLookupContextEfCore();
            //_devLogixLookupContext.DataProcessorType = DataProcessorTypes.MySql;
            //_devLogixLookupContext.DevLogixConfiguration.ConfigureLookups();
            Application.Run(new DevLogixTestForm());
        }

        protected override void FinishStartup()
        {
            ChangeEntityFrameworkVersion(RegistrySettings.GetEntityFrameworkVersion());

            RsDbLookupAppGlobals.EfProcessor.NorthwindLookupContext.LookupAddView += NorthwindLookupContext_LookupView;
            RsDbLookupAppGlobals.EfProcessor.MegaDbLookupContext.LookupAddView += MegaDbLookupContextOnLookupView;
            Application.Run(_mainForm);
        }

        private void ChangeEntityFrameworkVersion(EntityFrameworkVersions newVersion)
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

        private void MegaDbLookupContextOnLookupView(object sender, LookupAddViewArgs e)
        {
            if (e.LookupData.LookupDefinition.TableDefinition == RsDbLookupAppGlobals.EfProcessor.MegaDbLookupContext.Items)
            {
                var itemForm = new ItemForm();
                itemForm.InitializeFromLookupData(e);
                itemForm.ShowDialog();
            }
            else if (e.LookupData.LookupDefinition.TableDefinition == RsDbLookupAppGlobals.EfProcessor.MegaDbLookupContext.Locations)
            {
                var locationsForm = new LocationForm();
                locationsForm.InitializeFromLookupData(e);
                locationsForm.ShowDialog();
            }
            else if (e.LookupData.LookupDefinition.TableDefinition == RsDbLookupAppGlobals.EfProcessor.MegaDbLookupContext.Manufacturers)
            {
                var manufacturersForm = new ManufacturerForm();
                manufacturersForm.InitializeFromLookupData(e);
                manufacturersForm.ShowDialog();
            }
            else if (e.LookupData.LookupDefinition.TableDefinition ==
                     RsDbLookupAppGlobals.EfProcessor.MegaDbLookupContext.Stocks)
            {
                var stockMasterForm = new StockMasterForm();
                stockMasterForm.InitializeFromLookupData(e);
                stockMasterForm.ShowDialog();
            }
            else if (e.LookupData.LookupDefinition.TableDefinition ==
                     RsDbLookupAppGlobals.EfProcessor.MegaDbLookupContext.StockCostQuantities)
            {
                var stockCostQuantityForm = new StockCostQuantityForm();
                stockCostQuantityForm.InitializeFromLookupData(e);
                stockCostQuantityForm.ShowDialog();
            }
        }

        private void NorthwindLookupContext_LookupView(object sender, LookupAddViewArgs e)
        {
            if (e.LookupData.LookupDefinition.TableDefinition == RsDbLookupAppGlobals.EfProcessor.NorthwindLookupContext.Customers)
            {
                var customerForm = new CustomerForm();
                customerForm.InitializeFromLookupData(e);
                customerForm.ShowDialog();
            }
            else if (e.LookupData.LookupDefinition.TableDefinition == RsDbLookupAppGlobals.EfProcessor.NorthwindLookupContext.Orders)
            {
                var orderForm = new OrdersForm();
                orderForm.InitializeFromLookupData(e);
                orderForm.ShowDialog();
            }
            else if (e.LookupData.LookupDefinition.TableDefinition == RsDbLookupAppGlobals.EfProcessor.NorthwindLookupContext.Employees)
            {
                var employeeForm = new EmployeeForm();
                employeeForm.InitializeFromLookupData(e);
                employeeForm.ShowDialog();
            }
            else if (e.LookupData.LookupDefinition.TableDefinition ==
                     RsDbLookupAppGlobals.EfProcessor.NorthwindLookupContext.OrderDetails)
            {
                var orderDetailsForm = new OrderDetailsForm();
                orderDetailsForm.InitializeFromLookupData(e);
                orderDetailsForm.ShowDialog();
            }
        }
    }
}
