// ***********************************************************************
// Assembly         : RingSoft.DbLookup
// Author           : petem
// Created          : 12-19-2022
//
// Last Modified By : petem
// Last Modified On : 01-25-2023
// ***********************************************************************
// <copyright file="WhereItem.cs" company="Peter Ringering">
//     Copyright (c) 2023. All rights reserved.
// </copyright>
// <summary></summary>
// ***********************************************************************
using System.ComponentModel;

namespace RingSoft.DbLookup.QueryBuilder
{
    /// <summary>
    /// Enum WhereItemTypes
    /// </summary>
    public enum WhereItemTypes
    {
        /// <summary>
        /// The general
        /// </summary>
        General = 0,
        /// <summary>
        /// The formula
        /// </summary>
        Formula = 1,
        /// <summary>
        /// The enum
        /// </summary>
        Enum = 2
    }

    /// <summary>
    /// Enum Conditions
    /// </summary>
    public enum Conditions
    {
        /// <summary>
        /// The equals
        /// </summary>
        [Description("Equals (=)")]
        Equals = 0,
        /// <summary>
        /// The not equals
        /// </summary>
        [Description("Not Equals (<>)")]
        NotEquals = 1,
        /// <summary>
        /// The greater than
        /// </summary>
        [Description("Greater Than (>)")]
        GreaterThan = 2,
        /// <summary>
        /// The greater than equals
        /// </summary>
        [Description("Greater Than Or Equal To (>=)")]
        GreaterThanEquals = 3,
        /// <summary>
        /// The less than
        /// </summary>
        [Description("Less Than (<)")]
        LessThan = 4,
        /// <summary>
        /// The less than equals
        /// </summary>
        [Description("Less Than Or Equal To (<=)")]
        LessThanEquals = 5,
        /// <summary>
        /// Determines whether this instance contains the object.
        /// </summary>
        [Description("Contains")]
        Contains = 6,
        /// <summary>
        /// The not contains
        /// </summary>
        [Description("Does Not Contain")]
        NotContains = 7,
        /// <summary>
        /// The equals null
        /// </summary>
        [Description("Is NULL")]
        EqualsNull = 8,
        /// <summary>
        /// The not equals null
        /// </summary>
        [Description("Is Not NULL")]
        NotEqualsNull = 9,
        /// <summary>
        /// The begins with
        /// </summary>
        [Description("Begins With")]
        BeginsWith = 10,
        /// <summary>
        /// The ends with
        /// </summary>
        [Description("Ends With")]
        EndsWith = 11,
    }

    /// <summary>
    /// Enum ValueTypes
    /// </summary>
    public enum ValueTypes
    {
        /// <summary>
        /// The string
        /// </summary>
        String = 0,
        /// <summary>
        /// The numeric
        /// </summary>
        Numeric = 1,
        /// <summary>
        /// The date time
        /// </summary>
        DateTime = 2,
        /// <summary>
        /// The bool
        /// </summary>
        Bool = 3,
        /// <summary>
        /// The memo
        /// </summary>
        Memo = 4
    }

    /// <summary>
    /// Enum EndLogics
    /// </summary>
    public enum EndLogics
    {
        /// <summary>
        /// The and
        /// </summary>
        [Description("AND")]
        And = 0,
        /// <summary>
        /// The or
        /// </summary>
        [Description("OR")]
        Or = 1
    }

    /// <summary>
    /// Enum DbDateTypes
    /// </summary>
    public enum DbDateTypes
    {
        /// <summary>
        /// The date only
        /// </summary>
        DateOnly = 0,
        /// <summary>
        /// The date time
        /// </summary>
        DateTime = 1,
        /// <summary>
        /// The millisecond
        /// </summary>
        Millisecond = 2,
    }

    /// <summary>
    /// A segment of a WHERE clause.  e.g.(Field = Value AND)
    /// </summary>
    public class WhereItem
    {
        /// <summary>
        /// Gets the type of the where item.
        /// </summary>
        /// <value>The type of the where item.</value>
        public virtual WhereItemTypes WhereItemType => WhereItemTypes.General;

        /// <summary>
        /// Gets the left parentheses count.
        /// </summary>
        /// <value>The left parentheses count.</value>
        public int LeftParenthesesCount { get; internal set; }

        /// <summary>
        /// Gets the table object this Where Item is attached to.
        /// </summary>
        /// <value>The table.</value>
        public QueryTable Table { get; internal set; }

        /// <summary>
        /// Gets the name of the field.
        /// </summary>
        /// <value>The name of the field.</value>
        public string FieldName { get; internal set; }

        /// <summary>
        /// Gets the condition.
        /// </summary>
        /// <value>The condition.</value>
        public Conditions Condition { get; internal set; }

        /// <summary>
        /// Gets the value.
        /// </summary>
        /// <value>The value.</value>
        public string Value { get; internal set; }

        /// <summary>
        /// Gets the type of the value.
        /// </summary>
        /// <value>The type of the value.</value>
        public ValueTypes ValueType { get; internal set; }

        /// <summary>
        /// Gets the right parentheses count.
        /// </summary>
        /// <value>The right parentheses count.</value>
        public int RightParenthesesCount { get; internal set; }

        /// <summary>
        /// Gets the end logic.
        /// </summary>
        /// <value>The end logic.</value>
        public EndLogics EndLogic { get; internal set; }

        /// <summary>
        /// Gets the type of the date.  (For DateTime value types.)
        /// </summary>
        /// <value>The type of the date.</value>
        public DbDateTypes DateType { get; internal set; }

        /// <summary>
        /// Gets a value indicating whether the search is case sensitive.
        /// </summary>
        /// <value><c>true</c> if case sensitive; otherwise, <c>false</c>.</value>
        public bool CaseSensitive { get; internal set; }

        /// <summary>
        /// Gets a value indicating whether [check description for null].
        /// </summary>
        /// <value><c>true</c> if [check description for null]; otherwise, <c>false</c>.</value>
        public bool CheckDescriptionForNull { get; internal set; }

        /// <summary>
        /// Initializes a new instance of the <see cref="WhereItem"/> class.
        /// </summary>
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

        /// <summary>
        /// Returns a <see cref="System.String" /> that represents this instance.
        /// </summary>
        /// <returns>A <see cref="System.String" /> that represents this instance.</returns>
        public override string ToString()
        {
            return FieldName;
        }
    }
}
