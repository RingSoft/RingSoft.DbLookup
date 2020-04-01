using RingSoft.DbLookup.App.WPF.Views;
using System.Windows;

namespace RingSoft.DbLookup.App.WPF
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        protected override void OnStartup(StartupEventArgs e)
        {
            var wpfAppStart = new WpfAppStart(this, "WPF .NET Framework 4.7.2");
            wpfAppStart.StartApp(e.Args);

            base.OnStartup(e);
        }
    }
}
