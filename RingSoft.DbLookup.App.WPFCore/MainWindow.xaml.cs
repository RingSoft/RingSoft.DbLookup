﻿using RingSoft.DbLookup.App.WPFCore.MegaDb;
using RingSoft.DbLookup.App.WPFCore.Northwind;
using System;
using System.Windows.Threading;
using RingSoft.DbLookup.GetDataProcessor;

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

            DatabaseSetupButton.Click += (sender, args) =>
            {
                var dbSetupWindow = new DbSetupWindow();
                dbSetupWindow.ShowDialog();
            };

            NorthwindButton.Click += (sender, args) =>
            {
                var ordersWindow = new OrdersWindow();
                ordersWindow.ShowDialog();
            };

            MegaDbButton.Click += (sender, args) =>
            {
                var itemsWindow = new ItemsWindow();
                itemsWindow.ShowDialog();
            };

            CloseButton.Click += (sender, args) => Close();
        }
    }
}
