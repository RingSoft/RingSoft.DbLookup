using System.ComponentModel;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using RingSoft.DataEntryControls.Engine;
using RingSoft.DataEntryControls.WPF;

namespace RingSoft.DbLookup.Controls.WPF
{
    public class DbMaintenanceTabItem : TabItem, IUserControlHost
    {
        public DbMaintenanceUserControl UserControl { get; }

        public RelayCommand CloseCommand { get; }
        public DbMaintenanceTabItem(DbMaintenanceUserControl userControl, TabControl tabControl)
        {
            Header = userControl.Title;
            var dockPanel = new DockPanel();
            Content = dockPanel;
            UserControl = userControl;
            UserControl.Host = this;
            dockPanel.Children.Add(UserControl);

            CloseCommand = new RelayCommand((() =>
            {
                if (!CheckClose()) return;
                tabControl.Items.Remove(this);
            }));

            PreviewKeyDown += (sender, args) =>
            {
                if (Keyboard.IsKeyDown(Key.LeftCtrl) || Keyboard.IsKeyDown(Key.RightCtrl))
                {
                    if (Keyboard.IsKeyDown(Key.F4))
                    {
                        CloseCommand.Execute(null);
                    }
                }
            };

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
    }
}
