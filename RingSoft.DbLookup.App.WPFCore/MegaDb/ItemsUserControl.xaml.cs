using System.Windows.Controls;
using RingSoft.DbLookup.Controls.WPF;
using RingSoft.DbMaintenance;

namespace RingSoft.DbLookup.App.WPFCore.MegaDb
{
    /// <summary>
    /// Interaction logic for ItemsUserConrol.xaml
    /// </summary>
    public partial class ItemsUserControl
    {
        public ItemsUserControl()
        {
            InitializeComponent();
            RegisterFormKeyControl(NameControl);
        }

        protected override DbMaintenanceViewModelBase OnGetViewModel()
        {
            return ItemsViewModel;
        }

        protected override Control OnGetMaintenanceButtons()
        {
            return ButtonsControl;
        }

        protected override DbMaintenanceStatusBar OnGetStatusBar()
        {
            return StatusBar;
        }

        protected override string GetTitle()
        {
            return "Item";
        }
    }
}
