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

        public int AdvancedFindId { get; set; }

        public LookupDefinitionBase LookupDefinition { get; private set; }

        public string Path { get; internal set; }

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
                Path = advancedFindFilterDefinition.Path;
            }
            base.CopyFrom(source);
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
                LookupFilterReturn advFindFilterReturn = null;
                foreach (var advancedFindFilter in filters)
                {

                    if (advancedFindFilter.SearchForAdvancedFindId != null)
                    {
                        var newAdvancedFindFilter = new AdvancedFindFilterDefinition(LookupDefinition);
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
                        advFindFilterReturn = LookupDefinition.LoadFromAdvFindFilter(advancedFindFilter, false, foundTreeItem);
                        TableFilterDefinitionBase.ProcessFieldJoins(query, LookupDefinition.Joins);
                        wheres.AddRange(TableFilterDefinition.ProcessFilter(query, advFindFilterReturn.FilterItemDefinition,
                            ref lastWhereItem, ref firstWhereItem, tree));
                    }
                }
            }

            return wheres;
        }
    }
}
