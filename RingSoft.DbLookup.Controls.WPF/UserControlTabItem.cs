using System.Windows.Controls;
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
