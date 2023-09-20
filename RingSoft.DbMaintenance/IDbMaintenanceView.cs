using System;
using System.Collections.Generic;
using RingSoft.DbLookup;
using RingSoft.DbLookup.AutoFill;
using RingSoft.DbLookup.Lookup;
using RingSoft.DbLookup.ModelDefinition.FieldDefinitions;

namespace RingSoft.DbMaintenance
{
    public enum MessageButtons
    {
        Yes = 0,
        No = 1,
        Cancel = 2
    }

    public class TestDbMaintenanceView : IDbMaintenanceView
    {
        public void OnValidationFail(FieldDefinition fieldDefinition, string text, string caption)
        {
            
        }

        public void ResetViewForNewRecord()
        {
            
        }

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
        /// <param name="fieldDefinition">The field definition that failed.</param>
        /// <param name="text">The message box text.</param>
        /// <param name="caption">The message box caption.</param>
        //void OnValidationFail(FieldDefinition fieldDefinition, string text, string caption);

        /// <summary>
        /// Reset the view for new record.
        /// </summary>
        void ResetViewForNewRecord();

        void SetReadOnlyMode(bool readOnlyValue);
    }
}
