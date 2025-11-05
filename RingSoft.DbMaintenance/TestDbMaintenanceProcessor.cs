// ***********************************************************************
// Assembly         : RingSoft.DbMaintenance
// Author           : petem
// Created          : 12-19-2022
//
// Last Modified By : petem
// Last Modified On : 09-03-2024
// ***********************************************************************
// <copyright file="TestDbMaintenanceProcessor.cs" company="Peter Ringering">
//     Copyright (c) 2023. All rights reserved.
// </copyright>
// <summary></summary>
// ***********************************************************************
using System;
using System.Collections.Generic;
using RingSoft.DataEntryControls.Engine;
using RingSoft.DbLookup;
using RingSoft.DbLookup.Lookup;
using RingSoft.DbLookup.ModelDefinition.FieldDefinitions;

namespace RingSoft.DbMaintenance
{
    /// <summary>
    /// Class TestDbMaintenanceProcessor.
    /// Implements the <see cref="RingSoft.DbMaintenance.IDbMaintenanceDataProcessor" />
    /// </summary>
    /// <seealso cref="RingSoft.DbMaintenance.IDbMaintenanceDataProcessor" />
    public class TestDbMaintenanceProcessor : IDbMaintenanceDataProcessor
    {
        /// <summary>
        /// Gets or sets a value indicating whether [message box result].
        /// </summary>
        /// <value><c>true</c> if [message box result]; otherwise, <c>false</c>.</value>
        public bool MessageBoxResult { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether [delete children result].
        /// </summary>
        /// <value><c>true</c> if [delete children result]; otherwise, <c>false</c>.</value>
        public bool DeleteChildrenResult { get; set; }
        /// <summary>
        /// Gets or sets a value indicating whether [pre delete result].
        /// </summary>
        /// <value><c>true</c> if [pre delete result]; otherwise, <c>false</c>.</value>
        public bool PreDeleteResult { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether [key control registered].
        /// </summary>
        /// <value><c>true</c> if [key control registered]; otherwise, <c>false</c>.</value>
        public bool KeyControlRegistered { get; set; }

        public bool SetStartupFocus { get; set; } = true;

        /// <summary>
        /// Occurs when [lookup add view].
        /// </summary>
        public event EventHandler<LookupAddViewArgs> LookupAddView;

        /// <summary>
        /// Initializes a new instance of the <see cref="TestDbMaintenanceProcessor"/> class.
        /// </summary>
        public TestDbMaintenanceProcessor()
        {
            
        }
        /// <summary>
        /// Initializes from lookup data.
        /// </summary>
        /// <param name="e">The e.</param>
        public void InitializeFromLookupData(LookupAddViewArgs e)
        {
            
        }

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
        /// Called when [record selected].
        /// </summary>
        public void OnRecordSelected()
        {
            
        }

        /// <summary>
        /// Shows the find lookup window.
        /// </summary>
        /// <param name="lookupDefinition">The lookup definition.</param>
        /// <param name="allowAdd">if set to <c>true</c> [allow add].</param>
        /// <param name="allowView">if set to <c>true</c> [allow view].</param>
        /// <param name="initialSearchFor">The initial search for.</param>
        /// <param name="initialSearchForPrimaryKey">The initial search for primary key.</param>
        public void ShowFindLookupWindow(LookupDefinitionBase lookupDefinition, bool allowAdd, bool allowView, string initialSearchFor,
            PrimaryKeyValue initialSearchForPrimaryKey)
        {
            
        }

        /// <summary>
        /// Closes the window.
        /// </summary>
        public void CloseWindow()
        {
            
        }

        /// <summary>
        /// Shows the yes no cancel message.
        /// </summary>
        /// <param name="text">The text.</param>
        /// <param name="caption">The caption.</param>
        /// <param name="playSound">if set to <c>true</c> [play sound].</param>
        /// <returns>MessageButtons.</returns>
        /// <exception cref="System.NotImplementedException"></exception>
        public MessageButtons ShowYesNoCancelMessage(string text, string caption, bool playSound = false)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Shows the yes no message.
        /// </summary>
        /// <param name="text">The text.</param>
        /// <param name="caption">The caption.</param>
        /// <param name="playSound">if set to <c>true</c> [play sound].</param>
        /// <returns><c>true</c> if XXXX, <c>false</c> otherwise.</returns>
        public bool ShowYesNoMessage(string text, string caption, bool playSound = false)
        {
            return MessageBoxResult;
        }

        /// <summary>
        /// Shows the record saved message.
        /// </summary>
        public void ShowRecordSavedMessage()
        {
            
        }

        /// <summary>
        /// Called when [read only mode set].
        /// </summary>
        /// <param name="readOnlyValue">if set to <c>true</c> [read only value].</param>
        public void OnReadOnlyModeSet(bool readOnlyValue)
        {
            
        }

        /// <summary>
        /// Determines whether [is maintenance key down] [the specified key].
        /// </summary>
        /// <param name="key">The key.</param>
        /// <returns><c>true</c> if [is maintenance key down] [the specified key]; otherwise, <c>false</c>.</returns>
        public bool IsMaintenanceKeyDown(MaintenanceKey key)
        {
            return false;
        }

        /// <summary>
        /// Activates this instance.
        /// </summary>
        public void Activate()
        {
            
        }

        /// <summary>
        /// Sets the window read only mode.
        /// </summary>
        public void SetWindowReadOnlyMode()
        {
            
        }

        /// <summary>
        /// Shows the record lock window.
        /// </summary>
        /// <param name="lockKey">The lock key.</param>
        /// <param name="message">The message.</param>
        /// <param name="inputParameter">The input parameter.</param>
        /// <returns><c>true</c> if XXXX, <c>false</c> otherwise.</returns>
        public bool ShowRecordLockWindow(PrimaryKeyValue lockKey, string message, object inputParameter)
        {
            return true;
        }

        /// <summary>
        /// Checks the delete tables.
        /// </summary>
        /// <param name="deleteTables">The delete tables.</param>
        /// <returns><c>true</c> if XXXX, <c>false</c> otherwise.</returns>
        public bool CheckDeleteTables(DeleteTables deleteTables)
        {
            return true;
        }

        /// <summary>
        /// Prints the output.
        /// </summary>
        /// <param name="printerSetupArgs">The printer setup arguments.</param>
        /// <exception cref="System.NotImplementedException"></exception>
        public void PrintOutput(PrinterSetupArgs printerSetupArgs)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Sets the save status.
        /// </summary>
        /// <param name="message">The message.</param>
        /// <param name="alertLevel">The alert level.</param>
        public void SetSaveStatus(string message, AlertLevels alertLevel)
        {
            
        }

        public void SetPendingSaveStatus(string messaage)
        {
            
        }

        /// <summary>
        /// Gets the automatic fills.
        /// </summary>
        /// <returns>List&lt;DbAutoFillMap&gt;.</returns>
        public List<DbAutoFillMap> GetAutoFills()
        {
            return null;
        }

        /// <summary>
        /// Handles the automatic fill value fail.
        /// </summary>
        /// <param name="autoFillMap">The automatic fill map.</param>
        public void HandleAutoFillValFail(DbAutoFillMap autoFillMap)
        {
            
        }

        /// <summary>
        /// Gets the delete procedure.
        /// </summary>
        /// <param name="deleteTables">The delete tables.</param>
        /// <returns>ITwoTierProcessingProcedure.</returns>
        public ITwoTierProcessingProcedure GetDeleteProcedure(DeleteTables deleteTables)
        {
            return null;
        }

        /// <summary>
        /// Gets the pre delete procedure.
        /// </summary>
        /// <param name="fields">The fields.</param>
        /// <param name="deleteTables">The delete tables.</param>
        public void GetPreDeleteProcedure(
            List<FieldDefinition> fields
            , DeleteTables deleteTables)
        {
            
        }

        /// <summary>
        /// Sets the window read only mode.
        /// </summary>
        /// <param name="readOnlyMode">if set to <c>true</c> [read only mode].</param>
        public void SetWindowReadOnlyMode(bool readOnlyMode)
        {
            
        }
    }
}
