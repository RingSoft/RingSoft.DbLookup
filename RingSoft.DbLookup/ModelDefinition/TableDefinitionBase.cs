// ***********************************************************************
// Assembly         : RingSoft.DbLookup
// Author           : petem
// Created          : 12-19-2022
//
// Last Modified By : petem
// Last Modified On : 11-22-2023
// ***********************************************************************
// <copyright file="TableDefinitionBase.cs" company="Peter Ringering">
//     Copyright (c) 2023. All rights reserved.
// </copyright>
// <summary></summary>
// ***********************************************************************
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using RingSoft.DataEntryControls.Engine;
using RingSoft.DbLookup.AutoFill;
using RingSoft.DbLookup.DataProcessor;
using RingSoft.DbLookup.Lookup;
using RingSoft.DbLookup.ModelDefinition.FieldDefinitions;
using RingSoft.DbLookup.QueryBuilder;

namespace RingSoft.DbLookup.ModelDefinition
{
    /// <summary>
    /// A database table definition.
    /// </summary>
    public abstract class TableDefinitionBase
    {
        /// <summary>
        /// Gets the context.
        /// </summary>
        /// <value>The context.</value>
        public LookupContextBase Context { get; internal set; }

        /// <summary>
        /// Gets the name of the table.
        /// </summary>
        /// <value>The name of the table.</value>
        public string TableName { get; internal set; }

        /// <summary>
        /// Gets the name of the entity.
        /// </summary>
        /// <value>The name of the entity.</value>
        public string EntityName { get; internal set; }

        /// <summary>
        /// Gets the full name of the entity.
        /// </summary>
        /// <value>The full name of the entity.</value>
        public string FullEntityName { get; internal set; }

        /// <summary>
        /// Gets the field definitions.
        /// </summary>
        /// <value>The field definitions.</value>
        public IReadOnlyList<FieldDefinition> FieldDefinitions => _fields;

        /// <summary>
        /// Gets the primary key fields.
        /// </summary>
        /// <value>The primary key fields.</value>
        public IReadOnlyList<FieldDefinition> PrimaryKeyFields => _primaryKeyFields;

        /// <summary>
        /// Gets the lookup definition.  Used in mapping foreign key fields to controls.
        /// </summary>
        /// <value>The lookup definition.</value>
        public LookupDefinitionBase LookupDefinition { get; private set; }

        /// <summary>
        /// Gets the table description that the user will see.
        /// </summary>
        /// <value>
        /// The description.
        /// </value>
        private string _description;
        /// <summary>
        /// Gets the description.
        /// </summary>
        /// <value>The description.</value>
        public string Description
        {
            get
            {
                if (_description.IsNullOrEmpty())
                {
                    var newDescription = TableName.ConvertPropertyNameToDescription();
                    return newDescription;
                }
                return _description;
            }
            internal set
            {
                _description = value;
            }
        }

        /// <summary>
        /// Gets the record description.
        /// </summary>
        /// <value>
        /// The record description.
        /// </value>
        private string _recordDescription;

        /// <summary>
        /// Gets the record description.
        /// </summary>
        /// <value>The record description.</value>
        public string RecordDescription
        {
            get
            {
                if (_recordDescription.IsNullOrEmpty())
                {
                    var newDescription = Description.TrimEnd('s');
                    return newDescription;
                }

                return _recordDescription;
            }
            internal set => _recordDescription = value;
        }

        /// <summary>
        /// Gets or sets the child fields.
        /// </summary>
        /// <value>The child fields.</value>
        public List<FieldDefinition> ChildFields { get; set; } = new List<FieldDefinition>();

        /// <summary>
        /// Gets or sets the child keys.
        /// </summary>
        /// <value>The child keys.</value>
        public List<ForeignKeyDefinition> ChildKeys { get; set; } = new List<ForeignKeyDefinition>();

        /// <summary>
        /// Gets or sets the priority level.
        /// </summary>
        /// <value>The priority level.</value>
        public int PriorityLevel { get; set; } = 1000;

        /// <summary>
        /// Gets a value indicating whether this instance is advanced find.
        /// </summary>
        /// <value><c>true</c> if this instance is advanced find; otherwise, <c>false</c>.</value>
        public bool IsAdvancedFind { get; internal set; } = true;

        /// <summary>
        /// Gets a value indicating whether this instance can view table.
        /// </summary>
        /// <value><c>true</c> if this instance can view table; otherwise, <c>false</c>.</value>
        public bool CanViewTable
        {
            get
            {
                return Context.CanViewTable(this);
            }
        }

        /// <summary>
        /// Gets a value indicating whether this instance can edit tabe.
        /// </summary>
        /// <value><c>true</c> if this instance can edit tabe; otherwise, <c>false</c>.</value>
        public bool CanEditTabe
        {
            get
            {
                return Context.CanEditTable(this);
            }
        }

        /// <summary>
        /// Gets a value indicating whether this instance can add to table.
        /// </summary>
        /// <value><c>true</c> if this instance can add to table; otherwise, <c>false</c>.</value>
        public bool CanAddToTable
        {
            get
            {
                return Context.CanAddToTable(this);
            }
        }


        /// <summary>
        /// Determines whether this instance [can delete table] the specified lookup definition.
        /// </summary>
        /// <param name="lookupDefinition">The lookup definition.</param>
        /// <returns><c>true</c> if this instance [can delete table] the specified lookup definition; otherwise, <c>false</c>.</returns>
        public bool CanDeleteTable(LookupDefinitionBase lookupDefinition)
        {
            return Context.CanDeleteTable(this, lookupDefinition);
        }

        /// <summary>
        /// Gets a value indicating whether [validate delete].
        /// </summary>
        /// <value><c>true</c> if [validate delete]; otherwise, <c>false</c>.</value>
        public bool ValidateDelete { get; private set; } = true;

        /// <summary>
        /// Gets the header table.
        /// </summary>
        /// <value>The header table.</value>
        public TableDefinitionBase HeaderTable { get; private set; }

        /// <summary>
        /// Gets a value indicating whether [temporary table].
        /// </summary>
        /// <value><c>true</c> if [temporary table]; otherwise, <c>false</c>.</value>
        public bool TempTable { get; private set; }

        /// <summary>
        /// The fields
        /// </summary>
        private readonly List<FieldDefinition> _fields = new List<FieldDefinition>();
        /// <summary>
        /// The primary key fields
        /// </summary>
        private readonly List<FieldDefinition> _primaryKeyFields = new List<FieldDefinition>();

        /// <summary>
        /// Initializes a new instance of the <see cref="TableDefinitionBase"/> class.
        /// </summary>
        /// <param name="context">The context.</param>
        /// <param name="tableName">Name of the table.</param>
        public TableDefinitionBase(LookupContextBase context, string tableName)
        {
            Context = context;

            TableName = tableName;
            context.AddTable(this);
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="TableDefinitionBase"/> class.
        /// </summary>
        internal protected TableDefinitionBase()
        {
            
        }

        #region Field Definitions

        /// <summary>
        /// Adds a string field.
        /// </summary>
        /// <param name="fieldName">Name of the field.</param>
        /// <returns>The added string field definition.</returns>
        public StringFieldDefinition AddStringField(string fieldName)
        {
            var field = new StringFieldDefinition();
            AddField(field, fieldName);
            return field;
        }

        /// <summary>
        /// Adds the date field.
        /// </summary>
        /// <param name="fieldName">Name of the field.</param>
        /// <returns>The added dater field definition.</returns>
        public DateFieldDefinition AddDateField(string fieldName)
        {
            var field = new DateFieldDefinition();
            AddField(field, fieldName);
            return field;
        }

        /// <summary>
        /// Adds an integer (whole number) field.
        /// </summary>
        /// <param name="fieldName">Name of the field.</param>
        /// <returns>The added integer field definition.</returns>
        public IntegerFieldDefinition AddIntegerField(string fieldName)
        {
            var field = new IntegerFieldDefinition();
            AddField(field, fieldName);
            return field;
        }

        /// <summary>
        /// Adds a double field.
        /// </summary>
        /// <param name="fieldName">Name of the field.</param>
        /// <returns>The added double field definition.</returns>
        public DecimalFieldDefinition AddDecimalField(string fieldName)
        {
            var field = new DecimalFieldDefinition();
            AddField(field, fieldName);
            return field;
        }

        /// <summary>
        /// Adds a boolean field.
        /// </summary>
        /// <param name="fieldName">Name of the field.</param>
        /// <returns>The added boolean field definition.</returns>
        public BoolFieldDefinition AddBoolField(string fieldName)
        {
            var field = new BoolFieldDefinition();
            AddField(field, fieldName);
            return field;
        }

        /// <summary>
        /// Adds the field.
        /// </summary>
        /// <param name="field">The field.</param>
        /// <param name="fieldName">Name of the field.</param>
        private void AddField(FieldDefinition field, string fieldName)
        {
            field.TableDefinition = this;
            field.FieldName = fieldName;
            _fields.Add(field);
        }

        #endregion

        /// <summary>
        /// Adds the field to primary key.
        /// </summary>
        /// <param name="fieldDefinition">The field definition.</param>
        /// <returns>TableDefinitionBase.</returns>
        public TableDefinitionBase AddFieldToPrimaryKey(FieldDefinition fieldDefinition)
        {
            _primaryKeyFields.Add(fieldDefinition);
            fieldDefinition.AllowNulls = false;
            return this;
        }

        /// <summary>
        /// Returns a <see cref="System.String" /> that represents this instance.
        /// </summary>
        /// <returns>A <see cref="System.String" /> that represents this instance.</returns>
        public override string ToString()
        {
            if (!Description.IsNullOrEmpty())
                return Description;

            if (!EntityName.IsNullOrEmpty())
                return EntityName;

            return TableName;
        }

        /// <summary>
        /// Sets the name of the table.
        /// </summary>
        /// <param name="name">The database table name.</param>
        /// <returns>TableDefinitionBase.</returns>
        public TableDefinitionBase HasTableName(string name)
        {
            TableName = name;
            return this;
        }

        /// <summary>
        /// Sets this table's default lookup definition.
        /// </summary>
        /// <param name="lookupDefinition">The lookup definition.</param>
        /// <returns>TableDefinitionBase.</returns>
        /// <exception cref="System.ArgumentException">Lookup's table definition '{lookupDefinition.TableDefinition}' does not match this table definition. ({this})</exception>
        public TableDefinitionBase HasLookupDefinition(LookupDefinitionBase lookupDefinition)
        {
            if (LookupDefinition != null)
            {
                LookupDefinition.CopyLookupData(lookupDefinition);
                return this;
            }
            if (lookupDefinition.TableDefinition != this)
                throw new ArgumentException(
                    $"Lookup's table definition '{lookupDefinition.TableDefinition}' does not match this table definition. ({this})");

            //var message = $"The first column's field definition table must be of '{Description}'";
            //var caption = "Invalid Lookup Definition";

            //if (lookupDefinition.InitialSortColumnDefinition is LookupFieldColumnDefinition lookupFieldColumn)
            //{
            //    if (lookupFieldColumn.FieldDefinition.TableDefinition != this)
            //    {
            //        ControlsGlobals.UserInterface.ShowMessageBox(message, caption, RsMessageBoxIcons.Exclamation);
            //        return this;
            //    }
            //}

            //else if (lookupDefinition.InitialSortColumnDefinition is LookupFormulaColumnDefinition lookupFormulaColumn)
            //{
            //    if (lookupFormulaColumn.PrimaryField != null && lookupFormulaColumn.PrimaryField.TableDefinition != this)
            //    {
            //        ControlsGlobals.UserInterface.ShowMessageBox(message, caption, RsMessageBoxIcons.Exclamation);
            //        return this;
            //    }
            //}
            LookupDefinition = lookupDefinition;
            return this;
        }

        /// <summary>
        /// Sets the table description.
        /// </summary>
        /// <param name="description">The description.</param>
        /// <returns>This object.</returns>
        public TableDefinitionBase HasDescription(string description)
        {
            Description = description;
            return this;
        }

        /// <summary>
        /// Sets the description to describe an individual record name.
        /// </summary>
        /// <param name="description">The description.</param>
        /// <returns>TableDefinitionBase.</returns>
        public TableDefinitionBase HasRecordDescription(string description)
        {
            RecordDescription = description;
            return this;
        }

        /// <summary>
        /// Gets the chunk.
        /// </summary>
        /// <param name="chunkSize">Size of the chunk.</param>
        /// <param name="primaryKey">The primary key.</param>
        /// <returns>ChunkResult.</returns>
        public ChunkResult GetChunk(int chunkSize, PrimaryKeyValue primaryKey = null)
        {
            var result = new ChunkResult();
            var query = new SelectQuery(TableName).SetMaxRecords(chunkSize);
            foreach (var fieldDefinition in FieldDefinitions)
            {
                query.AddSelectColumn(fieldDefinition.FieldName);
            }

            foreach (var primaryKeyField in PrimaryKeyFields)
            {
                query.AddOrderBySegment(primaryKeyField.FieldName, OrderByTypes.Ascending);
            }

            if (primaryKey != null && primaryKey.IsValid() && primaryKey.TableDefinition == this)
            {
                if (primaryKey.KeyValueFields.Count > 1)
                {
                    var countQuery = new SelectQuery(TableName);
                    foreach (var primaryKeyKeyValueField in primaryKey.KeyValueFields)
                    {
                        //countQuery.AddWhereItem(primaryKeyKeyValueField.FieldDefinition.FieldName, Conditions.Equals,
                        //    primaryKeyKeyValueField.Value);
                        AddWhereChunk(primaryKeyKeyValueField, countQuery, Conditions.Equals);
                        countQuery.AddOrderBySegment(primaryKeyKeyValueField.FieldDefinition.FieldName,
                            OrderByTypes.Ascending);

                        var countResult = Context.DataProcessor.GetData(countQuery);
                        if (countResult.ResultCode == GetDataResultCodes.Success)
                        {
                            if (countResult.DataSet.Tables[0].Rows.Count > 1)
                            {
                                AddWhereChunk(primaryKeyKeyValueField, query, Conditions.Equals);
                            }
                            else
                            {
                                //query.AddWhereItem(primaryKeyKeyValueField.FieldDefinition.FieldName,
                                //    Conditions.GreaterThan,
                                //    primaryKeyKeyValueField.Value);
                                AddWhereChunk(primaryKeyKeyValueField, query, Conditions.GreaterThan);
                            }
                        }
                    }
                }
                else
                {
                    var primaryKeyField = primaryKey.KeyValueFields[0];
                    //query.AddWhereItem(primaryKeyField.FieldDefinition.FieldName, Conditions.GreaterThan,
                    //    primaryKeyField.Value);
                    AddWhereChunk(primaryKeyField, query, Conditions.GreaterThan);
                }
            }

            if (primaryKey == null || !primaryKey.IsValid())
            {
                ProcessChunkResult(query, result);
            }
            else
            {
                while (query.WhereItems.Count > 0)
                {
                    query.WhereItems.LastOrDefault()?.UpdateCondition(Conditions.GreaterThan);
                    ProcessChunkResult(query, result);
                    query.RemoveWhereItem(query.WhereItems.LastOrDefault());
                    if (query.MaxRecords <= 0)
                    {
                        break;
                    }
                }
            }

            return result;
        }

        /// <summary>
        /// Adds the where chunk.
        /// </summary>
        /// <param name="primaryKeyKeyValueField">The primary key key value field.</param>
        /// <param name="query">The query.</param>
        /// <param name="condition">The condition.</param>
        /// <exception cref="System.ArgumentOutOfRangeException"></exception>
        private static void AddWhereChunk(PrimaryKeyValueField primaryKeyKeyValueField, SelectQuery query, Conditions condition)
        {
            switch (primaryKeyKeyValueField.FieldDefinition.FieldDataType)
            {
                case FieldDataTypes.String:
                case FieldDataTypes.Memo:
                    query.AddWhereItem(primaryKeyKeyValueField.FieldDefinition.FieldName, condition,
                        primaryKeyKeyValueField.Value);
                    break;
                case FieldDataTypes.Integer:
                    query.AddWhereItem(primaryKeyKeyValueField.FieldDefinition.FieldName, condition,
                        primaryKeyKeyValueField.Value.ToInt());
                    break;
                case FieldDataTypes.Decimal:
                    query.AddWhereItem(primaryKeyKeyValueField.FieldDefinition.FieldName, condition,
                        primaryKeyKeyValueField.Value.ToDecimal());
                    break;
                case FieldDataTypes.DateTime:
                    DbDateTypes dateType = new DbDateTypes();
                    DateTime dateValue = DateTime.Now;
                    if (DateTime.TryParse(primaryKeyKeyValueField.Value, out dateValue))
                    {
                        var dateField =
                            primaryKeyKeyValueField.FieldDefinition as DateFieldDefinition;
                        if (dateField != null)
                            query.AddWhereItem(primaryKeyKeyValueField.FieldDefinition.FieldName,
                                condition,
                                dateValue, dateField.DateType);
                    }

                    break;
                case FieldDataTypes.Bool:
                    query.AddWhereItem(primaryKeyKeyValueField.FieldDefinition.FieldName, condition,
                        primaryKeyKeyValueField.Value.ToBool());
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }

        /// <summary>
        /// Processes the chunk result.
        /// </summary>
        /// <param name="query">The query.</param>
        /// <param name="result">The result.</param>
        private void ProcessChunkResult(SelectQuery query, ChunkResult result)
        {
            var getDataResult = Context.DataProcessor.GetData(query);
            if (getDataResult.ResultCode == GetDataResultCodes.Success)
            {
                if (result.Chunk == null)
                {
                    result.Chunk = getDataResult.DataSet.Tables[0].Copy();
                }
                else
                {
                    foreach (DataRow dataRow in getDataResult.DataSet.Tables[0].Rows)
                    {
                        result.Chunk.ImportRow(dataRow);
                    }
                }
                query.MaxRecords -= getDataResult.DataSet.Tables[0].Rows.Count;

                if (result.Chunk.Rows.Count > 0)
                {
                    var lastRecord = result.Chunk.Rows[result.Chunk.Rows.Count - 1];
                    var primaryKeyValue = new PrimaryKeyValue(this);
                    primaryKeyValue.PopulateFromDataRow(lastRecord);
                    result.BottomPrimaryKey = primaryKeyValue;
                }
            }
        }

        /// <summary>
        /// Copies the data to.
        /// </summary>
        /// <param name="destinationProcessor">The destination processor.</param>
        /// <param name="tableIndex">Index of the table.</param>
        /// <returns>System.String.</returns>
        /// <exception cref="System.ArgumentNullException"></exception>
        public string CopyDataTo(DbDataProcessor destinationProcessor, int tableIndex)
        {
            if (LookupDefinition == null)
            {
                throw new ArgumentNullException($"{EntityName} has no lookup definition");
            }
            return LookupDefinition.CopyDataTo(destinationProcessor, tableIndex);
            //var result = string.Empty;
            //bool identity = !(PrimaryKeyFields.Count > 1 || PrimaryKeyFields[0].FieldDataType != FieldDataTypes.Integer);

            //var lookupDefinition = new LookupDefinitionBase(this);
            //lookupDefinition.AddAllFieldsAsHiddenColumns(true);
            //var fieldColumns = lookupDefinition.HiddenColumns.OfType<LookupFieldColumnDefinition>();

            //var lookupUi = new LookupUserInterface()
            //{
            //    PageSize = 100,
            //};
            //var lookupData = new LookupDataBase(lookupDefinition, lookupUi);
            //var totalRecords = lookupData.GetRecordCountWait();
            //var recordIndex = 0;

            //if (totalRecords > 0)
            //{
            //    var updateSqls = new List<string>();
            //    lookupData.PrintDataChanged += (sender, changedArgs) =>
            //    {
            //        if (!result.IsNullOrEmpty())
            //        {
            //            changedArgs.Abort = true;
            //            return;
            //        }

            //        var sqlList = new List<string>();
            //        if (identity)
            //        {
            //            sqlList.Add(destinationProcessor.GetIdentityInsertSql(TableName));
            //        }

            //        recordIndex += changedArgs.OutputTable.Rows.Count;

            //        foreach (DataRow outputTableRow in changedArgs.OutputTable.Rows)
            //        {
            //            var primaryKey = new PrimaryKeyValue(this);
            //            primaryKey.PopulateFromDataRow(outputTableRow);

            //            var insertStatement = new InsertDataStatement(this);
            //            var updateStatement = new UpdateDataStatement(primaryKey);

            //            foreach (var column in fieldColumns)
            //            {
            //                var sqlValue = outputTableRow.GetRowValue(column.SelectSqlAlias);
            //                var addField = true;
            //                if (column.FieldDefinition.ParentJoinForeignKeyDefinition != null)
            //                {
            //                    if (column.FieldDefinition.ParentJoinForeignKeyDefinition.PrimaryTable == this)
            //                    {
            //                        var updateSqlData = new SqlData(column.FieldDefinition.FieldName, sqlValue,
            //                            column.FieldDefinition.ValueType);
            //                        updateStatement.AddSqlData(updateSqlData);
            //                        addField = false;
            //                    }
            //                }

            //                if (addField && !sqlValue.IsNullOrEmpty())
            //                {
            //                    var fieldName = column.FieldDefinition.FieldName;
            //                    var valueType = column.FieldDefinition.ValueType;
            //                    var dateType = DbDateTypes.DateOnly;

            //                    if (column.FieldDefinition is DateFieldDefinition dateField)
            //                    {
            //                        dateType = dateField.DateType;
            //                    }

            //                    var sqlData = new SqlData(fieldName, sqlValue, valueType, dateType);
            //                    insertStatement.AddSqlData(sqlData);
            //                }
            //            }

            //            sqlList.Add(destinationProcessor.SqlGenerator.GenerateInsertSqlStatement(insertStatement));
            //            if (updateStatement.SqlDatas.Any())
            //            {
            //                updateSqls.Add(destinationProcessor.SqlGenerator.GenerateUpdateSql(updateStatement));
            //            }
            //        }

            //        var getDataResult = destinationProcessor.ExecuteSqls(sqlList, true, true, false);
            //        if (getDataResult.ResultCode != GetDataResultCodes.Success)
            //        {
            //            result = getDataResult.Message;
            //        }

            //        var args = new CopyProcedureArgs
            //        {
            //            TableDefinitionBeingProcessed = this,
            //            TableIdBeingProcessed = tableIndex,
            //            TotalRecords = totalRecords,
            //            RecordBeingProcessed = recordIndex,
            //        };
            //        Context.FireCopyProcedureEvent(args);
            //    };

            //    lookupData.GetPrintData();

            //    if (updateSqls.Any())
            //    {
            //        var updateResult = destinationProcessor.ExecuteSqls(updateSqls, true
            //            , true, false);

            //        if (updateResult.ResultCode != GetDataResultCodes.Success)
            //        {
            //            return updateResult.Message;
            //        }
            //    }
            //}

            //return result;
        }

        /// <summary>
        /// Gets the entity.
        /// </summary>
        /// <returns>System.Object.</returns>
        public abstract object GetEntity();

        /// <summary>
        /// Determines whether [is temporary table] [the specified value].
        /// </summary>
        /// <param name="value">if set to <c>true</c> [value].</param>
        /// <returns>TableDefinitionBase.</returns>
        internal TableDefinitionBase IsTempTable(bool value = true)
        {
            TempTable = value;
            return this;
        }

        /// <summary>
        /// Fills the out object.
        /// </summary>
        /// <param name="obj">The object.</param>
        public abstract void FillOutObject(object obj);

        /// <summary>
        /// Gets the join parent object.
        /// </summary>
        /// <typeparam name="TChildEntity">The type of the t child entity.</typeparam>
        /// <param name="childEntity">The child entity.</param>
        /// <param name="foreignKey">The foreign key.</param>
        /// <returns>System.Object.</returns>
        public abstract object GetJoinParentObject<TChildEntity>(TChildEntity childEntity, ForeignKeyDefinition foreignKey) where TChildEntity : class, new();

        /// <summary>
        /// Gets the join collection.
        /// </summary>
        /// <typeparam name="TChildEntity">The type of the t child entity.</typeparam>
        /// <param name="childEntity">The child entity.</param>
        /// <param name="foreignKey">The foreign key.</param>
        /// <returns>System.Object.</returns>
        public abstract object GetJoinCollection<TChildEntity>(TChildEntity childEntity, ForeignKeyDefinition foreignKey) where TChildEntity : class, new();

        /// <summary>
        /// Sets the header entity.
        /// </summary>
        /// <typeparam name="THeaderEntity">The type of the t header entity.</typeparam>
        /// <returns>TableDefinitionBase.</returns>
        public TableDefinitionBase SetHeaderEntity<THeaderEntity>() where THeaderEntity : class, new()
        {
            HeaderTable = GblMethods.GetTableDefinition<THeaderEntity>(); 
            return this;
        }

        /// <summary>
        /// Validates the automatic fill value.
        /// </summary>
        /// <param name="autoFillValue">The automatic fill value.</param>
        /// <returns><c>true</c> if XXXX, <c>false</c> otherwise.</returns>
        public abstract bool ValidateAutoFillValue(AutoFillValue autoFillValue);

        /// <summary>
        /// Determines whether this instance is identity.
        /// </summary>
        /// <returns><c>true</c> if this instance is identity; otherwise, <c>false</c>.</returns>
        public bool IsIdentity()
        {
            var identity = PrimaryKeyFields.Count == 1 && PrimaryKeyFields[0].FieldDataType == FieldDataTypes.Integer;
            return identity;
        }

        /// <summary>
        /// Gets the identity field.
        /// </summary>
        /// <returns>IntegerFieldDefinition.</returns>
        public IntegerFieldDefinition GetIdentityField()
        {
            IntegerFieldDefinition result = null;
            if (IsIdentity())
            {
                return PrimaryKeyFields[0] as IntegerFieldDefinition;
            }
            return result;
        }
    }
}
