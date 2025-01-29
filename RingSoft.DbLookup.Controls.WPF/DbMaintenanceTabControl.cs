using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using RingSoft.DataEntryControls.WPF;
using RingSoft.DbLookup.Lookup;
using RingSoft.DbLookup.ModelDefinition;

namespace RingSoft.DbLookup.Controls.WPF
{
    public class DbMaintenanceTabPriority
    {
        public TabItem TabItem { get; }

        public DbMaintenanceTabPriority(TabItem tabItem)
        {
            TabItem = tabItem;
        }

        public override string ToString()
        {
            return $"{TabItem.Header}";
        }
    }
    public class DbMaintenanceTabOrder
    {
        public IReadOnlyList<DbMaintenanceTabPriority> TabPriorities { get; }

        private List<DbMaintenanceTabPriority> _tabPriorities = new List<DbMaintenanceTabPriority>();

        public DbMaintenanceTabOrder()
        {
            TabPriorities = _tabPriorities.AsReadOnly();
        }

        public void AddTabItem(TabItem tabItem, bool selectTab)
        {
            var tabPriority = new DbMaintenanceTabPriority(tabItem);
            if (selectTab)
            {
                _tabPriorities.Insert(0, tabPriority);
            }
            else
            {
                _tabPriorities.Add(tabPriority);
            }
        }

        public void SelectTabItem(TabItem tabItem)
        {
            var tabPriority = _tabPriorities.FirstOrDefault(p => p.TabItem == tabItem);
            if (tabPriority != null)
            {
                _tabPriorities.Remove(tabPriority);
                _tabPriorities.Insert(0, tabPriority);
            }
        }

        public void DeleteTabItem(TabItem tabItem)
        {
            var tabPriority = _tabPriorities.FirstOrDefault(p => p.TabItem == tabItem);
            if (tabPriority != null)
            {
                _tabPriorities.Remove(tabPriority);
            }
        }
    }
    public class DbMaintenanceTabControl : TabControl, ILookupAddViewDestination
    {
        public bool SetDestionationAsFirstTab { get; set; } = true;

        public DbMaintenanceTabOrder TabOrder { get; } = new DbMaintenanceTabOrder();

        protected override void OnSelectionChanged(SelectionChangedEventArgs e)
        {
            var item = SelectedItem;
            TabOrder.SelectTabItem(item as TabItem);
            base.OnSelectionChanged(e);
            item = SelectedItem;
        }

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
                    var setAsFirstTab = SetDestionationAsFirstTab;

                    ShowTabItem(tabItem, setAsFirstTab, true);

                    tabItem.IsSelected = true;
                    ucControl.Focus();
                    ucControl.SetInitialFocus();
                }
            }
        }

        public void ShowAddView(PrimaryKeyValue primaryKey, object inputParameter = null)
        {
            var lookupData = primaryKey.TableDefinition.LookupDefinition
                .GetLookupDataMaui(primaryKey.TableDefinition.LookupDefinition, true);

            var args = new LookupAddViewArgs(lookupData, true, LookupFormModes.View,
                string.Empty, Window.GetWindow(this))
            {
                SelectedPrimaryKeyValue = primaryKey,
                InputParameter = inputParameter,
                
            };
            args.LookupData.SelectedPrimaryKeyValue = primaryKey;
            ShowAddView(args, inputParameter);

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
            
                    ShowTabItem(tabItem, setAsFirstTab, true);

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

            ShowTabItem(tabItem, setAsFirstTab, selectTab);

            if (selectTab)
            {
                tabItem.IsSelected = true;
                userControl.Focus();
            }

        }

        private void ShowTabItem(TabItem tabItem, bool setAsFirstTab, bool selectTab)
        {
            if (setAsFirstTab)
            {
                Items.Insert(0, tabItem);
            }
            else
            {
                Items.Add(tabItem);
            }
            TabOrder.AddTabItem(tabItem, selectTab);
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
                if (!ucTabItem.CloseTab())
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

        protected override void OnKeyDown(KeyEventArgs e)
        {
            if (e.Key == Key.Tab)
            {
                if (Keyboard.IsKeyDown(Key.LeftCtrl) || Keyboard.IsKeyDown(Key.RightCtrl))
                {
                    return;
                }
            }
            base.OnKeyDown(e);
        }
    }
}
