using System;
using System.Collections.Generic;

namespace RingSoft.DbLookup.Lookup
{
    public class LookupDataMauiOutput
    {
        public LookupScrollPositions ScrollPosition { get; }

        public LookupDataMauiOutput(LookupScrollPositions scrollPosition)
        {
            ScrollPosition = scrollPosition;
        }
    }
    public abstract class LookupDataMauiBase
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

        public LookupScrollPositions ScrollPosition { get; private set; }

        /// <summary>
        /// Occurs when a user wishes to view a selected lookup row.  Used to show the appropriate editor for the selected lookup row.
        /// </summary>
        public event EventHandler<LookupAddViewArgs> LookupView;

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
    }
}