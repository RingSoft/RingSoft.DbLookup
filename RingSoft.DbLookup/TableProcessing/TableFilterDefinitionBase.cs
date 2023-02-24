using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using MySqlX.XDevAPI.Common;
using RingSoft.DataEntryControls.Engine;
using RingSoft.DbLookup.AdvancedFind;
using RingSoft.DbLookup.Lookup;
using RingSoft.DbLookup.ModelDefinition;
using RingSoft.DbLookup.ModelDefinition.FieldDefinitions;
using RingSoft.DbLookup.QueryBuilder;

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
        /// <value>
        /// The source.
        /// </value>
        public TableFilterDefinitionBase Source { get; }

        /// <summary>
        /// Initializes a new instance of the <see cref="TableFilterCopiedArgs"/> class.
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
        /// <value>
        /// The fixed filters.
        /// </value>
        public IReadOnlyList<FilterItemDefinition> FixedFilters => _fixedFilterDefinitions;

        /// <summary>
        /// Gets the user filters.
        /// </summary>
        /// <value>
        /// The user filters.
        /// </value>
        public IReadOnlyList<FilterItemDefinition> UserFilters => _userFilterDefinitions;

        /// <summary>
        /// Gets the joins.
        /// </summary>
        /// <value>
        /// The joins.
        /// </value>
        public IReadOnlyList<TableFieldJoinDefinition> Joins => _joinDefinitions;

        public TableDefinitionBase TableDefinition { get; set; }

        /// <summary>
        /// Occurs when this filter is copied from a source table filter definition.  Used by subscribers (like AutoFill) to synchronize filters.
        /// </summary>
        public event EventHandler<TableFilterCopiedArgs> FilterCopied;

        private readonly List<FilterItemDefinition> _fixedFilterDefinitions = new List<FilterItemDefinition>();
        private readonly List<FilterItemDefinition> _userFilterDefinitions = new List<FilterItemDefinition>();
        private readonly List<TableFieldJoinDefinition> _joinDefinitions = new List<TableFieldJoinDefinition>();

        internal TableFilterDefinitionBase(TableDefinitionBase tableDefinition)
        {
            TableDefinition = tableDefinition;
        }

        /// <summary>
        /// Clears the fixed filters.
        /// </summary>
        public void ClearFixedFilters()
        {
            _fixedFilterDefinitions.Clear();
        }

        public void ClearUserFilters()
        {
            _userFilterDefinitions.Clear();
        }

        internal void AddUserFilter(FilterItemDefinition filterItem)
        {
            _userFilterDefinitions.Add(filterItem);
        }

        internal void AddFixedFilter(FilterItemDefinition filterItem)
        {
            _fixedFilterDefinitions.Add(filterItem);
        }

        /// <summary>
        /// Clears this filter and copies the source filter data to this object.
        /// </summary>
        /// <param name="source">The source.</param>
        public void CopyFrom(TableFilterDefinitionBase source)
        {
            _joinDefinitions.Clear();
            CopyFilters(source.FixedFilters, _fixedFilterDefinitions);
            CopyFilters(source.UserFilters, _userFilterDefinitions);
            OnTableFilterCopied(new TableFilterCopiedArgs(source));
        }

        protected void OnTableFilterCopied(TableFilterCopiedArgs e)
        {
            FilterCopied?.Invoke(this, e);
        }

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

        public FieldFilterDefinition CreateFieldFilter(FieldDefinition fieldDefinition,
            Conditions condition,
            string value)
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

            var fieldFilter = new FieldFilterDefinition(this)
            {
                FieldDefinition = fieldDefinition,
                Condition = condition,
                Value = value,
                ReportDescription = fieldDefinition.Description
            };

            return fieldFilter;
        }

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

        private FieldFilterDefinition CreateAddFixedFilter(FieldDefinition fieldDefinition, Conditions condition,
            string value)
        {
            var fieldFilter = CreateFieldFilter(fieldDefinition, condition, value);
            _fixedFilterDefinitions.Add(fieldFilter);
            fieldFilter.IsFixed = true;
            return fieldFilter;
        }

        private FormulaFilterDefinition CreateAddFixedFilter(string description, Conditions? condition,
            string value, string formula, FieldDataTypes dataType)
        {
            var formulaFilter = CreateFormulaFilter(formula, dataType, condition, value);
            formulaFilter.Description = description;
            _fixedFilterDefinitions.Add(formulaFilter);
            formulaFilter.IsFixed = true;
            return formulaFilter;
        }

        internal FieldFilterDefinition AddFixedFilter(FieldDefinition fieldDefinition, Conditions condition,
            string value)
        {
            return CreateAddFixedFilter(fieldDefinition, condition, value);
        }

        public FieldFilterDefinition AddFixedFilter(StringFieldDefinition fieldDefinition, Conditions condition,
            string value)
        {
            return CreateAddFixedFilter(fieldDefinition, condition, value);
        }

        public FieldFilterDefinition AddFixedFilter(IntegerFieldDefinition fieldDefinition, Conditions condition,
            int value)
        {
            return CreateAddFixedFilter(fieldDefinition, condition, value.ToString());
        }

        public FieldFilterDefinition AddFixedFilter(DecimalFieldDefinition fieldDefinition, Conditions condition,
            double value)
        {
            return CreateAddFixedFilter(fieldDefinition, condition, value.ToString(CultureInfo.CurrentCulture));
        }

        public FieldFilterDefinition AddFixedFilter(DecimalFieldDefinition fieldDefinition, Conditions condition,
            decimal value)
        {
            return CreateAddFixedFilter(fieldDefinition, condition, value.ToString(CultureInfo.CurrentCulture));
        }

        public FieldFilterDefinition AddFixedFilter(DateFieldDefinition fieldDefinition, Conditions condition,
            DateTime value)
        {
            return CreateAddFixedFilter(fieldDefinition, condition, value.ToString(CultureInfo.CurrentCulture));
        }

        public FieldFilterDefinition AddFixedFilter(BoolFieldDefinition fieldDefinition, Conditions condition,
            bool value)
        {
            return CreateAddFixedFilter(fieldDefinition, condition, SelectQuery.BoolToString(value));
        }

        public FieldFilterDefinition AddFixedFieldFilter(FieldDefinition fieldDefinition, Conditions condition,
            string value)
        {
            return CreateAddFixedFilter(fieldDefinition, condition, value);
        }

        public FormulaFilterDefinition AddFixedFilter(string description, Conditions? condition,
            string value, string formula, FieldDataTypes dataType = FieldDataTypes.String)
        {
            return CreateAddFixedFilter(description, condition, value, formula, dataType);
        }

        public void AddJoin(TableFieldJoinDefinition foreignKeyDefinition)
        {
            if (!_joinDefinitions.Contains(foreignKeyDefinition))
                _joinDefinitions.Add(foreignKeyDefinition);
        }

        public FieldFilterDefinition AddUserFilter(FieldDefinition fieldDefinition, Conditions condition,
            string value, int index = -1)
        {
            var fieldFilter = CreateFieldFilter(fieldDefinition, condition, value);
            InternalAddUserFilter(index, fieldFilter);

            return fieldFilter;
        }

        private void InternalAddUserFilter(int index, FilterItemDefinition filter)
        {
            if (_userFilterDefinitions.Count < index)
            {
                index = -1;
            }
            if (index == -1)
            {
                _userFilterDefinitions.Add(filter);
            }
            else
            {
                _userFilterDefinitions.Insert(index, filter);
            }
        }

        public FormulaFilterDefinition AddUserFilter(string formula, Conditions condition, string value = "",
            string alias = "", FieldDataTypes dataType = FieldDataTypes.String, int index = -1)
        {
            var formulaFilter = CreateFormulaFilter(formula, dataType, condition, value, alias);
            InternalAddUserFilter(index, formulaFilter);
            return formulaFilter;
        }

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

        public void RemoveUserFilter(FilterItemDefinition filterItem)
        {
            _userFilterDefinitions.Remove(filterItem);
        }

        public void ReplaceUserFilter(FilterItemDefinition oldFilterItemDefinition,
            FilterItemDefinition newFilterItemDefinition)
        {
            var index = _userFilterDefinitions.IndexOf(oldFilterItemDefinition);
            _userFilterDefinitions.Remove(oldFilterItemDefinition);
            _userFilterDefinitions.Insert(index, newFilterItemDefinition);
        }

        public void RemoveFixedFilter(FilterItemDefinition filterItem)
        {
            _fixedFilterDefinitions.Remove(filterItem);
        }

    internal void ProcessQuery(SelectQuery query)
        {
            ProcessFieldJoins(query, Joins);
            ProcessFilters(query, FixedFilters);
            ProcessFilters(query, UserFilters);
        }

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

                if (fieldFilterDefinition.FormulaToSearch.IsNullOrEmpty())
                {
                    lastWhere = query.AddWhereItem(queryTable, fieldFilterDefinition.FieldToSearch.FieldName,
                        fieldFilterDefinition.Condition, fieldFilterDefinition.GetSearchValue(value), fieldFilterDefinition.FieldToSearch.ValueType,
                        dateType);
                }
                else
                {
                    lastWhere = query.AddWhereItemFormula(fieldFilterDefinition.FormulaToSearch,
                        fieldFilterDefinition.Condition, fieldFilterDefinition.GetSearchValue(value), ValueTypes.String);
                }

                lastWhere.IsCaseSensitive(fieldFilterDefinition.CaseSensitive);
                result.Add(lastWhere);
            }

            return result;
        }

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


        public void ProcessFilterWheres(List<WhereItem> wheres, ref WhereItem firstWhereItem, ref WhereItem lastWhereItem, FilterItemDefinition filterDefinition)
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
                _fixedFilterDefinitions.Add(newFilter);
            }
        }
    }
}
