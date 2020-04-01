using RingSoft.DbLookup.App.Library;
using RingSoft.DbLookup.App.Library.Ef6;
using RingSoft.DbLookup.App.Library.EfCore;
using RingSoft.DbLookup.GetDataProcessor;
using RingSoft.DbLookup.Lookup;
using System;
using System.Windows;
using System.Windows.Input;

namespace RingSoft.DbLookup.App.WPF.Views
{
    public class WpfAppStart : AppStart, IDataProcessResultViewer, IWindowCursor
    {
        public override IAppSplashWindow AppSplashWindow => _splashWindow;


        private Application _application;
        private MainWindow _mainWindow;
        private AppSplashWindow _splashWindow;
        private string _netVersion;

        public WpfAppStart(Application application, string netVersion)
        {
            _application = application;
            _netVersion = netVersion;
        }

        public override void StartApp(string[] args)
        {
            _mainWindow = new MainWindow();
            DbDataProcessor.DataProcessResultViewer = this;
            DbDataProcessor.WindowCursor = this;

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
            _splashWindow = new AppSplashWindow(_netVersion);
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

        public void ShowDataProcessResult(DataProcessResult dataProcessResult)
        {
            MessageBox.Show($"SQL Error\r\n\r\n{dataProcessResult.Message}");
        }

        public void SetWindowCursor(WindowCursorTypes cursor)
        {
            switch (cursor)
            {
                case WindowCursorTypes.Default:
                    Mouse.OverrideCursor = null;
                    break;
                case WindowCursorTypes.Wait:
                    Mouse.OverrideCursor = Cursors.Wait;
                    break;
                default:
                    throw new ArgumentOutOfRangeException(nameof(cursor), cursor, null);
            }
        }
    }
}
