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
        public static LookupWindowFactory LookupWindowFactory { get; private set; } = new LookupWindowFactory();

        private static ControlsUserInterface _userInterface = new ControlsUserInterface();

        public static void InitUi()
        {
            DbDataProcessor.UserInterface = _userInterface;
            WPFControlsGlobals.InitUi();
        }

        public static void InitUi(LookupWindowFactory lookupWindowFactory)
        {
            DbDataProcessor.UserInterface = _userInterface;
            LookupWindowFactory = lookupWindowFactory;
            WPFControlsGlobals.InitUi();
        }
    }
}
