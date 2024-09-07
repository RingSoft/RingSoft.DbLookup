// ***********************************************************************
// Assembly         : RingSoft.DbLookup.Controls.WPF
// Author           : petem
// Created          : 09-02-2024
//
// Last Modified By : petem
// Last Modified On : 09-03-2024
// ***********************************************************************
// <copyright file="PreDeleteProcedure.cs" company="Peter Ringering">
//     2024
// </copyright>
// <summary></summary>
// ***********************************************************************
using System.Collections.Generic;
using System.Windows;
using RingSoft.DataEntryControls.WPF;
using RingSoft.DbLookup.ModelDefinition.FieldDefinitions;
using RingSoft.DbMaintenance;

namespace RingSoft.DbLookup.Controls.WPF
{
    /// <summary>
    /// Class PreDeleteProcedure.
    /// Implements the <see cref="TwoTierProcessingProcedure" />
    /// </summary>
    /// <seealso cref="TwoTierProcessingProcedure" />
    internal class PreDeleteProcedure : TwoTierProcessingProcedure
    {
        /// <summary>
        /// Gets the view model.
        /// </summary>
        /// <value>The view model.</value>
        public DbMaintenanceViewModelBase ViewModel { get; }

        /// <summary>
        /// Gets the delete tables.
        /// </summary>
        /// <value>The delete tables.</value>
        public DeleteTables DeleteTables { get; }

        /// <summary>
        /// Gets the fields.
        /// </summary>
        /// <value>The fields.</value>
        public List<FieldDefinition> Fields { get; }

        /// <summary>
        /// Gets the processor.
        /// </summary>
        /// <value>The processor.</value>
        public IDbMaintenanceProcessor Processor { get; }
        /// <summary>
        /// Initializes a new instance of the <see cref="PreDeleteProcedure"/> class.
        /// </summary>
        /// <param name="ownerWindow">The owner window.</param>
        /// <param name="windowText">The window text.</param>
        /// <param name="viewModel">The view model.</param>
        /// <param name="deleteTables">The delete tables.</param>
        /// <param name="fields">The fields.</param>
        /// <param name="processor">The processor.</param>
        public PreDeleteProcedure(Window ownerWindow
            , string windowText
            , DbMaintenanceViewModelBase viewModel
            , DeleteTables deleteTables
            , List<FieldDefinition> fields
            , IDbMaintenanceProcessor processor
            ) : base(ownerWindow, windowText)
        {
            ViewModel = viewModel;
            DeleteTables = deleteTables;
            Fields = fields;
            Processor = processor;
        }

        /// <summary>
        /// Does the procedure.
        /// </summary>
        /// <returns><c>true</c> if XXXX, <c>false</c> otherwise.</returns>
        public override bool DoProcedure()
        {
            var result = ViewModel.DoGetDeleteTables(Fields, DeleteTables, this);
            Processor.PreDeleteResult = result;
            return result;
        }
    }
}
