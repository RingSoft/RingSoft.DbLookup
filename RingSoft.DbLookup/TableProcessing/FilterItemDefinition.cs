using RingSoft.DbLookup.AdvancedFind;
using RingSoft.DbLookup.QueryBuilder;
using System;
using System.Linq;
using RingSoft.DbLookup.Lookup;
using RingSoft.DataEntryControls.Engine;
using RingSoft.DbLookup.ModelDefinition;
using RingSoft.DbLookup.ModelDefinition.FieldDefinitions;

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

        public string TableDescription { get; set; }

        public string ReportDescription { get; set; }

        public FilterItemDefinition(TableFilterDefinitionBase tableFilterDefinition)
        {
            TableFilterDefinition = tableFilterDefinition;
        }

        internal virtual void CopyFrom(FilterItemDefinition source)
        {
            LeftParenthesesCount = source.LeftParenthesesCount;
            RightParenthesesCount = source.RightParenthesesCount;
            TableFilterDefinition = source.TableFilterDefinition;
            JoinDefinition = source.JoinDefinition;
            EndLogic = source.EndLogic;
            TableDescription = source.TableDescription;
            ReportDescription = source.ReportDescription;
        }

        public abstract string GetReportText();

        public static string GetConditionText(Conditions condition)
        {
            var searchValue = string.Empty;
            switch (condition)
            {
                case Conditions.Equals:
                    searchValue = "= ";
                    break;
                case Conditions.NotEquals:
                    searchValue = "<> ";
                    break;
                case Conditions.GreaterThan:
                    searchValue = "> ";
                    break;
                case Conditions.GreaterThanEquals:
                    searchValue = ">= ";
                    break;
                case Conditions.LessThan:
                    searchValue = "< ";
                    break;
                case Conditions.LessThanEquals:
                    searchValue = "<= ";
                    break;
                case Conditions.Contains:
                    searchValue = "Contains ";
                    break;
                case Conditions.NotContains:
                    searchValue = "Does Not Contain ";
                    break;
                case Conditions.EqualsNull:
                    searchValue = "Equals NULL";
                    break;
                case Conditions.NotEqualsNull:
                    searchValue = "Does Not Equal NULL";
                    break;
                case Conditions.BeginsWith:
                    searchValue = "Begins With ";
                    break;
                case Conditions.EndsWith:
                    searchValue = "Ends With ";
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }

            return searchValue;
        }

        public virtual void LoadFromEntity(AdvancedFindFilter entity, LookupDefinitionBase lookupDefinition)
        {
            LeftParenthesesCount = entity.LeftParentheses;
            RightParenthesesCount = entity.RightParentheses;
            EndLogic = (EndLogics)entity.EndLogic;
            TableFilterDefinition.AddUserFilter(this);
            //else
            {
                //if (filterItemDefinition is FormulaFilterDefinition formulaFilter)
                //{
                //    if (!entity.Path.IsNullOrEmpty())
                //    {
                //        filterItemDefinition.TableDescription = foundItem.Name;
                //    }
                //}
                //else
                //{
                //    filterItemDefinition.TableDescription = TableDefinition.Description;
                //}
            }

        }

        public virtual AdvancedFindFilter SaveToEntity(LookupDefinitionBase lookupDefinition)
        {
            return new AdvancedFindFilter();
        }

        public virtual void LoadFromFilterReturn(AdvancedFilterReturn filterReturn, TreeViewItem treeViewItem)
        {
        }

        public virtual AdvancedFilterReturn SaveToFilterReturn()
        {
            return new AdvancedFilterReturn();
        }

        protected internal TableDefinitionBase GetTableFieldForFilter(LookupDefinitionBase lookupDefinition,
            AdvancedFindFilter entity, out FieldDefinition fieldDefinition,
            out FieldDefinition filterField)
        {
            var tableDefinition =
                lookupDefinition.TableDefinition.Context.TableDefinitions.FirstOrDefault(p =>
                    p.EntityName == entity.TableName);

            fieldDefinition = null;
            filterField = null;
            if (!entity.FieldName.IsNullOrEmpty())
            {
                filterField = fieldDefinition =
                    tableDefinition.FieldDefinitions.FirstOrDefault(p => p.FieldName == entity.FieldName);
            }

            TableDefinitionBase primaryTable = null;
            FieldDefinition primaryField = null;
            if (!entity.PrimaryTableName.IsNullOrEmpty() && !entity.PrimaryFieldName.IsNullOrEmpty())
            {
                primaryTable =
                    lookupDefinition.TableDefinition.Context.TableDefinitions.FirstOrDefault(p =>
                        p.TableName == entity.PrimaryTableName);

                primaryField =
                    primaryTable.FieldDefinitions.FirstOrDefault(p => p.FieldName == entity.PrimaryFieldName);

                if (fieldDefinition == null)
                {
                    tableDefinition = primaryTable;
                    fieldDefinition = primaryField;
                }
            }

            return tableDefinition;
        }
    }
}
