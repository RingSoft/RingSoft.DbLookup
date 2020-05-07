using System.Windows;
using RingSoft.SimpleDemo.WPF.Northwind;

namespace RingSoft.SimpleDemo.WPF
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App
    {
        public static NorthwindLookupContext LookupContext => _lookupContext;

        private static NorthwindLookupContext _lookupContext;

        protected override void OnStartup(StartupEventArgs e)
        {
            _lookupContext = new NorthwindLookupContext();

            base.OnStartup(e);
        }
    }
}
