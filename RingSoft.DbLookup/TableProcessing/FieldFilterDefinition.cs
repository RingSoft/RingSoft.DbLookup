using RingSoft.DbLookup.ModelDefinition.FieldDefinitions;
using RingSoft.DbLookup.QueryBuilder;

namespace RingSoft.DbLookup.TableProcessing
{
    /// <summary>
    /// Represents a filter field item in a table filter definition.
    /// </summary>
    public class FieldFilterDefinition : FilterItemType<FieldFilterDefinition>
    {
        public override FilterItemTypes Type => FilterItemTypes.Field;


        /// <summary>
        /// Gets the field definition.
        /// </summary>
        /// <value>
        /// The field definition.
        /// </value>
        public FieldDefinition FieldDefinition { get; internal set; }

        /// <summary>
        /// Gets the condition.
        /// </summary>
        /// <value>
        /// The condition.
        /// </value>
        public Conditions Condition { get; internal set; }

        /// <summary>
        /// Gets the value to filter.
        /// </summary>
        /// <value>
        /// The value.
        /// </value>
        public string Value { get; internal set; }

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

        internal FieldFilterDefinition()
        {
            
        }

        /// <summary>
        /// Sets if the search is case sensitive.
        /// </summary>
        /// <param name="value">if set to True then the search is case sensitive.</param>
        /// <returns>This object.</returns>
        public FieldFilterDefinition IsCaseSensitive(bool value = true)
        {
            CaseSensitive = value;
            return this;
        }

        internal override void CopyFrom(FilterItemDefinition source)
        {
            var fieldFilterDefinition = (FieldFilterDefinition) source;
            FieldDefinition = fieldFilterDefinition.FieldDefinition;
            Condition = fieldFilterDefinition.Condition;
            Value = fieldFilterDefinition.Value;
            CastEnumValueAsInt = fieldFilterDefinition.CastEnumValueAsInt;
            CaseSensitive = fieldFilterDefinition.CaseSensitive;

            if (fieldFilterDefinition.JoinDefinition != null)
            {
                JoinDefinition = new TableFieldJoinDefinition();
                JoinDefinition.CopyFrom(fieldFilterDefinition.JoinDefinition);
                TableFilterDefinition.AddJoin(JoinDefinition);
            }

            base.CopyFrom(source);
        }
    }
}
