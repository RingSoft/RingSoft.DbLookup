using System;
using System.Windows.Threading;
using RingSoft.DbLookup.App.WPF.Views.MegaDb;

namespace RingSoft.DbLookup.App.WPF.Views
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

            MegaDbButton.Click += (sender, args) =>
            {
                var itemsWindow = new ItemsWindow();
                itemsWindow.ShowDialog();
            };
        }
    }
}
