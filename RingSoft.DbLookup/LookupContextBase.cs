using System;
using System.Collections.Generic;
using System.Linq;
using RingSoft.DataEntryControls.Engine;
using RingSoft.DbLookup.AutoFill;
using RingSoft.DbLookup.DataProcessor;
using RingSoft.DbLookup.Lookup;
using RingSoft.DbLookup.ModelDefinition;
using RingSoft.DbLookup.ModelDefinition.FieldDefinitions;
using RingSoft.DbLookup.QueryBuilder;

namespace RingSoft.DbLookup
{
    public class CopyProcedureArgs
    {
        public TableDefinitionBase TableDefinitionBeingProcessed { get; internal set; }

        public int TotalTables { get; internal set; }

        public int TableIdBeingProcessed { get; internal set; }

        public int RecordBeingProcessed { get; internal set; }

        public int TotalRecords { get; internal set; }
    }
    public class CanProcessTableArgs
    {
        public TableDefinitionBase TableDefinition { get; private set; }

        public bool AllowAdd { get; set; } = true;

        public bool AllowView { get; set; } = true;

        public bool AllowEdit { get; set; } = true;

        public bool AllowDelete { get; set; } = true;

        public bool DeleteMode { get; set; } = false;

        public LookupDefinitionBase LookupDefinition { get; internal set; }

        public CanProcessTableArgs(TableDefinitionBase tableDefinition)
        {
            TableDefinition = tableDefinition;
        }
    }

    public class UserAutoFill
    {
        public AutoFillSetup AutoFillSetup { get; set; }

        public AutoFillValue AutoFillValue { get; set; }
    }

    public class TableDefinitionValue
    {
        public AutoFillValue ReturnValue { get; set; }

        public TableDefinitionBase TableDefinition { get; set; }

        public string PrimaryKeyString { get; set; }
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
        /// Gets the data processor used to process all queries and SQL statements.
        /// </summary>
        /// <value>
        /// The data processor.
        /// </value>
        public abstract DbDataProcessor DataProcessor { get; }

        /// <summary>
        /// Gets a value indicating whether this class has been initialized by the Entity Framework platform.
        /// </summary>
        /// <value>
        ///   <c>true</c> if initialized; otherwise, <c>false</c>.
        /// </value>
        public bool Initialized { get; private set; }

        /// <summary>
        /// Gets the validate field fail caption.  Override this for localization.
        /// </summary>
        /// <value>
        /// The validate field fail caption.
        /// </value>
        public virtual string ValidateFieldFailCaption => "Validation Failure!";

        public bool CanViewTable(TableDefinitionBase tableDefinition)
        {
            var args = new CanProcessTableArgs(tableDefinition);
            CanProcessTableEvent?.Invoke(this, args);
            return args.AllowView;
        }

        public bool CanEditTable(TableDefinitionBase tableDefinition)
        {
            var args = new CanProcessTableArgs(tableDefinition);
            CanProcessTableEvent?.Invoke(this, args);
            return args.AllowEdit;
        }

        public bool CanAddToTable(TableDefinitionBase tableDefinition)
        {
            var args = new CanProcessTableArgs(tableDefinition);
            CanProcessTableEvent?.Invoke(this, args);
            return args.AllowAdd;
        }


        public bool CanDeleteTable(TableDefinitionBase tableDefinition, LookupDefinitionBase lookupDefinition)
        {
            var args = new CanProcessTableArgs(tableDefinition);
            args.LookupDefinition = lookupDefinition;
            args.DeleteMode = true;
            CanProcessTableEvent?.Invoke(this, args);
            return args.AllowDelete;
        }

        /// <summary>
        /// Occurs when a user wishes to view a selected lookup row.  Used to show the appropriate editor for the selected lookup row.
        /// </summary>
        public event EventHandler<LookupAddViewArgs> LookupAddView;

        public event EventHandler<TableDefinitionValue> GetAutoFillText;

        public event EventHandler<CanProcessTableArgs> CanProcessTableEvent;

        public event EventHandler<CopyProcedureArgs> CopyProcedureEvent;

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
        /// Has the Entity Framework platform set up this object's table and field properties based on DbContext's model setup.
        /// Derived classes constructor must execute this method.
        /// </summary>
        public virtual void Initialize()
        {
            if (!Initialized)
            {
                InitializeAdvFind();

                EfInitializeTableDefinitions();

                EfInitializePrimaryKeys();

                EfInitializeFieldDefinitions();

                Initialized = true;

                InitializeLookupDefinitions();
            }
        }

        protected internal virtual void InitializeAdvFind()
        {

        }

        protected abstract void EfInitializeTableDefinitions();

        protected abstract void EfInitializeFieldDefinitions();

        protected abstract void EfInitializePrimaryKeys();

        /// <summary>
        /// Called by Initialize for inheriting classes to create lookup definitions and attach them to table definitions.
        /// </summary>
        protected abstract void InitializeLookupDefinitions();

        internal void AddTable(TableDefinitionBase table)
        {
            _tables.Add(table);
        }

        /// <summary>
        /// Inheritor classes use this to set table and field definition properties not automatically set up by the Entity Framework platform.
        /// </summary>
        protected abstract void SetupModel();

        /// <summary>
        /// Occurs when a user wishes to view a selected lookup primary key value.  Fires the LookupAddView event.
        /// </summary>
        /// <param name="e">The lookup add view arguments.</param>
        public virtual void OnAddViewLookup(LookupAddViewArgs e)
        {
            if (!e.LookupData.LookupDefinition.TableDefinition.CanViewTable)
            {
                DbDataProcessor.UserInterface.PlaySystemSound(RsMessageBoxIcons.Exclamation);
                return;
            }

            if (e.OwnerWindow == null)
            {
                e.OwnerWindow = DbDataProcessor.UserInterface.GetOwnerWindow();
            }

            DbDataProcessor.UserInterface.ShowAddOnTheFlyWindow(e);
            LookupAddView?.Invoke(this, e);
        }

        /// <summary>
        /// Gets the validates the field fail message.  Override this for localization.
        /// </summary>
        /// <param name="fieldDefinition">The field definition.</param>
        /// <returns></returns>
        public virtual string ValidateFieldFailMessage(FieldDefinition fieldDefinition)
        {
            var message = $"{fieldDefinition} has an invalid value.  Please correct the value.";
            return message;
        }

        public virtual AutoFillValue OnAutoFillTextRequest(TableDefinitionBase tableDefinition, string primaryKeyString)
        {
            if (tableDefinition?.LookupDefinition == null)
            {
                throw new Exception($"There is no lookup defined for table {tableDefinition?.Description}");
            }

            var request = new TableDefinitionValue
            {
                TableDefinition = tableDefinition,
                PrimaryKeyString = primaryKeyString
            };
            GetAutoFillText?.Invoke(this, request);

            if (request.ReturnValue == null)
            {
                if (tableDefinition?.LookupDefinition.InitialSortColumnDefinition is LookupFieldColumnDefinition
                    lookupFieldColumn)
                {
                    var primaryKey = new PrimaryKeyValue(tableDefinition);
                    if (!primaryKeyString.IsNullOrEmpty())
                    {
                        primaryKey.LoadFromPrimaryString(primaryKeyString);
                        var query = new SelectQuery(tableDefinition.TableName);
                        query.AddSelectColumn(lookupFieldColumn.FieldDefinition.FieldName);
                        foreach (var primaryKeyField in tableDefinition.PrimaryKeyFields)
                        {
                            query.AddSelectColumn(primaryKeyField.FieldName);
                        }

                        foreach (var primaryKeyKeyValueField in primaryKey.KeyValueFields)
                        {
                            query.AddWhereItem(primaryKeyKeyValueField.FieldDefinition.FieldName, Conditions.Equals,
                                primaryKeyKeyValueField.Value);
                        }

                        var result = tableDefinition.Context.DataProcessor.GetData(query);
                        if (result.ResultCode == GetDataResultCodes.Success)
                        {
                            var primaryKeyValue = new PrimaryKeyValue(tableDefinition);
                            if (result.DataSet.Tables[0].Rows.Count > 0)
                            {
                                var text = result.DataSet.Tables[0].Rows[0]
                                    .GetRowValue(lookupFieldColumn.FieldDefinition.FieldName);
                                primaryKeyValue.PopulateFromDataRow(result.DataSet.Tables[0].Rows[0]);
                                return new AutoFillValue(primaryKeyValue, text);
                            }
                        }
                    }
                }
            }
            return request.ReturnValue;
        }

        public virtual UserAutoFill GetUserAutoFill(string userName)
        {
            return null;
        }

        public string CopyData(DbDataProcessor destinationProcessor)
        {
            var tableIndex = 0;
            var tablesToProcess = TableDefinitions.OrderBy(p => p.PriorityLevel);
            foreach (var tableDefinition in tablesToProcess)
            {
                tableIndex++;
                var result = tableDefinition.CopyDataTo(destinationProcessor, tableIndex);
                if (!result.IsNullOrEmpty())
                {
                    return result;
                }
            }

            return string.Empty;
        }

        internal void FireCopyProcedureEvent(CopyProcedureArgs args)
        {
            args.TotalTables = TableDefinitions.Count;
            CopyProcedureEvent?.Invoke(this, args);
        }

        public abstract LookupDataMauiBase GetQuery<TEntity>(TableDefinition<TEntity> table
            , LookupDefinitionBase lookupDefinition
            , int pageSize)
            where TEntity : new();
    }
}
