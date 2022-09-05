using RingSoft.DbLookup.QueryBuilder;

namespace RingSoft.DbLookup.TableProcessing
{
    public enum FilterItemTypes
    {
        Field = 0,
        Formula = 1,
        AdvancedFind = 2
    }

    /// <summary>
    /// Represents a filter item in a TableFilterDefinition.
    /// </summary>
    public abstract class FilterItemDefinition
    {
        /// <summary>
        /// Gets the type.
        /// </summary>
        /// <value>
        /// The type.
        /// </value>
        public abstract FilterItemTypes Type { get; }

        /// <summary>
        /// Gets the table filter definition.
        /// </summary>
        /// <value>
        /// The table filter definition.
        /// </value>
        public TableFilterDefinitionBase TableFilterDefinition { get; internal set; }

        /// <summary>
        /// Gets the left parentheses count.
        /// </summary>
        /// <value>
        /// The left parentheses count.
        /// </value>
        public int LeftParenthesesCount { get; set; }

        /// <summary>
        /// Gets the right parentheses count.
        /// </summary>
        /// <value>
        /// The right parentheses count.
        /// </value>
        public int RightParenthesesCount { get; set; }

        /// <summary>
        /// Gets the end logic.
        /// </summary>
        /// <value>
        /// The end logic.
        /// </value>
        public EndLogics EndLogic { get; set; }

        /// <summary>
        /// Gets the join definition.
        /// </summary>
        /// <value>
        /// The join definition.
        /// </value>
        public TableFieldJoinDefinition JoinDefinition { get; set; }


        internal virtual void CopyFrom(FilterItemDefinition source)
        {
            LeftParenthesesCount = source.LeftParenthesesCount;
            RightParenthesesCount = source.RightParenthesesCount;
            TableFilterDefinition = source.TableFilterDefinition;
            EndLogic = source.EndLogic;
        }
    }
}
