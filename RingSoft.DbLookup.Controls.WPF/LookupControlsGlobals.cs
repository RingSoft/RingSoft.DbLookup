using System;
using System.Media;
using RingSoft.DataEntryControls.WPF;
using RingSoft.DbLookup.DataProcessor;
using System.Windows;
using RingSoft.DataEntryControls.Engine;
using RingSoft.DbLookup.Controls.WPF.AdvancedFind;
using RingSoft.DbLookup.Lookup;

namespace RingSoft.DbLookup.Controls.WPF
{
    public class ControlsUserInterface : IDbLookupUserInterface
    {
        public void ShowDataProcessResult(DataProcessResult dataProcessResult)
        {
            Application.Current.Dispatcher.Invoke(() =>
            {
                var dataProcessResultWindow = new DataProcessResultWindow(dataProcessResult);
                dataProcessResultWindow.Owner = Application.Current.MainWindow;
                dataProcessResultWindow.ShowInTaskbar = false;
                dataProcessResultWindow.ShowDialog();
            });
        }

        public void ShowAddOnTheFlyWindow(LookupAddViewArgs e)
        {
            if (e.LookupData.LookupDefinition.TableDefinition == SystemGlobals.AdvancedFindLookupContext.AdvancedFinds)
            {
                ShowAddOnTheFlyWindow(new AdvancedFindWindow(), e);
            }
        }

        public void PlaySystemSound(RsMessageBoxIcons icon)
        {
            switch (icon)
            {
                case RsMessageBoxIcons.Error:
                    SystemSounds.Hand.Play();
                    break;
                case RsMessageBoxIcons.Exclamation:
                case RsMessageBoxIcons.Information:
                    SystemSounds.Exclamation.Play();
                    break;
                default:
                    throw new ArgumentOutOfRangeException(nameof(icon), icon, null);
            }
        }

        private void ShowAddOnTheFlyWindow(AdvancedFindWindow maintenanceWindow, LookupAddViewArgs e)
        {
            if (e.OwnerWindow is Window ownerWindow)
                maintenanceWindow.Owner = ownerWindow;

            maintenanceWindow.ShowInTaskbar = false;
            maintenanceWindow.Loaded += (sender, args) => maintenanceWindow.Processor.InitializeFromLookupData(e);
            maintenanceWindow.ShowDialog();
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

        public static DbMaintenanceButtonsFactory DbMaintenanceButtonsFactory { get; set; } = new DbMaintenanceButtonsFactory();

        public static void InitUi()
        {
            DbDataProcessor.UserInterface = _userInterface;
            WPFControlsGlobals.InitUi();
            WPFControlsGlobals.DataEntryGridHostFactory = new LookupGridEditHostFactory();
        }

    }
}
