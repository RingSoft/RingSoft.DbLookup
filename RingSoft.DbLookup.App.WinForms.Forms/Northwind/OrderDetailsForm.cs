using System.Windows.Forms;
using RingSoft.DbLookup.App.Library;
using RingSoft.DbLookup.App.Library.Northwind.ViewModels;
using RingSoft.DbLookup.ModelDefinition.FieldDefinitions;
using RingSoft.DbLookup.QueryBuilder;
using RingSoft.DbMaintenance;

namespace RingSoft.DbLookup.App.WinForms.Forms.Northwind
{
    public partial class OrderDetailsForm : DbMaintenanceForm
    {
        public override DbMaintenanceViewModelBase ViewModel => _orderDetailsViewModel;

        private readonly OrderDetailsViewModel _orderDetailsViewModel = new OrderDetailsViewModel();
        public OrderDetailsForm()
        {
            InitializeComponent();

            OrderIdLabel.DataBindings.Add(nameof(OrderIdLabel.Text), _orderDetailsViewModel,
                nameof(_orderDetailsViewModel.OrderId), false, DataSourceUpdateMode.OnPropertyChanged);
            CustomerLabel.DataBindings.Add(nameof(CustomerLabel.Text), _orderDetailsViewModel,
                nameof(_orderDetailsViewModel.Customer), false, DataSourceUpdateMode.OnPropertyChanged);
            OrderDateLabel.BindControlToDateFormat(_orderDetailsViewModel, nameof(_orderDetailsViewModel.OrderDate),
                DbDateTypes.DateOnly);

            ProductControl.DataBindings.Add(nameof(ProductControl.Setup), _orderDetailsViewModel,
                nameof(_orderDetailsViewModel.ProductAutoFillSetup), true, DataSourceUpdateMode.Never);
            ProductControl.DataBindings.Add(nameof(ProductControl.Value), _orderDetailsViewModel,
                nameof(_orderDetailsViewModel.ProductAutoFillValue), true, DataSourceUpdateMode.OnPropertyChanged);
            ProductControl.DataBindings.Add(nameof(ProductControl.Enabled), _orderDetailsViewModel,
                nameof(_orderDetailsViewModel.PrimaryKeyControlsEnabled), false,
                DataSourceUpdateMode.OnPropertyChanged);

            QuantityTextBox.BindTextBoxToIntFormat(_orderDetailsViewModel, nameof(_orderDetailsViewModel.Quantity));
            PriceTextBox.BindControlToDecimalFormat(_orderDetailsViewModel, nameof(_orderDetailsViewModel.Price));
            ExtPriceLabel.BindControlToDecimalFormat(_orderDetailsViewModel, nameof(_orderDetailsViewModel.ExtPrice));
            DiscountTextBox.BindControlToDecimalFormat(_orderDetailsViewModel, nameof(_orderDetailsViewModel.Discount));

            ProductControl.Leave += (sender, args) => _orderDetailsViewModel.OnKeyControlLeave();
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
