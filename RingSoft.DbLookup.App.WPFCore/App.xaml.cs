using System.Windows;
using RingSoft.DataEntryControls.WPF;
using RingSoft.DbLookup.App.Library;
using RingSoft.DbLookup.Controls.WPF;

namespace RingSoft.DbLookup.App.WPFCore
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App

    {
        protected override void OnStartup(StartupEventArgs e)
        {
            var wpfAppStart = new WpfAppStart(this);
            wpfAppStart.StartApp("WPF", e.Args);

            base.OnStartup(e);
        }
    }
}
