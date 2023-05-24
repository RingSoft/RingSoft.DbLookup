using RingSoft.DbLookup.App.Library;
using RingSoft.DbLookup.Controls.WPF;
using RingSoft.DbLookup.ModelDefinition.FieldDefinitions;
using RingSoft.DbMaintenance;
using System.Windows.Controls.Primitives;

namespace RingSoft.DbLookup.App.WPFCore.MegaDb
{
    /// <summary>
    /// Interaction logic for ManufacturerWindow.xaml
    /// </summary>
    public partial class ManufacturerWindow
    {
        public override DbMaintenanceViewModelBase ViewModel => ManufacturerViewModel;
        public override DbMaintenanceButtonsControl MaintenanceButtonsControl => ButtonsControl;
        public override DbMaintenanceStatusBar DbStatusBar => StatusBar;

        public ManufacturerWindow()
        {
            InitializeComponent();

            AddModifyButton.Click += (sender, args) => ManufacturerViewModel.OnAddModify();

            Initialize();
            RegisterFormKeyControl(NameControl);
        }

        public override void ResetViewForNewRecord()
        {
            NameControl.Focus();
            base.ResetViewForNewRecord();
        }

        public override void OnValidationFail(FieldDefinition fieldDefinition, string text, string caption)
        {
            var table = RsDbLookupAppGlobals.EfProcessor.MegaDbLookupContext.Manufacturers;
            if (fieldDefinition == table.GetFieldDefinition(p => p.Name))
                NameControl.Focus();

            base.OnValidationFail(fieldDefinition, text, caption);
        }
    }
}
