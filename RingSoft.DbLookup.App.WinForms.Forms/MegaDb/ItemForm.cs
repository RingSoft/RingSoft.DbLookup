using System.Windows.Forms;
using RingSoft.DbLookup.App.Library;
using RingSoft.DbLookup.App.Library.MegaDb.ViewModels;
using RingSoft.DbLookup.ModelDefinition.FieldDefinitions;
using RingSoft.DbMaintenance;

namespace RingSoft.DbLookup.App.WinForms.Forms.MegaDb
{
    public partial class ItemForm : DbMaintenanceForm
    {
        public override DbMaintenanceViewModelBase ViewModel => _itemViewModel;

        private ItemViewModel _itemViewModel = new ItemViewModel();

        public ItemForm()
        {
            InitializeComponent();

            ItemIdLabel.DataBindings.Add(nameof(ItemIdLabel.Text), _itemViewModel, nameof(_itemViewModel.ItemId), false,
                DataSourceUpdateMode.OnPropertyChanged);

            RegisterFormKeyControl(NameControl);

            LocationControl.DataBindings.Add(nameof(LocationControl.Setup), _itemViewModel,
                nameof(_itemViewModel.LocationAutoFillSetup), true, DataSourceUpdateMode.Never);
            LocationControl.DataBindings.Add(nameof(LocationControl.Value), _itemViewModel,
                nameof(_itemViewModel.LocationAutoFillValue), true, DataSourceUpdateMode.OnPropertyChanged);

            ManufacturerControl.DataBindings.Add(nameof(ManufacturerControl.Setup), _itemViewModel,
                nameof(_itemViewModel.ManufacturerAutoFillSetup), true, DataSourceUpdateMode.Never);
            ManufacturerControl.DataBindings.Add(nameof(ManufacturerControl.Value), _itemViewModel,
                nameof(_itemViewModel.ManufacturerAutoFillValue), true, DataSourceUpdateMode.OnPropertyChanged);
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
