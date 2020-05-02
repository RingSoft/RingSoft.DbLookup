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
                stockMasterWindow.ShowDialog();
            };

            CloseButton.Click += (sender, args) => Close();
        }

        private void DatabaseSetupClick()
        {
            var dbSetupWindow = new DbSetupWindow();
            dbSetupWindow.ShowDialog();
        }
    }
}
