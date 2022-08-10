using RingSoft.DataEntryControls.WPF;
using RingSoft.DbLookup.DataProcessor;
using System.Windows;

namespace RingSoft.DbLookup.Controls.WPF
{
    public class ControlsUserInterface : IDbLookupUserInterface
    {
        public void ShowDataProcessResult(DataProcessResult dataProcessResult)
        {
            Application.Current.Dispatcher.Invoke(() =>
            {
                var dataProcessResultWindow = new DataProcessResultWindow(dataProcessResult);
                dataProcessResultWindow.ShowDialog();
            });
        }
    }
    public static class LookupControlsGlobals
    {
        public static LookupWindowFactory LookupWindowFactory { get; set; } = new LookupWindowFactory();

        public static LookupSearchForHostFactory LookupControlSearchForFactory { get; set; } = new LookupSearchForHostFactory();

        public static LookupControlColumnFactory LookupControlColumnFactory { get; set; } =
            new LookupControlColumnFactory();

        public static LookupControlContentTemplateFactory LookupControlContentTemplateFactory { get; set; } =
            new LookupControlContentTemplateFactory();

        public static DbMaintenanceProcessorFactory DbMaintenanceProcessorFactory { get; set; }

        private static ControlsUserInterface _userInterface = new ControlsUserInterface();

        public static void InitUi()
        {
            DbDataProcessor.UserInterface = _userInterface;
            WPFControlsGlobals.InitUi();
            WPFControlsGlobals.DataEntryGridHostFactory = new LookupGridEditHostFactory();
        }

    }
}
