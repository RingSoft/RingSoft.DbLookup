// ***********************************************************************
// Assembly         : RingSoft.DbLookup.Controls.WPF
// Author           : petem
// Created          : 12-19-2022
//
// Last Modified By : petem
// Last Modified On : 12-19-2022
// ***********************************************************************
// <copyright file="LookupGridEditHostFactory.cs" company="Peter Ringering">
//     Copyright (c) . All rights reserved.
// </copyright>
// <summary></summary>
// ***********************************************************************
using RingSoft.DataEntryControls.WPF.DataEntryGrid;
using RingSoft.DataEntryControls.WPF.DataEntryGrid.EditingControlHost;
using RingSoft.DbLookup.AdvancedFind;
using RingSoft.DbLookup.Controls.WPF.AdvancedFind;
using RingSoft.DbMaintenance;

namespace RingSoft.DbLookup.Controls.WPF
{
    /// <summary>
    /// Class LookupGridEditHostFactory.
    /// Implements the <see cref="DataEntryGridHostFactory" />
    /// </summary>
    /// <seealso cref="DataEntryGridHostFactory" />
    public class LookupGridEditHostFactory : DataEntryGridHostFactory
    {
        /// <summary>
        /// Gets the control host.
        /// </summary>
        /// <param name="grid">The grid.</param>
        /// <param name="editingControlHostId">The editing control host identifier.</param>
        /// <returns>DataEntryGridEditingControlHostBase.</returns>
        public override DataEntryGridEditingControlHostBase GetControlHost(DataEntryGrid grid, int editingControlHostId)
        {
            if (editingControlHostId == DataEntryGridAutoFillCellProps.AutoFillControlHostId)
                return new DataEntryGridAutoFillHost(grid);

            if (editingControlHostId == AdvancedFindColumnFormulaCellProps.ColumnFormulaCellId)
            {
                return new DataEntryGridAdvancedFindFormulaColumnHost(grid);
            }

            if (editingControlHostId == AdvancedFindMemoCellProps.AdvancedFindMemoHostId)
            {
                return new DataEntryGridAdvancedFindMemoHost(grid);
            }

            if (editingControlHostId == AdvancedFindFilterCellProps.FilterControlId)
            {
                return new AdvancedFindFilterHost(grid);
            }

            if (editingControlHostId == AdvancedFilterParenthesesCellProps.ParenthesesHostId)
            {
                return new AdvancedFilterParenthesesHost(grid);
            }


            return base.GetControlHost(grid, editingControlHostId);
        }
    }
}
