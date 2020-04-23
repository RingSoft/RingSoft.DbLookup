using System.Windows;

namespace RingSoft.DbLookup.App.WPFCore
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        protected override void OnStartup(StartupEventArgs e)
        {
            var wpfAppStart = new WpfAppStart(this);
            wpfAppStart.StartApp("WPF", e.Args);

            base.OnStartup(e);
        }
    }
}
