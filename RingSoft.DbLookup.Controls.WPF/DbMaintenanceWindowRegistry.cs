﻿// ***********************************************************************
// Assembly         : RingSoft.DbLookup.Controls.WPF
// Author           : petem
// Created          : 12-14-2023
//
// Last Modified By : petem
// Last Modified On : 12-14-2023
// ***********************************************************************
// <copyright file="DbMaintenanceWindowRegistry.cs" company="Peter Ringering">
//     Copyright (c) 2023. All rights reserved.
// </copyright>
// <summary></summary>
// ***********************************************************************
using RingSoft.DbLookup.Lookup;
using RingSoft.DbLookup.ModelDefinition;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using RingSoft.DataEntryControls.Engine;
using RingSoft.DataEntryControls.WPF;

namespace RingSoft.DbLookup.Controls.WPF
{
    /// <summary>
    /// Maps a table definition to a database window.
    /// Implements the <see cref="RingSoft.DbLookup.DbLookupTableWindowRegistry" />
    /// </summary>
    /// <seealso cref="RingSoft.DbLookup.DbLookupTableWindowRegistry" />
    public class DbMaintenanceWindowRegistry : DbLookupTableWindowRegistry
    {
        /// <summary>
        /// Class WindowRegistryItem.
        /// </summary>
        public class WindowRegistryItem
        {
            /// <summary>
            /// Gets or sets the table definition.
            /// </summary>
            /// <value>The table definition.</value>
            public TableDefinitionBase TableDefinition { get; set; }

            /// <summary>
            /// Gets or sets the maintenance window.
            /// </summary>
            /// <value>The maintenance window.</value>
            public Type MaintenanceWindow { get; set; }
        }

        /// <summary>
        /// Gets the items.
        /// </summary>
        /// <value>The items.</value>
        public static List<WindowRegistryItem> Items { get; private set; } = new List<WindowRegistryItem>();

        /// <summary>
        /// Activates the registry.
        /// </summary>
        public override void ActivateRegistry()
        {
            LookupControlsGlobals.WindowRegistry = this;
            base.ActivateRegistry();
        }

        /// <summary>
        /// Registers the window.
        /// </summary>
        /// <typeparam name="TWindow">The type of the t window.</typeparam>
        /// <param name="tableDefinition">The table definition.</param>
        /// <exception cref="System.ArgumentException">tableDefinition</exception>
        public void RegisterWindow<TWindow>(TableDefinitionBase tableDefinition) where TWindow : DbMaintenanceWindow, new()
        {
            if (tableDefinition == null)
            {
                throw new ArgumentException(nameof(tableDefinition));
            }
            Items.Add(new WindowRegistryItem
            {
                MaintenanceWindow = typeof(TWindow),
                TableDefinition = tableDefinition
            });
        }

        public void RegisterWindow<TWindow, TEntity>()
            where TWindow : DbMaintenanceWindow, new()
            where TEntity: class, new()
        {
            var tableDefinition = GblMethods.GetTableDefinition<TEntity>();
            RegisterWindow<TWindow>(tableDefinition);
        }

        /// <summary>
        /// Gets the database maintenance window.
        /// </summary>
        /// <param name="tableDefinition">The table definition.</param>
        /// <returns>WindowRegistryItem.</returns>
        private WindowRegistryItem GetDbMaintenanceWindow(TableDefinitionBase tableDefinition)
        {
            var item = Items.FirstOrDefault(p => p.TableDefinition == tableDefinition);
            return item;
        }

        /// <summary>
        /// Determines whether [is table registered] [the specified table definition].
        /// </summary>
        /// <param name="tableDefinition">The table definition.</param>
        /// <returns><c>true</c> if [is table registered] [the specified table definition]; otherwise, <c>false</c>.</returns>
        public override bool IsTableRegistered(TableDefinitionBase tableDefinition)
        {
            if (GetDbMaintenanceWindow(tableDefinition) == null)
            {
                return false;
            }
            return true;
        }

        /// <summary>
        /// Shows the add onthe fly window.
        /// </summary>
        /// <param name="tableDefinition">The table definition.</param>
        /// <param name="addViewArgs">The add view arguments.</param>
        /// <param name="inputParameter">The input parameter.</param>
        public sealed override void ShowAddOntheFlyWindow(
            TableDefinitionBase tableDefinition
            , LookupAddViewArgs addViewArgs = null
            , object inputParameter = null)
        {
            var maintenanceWindow = CreateMaintenanceWindow(tableDefinition, addViewArgs, inputParameter);
            if (maintenanceWindow == null)
            {
                var item = GetDbMaintenanceWindow(tableDefinition);
                if (item != null)
                {
                    maintenanceWindow = Activator.CreateInstance(item.MaintenanceWindow) as DbMaintenanceWindow;
                }
            }
            ShowAddOnTheFlyWindow(maintenanceWindow, tableDefinition, addViewArgs, inputParameter);
        }

        /// <summary>
        /// Shows the window.
        /// </summary>
        /// <param name="tableDefinition">The table definition.</param>
        public override void ShowWindow(TableDefinitionBase tableDefinition)
        {
            ShowDbMaintenanceWindow(tableDefinition);
        }

        /// <summary>
        /// Creates the maintenance window.
        /// </summary>
        /// <param name="tableDefinition">The table definition.</param>
        /// <param name="addViewArgs">The add view arguments.</param>
        /// <param name="inputParameter">The input parameter.</param>
        /// <returns>DbMaintenanceWindow.</returns>
        protected virtual DbMaintenanceWindow CreateMaintenanceWindow(
            TableDefinitionBase tableDefinition
            , LookupAddViewArgs addViewArgs
            , object inputParameter)
        {
            return null;
        }

        /// <summary>
        /// Shows the add on the fly window.
        /// </summary>
        /// <param name="maintenanceWindow">The maintenance window.</param>
        /// <param name="tableDefinition">The table definition.</param>
        /// <param name="addViewArgs">The add view arguments.</param>
        /// <param name="addViewParameter">The add view parameter.</param>
        protected virtual void ShowAddOnTheFlyWindow(
            DbMaintenanceWindow maintenanceWindow
            , TableDefinitionBase tableDefinition
            , LookupAddViewArgs addViewArgs = null
            , object addViewParameter = null)
        {
            if (addViewArgs != null && addViewArgs.OwnerWindow is Window ownerWindow)
                maintenanceWindow.Owner = ownerWindow;
            else
            {
                maintenanceWindow.Owner = WPFControlsGlobals.ActiveWindow;
            }

            maintenanceWindow.Closed += (sender, args) =>
            {
                maintenanceWindow.Owner.Activate();
            };

            maintenanceWindow.ShowInTaskbar = false;
            maintenanceWindow.Processor.InitializeFromLookupData(addViewArgs);
            maintenanceWindow.Show();
        }

        public override void ShowDialog(TableDefinitionBase tableDefinition)
        {
            var item = GetDbMaintenanceWindow(tableDefinition);
            if (item != null)
            {
                var maintenanceWindow = Activator.CreateInstance(item.MaintenanceWindow) as DbMaintenanceWindow;
                maintenanceWindow.Owner = WPFControlsGlobals.ActiveWindow;
                maintenanceWindow.Closed += (sender, args) => { maintenanceWindow.Owner.Activate(); };
                maintenanceWindow.ShowInTaskbar = false;
                maintenanceWindow.ShowDialog();

            }
        }

        /// <summary>
        /// Shows the database maintenance window.
        /// </summary>
        /// <param name="tableDefinition">The table definition.</param>
        /// <param name="ownerWindow">The owner window.</param>
        public virtual void ShowDbMaintenanceWindow(TableDefinitionBase tableDefinition, Window ownerWindow = null)
        {
            var item = GetDbMaintenanceWindow(tableDefinition);
            if (item != null)
            {
                var maintenanceWindow = Activator.CreateInstance(item.MaintenanceWindow) as DbMaintenanceWindow;
                if (maintenanceWindow != null)
                {
                    maintenanceWindow.Owner = WPFControlsGlobals.ActiveWindow;
                    maintenanceWindow.Closed += (sender, args) => { maintenanceWindow.Owner.Activate(); };

                    maintenanceWindow.ShowInTaskbar = false;
                    maintenanceWindow.Show();
                }
            }
        }
    }
}
