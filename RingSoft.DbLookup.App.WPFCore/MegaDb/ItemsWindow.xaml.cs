using RingSoft.DbLookup.App.Library;
using RingSoft.DbLookup.ModelDefinition.FieldDefinitions;
using RingSoft.DbMaintenance;

namespace RingSoft.DbLookup.App.WPFCore.MegaDb
{
    /// <summary>
    /// Interaction logic for ItemsWindow.xaml
    /// </summary>
    public partial class ItemsWindow
    {
        public override DbMaintenanceViewModelBase ViewModel => ItemsViewModel;
        public override DbMaintenanceButtonsControl MaintenanceButtonsControl => ButtonsControl;

        public ItemsWindow()
        {
            InitializeComponent();

            Initialize();

            RegisterFormKeyControl(NameControl);
        }

        public override void OnValidationFail(FieldDefinition fieldDefinition, string text, string caption)
        {
            var table = RsDbLookupAppGlobals.EfProcessor.MegaDbLookupContext.Items;

            if (fieldDefinition == table.GetFieldDefinition(p => p.Name))
                NameControl.Focus();
            else if (fieldDefinition == table.GetFieldDefinition(p => p.LocationId))
                LocationControl.Focus();
            else if (fieldDefinition == table.GetFieldDefinition(p => p.ManufacturerId))
                ManufacturerControl.Focus();

            base.OnValidationFail(fieldDefinition, text, caption);
        }

        public override void ResetViewForNewRecord()
        {
            NameControl.Focus();
            base.ResetViewForNewRecord();
        }
    }
}
