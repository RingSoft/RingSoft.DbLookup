using System.Windows.Forms;
using RingSoft.DbLookup.App.Library;
using RingSoft.DbLookup.App.Library.MegaDb.ViewModels;
using RingSoft.DbLookup.ModelDefinition.FieldDefinitions;
using RingSoft.DbMaintenance;

namespace RingSoft.DbLookup.App.WinForms.Forms.MegaDb
{
    public partial class LocationForm : DbMaintenanceForm
    {
        public override DbMaintenanceViewModelBase ViewModel => _locationViewModel;

        private LocationViewModel _locationViewModel = new LocationViewModel();

        public LocationForm()
        {
            InitializeComponent();

            LocationIdLabel.DataBindings.Add(nameof(LocationIdLabel.Text), _locationViewModel,
                nameof(_locationViewModel.LocationId), false, DataSourceUpdateMode.OnPropertyChanged);
            RegisterFormKeyControl(NameControl);

            ItemsControl.DataBindings.Add(nameof(ItemsControl.LookupDefinition), _locationViewModel,
                nameof(_locationViewModel.ItemsLookupDefinition), true, DataSourceUpdateMode.Never);
            ItemsControl.DataBindings.Add(nameof(ItemsControl.Command), _locationViewModel,
                nameof(_locationViewModel.ItemsLookupCommand), true, DataSourceUpdateMode.OnPropertyChanged);

            AddModifyButton.Click += (sender, args) => _locationViewModel.OnAddModify();
        }

        public override void ResetViewForNewRecord()
        {
            NameControl.Focus();
            base.ResetViewForNewRecord();
        }

        public override void OnValidationFail(FieldDefinition fieldDefinition, string text, string caption)
        {
            var table = RsDbLookupAppGlobals.EfProcessor.MegaDbLookupContext.Locations;
            if (fieldDefinition == table.GetFieldDefinition(p => p.Name))
                NameControl.Focus();

            base.OnValidationFail(fieldDefinition, text, caption);
        }
    }
}
