using RingSoft.DbLookup.App.Library;
using RingSoft.DbLookup.App.WPFCore.MegaDb;
using RingSoft.DbLookup.App.WPFCore.Northwind;
using System;
using System.Windows;
using System.Windows.Threading;

namespace RingSoft.DbLookup.App.WPFCore
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow
    {
        public event EventHandler Done;

        private RegistrySettings _registrySettings;

        public MainWindow()
        {
            InitializeComponent();

            var timer = new DispatcherTimer
            {
                Interval = new TimeSpan(0,0,0, 0,10),
                IsEnabled = true
            };

            timer.Tick += (sender, args) =>
            {
                timer.Stop();
                Done?.Invoke(this, EventArgs.Empty);
            };

            ContentRendered += (sender, args) =>
            {
                Activate();
                timer.Start();
            };

            Loaded += (sender, args) =>
            {
                _registrySettings = new RegistrySettings();
            };

            //Closing += (sender, args) =>
            //{
            //    if (_openWindows > 0)
            //    {
            //        var message = "All open windows must be closed before exiting the application.";
            //        MessageBox.Show(this, message, "Application Exit", MessageBoxButton.OK,
            //            MessageBoxImage.Exclamation);

            //        args.Cancel = true;
            //    }
            //};

            DatabaseSetupButton.Click += (sender, args) => DatabaseSetupClick();

            NorthwindLookupButton.Click += (sender, args) =>
            {
                if (!RsDbLookupAppGlobals.EfProcessor.NorthwindLookupContext.NorthwindContextConfiguration
                    .TestConnection())
                {
                    DatabaseSetupClick();
                    return;
                }
                RsDbLookupAppGlobals.EfProcessor.NorthwindLookupContext.Initialize();
                var ordersWindow = new OrdersWindow();
                ordersWindow.Owner = this;
                ordersWindow.ShowDialog();
            };

            NorthwindGridButton.Click += (sender, args) =>
            {
                if (!RsDbLookupAppGlobals.EfProcessor.NorthwindLookupContext.NorthwindContextConfiguration
                    .TestConnection())
                {
                    DatabaseSetupClick();
                    return;
                }
                RsDbLookupAppGlobals.EfProcessor.NorthwindLookupContext.Initialize();
                var ordersWindow = new OrdersGridWindow();
                ordersWindow.Owner = this;
                ordersWindow.ShowDialog();
            };

            MegaDbButton.Click += (sender, args) =>
            {
                if (!ValidateMegaDbWindow())
                    return;

                RsDbLookupAppGlobals.EfProcessor.MegaDbLookupContext.Initialize();
                var itemsWindow = new ItemsWindow();
                itemsWindow.Owner = this;
                itemsWindow.ShowDialog();
            };

            StockTrackerButton.Click += (sender, args) =>
            {
                if (!ValidateMegaDbWindow())
                    return;

                RsDbLookupAppGlobals.EfProcessor.MegaDbLookupContext.Initialize();
                var stockMasterWindow = new StockMasterWindow();
                stockMasterWindow.Owner = this;
                stockMasterWindow.ShowDialog();
            };

            CloseButton.Click += (sender, args) => Close();
        }

        private bool ValidateMegaDbWindow()
        {
            var result = true;
            if (_registrySettings.MegaDbPlatformType == MegaDbPlatforms.None)
            {
                var message =
                    "The Mega Database platform type is set to None.  You must set it to a valid platform type before launching this window.";
                MessageBox.Show(this, message, "Invalid Mega Database Platform Type", MessageBoxButton.OK,
                    MessageBoxImage.Exclamation);
                result = false;
            }

            if (result && !RsDbLookupAppGlobals.EfProcessor.MegaDbLookupContext.MegaDbContextConfiguration
                .TestConnection())
                result = false;

            if (!result)
            {
                DatabaseSetupClick();
            }

            return result;
        }
        private void DatabaseSetupClick()
        {
            //if (_openWindows > 0)
            //{
            //    var message = "All open windows must be closed before modifying the Database Settings.";
            //    MessageBox.Show(this, message, "Database Settings", MessageBoxButton.OK, MessageBoxImage.Exclamation);
            //    return;
            //}
            var dbSetupWindow = new DbSetupWindow();
            dbSetupWindow.ShowInTaskbar = false;
            dbSetupWindow.Owner = this;
            dbSetupWindow.ShowDialog();
            _registrySettings.LoadFromRegistry();
        }
    }
}
