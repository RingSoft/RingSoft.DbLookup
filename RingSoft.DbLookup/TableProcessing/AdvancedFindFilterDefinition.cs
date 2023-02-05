using RingSoft.DataEntryControls.Engine;
using RingSoft.DbLookup.AdvancedFind;
using RingSoft.DbLookup.Lookup;
using RingSoft.DbLookup.QueryBuilder;
using System.Collections.Generic;
using System.Linq;

namespace RingSoft.DbLookup.TableProcessing
{
    public class AdvancedFindFilterDefinition : FilterItemType<AdvancedFindFilterDefinition>
    {
        public override FilterItemTypes Type => FilterItemTypes.AdvancedFind;

        public override TreeViewType TreeViewType => TreeViewType.AdvancedFind;

        public int AdvancedFindId { get; set; }

        public LookupDefinitionBase LookupDefinition { get; private set; }

        
        internal AdvancedFindFilterDefinition(TableFilterDefinitionBase tableFilterDefinition) : base(tableFilterDefinition)
        {
            
        }

        internal override void CopyFrom(FilterItemDefinition source)
        {
            if (source is AdvancedFindFilterDefinition advancedFindFilterDefinition)
            {
                AdvancedFindId = advancedFindFilterDefinition.AdvancedFindId;
                LookupDefinition = advancedFindFilterDefinition.LookupDefinition;
                Path = advancedFindFilterDefinition.Path;
            }
            base.CopyFrom(source);
        }

        public override string GetReportText(LookupDefinitionBase lookupDefinition, bool printMode = false)
        {
            var result = string.Empty;
            var advancedFind = SystemGlobals.AdvancedFindDbProcessor.GetAdvancedFind(AdvancedFindId);
            if (advancedFind != null)
            {
                foreach (var advancedFindFilter in advancedFind.Filters)
                {
                    var filterReturn = LookupDefinition.LoadFromAdvFindFilter(advancedFindFilter, false);
                    result += filterReturn.GetReportText(lookupDefinition, printMode);
                    result.TrimRight("\r\n");
                }
            }
            return result;
        }

        public override bool LoadFromEntity(AdvancedFindFilter entity, LookupDefinitionBase lookupDefinition)
        {
            if (entity.Path.IsNullOrEmpty())
            {
                TableDescription = lookupDefinition.TableDefinition.Description;
            }
            else
            {
                var afItem = lookupDefinition.AdvancedFindTree.ProcessFoundTreeViewItem(entity.Path,
                    TreeViewType.AdvancedFind);
                TableDescription = afItem.Name;
            }
            AdvancedFindId = entity.AdvancedFindId;
            LookupDefinition = lookupDefinition;
            return base.LoadFromEntity(entity, lookupDefinition);
        }

        public override string LoadFromFilterReturn(AdvancedFilterReturn filterReturn, TreeViewItem treeViewItem)
        {
            return base.LoadFromFilterReturn(filterReturn, treeViewItem);
        }

        public override FilterItemDefinition GetNewFilterItemDefinition()
        {
            throw new System.NotImplementedException();
        }

        internal override string GetNewPath()
        {
            throw new System.NotImplementedException();
        }

        public List<WhereItem> ProcessAdvancedFind(SelectQuery query, ref WhereItem firstWhereItem
            , ref WhereItem lastWhereItem, bool fromAdvancedFind, AdvancedFindTree tree = null)
        {
            var test = this;
            var wheres = new List<WhereItem>();
            var advancedFind = SystemGlobals.AdvancedFindDbProcessor.GetAdvancedFind(AdvancedFindId);
            var filters = advancedFind.Filters.ToList();
            FilterItemDefinition lastFilter = null;
            {
                FilterItemDefinition advFindFilterReturn = null;
                foreach (var advancedFindFilter in filters)
                {

                    if (advancedFindFilter.SearchForAdvancedFindId != null)
                    {
                        var newAdvancedFindFilter = new AdvancedFindFilterDefinition(LookupDefinition.FilterDefinition);
                        newAdvancedFindFilter.LookupDefinition = LookupDefinition;
                        newAdvancedFindFilter.AdvancedFindId = advancedFindFilter.SearchForAdvancedFindId.Value;
                        newAdvancedFindFilter.TableFilterDefinition = TableFilterDefinition;
                        newAdvancedFindFilter.Path = Path;
                        var newWheres = newAdvancedFindFilter.ProcessAdvancedFind(query, ref firstWhereItem, ref lastWhereItem,
                                true,
                                tree);
                        wheres.AddRange(newWheres);
                        TableFilterDefinition.ProcessFilterWheres(newWheres, ref firstWhereItem, ref lastWhereItem, advancedFindFilter);
                    }
                    else
                    {
                        var foundTreeItem = LookupDefinition.AdvancedFindTree.ProcessFoundTreeViewItem(Path);
                        advFindFilterReturn = LookupDefinition.LoadFromAdvFindFilter(advancedFindFilter, false);
                        TableFilterDefinitionBase.ProcessFieldJoins(query, LookupDefinition.Joins);
                        wheres.AddRange(TableFilterDefinition.ProcessFilter(query, advFindFilterReturn,
                            ref lastWhereItem, ref firstWhereItem, tree));
                    }
                }
            }

            return wheres;
        }
    }
}
