using System;
using System.Collections.Generic;
using RingSoft.DbLookup.GetDataProcessor;
using RingSoft.DbLookup.Lookup;
using RingSoft.DbLookup.ModelDefinition;

namespace RingSoft.DbLookup
{
    /// <summary>
    /// Arguments passed in by the TableProcessing event to show which table is being processed by the Entity Framework.
    /// </summary>
    public class TableProcessingArgs
    {
        /// <summary>
        /// Gets the table definition that's being processed.
        /// </summary>
        /// <value>
        /// The table definition.
        /// </value>
        public TableDefinitionBase TableDefinition { get; }

        /// <summary>
        /// Gets the status.
        /// </summary>
        /// <value>
        /// The status.
        /// </value>
        public string Status { get; }

        /// <summary>
        /// Initializes a new instance of the <see cref="TableProcessingArgs"/> class.
        /// </summary>
        /// <param name="tableDefinition">The table definition.</param>
        /// <param name="status">The status.</param>
        public TableProcessingArgs(TableDefinitionBase tableDefinition, string status)
        {
            TableDefinition = tableDefinition;

            Status = status;
        }
    }

    // ReSharper disable once InconsistentNaming
    /// <summary>
    /// Contains all the table definitions used in this context.
    /// </summary>
    public abstract class LookupContextBase
    {
        /// <summary>
        /// Gets the table definitions.
        /// </summary>
        /// <value>
        /// The table definitions.
        /// </value>
        public IReadOnlyList<TableDefinitionBase> TableDefinitions => _tables;

        /// <summary>
        /// Gets the data processor.
        /// </summary>
        /// <value>
        /// The data processor.
        /// </value>
        public abstract DbDataProcessor DataProcessor { get; }

        public bool Initialized { get; private set; }

        /// <summary>
        /// Occurs when a user wishes to view a selected lookup row.  Used to show the appropriate editor for the selected lookup row.
        /// </summary>
        public event EventHandler<LookupAddViewArgs> LookupAddView;

        /// <summary>
        /// Occurs when a table is being processed by the Entity Framework.
        /// </summary>
        public event EventHandler<TableProcessingArgs> TableProcessing;

        private readonly List<TableDefinitionBase> _tables = new List<TableDefinitionBase>();

        public LookupContextBase()
        {
            var properties = GetType().GetProperties();
            var entityDefinitionName = $"{nameof(TableDefinition<object>)}`1";
            foreach (var property in properties)
            {
                if (property.PropertyType.Name == entityDefinitionName)
                {
                    var instance = Activator.CreateInstance(property.PropertyType, this, property.Name);
                    property.SetValue(this, instance);
                }
            }
        }

        /// <summary>
        /// Initializes this instance.  Creates the primary and foreign keys.
        /// </summary>
        public void Initialize()
        {
            if (!Initialized)
            {
                BaseInitializeTableDefinitions();

                BaseInitializePrimaryKeys();

                BaseInitializeFieldDefinitions();

                Initialized = true;

                InitializeLookupDefinitions();
            }
        }

        protected abstract void BaseInitializeTableDefinitions();

        protected abstract void BaseInitializeFieldDefinitions();

        protected abstract void BaseInitializePrimaryKeys();

        /// <summary>
        /// Called by Initialize for derived classes to create lookup definitions and attach them to tables.
        /// </summary>
        protected abstract void InitializeLookupDefinitions();

        internal void AddTable(TableDefinitionBase table)
        {
            _tables.Add(table);
        }

        /// <summary>
        /// Occurs when a user wishes to view a selected lookup/autofill primary key value.  Fires the ViewSelectedPrimaryKey event.
        /// </summary>
        /// <param name="e">The lookup primary key arguments.</param>
        public virtual void OnAddViewLookup(LookupAddViewArgs e)
        {
            LookupAddView?.Invoke(this, e);
        }

        /// <summary>
        /// Occurs when a table is being processed by the Entity Framework.
        /// </summary>
        /// <param name="e">The table processing arguments.</param>
        protected virtual void OnTableProcessing(TableProcessingArgs e)
        {
            TableProcessing?.Invoke(this, e);
        }
    }
}
