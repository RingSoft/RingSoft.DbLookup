using System;
using System.Collections.Generic;
using System.Linq;
using RingSoft.DataEntryControls.Engine;
using RingSoft.DbLookup.AdvancedFind;
using RingSoft.DbLookup.DataProcessor;
using RingSoft.DbLookup.Lookup;
using RingSoft.DbLookup.ModelDefinition.FieldDefinitions;
using RingSoft.DbLookup.QueryBuilder;

namespace RingSoft.DbLookup.TableProcessing
{
    public class AdvancedFindFilterDefinition : FilterItemType<AdvancedFindFilterDefinition>
    {
        public override FilterItemTypes Type => FilterItemTypes.AdvancedFind;

        public int AdvancedFindId { get; set; }

        public LookupDefinitionBase LookupDefinition { get; private set; }

        //private List<AdvancedFindFilter> _filters;

        public AdvancedFindFilterDefinition(LookupDefinitionBase lookupDefinition)
        {
            LookupDefinition = lookupDefinition;
        }

        internal AdvancedFindFilterDefinition()
        {
            
        }

        internal override void CopyFrom(FilterItemDefinition source)
        {
            if (source is AdvancedFindFilterDefinition advancedFindFilterDefinition)
            {
                AdvancedFindId = advancedFindFilterDefinition.AdvancedFindId;
                LookupDefinition = advancedFindFilterDefinition.LookupDefinition;
            }
            base.CopyFrom(source);
        }

        public void ProcessAdvancedFind(SelectQuery query, ref WhereItem firstWhereItem
            , ref WhereItem lastWhereItem, bool fromAdvancedFind, AdvancedFindTree tree = null)
        {
            var wheres = new List<WhereItem>();
            var advancedFind = SystemGlobals.AdvancedFindDbProcessor.GetAdvancedFind(AdvancedFindId);
             var filters = advancedFind.Filters.ToList();
            if (tree == null)
            {
                tree = new AdvancedFindTree(LookupDefinition);
                tree.LoadTree(LookupDefinition.TableDefinition.TableName);
            }
            WhereItem lastAdvancedWhere = null;
            foreach (var advancedFindFilter in filters)
            {
                if (advancedFindFilter.Formula.IsNullOrEmpty())
                {
                    if (advancedFindFilter.SearchForAdvancedFindId > 0)
                    {
                        var advancedFindFilterDefinition = new AdvancedFindFilterDefinition(LookupDefinition);
                        advancedFindFilterDefinition.AdvancedFindId = advancedFindFilter.SearchForAdvancedFindId.Value;
                        advancedFindFilterDefinition.TableFilterDefinition = TableFilterDefinition;
                        advancedFindFilterDefinition.ProcessAdvancedFind(query, ref firstWhereItem, ref lastWhereItem,
                            true, tree);
                        if (lastAdvancedWhere != null)
                            lastAdvancedWhere.EndLogic = advancedFindFilterDefinition.EndLogic;

                        //ProcessAdvancedFind(query, ref firstWhereItem, ref lastWhereItem, tree);
                    }
                    else if (fromAdvancedFind)
                    {
                        var tableDefinition = LookupDefinition.TableDefinition;
                        tableDefinition =
                            tableDefinition.Context.TableDefinitions.FirstOrDefault(p =>
                                p.EntityName == advancedFindFilter.TableName);

                        if (tableDefinition != null)
                        {
                            var fieldDefinition =
                                tableDefinition.FieldDefinitions.FirstOrDefault(p =>
                                    p.PropertyName == advancedFindFilter.FieldName);

                            if (fieldDefinition != null)
                            {
                                var filterDefinition = TableFilterDefinition.CreateFieldFilter(fieldDefinition,
                                    (Conditions)advancedFindFilter.Operand, advancedFindFilter.SearchForValue);

                                ProcessFieldDefinition(query, tree, fieldDefinition, filterDefinition);

                                var queryTable =
                                    TableFilterDefinition.GetQueryTableForFieldFilter(query, filterDefinition);

                                var dateType = DbDateTypes.DateOnly;
                                if (fieldDefinition is DateFieldDefinition dateField)
                                    dateType = dateField.DateType;
                                var whereItem = query.AddWhereItem(queryTable, fieldDefinition.FieldName,
                                    (Conditions)advancedFindFilter.Operand, advancedFindFilter.SearchForValue,
                                    fieldDefinition.ValueType, dateType);
                                ProcessWhereItem(whereItem, ref lastAdvancedWhere, advancedFindFilter);
                                wheres.Add(whereItem);
                            }
                        }
                    }
                }
                else if (fromAdvancedFind)
                {
                    FormulaFilterDefinition formulaFilter = null;
                    QueryTable queryTable = null;
                    if (!advancedFindFilter.PrimaryTableName.IsNullOrEmpty() &&
                        !advancedFindFilter.PrimaryFieldName.IsNullOrEmpty())
                    {
                        var tableDefinition = LookupDefinition.TableDefinition;
                        tableDefinition =
                            tableDefinition.Context.TableDefinitions.FirstOrDefault(p =>
                                p.TableName == advancedFindFilter.PrimaryTableName);

                        if (tableDefinition != null)
                        {
                            var fieldDefinition =
                                tableDefinition.FieldDefinitions.FirstOrDefault(p =>
                                    p.FieldName == advancedFindFilter.PrimaryFieldName);

                            if (fieldDefinition != null)
                            {
                                formulaFilter = TableFilterDefinition.CreateFormulaFilter(
                                    advancedFindFilter.Formula, (FieldDataTypes) advancedFindFilter.FormulaDataType,
                                    (Conditions) advancedFindFilter.Operand, advancedFindFilter.SearchForValue, "");

                                ProcessFieldDefinition(query, tree, fieldDefinition, formulaFilter);
                                queryTable =
                                    TableFilterDefinition.GetQueryTableForFieldFilter(query, formulaFilter);
                            }
                        }
                    }
                    else
                    {
                        formulaFilter = TableFilterDefinition.CreateFormulaFilter(
                            advancedFindFilter.Formula, (FieldDataTypes)advancedFindFilter.FormulaDataType,
                            (Conditions)advancedFindFilter.Operand, advancedFindFilter.SearchForValue, "");

                        queryTable = query.BaseTable;
                    }

                    var valueType = formulaFilter.DataType.ConvertFieldTypeIntoValueType();
                    var formula = formulaFilter.Formula.Replace("{Alias}", queryTable?.Alias);
                    var whereItem = query.AddWhereItemFormula(formula,
                        formulaFilter.Condition.GetValueOrDefault(), formulaFilter.FilterValue,
                        valueType);
                    whereItem.Table = queryTable;
                    ProcessWhereItem(whereItem, ref lastAdvancedWhere, advancedFindFilter);
                    //lastWhereItem = whereItem;
                    //ProcessWhereItem(ref firstWhereItem, out lastWhereItem, lastWhereItem, advancedFindFilter);
                    wheres.Add(whereItem);
                }
            }
            ProcessFilterWheres(wheres, firstWhereItem, lastWhereItem);
            if (lastAdvancedWhere != null) lastAdvancedWhere.EndLogic = EndLogic;
            //DbDataProcessor.ShowSqlStatementWindow();
        }

        private void ProcessWhereItem(WhereItem whereItem, ref WhereItem lastWhereItem,
            AdvancedFindFilter advancedFindFilter)
        {
            whereItem.EndLogic = (EndLogics) advancedFindFilter.EndLogic;
            whereItem.LeftParenthesesCount = advancedFindFilter.LeftParentheses;
            whereItem.RightParenthesesCount = advancedFindFilter.RightParentheses;
            lastWhereItem = whereItem;
            //if (firstWhereItem == null)
            //    firstWhereItem = lastWhereItem;

        }

        private void ProcessFilterWheres(List<WhereItem> wheres, WhereItem firstWhereItem, WhereItem lastWhereItem)
        {
            if (wheres.Count >= 2)
            {
                if (wheres[0] != firstWhereItem)
                    wheres[0].LeftParenthesesCount++;
                if (wheres[1] != lastWhereItem)
                    wheres[wheres.Count - 1].RightParenthesesCount++;
            }

        }

        private static void ProcessFieldDefinition(SelectQuery query, AdvancedFindTree tree, FieldDefinition fieldDefinition,
            FilterItemDefinition filterDefinition)
        {
            var foundItem = tree.FindFieldInTree(tree.TreeRoot, fieldDefinition);
            if (foundItem != null)
            {
                var includeResult = tree.MakeIncludes(foundItem, "", false);
                if (includeResult != null)
                {
                    filterDefinition.JoinDefinition = includeResult.LookupJoin?.JoinDefinition;
                }

                var listJoins = new List<TableFieldJoinDefinition>();
                if (filterDefinition.JoinDefinition != null)
                {
                    listJoins.Add(includeResult.LookupJoin.JoinDefinition);
                    TableFilterDefinitionBase.ProcessFieldJoins(query, listJoins);
                }
            }
        }
    }
}
