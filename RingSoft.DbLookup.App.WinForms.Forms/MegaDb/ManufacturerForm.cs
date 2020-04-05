using System.Windows.Forms;
using RingSoft.DbLookup.App.Library.MegaDb.ViewModels;
using RingSoft.DbMaintenance;

namespace RingSoft.DbLookup.App.WinForms.Forms.MegaDb
{
    public partial class ManufacturerForm : DbMaintenanceForm
    {
        public override DbMaintenanceViewModelBase ViewModel => _manufacturerViewModel;

        private ManufacturerViewModel _manufacturerViewModel = new ManufacturerViewModel();

        public ManufacturerForm()
        {
            InitializeComponent();

            ManufacturerIdLabel.DataBindings.Add(nameof(ManufacturerIdLabel.Text), _manufacturerViewModel,
                nameof(_manufacturerViewModel.ManufacturerId), false, DataSourceUpdateMode.OnPropertyChanged);

            RegisterFormKeyControl(NameControl);

            ItemsControl.DataBindings.Add(nameof(ItemsControl.LookupDefinition), _manufacturerViewModel,
                nameof(_manufacturerViewModel.ItemsLookupDefinition), true, DataSourceUpdateMode.Never);
            ItemsControl.DataBindings.Add(nameof(ItemsControl.Command), _manufacturerViewModel,
                nameof(_manufacturerViewModel.ItemsLookupCommand), true, DataSourceUpdateMode.OnPropertyChanged);

            AddModifyButton.Click += (sender, args) => _manufacturerViewModel.OnAddModify();
        }

        public override void ResetViewForNewRecord()
        {
            NameControl.Focus();
            base.ResetViewForNewRecord();
        }
    }
}
