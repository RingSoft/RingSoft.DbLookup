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
using RingSoft.DataEntryControls.Engine.DataEntryGrid;
using RingSoft.DataEntryControls.WPF.DataEntryGrid;
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

        private static DbMaintenanceProcessorFactory _dbMaintProcessorFactory;
        public static DbMaintenanceProcessorFactory DbMaintenanceProcessorFactory
        {
            get
            {
                if (_dbMaintProcessorFactory == null)
                {
                    throw new Exception(
                        $"You must implement IDbMaintenanceProcessor and override DbMaintenanceProcessorFactory and set it to LookupControlsGlobals.DbMaintenanceProcessorFactory.");
                }
                return _dbMaintProcessorFactory;
            } 
            set
            {
            _dbMaintProcessorFactory = value;
        }
        }

        private static ControlsUserInterface _userInterface = new ControlsUserInterface();

        private static DbMaintenanceButtonsFactory _dbMaintenanceButtonsFactory;
        public static DbMaintenanceButtonsFactory DbMaintenanceButtonsFactory
        {
            get
            {
                if (_dbMaintenanceButtonsFactory == null)
                {
                    throw new Exception(
                        $"You must implement DbMaintenanceButtonsFactory and set it to LookupControlsGlobals.LookupControlsGlobals.DbMaintenanceButtonsFactory.");

                }
                return _dbMaintenanceButtonsFactory;
            }
            set
            {
                _dbMaintenanceButtonsFactory = value;
            }
        }

        public static void InitUi(string programDataFolder = "")
        {
            if (programDataFolder.IsNullOrEmpty())
            {
                programDataFolder = $"{Environment.GetFolderPath(Environment.SpecialFolder.CommonApplicationData)}\\RingSoft\\";
            }
            SystemGlobals.ProgramDataFolder = programDataFolder;
            DbDataProcessor.UserInterface = _userInterface;
            WPFControlsGlobals.InitUi();
            WPFControlsGlobals.DataEntryGridHostFactory = new LookupGridEditHostFactory();
        }

        public static async void PrintDocument(PrinterSetupArgs printerSetupArgs)
        {

            if (!await GblMethods.ValidatePrintingFile())
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

            var controls = window.GetLogicalChildren<AutoFillControl>();
            if (controls != null)
            {
                var foundControl = controls.FirstOrDefault(p => p.Setup == autoFillMap.AutoFillSetup);
                if (foundControl != null)
                {
                    foundControl.SetTabFocusToControl();
                    if (foundControl.GetLogicalParent<DataEntryGrid>() == null)
                    {
                        foundControl.Focus();
                        ControlsGlobals.UserInterface.ShowMessageBox(message, caption, RsMessageBoxIcons.Exclamation);
                    }
                }
            }
        }

        public static List<DbAutoFillMap> GetAutoFills(Window window)
        {
            var result = new List<DbAutoFillMap>();
            var autoFills = window.GetLogicalChildren<AutoFillControl>();

            FillAutoFillMaps(autoFills, result);

            return result;
        }

        private static void FillAutoFillMaps(List<AutoFillControl> autoFills, List<DbAutoFillMap> result)
        {
            foreach (var autoFillControl in autoFills)
            {
                var dataEntryGrid = autoFillControl.GetParentOfType<DataEntryGrid>();
                if (dataEntryGrid == null)
                {
                    result.Add(new DbAutoFillMap(autoFillControl.Setup, autoFillControl.Value));
                }
            }
        }
    }
}
