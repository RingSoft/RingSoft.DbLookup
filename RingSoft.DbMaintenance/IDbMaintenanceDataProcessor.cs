// ***********************************************************************
// Assembly         : RingSoft.DbMaintenance
// Author           : petem
// Created          : 12-19-2022
//
// Last Modified By : petem
// Last Modified On : 05-29-2023
// ***********************************************************************
// <copyright file="IDbMaintenanceDataProcessor.cs" company="Peter Ringering">
//     Copyright (c) . All rights reserved.
// </copyright>
// <summary></summary>
// ***********************************************************************
using System;
using System.Collections.Generic;
using System.Text;
using RingSoft.DbLookup;
using RingSoft.DbLookup.AutoFill;
using RingSoft.DbLookup.Lookup;
using RingSoft.DbLookup.ModelDefinition.FieldDefinitions;

namespace RingSoft.DbMaintenance
{
    /// <summary>
    /// Enum MaintenanceKey
    /// </summary>
    public enum MaintenanceKey
    {
        /// <summary>
        /// The alt
        /// </summary>
        Alt = 0,
        /// <summary>
        /// The control
        /// </summary>
        Ctrl = 1,
    }

    /// <summary>
    /// Class DbAutoFillMap.
    /// </summary>
    public class DbAutoFillMap
    {
        /// <summary>
        /// Gets the automatic fill setup.
        /// </summary>
        /// <value>The automatic fill setup.</value>
        public AutoFillSetup AutoFillSetup { get; }

        /// <summary>
        /// Gets the automatic fill value.
        /// </summary>
        /// <value>The automatic fill value.</value>
        public AutoFillValue AutoFillValue { get; }

        /// <summary>
        /// Initializes a new instance of the <see cref="DbAutoFillMap"/> class.
        /// </summary>
        /// <param name="autoFillSetup">The automatic fill setup.</param>
        /// <param name="autoFillValue">The automatic fill value.</param>
        public DbAutoFillMap(AutoFillSetup autoFillSetup, AutoFillValue autoFillValue)
        {
            AutoFillSetup = autoFillSetup;
            AutoFillValue = autoFillValue;
        }

        /// <summary>
        /// Returns a <see cref="System.String" /> that represents this instance.
        /// </summary>
        /// <returns>A <see cref="System.String" /> that represents this instance.</returns>
        public override string ToString()
        {
            if (AutoFillSetup.ForeignField != null)
            {
                return AutoFillSetup.ForeignField.Description;
            }

            return base.ToString();
        }
    }


    /// <summary>
    /// Interface IDbMaintenanceDataProcessor
    /// </summary>
    public interface IDbMaintenanceDataProcessor
    {
        /// <summary>
        /// Gets or sets a value indicating whether [key control registered].
        /// </summary>
        /// <value><c>true</c> if [key control registered]; otherwise, <c>false</c>.</value>
        bool KeyControlRegistered { get; set; }

        /// <summary>
        /// Occurs when [lookup add view].
        /// </summary>
        event EventHandler<LookupAddViewArgs> LookupAddView;

        /// <summary>
        /// Initializes from lookup data.
        /// </summary>
        /// <param name="e">The e.</param>
        void InitializeFromLookupData(LookupAddViewArgs e);

        /// <summary>
        /// Called when [validation fail].
        /// </summary>
        /// <param name="fieldDefinition">The field definition.</param>
        /// <param name="text">The text.</param>
        /// <param name="caption">The caption.</param>
        void OnValidationFail(FieldDefinition fieldDefinition, string text, string caption);

        /// <summary>
        /// Called when [record selected].
        /// </summary>
        void OnRecordSelected();

        /// <summary>
        /// Shows the find lookup window.
        /// </summary>
        /// <param name="lookupDefinition">The lookup definition.</param>
        /// <param name="allowAdd">if set to <c>true</c> [allow add].</param>
        /// <param name="allowView">if set to <c>true</c> [allow view].</param>
        /// <param name="initialSearchFor">The initial search for.</param>
        /// <param name="initialSearchForPrimaryKey">The initial search for primary key.</param>
        void ShowFindLookupWindow(LookupDefinitionBase lookupDefinition, bool allowAdd, bool allowView,
            string initialSearchFor, PrimaryKeyValue initialSearchForPrimaryKey);

        /// <summary>
        /// Closes the window.
        /// </summary>
        void CloseWindow();

        /// <summary>
        /// Shows the yes no cancel message.
        /// </summary>
        /// <param name="text">The text.</param>
        /// <param name="caption">The caption.</param>
        /// <param name="playSound">if set to <c>true</c> [play sound].</param>
        /// <returns>MessageButtons.</returns>
        MessageButtons ShowYesNoCancelMessage(string text, string caption, bool playSound = false);

        /// <summary>
        /// Shows the yes no message.
        /// </summary>
        /// <param name="text">The text.</param>
        /// <param name="caption">The caption.</param>
        /// <param name="playSound">if set to <c>true</c> [play sound].</param>
        /// <returns><c>true</c> if XXXX, <c>false</c> otherwise.</returns>
        bool ShowYesNoMessage(string text, string caption, bool playSound = false);

        /// <summary>
        /// Shows the record saved message.
        /// </summary>
        void ShowRecordSavedMessage();

        /// <summary>
        /// Called when [read only mode set].
        /// </summary>
        /// <param name="readOnlyValue">if set to <c>true</c> [read only value].</param>
        void OnReadOnlyModeSet(bool readOnlyValue);

        /// <summary>
        /// Determines whether [is maintenance key down] [the specified key].
        /// </summary>
        /// <param name="key">The key.</param>
        /// <returns><c>true</c> if [is maintenance key down] [the specified key]; otherwise, <c>false</c>.</returns>
        bool IsMaintenanceKeyDown(MaintenanceKey key);
        /// <summary>
        /// Activates this instance.
        /// </summary>
        void Activate();

        /// <summary>
        /// Sets the window read only mode.
        /// </summary>
        void SetWindowReadOnlyMode();

        /// <summary>
        /// Shows the record lock window.
        /// </summary>
        /// <param name="lockKey">The lock key.</param>
        /// <param name="message">The message.</param>
        /// <param name="inputParameter">The input parameter.</param>
        /// <returns><c>true</c> if XXXX, <c>false</c> otherwise.</returns>
        bool ShowRecordLockWindow(PrimaryKeyValue lockKey, string message, object inputParameter);

        /// <summary>
        /// Checks the delete tables.
        /// </summary>
        /// <param name="deleteTables">The delete tables.</param>
        /// <returns><c>true</c> if XXXX, <c>false</c> otherwise.</returns>
        bool CheckDeleteTables(DeleteTables deleteTables);

        /// <summary>
        /// Prints the output.
        /// </summary>
        /// <param name="printerSetupArgs">The printer setup arguments.</param>
        void PrintOutput(PrinterSetupArgs printerSetupArgs);

        /// <summary>
        /// Sets the save status.
        /// </summary>
        /// <param name="message">The message.</param>
        /// <param name="alertLevel">The alert level.</param>
        void SetSaveStatus(string message, AlertLevels alertLevel);

        /// <summary>
        /// Gets the automatic fills.
        /// </summary>
        /// <returns>List&lt;DbAutoFillMap&gt;.</returns>
        List<DbAutoFillMap> GetAutoFills();

        /// <summary>
        /// Handles the automatic fill value fail.
        /// </summary>
        /// <param name="autoFillMap">The automatic fill map.</param>
        void HandleAutoFillValFail(DbAutoFillMap autoFillMap);
    }
}
