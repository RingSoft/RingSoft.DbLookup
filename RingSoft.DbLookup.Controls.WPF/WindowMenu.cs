using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;

namespace RingSoft.DbLookup.Controls.WPF
{
    public class WindowMenu : MenuItem
    {
        private int _index = 0;

        public WindowMenu()
        {
            Header = "_Window";
        }

        protected override void OnClick()
        {
            Items.Clear();
            Items.Add(new MenuItem()
            {
                Header = "Test",
            });

            if (_index > 0)
            {
                Items.Add(new MenuItem()
                {
                    Header = "Test1",
                });
            }

            _index++;
            base.OnClick();
        }
    }
}
