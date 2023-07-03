using System;
using System.Globalization;
using System.Linq;
using RingSoft.DataEntryControls.Engine;
using RingSoft.DbLookup.Lookup;
using RingSoft.DbLookup.QueryBuilder;

namespace RingSoft.DbLookup.ModelDefinition.FieldDefinitions
{
    /// <summary>
    /// A database field definition.
    /// </summary>
    public abstract class FieldDefinition
    {
        /// <summary>
        /// Gets the table definition.
        /// </summary>
        /// <value>
        /// The table definition.
        /// </value>
        public TableDefinitionBase TableDefinition { get; internal set; }

        /// <summary>
        /// Gets the name of the field.
        /// </summary>
        /// <value>
        /// The name of the field.
        /// </value>
        public string FieldName { get; internal set; }

        /// <summary>
        /// Gets the name of the property.
        /// </summary>
        /// <value>
        /// The name of the property.
        /// </value>
        public string PropertyName { get; internal set; }

        /// <summary>
        /// Gets the type of the field data.
        /// </summary>
        /// <value>
        /// The type of the field data.
        /// </value>
        public abstract FieldDataTypes FieldDataType { get; }

        /// <summary>
        /// Gets the type of the value for use in queries.
        /// </summary>
        /// <value>
        /// The type of the value.
        /// </value>
        public virtual ValueTypes ValueType => GblMethods.GetValueTypeForFieldDataType(FieldDataType);

        /// <summary>
        /// Gets the parent join foreign key definition.
        /// </summary>
        /// <value>
        /// The parent join foreign key definition.
        /// </value>
        public ForeignKeyDefinition ParentJoinForeignKeyDefinition { get; internal set; }

        /// <summary>
        /// Gets a value indicating whether this field allows null values.
        /// </summary>
        /// <value>
        ///   <c>true</c> if [allow nulls]; otherwise, <c>false</c>.
        /// </value>
        public bool AllowNulls { get; internal set; }

        /// <summary>
        /// Gets the field description that the user will see.
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
                    var newDescription = PropertyName.ConvertPropertyNameToDescription();

                    if (newDescription.EndsWith("Id", true, CultureInfo.CurrentCulture) &&
                        ParentJoinForeignKeyDefinition != null)
                    {
                        newDescription = newDescription.Replace("Id", "", true, CultureInfo.CurrentCulture);
                    }

                    return newDescription;
                }

                return _description;
            }
            internal set { _description = value; }
        }

        public virtual int? SearchForHostId { get; private set; }

        public int LookupControlColumnId { get; internal set; }

        public bool AllowRecursion
        {
            get
            {
                var result = true;
                if (ParentJoinForeignKeyDefinition != null)
                {
                    if (ParentJoinForeignKeyDefinition.PrimaryTable == ParentJoinForeignKeyDefinition.ForeignTable)
                    {
                        return false;
                    }
                }
                return result;
            }
        }

        public bool UpdateOnly { get; private set; }

        public bool AllowUserNulls { get; private set; } = true;

        public bool SkipPrint { get; private set; }

        public ILookupFormula FormulaObject { get; private set; }

        public System.Type FieldType { get; private set; }

        internal void SetType(Type type)
        {
            FieldType = type;
        }

        public bool GeneratedKey { get; private set; }

        public Conditions? SearchForCondition { get; private set; }

        internal FieldDefinition()
        {
            AllowNulls = true;
        }

        /// <summary>
        /// Sets the name of the field.  For use only by the Entity Framework classes.
        /// </summary>
        /// <param name="fieldName">Name of the field.</param>
        public void HasFieldName(string fieldName)
        {
            var field = TableDefinition.FieldDefinitions.FirstOrDefault(f => f.FieldName == fieldName);
            if (field != null && field != this)
            {
                throw new ArgumentException($"Field name '{fieldName}' already exists in this table.");
            }

            FieldName = fieldName;
        }

        /// <summary>
        /// Sets the field description that the user will see.
        /// </summary>
        /// <param name="description">The description value.</param>
        /// <returns>This object.</returns>
        public FieldDefinition HasDescription(string description)
        {
            Description = description;
            return this;
        }

        /// <summary>
        /// Sets the parent foreign key definition.  For use only by the Entity Framework classes.
        /// </summary>
        /// <param name="parentFieldDefinition">The parent field definition.</param>
        /// <param name="propertyName">The object's property name</param>
        /// <returns>This object.</returns>
        public ForeignKeyDefinition SetParentField(FieldDefinition parentFieldDefinition, string propertyName)
        {
            if (ParentJoinForeignKeyDefinition == null)
            {
                ParentJoinForeignKeyDefinition = new ForeignKeyDefinition
                {
                    PrimaryTable = parentFieldDefinition.TableDefinition,
                    ForeignTable = TableDefinition,
                    ForeignObjectPropertyName = propertyName
                };
                ParentJoinForeignKeyDefinition.AddFieldJoin(parentFieldDefinition, this, true);
            }
            else
            {
                throw new Exception(
                    $"Table Definition '{TableDefinition.TableName}' already has a parent join defined.  You need to add a field to the ParentJoinForeignKey object instead.");
            }

            return ParentJoinForeignKeyDefinition;
        }

        /// <summary>
        /// Determines whether this field will allow nulls.  For use only by the Entity Framework classes.
        /// </summary>
        /// <param name="value">if set to <c>true</c> [value].</param>
        /// <returns></returns>
        public void IsRequired(bool value = true)
        {
            AllowNulls = !value;
        }

        public override string ToString()
        {
            if (!Description.IsNullOrEmpty())
                return Description;

            if (ParentJoinForeignKeyDefinition != null)
                if (!ParentJoinForeignKeyDefinition.ForeignObjectPropertyName.IsNullOrEmpty())
                    return ParentJoinForeignKeyDefinition.ForeignObjectPropertyName;

            if (!PropertyName.IsNullOrEmpty())
                return PropertyName;

            return FieldName;
        }

        /// <summary>
        /// Formats the value to display.
        /// </summary>
        /// <param name="value">The value from the database.</param>
        /// <returns>The formatted value.</returns>
        public abstract string FormatValue(string value);

        /// <summary>
        /// Validates a value to see if it's a valid value to save to the database.
        /// </summary>
        /// <param name="value">The value to validate.</param>
        /// <returns>True if the value is safe to save to the database.</returns>
        public virtual bool ValidateValueForSavingToDb(string value)
        {
            if (GeneratedKey && value.IsNullOrEmpty())
            {
                return true;
            }
            if (!AllowNulls && value.IsNullOrEmpty())
                return false;

            return true;
        }

        public void HasSearchForHostId(int hostId)
        {
            SearchForHostId = hostId;
        }

        public void HasLookupControlColumnId(int lookupControlColumnId)
        {
            LookupControlColumnId = lookupControlColumnId;
        }

        //public FieldDefinition DoesAllowRecursion(bool value = true)
        //{
        //    AllowRecursion = value;
        //    return this;
        //}

        internal void SetUpdateOnly(bool value = true)
        {
            UpdateOnly = value;
        }

        public string GetSqlFormatObject()
        {
            var tableName = TableDefinition.TableName;
            tableName = TableDefinition.Context.DataProcessor.SqlGenerator.FormatSqlObject(tableName);
            var result = $"{tableName}.{TableDefinition.Context.DataProcessor.SqlGenerator.FormatSqlObject(FieldName)}";
            return result;
        }

        public FieldDefinition CanSetNull(bool value = true)
        {
            AllowUserNulls = value;
            return this;
        }

        public string MakePath()
        {
            return TableDefinition.TableName + "@" + FieldName + ";";
        }

        public virtual string GetUserValue(string dbIdValue)
        {
            var result = string.Empty;

            if (TableDefinition.PrimaryKeyFields.Contains(this) && TableDefinition.PrimaryKeyFields.Count <= 1)
            {
                var autoFillValue = TableDefinition.Context
                    .OnAutoFillTextRequest(TableDefinition, dbIdValue);
                if (autoFillValue == null)
                {
                    result = "<Not Found>";
                }
                else
                {
                    result = autoFillValue.Text;
                }
            }
            else if (ParentJoinForeignKeyDefinition != null)
            {


                var requestResult = TableDefinition.Context
                    .OnAutoFillTextRequest(ParentJoinForeignKeyDefinition.PrimaryTable, dbIdValue);
                result = requestResult?.Text;
                if (result.IsNullOrEmpty())
                {
                    if (ParentJoinForeignKeyDefinition.FieldJoins.Count == 1)
                    {
                        result = "<Not Found>";
                    }
                    else
                    {
                        result = dbIdValue;
                    }
                }


                return result;
            }
            else
            {
                result = FormatValue(dbIdValue);
            }

            return result;
        }

        public virtual string FormatValueForColumnMap(string value)
        {
            return FormatValue(value);
        }

        internal void DoSkipPrint(bool value = true)
        {
            SkipPrint = value;
        }

        internal FieldDefinition HasFormulaObject(ILookupFormula lookupFormula)
        {
            FormulaObject = lookupFormula;
            TableDefinition.Context.RegisterLookupFormula(lookupFormula);
            return this;
        }

        internal FieldDefinition IsGeneratedKey(bool value = true)
        {
            GeneratedKey = value;
            return this;
        }
    }
}
