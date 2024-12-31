using System.Windows;
using System.Windows.Input;
using RingSoft.DataEntryControls.WPF;

namespace RingSoft.DbLookup.Controls.WPF
{
    public abstract class MainTabWindow : Window
    {
        public DbMaintenanceTabControl TabControl { get; private set; }

        public MainTabWindow()
        {
            Loaded += (sender, args) =>
            {
                TabControl = GetTabControl();
            };
        }
        protected abstract DbMaintenanceTabControl GetTabControl();

        protected override void OnPreviewKeyDown(KeyEventArgs e)
        {
            if (TabControl.Items.Count == 0)
            {
                return;
            }
            if (Keyboard.IsKeyDown(Key.LeftCtrl) || Keyboard.IsKeyDown(Key.RightCtrl))
            {
                if (e.Key == Key.Tab)
                {
                    if (TabControl.SelectedItem is DbMaintenanceTabItem origTabItem)
                    {
                        origTabItem.UserControl.IgnoreTab = true;
                    }
                    var taskSwitcherWindow = new TabIControlSwitcherWindow(TabControl);
                    taskSwitcherWindow.ShowDialog();
                    taskSwitcherWindow.LocalViewModel.SelectedItem.TabItem.IsSelected = true;
                    if (taskSwitcherWindow.LocalViewModel.SelectedItem.TabItem is DbMaintenanceTabItem dbMaintenanceTabItem)
                    {
                        //TabControl.UpdateLayout();
                        //dbMaintenanceTabItem.UpdateLayout();
                        //Peter Ringering - 12/11/2024 11:20:23 AM - E-77
                        e.Handled = true;
                        //dbMaintenanceTabItem.UserControl.SetInitialFocus();
                    }
                    return;
                }
            }
            base.OnPreviewKeyDown(e);
        }
    }
}
