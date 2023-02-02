using RingSoft.DbLookup.Lookup;
using RingSoft.DbLookup.QueryBuilder;

namespace RingSoft.DbLookup.TableProcessing
{
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

        protected FilterItemType(TableFilterDefinitionBase tableFilterDefinition) : base(tableFilterDefinition)
        {
        }
    }
}
