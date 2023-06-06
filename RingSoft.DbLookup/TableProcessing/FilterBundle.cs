using System;
using System.Collections.Generic;
using System.Diagnostics;

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

        public void AddFilter(FilterItemDefinition filterItem)
        {
            _filters.Add(filterItem);
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
    }
}
