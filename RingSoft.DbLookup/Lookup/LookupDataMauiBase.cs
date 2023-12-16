// ***********************************************************************
// Assembly         : RingSoft.DbLookup
// Author           : petem
// Created          : 06-09-2023
//
// Last Modified By : petem
// Last Modified On : 11-23-2023
// ***********************************************************************
// <copyright file="LookupDataMauiBase.cs" company="Peter Ringering">
//     Copyright (c) 2023. All rights reserved.
// </copyright>
// <summary></summary>
// ***********************************************************************
using System;
using System.Collections.Generic;
using RingSoft.Printing.Interop;

namespace RingSoft.DbLookup.Lookup
{
    /// <summary>
    /// Enum LookupScrollPositions
    /// </summary>
    public enum LookupScrollPositions
    {
        /// <summary>
        /// The disabled
        /// </summary>
        Disabled = 0,
        /// <summary>
        /// The top
        /// </summary>
        Top = 1,
        /// <summary>
        /// The middle
        /// </summary>
        Middle = 2,
        /// <summary>
        /// The bottom
        /// </summary>
        Bottom = 3
    }

    /// <summary>
    /// Interface ILookupDataBase
    /// </summary>
    public interface ILookupDataBase
    {
        /// <summary>
        /// Occurs when [print output].
        /// </summary>
        event EventHandler<LookupDataMauiPrintOutput> PrintOutput;

        /// <summary>
        /// Gets the record count.
        /// </summary>
        /// <returns>System.Int32.</returns>
        int GetRecordCount();

        /// <summary>
        /// Does the print output.
        /// </summary>
        /// <param name="pageSize">Size of the page.</param>
        void DoPrintOutput(int pageSize);
    }

    /// <summary>
    /// Arguments sent when the lookup's selected row index changes.
    /// </summary>
    public class SelectedIndexChangedEventArgs
    {
        /// <summary>
        /// Creates new index.
        /// </summary>
        /// <value>The new index.</value>
        public int NewIndex { get; }

        /// <summary>
        /// Initializes a new instance of the <see cref="SelectedIndexChangedEventArgs" /> class.
        /// </summary>
        /// <param name="newIndex">The new index.</param>
        public SelectedIndexChangedEventArgs(int newIndex)
        {
            NewIndex = newIndex;
        }
    }

    /// <summary>
    /// Class LookupDataMauiOutput.
    /// </summary>
    public class LookupDataMauiOutput
    {
        /// <summary>
        /// Gets the scroll position.
        /// </summary>
        /// <value>The scroll position.</value>
        public LookupScrollPositions ScrollPosition { get; }

        /// <summary>
        /// Initializes a new instance of the <see cref="LookupDataMauiOutput"/> class.
        /// </summary>
        /// <param name="scrollPosition">The scroll position.</param>
        public LookupDataMauiOutput(LookupScrollPositions scrollPosition)
        {
            ScrollPosition = scrollPosition;
        }
    }

    /// <summary>
    /// The base class of LookupDataMaui
    /// </summary>
    public class LookupDataMauiPrintOutput
    {
        /// <summary>
        /// Gets the result.
        /// </summary>
        /// <value>The result.</value>
        public List<PrimaryKeyValue> Result { get; }

        /// <summary>
        /// Gets or sets a value indicating whether this <see cref="LookupDataMauiPrintOutput"/> is abort.
        /// </summary>
        /// <value><c>true</c> if abort; otherwise, <c>false</c>.</value>
        public bool Abort { get; set; }

        /// <summary>
        /// Initializes a new instance of the <see cref="LookupDataMauiPrintOutput"/> class.
        /// </summary>
        public LookupDataMauiPrintOutput()
        {
            Result = new List<PrimaryKeyValue>();
        }
    }
    /// <summary>
    /// Class LookupDataMauiBase.
    /// Implements the <see cref="RingSoft.DbLookup.Lookup.ILookupDataBase" />
    /// </summary>
    /// <seealso cref="RingSoft.DbLookup.Lookup.ILookupDataBase" />
    public abstract class LookupDataMauiBase : ILookupDataBase
    {
        /// <summary>
        /// Gets the row count.
        /// </summary>
        /// <value>The row count.</value>
        public abstract int RowCount { get; }

        /// <summary>
        /// Gets the order by list.
        /// </summary>
        /// <value>The order by list.</value>
        public List<LookupFieldColumnDefinition> OrderByList { get; }

        /// <summary>
        /// Gets the lookup definition.
        /// </summary>
        /// <value>The lookup definition.</value>
        public LookupDefinitionBase LookupDefinition { get; }

        /// <summary>
        /// Gets or sets the parent window's primary key value.
        /// </summary>
        /// <value>The parent window's primary key value.</value>
        public PrimaryKeyValue ParentWindowPrimaryKeyValue { get; set; }

        /// <summary>
        /// Gets the lookup control.
        /// </summary>
        /// <value>The lookup control.</value>
        public ILookupControl LookupControl { get; private set; }

        /// <summary>
        /// Gets the lookup window.
        /// </summary>
        /// <value>The lookup window.</value>
        public ILookupWindow LookupWindow { get; private set; }

        /// <summary>
        /// Gets or sets the scroll position.
        /// </summary>
        /// <value>The scroll position.</value>
        public LookupScrollPositions ScrollPosition { get; protected internal set; }

        public bool DbMaintenanceMode { get; set; }

        /// <summary>
        /// Occurs when a user wishes to view a selected lookup row.  Used to show the appropriate editor for the selected lookup row.
        /// </summary>
        public event EventHandler<LookupAddViewArgs> LookupView;

        /// <summary>
        /// Occurs when [print output].
        /// </summary>
        public event EventHandler<LookupDataMauiPrintOutput> PrintOutput;
        /// <summary>
        /// Occurs when [lookup data changed].
        /// </summary>
        public event EventHandler<LookupDataMauiOutput> LookupDataChanged;
        /// <summary>
        /// Occurs when [data source changed].
        /// </summary>
        public event EventHandler DataSourceChanged;

        /// <summary>
        /// Initializes a new instance of the <see cref="LookupDataMauiBase"/> class.
        /// </summary>
        /// <param name="lookupDefinition">The lookup definition.</param>
        public LookupDataMauiBase(LookupDefinitionBase lookupDefinition)
        {
            SystemGlobals.LookupContext.Initialize();
            LookupDefinition = lookupDefinition;
            OrderByList = new List<LookupFieldColumnDefinition>();
        }

        /// <summary>
        /// Fires the lookup data changed event.
        /// </summary>
        /// <param name="lookupOutput">The lookup output.</param>
        protected void FireLookupDataChangedEvent(LookupDataMauiOutput lookupOutput)
        {
            ScrollPosition = lookupOutput.ScrollPosition;
            LookupDataChanged?.Invoke(this, lookupOutput);
        }

        /// <summary>
        /// Fires the print output event.
        /// </summary>
        /// <param name="output">The output.</param>
        protected void FirePrintOutputEvent(LookupDataMauiPrintOutput output)
        {
            PrintOutput?.Invoke(this, output);
        }

        /// <summary>
        /// Gets the initialize data.
        /// </summary>
        public abstract void GetInitData();

        /// <summary>
        /// Gets the formatted row value.
        /// </summary>
        /// <param name="rowIndex">Index of the row.</param>
        /// <param name="column">The column.</param>
        /// <returns>System.String.</returns>
        public abstract string GetFormattedRowValue(int rowIndex, LookupColumnDefinitionBase column);

        /// <summary>
        /// Gets the database row value.
        /// </summary>
        /// <param name="rowIndex">Index of the row.</param>
        /// <param name="column">The column.</param>
        /// <returns>System.String.</returns>
        public abstract string GetDatabaseRowValue(int rowIndex, LookupColumnDefinitionBase column);

        /// <summary>
        /// Gets the record count.
        /// </summary>
        /// <returns>System.Int32.</returns>
        public abstract int GetRecordCount();

        /// <summary>
        /// Clears the data.
        /// </summary>
        public abstract void ClearData();

        /// <summary>
        /// Gets or sets a value indicating whether [input mode].
        /// </summary>
        /// <value><c>true</c> if [input mode]; otherwise, <c>false</c>.</value>
        public bool InputMode { get; protected set; }

        /// <summary>
        /// Gets the selected primary key value.
        /// </summary>
        /// <value>The selected primary key value.</value>
        public PrimaryKeyValue SelectedPrimaryKeyValue { get; internal set; }

        /// <summary>
        /// Gets the primary key value for search text.
        /// </summary>
        /// <param name="searchText">The search text.</param>
        /// <returns>PrimaryKeyValue.</returns>
        public abstract PrimaryKeyValue GetPrimaryKeyValueForSearchText(string searchText);

        /// <summary>
        /// Selects the primary key.
        /// </summary>
        /// <param name="primaryKeyValue">The primary key value.</param>
        public abstract void SelectPrimaryKey(PrimaryKeyValue primaryKeyValue);

        /// <summary>
        /// Sets the new primary key value.
        /// </summary>
        /// <param name="primaryKeyValue">The primary key value.</param>
        public abstract void SetNewPrimaryKeyValue(PrimaryKeyValue primaryKeyValue);

        /// <summary>
        /// Views the selected row.
        /// </summary>
        /// <param name="ownerWindow">The owner window.</param>
        /// <param name="AddViewParameter">The add view parameter.</param>
        /// <param name="readOnlyMode">if set to <c>true</c> [read only mode].</param>
        public abstract void ViewSelectedRow(object ownerWindow, object AddViewParameter, bool readOnlyMode = false);

        /// <summary>
        /// Adds the new row.
        /// </summary>
        /// <param name="ownerWindow">The owner window.</param>
        /// <param name="inputParameter">The input parameter.</param>
        public abstract void AddNewRow(object ownerWindow, object inputParameter = null);

        /// <summary>
        /// Refreshes the data.
        /// </summary>
        /// <param name="newText">The new text.</param>
        public abstract void RefreshData(string newText = "");

        /// <summary>
        /// Called when [column click].
        /// </summary>
        /// <param name="column">The column.</param>
        /// <param name="resetSortOrder">if set to <c>true</c> [reset sort order].</param>
        public abstract void OnColumnClick(LookupFieldColumnDefinition column, bool resetSortOrder);

        /// <summary>
        /// Called when [mouse wheel forward].
        /// </summary>
        public abstract void OnMouseWheelForward();

        /// <summary>
        /// Called when [mouse wheel back].
        /// </summary>
        public abstract void OnMouseWheelBack();

        /// <summary>
        /// Determines whether [is there data].
        /// </summary>
        /// <returns><c>true</c> if [is there data]; otherwise, <c>false</c>.</returns>
        public abstract bool IsThereData();

        /// <summary>
        /// Sets the parent controls.
        /// </summary>
        /// <param name="control">The control.</param>
        /// <param name="lookupWindow">The lookup window.</param>
        public void SetParentControls(ILookupControl control, ILookupWindow lookupWindow = null)
        {
            LookupControl = control;
            LookupWindow = lookupWindow;
        }

        /// <summary>
        /// Gets the selected text.
        /// </summary>
        /// <returns>System.String.</returns>
        public abstract string GetSelectedText();

        /// <summary>
        /// Occurs when a user wishes to view a selected lookup row.  Fires the LookupView event.
        /// </summary>
        /// <param name="e">The lookup primary key row arguments.</param>
        protected virtual void OnLookupView(LookupAddViewArgs e)
        {
            LookupView?.Invoke(this, e);
        }

        /// <summary>
        /// Called when [data source changed].
        /// </summary>
        protected virtual void OnDataSourceChanged()
        {
            DataSourceChanged?.Invoke(this, EventArgs.Empty);
        }

        /// <summary>
        /// Gets the selected primary key value.
        /// </summary>
        /// <returns>PrimaryKeyValue.</returns>
        public abstract PrimaryKeyValue GetSelectedPrimaryKeyValue();

        /// <summary>
        /// Gotoes the top.
        /// </summary>
        public abstract void GotoTop();

        /// <summary>
        /// Gotoes the bottom.
        /// </summary>
        public abstract void GotoBottom();

        /// <summary>
        /// Gotoes the next record.
        /// </summary>
        public abstract void GotoNextRecord();

        /// <summary>
        /// Gotoes the previous record.
        /// </summary>
        public abstract void GotoPreviousRecord();

        /// <summary>
        /// Gotoes the next page.
        /// </summary>
        public abstract void GotoNextPage();

        /// <summary>
        /// Gotoes the previous page.
        /// </summary>
        public abstract void GotoPreviousPage();

        /// <summary>
        /// Called when [search for change].
        /// </summary>
        /// <param name="searchForText">The search for text.</param>
        /// <param name="initialValue">if set to <c>true</c> [initial value].</param>
        public abstract void OnSearchForChange(string searchForText, bool initialValue = false);

        /// <summary>
        /// Does the print output.
        /// </summary>
        /// <param name="pageSize">Size of the page.</param>
        public abstract void DoPrintOutput(int pageSize);

        /// <summary>
        /// Gets the printer header row.
        /// </summary>
        /// <param name="primaryKeyValue">The primary key value.</param>
        /// <param name="printerSetupArgs">The printer setup arguments.</param>
        /// <returns>PrintingInputHeaderRow.</returns>
        public abstract PrintingInputHeaderRow GetPrinterHeaderRow(PrimaryKeyValue primaryKeyValue
            , PrinterSetupArgs printerSetupArgs);

        /// <summary>
        /// Gets the entity for primary key.
        /// </summary>
        /// <param name="primaryKeyValue">The primary key value.</param>
        /// <returns>System.Object.</returns>
        public abstract object GetEntityForPrimaryKey(PrimaryKeyValue primaryKeyValue);

        /// <summary>
        /// Called when [size changed].
        /// </summary>
        public abstract void OnSizeChanged();
    }
}