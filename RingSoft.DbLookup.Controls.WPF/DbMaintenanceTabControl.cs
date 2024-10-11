using System.Windows.Controls;
using RingSoft.DbLookup.Lookup;
using RingSoft.DbLookup.ModelDefinition;

namespace RingSoft.DbLookup.Controls.WPF
{
    public class DbMaintenanceTabControl : TabControl, ILookupAddViewDestination
    {
        public void ShowAddView(LookupAddViewArgs addViewArgs = null, object inputParameter = null)
        {
            if (LookupControlsGlobals.WindowRegistry.IsTableRegistered(addViewArgs.LookupData.LookupDefinition.TableDefinition))
            {
                var ucControl = LookupControlsGlobals.WindowRegistry.GetUserControl(
                    addViewArgs.LookupData.LookupDefinition.TableDefinition);
                if (ucControl != null)
                {
                    ucControl.LookupAddViewArgs = addViewArgs;
                    ucControl.AddViewParameter = inputParameter;
                    var tabItem = new DbMaintenanceTabItem(
                        ucControl, this);
                    Items.Insert(0, tabItem);
                    tabItem.IsSelected = true;
                    ucControl.Focus();
                    ucControl.SetInitialFocus();
                }
            }
        }

        public DbMaintenanceUserControl ShowTableControl(TableDefinitionBase tableDefinition)
        {
            DbMaintenanceUserControl result = null;
            if (LookupControlsGlobals.WindowRegistry.IsTableRegistered(tableDefinition))
            {
                var ucControl = LookupControlsGlobals.WindowRegistry.GetUserControl(
                    tableDefinition);
                if (ucControl != null)
                {
                    result = ucControl;
                    var tabItem = new DbMaintenanceTabItem(
                        ucControl, this);
                    Items.Insert(0, tabItem);
                    tabItem.IsSelected = true;
                    ucControl.Focus();
                    ucControl.SetInitialFocus();
                }
            }

            return result;
        }
    }
}
