using System;
using System.Diagnostics;
using System.Media;
using RingSoft.DataEntryControls.WPF;
using RingSoft.DbLookup.DataProcessor;
using System.Windows;
using RingSoft.DataEntryControls.Engine;
using RingSoft.DbLookup.Controls.WPF.AdvancedFind;
using RingSoft.DbLookup.Lookup;
using System.Linq;
using System.Windows.Documents;
using System.Windows.Input;
using System.Collections.Generic;
using System.Windows.Controls;
using System.Windows.Media;
using RingSoft.DbMaintenance;

namespace RingSoft.DbLookup.Controls.WPF
{
    public class ControlsUserInterface : IDbLookupUserInterface
    {
        public static Window GetActiveWindow()
        {
            var activeWindow = WPFControlsGlobals.ActiveWindow;
            return activeWindow;
        }
        public void ShowDataProcessResult(DataProcessResult dataProcessResult)
        {
            var activeWindow = GetActiveWindow();
            if (activeWindow == null)
            {
                //var dataProcessResultWindow = new DataProcessResultWindow(dataProcessResult);
                //dataProcessResultWindow.Owner = activeWindow;
                //dataProcessResultWindow.ShowInTaskbar = false;
                //dataProcessResultWindow.ShowDialog();
            }
            else
            {
                activeWindow.Dispatcher.Invoke(() =>
                {
                    var dataProcessResultWindow = new DataProcessResultWindow(dataProcessResult);
                    dataProcessResultWindow.Owner = activeWindow;
                    dataProcessResultWindow.ShowInTaskbar = false;
                    dataProcessResultWindow.ShowDialog();
                });
            }
        }

        public void ShowAddOnTheFlyWindow(LookupAddViewArgs e)
        {
            var activeWindow = GetActiveWindow();
            if (e.LookupData.LookupDefinition.TableDefinition == SystemGlobals.AdvancedFindLookupContext.AdvancedFinds)
            {
                var maintenanceWindow = new AdvancedFindWindow();
                //if (e.OwnerWindow is Window ownerWindow)
                //    maintenanceWindow.Owner = ownerWindow;

                maintenanceWindow.Owner = activeWindow;
                maintenanceWindow.ShowInTaskbar = false;
                maintenanceWindow.Loaded += (sender, args) => maintenanceWindow.Processor.InitializeFromLookupData(e);
                maintenanceWindow.Closed += (sender, args) =>
                {
                    activeWindow.Activate();
                    e.LookupData.LookupDefinition.FireCloseEvent(e.LookupData);
                };
                maintenanceWindow.Show();
            }
            else if (e.LookupData.LookupDefinition.TableDefinition ==
                     SystemGlobals.AdvancedFindLookupContext.RecordLocks)
            {
                var maintenanceWindow = new RecordLockingWindow();
                //if (e.OwnerWindow is Window ownerWindow)
                //    maintenanceWindow.Owner = ownerWindow;
                maintenanceWindow.Owner = activeWindow;

                maintenanceWindow.ShowInTaskbar = false;
                maintenanceWindow.Loaded += (sender, args) => maintenanceWindow.Processor.InitializeFromLookupData(e);
                maintenanceWindow.Closed += (sender, args) => activeWindow.Activate();
                maintenanceWindow.ShowDialog();

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

        public object GetOwnerWindow()
        {
            return GetActiveWindow();
        }

        public string FormatValue(string value, int hostId)
        {
            return LookupControlsGlobals.LookupControlSearchForFactory.FormatValue(hostId, value);
        }

        //private void ShowAddOnTheFlyWindow(AdvancedFindWindow maintenanceWindow, LookupAddViewArgs e)
        //{
        //    if (e.OwnerWindow is Window ownerWindow)
        //        maintenanceWindow.Owner = ownerWindow;

        //    maintenanceWindow.ShowInTaskbar = false;
        //    maintenanceWindow.Loaded += (sender, args) => maintenanceWindow.Processor.InitializeFromLookupData(e);
        //    maintenanceWindow.ShowDialog();
        //}

    }
    public static class LookupControlsGlobals
    {
        public static Window ActiveWindow => ControlsUserInterface.GetActiveWindow();

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

        public static void PrintDocument(PrinterSetupArgs printerSetupArgs)
        {

            if (!GblMethods.ValidatePrintingFile())
            {
                return;
            }

            var optionsWindow = new PrintSetupWindow(printerSetupArgs);
            optionsWindow.Owner = ActiveWindow;
            optionsWindow.ShowInTaskbar = false;
            optionsWindow.ShowDialog();
        }

        public static bool IsShiftKeyDown()
        {
            if (Keyboard.IsKeyDown(Key.LeftShift))
            {
                return true;
            }

            return Keyboard.IsKeyDown(Key.RightShift);
        }


        public static void HandleValFail(Window window, DbAutoFillMap autoFillMap)
        {
            var caption = "Validation Fail";
            var message = $"{autoFillMap.AutoFillSetup.ForeignField.Description} has an invalid value.";

            var controls = window.GetChildrenOfType<AutoFillControl>();
            if (controls != null)
            {
                var foundControl = controls.FirstOrDefault(p => p.Setup == autoFillMap.AutoFillSetup);
                if (foundControl != null)
                {
                    var tabItem = foundControl.GetParentOfType<TabItem>();
                    if (tabItem != null)
                    {

                    }

                    foundControl.Focus();
                    ControlsGlobals.UserInterface.ShowMessageBox(message, caption, RsMessageBoxIcons.Exclamation);
                }
            }
        }

        public static List<DbAutoFillMap> GetAutoFills(Window window)
        {
            var result = new List<DbAutoFillMap>();
            var autoFills = window.GetChildrenOfType<AutoFillControl>();

            foreach (var autoFillControl in autoFills)
            {
                result.Add(new DbAutoFillMap(autoFillControl.Setup, autoFillControl.Value));
            }

            return result;
        }

    }
}
