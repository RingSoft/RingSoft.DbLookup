using System;
using System.Collections.Generic;
using RingSoft.Printing.Interop;

namespace RingSoft.DbLookup.Lookup
{
    public enum LookupScrollPositions
    {
        Disabled = 0,
        Top = 1,
        Middle = 2,
        Bottom = 3
    }

    public interface ILookupDataBase
    {
        event EventHandler<LookupDataMauiPrintOutput> PrintOutput;

        int GetRecordCount();

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
        /// <value>
        /// The new index.
        /// </value>
        public int NewIndex { get; }

        /// <summary>
        /// Initializes a new instance of the <see cref="SelectedIndexChangedEventArgs"/> class.
        /// </summary>
        /// <param name="newIndex">The new index.</param>
        public SelectedIndexChangedEventArgs(int newIndex)
        {
            NewIndex = newIndex;
        }
    }

    public class LookupDataMauiOutput
    {
        public LookupScrollPositions ScrollPosition { get; }

        public LookupDataMauiOutput(LookupScrollPositions scrollPosition)
        {
            ScrollPosition = scrollPosition;
        }
    }

    public class LookupDataMauiPrintOutput
    {
        public List<PrimaryKeyValue> Result { get; }

        public bool Abort { get; set; }

        public LookupDataMauiPrintOutput()
        {
            Result = new List<PrimaryKeyValue>();
        }
    }
    public abstract class LookupDataMauiBase : ILookupDataBase
    {
        public abstract int RowCount { get; }

        public List<LookupFieldColumnDefinition> OrderByList { get; }

        public LookupDefinitionBase LookupDefinition { get; }

        /// <summary>
        /// Gets or sets the parent window's primary key value.
        /// </summary>
                /// <value>
        /// The parent window's primary key value.
        /// </value>
        public PrimaryKeyValue ParentWindowPrimaryKeyValue { get; set; }

        public ILookupControl LookupControl { get; private set; }

        public ILookupWindow LookupWindow { get; private set; }

        public LookupScrollPositions ScrollPosition { get; protected internal set; }

        /// <summary>
        /// Occurs when a user wishes to view a selected lookup row.  Used to show the appropriate editor for the selected lookup row.
        /// </summary>
        public event EventHandler<LookupAddViewArgs> LookupView;

        public event EventHandler<LookupDataMauiPrintOutput> PrintOutput;
        public event EventHandler<LookupDataMauiOutput> LookupDataChanged;
        public event EventHandler DataSourceChanged;

        public LookupDataMauiBase(LookupDefinitionBase lookupDefinition)
        {
            LookupDefinition = lookupDefinition;
            OrderByList = new List<LookupFieldColumnDefinition>();
        }

        protected void FireLookupDataChangedEvent(LookupDataMauiOutput lookupOutput)
        {
            ScrollPosition = lookupOutput.ScrollPosition;
            LookupDataChanged?.Invoke(this, lookupOutput);
        }

        protected void FirePrintOutputEvent(LookupDataMauiPrintOutput output)
        {
            PrintOutput?.Invoke(this, output);
        }

        public abstract void GetInitData();

        public abstract string GetFormattedRowValue(int rowIndex, LookupColumnDefinitionBase column);

        public abstract string GetDatabaseRowValue(int rowIndex, LookupColumnDefinitionBase column);

        public abstract int GetRecordCount();

        public abstract void ClearData();

        public bool InputMode { get; protected set; }

        public PrimaryKeyValue SelectedPrimaryKeyValue { get; internal set; }

        public abstract PrimaryKeyValue GetPrimaryKeyValueForSearchText(string searchText);

        public abstract void SelectPrimaryKey(PrimaryKeyValue primaryKeyValue);

        public abstract void SetNewPrimaryKeyValue(PrimaryKeyValue primaryKeyValue);

        public abstract void ViewSelectedRow(object ownerWindow, object AddViewParameter, bool readOnlyMode = false);

        public abstract void AddNewRow(object ownerWindow, object inputParameter = null);

        public abstract void RefreshData(string newText = "");

        public abstract void OnColumnClick(LookupFieldColumnDefinition column, bool resetSortOrder);

        public abstract void OnMouseWheelForward();

        public abstract void OnMouseWheelBack();

        public abstract bool IsThereData();

        public void SetParentControls(ILookupControl control, ILookupWindow lookupWindow = null)
        {
            LookupControl = control;
            LookupWindow = lookupWindow;
        }

        public abstract string GetSelectedText();

        /// <summary>
        /// Occurs when a user wishes to view a selected lookup row.  Fires the LookupView event.
        /// </summary>
        /// <param name="e">The lookup primary key row arguments.</param>
        protected virtual void OnLookupView(LookupAddViewArgs e)
        {
            LookupView?.Invoke(this, e);
        }

        protected virtual void OnDataSourceChanged()
        {
            DataSourceChanged?.Invoke(this, EventArgs.Empty);
        }

        public abstract PrimaryKeyValue GetSelectedPrimaryKeyValue();

        public abstract void GotoTop();

        public abstract void GotoBottom();

        public abstract void GotoNextRecord();

        public abstract void GotoPreviousRecord();

        public abstract void GotoNextPage();
        
        public abstract void GotoPreviousPage();

        public abstract void OnSearchForChange(string searchForText, bool initialValue = false);

        public abstract void DoPrintOutput(int pageSize);

        public abstract PrintingInputHeaderRow GetPrinterHeaderRow(PrimaryKeyValue primaryKeyValue
            , PrinterSetupArgs printerSetupArgs);

        public abstract object GetEntityForPrimaryKey(PrimaryKeyValue primaryKeyValue);

        public abstract void OnSizeChanged();
    }
}