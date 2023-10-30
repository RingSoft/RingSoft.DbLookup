using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using RingSoft.DataEntryControls.Engine;
using RingSoft.DbLookup.ModelDefinition;
using RingSoft.DbLookup.ModelDefinition.FieldDefinitions;

namespace RingSoft.DbLookup
{
    /// <summary>
    /// A primary key field definition/value pair.
    /// </summary>
    public class PrimaryKeyValueField
    {
        /// <summary>
        /// Gets the field definition.
        /// </summary>
        /// <value>
        /// The field definition.
        /// </value>
        public FieldDefinition FieldDefinition { get; }

        /// <summary>
        /// Gets or sets the value.
        /// </summary>
        /// <value>
        /// The value.
        /// </value>
        public string Value { get; set; }

        /// <summary>
        /// Initializes a new instance of the <see cref="PrimaryKeyValueField"/> class.
        /// </summary>
        /// <param name="fieldDefinition">The field definition.</param>
        public PrimaryKeyValueField(FieldDefinition fieldDefinition)
        {
            FieldDefinition = fieldDefinition;
        }

        public override string ToString()
        {
            return Value;
        }
    }
    /// <summary>
    /// A collection of all the fields and associated values of a record's primary key.
    /// </summary>
    public class PrimaryKeyValue
    {
        /// <summary>
        /// Gets the table definition.
        /// </summary>
        /// <value>
        /// The table definition.
        /// </value>
        public TableDefinitionBase TableDefinition { get; }

        /// <summary>
        /// Gets the primary key value fields.
        /// </summary>
        /// <value>
        /// The key value fields.
        /// </value>
        public IReadOnlyList<PrimaryKeyValueField> KeyValueFields => _fieldValues;

        private List<PrimaryKeyValueField> _fieldValues = new List<PrimaryKeyValueField>();

        /// <summary>
        /// Determines whether all the primary key value fields have data.
        /// </summary>
        /// <returns>
        ///   <c>false</c> if at least one value field is null; otherwise, <c>true</c>.
        /// </returns>
        internal bool IntIsValid
        {
            get
            {
                foreach (var primaryKeyValueField in KeyValueFields)
                {
                    if (primaryKeyValueField.Value.IsNullOrEmpty())
                        return false;
                }
                return true;
            }
        }

        public string KeyString
        {
            get
            {
                var result = string.Empty;

                if (IntIsValid)
                {
                    var firstRecord = true;
                    foreach (var keyValueField in KeyValueFields)
                    {
                        if (!firstRecord)
                        {
                            result += '\n';
                        }
                        result += keyValueField.Value;
                        firstRecord = false;
                    }
                }
                return result;
            }
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="PrimaryKeyValue"/> class.
        /// </summary>
        /// <param name="tableDefinition">The table definition.</param>
        public PrimaryKeyValue(TableDefinitionBase tableDefinition)
        {
            TableDefinition = tableDefinition;

            foreach (var primaryKeyField in tableDefinition.PrimaryKeyFields)
            {
                _fieldValues.Add(new PrimaryKeyValueField(primaryKeyField));
            }
        }

        public void PopulateFromDataRow(DataRow dataRow)
        {
            foreach (var keyValueField in _fieldValues)
            {
                if (dataRow.Table.Columns.Contains(keyValueField.FieldDefinition.FieldName))
                    keyValueField.Value = dataRow.GetRowValue(keyValueField.FieldDefinition.FieldName);
            }
        }

        public void LoadFromIdValue(string idValue)
        {
            if (KeyValueFields.Count > 1)
            {
                throw new Exception(
                    $"You can't run {nameof(LoadFromIdValue)} on PrimaryKeyValues that have more than 1 Primary Key Field");
            }

            KeyValueFields[0].Value = idValue;
        }

        public void LoadFromPrimaryString(string primaryKeyString)
        {
            var processedKeyString = primaryKeyString;
            var lfCharPos = processedKeyString.IndexOf("\n");
            var keyPos = 0;
            while (lfCharPos >= 0)
            {
                var keyValue = processedKeyString.LeftStr(lfCharPos);
                KeyValueFields[keyPos].Value = keyValue;
                processedKeyString = processedKeyString.RightStr(processedKeyString.Length - (lfCharPos + 1));
                keyPos++;
                lfCharPos = processedKeyString.IndexOf("\n");
            }
            KeyValueFields[keyPos].Value = processedKeyString;
        }

        /// <summary>
        /// Determines whether this primary key value is equal to the specified compare to primary key value.
        /// </summary>
        /// <param name="compareTo">The compare to.</param>
        /// <returns>
        ///   <c>true</c> if the primary key value is equal to the specified compare to primary key value; otherwise, <c>false</c>.
        /// </returns>
        /// <exception cref="ArgumentException">Compare To Table Definition does not match this Table Definition</exception>
        public bool IsEqualTo(PrimaryKeyValue compareTo)
        {
            if (compareTo == null)
                return false;

            if (TableDefinition != compareTo.TableDefinition)
                throw new ArgumentException("Compare To Table Definition does not match this Table Definition");

            foreach (var primaryKeyValueField in KeyValueFields)
            {
                var compareToKeyValueField = compareTo.KeyValueFields.FirstOrDefault(f =>
                    f.FieldDefinition.FieldName == primaryKeyValueField.FieldDefinition.FieldName);
                if (compareToKeyValueField != null)
                {
                    if (compareToKeyValueField.Value != primaryKeyValueField.Value)
                        return false;
                }
            }

            return true;
        }

        /// <summary>
        /// Copies from primary key value.
        /// </summary>
        /// <param name="sourcePrimaryKeyValue">The source primary key value.</param>
        /// <exception cref="ArgumentException">Source Table Definition does not match this Table Definition</exception>
        public void CopyFromPrimaryKeyValue(PrimaryKeyValue sourcePrimaryKeyValue)
        {
            if (TableDefinition != sourcePrimaryKeyValue.TableDefinition)
                throw new ArgumentException("Source Table Definition does not match this Table Definition");

            foreach (var keyValueField in _fieldValues)
            {
                keyValueField.Value = sourcePrimaryKeyValue.KeyValueFields
                    .FirstOrDefault(f => f.FieldDefinition.FieldName == keyValueField.FieldDefinition.FieldName)
                    ?.Value;
            }
        }

        public bool CreateRecordLock()
        {
            return GblMethods.DoRecordLock(this);
        }
    }
}
