// ***********************************************************************
// Assembly         : RingSoft.DbLookup.Controls.WPF
// Author           : petem
// Created          : 07-11-2024
//
// Last Modified By : petem
// Last Modified On : 07-11-2024
// ***********************************************************************
// <copyright file="DeleteProcedure.cs" company="Peter Ringering">
//     2024
// </copyright>
// <summary></summary>
// ***********************************************************************
using System.Windows;
using RingSoft.DataEntryControls.WPF;
using RingSoft.DbMaintenance;

namespace RingSoft.DbLookup.Controls.WPF
{
    /// <summary>
    /// Class DeleteProcedure.
    /// Implements the <see cref="TwoTierProcessingProcedure" />
    /// </summary>
    /// <seealso cref="TwoTierProcessingProcedure" />
    internal class DeleteProcedure : TwoTierProcessingProcedure
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
        /// Initializes a new instance of the <see cref="DeleteProcedure"/> class.
        /// </summary>
        /// <param name="ownerWindow">The owner window.</param>
        /// <param name="windowText">The window text.</param>
        /// <param name="viewModel">The view model.</param>
        /// <param name="deleteTables">The delete tables.</param>
        public DeleteProcedure(Window ownerWindow
            , string windowText
            , DbMaintenanceViewModelBase viewModel
            , DeleteTables deleteTables) : base(ownerWindow, windowText)
        {
            ViewModel = viewModel;
            DeleteTables = deleteTables;
        }

        /// <summary>
        /// Does the procedure.
        /// </summary>
        /// <returns><c>true</c> if XXXX, <c>false</c> otherwise.</returns>
        public override bool DoProcedure()
        {
            ViewModel.Processor.DeleteChildrenResult = ViewModel.DeleteChildren(DeleteTables, this);
            return true;
        }
    }
}
