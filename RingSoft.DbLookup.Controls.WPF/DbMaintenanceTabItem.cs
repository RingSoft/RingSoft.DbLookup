﻿using RingSoft.DataEntryControls.Engine;
using System.ComponentModel;
using System.Windows.Controls;
using System.Windows.Input;

namespace RingSoft.DbLookup.Controls.WPF
{
    public class DbMaintenanceTabItem : UserControlTabItem, IUserControlHost
    {
        public new DbMaintenanceUserControl UserControl { get; }

        public DbMaintenanceTabItem(DbMaintenanceUserControl userControl, TabControl tabControl)
            : base(userControl, userControl.Title, tabControl)
        {
            UserControl = userControl;
            UserControl.Host = this;
        }

        public override bool CloseTab(TabControl tabControl)
        {
            if (!CheckClose()) 
                return false;
            return base.CloseTab(tabControl);
        }

        protected override void OnKeyDown(KeyEventArgs e)
        {
            base.OnKeyDown(e);
            if (e.Key == Key.Tab)
            {
                var focusedElement = FocusManager.GetFocusedElement(this);
                if (focusedElement == null)
                {
                    UserControl.SetInitialFocus();
                }
            }
        }

        private bool CheckClose()
        {
            var closingArgs = new CancelEventArgs();
            UserControl.ViewModel.OnWindowClosing(closingArgs);
            if (closingArgs.Cancel)
            {
                return false;
            }

            return true;
        }

        public void CloseHost()
        {
            CloseCommand.Execute(null);
        }

        public void ChangeTitle(string title)
        {
            Header = title;
        }
    }
}
