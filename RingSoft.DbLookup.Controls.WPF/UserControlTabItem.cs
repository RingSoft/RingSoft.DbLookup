using System.Windows.Controls;
using System.Windows.Input;
using RingSoft.DataEntryControls.Engine;
using RingSoft.DataEntryControls.WPF;

namespace RingSoft.DbLookup.Controls.WPF
{
    public class UserControlTabItem : TabItem
    {
        public BaseUserControl UserControl { get; }

        public RelayCommand CloseCommand { get; }

        public UserControlTabItem(BaseUserControl userControl, string header, TabControl tabControl)
        {
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

            UserControl = userControl;

            CloseCommand = new RelayCommand((() =>
            {
                CloseTab(tabControl);
            }));

            Header = header;
            var dockPanel = new DockPanel();
            Content = dockPanel;
            dockPanel.Children.Add(userControl);
        }

        public virtual bool CloseTab(TabControl tabControl)
        {
            tabControl.Items.Remove(this);
            return true;
        }

    }
}
