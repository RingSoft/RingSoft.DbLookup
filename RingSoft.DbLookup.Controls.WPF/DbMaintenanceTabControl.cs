using System.Windows.Controls;
using RingSoft.DataEntryControls.WPF;
using RingSoft.DbLookup.Lookup;
using RingSoft.DbLookup.ModelDefinition;

namespace RingSoft.DbLookup.Controls.WPF
{
    public class DbMaintenanceTabControl : TabControl, ILookupAddViewDestination
    {
        public bool SetDestionationAsFirstTab { get; set; } = true;
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
                    if (SetDestionationAsFirstTab)
                    {
                        Items.Insert(0, tabItem);
                    }
                    else
                    {
                        Items.Add(tabItem);
                    }
                    tabItem.IsSelected = true;
                    ucControl.Focus();
                    ucControl.SetInitialFocus();
                }
            }
        }

        public DbMaintenanceUserControl ShowTableControl(
            TableDefinitionBase tableDefinition
            , bool setAsFirstTab = true)
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
                    if (setAsFirstTab)
                    {
                        Items.Insert(0, tabItem);
                    }
                    else
                    {
                        Items.Add(tabItem);
                    }

                    tabItem.IsSelected = true;
                    ucControl.Focus();
                }
            }

            return result;
        }

        public void ShowUserControl(
            BaseUserControl userControl
            , string header
            , bool setAsFirstTab = true
            , bool selectTab = true)
        {
            var tabItem = new UserControlTabItem(userControl, header, this);
            if (setAsFirstTab)
            {
                Items.Insert(0, tabItem);
            }
            else
            {
                Items.Add(tabItem);
            }

            if (selectTab)
            {
                tabItem.IsSelected = true;
                userControl.Focus();
            }
        }

        public bool CloseAllTabs()
        {
            var result = true;
            if (Items.Count == 0)
            {
                return result;
            }

            var tabIndex = 0;
            UserControlTabItem ucTabItem = Items[tabIndex] as UserControlTabItem;
            while (ucTabItem != null)
            {
                ucTabItem.IsSelected = true;
                if (!ucTabItem.CloseTab(this))
                {
                    tabIndex ++;
                    result = false;
                }
                if (Items.Count == tabIndex)
                {
                    break;
                }
                ucTabItem = Items[tabIndex] as UserControlTabItem;
            }
            return result;
        }
    }
}
