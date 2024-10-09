// ***********************************************************************
// Assembly         : RingSoft.DbLookup.Controls.WPF
// Author           : petem
// Created          : 12-19-2022
//
// Last Modified By : petem
// Last Modified On : 12-10-2023
// ***********************************************************************
// <copyright file="LookupControlsGlobals.cs" company="Peter Ringering">
//     Copyright (c) 2023. All rights reserved.
// </copyright>
// <summary></summary>
// ***********************************************************************
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
using System.IO;

namespace RingSoft.DbLookup.Controls.WPF
{
    /// <summary>
    /// Class ControlsUserInterface.
    /// Implements the <see cref="RingSoft.DbLookup.IDbLookupUserInterface" />
    /// </summary>
    /// <seealso cref="RingSoft.DbLookup.IDbLookupUserInterface" />
    public class ControlsUserInterface : IDbLookupUserInterface
    {
        /// <summary>
        /// Gets the active window.
        /// </summary>
        /// <returns>Window.</returns>
        public static Window GetActiveWindow()
        {
            var activeWindow = WPFControlsGlobals.ActiveWindow;
            return activeWindow;
        }
        /// <summary>
        /// Shows the data process execution result.
        /// </summary>
        /// <param name="dataProcessResult">The data process result.</param>
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

        /// <summary>
        /// Shows the add on the fly window.
        /// </summary>
        /// <param name="e">The e.</param>
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

        /// <summary>
        /// Plays the system sound.
        /// </summary>
        /// <param name="icon">The icon.</param>
        /// <exception cref="System.ArgumentOutOfRangeException">icon - null</exception>
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

        /// <summary>
        /// Gets the owner window.
        /// </summary>
        /// <returns>System.Object.</returns>
        public object GetOwnerWindow()
        {
            return GetActiveWindow();
        }

        /// <summary>
        /// Formats the value.
        /// </summary>
        /// <param name="value">The value.</param>
        /// <param name="hostId">The host identifier.</param>
        /// <returns>System.String.</returns>
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
    /// <summary>
    /// Methods used by all classes in this library.
    /// </summary>
    public static class LookupControlsGlobals
    {
        /// <summary>
        /// Gets the active window.
        /// </summary>
        /// <value>The active window.</value>
        public static Window ActiveWindow => ControlsUserInterface.GetActiveWindow();

        /// <summary>
        /// Gets the lookup window factory.
        /// </summary>
        /// <value>The lookup window factory.</value>
        public static LookupWindowFactory LookupWindowFactory { get; internal set; } = new LookupWindowFactory();

        /// <summary>
        /// Gets the lookup control search for factory.
        /// </summary>
        /// <value>The lookup control search for factory.</value>
        public static LookupSearchForHostFactory LookupControlSearchForFactory { get; internal set; } = new LookupSearchForHostFactory();

        /// <summary>
        /// Gets the lookup control column factory.
        /// </summary>
        /// <value>The lookup control column factory.</value>
        public static LookupControlColumnFactory LookupControlColumnFactory { get; internal set; } =
            new LookupControlColumnFactory();

        /// <summary>
        /// Gets the lookup control content template factory.
        /// </summary>
        /// <value>The lookup control content template factory.</value>
        public static LookupControlContentTemplateFactory LookupControlContentTemplateFactory { get; internal set; } =
            new LookupControlContentTemplateFactory();

        /// <summary>
        /// The database maint processor factory
        /// </summary>
        private static DbMaintenanceProcessorFactory _dbMaintProcessorFactory;
        /// <summary>
        /// Gets the database maintenance processor factory.
        /// </summary>
        /// <value>The database maintenance processor factory.</value>
        /// <exception cref="System.Exception">You must inherit and instantiate DbMaintenanceProcessorFactory.</exception>
        public static DbMaintenanceProcessorFactory DbMaintenanceProcessorFactory
        {
            get
            {
                if (_dbMaintProcessorFactory == null)
                {
                    throw new Exception(
                        $"You must inherit and instantiate DbMaintenanceProcessorFactory.");
                }
                return _dbMaintProcessorFactory;
            } 
            internal set
            {
            _dbMaintProcessorFactory = value;
        }
        }

        /// <summary>
        /// The user interface
        /// </summary>
        private static ControlsUserInterface _userInterface = new ControlsUserInterface();

        /// <summary>
        /// The database maintenance buttons factory
        /// </summary>
        private static DbMaintenanceButtonsFactory _dbMaintenanceButtonsFactory;
        /// <summary>
        /// Gets the database maintenance buttons factory.
        /// </summary>
        /// <value>The database maintenance buttons factory.</value>
        /// <exception cref="System.Exception">You must inherit and instantiate DbMaintenanceButtonsFactory.</exception>
        public static DbMaintenanceButtonsFactory DbMaintenanceButtonsFactory
        {
            get
            {
                if (_dbMaintenanceButtonsFactory == null)
                {
                    throw new Exception(
                        $"You must inherit and instantiate DbMaintenanceButtonsFactory.");

                }
                return _dbMaintenanceButtonsFactory;
            }
            internal set
            {
                _dbMaintenanceButtonsFactory = value;
            }
        }

        public static DbMaintenanceWindowRegistry WindowRegistry { get; internal set; } = new DbMaintenanceWindowRegistry();

        static LookupControlsGlobals()
        {
            InitUi(SystemGlobals.ProgramDataFolder);
        }
        /// <summary>
        /// Initializes the UI.
        /// </summary>
        /// <param name="programDataFolder">The program data folder.</param>
        private static void InitUi(string programDataFolder = "")
        {
            if (programDataFolder.IsNullOrEmpty())
            {
                programDataFolder = $"{Environment.GetFolderPath(Environment.SpecialFolder.CommonApplicationData)}\\RingSoft\\";
            }
            SystemGlobals.ProgramDataFolder = programDataFolder;
            if (!Directory.Exists(SystemGlobals.ProgramDataFolder))
            {
                Directory.CreateDirectory(SystemGlobals.ProgramDataFolder);
            }
            DbDataProcessor.UserInterface = _userInterface;
            var lookupGridEditHostFactory = new LookupGridEditHostFactory();
        }

        /// <summary>
        /// Prints the document.
        /// </summary>
        /// <param name="printerSetupArgs">The printer setup arguments.</param>
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

        /// <summary>
        /// Determines whether [is shift key down].
        /// </summary>
        /// <returns><c>true</c> if [is shift key down]; otherwise, <c>false</c>.</returns>
        public static bool IsShiftKeyDown()
        {
            if (Keyboard.IsKeyDown(Key.LeftShift))
            {
                return true;
            }

            return Keyboard.IsKeyDown(Key.RightShift);
        }


        /// <summary>
        /// Handles the value fail.
        /// </summary>
        /// <param name="window">The window.</param>
        /// <param name="autoFillMap">The automatic fill map.</param>
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

        /// <summary>
        /// Gets the automatic fills.
        /// </summary>
        /// <param name="window">The window.</param>
        /// <returns>List&lt;DbAutoFillMap&gt;.</returns>
        public static List<DbAutoFillMap> GetAutoFills(ContentControl window)
        {
            var result = new List<DbAutoFillMap>();
            var autoFills = window.GetLogicalChildren<AutoFillControl>();

            FillAutoFillMaps(autoFills, result);

            return result;
        }

        /// <summary>
        /// Fills the automatic fill maps.
        /// </summary>
        /// <param name="autoFills">The automatic fills.</param>
        /// <param name="result">The result.</param>
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
