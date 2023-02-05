using System;
using System.Collections.Generic;
using System.Linq;
using RingSoft.DataEntryControls.Engine;
using RingSoft.DbLookup.AdvancedFind;
using RingSoft.DbLookup.DataProcessor;
using RingSoft.DbLookup.ModelDefinition.FieldDefinitions;
using RingSoft.DbLookup.QueryBuilder;

namespace RingSoft.DbLookup.Lookup
{
    /// <summary>
    /// A lookup column based on a field definition.
    /// </summary>
    /// <seealso cref="LookupColumnDefinitionBase" />
    public class LookupFieldColumnDefinition
        : LookupColumnDefinitionType<LookupFieldColumnDefinition>
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
        public FieldDefinition FieldDefinition { get; internal set; }

        /// <summary>
        /// Gets a value indicating whether this column is distinct.
        /// </summary>
        /// <value>
        ///   <c>true</c> if distinct; otherwise, <c>false</c>.
        /// </value>
        public bool Distinct { get; private set; }

        public override int? SearchForHostId
        {
            get
            {
                var result = base.SearchForHostId;
                if (result == null)
                    result = FieldDefinition.SearchForHostId;

                return result;
            }
            internal set => base.SearchForHostId = value;
        } 

        private string _selectSqlAlias = string.Empty;

        internal LookupFieldColumnDefinition(FieldDefinition fieldDefinition)
        {
            SetFieldDefinition(fieldDefinition);
            SetupColumn();
        }

        internal LookupFieldColumnDefinition()
        {

        }

        internal override void CopyFrom(LookupColumnDefinitionBase source)
        {
            if (source is LookupFieldColumnDefinition sourceFieldColumn)
            {
                JoinQueryTableAlias = sourceFieldColumn.JoinQueryTableAlias;
                Distinct = sourceFieldColumn.Distinct;
                ParentField = sourceFieldColumn.ParentField;
                var test = this;
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

        public override string GetTextForColumn(PrimaryKeyValue primaryKeyValue)
        {
            if (primaryKeyValue.TableDefinition != FieldDefinition.TableDefinition)
            {
                throw new Exception("Invalid column primaryKey");
            }
            var query = new SelectQuery(primaryKeyValue.TableDefinition.TableName);
            query.AddSelectColumn(FieldDefinition.FieldName);
            foreach (var primaryKeyField in primaryKeyValue.KeyValueFields)
            {
                query.AddWhereItem(primaryKeyField.FieldDefinition.FieldName, Conditions.Equals, primaryKeyField.Value);
            }

            var dataProcessResult = primaryKeyValue.TableDefinition.Context.DataProcessor.GetData(query);
            if (dataProcessResult.ResultCode == GetDataResultCodes.Success)
            {
                return dataProcessResult.DataSet.Tables[0].Rows[0]
                    .GetRowValue(FieldDefinition.FieldName);
            }

            return "";
        }

        private void SetFieldDefinition(FieldDefinition fieldDefinition)
        {
            FieldDefinition = fieldDefinition;
            _selectSqlAlias = $"{fieldDefinition.FieldName}_{Guid.NewGuid().ToString().Replace("-", "").ToUpper()}";
            if (LookupControlColumnId == 0 && fieldDefinition.LookupControlColumnId != 0)
                LookupControlColumnId = fieldDefinition.LookupControlColumnId;
        }

        /// <summary>
        /// Determines whether this column is distinct.
        /// </summary>
        /// <param name="value">if set to <c>true</c> [value].</param>
        /// <returns>This object for fluent processing.</returns>
        public LookupFieldColumnDefinition IsDistinct(bool value = true)
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

        private void ValidateNonPrimaryKeyFields(IReadOnlyList<LookupColumnDefinitionBase> columns)
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

        public override TreeViewType TreeViewType => TreeViewType.Field;

        protected override LookupColumnAlignmentTypes SetupDefaultHorizontalAlignment()
        {
            if (DataType == FieldDataTypes.Integer)
            {
                if (FieldDefinition.TableDefinition.PrimaryKeyFields.Contains(FieldDefinition))
                    return LookupColumnAlignmentTypes.Left;

                if (FieldDefinition is IntegerFieldDefinition integerFieldDefinition &&
                    integerFieldDefinition.EnumTranslation != null)
                    return LookupColumnAlignmentTypes.Left;
            }

            return base.SetupDefaultHorizontalAlignment();
        }

        public override FieldDefinition GetFieldForColumn()
        {
            return FieldDefinition;
        }

        public override void AddNewColumnDefinition(LookupDefinitionBase lookupDefinition)
        {
            LookupColumnDefinitionBase newColumn = new LookupFieldColumnDefinition(FieldDefinition);

            if (FieldDefinition.ParentJoinForeignKeyDefinition != null)
            {
                var initColumn = FieldDefinition.ParentJoinForeignKeyDefinition.PrimaryTable.LookupDefinition
                    .InitialSortColumnDefinition;

                var formula = initColumn.GetFormulaForColumn();
                if (!formula.IsNullOrEmpty())
                {
                    newColumn = new LookupFormulaColumnDefinition(formula, FieldDataTypes.String);
                }

            }
            if (Path.IsNullOrEmpty())
            {
                Path = FieldDefinition.MakePath();
            }

            newColumn.LookupDefinition = lookupDefinition;
            //newColumn.JoinQueryTableAlias = JoinQueryTableAlias;
            ProcessNewVisibleColumn(newColumn, lookupDefinition);

            base.AddNewColumnDefinition(lookupDefinition);
        }

        internal override string LoadFromTreeViewItem(TreeViewItem item)
        {
            LookupDefinition = item.BaseTree.LookupDefinition;
            var formula = CheckForeignFormula(item);
            if (!formula.IsNullOrEmpty())
            {
                return formula;
            }
            return base.LoadFromTreeViewItem(item);
        }

        private string CheckForeignFormula(TreeViewItem item)
        {
            if (FieldDefinition.ParentJoinForeignKeyDefinition != null)
            {
                var initialColumn = FieldDefinition.ParentJoinForeignKeyDefinition.PrimaryTable.LookupDefinition
                    .InitialSortColumnDefinition;

                var formula = initialColumn.GetFormulaForColumn();
                if (!formula.IsNullOrEmpty())
                {
                    return formula;
                }

                if (initialColumn is LookupFieldColumnDefinition lookupFieldColumn)
                {
                    CopyFrom(initialColumn);
                    LookupDefinition = item.BaseTree.LookupDefinition;
                    SetFieldDefinition(lookupFieldColumn.FieldDefinition);
                    var path = item.MakePath() + FieldDefinition.MakePath();
                    var newItem = item.BaseTree.ProcessFoundTreeViewItem(path);
                    if (newItem != null)
                    {
                        var newFieldResult = item.BaseTree.MakeIncludes(newItem);
                        if (newFieldResult != null)
                        {
                            JoinQueryTableAlias = newFieldResult.LookupJoin.JoinDefinition.Alias;
                        }
                    }
                }
            }

            return string.Empty;
        }

        public override string ToString()
        {
            if (Caption.IsNullOrEmpty())
            {
                return FieldDefinition.Description;
            }
            return base.ToString();
        }

        internal override void LoadFromEntity(AdvancedFindColumn entity, LookupDefinitionBase lookupDefinition)
        {
            TreeViewItem foundItem = null;
            if (!entity.Path.IsNullOrEmpty())
            {
                foundItem =
                    lookupDefinition.AdvancedFindTree.ProcessFoundTreeViewItem(entity.Path, TreeViewType.Field);
            }

            if (foundItem != null)
            {
                SetFieldDefinition(foundItem.FieldDefinition);
            }
            var test = this;
            base.LoadFromEntity(entity, lookupDefinition);
        }
    }
}