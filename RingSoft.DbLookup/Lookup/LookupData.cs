using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Linq.Expressions;
using RingSoft.DataEntryControls.Engine;

namespace RingSoft.DbLookup.Lookup
{
    /// <summary>
    /// The arguments sent to the LookupDataChanged event.
    /// </summary>
    /// <typeparam name="TLookupEntity">The lookup entity.</typeparam>
    /// <typeparam name="TEntity">The entity</typeparam>
    public class LookupDataChangedArgs<TLookupEntity, TEntity> 
        where TLookupEntity : new() where TEntity : new()
    {
        // ReSharper disable once UnusedAutoPropertyAccessor.Global
        /// <summary>
        /// Gets the lookup data.
        /// </summary>
        /// <value>
        /// The lookup data.
        /// </value>
        public LookupData<TLookupEntity, TEntity> LookupData { get; }

        /// <summary>
        /// Initializes a new instance of the <see cref="LookupDataChangedArgs{TLookupEntity, TEntity}"/> class.
        /// </summary>
        /// <param name="lookupData">The lookup data.</param>
        public LookupDataChangedArgs(LookupData<TLookupEntity, TEntity> lookupData)
        {
            LookupData = lookupData;
        }
    }

    /// <summary>
    /// A lookup's data.
    /// </summary>
    /// <typeparam name="TLookupEntity">The type of the lookup entity.</typeparam>
    /// <typeparam name="TEntity">The type of the entity.</typeparam>
    /// <seealso cref="LookupDataBase" />
    public class LookupData<TLookupEntity, TEntity> : LookupDataBase 
        where TLookupEntity : new() where TEntity : new()
    {
        /// <summary>
        /// Gets the lookup results list.
        /// </summary>
        /// <value>
        /// The lookup results list.
        /// </value>
        public IReadOnlyList<TLookupEntity> LookupResultsList => _lookupResults;

        /// <summary>
        /// Gets the selected item.
        /// </summary>
        /// <value>
        /// The selected item.
        /// </value>
        public TLookupEntity SelectedItem
        {
            get
            {
                if (SelectedRowIndex >= 0 && SelectedRowIndex < _lookupResults.Count)
                    return _lookupResults[SelectedRowIndex];

                return new TLookupEntity();
            }
        }

        /// <summary>
        /// Occurs when this object's data changes.
        /// </summary>
        public new event EventHandler<LookupDataChangedArgs<TLookupEntity, TEntity>> LookupDataChanged;

        private List<TLookupEntity> _lookupResults = new List<TLookupEntity>();

        /// <summary>
        /// Initializes a new instance of the <see cref="LookupData{TLookupEntity, TEntity}"/> class.
        /// </summary>
        /// <param name="lookupDefinition">The lookup definition.</param>
        /// <param name="userInterface">The user interface.</param>
        public LookupData(LookupDefinition<TLookupEntity, TEntity> lookupDefinition, ILookupControl userInterface)
            : base(lookupDefinition, userInterface)
        {
        }

        protected override void ProcessLookupData()
        {
            base.ProcessLookupData();
            _lookupResults.Clear();
            _lookupResults = GetLookupResultsListFromLookupData(this);

            var output = new LookupDataChangedArgs<TLookupEntity, TEntity>(this);
            LookupDataChanged?.Invoke(this, output);
        }

        /// <summary>
        /// Gets the lookup results list from lookup data.
        /// </summary>
        /// <param name="lookupData">The lookup data.</param>
        /// <returns></returns>
        public List<TLookupEntity> GetLookupResultsListFromLookupData(LookupDataBase lookupData)
        {
            var lookupResults = new List<TLookupEntity>();

            if (lookupData.LookupResultsDataTable != null)
            {
                foreach (DataRow dataRow in lookupData.LookupResultsDataTable.Rows)
                {
                    lookupResults.Add(GetEntityFromDataRow(dataRow));
                }
            }

            return lookupResults;
        }


        private TLookupEntity GetEntityFromDataRow(DataRow dataRow)
        {
            var entity = (TLookupEntity)Activator.CreateInstance(typeof(TLookupEntity));

            foreach (var lookupDefinitionVisibleColumn in LookupDefinition.VisibleColumns)
            {
                ProcessColumn(entity, dataRow, lookupDefinitionVisibleColumn);
            }

            foreach (var lookupDefinitionHiddenColumn in LookupDefinition.HiddenColumns)
            {
                ProcessColumn(entity, dataRow, lookupDefinitionHiddenColumn);
            }

            return entity;
        }

        private void ProcessColumn(TLookupEntity listItem, DataRow dataRow, LookupColumnDefinitionBase column)
        {
            if (column.PropertyName.IsNullOrEmpty())
                return;

            var value = dataRow.GetRowValue(column.SelectSqlAlias);
            GblMethods.SetPropertyValue(listItem, column.PropertyName, value);
        }

        /// <summary>
        /// Called when a column's header is clicked.
        /// </summary>
        /// <param name="property">The property.</param>
        /// <param name="resetSortOrder">if set to <c>true</c> [reset sort order].</param>
        public void OnColumnClick(Expression<Func<TLookupEntity, object>> property, bool resetSortOrder = true)
        {
            var column =
                LookupDefinition.VisibleColumns.FirstOrDefault(f => f.PropertyName == property.GetFullPropertyName());
            if (column != null)
            {
                OnColumnClick(LookupDefinition.GetIndexOfVisibleColumn(column), resetSortOrder);
            }
        }
    }
}
