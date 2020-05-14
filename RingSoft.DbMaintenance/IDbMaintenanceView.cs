using System;
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
    public interface IDbMaintenanceView
    {
        /// <summary>
        /// Called when validation fails for a field control.
        /// </summary>
        /// <param name="fieldDefinition">The field definition.</param>
        /// <param name="text">The text.</param>
        /// <param name="caption">The caption.</param>
        void OnValidationFail(FieldDefinition fieldDefinition, string text, string caption);

        /// <summary>
        /// Resets the view for new record.
        /// </summary>
        void ResetViewForNewRecord();

        /// <summary>
        /// Shows the find lookup form.
        /// </summary>
        /// <param name="lookupDefinition">The lookup definition.</param>
        /// <param name="allowAdd">if set to <c>true</c> allow add.</param>
        /// <param name="allowView">if set to <c>true</c> allow view.</param>
        /// <param name="initialSearchFor">The initial search for.</param>
        void ShowFindLookupWindow(LookupDefinitionBase lookupDefinition, bool allowAdd, bool allowView, string initialSearchFor);

        /// <summary>
        /// Occurs when the lookup form closes and the user selects a record from the lookup form.
        /// </summary>
        event EventHandler<LookupSelectArgs> LookupFormReturn;

        /// <summary>
        /// Closes the window.
        /// </summary>
        void CloseWindow();

        /// <summary>
        /// Shows a yes/no/cancel message.
        /// </summary>
        /// <param name="text">The text.</param>
        /// <param name="caption">The caption.</param>
        /// <returns>The button that the user clicked.</returns>
        MessageButtons ShowYesNoCancelMessage(string text, string caption);

        /// <summary>
        /// Shows a yes no message.
        /// </summary>
        /// <param name="text">The text.</param>
        /// <param name="caption">The caption.</param>
        /// <returns>True if the user clicked Yes, otherwise false.</returns>
        bool ShowYesNoMessage(string text, string caption);

        /// <summary>
        /// Shows the record saved message.
        /// </summary>
        void ShowRecordSavedMessage();
    }
}
