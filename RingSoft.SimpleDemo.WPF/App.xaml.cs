using System.Windows;
using RingSoft.DbLookup.Controls.WPF;
using RingSoft.SimpleDemo.WPF.Northwind;

namespace RingSoft.SimpleDemo.WPF
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App
    {
        public static NorthwindLookupContext LookupContext { get; private set; }

        public static NorthwindEfDataProcessor EfDataProcessor { get; private set; }



        protected override void OnStartup(StartupEventArgs e)
        {
            LookupContext = new NorthwindLookupContext();
            EfDataProcessor = new NorthwindEfDataProcessor();

            ControlsGlobals.InitUi(new DemoLookupWindowFactory());
            base.OnStartup(e);
        }
    }
}
