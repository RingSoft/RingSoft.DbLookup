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

        public override Button SaveButton { get; protected set; }
        public override Button SelectButton { get; protected set; }
        public override Button DeleteButton { get; protected set; }
        public override Button FindButton { get; protected set; }
        public override Button NewButton { get; protected set; }
        public override Button CloseButton { get; protected set; }
        public override Button NextButton { get; protected set; }
        public override Button PreviousButton { get; protected set; }
        public override Button PrintButton { get; protected set; }

        public override void Initialize(BaseWindow window, Control buttonsControl,
            DbMaintenanceViewModelBase viewModel, IDbMaintenanceView view, DbMaintenanceStatusBar statusBar = null)
        {
            //ViewModel = viewModel;
            //MaintenanceButtonsControl = buttonsControl;
            //SetupStatusBar(viewModel, statusBar);
            //MaintenanceWindow = window;

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

            base.Initialize(window, buttonsControl, viewModel, view, statusBar);
        }
    }
}
