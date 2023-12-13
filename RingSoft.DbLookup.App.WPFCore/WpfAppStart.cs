using RingSoft.DataEntryControls.Engine;
using RingSoft.DbLookup.App.Library;
using RingSoft.DbLookup.App.Library.EfCore;
using RingSoft.DbLookup.App.Library.Northwind.ViewModels;
using RingSoft.DbLookup.App.WPFCore.MegaDb;
using RingSoft.DbLookup.App.WPFCore.Northwind;
using RingSoft.DbLookup.Controls.WPF;
using RingSoft.DbLookup.DataProcessor;
using RingSoft.DbLookup.Lookup;
using System;
using System.Media;
using System.Threading.Tasks;
using System.Windows;
using RingSoft.DataEntryControls.WPF;

namespace RingSoft.DbLookup.App.WPFCore
{
    public class WpfAppStart : AppStart, IDbLookupUserInterface, IControlsUserInterface
    {
        public static string ProgramDataFolder
        {
            get
            {
#if DEBUG
                return AppDomain.CurrentDomain.BaseDirectory;
#else
                return
                    $"{Environment.GetFolderPath(Environment.SpecialFolder.CommonApplicationData)}\\RingSoft\\DbLookupDemoApp\\";
#endif
            }
        }

        public override IAppSplashWindow AppSplashWindow => _splashWindow;

        private Application _application;
        private MainWindow _mainWindow;
        private AppSplashWindow _splashWindow;

        public WpfAppStart(Application application)
        {
            _application = application;
        }

        public override void StartApp(string appSection, string[] args)
        {
            //LookupControlsGlobals.InitUi(ProgramDataFolder);

            LookupControlsGlobals.DbMaintenanceProcessorFactory = new AppDbMaintenanceProcessorFactory();
            LookupControlsGlobals.DbMaintenanceButtonsFactory = new AppDbMaintenanceButtonsFactory();

            LookupControlsGlobals.LookupControlContentTemplateFactory =
                new AppLookupContentTemplateFactory(_application);

            _mainWindow = new MainWindow();
            base.StartApp(appSection, args);
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

        protected override void FinishStartup()
        {
            //var lookupUserInterface = DbDataProcessor.UserInterface;
            //DbDataProcessor.UserInterface = this;
            //var controlsUserInterface = ControlsGlobals.UserInterface;
            //ControlsGlobals.UserInterface = this;

            ChangeEntityFrameworkVersion(RegistrySettings.GetEntityFrameworkVersion());

            //DbDataProcessor.UserInterface = lookupUserInterface;
            //ControlsGlobals.UserInterface = controlsUserInterface;

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
                default:
                    throw new ArgumentOutOfRangeException(nameof(newVersion), newVersion, null);
            }
        }

        private void MegaDbLookupContextOnLookupView(object sender, LookupAddViewArgs e)
        {
            if (e.LookupData.LookupDefinition.TableDefinition == RsDbLookupAppGlobals.EfProcessor.MegaDbLookupContext.Items)
            {
                ShowAddOnTheFlyWindow(new ItemsWindow(), e);
            }
            else if (e.LookupData.LookupDefinition.TableDefinition == RsDbLookupAppGlobals.EfProcessor.MegaDbLookupContext.Locations)
            {
                ShowAddOnTheFlyWindow(new LocationWindow(), e);
            }
            else if (e.LookupData.LookupDefinition.TableDefinition == RsDbLookupAppGlobals.EfProcessor.MegaDbLookupContext.Manufacturers)
            {
                ShowAddOnTheFlyWindow(new ManufacturerWindow(), e);
            }
            else if (e.LookupData.LookupDefinition.TableDefinition ==
                     RsDbLookupAppGlobals.EfProcessor.MegaDbLookupContext.StockMasters)
            {
                ShowAddOnTheFlyWindow(new StockMasterWindow(), e);
            }
            else if (e.LookupData.LookupDefinition.TableDefinition ==
                     RsDbLookupAppGlobals.EfProcessor.MegaDbLookupContext.StockCostQuantities)
            {
                ShowAddOnTheFlyWindow(new StockCostQuantityWindow(), e);
            }
        }

        private void NorthwindLookupContext_LookupView(object sender, LookupAddViewArgs e)
        {
            if (e.LookupData.LookupDefinition.TableDefinition == RsDbLookupAppGlobals.EfProcessor.NorthwindLookupContext.Customers)
            {
                ShowAddOnTheFlyWindow(new CustomersWindow(), e);
            }
            else if (e.LookupData.LookupDefinition.TableDefinition == RsDbLookupAppGlobals.EfProcessor.NorthwindLookupContext.Orders)
            {
                if (e.InputParameter is NorthwindViewModelInput northwindViewModelInput)
                {
                    if (northwindViewModelInput.OrderInput.GridMode)
                        ShowAddOnTheFlyWindow(new OrdersGridWindow(), e);
                    else 
                        ShowAddOnTheFlyWindow(new OrdersWindow(), e);
                    return;
                }
                
                ShowAddOnTheFlyWindow(new OrdersWindow(), e);
            }
            else if (e.LookupData.LookupDefinition.TableDefinition == RsDbLookupAppGlobals.EfProcessor.NorthwindLookupContext.Employees)
            {
                ShowAddOnTheFlyWindow(new EmployeesWindow(), e);
            }
            else if (e.LookupData.LookupDefinition.TableDefinition ==
                     RsDbLookupAppGlobals.EfProcessor.NorthwindLookupContext.OrderDetails)
            {
                if (e.InputParameter is not NorthwindViewModelInput northwindViewModelInput) return;
                if (northwindViewModelInput.OrderInput.GridMode)
                    ShowAddOnTheFlyWindow(new OrdersGridWindow(), e);
                else
                {
                    if (northwindViewModelInput.OrderInput.FromProductOrders)
                    {
                        ShowAddOnTheFlyWindow(new OrdersWindow(), e);
                    }
                    else
                    {
                        ShowAddOnTheFlyWindow(new OrderDetailsWindow(), e);
                    }
                }
            }
            else if (e.LookupData.LookupDefinition.TableDefinition ==
                     RsDbLookupAppGlobals.EfProcessor.NorthwindLookupContext.Products)
            {
                ShowAddOnTheFlyWindow(new ProductsWindow(), e);
            }
        }

        private void ShowAddOnTheFlyWindow(AppDbMaintenanceWindow maintenanceWindow, LookupAddViewArgs e)
        {
            if (e.OwnerWindow is Window ownerWindow)
                maintenanceWindow.Owner = ownerWindow;

            maintenanceWindow.ShowInTaskbar = false;
            maintenanceWindow.Processor.InitializeFromLookupData(e);
            maintenanceWindow.ShowDialog();
        }

        public void ShowDataProcessResult(DataProcessResult dataProcessResult)
        {
            _splashWindow.ShowErrorMessageBox(dataProcessResult.Message, "Database Connection Error!");
        }

        public void ShowAddOnTheFlyWindow(LookupAddViewArgs e)
        {
            
        }

        public void PlaySystemSound(RsMessageBoxIcons icon)
        {
            switch (icon)
            {
                case RsMessageBoxIcons.Error:
                    SystemSounds.Hand.Play();
                    break;
                case RsMessageBoxIcons.Exclamation:
                    SystemSounds.Exclamation.Play();
                    break;
                case RsMessageBoxIcons.Information:
                    SystemSounds.Exclamation.Play();
                    break;
                default:
                    throw new ArgumentOutOfRangeException(nameof(icon), icon, null);
            }
        }

        public object GetOwnerWindow()
        {
            return LookupControlsGlobals.ActiveWindow;
        }

        public string FormatValue(string value, int hostId)
        {
            return value;
        }

        public void SetWindowCursor(WindowCursorTypes cursor)
        {
            
        }

        public async Task ShowMessageBox(string text, string caption, RsMessageBoxIcons icon)
        {
            _splashWindow.ShowErrorMessageBox(text, caption);
        }

        public async Task<MessageBoxButtonsResult> ShowYesNoMessageBox(string text, string caption, bool playSound = false)
        {
            return MessageBoxButtonsResult.Yes;
        }

        public async Task<MessageBoxButtonsResult> ShowYesNoCancelMessageBox(string text, string caption, bool playSound = false)
        {
            return MessageBoxButtonsResult.Cancel;
        }
    }
}
