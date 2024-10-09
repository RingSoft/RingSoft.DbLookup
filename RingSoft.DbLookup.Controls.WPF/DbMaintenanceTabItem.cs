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
            var dockPanel = new DockPanel();
            Content = dockPanel;
            UserControl = userControl;
            UserControl.Host = this;
            dockPanel.Children.Add(UserControl);

            CloseCommand = new RelayCommand((() =>
            {
                tabControl.Items.Remove(this);
            }));

            PreviewKeyDown += (sender, args) =>
            {
                if (Keyboard.IsKeyDown(Key.LeftCtrl) || Keyboard.IsKeyDown(Key.RightCtrl))
                {
                    if (args.Key == Key.F4)
                    {
                        tabControl.Items.Remove(this);
                    }
                }
            };

        }

        public void CloseHost()
        {
            CloseCommand.Execute(null);
        }
    }
}
