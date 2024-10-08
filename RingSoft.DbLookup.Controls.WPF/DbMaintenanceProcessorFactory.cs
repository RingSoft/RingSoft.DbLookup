// ***********************************************************************
// Assembly         : RingSoft.DbLookup.Controls.WPF
// Author           : petem
// Created          : 12-19-2022
//
// Last Modified By : petem
// Last Modified On : 12-10-2023
// ***********************************************************************
// <copyright file="DbMaintenanceProcessorFactory.cs" company="Peter Ringering">
//     Copyright (c) 2023. All rights reserved.
// </copyright>
// <summary></summary>
// ***********************************************************************
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using RingSoft.DbLookup.Controls.WPF;

namespace RingSoft.DbLookup.Controls.WPF
{
    /// <summary>
    /// Constructs a database maintenance processor to handle the behavior of a database maintenance window.
    /// </summary>
    public abstract class DbMaintenanceProcessorFactory
    {
        public DbMaintenanceProcessorFactory()
        {
            LookupControlsGlobals.DbMaintenanceProcessorFactory = this;
        }

        /// <summary>
        /// Gets the processor.
        /// </summary>
        /// <returns>IDbMaintenanceProcessor.</returns>
        public abstract DbMaintenanceWindowProcessor GetProcessor();
    }
}
