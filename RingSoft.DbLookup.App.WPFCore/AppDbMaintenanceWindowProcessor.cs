using System.Windows.Controls;
using RingSoft.DataEntryControls.WPF;
using RingSoft.DbLookup.Controls.WPF;
using RingSoft.DbMaintenance;

namespace RingSoft.DbLookup.App.WPFCore
{
    public class AppDbMaintenanceWindowProcessor : DbMaintenanceWindowProcessor, IDbMaintenanceProcessor
    {
        public AppDbMaintenanceWindowProcessor()
        {
        }

        public override DbMaintenanceViewModelBase ViewModel { get; set; }
        public override Button SaveButton { get; set; }
        public override Button SelectButton { get; set; }
        public override Button DeleteButton { get; set; }
        public override Button FindButton { get; set; }
        public override Button NewButton { get; set; }
        public override Button CloseButton { get; set; }
        public override Button NextButton { get; set; }
        public override Button PreviousButton { get; set; }
        public override Button PrintButton { get; set; }
        public override BaseWindow MaintenanceWindow { get; set; }
        public override Control MaintenanceButtonsControl { get; set; }

        public override void Initialize(BaseWindow window, Control buttonsControl,
            DbMaintenanceViewModelBase viewModel, IDbMaintenanceView view)
        {
            MaintenanceWindow = window;
            ViewModel = viewModel;
            MaintenanceButtonsControl = buttonsControl;
            
            var dbMaintenanceButtons = (DbMaintenanceButtonsControl) buttonsControl;
            SaveButton = dbMaintenanceButtons.SaveButton;
            SelectButton = dbMaintenanceButtons.SelectButton;
            DeleteButton = dbMaintenanceButtons.DeleteButton;
            FindButton = dbMaintenanceButtons.FindButton;
            NewButton = dbMaintenanceButtons.NewButton;
            CloseButton = dbMaintenanceButtons.CloseButton;
            NextButton = dbMaintenanceButtons.NextButton;
            PreviousButton = dbMaintenanceButtons.PreviousButton;
            PrintButton = dbMaintenanceButtons.PrintButton;

            SetupControl(view);
        }
    }
}
