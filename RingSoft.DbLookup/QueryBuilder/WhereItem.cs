using System.ComponentModel;

namespace RingSoft.DbLookup.QueryBuilder
{
    public enum WhereItemTypes
    {
        General = 0,
        Formula = 1,
        Enum = 2
    }

    public enum Conditions
    {
        [Description("Equals (=)")]
        Equals = 0,
        [Description("Not Equals (<>)")]
        NotEquals = 1,
        [Description("Greater Than (>)")]
        GreaterThan = 2,
        [Description("Greater Than Or Equal To (>=)")]
        GreaterThanEquals = 3,
        [Description("Less Than (<)")]
        LessThan = 4,
        [Description("Less Than Or Equal To (<=)")]
        LessThanEquals = 5,
        [Description("Contains")]
        Contains = 6,
        [Description("Does Not Contain")]
        NotContains = 7,
        [Description("Is NULL")]
        EqualsNull = 8,
        [Description("Is Not NULL")]
        NotEqualsNull = 9,
        [Description("Begins With")]
        BeginsWith = 10,
        [Description("Ends With")]
        EndsWith = 11,
    }

    public enum ValueTypes
    {
        String = 0,
        Numeric = 1,
        DateTime = 2,
        Bool = 3,
        Memo = 4
    }

    public enum EndLogics
    {
        [Description("AND")]
        And = 0,
        [Description("OR")]
        Or = 1
    }

    public enum DbDateTypes
    {
        DateOnly = 0,
        DateTime = 1
    }

    /// <summary>
    /// A segment of a WHERE clause.  e.g.(Field = Value AND)
    /// </summary>
    public class WhereItem
    {
        /// <summary>
        /// Gets the type of the where item.
        /// </summary>
        /// <value>
        /// The type of the where item.
        /// </value>
        public virtual WhereItemTypes WhereItemType => WhereItemTypes.General;

        /// <summary>
        /// Gets the left parentheses count.
        /// </summary>
        /// <value>
        /// The left parentheses count.
        /// </value>
        public int LeftParenthesesCount { get; internal set; }

        /// <summary>
        /// Gets the table object this Where Item is attached to.
        /// </summary>
        /// <value>
        /// The table.
        /// </value>
        public QueryTable Table { get; internal set; }

        /// <summary>
        /// Gets the name of the field.
        /// </summary>
        /// <value>
        /// The name of the field.
        /// </value>
        public string FieldName { get; internal set; }

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
        /// Gets the type of the value.
        /// </summary>
        /// <value>
        /// The type of the value.
        /// </value>
        public ValueTypes ValueType { get; internal set; }

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
        /// Gets the type of the date.  (For DateTime value types.)
        /// </summary>
        /// <value>
        /// The type of the date.
        /// </value>
        public DbDateTypes DateType { get; internal set; }

        /// <summary>
        /// Gets a value indicating whether the search is case sensitive.
        /// </summary>
        /// <value>
        ///   <c>true</c> if case sensitive; otherwise, <c>false</c>.
        /// </value>
        public bool CaseSensitive { get; internal set; }

        public bool CheckDescriptionForNull { get; internal set; }

        internal WhereItem()
        {
            
        }

        /// <summary>
        /// Sets the left parentheses count.
        /// </summary>
        /// <param name="count">The count.</param>
        /// <returns>This object.</returns>
        public WhereItem SetLeftParenthesesCount(int count)
        {
            LeftParenthesesCount = count;
            return this;
        }

        /// <summary>
        /// Sets the right parentheses count.
        /// </summary>
        /// <param name="count">The count.</param>
        /// <returns>This object.</returns>
        public WhereItem SetRightParenthesesCount(int count)
        {
            RightParenthesesCount = count;
            return this;
        }

        /// <summary>
        /// Sets the end logic.
        /// </summary>
        /// <param name="endLogic">The end logic (AND or OR).</param>
        /// <returns>This object.</returns>
        public WhereItem SetEndLogic(EndLogics endLogic)
        {
            EndLogic = endLogic;
            return this;
        }

        /// <summary>
        /// Updates the condition.
        /// </summary>
        /// <param name="condition">The condition.</param>
        /// <returns>This object.</returns>
        public WhereItem UpdateCondition(Conditions condition)
        {
            Condition = condition;
            return this;
        }

        /// <summary>
        /// Sets if the search is case sensitive.
        /// </summary>
        /// <param name="value">if set to <c>true</c> then the search is case sensitive.</param>
        /// <returns>This object.</returns>
        public WhereItem IsCaseSensitive(bool value = true)
        {
            CaseSensitive = value;
            return this;
        }

        public override string ToString()
        {
            return FieldName;
        }
    }
}
