using RingSoft.DbLookup.App.Library;
using RingSoft.DbLookup.ModelDefinition.FieldDefinitions;
using RingSoft.DbMaintenance;

namespace RingSoft.DbLookup.App.WPFCore.MegaDb
{
    /// <summary>
    /// Interaction logic for LocationWindow.xaml
    /// </summary>
    public partial class LocationWindow
    {
        public override DbMaintenanceViewModelBase ViewModel => LocationViewModel;
        public override DbMaintenanceButtonsControl MaintenanceButtonsControl => ButtonsControl;

        public LocationWindow()
        {
            InitializeComponent();

            RegisterFormKeyControl(NameControl);

            AddModifyButton.Click += (sender, args) => LocationViewModel.OnAddModify();

            Initialize();
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
