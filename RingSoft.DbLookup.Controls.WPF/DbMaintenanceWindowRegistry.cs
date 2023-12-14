using RingSoft.DbLookup.Lookup;
using RingSoft.DbLookup.ModelDefinition;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;

namespace RingSoft.DbLookup.Controls.WPF
{
    public class DbMaintenanceWindowRegistry : DbLookupTableWindowRegistry
    {
        public class WindowRegistryItem
        {
            public TableDefinitionBase TableDefinition { get; set; }

            public Type MaintenanceWindow { get; set; }
        }

        public static List<WindowRegistryItem> Items { get; private set; } = new List<WindowRegistryItem>();

        public override void ActivateRegistry()
        {
            LookupControlsGlobals.WindowRegistry = this;
            base.ActivateRegistry();
        }

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

        private WindowRegistryItem GetDbMaintenanceWindow(TableDefinitionBase tableDefinition)
        {
            var item = Items.FirstOrDefault(p => p.TableDefinition == tableDefinition);
            return item;
        }

        public override bool IsTableRegistered(TableDefinitionBase tableDefinition)
        {
            if (GetDbMaintenanceWindow(tableDefinition) == null)
            {
                return false;
            }
            return true;
        }

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
                    if (maintenanceWindow.ViewModel.TableDefinitionBase != tableDefinition)
                    {
                        throw new Exception("Invalid Table Definition or DbMaintenance Window");
                    }
                }
            }
            ShowAddOnTheFlyWindow(maintenanceWindow, tableDefinition, addViewArgs, inputParameter);
        }

        protected virtual DbMaintenanceWindow CreateMaintenanceWindow(
            TableDefinitionBase tableDefinition
            , LookupAddViewArgs addViewArgs
            , object inputParameter)
        {
            return null;
        }

        protected virtual void ShowAddOnTheFlyWindow(
            DbMaintenanceWindow maintenanceWindow
            , TableDefinitionBase tableDefinition
            , LookupAddViewArgs addViewArgs = null
            , object addViewParameter = null)
        {
            if (addViewArgs.OwnerWindow is Window ownerWindow)
                maintenanceWindow.Owner = ownerWindow;

            maintenanceWindow.ShowInTaskbar = false;
            maintenanceWindow.Processor.InitializeFromLookupData(addViewArgs);
            maintenanceWindow.ShowDialog();
        }

        public virtual void ShowDbMaintenanceWindow(TableDefinitionBase tableDefinition, Window ownerWindow = null)
        {
            var item = GetDbMaintenanceWindow(tableDefinition);
            if (item != null)
            {
                var maintenanceWindow = Activator.CreateInstance(item.MaintenanceWindow) as DbMaintenanceWindow;
                if (ownerWindow != null)
                {
                    maintenanceWindow.Owner = ownerWindow;
                }

                maintenanceWindow.ShowInTaskbar = false;
                maintenanceWindow.ShowDialog();
            }
        }
        }
}
