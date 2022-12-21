using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using RingSoft.DataEntryControls.Engine;
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
        /// <value>
        /// The context.
        /// </value>
        public LookupContextBase Context { get; internal set; }

        /// <summary>
        /// Gets the name of the table.
        /// </summary>
        /// <value>
        /// The name of the table.
        /// </value>
        public string TableName { get; internal set; }

        /// <summary>
        /// Gets the name of the entity.
        /// </summary>
        /// <value>
        /// The name of the entity.
        /// </value>
        public string EntityName { get; internal set; }

        /// <summary>
        /// Gets the full name of the entity.
        /// </summary>
        /// <value>
        /// The full name of the entity.
        /// </value>
        public string FullEntityName { get; internal set; }

        /// <summary>
        /// Gets the field definitions.
        /// </summary>
        /// <value>
        /// The field definitions.
        /// </value>
        public IReadOnlyList<FieldDefinition> FieldDefinitions => _fields;

        /// <summary>
        /// Gets the primary key fields.
        /// </summary>
        /// <value>
        /// The primary key fields.
        /// </value>
        public IReadOnlyList<FieldDefinition> PrimaryKeyFields => _primaryKeyFields;

        /// <summary>
        /// Gets the lookup definition.  Used in mapping foreign key fields to controls.
        /// </summary>
        /// <value>
        /// The lookup definition.
        /// </value>
        public LookupDefinitionBase LookupDefinition { get; private set; }

        /// <summary>
        /// Gets the table description that the user will see.
        /// </summary>
        /// <value>
        /// The description.
        /// </value>
        private string _description;
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
        public string RecordDescription { get; internal set; }

        public List<FieldDefinition> ChildFields { get; set; } = new List<FieldDefinition>();

        public int PriorityLevel { get; set; } = 1000;

        public bool IsAdvancedFind { get; internal set; }

        public bool CanViewTable
        {
            get
            {
                return Context.CanViewTable(this);
            }
        }

        public bool CanEditTabe
        {
            get
            {
                return Context.CanEditTable(this);
            }
        }

        public bool CanDeleteTable
        {
            get
            {
                return Context.CanDeleteTable(this);
            }
        }


        private readonly List<FieldDefinition> _fields = new List<FieldDefinition>();
        private readonly List<FieldDefinition> _primaryKeyFields = new List<FieldDefinition>();

        public TableDefinitionBase(LookupContextBase context, string tableName)
        {
            Context = context;

            TableName = tableName;
            context.AddTable(this);
        }

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
        /// Adds a decimal field.
        /// </summary>
        /// <param name="fieldName">Name of the field.</param>
        /// <returns>The added decimal field definition.</returns>
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
        public TableDefinitionBase AddFieldToPrimaryKey(FieldDefinition fieldDefinition)
        {
            _primaryKeyFields.Add(fieldDefinition);
            fieldDefinition.AllowNulls = false;
            return this;
        }

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
        /// <returns></returns>
        public TableDefinitionBase HasTableName(string name)
        {
            TableName = name;
            return this;
        }

        /// <summary>
        /// Sets this table's default lookup definition.
        /// </summary>
        /// <param name="lookupDefinition">The lookup definition.</param>
        /// <returns></returns>
        public TableDefinitionBase HasLookupDefinition(LookupDefinitionBase lookupDefinition)
        {
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
        /// <returns></returns>
        public TableDefinitionBase HasRecordDescription(string description)
        {
            RecordDescription = description;
            return this;
        }

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

            if (primaryKey != null && primaryKey.IsValid && primaryKey.TableDefinition == this)
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

            if (primaryKey == null || !primaryKey.IsValid)
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
                        primaryKeyKeyValueField.Value.ToDecimal().ToDouble());
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

    }
}
