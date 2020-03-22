using RingSoft.DbLookupCore.ModelDefinition.FieldDefinitions;
using RingSoft.DbLookupCore.QueryBuilder;

namespace RingSoft.DbLookupCore.TableProcessing
{
    public class FieldFilterDefinition
    {
        /// <summary>
        /// Gets the table filter definition.
        /// </summary>
        /// <value>
        /// The table filter definition.
        /// </value>
        public TableFilterDefinitionBase TableFilterDefinition { get; internal set; }

        /// <summary>
        /// Gets the field definition.
        /// </summary>
        /// <value>
        /// The field definition.
        /// </value>
        public FieldDefinition FieldDefinition { get; internal set; }

        /// <summary>
        /// Gets the left parentheses count.
        /// </summary>
        /// <value>
        /// The left parentheses count.
        /// </value>
        public int LeftParenthesesCount { get; internal set; }

        /// <summary>
        /// Gets the condition.
        /// </summary>
        /// <value>
        /// The condition.
        /// </value>
        public Conditions Condition { get; internal set; }

        /// <summary>
        /// Gets the value.
        /// </summary>
        /// <value>
        /// The value.
        /// </value>
        public string Value { get; internal set; }

        /// <summary>
        /// Gets the right parentheses count.
        /// </summary>
        /// <value>
        /// The right parentheses count.
        /// </value>
        public int RightParenthesesCount { get; internal set; }

        /// <summary>
        /// Gets the end logic.
        /// </summary>
        /// <value>
        /// The end logic.
        /// </value>
        public EndLogics EndLogic { get; internal set; }

        /// <summary>
        /// Gets a value indicating whether the search is case sensitive.
        /// </summary>
        /// <value>
        ///   <c>true</c> if case sensitive; otherwise, <c>false</c>.
        /// </value>
        public bool CaseSensitive { get; internal set; }

        /// <summary>
        /// Gets a value indicating whether to cast enum value as int.
        /// </summary>
        /// <value>
        ///   <c>true</c> if cast enum value as int; otherwise, <c>false</c>.
        /// </value>
        public bool CastEnumValueAsInt { get; internal set; } = true;

        /// <summary>
        /// Gets the join definition.
        /// </summary>
        /// <value>
        /// The join definition.
        /// </value>
        public TableFieldJoinDefinition JoinDefinition { get; internal set; }

        internal FieldFilterDefinition()
        {
            
        }

        /// <summary>
        /// Sets the left parentheses count.
        /// </summary>
        /// <param name="count">The count.</param>
        /// <returns>This object.</returns>
        public FieldFilterDefinition SetLeftParenthesesCount(int count)
        {
            LeftParenthesesCount = count;
            return this;
        }

        /// <summary>
        /// Sets the right parentheses count.
        /// </summary>
        /// <param name="count">The count.</param>
        /// <returns>This object.</returns>
        public FieldFilterDefinition SetRightParenthesesCount(int count)
        {
            RightParenthesesCount = count;
            return this;
        }

        /// <summary>
        /// Sets the end logic.
        /// </summary>
        /// <param name="endLogic">The end logic (AND or OR).</param>
        /// <returns>This object.</returns>
        public FieldFilterDefinition SetEndLogic(EndLogics endLogic)
        {
            EndLogic = endLogic;
            return this;
        }

        /// <summary>
        /// Updates the condition.
        /// </summary>
        /// <param name="condition">The condition.</param>
        /// <returns>This object.</returns>
        public FieldFilterDefinition UpdateCondition(Conditions condition)
        {
            Condition = condition;
            return this;
        }

        /// <summary>
        /// Sets if the search is case sensitive.
        /// </summary>
        /// <param name="value">if set to <c>true</c> then the search is case sensitive.</param>
        /// <returns>This object.</returns>
        public FieldFilterDefinition IsCaseSensitive(bool value = true)
        {
            CaseSensitive = value;
            return this;
        }

        public void CopyFrom(FieldFilterDefinition source)
        {
            FieldDefinition = source.FieldDefinition;
            Condition = source.Condition;
            Value = source.Value;
            LeftParenthesesCount = source.LeftParenthesesCount;
            RightParenthesesCount = source.RightParenthesesCount;
            EndLogic = source.EndLogic;
            CastEnumValueAsInt = source.CastEnumValueAsInt;
            CaseSensitive = source.CaseSensitive;
            if (source.JoinDefinition != null)
            {
                JoinDefinition = new TableFieldJoinDefinition();
                JoinDefinition.CopyFrom(source.JoinDefinition);
                TableFilterDefinition.AddJoin(JoinDefinition);
            }

        }
    }
}
