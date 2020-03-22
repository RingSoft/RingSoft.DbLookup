using System;
using System.Collections.Generic;
using System.Linq;
using RingSoft.DbLookup.ModelDefinition.FieldDefinitions;

namespace RingSoft.DbLookup.Lookup
{
    /// <summary>
    /// A lookup column based on a field definition.
    /// </summary>
    /// <seealso cref="LookupColumnBase" />
    public class LookupFieldColumnDefinition : LookupColumnType<LookupFieldColumnDefinition>
    {
        /// <summary>
        /// Gets the type of the column.
        /// </summary>
        /// <value>
        /// The type of the column.
        /// </value>
        public override LookupColumnTypes ColumnType => LookupColumnTypes.Field;
        /// <summary>
        /// Gets the type of the data.
        /// </summary>
        /// <value>
        /// The type of the data.
        /// </value>
        public override FieldDataTypes DataType => FieldDefinition.FieldDataType;
        /// <summary>
        /// Gets the select SQL alias.
        /// </summary>
        /// <value>
        /// The select SQL alias.
        /// </value>
        public override string SelectSqlAlias => _selectSqlAlias;

        /// <summary>
        /// Gets the field definition.
        /// </summary>
        /// <value>
        /// The field definition.
        /// </value>
        public FieldDefinition FieldDefinition { get; private set; }

        /// <summary>
        /// Gets the join query table alias.
        /// </summary>
        /// <value>
        /// The join query table alias.
        /// </value>
        public string JoinQueryTableAlias { get; internal set; }

        /// <summary>
        /// Gets a value indicating whether this column is distinct.
        /// </summary>
        /// <value>
        ///   <c>true</c> if distinct; otherwise, <c>false</c>.
        /// </value>
        public bool Distinct { get; private set; }

        private string _selectSqlAlias = string.Empty;

        internal LookupFieldColumnDefinition(FieldDefinition fieldDefinition)
        {
            SetFieldDefinition(fieldDefinition);
            SetupColumn();
        }


        internal override void CopyFrom(LookupColumnBase source)
        {
            if (source is LookupFieldColumnDefinition sourceFieldColumn)
            {
                JoinQueryTableAlias = sourceFieldColumn.JoinQueryTableAlias;
                Distinct = sourceFieldColumn.Distinct;
            }
            base.CopyFrom(source);
        }

        /// <summary>
        /// Formats the value to display in the lookup view.
        /// </summary>
        /// <param name="value">The value from the database.</param>
        /// <returns>
        /// The formatted value.
        /// </returns>
        public override string FormatValue(string value)
        {
            return FieldDefinition.FormatValue(value);
        }

        private void SetFieldDefinition(FieldDefinition fieldDefinition)
        {
            FieldDefinition = fieldDefinition;
            _selectSqlAlias = $"{fieldDefinition.FieldName}_{Guid.NewGuid().ToString().Replace("-", "").ToUpper()}";
        }

        /// <summary>
        /// Determines whether this column is distinct.
        /// </summary>
        /// <param name="value">if set to <c>true</c> [value].</param>
        /// <returns>This object for fluent processing.</returns>
        public LookupColumnBase IsDistinct(bool value = true)
        {
            var isPrimaryKey = FieldDefinition.TableDefinition.PrimaryKeyFields.Count > 1 &&
                               FieldDefinition.TableDefinition.PrimaryKeyFields.Contains(FieldDefinition);
            if (!isPrimaryKey && value)
                throw new ArgumentException(
                    "The distinct value can only be set on primary key field columns where there are at least 2 fields in the primary key.");
            else if (isPrimaryKey && value)
            {
                ValidateNonPrimaryKeyFields(LookupDefinition.VisibleColumns);
                ValidateNonPrimaryKeyFields(LookupDefinition.HiddenColumns);
            }

            Distinct = value;
            return this;
        }

        private void ValidateNonPrimaryKeyFields(IReadOnlyList<LookupColumnBase> columns)
        {
            var nonPrimaryFieldsFound = columns.Any(a => a.ColumnType == LookupColumnTypes.Formula);
            if (!nonPrimaryFieldsFound)
            {
                if (columns is IReadOnlyList<LookupFieldColumnDefinition> fieldColumns)
                    foreach (var fieldColumn in fieldColumns)
                    {
                        if (!fieldColumn.FieldDefinition.TableDefinition.PrimaryKeyFields.Contains(fieldColumn
                            .FieldDefinition))
                            throw new ArgumentException(
                                "Setting the distinct property value on primary key columns on lookup definitions with non-primary key columns is not allowed.");
                    }
            }
        }
    }
}
