﻿using System.ComponentModel;
using System.Windows;
using RingSoft.DataEntryControls.WPF;

namespace RingSoft.DbLookup.Controls.WPF
{
    /// <summary>
    /// Interaction logic for DbMaintenanceUcWindow.xaml
    /// </summary>
    public partial class DbMaintenanceUcWindow : IUserControlHost
    {
        private DbMaintenanceUserControl UserControl { get; }
        public DbMaintenanceUcWindow(DbMaintenanceUserControl userControl)
        {
            UserControl = userControl;
            InitializeComponent();
            Title = userControl.Title;
        }

        public void CloseHost()
        {
            Close();
        }

        protected override void OnClosing(CancelEventArgs e)
        {
            UserControl.ViewModel.OnWindowClosing(e);
            base.OnClosing(e);
        }
    }
}