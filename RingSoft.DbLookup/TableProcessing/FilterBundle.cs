// ***********************************************************************
// Assembly         : RingSoft.DbLookup
// Author           : petem
// Created          : 06-06-2023
//
// Last Modified By : petem
// Last Modified On : 12-16-2023
// ***********************************************************************
// <copyright file="FilterBundle.cs" company="Peter Ringering">
//     Copyright (c) 2023. All rights reserved.
// </copyright>
// <summary></summary>
// ***********************************************************************
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Linq.Expressions;
using RingSoft.DataEntryControls.Engine;
using RingSoft.DbLookup.QueryBuilder;

namespace RingSoft.DbLookup.TableProcessing
{
    /// <summary>
    /// Class FilterBundle.
    /// </summary>
    public class FilterBundle
    {
        /// <summary>
        /// Gets the filters.
        /// </summary>
        /// <value>The filters.</value>
        public IReadOnlyList<FilterItemDefinition> Filters => _filters.AsReadOnly();

        /// <summary>
        /// Gets the table filter.
        /// </summary>
        /// <value>The table filter.</value>
        public TableFilterDefinitionBase TableFilter { get; }

        /// <summary>
        /// The filters
        /// </summary>
        private readonly List<FilterItemDefinition> _filters = new List<FilterItemDefinition>();

        /// <summary>
        /// Initializes a new instance of the <see cref="FilterBundle" /> class.
        /// </summary>
        /// <param name="tableFilter">The table filter.</param>
        public FilterBundle(TableFilterDefinitionBase tableFilter)
        {
            TableFilter = tableFilter;
        }

        /// <summary>
        /// Indexes the of.
        /// </summary>
        /// <param name="filterItem">The filter item.</param>
        /// <returns>System.Int32.</returns>
        public int IndexOf(FilterItemDefinition filterItem)
        {
            return _filters.IndexOf(filterItem);
        }
        /// <summary>
        /// Clears the filters.
        /// </summary>
        public void ClearFilters()
        {
            _filters.Clear();
        }

        /// <summary>
        /// Adds the filter.
        /// </summary>
        /// <param name="filterItem">The filter item.</param>
        /// <param name="index">The index.</param>
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

        /// <summary>
        /// Copies the filters.
        /// </summary>
        /// <param name="sourceBundle">The source bundle.</param>
        /// <exception cref="System.ArgumentOutOfRangeException"></exception>
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

        /// <summary>
        /// Internals the add filter.
        /// </summary>
        /// <param name="index">The index.</param>
        /// <param name="filter">The filter.</param>
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

        /// <summary>
        /// Internals the remove filter.
        /// </summary>
        /// <param name="filter">The filter.</param>
        internal void InternalRemoveFilter(FilterItemDefinition filter)
        {
            _filters.Remove(filter);
        }

        /// <summary>
        /// Gets the maui filter.
        /// </summary>
        /// <typeparam name="TEntity">The type of the t entity.</typeparam>
        /// <param name="param">The parameter.</param>
        /// <returns>Expression.</returns>
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
