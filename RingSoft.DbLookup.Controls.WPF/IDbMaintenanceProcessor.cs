// ***********************************************************************
// Assembly         : RingSoft.DbLookup.Controls.WPF
// Author           : petem
// Created          : 12-19-2022
//
// Last Modified By : petem
// Last Modified On : 12-07-2023
// ***********************************************************************
// <copyright file="IDbMaintenanceProcessor.cs" company="Peter Ringering">
//     Copyright (c) 2023. All rights reserved.
// </copyright>
// <summary></summary>
// ***********************************************************************
using System.Collections.Generic;
using System.Windows.Controls;
using RingSoft.DataEntryControls.Engine;
using RingSoft.DataEntryControls.WPF;
using RingSoft.DbLookup.Lookup;
using RingSoft.DbLookup.ModelDefinition.FieldDefinitions;
using RingSoft.DbMaintenance;

namespace RingSoft.DbLookup.Controls.WPF
{
    /// <summary>
    /// Interface IDbMaintenanceProcessor
    /// Extends the <see cref="IDbMaintenanceDataProcessor" />
    /// </summary>
    /// <seealso cref="IDbMaintenanceDataProcessor" />
    public interface IDbMaintenanceProcessor : IDbMaintenanceDataProcessor
    {
        /// <summary>
        /// Initializes the specified window.
        /// </summary>
        /// <param name="window">The window.</param>
        /// <param name="buttonsControl">The buttons control.</param>
        /// <param name="viewModel">The view model.</param>
        /// <param name="view">The view.</param>
        /// <param name="statusBar">The status bar.</param>
        void Initialize(IDbMaintenanceVisualView visualView, Control buttonsControl,
            DbMaintenanceViewModelBase viewModel, IDbMaintenanceView view, DbMaintenanceStatusBar statusBar = null);

        /// <summary>
        /// Registers the form key control.
        /// </summary>
        /// <param name="keyAutoFillControl">The key automatic fill control.</param>
        void RegisterFormKeyControl(AutoFillControl keyAutoFillControl);

        /// <summary>
        /// Sets the control read only mode.
        /// </summary>
        /// <param name="control">The control.</param>
        /// <param name="readOnlyValue">if set to <c>true</c> [read only value].</param>
        /// <returns><c>true</c> if XXXX, <c>false</c> otherwise.</returns>
        bool SetControlReadOnlyMode(Control control, bool readOnlyValue);

        /// <summary>
        /// Checks the add on fly mode.
        /// </summary>
        void CheckAddOnFlyMode();
    }
}
