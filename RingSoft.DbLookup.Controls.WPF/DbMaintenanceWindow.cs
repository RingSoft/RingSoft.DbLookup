// ***********************************************************************
// Assembly         : RingSoft.DbLookup.Controls.WPF
// Author           : petem
// Created          : 12-13-2023
//
// Last Modified By : petem
// Last Modified On : 12-16-2023
// ***********************************************************************
// <copyright file="DbMaintenanceWindow.cs" company="Peter Ringering">
//     Copyright (c) 2023. All rights reserved.
// </copyright>
// <summary></summary>
// ***********************************************************************
using System.Windows.Controls;
using RingSoft.DataEntryControls.WPF;
using RingSoft.DbMaintenance;

namespace RingSoft.DbLookup.Controls.WPF
{
    /// <summary>
    /// The base class of a database entity maintenance window.
    /// Implements the <see cref="BaseWindow" />
    /// Implements the <see cref="IDbMaintenanceView" />
    /// </summary>
    /// <seealso cref="BaseWindow" />
    /// <seealso cref="IDbMaintenanceView" />
    public abstract class DbMaintenanceWindow : BaseWindow, IDbMaintenanceView
    {
        /// <summary>
        /// Gets the view model.
        /// </summary>
        /// <value>The view model.</value>
        public abstract DbMaintenanceViewModelBase ViewModel { get; }

        /// <summary>
        /// Gets the maintenance buttons control.
        /// </summary>
        /// <value>The maintenance buttons control.</value>
        public abstract Control MaintenanceButtonsControl { get; }

        /// <summary>
        /// Gets the database status bar.
        /// </summary>
        /// <value>The database status bar.</value>
        public abstract DbMaintenanceStatusBar DbStatusBar { get; }

        /// <summary>
        /// Gets the key automatic fill control.
        /// </summary>
        /// <value>The key automatic fill control.</value>
        public AutoFillControl KeyAutoFillControl { get; private set; }

        /// <summary>
        /// Gets the processor.
        /// </summary>
        /// <value>The processor.</value>
        public IDbMaintenanceProcessor Processor { get; }

        /// <summary>
        /// Initializes a new instance of the <see cref="DbMaintenanceWindow" /> class.
        /// </summary>
        public DbMaintenanceWindow()
        {
            Processor = LookupControlsGlobals.DbMaintenanceProcessorFactory.GetProcessor();
            Loaded += (sender, args) =>
            {
                Processor.Initialize(this, MaintenanceButtonsControl, ViewModel, this, DbStatusBar);
                Closing += (sender, args) => ViewModel.OnWindowClosing(args);
                ViewModel.OnViewLoaded(this);
            };
        }

        /// <summary>
        /// Called when validation fails for a field control.  It gives opportunity to set focus to the control and display a validation error message.
        /// </summary>
        public virtual void ResetViewForNewRecord()
        {
        }

        /// <summary>
        /// Called when [read only mode set].
        /// </summary>
        /// <param name="readOnlyValue">if set to <c>true</c> [read only value].</param>
        protected override void OnReadOnlyModeSet(bool readOnlyValue)
        {
            Processor.OnReadOnlyModeSet(readOnlyValue);
            base.OnReadOnlyModeSet(readOnlyValue);
        }

        /// <summary>
        /// Sets the control read only mode.
        /// </summary>
        /// <param name="control">The control.</param>
        /// <param name="readOnlyValue">if set to <c>true</c> [read only value].</param>
        public override void SetControlReadOnlyMode(Control control, bool readOnlyValue)
        {
            if (Processor.SetControlReadOnlyMode(control, readOnlyValue))
                base.SetControlReadOnlyMode(control, readOnlyValue);
        }

        /// <summary>
        /// Registers the form key control.
        /// </summary>
        /// <param name="keyAutoFillControl">The key automatic fill control.</param>
        protected void RegisterFormKeyControl(AutoFillControl keyAutoFillControl)
        {
            Processor.RegisterFormKeyControl(keyAutoFillControl);
        }
    }
}
