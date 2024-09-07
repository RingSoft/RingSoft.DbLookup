// ***********************************************************************
// Assembly         : RingSoft.DbLookup
// Author           : petem
// Created          : 12-19-2022
//
// Last Modified By : petem
// Last Modified On : 12-16-2023
// ***********************************************************************
// <copyright file="FilterItemType.cs" company="Peter Ringering">
//     Copyright (c) 2023. All rights reserved.
// </copyright>
// <summary></summary>
// ***********************************************************************
using RingSoft.DbLookup.Lookup;
using RingSoft.DbLookup.QueryBuilder;

namespace RingSoft.DbLookup.TableProcessing
{
    /// <summary>
    /// Class FilterItemType.
    /// Implements the <see cref="RingSoft.DbLookup.TableProcessing.FilterItemDefinition" />
    /// </summary>
    /// <typeparam name="TFilterItem">The type of the t filter item.</typeparam>
    /// <seealso cref="RingSoft.DbLookup.TableProcessing.FilterItemDefinition" />
    public abstract class FilterItemType<TFilterItem> : FilterItemDefinition
        where TFilterItem : FilterItemType<TFilterItem>
    {
        /// <summary>
        /// Sets the left parentheses count.
        /// </summary>
        /// <param name="count">The count.</param>
        /// <returns>This object.</returns>
        public TFilterItem SetLeftParenthesesCount(int count)
        {
            LeftParenthesesCount = count;
            return (TFilterItem)this;
        }

        /// <summary>
        /// Sets the right parentheses count.
        /// </summary>
        /// <param name="count">The count.</param>
        /// <returns>This object.</returns>
        public TFilterItem SetRightParenthesesCount(int count)
        {
            RightParenthesesCount = count;
            return (TFilterItem)this;
        }

        /// <summary>
        /// Sets the end logic.
        /// </summary>
        /// <param name="endLogic">The end logic (AND or OR).</param>
        /// <returns>This object.</returns>
        public TFilterItem SetEndLogic(EndLogics endLogic)
        {
            EndLogic = endLogic;
            return (TFilterItem)this;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="FilterItemType{TFilterItem}" /> class.
        /// </summary>
        /// <param name="tableFilterDefinition">The table filter definition.</param>
        protected FilterItemType(TableFilterDefinitionBase tableFilterDefinition) : base(tableFilterDefinition)
        {
        }
    }
}
