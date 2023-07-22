using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Linq.Expressions;
using RingSoft.DataEntryControls.Engine;
using RingSoft.DbLookup.QueryBuilder;

namespace RingSoft.DbLookup.TableProcessing
{
    public class FilterBundle
    {
        public IReadOnlyList<FilterItemDefinition> Filters => _filters.AsReadOnly();

        public TableFilterDefinitionBase TableFilter { get; }

        private readonly List<FilterItemDefinition> _filters = new List<FilterItemDefinition>();

        public FilterBundle(TableFilterDefinitionBase tableFilter)
        {
            TableFilter = tableFilter;
        }

        public int IndexOf(FilterItemDefinition filterItem)
        {
            return _filters.IndexOf(filterItem);
        }
        public void ClearFilters()
        {
            _filters.Clear();
        }

        public void AddFilter(FilterItemDefinition filterItem, int index = -1)
        {
            if (index < 0)
            {
                _filters.Add(filterItem);
            }
            else
            {
                _filters.Insert(index, filterItem);
            }
        }

        public void CopyFilters(FilterBundle sourceBundle)
        {
            _filters.Clear();
            foreach (var sourceFilter in sourceBundle.Filters)
            {
                FilterItemDefinition newFilterItem;
                switch (sourceFilter.Type)
                {
                    case FilterItemTypes.Field:
                        newFilterItem = new FieldFilterDefinition(TableFilter);
                        break;
                    case FilterItemTypes.Formula:
                        newFilterItem = new FormulaFilterDefinition(TableFilter);
                        break;
                    case FilterItemTypes.AdvancedFind:
                        newFilterItem = new AdvancedFindFilterDefinition(TableFilter);
                        break;
                    default:
                        throw new ArgumentOutOfRangeException();
                }

                newFilterItem.CopyFrom(sourceFilter);
                AddFilter(newFilterItem);
            }
        }

        internal void InternalAddFilter(int index, FilterItemDefinition filter)
        {
            if (_filters.Count < index)
            {
                index = -1;
            }
            if (index == -1 || !_filters.Any())
            {
                _filters.Add(filter);
            }
            else
            {
                _filters.Insert(index, filter);
            }
        }

        internal void InternalRemoveFilter(FilterItemDefinition filter)
        {
            _filters.Remove(filter);
        }

        public Expression GetMauiFilter<TEntity>(ParameterExpression param)
        {
            Expression result = null;
            
            Expression leftExpression = null;
            EndLogics endLogic = EndLogics.And;

            foreach (var filter in Filters)
            {
                var rightExpression  = filter.GetMauiFilter<TEntity>(param);
                if (rightExpression != null)
                {
                    if (leftExpression == null)
                    {
                        leftExpression = rightExpression;
                        rightExpression = null;
                    }
                }

                if (leftExpression != null && rightExpression != null)
                {
                    if (result == null)
                    {
                        result = FilterItemDefinition.AppendExpression(leftExpression, rightExpression,
                            endLogic);
                    }
                    else
                    {
                        result = FilterItemDefinition.AppendExpression(result, rightExpression, endLogic);
                    }
                }
                else
                {
                    result = leftExpression;
                }
                endLogic = filter.EndLogic;
            }

            return result;
        }

    }
}
