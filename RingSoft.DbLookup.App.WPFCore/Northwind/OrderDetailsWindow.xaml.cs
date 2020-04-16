using RingSoft.DbLookup.App.Library;
using RingSoft.DbLookup.ModelDefinition.FieldDefinitions;
using RingSoft.DbMaintenance;

namespace RingSoft.DbLookup.App.WPFCore.Northwind
{
    /// <summary>
    /// Interaction logic for OrderDetailsWindow.xaml
    /// </summary>
    public partial class OrderDetailsWindow
    {
        public override DbMaintenanceViewModelBase ViewModel => OrderDetailsViewModel;
        public override DbMaintenanceButtonsControl MaintenanceButtonsControl => ButtonsControl;

        public OrderDetailsWindow()
        {
            InitializeComponent();

            Initialize();

            ProductControl.LostFocus += (sender, args) => OrderDetailsViewModel.OnKeyControlLeave();
        }

        public override void ResetViewForNewRecord()
        {
            ProductControl.Focus();
            base.ResetViewForNewRecord();
        }

        public override void OnValidationFail(FieldDefinition fieldDefinition, string text, string caption)
        {
            var table = RsDbLookupAppGlobals.EfProcessor.NorthwindLookupContext.OrderDetails;
            if (fieldDefinition == table.GetFieldDefinition(p => p.ProductID))
                ProductControl.Focus();
            base.OnValidationFail(fieldDefinition, text, caption);
        }
    }
}
