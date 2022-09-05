using System.Collections.Generic;
using System.Linq;
using RingSoft.DataEntryControls.Engine;
using RingSoft.DbLookup.AdvancedFind;
using RingSoft.DbLookup.DataProcessor;
using RingSoft.DbLookup.Lookup;
using RingSoft.DbLookup.QueryBuilder;

namespace RingSoft.DbLookup.TableProcessing
{
    public class AdvancedFindFilterDefinition : FilterItemType<AdvancedFindFilterDefinition>
    {
        public override FilterItemTypes Type => FilterItemTypes.AdvancedFind;

        public int AdvancedFindId { get; set; }

        public LookupDefinitionBase LookupDefinition { get; private set; }

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
            , ref WhereItem lastWhereItem, AdvancedFindTree tree = null)
        {
            var advancedFind = SystemGlobals.AdvancedFindDbProcessor.GetAdvancedFind(AdvancedFindId);
            if (tree == null)
            {
                tree = new AdvancedFindTree(LookupDefinition);
                tree.LoadTree(LookupDefinition.TableDefinition.TableName);
            }
            foreach (var advancedFindFilter in advancedFind.Filters)
            {
                if (advancedFindFilter.Formula.IsNullOrEmpty())
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
                            var foundItem = tree.FindFieldInTree(tree.TreeRoot, fieldDefinition);
                            if (foundItem != null)
                            {
                                var includeResult = tree.MakeIncludes(foundItem, "", false);
                                if (includeResult != null)
                                {
                                    filterDefinition.JoinDefinition = includeResult.LookupJoin.JoinDefinition;
                                }

                                var listJoins = new List<TableFieldJoinDefinition>();
                                if (filterDefinition.JoinDefinition != null)
                                {
                                    listJoins.Add(includeResult.LookupJoin.JoinDefinition);
                                    TableFilterDefinitionBase.ProcessFieldJoins(query, listJoins);
                                }
                            }
                            var queryTable =TableFilterDefinition.GetQueryTableForFieldFilter(query, filterDefinition);
                            var whereItem = query.AddWhereItem(queryTable, fieldDefinition.FieldName,
                                (Conditions) advancedFindFilter.Operand, advancedFindFilter.SearchForValue);
                            whereItem.EndLogic = (EndLogics) advancedFindFilter.EndLogic;
                            whereItem.LeftParenthesesCount = advancedFindFilter.LeftParentheses;
                            whereItem.RightParenthesesCount = advancedFindFilter.RightParentheses;
                            lastWhereItem = whereItem;
                            if (firstWhereItem == null)
                                firstWhereItem = lastWhereItem;

                        }
                    }
                }
            }
        }
    }
}
