﻿using MySqlX.XDevAPI.Common;
using RingSoft.DbLookup.AdvancedFind;
using RingSoft.DbLookup.Lookup;
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
        public FieldDefinition FieldDefinition { get; set; }

        /// <summary>
        /// Gets the condition.
        /// </summary>
        /// <value>
        /// The condition.
        /// </value>
        public Conditions Condition { get; set; }

        /// <summary>
        /// Gets the value to filter.
        /// </summary>
        /// <value>
        /// The value.
        /// </value>
        public string Value { get; set; }

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

        private FieldDefinition _parentField;
        public FieldDefinition ParentField
        {
            get => _parentField;
            set
            {
                _parentField = value;
            }
        }

        public string Path { get; internal set; }

        internal FieldFilterDefinition(TableFilterDefinitionBase tableFilterDefinition) : base(tableFilterDefinition)
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
            ParentField = fieldFilterDefinition.ParentField;
            Path = fieldFilterDefinition.Path;

            if (fieldFilterDefinition.JoinDefinition != null)
            {
                JoinDefinition = new TableFieldJoinDefinition();
                JoinDefinition.CopyFrom(fieldFilterDefinition.JoinDefinition);
                TableFilterDefinition.AddJoin(JoinDefinition);
            }

            base.CopyFrom(source);
        }

        public override string GetReportText()
        {
            var result = GetConditionText(Condition) + " ";
            result += FieldDefinition.GetUserValue(Value);
            return result;
        }

        public override void LoadFromEntity(AdvancedFindFilter entity, LookupDefinitionBase lookupDefinition)
        {
            base.LoadFromEntity(entity, lookupDefinition);
        }

        public override AdvancedFindFilter SaveToEntity(LookupDefinitionBase lookupDefinition)
        {
            return base.SaveToEntity(lookupDefinition);
        }

        public override void LoadFromFilterReturn(AdvancedFilterReturn filterReturn)
        {
            base.LoadFromFilterReturn(filterReturn);
        }

        public override AdvancedFilterReturn SaveToFilterReturn()
        {
            return base.SaveToFilterReturn();
        }

        public override string ToString()
        {
            return FieldDefinition.ToString();
        }
    }
}
