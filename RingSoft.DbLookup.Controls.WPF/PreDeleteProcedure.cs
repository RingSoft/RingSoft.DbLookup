using System.Collections.Generic;
using System.Windows;
using RingSoft.DataEntryControls.WPF;
using RingSoft.DbLookup.ModelDefinition.FieldDefinitions;
using RingSoft.DbMaintenance;

namespace RingSoft.DbLookup.Controls.WPF
{
    internal class PreDeleteProcedure : TwoTierProcessingProcedure
    {
        public DbMaintenanceViewModelBase ViewModel { get; }

        public DeleteTables DeleteTables { get; }

        public List<FieldDefinition> Fields { get; }

        public IDbMaintenanceDataProcessor Processor { get; }
        public PreDeleteProcedure(Window ownerWindow
            , string windowText
            , DbMaintenanceViewModelBase viewModel
            , DeleteTables deleteTables
            , List<FieldDefinition> fields
            , IDbMaintenanceDataProcessor processor
            ) : base(ownerWindow, windowText)
        {
            ViewModel = viewModel;
            DeleteTables = deleteTables;
            Fields = fields;
            Processor = processor;
        }

        public override bool DoProcedure()
        {
            var result = ViewModel.DoGetDeleteTables(Fields, DeleteTables, this);
            Processor.PreDeleteResult = result;
            return result;
        }
    }
}
