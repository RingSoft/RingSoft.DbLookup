using RingSoft.DbLookup.App.Library;
using RingSoft.DbLookup.App.Library.Ef6;
using RingSoft.DbLookup.App.Library.EfCore;
using RingSoft.DbLookup.App.Library.EfCore.DevLogix;
using RingSoft.DbLookup.App.WPFCore.DevLogix;
using RingSoft.DbLookup.App.WPFCore.MegaDb;
using RingSoft.DbLookup.App.WPFCore.Northwind;
using RingSoft.DbLookup.Controls.WPF;
using RingSoft.DbLookup.Lookup;
using System;
using System.Windows;

namespace RingSoft.DbLookup.App.WPFCore
{
    public class WpfAppStart : AppStart
    {
        public override IAppSplashWindow AppSplashWindow => _splashWindow;

        private Application _application;
        private MainWindow _mainWindow;
        private DevLogixTestWindow _devLogixTestWindow;
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
            _devLogixTestWindow = new DevLogixTestWindow();

            _application.MainWindow = _devLogixTestWindow;
            _devLogixTestWindow.ShowDialog();
            _application.Shutdown();
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
            if (e.LookupData.LookupDefinition.TableDefinition == RsDbLookupAppGlobals.EfProcessor.MegaDbLookupContext.Items)
            {
                var itemsWindow = new ItemsWindow();
                itemsWindow.InitializeFromLookupData(e);
                itemsWindow.ShowDialog();
            }
            else if (e.LookupData.LookupDefinition.TableDefinition == RsDbLookupAppGlobals.EfProcessor.MegaDbLookupContext.Locations)
            {
                var locationsWindow = new LocationWindow();
                locationsWindow.InitializeFromLookupData(e);
                locationsWindow.ShowDialog();
            }
            else if (e.LookupData.LookupDefinition.TableDefinition == RsDbLookupAppGlobals.EfProcessor.MegaDbLookupContext.Manufacturers)
            {
                var manufacturersWindow = new ManufacturerWindow();
                manufacturersWindow.InitializeFromLookupData(e);
                manufacturersWindow.ShowDialog();
            }
            else if (e.LookupData.LookupDefinition.TableDefinition ==
                     RsDbLookupAppGlobals.EfProcessor.MegaDbLookupContext.Stocks)
            {
                var stockMasterWindow = new StockMasterWindow();
                stockMasterWindow.InitializeFromLookupData(e);
                stockMasterWindow.ShowDialog();
            }
            else if (e.LookupData.LookupDefinition.TableDefinition ==
                     RsDbLookupAppGlobals.EfProcessor.MegaDbLookupContext.StockCostQuantities)
            {
                var stockCostQuantityWindow = new StockCostQuantityWindow();
                stockCostQuantityWindow.InitializeFromLookupData(e);
                stockCostQuantityWindow.ShowDialog();
            }
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
