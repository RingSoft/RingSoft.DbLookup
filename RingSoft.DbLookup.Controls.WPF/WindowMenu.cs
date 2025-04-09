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
        private int _index = 0;

        public WindowMenu()
        {
            Header = "_Window";
            Items.Add(new MenuItem()
            {
                Header = "Test",
            });
        }

        protected override void OnSubmenuOpened(RoutedEventArgs e)
        {
            Items.Clear();
            Items.Add(new MenuItem()
            {
                Header = "Test1",
            });

            if (_index > 0)
            {
                Items.Add(new MenuItem()
                {
                    Header = "Test2",
                });
            }

            _index++;

            base.OnSubmenuOpened(e);
        }
    }
}
