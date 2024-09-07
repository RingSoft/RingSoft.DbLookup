// ***********************************************************************
// Assembly         : RingSoft.DbLookup.Controls.WPF
// Author           : petem
// Created          : 12-19-2022
//
// Last Modified By : petem
// Last Modified On : 12-16-2023
// ***********************************************************************
// <copyright file="DbMaintenanceButtonsFactory.cs" company="Peter Ringering">
//     Copyright (c) 2023. All rights reserved.
// </copyright>
// <summary></summary>
// ***********************************************************************
using System.Windows.Controls;
using RingSoft.DbMaintenance;

namespace RingSoft.DbLookup.Controls.WPF
{
    /// <summary>
    /// Creates a database window buttons control.
    /// </summary>
    public abstract class DbMaintenanceButtonsFactory
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="DbMaintenanceButtonsFactory"/> class.
        /// </summary>
        public DbMaintenanceButtonsFactory()
        {
            LookupControlsGlobals.DbMaintenanceButtonsFactory = this;
        }
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
