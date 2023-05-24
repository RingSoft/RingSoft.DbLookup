using RingSoft.DbLookup.App.Library;
using RingSoft.DbLookup.Controls.WPF;
using RingSoft.DbLookup.ModelDefinition.FieldDefinitions;
using RingSoft.DbMaintenance;
using System.Windows.Controls.Primitives;

namespace RingSoft.DbLookup.App.WPFCore.MegaDb
{
    /// <summary>
    /// Interaction logic for ItemsWindow.xaml
    /// </summary>
    public partial class ItemsWindow
    {
        public override DbMaintenanceViewModelBase ViewModel => ItemsViewModel;
        public override DbMaintenanceButtonsControl MaintenanceButtonsControl => ButtonsControl;
        public override DbMaintenanceStatusBar DbStatusBar => StatusBar;

        public ItemsWindow()
        {
            InitializeComponent();

            Initialize();

            RegisterFormKeyControl(NameControl);

            LocationControl.PreviewLostKeyboardFocus += (sender, args) =>
            {
                if (!ItemsViewModel.LocationLostFocusValidation(this))
                    args.Handled = true;
            };

            ManufacturerControl.PreviewLostKeyboardFocus += (sender, args) =>
            {
                if (!ItemsViewModel.ManufacturerLostFocusValidation(this))
                    args.Handled = true;
            };
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
