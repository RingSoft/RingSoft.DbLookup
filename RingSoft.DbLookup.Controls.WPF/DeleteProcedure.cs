using System.Windows;
using RingSoft.DataEntryControls.WPF;
using RingSoft.DbMaintenance;

namespace RingSoft.DbLookup.Controls.WPF
{
    internal class DeleteProcedure : TwoTierProcessingProcedure
    {
        public DbMaintenanceViewModelBase ViewModel { get; }

        public DeleteTables DeleteTables { get; }
        public DeleteProcedure(Window ownerWindow
            , string windowText
            , DbMaintenanceViewModelBase viewModel
            , DeleteTables deleteTables) : base(ownerWindow, windowText)
        {
            ViewModel = viewModel;
            DeleteTables = deleteTables;
        }

        public override bool DoProcedure()
        {
            ViewModel.Processor.DeleteChildrenResult = ViewModel.DeleteChildren(DeleteTables, this);
            return true;
        }
    }
}
