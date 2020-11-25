using System;
using RingSoft.DbLookup;
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
    /// <summary>
    /// The view for DbMaintenanceViewModel classes.  This should be implemented by the base class of DbMaintenance windows to avoid duplicate code.
    /// </summary>
    public interface IDbMaintenanceView
    {
        /// <summary>
        /// Occurs when the user selects a record from the Find LookupWindow.  The DbMaintenanceViewModelBase subscribes to this event to load the record that the user selected.
        /// </summary>
        event EventHandler<LookupSelectArgs> LookupFormReturn;

        /// <summary>
        /// Called when validation fails for a field control.  It gives opportunity to set focus to the control and display a validation error message.
        /// </summary>
        /// <param name="fieldDefinition">The field definition that failed.</param>
        /// <param name="text">The message box text.</param>
        /// <param name="caption">The message box caption.</param>
        void OnValidationFail(FieldDefinition fieldDefinition, string text, string caption);

        /// <summary>
        /// Reset the view for new record.
        /// </summary>
        void ResetViewForNewRecord();

        void OnRecordSelected();

        /// <summary>
        /// Show the Find button's lookup window.
        /// </summary>
        /// <param name="lookupDefinition">The lookup definition.</param>
        /// <param name="allowAdd">Should the LookupWindow allow Add?</param>
        /// <param name="allowView">Should the LookupWindow allow View?</param>
        /// <param name="initialSearchFor">The initial search for text.</param>
        /// <param name="initialSearchForPrimaryKey">The initial search for primary key value.</param>
        void ShowFindLookupWindow(LookupDefinitionBase lookupDefinition, bool allowAdd, bool allowView,
            string initialSearchFor, PrimaryKeyValue initialSearchForPrimaryKey);

        /// <summary>
        /// Close this window.
        /// </summary>
        void CloseWindow();

        /// <summary>
        /// Show a yes/no/cancel message box.
        /// </summary>
        /// <param name="text">The message box text.</param>
        /// <param name="caption">The message box caption.</param>
        /// <returns>The button that the user clicked.</returns>
        MessageButtons ShowYesNoCancelMessage(string text, string caption);

        /// <summary>
        /// Show a yes/no message box.
        /// </summary>
        /// <param name="text">The message box text.</param>
        /// <param name="caption">The message box caption.</param>
        /// <returns>True if the user clicked Yes, otherwise false.</returns>
        bool ShowYesNoMessage(string text, string caption);

        /// <summary>
        /// Show the record saved message.
        /// </summary>
        void ShowRecordSavedMessage();
    }
}
