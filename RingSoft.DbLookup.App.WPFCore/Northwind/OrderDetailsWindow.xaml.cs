using RingSoft.DbLookup.App.Library;
using RingSoft.DbLookup.Controls.WPF;
using RingSoft.DbLookup.ModelDefinition.FieldDefinitions;
using RingSoft.DbMaintenance;
using System.Windows.Controls.Primitives;

namespace RingSoft.DbLookup.App.WPFCore.Northwind
{
    /// <summary>
    /// Interaction logic for OrderDetailsWindow.xaml
    /// </summary>
    public partial class OrderDetailsWindow
    {
        public override DbMaintenanceViewModelBase ViewModel => OrderDetailsViewModel;
        public override DbMaintenanceButtonsControl MaintenanceButtonsControl => ButtonsControl;
        public override DbMaintenanceStatusBar DbStatusBar => StatusBar;

        public OrderDetailsWindow()
        {
            InitializeComponent();

            //ProductControl.LostFocus += (sender, args) => OrderDetailsViewModel.OnKeyControlLeave();
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
