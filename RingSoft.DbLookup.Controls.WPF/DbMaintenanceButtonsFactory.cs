// ***********************************************************************
// Assembly         : RingSoft.DbLookup.Controls.WPF
// Author           : petem
// Created          : 12-19-2022
//
// Last Modified By : petem
// Last Modified On : 12-10-2023
// ***********************************************************************
// <copyright file="DbMaintenanceButtonsFactory.cs" company="Peter Ringering">
//     Copyright (c) . All rights reserved.
// </copyright>
// <summary></summary>
// ***********************************************************************
using System.Windows.Controls;
using RingSoft.DbMaintenance;

namespace RingSoft.DbLookup.Controls.WPF
{
    /// <summary>
    /// Class DbMaintenanceButtonsFactory.
    /// </summary>
    public abstract class DbMaintenanceButtonsFactory
    {
        /// <summary>
        /// Gets the buttons control.
        /// </summary>
        /// <returns>Control.</returns>
        public abstract Control GetButtonsControl();

        /// <summary>
        /// Gets the advanced find buttons control.
        /// </summary>
        /// <param name="viewModel">The view model.</param>
        /// <returns>Control.</returns>
        public abstract Control GetAdvancedFindButtonsControl(AdvancedFindViewModel viewModel);

        /// <summary>
        /// Gets the record locking buttons control.
        /// </summary>
        /// <param name="viewModel">The view model.</param>
        /// <returns>Control.</returns>
        public abstract Control GetRecordLockingButtonsControl(RecordLockingViewModel viewModel);
    }
}
