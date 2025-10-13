// ***********************************************************************
// Assembly         : RingSoft.DbLookup
// Author           : petem
// Created          : 12-19-2022
//
// Last Modified By : petem
// Last Modified On : 08-30-2024
// ***********************************************************************
// <copyright file="LookupContextBase.cs" company="Peter Ringering">
//     Copyright (c) 2023. All rights reserved.
// </copyright>
// <summary></summary>
// ***********************************************************************
using System;
using System.Collections.Generic;
using System.Linq;
using RingSoft.DataEntryControls.Engine;
using RingSoft.DbLookup.AdvancedFind;
using RingSoft.DbLookup.AutoFill;
using RingSoft.DbLookup.DataProcessor;
using RingSoft.DbLookup.Lookup;
using RingSoft.DbLookup.ModelDefinition;
using RingSoft.DbLookup.ModelDefinition.FieldDefinitions;
using RingSoft.DbLookup.QueryBuilder;
using RingSoft.DbLookup.RecordLocking;

namespace RingSoft.DbLookup
{
    /// <summary>
    /// Class CopyProcedureArgs.
    /// </summary>
    public class CopyProcedureArgs
    {
        /// <summary>
        /// Gets the table definition being processed.
        /// </summary>
        /// <value>The table definition being processed.</value>
        public TableDefinitionBase TableDefinitionBeingProcessed { get; internal set; }

        /// <summary>
        /// Gets the total tables.
        /// </summary>
        /// <value>The total tables.</value>
        public int TotalTables { get; internal set; }

        /// <summary>
        /// Gets the table identifier being processed.
        /// </summary>
        /// <value>The table identifier being processed.</value>
        public int TableIdBeingProcessed { get; internal set; }

        /// <summary>
        /// Gets the record being processed.
        /// </summary>
        /// <value>The record being processed.</value>
        public int RecordBeingProcessed { get; internal set; }

        /// <summary>
        /// Gets the total records.
        /// </summary>
        /// <value>The total records.</value>
        public int TotalRecords { get; internal set; }
    }
    /// <summary>
    /// Class CanProcessTableArgs.
    /// </summary>
    public class CanProcessTableArgs
    {
        /// <summary>
        /// Gets the table definition.
        /// </summary>
        /// <value>The table definition.</value>
        public TableDefinitionBase TableDefinition { get; private set; }

        /// <summary>
        /// Gets or sets a value indicating whether [allow add].
        /// </summary>
        /// <value><c>true</c> if [allow add]; otherwise, <c>false</c>.</value>
        public bool AllowAdd { get; set; } = true;

        /// <summary>
        /// Gets or sets a value indicating whether [allow view].
        /// </summary>
        /// <value><c>true</c> if [allow view]; otherwise, <c>false</c>.</value>
        public bool AllowView { get; set; } = true;

        /// <summary>
        /// Gets or sets a value indicating whether [allow edit].
        /// </summary>
        /// <value><c>true</c> if [allow edit]; otherwise, <c>false</c>.</value>
        public bool AllowEdit { get; set; } = true;

        /// <summary>
        /// Gets or sets a value indicating whether [allow delete].
        /// </summary>
        /// <value><c>true</c> if [allow delete]; otherwise, <c>false</c>.</value>
        public bool AllowDelete { get; set; } = true;

        /// <summary>
        /// Gets or sets a value indicating whether [delete mode].
        /// </summary>
        /// <value><c>true</c> if [delete mode]; otherwise, <c>false</c>.</value>
        public bool DeleteMode { get; set; } = false;

        /// <summary>
        /// Gets the lookup definition.
        /// </summary>
        /// <value>The lookup definition.</value>
        public LookupDefinitionBase LookupDefinition { get; internal set; }

        /// <summary>
        /// Initializes a new instance of the <see cref="CanProcessTableArgs" /> class.
        /// </summary>
        /// <param name="tableDefinition">The table definition.</param>
        public CanProcessTableArgs(TableDefinitionBase tableDefinition)
        {
            TableDefinition = tableDefinition;
        }
    }

    /// <summary>
    /// Class SearchHostFormatArgs.
    /// </summary>
    public class SearchHostFormatArgs
    {
        /// <summary>
        /// Gets or sets the value.
        /// </summary>
        /// <value>The value.</value>
        public string Value { get; set; }

        /// <summary>
        /// Gets or sets the raw value.
        /// </summary>
        /// <value>The raw value.</value>
        public string RawValue { get; set; }

        /// <summary>
        /// Gets or sets the search for host identifier.
        /// </summary>
        /// <value>The search for host identifier.</value>
        public int SearchForHostId { get; set; }

        /// <summary>
        /// Gets the field definition.
        /// </summary>
        /// <value>The field definition.</value>
        public FieldDefinition FieldDefinition { get; internal set; }
    }

    /// <summary>
    /// Class UserAutoFill.
    /// </summary>
    public class UserAutoFill
    {
        /// <summary>
        /// Gets or sets the automatic fill setup.
        /// </summary>
        /// <value>The automatic fill setup.</value>
        public AutoFillSetup AutoFillSetup { get; set; }

        /// <summary>
        /// Gets or sets the automatic fill value.
        /// </summary>
        /// <value>The automatic fill value.</value>
        public AutoFillValue AutoFillValue { get; set; }
    }

    /// <summary>
    /// Class TableDefinitionValue.
    /// </summary>
    public class TableDefinitionValue
    {
        /// <summary>
        /// Gets or sets the return value.
        /// </summary>
        /// <value>The return value.</value>
        public AutoFillValue ReturnValue { get; set; }

        /// <summary>
        /// Gets or sets the table definition.
        /// </summary>
        /// <value>The table definition.</value>
        public TableDefinitionBase TableDefinition { get; set; }

        /// <summary>
        /// Gets or sets the primary key string.
        /// </summary>
        /// <value>The primary key string.</value>
        public string PrimaryKeyString { get; set; }

        /// <summary>
        /// Gets or sets the lookup definition.
        /// </summary>
        /// <value>The lookup definition.</value>
        public LookupDefinitionBase LookupDefinition { get; set; }
    }

    // ReSharper disable once InconsistentNaming
    /// <summary>
    /// Contains all the table definitions used in this context.
    /// </summary>
    public abstract class LookupContextBase : IAdvancedFindLookupContext
    {
        /// <summary>
        /// Gets or sets the advanced finds.
        /// </summary>
        /// <value>The advanced finds.</value>
        public TableDefinition<AdvancedFind.AdvancedFind> AdvancedFinds { get; set; }
        /// <summary>
        /// Gets or sets the advanced find columns.
        /// </summary>
        /// <value>The advanced find columns.</value>
        public TableDefinition<AdvancedFindColumn> AdvancedFindColumns { get; set; }
        /// <summary>
        /// Gets or sets the advanced find filters.
        /// </summary>
        /// <value>The advanced find filters.</value>
        public TableDefinition<AdvancedFindFilter> AdvancedFindFilters { get; set; }
        /// <summary>
        /// Gets the context.
        /// </summary>
        /// <value>The context.</value>
        public LookupContextBase Context => this;
        /// <summary>
        /// Gets or sets the record locks.
        /// </summary>
        /// <value>The record locks.</value>
        public TableDefinition<RecordLock> RecordLocks { get; set; }

        /// <summary>
        /// Gets or sets the advanced find lookup.
        /// </summary>
        /// <value>The advanced find lookup.</value>
        public LookupDefinition<AdvancedFindLookup, AdvancedFind.AdvancedFind> AdvancedFindLookup { get; set; }
        /// <summary>
        /// Gets or sets the advanced find column lookup.
        /// </summary>
        /// <value>The advanced find column lookup.</value>
        public LookupDefinition<AdvancedFindLookup, AdvancedFindColumn> AdvancedFindColumnLookup { get; set; }
        /// <summary>
        /// Gets or sets the advanced find filter lookup.
        /// </summary>
        /// <value>The advanced find filter lookup.</value>
        public LookupDefinition<AdvFindFilterLookup, AdvancedFindFilter> AdvancedFindFilterLookup { get; set; }
        /// <summary>
        /// Gets or sets the record locking lookup.
        /// </summary>
        /// <value>The record locking lookup.</value>
        public LookupDefinition<RecordLockingLookup, RecordLock> RecordLockingLookup { get; set; }

        /// <summary>
        /// Gets the table definitions.
        /// </summary>
        /// <value>The table definitions.</value>
        public IReadOnlyList<TableDefinitionBase> TableDefinitions => _tables;

        /// <summary>
        /// Gets the data processor used to process all queries and SQL statements.
        /// </summary>
        /// <value>The data processor.</value>
        public abstract DbDataProcessor DataProcessor { get; }

        /// <summary>
        /// Gets a value indicating whether this class has been initialized by the Entity Framework platform.
        /// </summary>
        /// <value><c>true</c> if initialized; otherwise, <c>false</c>.</value>
        public bool Initialized { get; private set; }

        /// <summary>
        /// Gets the validate field fail caption.  Override this for localization.
        /// </summary>
        /// <value>The validate field fail caption.</value>
        public virtual string ValidateFieldFailCaption => "Validation Failure!";

        /// <summary>
        /// Determines whether this instance [can view table] the specified table definition.
        /// </summary>
        /// <param name="tableDefinition">The table definition.</param>
        /// <returns><c>true</c> if this instance [can view table] the specified table definition; otherwise, <c>false</c>.</returns>
        public bool CanViewTable(TableDefinitionBase tableDefinition)
        {
            var args = new CanProcessTableArgs(tableDefinition);
            CanProcessTableEvent?.Invoke(this, args);
            return args.AllowView;
        }

        /// <summary>
        /// Determines whether this instance [can edit table] the specified table definition.
        /// </summary>
        /// <param name="tableDefinition">The table definition.</param>
        /// <returns><c>true</c> if this instance [can edit table] the specified table definition; otherwise, <c>false</c>.</returns>
        public bool CanEditTable(TableDefinitionBase tableDefinition)
        {
            var args = new CanProcessTableArgs(tableDefinition);
            CanProcessTableEvent?.Invoke(this, args);
            return args.AllowEdit;
        }

        /// <summary>
        /// Determines whether this instance [can add to table] the specified table definition.
        /// </summary>
        /// <param name="tableDefinition">The table definition.</param>
        /// <returns><c>true</c> if this instance [can add to table] the specified table definition; otherwise, <c>false</c>.</returns>
        public bool CanAddToTable(TableDefinitionBase tableDefinition)
        {
            var args = new CanProcessTableArgs(tableDefinition);
            CanProcessTableEvent?.Invoke(this, args);
            return args.AllowAdd;
        }


        /// <summary>
        /// Determines whether this instance [can delete table] the specified table definition.
        /// </summary>
        /// <param name="tableDefinition">The table definition.</param>
        /// <param name="lookupDefinition">The lookup definition.</param>
        /// <returns><c>true</c> if this instance [can delete table] the specified table definition; otherwise, <c>false</c>.</returns>
        public bool CanDeleteTable(TableDefinitionBase tableDefinition, LookupDefinitionBase lookupDefinition)
        {
            var args = new CanProcessTableArgs(tableDefinition);
            args.LookupDefinition = lookupDefinition;
            args.DeleteMode = true;
            CanProcessTableEvent?.Invoke(this, args);
            return args.AllowDelete;
        }

        /// <summary>
        /// Gets the formula registry.
        /// </summary>
        /// <value>The formula registry.</value>
        public IReadOnlyList<ILookupFormula> FormulaRegistry => _formulas.AsReadOnly();

        /// <summary>
        /// Registers the lookup formula.
        /// </summary>
        /// <param name="lookupFormula">The lookup formula.</param>
        internal void RegisterLookupFormula(ILookupFormula lookupFormula)
        {
            if (lookupFormula != null)
            {
                var existingFormula = _formulas.FirstOrDefault(p => p.Id == lookupFormula.Id);
                if (existingFormula == null)
                {
                    _formulas.Add(lookupFormula);
                }
            }
        }

        /// <summary>
        /// Occurs when a user wishes to view a selected lookup row.  Used to show the appropriate editor for the selected lookup row.
        /// </summary>
        public event EventHandler<LookupAddViewArgs> LookupAddView;

        /// <summary>
        /// Occurs when [get automatic fill text].
        /// </summary>
        public event EventHandler<TableDefinitionValue> GetAutoFillText;

        /// <summary>
        /// Occurs when [can process table event].
        /// </summary>
        public event EventHandler<CanProcessTableArgs> CanProcessTableEvent;

        /// <summary>
        /// Occurs when [copy procedure event].
        /// </summary>
        public event EventHandler<CopyProcedureArgs> CopyProcedureEvent;

        /// <summary>
        /// Occurs when [format search for event].
        /// </summary>
        public event EventHandler<SearchHostFormatArgs> FormatSearchForEvent;

        /// <summary>
        /// The tables
        /// </summary>
        private readonly List<TableDefinitionBase> _tables = new List<TableDefinitionBase>();
        /// <summary>
        /// The formulas
        /// </summary>
        public readonly List<ILookupFormula> _formulas = new List<ILookupFormula>();

        /// <summary>
        /// Initializes a new instance of the <see cref="LookupContextBase" /> class.
        /// </summary>
        public LookupContextBase()
        {
            SystemGlobals.LookupContext = this;
            SystemGlobals.AdvancedFindLookupContext = this;

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
            SystemGlobals.LookupContext = this;
            SystemGlobals.AdvancedFindLookupContext = this;
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

        /// <summary>
        /// Initializes the adv find.
        /// </summary>
        protected internal virtual void InitializeAdvFind()
        {

        }

        /// <summary>
        /// Efs the initialize table definitions.
        /// </summary>
        protected abstract void EfInitializeTableDefinitions();

        /// <summary>
        /// Efs the initialize field definitions.
        /// </summary>
        protected abstract void EfInitializeFieldDefinitions();

        /// <summary>
        /// Efs the initialize primary keys.
        /// </summary>
        protected abstract void EfInitializePrimaryKeys();

        /// <summary>
        /// Called by Initialize for inheriting classes to create lookup definitions and attach them to table definitions.
        /// </summary>
        protected abstract void InitializeLookupDefinitions();

        /// <summary>
        /// Adds the table.
        /// </summary>
        /// <param name="table">The table.</param>
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

            if (e.LookupData.LookupDefinition.Destination != null)
            {
                //Peter Ringering - 12/13/2024 01:19:55 PM - E-68
                if (SystemGlobals.TableRegistry.IsControlRegistered(e.LookupData.LookupDefinition.TableDefinition))
                {
                    e.LookupData.LookupDefinition.Destination.ShowAddView(e, e.InputParameter);
                    return;
                }
            }
            if (e.OwnerWindow == null)
            {
                e.OwnerWindow = DbDataProcessor.UserInterface.GetOwnerWindow();
            }

            var isTableRegistered = SystemGlobals
                .TableRegistry
                .IsTableRegistered(e.LookupData.LookupDefinition.TableDefinition);

            if (isTableRegistered)
            {
                SystemGlobals.TableRegistry.ShowAddOntheFlyWindow(
                    e.LookupData.LookupDefinition.TableDefinition
                    , e, e.InputParameter);
                if (e.CallBackToken != null)
                {
                    if (e.FromLookupControl)
                    {
                        e.CallBackToken.OnRefreshData();
                    }
                }
            }
            else
            {
                DbDataProcessor.UserInterface.ShowAddOnTheFlyWindow(e);
                LookupAddView?.Invoke(this, e);
            }
        }

        /// <summary>
        /// Gets the validates the field fail message.  Override this for localization.
        /// </summary>
        /// <param name="fieldDefinition">The field definition.</param>
        /// <returns>System.String.</returns>
        public virtual string ValidateFieldFailMessage(FieldDefinition fieldDefinition)
        {
            var message = $"{fieldDefinition} has an invalid value.  Please correct the value.";
            return message;
        }

        /// <summary>
        /// Called when [automatic fill text request].
        /// </summary>
        /// <param name="tableDefinition">The table definition.</param>
        /// <param name="primaryKeyString">The primary key string.</param>
        /// <param name="lookupDefinition">The lookup definition.</param>
        /// <returns>AutoFillValue.</returns>
        /// <exception cref="System.Exception">There is no lookup defined for table {tableDefinition?.Description}</exception>
        public virtual AutoFillValue OnAutoFillTextRequest(
            TableDefinitionBase tableDefinition
            , string primaryKeyString
            , LookupDefinitionBase lookupDefinition = null)
        {
            if (tableDefinition?.LookupDefinition == null)
            {
                throw new Exception($"There is no lookup defined for table {tableDefinition?.Description}");
            }

            var request = new TableDefinitionValue
            {
                TableDefinition = tableDefinition,
                PrimaryKeyString = primaryKeyString,
                LookupDefinition = lookupDefinition,
            };
            GetAutoFillText?.Invoke(this, request);

            var go = request.ReturnValue == null;
            if (go)
            {
                if (tableDefinition.PrimaryKeyFields.Count > 1)
                {
                    go = false;
                }
            }
            if (go)
            {
                if (tableDefinition?.LookupDefinition.InitialSortColumnDefinition is LookupFieldColumnDefinition
                    lookupFieldColumn)
                {
                    var primaryKey = new PrimaryKeyValue(tableDefinition);
                    if (!primaryKeyString.IsNullOrEmpty())
                    {
                        primaryKey.LoadFromPrimaryString(primaryKeyString);
                        return primaryKey.TableDefinition.LookupDefinition.GetAutoFillValue(primaryKey);
                        //var query = new SelectQuery(tableDefinition.TableName);
                        //query.AddSelectColumn(lookupFieldColumn.FieldDefinition.FieldName);
                        //foreach (var primaryKeyField in tableDefinition.PrimaryKeyFields)
                        //{
                        //    query.AddSelectColumn(primaryKeyField.FieldName);
                        //}

                        //foreach (var primaryKeyKeyValueField in primaryKey.KeyValueFields)
                        //{
                        //    query.AddWhereItem(primaryKeyKeyValueField.FieldDefinition.FieldName, Conditions.Equals,
                        //        primaryKeyKeyValueField.Value);
                        //}

                        //var result = tableDefinition.Context.DataProcessor.GetData(query);
                        //if (result.ResultCode == GetDataResultCodes.Success)
                        //{
                        //    var primaryKeyValue = new PrimaryKeyValue(tableDefinition);
                        //    if (result.DataSet.Tables[0].Rows.Count > 0)
                        //    {
                        //        var text = result.DataSet.Tables[0].Rows[0]
                        //            .GetRowValue(lookupFieldColumn.FieldDefinition.FieldName);
                        //        primaryKeyValue.PopulateFromDataRow(result.DataSet.Tables[0].Rows[0]);
                        //        return new AutoFillValue(primaryKeyValue, text);
                        //    }
                        //}
                    }
                }
            }
            return request.ReturnValue;
        }

        /// <summary>
        /// Gets the user automatic fill.
        /// </summary>
        /// <param name="userName">Name of the user.</param>
        /// <returns>UserAutoFill.</returns>
        public virtual UserAutoFill GetUserAutoFill(string userName)
        {
            return null;
        }

        /// <summary>
        /// Copies the data.
        /// </summary>
        /// <param name="destinationProcessor">The destination processor.</param>
        /// <returns>System.String.</returns>
        public string CopyData(DbDataProcessor destinationProcessor)
        {
            var tableIndex = 0;
            var tablesToProcess
                = TableDefinitions.OrderBy(p => p.PriorityLevel)
                    .ToList();
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

        /// <summary>
        /// Fires the copy procedure event.
        /// </summary>
        /// <param name="args">The arguments.</param>
        internal void FireCopyProcedureEvent(CopyProcedureArgs args)
        {
            args.TotalTables = TableDefinitions.Count;
            CopyProcedureEvent?.Invoke(this, args);
        }

        /// <summary>
        /// Gets the lookup data maui.
        /// </summary>
        /// <typeparam name="TEntity">The type of the t entity.</typeparam>
        /// <param name="lookupDefinition">The lookup definition.</param>
        /// <returns>LookupDataMauiBase.</returns>
        public abstract LookupDataMauiBase GetLookupDataMaui<TEntity>(LookupDefinitionBase lookupDefinition)
            where TEntity : class, new();

        /// <summary>
        /// Gets the queryable.
        /// </summary>
        /// <typeparam name="TEntity">The type of the t entity.</typeparam>
        /// <param name="lookupDefinition">The lookup definition.</param>
        /// <param name="context">The context.</param>
        /// <returns>IQueryable&lt;TEntity&gt;.</returns>
        public abstract IQueryable<TEntity> GetQueryable<TEntity>(LookupDefinitionBase lookupDefinition
        , IDbContext context = null)
            where TEntity : class, new();

        /// <summary>
        /// Gets the queryable table.
        /// </summary>
        /// <typeparam name="TEntity">The type of the t entity.</typeparam>
        /// <param name="tableDefinition">The table definition.</param>
        /// <param name="getRelatedEntities">if set to <c>true</c> [get related entities].</param>
        /// <param name="context">The context.</param>
        /// <returns>IQueryable&lt;TEntity&gt;.</returns>
        public abstract IQueryable<TEntity> GetQueryableTable<TEntity>(TableDefinition<TEntity> tableDefinition
            , bool getRelatedEntities, IDbContext context = null) where TEntity : class, new();

        /// <summary>
        /// Gets the queryable for table grid.
        /// </summary>
        /// <typeparam name="TEntity">The type of the t entity.</typeparam>
        /// <param name="tableDefinition">The table definition.</param>
        /// <param name="gridTables">The grid tables.</param>
        /// <param name="context">The context.</param>
        /// <returns>IQueryable&lt;TEntity&gt;.</returns>
        public abstract IQueryable<TEntity> GetQueryableForTableGrid<TEntity>(TableDefinition<TEntity> tableDefinition
            , List<TableDefinitionBase> gridTables, IDbContext context = null) where TEntity : class, new();

        /// <summary>
        /// Gets the automatic fill data maui.
        /// </summary>
        /// <typeparam name="TEntity">The type of the t entity.</typeparam>
        /// <param name="setup">The setup.</param>
        /// <param name="control">The control.</param>
        /// <returns>AutoFillDataMauiBase.</returns>
        public abstract AutoFillDataMauiBase GetAutoFillDataMaui<TEntity>(AutoFillSetup setup, IAutoFillControl control)
            where TEntity : class, new();

        /// <summary>
        /// Formats the value for search host.
        /// </summary>
        /// <param name="searchForHostId">The search for host identifier.</param>
        /// <param name="value">The value.</param>
        /// <param name="fieldDefinition">The field definition.</param>
        /// <returns>System.String.</returns>
        public virtual string FormatValueForSearchHost(int searchForHostId, string value, FieldDefinition fieldDefinition)
        {
            if (searchForHostId == GblMethods.SearchForEnumHostId 
                && fieldDefinition is IntegerFieldDefinition integerField
                && integerField.EnumTranslation != null)
            {
                var numValue = value.ToInt();
                var translation = integerField
                    .EnumTranslation
                    .TypeTranslations.FirstOrDefault(p => p.NumericValue == numValue);
                if (translation != null)
                {
                    return translation.TextValue;
                }
            }
            var args = new SearchHostFormatArgs()
            {
                SearchForHostId = searchForHostId,
                RawValue = value,
                FieldDefinition = fieldDefinition,
            };

            FormatSearchForEvent?.Invoke(this, args);
            return args.Value;
        }
    }
}
