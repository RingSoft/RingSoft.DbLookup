﻿using System.Windows.Controls;
using System.Windows.Input;
using RingSoft.DataEntryControls.Engine;
using RingSoft.DataEntryControls.WPF;

namespace RingSoft.DbLookup.Controls.WPF
{
    public class UserControlTabItem : TabItem
    {
        public BaseUserControl UserControl { get; }

        public RelayCommand CloseCommand { get; }

        private DbMaintenanceTabControl _tabControl;

        public UserControlTabItem(BaseUserControl userControl, string header, DbMaintenanceTabControl tabControl)
        {
            _tabControl = tabControl;
            PreviewKeyDown += (sender, args) =>
            {
                if (Keyboard.IsKeyDown(Key.LeftCtrl) || Keyboard.IsKeyDown(Key.RightCtrl))
                {
                    if (Keyboard.IsKeyDown(Key.F4))
                    {
                        args.Handled = true;  //Peter Ringering - 01/14/2025 03:19:23 PM - E-111
                        CloseCommand.Execute(null);
                    }
                }
            };

            UserControl = userControl;

            CloseCommand = new RelayCommand((() =>
            {
                CloseTab();
            }));

            Header = header;
            var dockPanel = new DockPanel();
            Content = dockPanel;
            dockPanel.Children.Add(userControl);
        }

        public virtual bool CloseTab()
        {
            _tabControl.TabOrder.DeleteTabItem(this);
            _tabControl.Items.Remove(this);
            return true;
        }

    }
}
