using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using RingSoft.DataEntryControls.Engine;
using RingSoft.DataEntryControls.WPF;

namespace RingSoft.DbLookup.Controls.WPF
{
    public class WindowMenu : MenuItem
    {
        private RelayCommand<TabItem> _tabCommand;

        private RelayCommand _closeAllTabs;
        public WindowMenu()
        {
            Header = "_Window";
            Items.Add(new MenuItem()
            {
                Header = "Test",
            });

            _tabCommand = new RelayCommand<TabItem>(ShowTab);

            _closeAllTabs = new RelayCommand((() =>
            {
                LookupControlsGlobals.TabControl.CloseAllTabs();
            }));
        }

        protected override void OnSubmenuOpened(RoutedEventArgs e)
        {
            Items.Clear();

            foreach (var priority in LookupControlsGlobals.TabControl.TabOrder.TabPriorities)
            {
                Items.Add(new MenuItem()
                {
                    Header = priority.TabItem.Header,
                    Command = _tabCommand,
                    CommandParameter = priority.TabItem,
                });
            }

            Items.Add(new Separator());

            Items.Add(new MenuItem()
            {
                Header = "_Close All Tabs",
                Command = _closeAllTabs,
            });
            base.OnSubmenuOpened(e);
        }

        private void ShowTab(TabItem item)
        {
            LookupControlsGlobals.TabControl.SelectedItem = item;
        }
    }
}
