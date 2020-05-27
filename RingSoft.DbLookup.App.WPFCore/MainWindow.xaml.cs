using RingSoft.DbLookup.App.Library;
using RingSoft.DbLookup.App.WPFCore.MegaDb;
using RingSoft.DbLookup.App.WPFCore.Northwind;
using System;
using System.Windows.Threading;

namespace RingSoft.DbLookup.App.WPFCore
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow
    {
        public event EventHandler Done;

        private DbSetupWindow _dbSetupWindow;

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
                if (_dbSetupWindow != null)
                    _dbSetupWindow.Activate();
                else 
                    Activate();
                timer.Start();
            };

            Loaded += (sender, args) =>
            {
                if (RsDbLookupAppGlobals.FirstTime)
                    DatabaseSetupClick();
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

            NorthwindButton.Click += (sender, args) =>
            {
                if (!RsDbLookupAppGlobals.EfProcessor.NorthwindLookupContext.NorthwindContextConfiguration
                    .TestConnection())
                {
                    DatabaseSetupClick();
                    return;
                }
                var ordersWindow = new OrdersWindow();
                ordersWindow.Owner = this;
                ordersWindow.ShowDialog();
            };

            MegaDbButton.Click += (sender, args) =>
            {
                if (!RsDbLookupAppGlobals.EfProcessor.MegaDbLookupContext.MegaDbContextConfiguration.TestConnection())
                {
                    DatabaseSetupClick();
                    return;
                }
                var itemsWindow = new ItemsWindow();
                itemsWindow.Owner = this;
                itemsWindow.ShowDialog();
            };

            StockTrackerButton.Click += (sender, args) =>
            {
                if (!RsDbLookupAppGlobals.EfProcessor.MegaDbLookupContext.MegaDbContextConfiguration.TestConnection())
                {
                    DatabaseSetupClick();
                    return;
                }
                var stockMasterWindow = new StockMasterWindow();
                stockMasterWindow.Owner = this;
                stockMasterWindow.ShowDialog();
            };

            CloseButton.Click += (sender, args) => Close();
        }

        private void DatabaseSetupClick()
        {
            //if (_openWindows > 0)
            //{
            //    var message = "All open windows must be closed before modifying the Database Settings.";
            //    MessageBox.Show(this, message, "Database Settings", MessageBoxButton.OK, MessageBoxImage.Exclamation);
            //    return;
            //}
            _dbSetupWindow = new DbSetupWindow();
            _dbSetupWindow.ShowInTaskbar = false;
            _dbSetupWindow.Owner = this;
            _dbSetupWindow.ShowDialog();
        }
    }
}
