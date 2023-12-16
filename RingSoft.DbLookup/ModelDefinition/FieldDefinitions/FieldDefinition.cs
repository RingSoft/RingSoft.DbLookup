// ***********************************************************************
// Assembly         : RingSoft.DbLookup
// Author           : petem
// Created          : 12-19-2022
//
// Last Modified By : petem
// Last Modified On : 09-10-2023
// ***********************************************************************
// <copyright file="FieldDefinition.cs" company="Peter Ringering">
//     Copyright (c) 2023. All rights reserved.
// </copyright>
// <summary></summary>
// ***********************************************************************
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
        /// <value>The table definition.</value>
        public TableDefinitionBase TableDefinition { get; internal set; }

        /// <summary>
        /// Gets the name of the field.
        /// </summary>
        /// <value>The name of the field.</value>
        public string FieldName { get; internal set; }

        /// <summary>
        /// Gets the name of the property.
        /// </summary>
        /// <value>The name of the property.</value>
        public string PropertyName { get; internal set; }

        /// <summary>
        /// Gets the type of the field data.
        /// </summary>
        /// <value>The type of the field data.</value>
        public abstract FieldDataTypes FieldDataType { get; }

        /// <summary>
        /// Gets the type of the value for use in queries.
        /// </summary>
        /// <value>The type of the value.</value>
        public virtual ValueTypes ValueType => GblMethods.GetValueTypeForFieldDataType(FieldDataType);

        /// <summary>
        /// Gets the parent join foreign key definition.
        /// </summary>
        /// <value>The parent join foreign key definition.</value>
        public ForeignKeyDefinition ParentJoinForeignKeyDefinition
        {
            get => _parentJoinForeignKeyDefinition;
            internal set
            {
                _parentJoinForeignKeyDefinition = value;
            }
        }

        /// <summary>
        /// Gets a value indicating whether this field allows null values.
        /// </summary>
        /// <value><c>true</c> if [allow nulls]; otherwise, <c>false</c>.</value>
        public bool AllowNulls { get; internal set; }

        /// <summary>
        /// Gets the field description that the user will see.
        /// </summary>
        /// <value>
        /// The description.
        /// </value>
        private string _description;

        /// <summary>
        /// The parent join foreign key definition
        /// </summary>
        private ForeignKeyDefinition _parentJoinForeignKeyDefinition;

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

        /// <summary>
        /// Gets the search for host identifier.
        /// </summary>
        /// <value>The search for host identifier.</value>
        public virtual int? SearchForHostId { get; private set; }

        /// <summary>
        /// Gets the lookup control column identifier.
        /// </summary>
        /// <value>The lookup control column identifier.</value>
        public int LookupControlColumnId { get; internal set; }

        /// <summary>
        /// Gets a value indicating whether [allow recursion].
        /// </summary>
        /// <value><c>true</c> if [allow recursion]; otherwise, <c>false</c>.</value>
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

        /// <summary>
        /// Gets a value indicating whether [update only].
        /// </summary>
        /// <value><c>true</c> if [update only]; otherwise, <c>false</c>.</value>
        public bool UpdateOnly { get; private set; }

        /// <summary>
        /// Gets a value indicating whether [allow user nulls].
        /// </summary>
        /// <value><c>true</c> if [allow user nulls]; otherwise, <c>false</c>.</value>
        public bool AllowUserNulls { get; private set; } = true;

        /// <summary>
        /// Gets a value indicating whether [skip print].
        /// </summary>
        /// <value><c>true</c> if [skip print]; otherwise, <c>false</c>.</value>
        public bool SkipPrint { get; private set; }

        /// <summary>
        /// Gets the formula object.
        /// </summary>
        /// <value>The formula object.</value>
        public ILookupFormula FormulaObject { get; private set; }

        /// <summary>
        /// Gets the type of the field.
        /// </summary>
        /// <value>The type of the field.</value>
        public System.Type FieldType { get; private set; }

        /// <summary>
        /// Sets the type.
        /// </summary>
        /// <param name="type">The type.</param>
        internal void SetType(Type type)
        {
            FieldType = type;
        }

        /// <summary>
        /// Gets a value indicating whether [generated key].
        /// </summary>
        /// <value><c>true</c> if [generated key]; otherwise, <c>false</c>.</value>
        public bool GeneratedKey { get; private set; }

        /// <summary>
        /// Gets the search for condition.
        /// </summary>
        /// <value>The search for condition.</value>
        public Conditions? SearchForCondition { get; private set; }

        /// <summary>
        /// Gets a value indicating whether this instance can format value.
        /// </summary>
        /// <value><c>true</c> if this instance can format value; otherwise, <c>false</c>.</value>
        public bool CanFormatValue { get; private set; } = true;

        /// <summary>
        /// Initializes a new instance of the <see cref="FieldDefinition"/> class.
        /// </summary>
        internal FieldDefinition()
        {
            AllowNulls = true;
        }

        /// <summary>
        /// Sets the name of the field.  For use only by the Entity Framework classes.
        /// </summary>
        /// <param name="fieldName">Name of the field.</param>
        /// <exception cref="System.ArgumentException">Field name '{fieldName}' already exists in this table.</exception>
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
        /// <exception cref="System.Exception">Table Definition '{TableDefinition.TableName}' already has a parent join defined.  You need to add a field to the ParentJoinForeignKey object instead.</exception>
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
        public void IsRequired(bool value = true)
        {
            AllowNulls = !value;
        }

        /// <summary>
        /// Returns a <see cref="System.String" /> that represents this instance.
        /// </summary>
        /// <returns>A <see cref="System.String" /> that represents this instance.</returns>
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

        /// <summary>
        /// Determines whether [has search for host identifier] [the specified host identifier].
        /// </summary>
        /// <param name="hostId">The host identifier.</param>
        public void HasSearchForHostId(int hostId)
        {
            SearchForHostId = hostId;
        }

        /// <summary>
        /// Determines whether [has lookup control column identifier] [the specified lookup control column identifier].
        /// </summary>
        /// <param name="lookupControlColumnId">The lookup control column identifier.</param>
        public void HasLookupControlColumnId(int lookupControlColumnId)
        {
            LookupControlColumnId = lookupControlColumnId;
        }

        //public FieldDefinition DoesAllowRecursion(bool value = true)
        //{
        //    AllowRecursion = value;
        //    return this;
        //}

        /// <summary>
        /// Sets the update only.
        /// </summary>
        /// <param name="value">if set to <c>true</c> [value].</param>
        internal void SetUpdateOnly(bool value = true)
        {
            UpdateOnly = value;
        }

        /// <summary>
        /// Gets the SQL format object.
        /// </summary>
        /// <returns>System.String.</returns>
        public string GetSqlFormatObject()
        {
            var tableName = TableDefinition.TableName;
            tableName = TableDefinition.Context.DataProcessor.SqlGenerator.FormatSqlObject(tableName);
            var result = $"{tableName}.{TableDefinition.Context.DataProcessor.SqlGenerator.FormatSqlObject(FieldName)}";
            return result;
        }

        /// <summary>
        /// Determines whether this instance [can set null] the specified value.
        /// </summary>
        /// <param name="value">if set to <c>true</c> [value].</param>
        /// <returns>FieldDefinition.</returns>
        public FieldDefinition CanSetNull(bool value = true)
        {
            AllowUserNulls = value;
            return this;
        }

        /// <summary>
        /// Makes the path.
        /// </summary>
        /// <returns>System.String.</returns>
        public string MakePath()
        {
            return TableDefinition.TableName + "@" + FieldName + ";";
        }

        /// <summary>
        /// Gets the user value.
        /// </summary>
        /// <param name="dbIdValue">The database identifier value.</param>
        /// <returns>System.String.</returns>
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

        /// <summary>
        /// Formats the value for column map.
        /// </summary>
        /// <param name="value">The value.</param>
        /// <returns>System.String.</returns>
        public virtual string FormatValueForColumnMap(string value)
        {
            return FormatValue(value);
        }

        /// <summary>
        /// Does the skip print.
        /// </summary>
        /// <param name="value">if set to <c>true</c> [value].</param>
        internal void DoSkipPrint(bool value = true)
        {
            SkipPrint = value;
        }

        /// <summary>
        /// Determines whether [has formula object] [the specified lookup formula].
        /// </summary>
        /// <param name="lookupFormula">The lookup formula.</param>
        /// <returns>FieldDefinition.</returns>
        internal FieldDefinition HasFormulaObject(ILookupFormula lookupFormula)
        {
            FormulaObject = lookupFormula;
            TableDefinition.Context.RegisterLookupFormula(lookupFormula);
            return this;
        }

        /// <summary>
        /// Determines whether [is generated key] [the specified value].
        /// </summary>
        /// <param name="value">if set to <c>true</c> [value].</param>
        /// <returns>FieldDefinition.</returns>
        internal FieldDefinition IsGeneratedKey(bool value = true)
        {
            GeneratedKey = value;
            return this;
        }
    }
}
