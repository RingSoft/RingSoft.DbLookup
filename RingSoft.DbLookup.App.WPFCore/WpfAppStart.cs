using System;
using System.Windows;
using System.Windows.Input;
using RingSoft.DbLookup.App.Library;
using RingSoft.DbLookup.App.Library.Ef6;
using RingSoft.DbLookup.App.Library.EfCore;
using RingSoft.DbLookup.App.WPFCore.Northwind;
using RingSoft.DbLookup.Controls.WPF;
using RingSoft.DbLookup.GetDataProcessor;
using RingSoft.DbLookup.Lookup;

namespace RingSoft.DbLookup.App.WPFCore
{
    public class WpfAppStart : AppStart
    {
        public override IAppSplashWindow AppSplashWindow => _splashWindow;


        private Application _application;
        private MainWindow _mainWindow;
        private AppSplashWindow _splashWindow;

        public WpfAppStart(Application application)
        {
            _application = application;
        }

        public override void StartApp(string[] args)
        {
            ControlsGlobals.InitUi();
            _mainWindow = new MainWindow();
            base.StartApp(args);
        }

        protected override void InitializeSplash()
        {
            _mainWindow.Done += (sender, args) =>
            {
                OnMainWindowShown();
            };
        }

        protected override void ShowSplash()
        {
            _splashWindow = new AppSplashWindow();
            _splashWindow.ShowDialog();
        }

        protected override void ShowDevLogix()
        {
            throw new NotImplementedException();
        }

        protected override void FinishStartup()
        {
            ChangeEntityFrameworkVersion(RegistrySettings.GetEntityFrameworkVersion());

            RsDbLookupAppGlobals.EfProcessor.NorthwindLookupContext.LookupAddView += NorthwindLookupContext_LookupView;
            RsDbLookupAppGlobals.EfProcessor.MegaDbLookupContext.LookupAddView += MegaDbLookupContextOnLookupView;

            _application.MainWindow = _mainWindow;
            _mainWindow.Show();
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

        private void NorthwindLookupContext_LookupView(object sender, LookupAddViewArgs e)
        {
            if (e.LookupData.LookupDefinition.TableDefinition == RsDbLookupAppGlobals.EfProcessor.NorthwindLookupContext.Customers)
            {
                var customersWindow = new CustomersWindow();
                customersWindow.InitializeFromLookupData(e);
                customersWindow.ShowDialog();
            }
            else if (e.LookupData.LookupDefinition.TableDefinition == RsDbLookupAppGlobals.EfProcessor.NorthwindLookupContext.Orders)
            {
                var ordersWindow = new OrdersWindow();
                ordersWindow.InitializeFromLookupData(e);
                ordersWindow.ShowDialog();
            }
            else if (e.LookupData.LookupDefinition.TableDefinition == RsDbLookupAppGlobals.EfProcessor.NorthwindLookupContext.Employees)
            {
                var employeesWindow = new EmployeesWindow();
                employeesWindow.InitializeFromLookupData(e);
                employeesWindow.ShowDialog();
            }
            else if (e.LookupData.LookupDefinition.TableDefinition ==
                     RsDbLookupAppGlobals.EfProcessor.NorthwindLookupContext.OrderDetails)
            {
                var orderDetailsWindow = new OrderDetailsWindow();
                orderDetailsWindow.InitializeFromLookupData(e);
                orderDetailsWindow.ShowDialog();
            }
        }
    }
}
