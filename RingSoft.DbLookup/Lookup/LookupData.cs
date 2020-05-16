using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;

namespace RingSoft.DbLookup.Lookup
{
    /// <summary>
    /// The arguments sent to the LookupDataChanged event.
    /// </summary>
    /// <typeparam name="TLookupEntity">The lookup entity.</typeparam>
    public class LookupDataChangedArgs<TLookupEntity> where TLookupEntity : new()
    {
        // ReSharper disable once UnusedAutoPropertyAccessor.Global
        /// <summary>
        /// Gets the lookup data.
        /// </summary>
        /// <value>
        /// The lookup data.
        /// </value>
        public LookupData<TLookupEntity> LookupData { get; }

        /// <summary>
        /// Initializes a new instance of the <see cref="LookupDataChangedArgs{TLookupEntity}"/> class.
        /// </summary>
        /// <param name="lookupData">The lookup data.</param>
        public LookupDataChangedArgs(LookupData<TLookupEntity> lookupData)
        {
            LookupData = lookupData;
        }
    }

    /// <summary>
    /// A lookup's data.
    /// </summary>
    /// <typeparam name="TLookupEntity">The type of the lookup entity.</typeparam>
    /// <seealso cref="LookupDataBase" />
    public class LookupData<TLookupEntity> : LookupDataBase where TLookupEntity : new()
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
        public new event EventHandler<LookupDataChangedArgs<TLookupEntity>> LookupDataChanged;

        private List<TLookupEntity> _lookupResults = new List<TLookupEntity>();
        private LookupEntityDefinition<TLookupEntity> _lookupEntityDefinition;

        /// <summary>
        /// Initializes a new instance of the <see cref="LookupData{TLookupEntity}"/> class.
        /// </summary>
        /// <param name="lookupDefinition">The lookup definition.</param>
        /// <param name="userInterface">The user interface.</param>
        public LookupData(LookupEntityDefinition<TLookupEntity> lookupDefinition, ILookupControl userInterface)
            : base(lookupDefinition, userInterface)
        {
            _lookupEntityDefinition = lookupDefinition;
        }

        protected override void ProcessLookupData()
        {
            base.ProcessLookupData();
            _lookupResults.Clear();
            _lookupResults = _lookupEntityDefinition.GetLookupResultsListFromLookupData(this);

            var output = new LookupDataChangedArgs<TLookupEntity>(this);
            LookupDataChanged?.Invoke(this, output);
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
