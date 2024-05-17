// ***********************************************************************
// Assembly         : RingSoft.DbLookup
// Author           : petem
// Created          : 12-19-2022
//
// Last Modified By : petem
// Last Modified On : 07-11-2023
// ***********************************************************************
// <copyright file="TableFilterDefinitionBase.cs" company="Peter Ringering">
//     Copyright (c) 2023. All rights reserved.
// </copyright>
// <summary></summary>
// ***********************************************************************
using RingSoft.DataEntryControls.Engine;
using RingSoft.DbLookup.AdvancedFind;
using RingSoft.DbLookup.Lookup;
using RingSoft.DbLookup.ModelDefinition;
using RingSoft.DbLookup.ModelDefinition.FieldDefinitions;
using RingSoft.DbLookup.QueryBuilder;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Linq.Expressions;
using RingSoft.DbLookup.DataProcessor;

namespace RingSoft.DbLookup.TableProcessing
{
    /// <summary>
    /// Arguments sent with the FilterCopied event.
    /// </summary>
    public class TableFilterCopiedArgs
    {
        /// <summary>
        /// Gets the source.
        /// </summary>
        /// <value>The source.</value>
        public TableFilterDefinitionBase Source { get; }

        /// <summary>
        /// Initializes a new instance of the <see cref="TableFilterCopiedArgs" /> class.
        /// </summary>
        /// <param name="source">The source.</param>
        public TableFilterCopiedArgs(TableFilterDefinitionBase source)
        {
            Source = source;
        }
    }

    /// <summary>
    /// A table filter definition not based on an entity.
    /// </summary>
    public class TableFilterDefinitionBase
    {
        /// <summary>
        /// Gets the fixed filters.
        /// </summary>
        /// <value>The fixed filters.</value>
        public IReadOnlyList<FilterItemDefinition> FixedFilters => FixedBundle.Filters;

        /// <summary>
        /// Gets the user filters.
        /// </summary>
        /// <value>The user filters.</value>
        public IReadOnlyList<FilterItemDefinition> UserFilters => UserBundle.Filters;

        /// <summary>
        /// Gets the joins.
        /// </summary>
        /// <value>The joins.</value>
        public IReadOnlyList<TableFieldJoinDefinition> Joins => _joinDefinitions;

        /// <summary>
        /// Gets or sets the table definition.
        /// </summary>
        /// <value>The table definition.</value>
        public TableDefinitionBase TableDefinition { get; set; }

        /// <summary>
        /// Gets the fixed bundle.
        /// </summary>
        /// <value>The fixed bundle.</value>
        public FilterBundle FixedBundle { get; }
        /// <summary>
        /// Gets the user bundle.
        /// </summary>
        /// <value>The user bundle.</value>
        public FilterBundle UserBundle { get; }

        /// <summary>
        /// Occurs when this filter is copied from a source table filter definition.  Used by subscribers (like AutoFill) to synchronize filters.
        /// </summary>
        public event EventHandler<TableFilterCopiedArgs> FilterCopied;

        //private readonly List<FilterItemDefinition> _fixedFilterDefinitions = new List<FilterItemDefinition>();
        //private readonly List<FilterItemDefinition> _userFilterDefinitions = new List<FilterItemDefinition>();

        /// <summary>
        /// The join definitions
        /// </summary>
        private readonly List<TableFieldJoinDefinition> _joinDefinitions = new List<TableFieldJoinDefinition>();

        /// <summary>
        /// Initializes a new instance of the <see cref="TableFilterDefinitionBase"/> class.
        /// </summary>
        /// <param name="tableDefinition">The table definition.</param>
        internal TableFilterDefinitionBase(TableDefinitionBase tableDefinition)
        {
            TableDefinition = tableDefinition;
            FixedBundle = new FilterBundle(this);
            UserBundle = new FilterBundle(this);
        }

        /// <summary>
        /// Clears the fixed filters.
        /// </summary>
        public void ClearFixedFilters()
        {
            FixedBundle.ClearFilters();
        }

        /// <summary>
        /// Clears the user filters.
        /// </summary>
        public void ClearUserFilters()
        {
            UserBundle.ClearFilters();
        }

        /// <summary>
        /// Adds the user filter.
        /// </summary>
        /// <param name="filterItem">The filter item.</param>
        internal void AddUserFilter(FilterItemDefinition filterItem)
        {
            UserBundle.AddFilter(filterItem);
        }

        /// <summary>
        /// Adds the fixed filter.
        /// </summary>
        /// <param name="filterItem">The filter item.</param>
        internal void AddFixedFilter(FilterItemDefinition filterItem)
        {
            FixedBundle.AddFilter(filterItem);
        }

        /// <summary>
        /// Clears this filter and copies the source filter data to this object.
        /// </summary>
        /// <param name="source">The source.</param>
        public void CopyFrom(TableFilterDefinitionBase source)
        {
            _joinDefinitions.Clear();
            FixedBundle.CopyFilters(source.FixedBundle);
            UserBundle.CopyFilters(source.UserBundle);
            OnTableFilterCopied(new TableFilterCopiedArgs(source));
        }

        /// <summary>
        /// Called when [table filter copied].
        /// </summary>
        /// <param name="e">The e.</param>
        protected void OnTableFilterCopied(TableFilterCopiedArgs e)
        {
            FilterCopied?.Invoke(this, e);
        }

        /// <summary>
        /// Copies the filters.
        /// </summary>
        /// <param name="sourceFilters">The source filters.</param>
        /// <param name="destinationFilters">The destination filters.</param>
        /// <exception cref="System.ArgumentOutOfRangeException"></exception>
        private void CopyFilters(IReadOnlyList<FilterItemDefinition> sourceFilters,
            List<FilterItemDefinition> destinationFilters)
        {
            destinationFilters.Clear();
            foreach (var sourceFilter in sourceFilters)
            {
                FilterItemDefinition newFilterItem;
                switch (sourceFilter.Type)
                {
                    case FilterItemTypes.Field:
                        newFilterItem = new FieldFilterDefinition(this);
                        break;
                    case FilterItemTypes.Formula:
                        newFilterItem = new FormulaFilterDefinition(this);
                        break;
                    case FilterItemTypes.AdvancedFind:
                        newFilterItem = new AdvancedFindFilterDefinition(this);
                        break;
                    default:
                        throw new ArgumentOutOfRangeException();
                }

                newFilterItem.TableFilterDefinition = this;
                newFilterItem.CopyFrom(sourceFilter);
                destinationFilters.Add(newFilterItem);
            }
        }

        /// <summary>
        /// Creates the field filter.
        /// </summary>
        /// <param name="fieldDefinition">The field definition.</param>
        /// <param name="condition">The condition.</param>
        /// <param name="value">The value.</param>
        /// <returns>FieldFilterDefinition.</returns>
        public FieldFilterDefinition CreateFieldFilter(FieldDefinition fieldDefinition,
            Conditions condition,
            string value)
        {
            var fieldFilter = new FieldFilterDefinition(this)
            {
                FieldDefinition = fieldDefinition,
                Condition = condition,
                Value = value,
                ReportDescription = fieldDefinition.Description,
            };

            if (fieldDefinition is DateFieldDefinition dateFieldDefinition)
            {
                fieldFilter.DateType = dateFieldDefinition.DateType;
            }

            return fieldFilter;
        }

        /// <summary>
        /// Creates the formula filter.
        /// </summary>
        /// <param name="formula">The formula.</param>
        /// <param name="dataType">Type of the data.</param>
        /// <param name="condition">The condition.</param>
        /// <param name="value">The value.</param>
        /// <param name="alias">The alias.</param>
        /// <returns>FormulaFilterDefinition.</returns>
        public FormulaFilterDefinition CreateFormulaFilter(string formula, FieldDataTypes dataType, Conditions? condition,
            string value = "", string alias = "")
        {
            if (!alias.IsNullOrEmpty())
            {
                formula = formula.Replace("{Alias}", alias);
            }

            var formulaFilter = new FormulaFilterDefinition(this)
            {
                Formula = formula,
                Condition = condition,
                FilterValue = value,
                DataType = dataType,
                Alias = alias
            };

            return formulaFilter;
        }

        /// <summary>
        /// Creates the add fixed filter.
        /// </summary>
        /// <param name="fieldDefinition">The field definition.</param>
        /// <param name="condition">The condition.</param>
        /// <param name="value">The value.</param>
        /// <param name="index">The index.</param>
        /// <returns>FieldFilterDefinition.</returns>
        private FieldFilterDefinition CreateAddFixedFilter(FieldDefinition fieldDefinition, Conditions condition,
            string value, int index = -1)
        {
            if (value.IsNullOrEmpty())
            {

            }

            var fieldFilter = CreateFieldFilter(fieldDefinition, condition, value);
            FixedBundle.AddFilter(fieldFilter, index);
            fieldFilter.IsFixed = true;
            return fieldFilter;
        }

        /// <summary>
        /// Creates the add fixed filter.
        /// </summary>
        /// <param name="description">The description.</param>
        /// <param name="condition">The condition.</param>
        /// <param name="value">The value.</param>
        /// <param name="formula">The formula.</param>
        /// <param name="dataType">Type of the data.</param>
        /// <returns>FormulaFilterDefinition.</returns>
        private FormulaFilterDefinition CreateAddFixedFilter(string description, Conditions? condition,
            string value, string formula, FieldDataTypes dataType)
        {
            var formulaFilter = CreateFormulaFilter(formula, dataType, condition, value);
            formulaFilter.Description = description;
            FixedBundle.AddFilter(formulaFilter);
            formulaFilter.IsFixed = true;
            return formulaFilter;
        }

        /// <summary>
        /// Adds the fixed filter.
        /// </summary>
        /// <param name="fieldDefinition">The field definition.</param>
        /// <param name="condition">The condition.</param>
        /// <param name="value">The value.</param>
        /// <param name="index">The index.</param>
        /// <returns>FieldFilterDefinition.</returns>
        internal FieldFilterDefinition AddFixedFilter(FieldDefinition fieldDefinition, Conditions condition,
            string value, int index = -1)
        {
            return CreateAddFixedFilter(fieldDefinition, condition, value, index);
        }

        /// <summary>
        /// Adds the fixed filter.
        /// </summary>
        /// <param name="fieldDefinition">The field definition.</param>
        /// <param name="condition">The condition.</param>
        /// <param name="value">The value.</param>
        /// <returns>FieldFilterDefinition.</returns>
        public FieldFilterDefinition AddFixedFilter(StringFieldDefinition fieldDefinition, Conditions condition,
            string value)
        {
            return CreateAddFixedFilter(fieldDefinition, condition, value);
        }

        /// <summary>
        /// Adds the fixed filter.
        /// </summary>
        /// <param name="fieldDefinition">The field definition.</param>
        /// <param name="condition">The condition.</param>
        /// <param name="value">The value.</param>
        /// <returns>FieldFilterDefinition.</returns>
        public FieldFilterDefinition AddFixedFilter(IntegerFieldDefinition fieldDefinition, Conditions condition,
            int value)
        {
            return CreateAddFixedFilter(fieldDefinition, condition, value.ToString());
        }

        /// <summary>
        /// Adds the fixed filter.
        /// </summary>
        /// <param name="fieldDefinition">The field definition.</param>
        /// <param name="condition">The condition.</param>
        /// <param name="value">The value.</param>
        /// <returns>FieldFilterDefinition.</returns>
        public FieldFilterDefinition AddFixedFilter(DecimalFieldDefinition fieldDefinition, Conditions condition,
            double value)
        {
            return CreateAddFixedFilter(fieldDefinition, condition, value.ToString(CultureInfo.CurrentCulture));
        }


        /// <summary>
        /// Adds the fixed filter.
        /// </summary>
        /// <param name="fieldDefinition">The field definition.</param>
        /// <param name="condition">The condition.</param>
        /// <param name="value">The value.</param>
        /// <returns>FieldFilterDefinition.</returns>
        public FieldFilterDefinition AddFixedFilter(DateFieldDefinition fieldDefinition, Conditions condition,
            DateTime value)
        {
            if (fieldDefinition.ConvertToLocalTime || SystemGlobals.ConvertAllDatesToUniversalTime)
            {
                value = value.ToUniversalTime();
            }

            var filter = CreateAddFixedFilter(fieldDefinition, condition, value.ToString(CultureInfo.CurrentCulture));
            return filter;
        }

        /// <summary>
        /// Adds the fixed filter.
        /// </summary>
        /// <param name="fieldDefinition">The field definition.</param>
        /// <param name="condition">The condition.</param>
        /// <param name="value">if set to <c>true</c> [value].</param>
        /// <returns>FieldFilterDefinition.</returns>
        public FieldFilterDefinition AddFixedFilter(BoolFieldDefinition fieldDefinition, Conditions condition,
            bool value)
        {
            return CreateAddFixedFilter(fieldDefinition, condition, SelectQuery.BoolToString(value));
        }

        /// <summary>
        /// Adds the fixed field filter.
        /// </summary>
        /// <param name="fieldDefinition">The field definition.</param>
        /// <param name="condition">The condition.</param>
        /// <param name="value">The value.</param>
        /// <returns>FieldFilterDefinition.</returns>
        public FieldFilterDefinition AddFixedFieldFilter(FieldDefinition fieldDefinition, Conditions condition,
            string value)
        {
            return CreateAddFixedFilter(fieldDefinition, condition, value);
        }

        /// <summary>
        /// Adds the fixed filter.
        /// </summary>
        /// <param name="description">The description.</param>
        /// <param name="condition">The condition.</param>
        /// <param name="value">The value.</param>
        /// <param name="formula">The formula.</param>
        /// <param name="dataType">Type of the data.</param>
        /// <returns>FormulaFilterDefinition.</returns>
        public FormulaFilterDefinition AddFixedFilter(string description, Conditions? condition,
            string value, string formula, FieldDataTypes dataType = FieldDataTypes.String)
        {
            return CreateAddFixedFilter(description, condition, value, formula, dataType);
        }

        /// <summary>
        /// Adds the join.
        /// </summary>
        /// <param name="foreignKeyDefinition">The foreign key definition.</param>
        public void AddJoin(TableFieldJoinDefinition foreignKeyDefinition)
        {
            if (!_joinDefinitions.Contains(foreignKeyDefinition))
                _joinDefinitions.Add(foreignKeyDefinition);
        }

        /// <summary>
        /// Adds the user filter.
        /// </summary>
        /// <param name="fieldDefinition">The field definition.</param>
        /// <param name="condition">The condition.</param>
        /// <param name="value">The value.</param>
        /// <param name="index">The index.</param>
        /// <returns>FieldFilterDefinition.</returns>
        public FieldFilterDefinition AddUserFilter(FieldDefinition fieldDefinition, Conditions condition,
            string value, int index = -1)
        {
            if (fieldDefinition is DateFieldDefinition dateField)
            {
                if (dateField.ConvertToLocalTime)
                {
                    var date = value.ToDate();
                    if (date != null)
                    {
                        value = date.Value.ToUniversalTime().FormatDateValue(dateField.DateType);
                    }
                }
            }

            var fieldFilter = CreateFieldFilter(fieldDefinition, condition, value);
            fieldFilter.SetFieldToDisplay(condition);
            InternalAddUserFilter(index, fieldFilter);

            return fieldFilter;
        }

        /// <summary>
        /// Internals the add user filter.
        /// </summary>
        /// <param name="index">The index.</param>
        /// <param name="filter">The filter.</param>
        private void InternalAddUserFilter(int index, FilterItemDefinition filter)
        {
            UserBundle.InternalAddFilter(index, filter);
        }

        /// <summary>
        /// Adds the user filter.
        /// </summary>
        /// <param name="formula">The formula.</param>
        /// <param name="condition">The condition.</param>
        /// <param name="value">The value.</param>
        /// <param name="alias">The alias.</param>
        /// <param name="dataType">Type of the data.</param>
        /// <param name="index">The index.</param>
        /// <returns>FormulaFilterDefinition.</returns>
        public FormulaFilterDefinition AddUserFilter(string formula, Conditions condition, string value = "",
            string alias = "", FieldDataTypes dataType = FieldDataTypes.String, int index = -1)
        {
            var formulaFilter = CreateFormulaFilter(formula, dataType, condition, value, alias);
            InternalAddUserFilter(index, formulaFilter);
            return formulaFilter;
        }

        /// <summary>
        /// Adds the user filter.
        /// </summary>
        /// <param name="advancedFindId">The advanced find identifier.</param>
        /// <param name="lookupDefinition">The lookup definition.</param>
        /// <param name="path">The path.</param>
        /// <param name="addToUsersFilters">if set to <c>true</c> [add to users filters].</param>
        /// <param name="index">The index.</param>
        /// <returns>AdvancedFindFilterDefinition.</returns>
        public AdvancedFindFilterDefinition AddUserFilter(int advancedFindId, LookupDefinitionBase lookupDefinition,
            string path, bool addToUsersFilters = true, int index = -1)
        {
            var advancedFindFilter = new AdvancedFindFilterDefinition(this)
            {
                AdvancedFindId = advancedFindId,
                Path = path
            };
            advancedFindFilter.TableFilterDefinition = this;
            advancedFindFilter.LookupDefinition = lookupDefinition;
            if (addToUsersFilters)
            {
                InternalAddUserFilter(index, advancedFindFilter);
            }

            return advancedFindFilter;
        }

        /// <summary>
        /// Removes the user filter.
        /// </summary>
        /// <param name="filterItem">The filter item.</param>
        public void RemoveUserFilter(FilterItemDefinition filterItem)
        {
            UserBundle.InternalRemoveFilter(filterItem);
        }

        /// <summary>
        /// Removes the fixed filter.
        /// </summary>
        /// <param name="filterItem">The filter item.</param>
        public void RemoveFixedFilter(FilterItemDefinition filterItem)
        {
            FixedBundle.InternalRemoveFilter(filterItem);
        }

        /// <summary>
        /// Processes the query.
        /// </summary>
        /// <param name="query">The query.</param>
        internal void ProcessQuery(SelectQuery query)
        {
            ProcessFieldJoins(query, Joins);
            ProcessFilters(query, FixedFilters);
            ProcessFilters(query, UserFilters);
        }

        /// <summary>
        /// Processes the filters.
        /// </summary>
        /// <param name="query">The query.</param>
        /// <param name="filters">The filters.</param>
        private void ProcessFilters(SelectQuery query, IReadOnlyList<FilterItemDefinition> filters)
        {
            WhereItem firstWhere = null, lastWhere = null;

            var newfilters = filters.ToList();
            var wheres = new List<WhereItem>();
            foreach (var filterDefinition in newfilters)
            {
                var newWheres = ProcessFilter(query, filterDefinition, ref lastWhere, ref firstWhere);
                //ProcessFilterWheres(newWheres, ref firstWhere, ref lastWhere, filterDefinition);

                wheres.AddRange(newWheres);
                
                //if (whereItem != null)
                //{
                //    wheres.Add(whereItem);
                //}
                //if (firstWhere == null)
                //    firstWhere = lastWhere;

                //if (lastWhere != null)
                //{
                //    lastWhere.SetEndLogic(filterDefinition.EndLogic)
                //        .SetLeftParenthesesCount(filterDefinition.LeftParenthesesCount)
                //        .SetRightParenthesesCount(filterDefinition.RightParenthesesCount);

                //}
                if (wheres.Count > 0)
                {
                    wheres[0].LeftParenthesesCount++;
                    wheres[wheres.Count - 1].RightParenthesesCount ++;
                }
            }

            //if (wheres.Count >= 1) wheres[0].SetLeftParenthesesCount(wheres[0].LeftParenthesesCount + 1);
            //ProcessFilterWheres(wheres, ref firstWhere, ref lastWhere);
            //if (firstWhere != null)
            //{
            //    firstWhere.SetLeftParenthesesCount(firstWhere.LeftParenthesesCount + 1);
            //    if (lastWhere != null)
            //    {
            //        lastWhere.SetRightParenthesesCount(lastWhere.RightParenthesesCount + 1);
            //        lastWhere.SetEndLogic(EndLogics.And);
            //    }
            //}
        }

        /// <summary>
        /// Processes the filter.
        /// </summary>
        /// <param name="query">The query.</param>
        /// <param name="filterDefinition">The filter definition.</param>
        /// <param name="lastWhere">The last where.</param>
        /// <param name="firstWhere">The first where.</param>
        /// <param name="advancedFindTree">The advanced find tree.</param>
        /// <returns>List&lt;WhereItem&gt;.</returns>
        /// <exception cref="System.ArgumentOutOfRangeException"></exception>
        public List<WhereItem> ProcessFilter(SelectQuery query, FilterItemDefinition filterDefinition, ref WhereItem lastWhere,
            ref WhereItem firstWhere,
            AdvancedFindTree advancedFindTree = null)
        {
            var result = new List<WhereItem>();
            switch (filterDefinition.Type)
            {
                case FilterItemTypes.Field:
                    var fieldFilterDefinition = (FieldFilterDefinition) filterDefinition;
                    result.AddRange(ProcessFieldFilter(query, fieldFilterDefinition));
                    break;
                case FilterItemTypes.Formula:
                    var formulaFilter = (FormulaFilterDefinition) filterDefinition;

                    ValueTypes valueType = ValueTypes.String;
                    switch (formulaFilter.DataType)
                    {
                        case FieldDataTypes.Integer:
                        case FieldDataTypes.Decimal:
                            valueType = ValueTypes.Numeric;
                            break;
                        case FieldDataTypes.DateTime:
                            valueType = ValueTypes.DateTime;
                            break;
                        case FieldDataTypes.Bool:
                            valueType = ValueTypes.Bool;
                            break;
                    }

                    var formula = formulaFilter.Formula.Replace("{Alias}", formulaFilter.Alias);
                    var addFilter = true;
                    switch (formulaFilter.Condition)
                    {
                        case Conditions.EqualsNull:
                        case Conditions.NotEqualsNull:
                            break;
                        default:
                            addFilter = !formulaFilter.FilterValue.IsNullOrEmpty();
                            break;
                    }
                    if (formulaFilter.Condition != null && addFilter)
                    {
                        lastWhere = query.AddWhereItemFormula(formula, (Conditions) formulaFilter.Condition,
                            formulaFilter.GetSearchValue(formulaFilter.FilterValue), valueType
                            , formulaFilter.DateType);
                    }
                    else
                    {
                        lastWhere = query.AddWhereItemFormula(formula);
                    }
                    result.Add(lastWhere);
                    break;
                case FilterItemTypes.AdvancedFind:
                    var advancedFindFilterDefinition = (AdvancedFindFilterDefinition) filterDefinition;
                    var wheres = advancedFindFilterDefinition.ProcessAdvancedFind(query, ref firstWhere, ref lastWhere, false, advancedFindTree);
                    result.AddRange(wheres);
                    if (result.Count > 0)
                    {
                        result[0].SetLeftParenthesesCount(wheres[0].LeftParenthesesCount + 1);
                        result[result.Count - 1]
                            .SetRightParenthesesCount(result[result.Count - 1].RightParenthesesCount + 1);
                    }

                    break;
                default:
                    throw new ArgumentOutOfRangeException();
                
            }

            ProcessFilterWheres(result, ref firstWhere, ref lastWhere, filterDefinition);
 
            return result;
        }

        /// <summary>
        /// Processes the field filter.
        /// </summary>
        /// <param name="query">The query.</param>
        /// <param name="fieldFilterDefinition">The field filter definition.</param>
        /// <returns>List&lt;WhereItem&gt;.</returns>
        private List<WhereItem> ProcessFieldFilter(SelectQuery query, FieldFilterDefinition fieldFilterDefinition)
        {
            var value = fieldFilterDefinition.Value;
            var queryTable = GetQueryTableForFieldFilter(query, fieldFilterDefinition);

            var dateType = DbDateTypes.DateOnly;
            if (fieldFilterDefinition.FieldDefinition != null && fieldFilterDefinition.FieldDefinition.FieldDataType == FieldDataTypes.DateTime)
            {
                var dateField = fieldFilterDefinition.FieldDefinition as DateFieldDefinition;
                if (dateField != null)
                    dateType = dateField.DateType;
            }

            WhereItem lastWhere = null;
            var result = new List<WhereItem>();
            switch (fieldFilterDefinition.Condition)
            {
                case Conditions.EqualsNull:
                case Conditions.NotEqualsNull:
                    if (fieldFilterDefinition.ParentField != null)
                    {
                        var primaryKeyField = fieldFilterDefinition.ParentField;
                        queryTable = GetQueryTableForFieldFilter(query, primaryKeyField);
                        if (primaryKeyField.ParentJoinForeignKeyDefinition != null)
                        {
                            result.Add(query.AddWhereItemCheckNull(queryTable, primaryKeyField.ParentJoinForeignKeyDefinition.FieldJoins[0].PrimaryField.FieldName,
                                fieldFilterDefinition.Condition, false));
                        }
                        else
                        {
                            result.Add(query.AddWhereItemCheckNull(queryTable, primaryKeyField.FieldName,
                                fieldFilterDefinition.Condition, false));
                        }
                    }
                    break;
            }

            if (lastWhere == null)
            {
                queryTable = GetQueryTableForFieldFilter(query, fieldFilterDefinition);

                //if (fieldFilterDefinition.FormulaToSearch.IsNullOrEmpty())
                //{
                //    if (fieldFilterDefinition.FieldToSearch != null)
                //    {
                //        lastWhere = query.AddWhereItem(queryTable, fieldFilterDefinition.FieldToSearch.FieldName,
                //            fieldFilterDefinition.Condition, fieldFilterDefinition.GetSearchValue(value),
                //            fieldFilterDefinition.FieldToSearch.ValueType,
                //            dateType);
                //    }
                //}
                //else
                //{
                //    lastWhere = query.AddWhereItemFormula(fieldFilterDefinition.FormulaToSearch,
                //        fieldFilterDefinition.Condition, fieldFilterDefinition.GetSearchValue(value), ValueTypes.String);
                //}

                if (lastWhere != null)
                {
                    lastWhere.IsCaseSensitive(fieldFilterDefinition.CaseSensitive);
                    result.Add(lastWhere);
                }
            }

            return result;
        }

        /// <summary>
        /// Processes the field joins.
        /// </summary>
        /// <param name="query">The query.</param>
        /// <param name="joins">The joins.</param>
        internal static void ProcessFieldJoins(SelectQuery query, IReadOnlyList<TableFieldJoinDefinition> joins)
        {
            foreach (var tableFieldJoinDefinition in joins)
            {
                var joinType = JoinTypes.InnerJoin;
                if (tableFieldJoinDefinition.ForeignKeyDefinition.FieldJoins[0].ForeignField.AllowNulls)
                    joinType = JoinTypes.LeftOuterJoin;

                tableFieldJoinDefinition.JoinType = joinType;
                QueryTable foreignTable =
                    query.JoinTables.FirstOrDefault(f => f.Alias == tableFieldJoinDefinition.ParentAlias);
                if (foreignTable == null)
                {
                    foreignTable = query.BaseTable;
                }
                else
                {
                    var parentFilter = joins.FirstOrDefault(p => 
                        p.Alias == tableFieldJoinDefinition.ParentAlias);
                    if (parentFilter != null)
                    {
                        if (joinType == JoinTypes.InnerJoin)
                        {
                            joinType = parentFilter.JoinType;
                        }
                        tableFieldJoinDefinition .JoinType = joinType;
                    }
                }

                if (query.JoinTables.All(a => a.Alias != tableFieldJoinDefinition.Alias))
                {
                    var joinTable = query.AddPrimaryJoinTable(joinType, foreignTable,
                        tableFieldJoinDefinition.ForeignKeyDefinition.PrimaryTable.TableName,
                        tableFieldJoinDefinition.Alias);

                    foreach (var foreignKeyFieldJoin in tableFieldJoinDefinition.ForeignKeyDefinition.FieldJoins)
                    {
                        joinTable.AddJoinField(foreignKeyFieldJoin.PrimaryField.FieldName,
                            foreignKeyFieldJoin.ForeignField.FieldName);
                    }
                }
            }
        }

        /// <summary>
        /// Gets the query table for field filter.
        /// </summary>
        /// <param name="query">The query.</param>
        /// <param name="fieldFilterDefinition">The field filter definition.</param>
        /// <returns>QueryTable.</returns>
        internal QueryTable GetQueryTableForFieldFilter(SelectQuery query, FilterItemDefinition fieldFilterDefinition)
        {
            if (fieldFilterDefinition.JoinDefinition != null)
            {
                QueryTable foreignTable =
                    query.JoinTables.FirstOrDefault(f => f.Alias == fieldFilterDefinition.JoinDefinition.Alias);
                if (foreignTable != null)
                    return foreignTable;
            }

            return query.BaseTable;
        }

        /// <summary>
        /// Gets the query table for field filter.
        /// </summary>
        /// <param name="query">The query.</param>
        /// <param name="fieldDefinition">The field definition.</param>
        /// <returns>QueryTable.</returns>
        internal QueryTable GetQueryTableForFieldFilter(SelectQuery query, FieldDefinition fieldDefinition)
        {
            if (fieldDefinition.TableDefinition.TableName != query.BaseTable.Name)
            {
                if (fieldDefinition.ParentJoinForeignKeyDefinition != null)
                {
                    var foreignTable = query.JoinTables.FirstOrDefault(p =>
                        p.Alias == fieldDefinition.ParentJoinForeignKeyDefinition.Alias);
                    return foreignTable;
                }
            }

            return query.BaseTable;
        }


        /// <summary>
        /// Processes the filter wheres.
        /// </summary>
        /// <param name="wheres">The wheres.</param>
        /// <param name="firstWhereItem">The first where item.</param>
        /// <param name="lastWhereItem">The last where item.</param>
        /// <param name="filterDefinition">The filter definition.</param>
        public void ProcessFilterWheres(List<WhereItem> wheres
            , ref WhereItem firstWhereItem, ref WhereItem lastWhereItem, FilterItemDefinition filterDefinition)
        {
            if (wheres.Count > 0)
            {
                wheres[0].SetLeftParenthesesCount(
                    wheres[0].LeftParenthesesCount + filterDefinition.LeftParenthesesCount);

                wheres[wheres.Count - 1].SetRightParenthesesCount(
                    wheres[wheres.Count - 1].RightParenthesesCount + filterDefinition.RightParenthesesCount);

                wheres[wheres.Count - 1].EndLogic = filterDefinition.EndLogic;
            }

        }

        /// <summary>
        /// Processes the filter wheres.
        /// </summary>
        /// <param name="wheres">The wheres.</param>
        /// <param name="firstWhereItem">The first where item.</param>
        /// <param name="lastWhereItem">The last where item.</param>
        /// <param name="advancedFindFilter">The advanced find filter.</param>
        public void ProcessFilterWheres(List<WhereItem> wheres, ref WhereItem firstWhereItem, ref WhereItem lastWhereItem, AdvancedFindFilter advancedFindFilter)
        {
            if (wheres.Count > 0)
            {
                wheres[0].SetLeftParenthesesCount(
                    wheres[0].LeftParenthesesCount + advancedFindFilter.LeftParentheses);

                wheres[wheres.Count - 1].SetRightParenthesesCount(
                    wheres[wheres.Count - 1].RightParenthesesCount + advancedFindFilter.RightParentheses);

                wheres[wheres.Count - 1].EndLogic = (EndLogics)advancedFindFilter.EndLogic;
            }

        }

        /// <summary>
        /// Loads the fixed from lookup.
        /// </summary>
        /// <param name="filterDefinition">The filter definition.</param>
        /// <exception cref="System.ArgumentOutOfRangeException"></exception>
        public void LoadFixedFromLookup(TableFilterDefinitionBase filterDefinition)
        {
            foreach (var filterDefinitionFixedFilter in filterDefinition.FixedFilters)
            {
                FilterItemDefinition newFilter = null;
                switch (filterDefinitionFixedFilter.Type)
                {
                    case FilterItemTypes.Field:
                        newFilter = new FieldFilterDefinition(this);
                        break;
                    case FilterItemTypes.Formula:
                        newFilter = new FormulaFilterDefinition(this);
                        break;
                    case FilterItemTypes.AdvancedFind:
                        newFilter = new AdvancedFindFilterDefinition(this);
                        break;
                    default:
                        throw new ArgumentOutOfRangeException();
                }
                newFilter.CopyFrom(filterDefinitionFixedFilter);
                FixedBundle.AddFilter(newFilter);
            }
        }

        /// <summary>
        /// Gets the where expresssion.
        /// </summary>
        /// <typeparam name="TEntity">The type of the t entity.</typeparam>
        /// <param name="param">The parameter.</param>
        /// <returns>Expression.</returns>
        public Expression GetWhereExpresssion<TEntity>(ParameterExpression param)
        {
            Expression result = null;

            Expression fixedFilter = null;
            Expression userFilter = null;

            try
            {
                fixedFilter = FixedBundle.GetMauiFilter<TEntity>(param);
                userFilter = UserBundle.GetMauiFilter<TEntity>(param);

                if (fixedFilter != null && userFilter != null)
                {
                    result = FilterItemDefinition.AppendExpression(fixedFilter, userFilter, EndLogics.And);
                }
                else if (fixedFilter != null)
                {
                    result = fixedFilter;
                }
                else
                {
                    result = userFilter;
                }
            }
            catch (Exception e)
            {
                DbDataProcessor.DisplayDataException(e, "Filter Operation");

            }
            return result;
        }
    }
}
