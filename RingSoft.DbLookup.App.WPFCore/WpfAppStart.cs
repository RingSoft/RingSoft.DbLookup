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
using RingSoft.DbLookup.Controls.WPF.AdvancedFind;

namespace RingSoft.DbLookup.App.WPFCore
{
    public class WpfAppStart : AppStart
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

        public static NorthwindWindowRegistry NorthwindWindowRegistry { get; } = new NorthwindWindowRegistry();
        public static DbMaintenanceWindowRegistry MegaDbWindowRegistry { get; } = new DbMaintenanceWindowRegistry();

        private Application _application;
        private MainWindow _mainWindow;
        private AppSplashWindow _splashWindow;

        public WpfAppStart(Application application)
        {
            _application = application;
        }

        public override void StartApp(string appSection, string[] args)
        {
            var appDbMaintenanceProcessorFactory = new AppDbMaintenanceProcessorFactory();
            var appDbMaintenanceButtonsFactory = new AppDbMaintenanceButtonsFactory();
            var appLookupContentTemplateFactory = new AppLookupContentTemplateFactory(_application);
            var appLookupWindowFactory = new AppLookupWindowFactory();

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
            ChangeEntityFrameworkVersion(RegistrySettings.GetEntityFrameworkVersion());

            MegaDbWindowRegistry.RegisterUserControl
                <AdvancedFindUserControl>(RsDbLookupAppGlobals.EfProcessor.MegaDbLookupContext.AdvancedFinds);

            MegaDbWindowRegistry.RegisterUserControl<ItemsUserControl>(
                RsDbLookupAppGlobals.EfProcessor.MegaDbLookupContext.Items);

            MegaDbWindowRegistry.RegisterWindow<LocationWindow>(
                RsDbLookupAppGlobals.EfProcessor.MegaDbLookupContext.Locations);

            MegaDbWindowRegistry.RegisterWindow<ManufacturerWindow>(
                RsDbLookupAppGlobals.EfProcessor.MegaDbLookupContext.Manufacturers);

            MegaDbWindowRegistry.RegisterUserControl<StockMasterUserControl>(
                RsDbLookupAppGlobals.EfProcessor.MegaDbLookupContext.StockMasters);

            MegaDbWindowRegistry.RegisterWindow<StockCostQuantityWindow>(
                RsDbLookupAppGlobals.EfProcessor.MegaDbLookupContext.StockCostQuantities);

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

    }
}
