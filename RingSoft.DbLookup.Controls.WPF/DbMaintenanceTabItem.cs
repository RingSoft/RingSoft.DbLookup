using RingSoft.DataEntryControls.Engine;
using System.ComponentModel;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using RingSoft.DataEntryControls.WPF;

namespace RingSoft.DbLookup.Controls.WPF
{
    public class DbMaintenanceTabItem : UserControlTabItem, IUserControlHost
    {
        public new DbMaintenanceUserControl UserControl { get; }

        public HostTypes HostType => HostTypes.Tab;

        public DbMaintenanceTabItem(DbMaintenanceUserControl userControl, DbMaintenanceTabControl tabControl)
            : base(userControl, userControl.Title, tabControl)
        {
            UserControl = userControl;
            UserControl.Host = this;
            IsTabStop = false;
        }

        public override bool CloseTab()
        {
            if (!CheckClose()) 
                return false;
            return base.CloseTab();
        }

        protected override void OnKeyDown(KeyEventArgs e)
        {
            base.OnKeyDown(e);
            if (e.Key == Key.Tab)
            {
                if (Keyboard.IsKeyDown(Key.LeftCtrl) || Keyboard.IsKeyDown(Key.RightCtrl))
                {
                    return;
                }
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
