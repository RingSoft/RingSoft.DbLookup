using System.Windows.Controls;
using RingSoft.DbLookup.Controls.WPF;
using RingSoft.DbMaintenance;

namespace RingSoft.DbLookup.App.WPFCore
{
    public class AppDbMaintenanceUserControlProcessor : DbMaintenanceUserControlProcessor
    {
        public override Button FindButton => _buttonsControl.FindButton;
        public override Button NextButton => _buttonsControl.NextButton;
        public override Button PreviousButton => _buttonsControl.PreviousButton;
        public override Button SaveButton => _buttonsControl.SaveButton;
        public override Button SelectButton => _buttonsControl.SelectButton;
        public override Button DeleteButton => _buttonsControl.DeleteButton;
        public override Button NewButton => _buttonsControl.NewButton;
        public override Button PrintButton => _buttonsControl.PrintButton;
        public override Button CloseButton => _buttonsControl.CloseButton;

        private DbMaintenanceButtonsControl _buttonsControl;

        public AppDbMaintenanceUserControlProcessor(
            DbMaintenanceViewModelBase viewModel
            , Control buttonsControl
            , DbMaintenanceUserControl userControl
            , DbMaintenanceStatusBar statusBar
            , IUserControlHost host) 
            : base(viewModel, buttonsControl, userControl, statusBar, host)
        {
            if (buttonsControl is DbMaintenanceButtonsControl dbMaintenanceButtonsControl)
            {
                _buttonsControl = dbMaintenanceButtonsControl;
            }
        }
    }
}
