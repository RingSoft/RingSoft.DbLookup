// ***********************************************************************
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

        public class UserControlRegistryItem
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
            public Type MaintenanceUserControl { get; set; }
        }

        /// <summary>
        /// Gets the items.
        /// </summary>
        /// <value>The items.</value>
        public static List<WindowRegistryItem> Items { get; private set; } = new List<WindowRegistryItem>();
        public static List<UserControlRegistryItem> UcItems { get; private set; } = new List<UserControlRegistryItem>();

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

        public void RegisterUserControl<TUserControl>(TableDefinitionBase tableDefinition) where TUserControl : DbMaintenanceUserControl, new()
        {
            if (tableDefinition == null)
            {
                throw new ArgumentException(nameof(tableDefinition));
            }
            UcItems.Add(new UserControlRegistryItem()
            {
                MaintenanceUserControl = typeof(TUserControl),
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

        public void RegisterUserControl<TUserControl, TEntity>()
            where TUserControl : DbMaintenanceUserControl, new()
            where TEntity : class, new()
        {
            var tableDefinition = GblMethods.GetTableDefinition<TEntity>();
            RegisterUserControl<TUserControl>(tableDefinition);
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

        private UserControlRegistryItem GetDbUserControl(TableDefinitionBase tableDefinition)
        {
            var item = UcItems.FirstOrDefault(p => p.TableDefinition == tableDefinition);
            return item;
        }

        public DbMaintenanceUserControl GetUserControl(TableDefinitionBase tableDefinition)
        {
            var item = GetDbUserControl(tableDefinition);
            if (item != null)
            {
                var userControl = Activator.CreateInstance(item.MaintenanceUserControl) as DbMaintenanceUserControl;
                return userControl;
            }
            return null;
        }

        /// <summary>
        /// Determines whether [is table registered] [the specified table definition].
        /// </summary>
        /// <param name="tableDefinition">The table definition.</param>
        /// <returns><c>true</c> if [is table registered] [the specified table definition]; otherwise, <c>false</c>.</returns>
        public override bool IsTableRegistered(TableDefinitionBase tableDefinition)
        {
            if (GetDbUserControl(tableDefinition) != null)
            {
                return true;
            }
            if (GetDbMaintenanceWindow(tableDefinition) == null)
            {
                return false;
            }
            return true;
        }

        //Peter Ringering - 12/13/2024 01:19:55 PM - E-68
        public override bool IsControlRegistered(TableDefinitionBase tableDefinition)
        {
            if (GetDbUserControl(tableDefinition) != null)
            {
                return true;
            }

            return false;
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
            var Ucitem = GetDbUserControl(tableDefinition);
            if (Ucitem != null)
            {
                var userControl = Activator.CreateInstance(Ucitem.MaintenanceUserControl) as DbMaintenanceUserControl;
                ShowAddOnTheFlyWindow(userControl, tableDefinition, addViewArgs, inputParameter);
                return;
            }

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

        public override void ShowNewAddOnTheFly(TableDefinitionBase tableDefinition, PrimaryKeyValue parentPrimaryKeyValue = null, string initialText = "", object inputParameter = null)
        {
            var lookupData = tableDefinition.LookupDefinition
                .GetLookupDataMaui(tableDefinition.LookupDefinition, true);

            var args = new LookupAddViewArgs(lookupData, true, LookupFormModes.Add,
                initialText, null)
            {
                ParentWindowPrimaryKeyValue = parentPrimaryKeyValue,
                InputParameter = inputParameter,
            };
            ShowAddOntheFlyWindow(tableDefinition, args, inputParameter);
        }

        public override void ShowEditAddOnTheFly(PrimaryKeyValue primaryKey, object inputParameter = null, LookupAddViewArgs lookupAvArgs = null)
        {
            var lookupData = primaryKey.TableDefinition.LookupDefinition
                .GetLookupDataMaui(primaryKey.TableDefinition.LookupDefinition, true);

            var args = new LookupAddViewArgs(lookupData, true, LookupFormModes.View,
                string.Empty, null)
            {
                SelectedPrimaryKeyValue = primaryKey,
                InputParameter = inputParameter,
            };
            args.LookupData.SelectedPrimaryKeyValue = primaryKey;
            if (lookupAvArgs != null)
            {
                args.CallBackToken = lookupAvArgs.CallBackToken;
            }
            ShowAddOntheFlyWindow(primaryKey.TableDefinition, args, inputParameter);

        }

        /// <summary>
        /// Shows the window.
        /// </summary>
        /// <param name="tableDefinition">The table definition.</param>
        public override void ShowWindow(TableDefinitionBase tableDefinition, object inputParameter = null)
        {
            ShowDbMaintenanceWindow(tableDefinition, inputParameter);
        }

        public void ShowWindow(Window window)
        {
            window.Owner = WPFControlsGlobals.ActiveWindow;
            window.Closed += (sender, args) => { window.Owner.Activate(); };
            window.ShowInTaskbar = false;
            window.Show();
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
            //Peter Ringering - 11/23/2024 10:51:37 AM - E-71
            if (addViewArgs != null && addViewArgs.OwnerWindow is Window ownerWindow)
                maintenanceWindow.Owner = ownerWindow;
            else
            {
                maintenanceWindow.Owner = WPFControlsGlobals.ActiveWindow;
            }

            //maintenanceWindow.Closed += (sender, args) =>
            //{
            //    maintenanceWindow.Owner.Activate();
            //};

            maintenanceWindow.ShowInTaskbar = false;
            maintenanceWindow.Processor.InitializeFromLookupData(addViewArgs);
            maintenanceWindow.ViewModel.InputParameter = addViewParameter;
            maintenanceWindow.ShowDialog();
        }

        /// <summary>
        /// Shows the add on the fly window.
        /// </summary>
        /// <param name="maintenanceUserControl">The maintenance window.</param>
        /// <param name="tableDefinition">The table definition.</param>
        /// <param name="addViewArgs">The add view arguments.</param>
        /// <param name="addViewParameter">The add view parameter.</param>
        protected virtual void ShowAddOnTheFlyWindow(
            DbMaintenanceUserControl maintenanceUserControl
            , TableDefinitionBase tableDefinition
            , LookupAddViewArgs addViewArgs = null
            , object addViewParameter = null)
        {
            var win = GetMaintenanceWindow(maintenanceUserControl);

            //Peter Ringering - 11/23/2024 10:59:03 AM - E-71
            if (addViewArgs != null && addViewArgs.OwnerWindow is Window ownerWindow)
                win.Owner = ownerWindow;
            else
            {
                win.Owner = WPFControlsGlobals.ActiveWindow;
            }

            //win.Closed += (sender, args) =>
            //{
            //    win.Owner.Activate();
            //};

            win.ShowInTaskbar = false;
            maintenanceUserControl.LookupAddViewArgs = addViewArgs;
            maintenanceUserControl.AddViewParameter = addViewParameter;
            win.ShowDialog();
        }

        public DbMaintenanceUcWindow GetMaintenanceWindow(DbMaintenanceUserControl userControl)
        {
            var result = new DbMaintenanceUcWindow(userControl);
            userControl.Host = result;
            result.DockPanel.Children.Add(userControl);
            return result;
        }

        public override void ShowDialog(TableDefinitionBase tableDefinition, object inputParameter = null)
        {
            var ucItem = GetDbUserControl(tableDefinition);
            if (ucItem != null)
            {
                var userControl = Activator.CreateInstance(ucItem.MaintenanceUserControl) as DbMaintenanceUserControl;
                if (userControl != null)
                {
                    var win = GetMaintenanceWindow(userControl);
                    if (win != null)
                    {
                        //Peter Ringering - 11/23/2024 11:24:34 AM - E-71
                        win.Owner = WPFControlsGlobals.ActiveWindow;
                        //win.Closed += (sender, args) => { win.Owner.Activate(); };
                        userControl.AddViewParameter = inputParameter;
                        win.ShowInTaskbar = false;
                        win.ShowDialog();
                        return;
                    }
                }
            }

            var item = GetDbMaintenanceWindow(tableDefinition);
            if (item != null)
            {
                var maintenanceWindow = Activator.CreateInstance(item.MaintenanceWindow) as DbMaintenanceWindow;

                //Peter Ringering - 11/23/2024 11:24:34 AM - E-71
                maintenanceWindow.Owner = WPFControlsGlobals.ActiveWindow;
                //maintenanceWindow.Closed += (sender, args) => { maintenanceWindow.Owner.Activate(); };
                maintenanceWindow.ShowInTaskbar = false;
                maintenanceWindow.ViewModel.InputParameter = inputParameter;
                maintenanceWindow.ShowDialog();

            }
        }

        public void ShowDialog(Window window)
        {
            //Peter Ringering - 11/23/2024 11:15:19 AM - E-71
            window.Owner = WPFControlsGlobals.ActiveWindow;
            //window.Closed += (sender, args) => { window.Owner.Activate(); };

            window.ShowInTaskbar = false;
            window.ShowDialog();
        }


        /// <summary>
        /// Shows the database maintenance window.
        /// </summary>
        /// <param name="tableDefinition">The table definition.</param>
        /// <param name="ownerWindow">The owner window.</param>
        public virtual void ShowDbMaintenanceWindow(TableDefinitionBase tableDefinition, object inputParameter = null)
        {
            var ucItem = GetDbUserControl(tableDefinition);
            if (ucItem != null)
            {
                var userControl = Activator.CreateInstance(ucItem.MaintenanceUserControl) as DbMaintenanceUserControl;
                if (userControl != null)
                {
                    var win = GetMaintenanceWindow(userControl);
                    if (win != null)
                    {
                        win.Owner = WPFControlsGlobals.ActiveWindow;
                        win.Closed += (sender, args) => { win.Owner.Activate(); };
                        userControl.AddViewParameter = inputParameter;
                        win.ShowInTaskbar = false;
                        win.Show();
                        return;
                    }
                }
            }
            var item = GetDbMaintenanceWindow(tableDefinition);
            if (item != null)
            {
                var maintenanceWindow = Activator.CreateInstance(item.MaintenanceWindow) as DbMaintenanceWindow;
                if (maintenanceWindow != null)
                {
                    maintenanceWindow.ViewModel.InputParameter = inputParameter;
                    ShowWindow(maintenanceWindow);
                }
            }
        }
    }
}
