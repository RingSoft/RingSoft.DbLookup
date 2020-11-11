using RingSoft.DbLookup.App.Library;
using RingSoft.DbLookup.ModelDefinition.FieldDefinitions;
using RingSoft.DbMaintenance;

namespace RingSoft.DbLookup.App.WPFCore.Northwind
{
    /// <summary>
    /// Interaction logic for ProductsWindow.xaml
    /// </summary>
    public partial class ProductsWindow
    {
        public override DbMaintenanceViewModelBase ViewModel => ProductViewModel;
        public override DbMaintenanceButtonsControl MaintenanceButtonsControl => ButtonsControl;

        public ProductsWindow()
        {
            InitializeComponent();
            Initialize();

            RegisterFormKeyControl(ProductNameControl);

            QtyPerUnitEdit.MaxLength =
                RsDbLookupAppGlobals.EfProcessor.NorthwindLookupContext.Products
                    .GetFieldDefinition(p => p.QuantityPerUnit).MaxLength;
        }

        public override void ResetViewForNewRecord()
        {
            ProductNameControl.Focus();
            base.ResetViewForNewRecord();
        }

        public override void OnValidationFail(FieldDefinition fieldDefinition, string text, string caption)
        {
            var table = RsDbLookupAppGlobals.EfProcessor.NorthwindLookupContext.Products;

            if (fieldDefinition == table.GetFieldDefinition(p => p.ProductName))
                ProductNameControl.Focus();
            else if (fieldDefinition == table.GetFieldDefinition(p => p.SupplierID))
                SupplierControl.Focus();
            else if (fieldDefinition == table.GetFieldDefinition(p => p.CategoryID))
                CategoryControl.Focus();

            base.OnValidationFail(fieldDefinition, text, caption);
        }
    }
}
