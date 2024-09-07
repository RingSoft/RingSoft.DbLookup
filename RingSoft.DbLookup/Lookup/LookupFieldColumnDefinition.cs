// ***********************************************************************
// Assembly         : RingSoft.DbLookup
// Author           : petem
// Created          : 12-19-2022
//
// Last Modified By : petem
// Last Modified On : 08-02-2024
// ***********************************************************************
// <copyright file="LookupFieldColumnDefinition.cs" company="Peter Ringering">
//     Copyright (c) 2023. All rights reserved.
// </copyright>
// <summary></summary>
// ***********************************************************************
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
        /// <value>The type of the column.</value>
        public override LookupColumnTypes ColumnType => LookupColumnTypes.Field;

        /// <summary>
        /// Gets the type of the data.
        /// </summary>
        /// <value>The type of the data.</value>
        public override FieldDataTypes DataType
        {
            get
            {
                if (FieldToDisplay == null)
                {
                    return FieldDefinition.FieldDataType;
                }

                return FieldToDisplay.FieldDataType;
            }
        }
        /// <summary>
        /// Gets the select SQL alias.
        /// </summary>
        /// <value>The select SQL alias.</value>
        public override string SelectSqlAlias => _selectSqlAlias;

        /// <summary>
        /// Gets the field definition.
        /// </summary>
        /// <value>The field definition.</value>
        public FieldDefinition FieldDefinition { get; internal set; }

        /// <summary>
        /// The field to display
        /// </summary>
        private FieldDefinition _fieldToDisplay;

        /// <summary>
        /// Gets the field to display.
        /// </summary>
        /// <value>The field to display.</value>
        public FieldDefinition FieldToDisplay
        {
            get
            {
                if (_fieldToDisplay == null)
                {
                    return FieldDefinition;
                }
                return _fieldToDisplay;
            }
            internal set => _fieldToDisplay = value;
        }


        /// <summary>
        /// Gets a value indicating whether this column is distinct.
        /// </summary>
        /// <value><c>true</c> if distinct; otherwise, <c>false</c>.</value>
        public bool Distinct { get; private set; }

        /// <summary>
        /// Gets the search for host identifier.
        /// </summary>
        /// <value>The search for host identifier.</value>
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

        /// <summary>
        /// Gets a value indicating whether [allow nulls].
        /// </summary>
        /// <value><c>true</c> if [allow nulls]; otherwise, <c>false</c>.</value>
        public bool AllowNulls { get; internal set; }

        /// <summary>
        /// The select SQL alias
        /// </summary>
        private string _selectSqlAlias = string.Empty;

        /// <summary>
        /// Initializes a new instance of the <see cref="LookupFieldColumnDefinition" /> class.
        /// </summary>
        /// <param name="fieldDefinition">The field definition.</param>
        internal LookupFieldColumnDefinition(FieldDefinition fieldDefinition)
        {
            SetFieldDefinition(fieldDefinition);
            SetupColumn();
        }

        /// <summary>
        /// Setups the column.
        /// </summary>
        internal override void SetupColumn()
        {
            base.SetupColumn();
            if (FieldDefinition.ParentJoinForeignKeyDefinition != null)
            {
                HorizontalAlignment = LookupColumnAlignmentTypes.Left;
            }
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="LookupFieldColumnDefinition" /> class.
        /// </summary>
        internal LookupFieldColumnDefinition()
        {

        }

        /// <summary>
        /// Copies from.
        /// </summary>
        /// <param name="source">The source.</param>
        internal override void CopyFrom(LookupColumnDefinitionBase source)
        {
            if (source is LookupFieldColumnDefinition sourceFieldColumn)
            {
                JoinQueryTableAlias = sourceFieldColumn.JoinQueryTableAlias;
                Distinct = sourceFieldColumn.Distinct;
                ParentField = sourceFieldColumn.ParentField;
                SearchForHostId = sourceFieldColumn.SearchForHostId;
                var test = this;
            }
            base.CopyFrom(source);
        }

        /// <summary>
        /// Formats the value to display in the lookup view.
        /// </summary>
        /// <param name="value">The value from the database.</param>
        /// <returns>The formatted value.</returns>
        public override string FormatValue(string value)
        {
            if (SearchForHostId != null)
            {
                var formattedVaue =
                    FieldDefinition.TableDefinition.Context.FormatValueForSearchHost(
                        SearchForHostId.GetValueOrDefault(), value, FieldDefinition);

                if (formattedVaue.IsNullOrEmpty())
                {
                    return FieldDefinition.FormatValue(value);
                }
                else
                {
                    return formattedVaue;
                }
            }
            return FieldDefinition.FormatValue(value);
        }

        /// <summary>
        /// Gets the text for column.
        /// </summary>
        /// <param name="primaryKeyValue">The primary key value.</param>
        /// <returns>System.String.</returns>
        /// <exception cref="System.Exception">Invalid column primaryKey</exception>
        public override string GetTextForColumn(PrimaryKeyValue primaryKeyValue)
        {
            if (primaryKeyValue.TableDefinition != FieldDefinition.TableDefinition)
            {
                throw new Exception("Invalid column primaryKey");
            }

            var selectQuery = FieldDefinition
                .TableDefinition
                .LookupDefinition
                .GetSelectQueryMaui();

            selectQuery.SetMaxRecords(1);
            selectQuery.AddColumn(FieldDefinition);
            foreach (var primaryKeyValueField in primaryKeyValue.KeyValueFields)
            {
                selectQuery.Filter.AddFixedFilter(primaryKeyValueField.FieldDefinition, Conditions.Equals,
                    primaryKeyValueField.Value);
            }
            if (selectQuery.GetData())
            {
                var result= selectQuery.GetPropertyValue(0, FieldDefinition.PropertyName);
                if (FieldDefinition is DateFieldDefinition dateField)
                {
                    var date = result.ToDate();
                    if (dateField.ConvertToLocalTime)
                    {
                        date = date.GetValueOrDefault().ToLocalTime();
                        result = date.GetValueOrDefault().FormatDateValue(dateField.DateType);
                    }
                }

                return result;
            }

            //var query = new SelectQuery(primaryKeyValue.TableDefinition.TableName);
            //query.AddSelectColumn(FieldDefinition.FieldName);
            //foreach (var primaryKeyField in primaryKeyValue.KeyValueFields)
            //{
            //    query.AddWhereItem(primaryKeyField.FieldDefinition.FieldName, Conditions.Equals, primaryKeyField.Value);
            //}

            //var dataProcessResult = primaryKeyValue.TableDefinition.Context.DataProcessor.GetData(query);
            //if (dataProcessResult.ResultCode == GetDataResultCodes.Success)
            //{
            //    return dataProcessResult.DataSet.Tables[0].Rows[0]
            //        .GetRowValue(FieldDefinition.FieldName);
            //}

            return "";
        }

        /// <summary>
        /// Sets the field definition.
        /// </summary>
        /// <param name="fieldDefinition">The field definition.</param>
        private void SetFieldDefinition(FieldDefinition fieldDefinition)
        {
            FieldDefinition = fieldDefinition;
            SearchForHostId = fieldDefinition.SearchForHostId;
            AllowNulls = FieldDefinition.AllowNulls;

            FieldToDisplay = fieldDefinition;
            _selectSqlAlias = $"{fieldDefinition.FieldName}_{Guid.NewGuid().ToString().Replace("-", "").ToUpper()}";
            if (LookupControlColumnId == 0 && fieldDefinition.LookupControlColumnId != 0)
                LookupControlColumnId = fieldDefinition.LookupControlColumnId;
        }

        /// <summary>
        /// Determines whether this column is distinct.
        /// </summary>
        /// <param name="value">if set to <c>true</c> [value].</param>
        /// <returns>This object for fluent processing.</returns>
        /// <exception cref="System.ArgumentException">The distinct value can only be set on primary key field columns where there are at least 2 fields in the primary key.</exception>
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

        /// <summary>
        /// Validates the non primary key fields.
        /// </summary>
        /// <param name="columns">The columns.</param>
        /// <exception cref="System.ArgumentException">Setting the distinct property value on primary key columns on lookup definitions with non-primary key columns is not allowed.</exception>
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

        /// <summary>
        /// Gets the type of the TreeView.
        /// </summary>
        /// <value>The type of the TreeView.</value>
        public override TreeViewType TreeViewType => TreeViewType.Field;

        /// <summary>
        /// Setups the default horizontal alignment.
        /// </summary>
        /// <returns>LookupColumnAlignmentTypes.</returns>
        protected override LookupColumnAlignmentTypes SetupDefaultHorizontalAlignment()
        {
            if (FieldDefinition != null)
            {
                if (DataType == FieldDataTypes.Integer)
                {
                    if (FieldDefinition.TableDefinition.PrimaryKeyFields.Contains(FieldDefinition))
                        return LookupColumnAlignmentTypes.Left;

                    if (FieldDefinition is IntegerFieldDefinition integerFieldDefinition &&
                        integerFieldDefinition.EnumTranslation != null)
                        return LookupColumnAlignmentTypes.Left;
                }

                if (SearchForHostId > 0)
                {
                    return LookupColumnAlignmentTypes.Left;
                }
            }

            return base.SetupDefaultHorizontalAlignment();
        }

        /// <summary>
        /// Gets the field for column.
        /// </summary>
        /// <returns>FieldDefinition.</returns>
        public override FieldDefinition GetFieldForColumn()
        {
            return FieldDefinition;
        }

        /// <summary>
        /// Adds the new column definition.
        /// </summary>
        /// <param name="lookupDefinition">The lookup definition.</param>
        public override void AddNewColumnDefinition(LookupDefinitionBase lookupDefinition)
        {
            LookupColumnDefinitionBase newColumn = new LookupFieldColumnDefinition(FieldDefinition);
            if (FieldDefinition.ParentJoinForeignKeyDefinition != null)
            {
                LookupColumnDefinitionBase initColumn = null;
                if (FieldDefinition.ParentJoinForeignKeyDefinition.PrimaryTable.LookupDefinition != null)
                {
                    initColumn = FieldDefinition.ParentJoinForeignKeyDefinition.PrimaryTable.LookupDefinition
                        .InitialSortColumnDefinition;
                }

                if (initColumn != null)
                {
                    var lookupFormula = initColumn.GetFormulaForColumn();
                    if (lookupFormula != null)
                    {
                        newColumn = new LookupFormulaColumnDefinition(lookupFormula, FieldDataTypes.String);
                    }
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

        /// <summary>
        /// Processes the new visible column.
        /// </summary>
        /// <param name="columnDefinition">The column definition.</param>
        /// <param name="lookupDefinition">The lookup definition.</param>
        /// <param name="copyFrom">if set to <c>true</c> [copy from].</param>
        protected internal override void ProcessNewVisibleColumn(LookupColumnDefinitionBase columnDefinition, LookupDefinitionBase lookupDefinition,
            bool copyFrom = true)
        {
            base.ProcessNewVisibleColumn(columnDefinition, lookupDefinition, copyFrom);


            if (!Path.IsNullOrEmpty())
            {
                var foundTreeItem = lookupDefinition.AdvancedFindTree.ProcessFoundTreeViewItem(Path);

                if (foundTreeItem.Items.Any() || !foundTreeItem.FieldDefinition.AllowRecursion)
                {
                    var searchField = foundTreeItem.FieldDefinition;

                    var columnToDisplay = searchField
                        .ParentJoinForeignKeyDefinition
                        .PrimaryTable
                        .LookupDefinition.InitialSortColumnDefinition;

                    if (columnToDisplay is LookupFieldColumnDefinition fieldColumn)
                    {
                        FieldToDisplay = fieldColumn.FieldDefinition;
                    }

                }

            }
        }

        /// <summary>
        /// Loads from TreeView item.
        /// </summary>
        /// <param name="item">The item.</param>
        /// <returns>System.String.</returns>
        internal override string LoadFromTreeViewItem(TreeViewItem item)
        {
            LookupDefinition = item.BaseTree.LookupDefinition;
            return base.LoadFromTreeViewItem(item);
        }

        /// <summary>
        /// Checks the foreign formula.
        /// </summary>
        /// <param name="item">The item.</param>
        /// <returns>ILookupFormula.</returns>
        private ILookupFormula CheckForeignFormula(TreeViewItem item)
        {
            if (FieldDefinition.ParentJoinForeignKeyDefinition != null && FieldDefinition.ParentJoinForeignKeyDefinition.FieldJoins.Count == 1)
            {
                var initialColumn = FieldDefinition.ParentJoinForeignKeyDefinition.PrimaryTable.LookupDefinition
                    .InitialSortColumnDefinition;

                var lookupFormula = initialColumn.GetFormulaForColumn();
                if (lookupFormula != null)
                {
                    return lookupFormula;
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
                            NavigationProperties = newFieldResult
                                .LookupJoin.JoinDefinition
                                .GetNavigationProperties();
                        }
                    }
                }
            }

            return null;
        }

        /// <summary>
        /// Returns a <see cref="System.String" /> that represents this instance.
        /// </summary>
        /// <returns>A <see cref="System.String" /> that represents this instance.</returns>
        public override string ToString()
        {
            if (Caption.IsNullOrEmpty())
            {
                return FieldDefinition.Description;
            }
            return base.ToString();
        }

        /// <summary>
        /// Loads from entity.
        /// </summary>
        /// <param name="entity">The entity.</param>
        /// <param name="lookupDefinition">The lookup definition.</param>
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
            //var test = this;
            base.LoadFromEntity(entity, lookupDefinition);

            if (foundItem != null)
            {
                if (ParentObject != null)
                {
                    var properties = ParentObject.GetNavigationProperties();

                    HasNavProperties = properties.Any();
                }
            }

        }

        /// <summary>
        /// Formats the value for column map.
        /// </summary>
        /// <param name="value">The value.</param>
        /// <returns>System.String.</returns>
        public override string FormatValueForColumnMap(string value)
        {
            return FieldDefinition.FormatValueForColumnMap(value);
        }

        /// <summary>
        /// Gets the name of the property join.
        /// </summary>
        /// <param name="useDbField">if set to <c>true</c> [use database field].</param>
        /// <returns>System.String.</returns>
        public override string GetPropertyJoinName(bool useDbField = false)
        {
            var result = FieldDefinition.PropertyName;
            if (ParentObject is LookupJoin parentLookupJoin)
            {
                var test = this;
                if (FieldDefinition.TableDefinition == LookupDefinition.TableDefinition)
                {
                    if (!FieldDefinition.AllowRecursion)
                    {
                        return FieldDefinition.PropertyName;
                    }
                }

                if (useDbField)
                {
                    result = parentLookupJoin.JoinDefinition.GetPropertyJoinName(FieldDefinition.PropertyName, useDbField);
                    return result;
                }
                return parentLookupJoin.JoinDefinition.GetPropertyJoinName(FieldToDisplay.PropertyName);
            }

            return result;
        }

        /// <summary>
        /// Gets the navigation properties.
        /// </summary>
        /// <returns>List&lt;JoinInfo&gt;.</returns>
        public List<JoinInfo> GetNavigationProperties()
        {
            if (ParentObject != null)
            {
                return ParentObject.GetNavigationProperties();
            }

            return NavigationProperties;
        }

        /// <summary>
        /// Gets the database value.
        /// </summary>
        /// <typeparam name="TEntity">The type of the t entity.</typeparam>
        /// <param name="entity">The entity.</param>
        /// <returns>System.String.</returns>
        public override string GetDatabaseValue<TEntity>(TEntity entity)
        {
            var result = string.Empty;

            object propertyObject = null;
            if (NavigationProperties == null)
            {
                propertyObject = GetPropertyObject(entity);
            }
            else
            {
                propertyObject = GetPropertyObject(entity, NavigationProperties);
            }

            var formulaObject = FieldDefinition.FormulaObject;

            if (formulaObject != null)
            {
                if (propertyObject == null)
                {
                    if (!AllowNulls)
                    {
                        result = formulaObject.GetDatabaseValue(entity);
                    }
                }
                else
                {
                    result = formulaObject.GetDatabaseValue(propertyObject);
                }
            }
            else
            {
                if (HasNavProperties && propertyObject == null)
                {
                    return null;
                }
                DbDateTypes? dateType = null;
                if (FieldDefinition is DateFieldDefinition dateFieldDefinition)
                {
                    dateType = dateFieldDefinition.DateType;
                }
                if (propertyObject == null)
                {
                    if (!AllowNulls || !HasNavProperties)
                    {
                        result = GblMethods.GetPropertyValue(entity, FieldToDisplay.PropertyName, dateType);
                    }
                }
                else
                {
                    result = GblMethods.GetPropertyValue(propertyObject, FieldToDisplay.PropertyName, dateType);
                }
            }

            return result;
        }

        /// <summary>
        /// Gets the formatted value.
        /// </summary>
        /// <typeparam name="TEntity">The type of the t entity.</typeparam>
        /// <param name="entity">The entity.</param>
        /// <returns>System.String.</returns>
        public override string GetFormattedValue<TEntity>(TEntity entity)
        {
            var result = string.Empty;
            var value = GetDatabaseValue(entity);
            if (!value.IsNullOrEmpty())
            {
                result = FormatValue(value);
            }
            return result;
        }
    }
}