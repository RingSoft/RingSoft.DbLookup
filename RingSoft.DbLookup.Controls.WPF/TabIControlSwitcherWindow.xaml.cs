using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Windows.Controls;
using System.Windows.Input;
using RingSoft.DataEntryControls.WPF;

namespace RingSoft.DbLookup.Controls.WPF
{
    public class ListBoxTabItem
    {
        public string Text { get; set; }

        public TabItem TabItem { get; set; }
    }

    public class TabSwitcherViewModel : INotifyPropertyChanged
    {
        private ObservableCollection<ListBoxTabItem> _listBoxItems;

        public ObservableCollection<ListBoxTabItem> Items
        {
            get { return _listBoxItems; }
            set
            {
                if (_listBoxItems == value)
                {
                    return;
                }
                _listBoxItems = value;
                OnPropertyChanged();
            }
        }

        private ListBoxTabItem _selectedItem;

        public ListBoxTabItem SelectedItem
        {
            get => _selectedItem;
            set
            {
                if (_selectedItem == value)
                    return;

                _selectedItem = value;

                OnPropertyChanged();
            }
        }

        public void Initialize(DbMaintenanceTabControl tabControl)
        {
            Items = new ObservableCollection<ListBoxTabItem>();

            foreach (var tabPriority in tabControl.TabOrder.TabPriorities)
            {
                Items.Add(new ListBoxTabItem()
                {
                    TabItem = tabPriority.TabItem,
                    Text = tabPriority.TabItem.Header.ToString(),
                });
            }

            if (Items.Count > 1)
            {
                SelectedItem = Items[1];
            }
            else if (Items.Count > 0)
            {
                SelectedItem = Items[0];
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;

        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        protected bool SetField<T>(ref T field, T value, [CallerMemberName] string propertyName = null)
        {
            if (EqualityComparer<T>.Default.Equals(field, value)) return false;
            field = value;
            OnPropertyChanged(propertyName);
            return true;
        }
    }
    /// <summary>
    /// Interaction logic for TabIControlSwitcherWindow.xaml
    /// </summary>
    public partial class TabIControlSwitcherWindow
    {
        private DbMaintenanceTabControl _tabControl;
        public TabIControlSwitcherWindow(DbMaintenanceTabControl tabControl)
        {
            InitializeComponent();
            _tabControl = tabControl;

            Loaded += (sender, args) =>
            {
                LocalViewModel.Initialize(_tabControl);
                PreviewKeyUp += (sender, args) =>
                {
                    if (args.Key == Key.LeftCtrl || args.Key == Key.RightCtrl)
                    {
                        Close();
                    }
                };
            };
            _tabControl = tabControl;
        }

        protected override void OnPreviewKeyDown(KeyEventArgs e)
        {
            if (e.Key == Key.Tab)
            {
                var index = LocalViewModel.Items.IndexOf(LocalViewModel.SelectedItem);
                if (index >= LocalViewModel.Items.Count - 1)
                {
                    LocalViewModel.SelectedItem = LocalViewModel.Items.FirstOrDefault();
                }
                else
                {
                    LocalViewModel.SelectedItem = LocalViewModel.Items[index + 1];
                }
                e.Handled = true;
            }
            base.OnPreviewKeyDown(e);
        }
    }
}
