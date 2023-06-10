using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq.Expressions;

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
            if (index == -1)
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
                    result = FilterItemDefinition.AppendExpression(leftExpression, rightExpression,
                        filter.EndLogic);
                }
                else
                {
                    result = leftExpression;
                }
            }

            return result;
        }

    }
}
