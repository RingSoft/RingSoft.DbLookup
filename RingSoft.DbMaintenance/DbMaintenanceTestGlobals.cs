// ***********************************************************************
// Assembly         : RingSoft.DbMaintenance
// Author           : petem
// Created          : 05-22-2023
//
// Last Modified By : petem
// Last Modified On : 09-03-2024
// ***********************************************************************
// <copyright file="DbMaintenanceTestGlobals.cs" company="Peter Ringering">
//     Copyright (c) 2023. All rights reserved.
// </copyright>
// <summary></summary>
// ***********************************************************************
using RingSoft.DataEntryControls.Engine;
using RingSoft.DbLookup;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using RingSoft.DbLookup.Lookup;
using RingSoft.DbLookup.ModelDefinition.FieldDefinitions;
using RingSoft.DbLookup.Testing;

namespace RingSoft.DbMaintenance
{
    /// <summary>
    /// Class TestTwoTierProcedure.
    /// Implements the <see cref="ITwoTierProcessingProcedure" />
    /// </summary>
    /// <seealso cref="ITwoTierProcessingProcedure" />
    public class TestTwoTierProcedure : ITwoTierProcessingProcedure
    {
        /// <summary>
        /// Sets the progress.
        /// </summary>
        /// <param name="topMax">The top maximum.</param>
        /// <param name="topValue">The top value.</param>
        /// <param name="topText">The top text.</param>
        /// <param name="bottomMax">The bottom maximum.</param>
        /// <param name="bottomValue">The bottom value.</param>
        /// <param name="bottomText">The bottom text.</param>
        public void SetProgress(int topMax = 0, int topValue = 0, string topText = "", int bottomMax = 0, int bottomValue = 0,
            string bottomText = "")
        {
            
        }
    }
    /// <summary>
    /// Class DbMaintenanceTestGlobals.  Used to unit test a DbMaintenanceViewModel.
    /// Implements the <see cref="IControlsUserInterface" />
    /// Implements the <see cref="RingSoft.DbMaintenance.IDbMaintenanceDataProcessor" />
    /// </summary>
    /// <typeparam name="TViewModel">The type of the t view model.</typeparam>
    /// <typeparam name="TView">The type of the t view.</typeparam>
    /// <seealso cref="IControlsUserInterface" />
    /// <seealso cref="RingSoft.DbMaintenance.IDbMaintenanceDataProcessor" />
    public class DbMaintenanceTestGlobals<TViewModel, TView> 
        : IControlsUserInterface
    , IDbMaintenanceDataProcessor

        where TViewModel : DbMaintenanceViewModelBase
        where TView : IDbMaintenanceView, new()
    {
        /// <summary>
        /// Gets the view model.
        /// </summary>
        /// <value>The view model.</value>
        public TViewModel ViewModel { get; private set; }

        /// <summary>
        /// Gets the view.
        /// </summary>
        /// <value>The view.</value>
        public TView View { get; private set; }


        /// <summary>
        /// Gets the data repository.
        /// </summary>
        /// <value>The data repository.</value>
        public TestDataRepository DataRepository { get; }

        /// <summary>
        /// Gets or sets the message box result.
        /// </summary>
        /// <value>The message box result.</value>
        public MessageBoxButtonsResult MessageBoxResult { get; set; } = MessageBoxButtonsResult.Yes;

        public event EventHandler PreInitializeEvent;

        /// <summary>
        /// Initializes a new instance of the <see cref="DbMaintenanceTestGlobals{TViewModel, TView}" /> class.
        /// </summary>
        /// <param name="dataDepository">The data depository.</param>
        public DbMaintenanceTestGlobals(TestDataRepository dataDepository)
        {
            DataRepository = dataDepository;
        }

        /// <summary>
        /// Initializes this instance.
        /// </summary>
        public virtual void Initialize()
        {
            ControlsGlobals.UserInterface = this;
            SystemGlobals.UnitTestMode = true;
            DataRepository.Initialize();

            var viewModel = (TViewModel)Activator.CreateInstance(typeof(TViewModel));
            ViewModel = viewModel;
            ViewModel.Processor = this;

            var view = (TView)Activator.CreateInstance(typeof(TView));
            View = view;

            ViewModel.CheckDirtyFlag = false;

            ViewModel.PreInitializeEvent += (sender, args) =>
            {
                PreInitializeEvent?.Invoke(this, EventArgs.Empty);
            };
            ViewModel.OnViewLoaded(View);
        }

        /// <summary>
        /// Clears the data.
        /// </summary>
        public virtual void ClearData()
        {

            ViewModel.NewCommand.Execute(null);
            DataRepository.ClearData();
        }

        /// <summary>
        /// Sets the window cursor.
        /// </summary>
        /// <param name="cursor">The cursor.</param>
        public virtual void SetWindowCursor(WindowCursorTypes cursor)
        {

        }

        public WindowCursorTypes GetWindowCursor()
        {
            return WindowCursorTypes.Default;
        }

        /// <summary>
        /// Shows the message box.
        /// </summary>
        /// <param name="text">The text.</param>
        /// <param name="caption">The caption.</param>
        /// <param name="icon">The icon.</param>
        /// <returns>Task.</returns>
        public virtual async Task ShowMessageBox(string text, string caption, RsMessageBoxIcons icon)
        {

        }

        /// <summary>
        /// Shows the yes no message box.
        /// </summary>
        /// <param name="text">The text.</param>
        /// <param name="caption">The caption.</param>
        /// <param name="playSound">if set to <c>true</c> [play sound].</param>
        /// <returns>MessageBoxButtonsResult.</returns>
        public virtual async Task<MessageBoxButtonsResult> ShowYesNoMessageBox(string text, string caption, bool playSound = false)
        {
            return MessageBoxResult;
        }

        /// <summary>
        /// Shows the yes no cancel message box.
        /// </summary>
        /// <param name="text">The text.</param>
        /// <param name="caption">The caption.</param>
        /// <param name="playSound">if set to <c>true</c> [play sound].</param>
        /// <returns>MessageBoxButtonsResult.</returns>
        public virtual async Task<MessageBoxButtonsResult> ShowYesNoCancelMessageBox(string text, string caption, bool playSound = false)
        {
            return MessageBoxResult;
        }

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
        /// <summary>
        /// Occurs when [lookup add view].
        /// </summary>
        public event EventHandler<LookupAddViewArgs> LookupAddView;
        /// <summary>
        /// Initializes from lookup data.
        /// </summary>
        /// <param name="e">The e.</param>
        public virtual void InitializeFromLookupData(LookupAddViewArgs e)
        {
            
        }

        /// <summary>
        /// Called when [validation fail].
        /// </summary>
        /// <param name="fieldDefinition">The field definition.</param>
        /// <param name="text">The text.</param>
        /// <param name="caption">The caption.</param>
        public virtual void OnValidationFail(FieldDefinition fieldDefinition, string text, string caption)
        {
            
        }

        /// <summary>
        /// Called when [record selected].
        /// </summary>
        public virtual void OnRecordSelected()
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
        public virtual void ShowFindLookupWindow(LookupDefinitionBase lookupDefinition, bool allowAdd, bool allowView, string initialSearchFor,
            PrimaryKeyValue initialSearchForPrimaryKey)
        {
            
        }

        /// <summary>
        /// Closes the window.
        /// </summary>
        public virtual void CloseWindow()
        {
            
        }

        /// <summary>
        /// Shows the yes no cancel message.
        /// </summary>
        /// <param name="text">The text.</param>
        /// <param name="caption">The caption.</param>
        /// <param name="playSound">if set to <c>true</c> [play sound].</param>
        /// <returns>MessageButtons.</returns>
        public virtual MessageButtons ShowYesNoCancelMessage(string text, string caption, bool playSound = false)
        {
            return MessageButtons.Yes;
        }

        /// <summary>
        /// Shows the yes no message.
        /// </summary>
        /// <param name="text">The text.</param>
        /// <param name="caption">The caption.</param>
        /// <param name="playSound">if set to <c>true</c> [play sound].</param>
        /// <returns><c>true</c> if XXXX, <c>false</c> otherwise.</returns>
        public virtual bool ShowYesNoMessage(string text, string caption, bool playSound = false)
        {
            return MessageBoxResult == MessageBoxButtonsResult.Yes;
        }

        /// <summary>
        /// Shows the record saved message.
        /// </summary>
        public virtual void ShowRecordSavedMessage()
        {
            
        }

        /// <summary>
        /// Called when [read only mode set].
        /// </summary>
        /// <param name="readOnlyValue">if set to <c>true</c> [read only value].</param>
        public virtual void OnReadOnlyModeSet(bool readOnlyValue)
        {
            
        }

        /// <summary>
        /// Determines whether [is maintenance key down] [the specified key].
        /// </summary>
        /// <param name="key">The key.</param>
        /// <returns><c>true</c> if [is maintenance key down] [the specified key]; otherwise, <c>false</c>.</returns>
        public virtual bool IsMaintenanceKeyDown(MaintenanceKey key)
        {
            return false;
        }

        /// <summary>
        /// Activates this instance.
        /// </summary>
        public virtual void Activate()
        {
            
        }

        /// <summary>
        /// Sets the window read only mode.
        /// </summary>
        public virtual void SetWindowReadOnlyMode()
        {
            
        }

        /// <summary>
        /// Shows the record lock window.
        /// </summary>
        /// <param name="lockKey">The lock key.</param>
        /// <param name="message">The message.</param>
        /// <param name="inputParameter">The input parameter.</param>
        /// <returns><c>true</c> if XXXX, <c>false</c> otherwise.</returns>
        public virtual bool ShowRecordLockWindow(PrimaryKeyValue lockKey, string message, object inputParameter)
        {
            return true;
        }

        /// <summary>
        /// Checks the delete tables.
        /// </summary>
        /// <param name="deleteTables">The delete tables.</param>
        /// <returns><c>true</c> if XXXX, <c>false</c> otherwise.</returns>
        public virtual bool CheckDeleteTables(DeleteTables deleteTables)
        {
            return true;
        }

        /// <summary>
        /// Prints the output.
        /// </summary>
        /// <param name="printerSetupArgs">The printer setup arguments.</param>
        public virtual void PrintOutput(PrinterSetupArgs printerSetupArgs)
        {
            
        }

        /// <summary>
        /// Sets the save status.
        /// </summary>
        /// <param name="message">The message.</param>
        /// <param name="alertLevel">The alert level.</param>
        public virtual void SetSaveStatus(string message, AlertLevels alertLevel)
        {
            
        }

        /// <summary>
        /// Gets the automatic fills.
        /// </summary>
        /// <returns>List&lt;DbAutoFillMap&gt;.</returns>
        public virtual List<DbAutoFillMap> GetAutoFills()
        {
            return null;
        }

        /// <summary>
        /// Handles the automatic fill value fail.
        /// </summary>
        /// <param name="autoFillMap">The automatic fill map.</param>
        public virtual void HandleAutoFillValFail(DbAutoFillMap autoFillMap)
        {
            
        }

        /// <summary>
        /// Gets the delete procedure.
        /// </summary>
        /// <param name="deleteTables">The delete tables.</param>
        /// <returns>ITwoTierProcessingProcedure.</returns>
        public ITwoTierProcessingProcedure GetDeleteProcedure(DeleteTables deleteTables)
        {
            var result = new TestTwoTierProcedure();
            ViewModel.Processor.DeleteChildrenResult = ViewModel.DeleteChildren(deleteTables, result);
            return result;
        }

        /// <summary>
        /// Gets the pre delete procedure.
        /// </summary>
        /// <param name="fields">The fields.</param>
        /// <param name="deleteTables">The delete tables.</param>
        public void GetPreDeleteProcedure(List<FieldDefinition> fields, DeleteTables deleteTables)
        {
            var procedure = new TestTwoTierProcedure();
            ViewModel.Processor.PreDeleteResult = ViewModel.DoGetDeleteTables(fields, deleteTables, procedure);
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
