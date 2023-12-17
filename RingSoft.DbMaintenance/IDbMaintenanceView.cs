// ***********************************************************************
// Assembly         : RingSoft.DbMaintenance
// Author           : petem
// Created          : 12-19-2022
//
// Last Modified By : petem
// Last Modified On : 09-20-2023
// ***********************************************************************
// <copyright file="IDbMaintenanceView.cs" company="Peter Ringering">
//     Copyright (c) 2023. All rights reserved.
// </copyright>
// <summary></summary>
// ***********************************************************************
using System;
using System.Collections.Generic;
using RingSoft.DbLookup;
using RingSoft.DbLookup.AutoFill;
using RingSoft.DbLookup.Lookup;
using RingSoft.DbLookup.ModelDefinition.FieldDefinitions;

namespace RingSoft.DbMaintenance
{
    /// <summary>
    /// Enum MessageButtons
    /// </summary>
    public enum MessageButtons
    {
        /// <summary>
        /// The yes
        /// </summary>
        Yes = 0,
        /// <summary>
        /// The no
        /// </summary>
        No = 1,
        /// <summary>
        /// The cancel
        /// </summary>
        Cancel = 2
    }

    /// <summary>
    /// Class TestDbMaintenanceView.
    /// Implements the <see cref="RingSoft.DbMaintenance.IDbMaintenanceView" />
    /// </summary>
    /// <seealso cref="RingSoft.DbMaintenance.IDbMaintenanceView" />
    public class TestDbMaintenanceView : IDbMaintenanceView
    {
        /// <summary>
        /// Called when [validation fail].
        /// </summary>
        /// <param name="fieldDefinition">The field definition.</param>
        /// <param name="text">The text.</param>
        /// <param name="caption">The caption.</param>
        public void OnValidationFail(FieldDefinition fieldDefinition, string text, string caption)
        {
            
        }

        /// <summary>
        /// Called when validation fails for a field control.  It gives opportunity to set focus to the control and display a validation error message.
        /// </summary>
        public void ResetViewForNewRecord()
        {
            
        }

        /// <summary>
        /// Sets the read only mode.
        /// </summary>
        /// <param name="readOnlyValue">if set to <c>true</c> [read only value].</param>
        public void SetReadOnlyMode(bool readOnlyValue)
        {
        }
    }

    /// <summary>
    /// The view for DbMaintenanceViewModel classes.  This should be implemented by the base class of DbMaintenance windows to avoid duplicate code.
    /// </summary>
    public interface IDbMaintenanceView
    {
        /// <summary>
        /// Called when validation fails for a field control.  It gives opportunity to set focus to the control and display a validation error message.
        /// </summary>
        void ResetViewForNewRecord();

        /// <summary>
        /// Sets the read only mode.
        /// </summary>
        /// <param name="readOnlyValue">if set to <c>true</c> [read only value].</param>
        void SetReadOnlyMode(bool readOnlyValue);
    }
}
