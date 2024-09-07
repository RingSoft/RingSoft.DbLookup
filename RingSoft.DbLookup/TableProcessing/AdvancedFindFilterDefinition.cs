// ***********************************************************************
// Assembly         : RingSoft.DbLookup
// Author           : petem
// Created          : 12-19-2022
//
// Last Modified By : petem
// Last Modified On : 12-20-2023
// ***********************************************************************
// <copyright file="AdvancedFindFilterDefinition.cs" company="Peter Ringering">
//     Copyright (c) 2023. All rights reserved.
// </copyright>
// <summary></summary>
// ***********************************************************************
using RingSoft.DataEntryControls.Engine;
using RingSoft.DbLookup.AdvancedFind;
using RingSoft.DbLookup.Lookup;
using RingSoft.DbLookup.QueryBuilder;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;

namespace RingSoft.DbLookup.TableProcessing
{
    /// <summary>
    /// Class AdvancedFindFilterDefinition.
    /// Implements the <see cref="RingSoft.DbLookup.TableProcessing.FilterItemType{RingSoft.DbLookup.TableProcessing.AdvancedFindFilterDefinition}" />
    /// </summary>
    /// <seealso cref="RingSoft.DbLookup.TableProcessing.FilterItemType{RingSoft.DbLookup.TableProcessing.AdvancedFindFilterDefinition}" />
    public class AdvancedFindFilterDefinition : FilterItemType<AdvancedFindFilterDefinition>
    {
        /// <summary>
        /// Gets the type.
        /// </summary>
        /// <value>The type.</value>
        public override FilterItemTypes Type => FilterItemTypes.AdvancedFind;

        /// <summary>
        /// Gets the type of the TreeView.
        /// </summary>
        /// <value>The type of the TreeView.</value>
        public override TreeViewType TreeViewType => TreeViewType.AdvancedFind;
        /// <summary>
        /// Gets the name of the property.
        /// </summary>
        /// <value>The name of the property.</value>
        public override string PropertyName { get; internal set; }

        /// <summary>
        /// Gets or sets the advanced find identifier.
        /// </summary>
        /// <value>The advanced find identifier.</value>
        public int AdvancedFindId { get; set; }

        /// <summary>
        /// Gets the lookup definition.
        /// </summary>
        /// <value>The lookup definition.</value>
        public LookupDefinitionBase LookupDefinition { get; internal set; }


        /// <summary>
        /// Initializes a new instance of the <see cref="AdvancedFindFilterDefinition" /> class.
        /// </summary>
        /// <param name="tableFilterDefinition">The table filter definition.</param>
        internal AdvancedFindFilterDefinition(TableFilterDefinitionBase tableFilterDefinition) : base(tableFilterDefinition)
        {
            
        }

        /// <summary>
        /// Copies from.
        /// </summary>
        /// <param name="source">The source.</param>
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

        /// <summary>
        /// Gets the report text.
        /// </summary>
        /// <param name="lookupDefinition">The lookup definition.</param>
        /// <param name="printMode">if set to <c>true</c> [print mode].</param>
        /// <returns>System.String.</returns>
        public override string GetReportText(LookupDefinitionBase lookupDefinition,bool printMode)
        {
            if (!printMode)
                return string.Empty;

            var result = string.Empty;
            var advancedFind = SystemGlobals.AdvancedFindDbProcessor.GetAdvancedFind(AdvancedFindId);
            if (advancedFind != null)
            {
                var index = 0;
                foreach (var advancedFindFilter in advancedFind.Filters)
                {
                    var test = this;
                    advancedFindFilter.Path = Path + advancedFindFilter.Path;
                    var filterReturn = LookupDefinition.LoadFromAdvFindFilter(advancedFindFilter, false);
                    var recursive = false;
                    if (filterReturn is AdvancedFindFilterDefinition)
                    {
                        recursive = true;
                    }
                    result += filterReturn.GetPrintText(lookupDefinition);

                    var end = index == advancedFind.Filters.Count - 1;

                    if (!end)
                    {
                        result += filterReturn.PrintEndLogicText();
                    }

                    index++;
                    
                    result.TrimRight("\r\n");
                }
            }
            return result;
        }

        /// <summary>
        /// Loads from entity.
        /// </summary>
        /// <param name="entity">The entity.</param>
        /// <param name="lookupDefinition">The lookup definition.</param>
        /// <param name="path">The path.</param>
        /// <returns><c>true</c> if XXXX, <c>false</c> otherwise.</returns>
        public override bool LoadFromEntity(AdvancedFindFilter entity, LookupDefinitionBase lookupDefinition,
            string path = "")
        {
            if (entity.Path.IsNullOrEmpty())
            {
                TableDescription = lookupDefinition.TableDefinition.Description;
            }
            else
            {
                var afItem = lookupDefinition.AdvancedFindTree.ProcessFoundTreeViewItem(entity.Path,
                    TreeViewType.AdvancedFind);
                if (afItem == null)
                {
                    TableDescription = lookupDefinition.TableDefinition.Description;
                }
                else
                {
                    TableDescription = afItem.Name;
                }
            }

            if (entity.SearchForAdvancedFindId != null) 
                AdvancedFindId = entity.SearchForAdvancedFindId.Value;
            LookupDefinition = lookupDefinition;
            return base.LoadFromEntity(entity, lookupDefinition);
        }

        /// <summary>
        /// Loads from filter return.
        /// </summary>
        /// <param name="filterReturn">The filter return.</param>
        /// <param name="treeViewItem">The tree view item.</param>
        /// <returns>System.String.</returns>
        public override string LoadFromFilterReturn(AdvancedFilterReturn filterReturn, TreeViewItem treeViewItem)
        {
            return base.LoadFromFilterReturn(filterReturn, treeViewItem);
        }

        /// <summary>
        /// Gets the new filter item definition.
        /// </summary>
        /// <returns>FilterItemDefinition.</returns>
        /// <exception cref="System.NotImplementedException"></exception>
        public override FilterItemDefinition GetNewFilterItemDefinition()
        {
            throw new System.NotImplementedException();
        }

        /// <summary>
        /// Gets the new path.
        /// </summary>
        /// <returns>System.String.</returns>
        /// <exception cref="System.NotImplementedException"></exception>
        internal override string GetNewPath()
        {
            throw new System.NotImplementedException();
        }

        /// <summary>
        /// Gets the maui filter.
        /// </summary>
        /// <typeparam name="TEntity">The type of the t entity.</typeparam>
        /// <param name="param">The parameter.</param>
        /// <returns>Expression.</returns>
        public override Expression GetMauiFilter<TEntity>(ParameterExpression param)
        {
            var advancedFind = SystemGlobals.AdvancedFindDbProcessor.GetAdvancedFind(AdvancedFindId);

            return GetAdvFindExpression<TEntity>(advancedFind, param);
        }

        /// <summary>
        /// Gets the adv find expression.
        /// </summary>
        /// <typeparam name="TEntity">The type of the t entity.</typeparam>
        /// <param name="advancedFind">The advanced find.</param>
        /// <param name="param">The parameter.</param>
        /// <returns>Expression.</returns>
        private Expression GetAdvFindExpression<TEntity>(
            AdvancedFind.AdvancedFind advancedFind
            , ParameterExpression param)
        {
            var filters = advancedFind.Filters.ToList();

            Expression result = null;
            if (filters != null)
            {
                foreach (var advancedFindFilter in filters)
                {
                    Expression advFindExpr = null;
                    if (advancedFindFilter.SearchForAdvancedFindId.HasValue)
                    {
                        var subAdvancedFind = SystemGlobals.AdvancedFindDbProcessor.GetAdvancedFind(
                            advancedFindFilter.SearchForAdvancedFindId.Value);
                        var subFilter = GetAdvFindExpression<TEntity>(subAdvancedFind, param);
                        if (subFilter != null)
                        {
                            if (result == null)
                            {
                                result = subFilter;
                            }
                            else
                            {
                                result = FilterItemDefinition.AppendExpression(result, subFilter, EndLogic);
                            }
                        }
                    }
                    else
                    {
                        var lookupDef = new LookupDefinitionBase(advancedFind.Id);
                        result = lookupDef.FilterDefinition.GetWhereExpresssion<TEntity>(param);
                    }
                }
            }
            return result;
        }

        /// <summary>
        /// Processes the advanced find.
        /// </summary>
        /// <param name="query">The query.</param>
        /// <param name="firstWhereItem">The first where item.</param>
        /// <param name="lastWhereItem">The last where item.</param>
        /// <param name="fromAdvancedFind">if set to <c>true</c> [from advanced find].</param>
        /// <param name="tree">The tree.</param>
        /// <returns>List&lt;WhereItem&gt;.</returns>
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
                    var newPath = Path + advancedFindFilter.Path;

                    if (advancedFindFilter.SearchForAdvancedFindId != null)
                    {
                        var newAdvancedFindFilter = new AdvancedFindFilterDefinition(LookupDefinition.FilterDefinition);
                        newAdvancedFindFilter.Path = newPath;
                        newAdvancedFindFilter.LookupDefinition = LookupDefinition;
                        newAdvancedFindFilter.AdvancedFindId = advancedFindFilter.SearchForAdvancedFindId.Value;
                        newAdvancedFindFilter.TableFilterDefinition = TableFilterDefinition;
                        //newAdvancedFindFilter.Path = Path;
                        var newWheres = newAdvancedFindFilter.ProcessAdvancedFind(query, ref firstWhereItem, ref lastWhereItem,
                                true,
                                tree);
                        wheres.AddRange(newWheres);
                        TableFilterDefinition.ProcessFilterWheres(newWheres, ref firstWhereItem, ref lastWhereItem, advancedFindFilter);
                    }
                    else
                    {
                        //var foundTreeItem = LookupDefinition.AdvancedFindTree.ProcessFoundTreeViewItem(Path);
                        
                        advFindFilterReturn = LookupDefinition.LoadFromAdvFindFilter(advancedFindFilter
                        , false
                        , null, Path);
                        
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
