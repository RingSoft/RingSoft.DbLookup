
using RingSoft.DataEntryControls.Engine;
using RingSoft.DbLookup.AdvancedFind;
using RingSoft.DbLookup.Lookup;
using RingSoft.DbLookup.ModelDefinition.FieldDefinitions;
using RingSoft.DbLookup.QueryBuilder;
using System;
using System.ComponentModel;
using System.Linq.Expressions;

namespace RingSoft.DbLookup.TableProcessing
{
    public enum DateFilterTypes
    {
        [Description("Specific Date")]
        SpecificDate = 0,
        [Description("Day(s) Ago")]
        Days = 1,
        [Description("Week(s) Ago")]
        Weeks = 2,
        [Description("Month(s) Ago")]
        Months = 3,
        [Description("Year(s) Ago")]
        Years = 4,
        [Description("Hour(s) Ago")]
        Hours = 5,
        [Description("Minute(s) Ago")]
        Minutes = 6,
        [Description("Second(s) Ago")]
        Seconds = 7,
    }

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
        private FieldDefinition _fieldDefinition;

        public FieldDefinition FieldDefinition
        {
            get => _fieldDefinition;
            internal set
            {
                _fieldDefinition = value;
                ValueType = _fieldDefinition.ValueType;
                if (FieldDefinition is DateFieldDefinition dateFieldDefinition)
                {
                    DateType = dateFieldDefinition.DateType;
                }
            }
        }


        /// <summary>
        /// Gets the condition.
        /// </summary>
        /// <value>
        /// The condition.
        /// </value>
        public Conditions Condition { get; set; }

        /// <summary>
        /// Gets a value indicating whether the search is case sensitive.
        /// </summary>
        /// <value>
        ///   <c>true</c> if case sensitive; otherwise, <c>false</c>.
        /// </value>
        private bool _caseSensitive;

        public bool CaseSensitive
        {
            get
            {
                switch (ValueType)
                {
                    case ValueTypes.Numeric:
                    case ValueTypes.DateTime:
                    case ValueTypes.Bool:
                        return false;
                    case ValueTypes.String:
                    case ValueTypes.Memo:
                        return _caseSensitive;
                    default:
                        throw new ArgumentOutOfRangeException();
                }

            }
            internal set
            {
                _caseSensitive = value;
            }
        }

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
            set { _parentField = value; }
        }

        private FieldDefinition _fieldToSearch;

        public FieldDefinition FieldToSearch
        {
            get
            {
                if (_fieldToSearch == null)
                {
                    return FieldDefinition;
                }

                return _fieldToSearch;
            }
            internal set => _fieldToSearch = value;
        }

        public ILookupFormula FormulaToSearch { get; internal set; }

        public LookupDefinitionBase BaseLookup { get; internal set; }

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

        public override TreeViewType TreeViewType => TreeViewType.Field;

        public string SetPropertyName { get; set; }

        public override string PropertyName
        {
            get
            {
                if (SetPropertyName.IsNullOrEmpty())
                {
                    var field = FieldDefinition;
                    if (FieldToSearch != null)
                    {
                        field = FieldToSearch;
                    }

                    return JoinDefinition.GetPropertyJoinName(field.PropertyName);
                }

                return SetPropertyName;
            }

            internal set
            {
                SetPropertyName = value;
            }
        }

        internal override void CopyFrom(FilterItemDefinition source)
        {
            var fieldFilterDefinition = (FieldFilterDefinition)source;
            FieldDefinition = fieldFilterDefinition.FieldDefinition;
            Condition = fieldFilterDefinition.Condition;
            Value = fieldFilterDefinition.Value;
            CastEnumValueAsInt = fieldFilterDefinition.CastEnumValueAsInt;
            CaseSensitive = fieldFilterDefinition.CaseSensitive;
            ParentField = fieldFilterDefinition.ParentField;
            Path = fieldFilterDefinition.Path;
            SetPropertyName = fieldFilterDefinition.SetPropertyName;

            if (fieldFilterDefinition.JoinDefinition != null)
            {
                JoinDefinition = new TableFieldJoinDefinition();
                JoinDefinition.CopyFrom(fieldFilterDefinition.JoinDefinition);
                SetTableDescription();
                TableFilterDefinition.AddJoin(JoinDefinition);
            }

            base.CopyFrom(source);
        }

        protected internal override string GetReportBeginTextPrintMode(LookupDefinitionBase lookupDefinition)
        {
            var tableDescription = TableDescription;

            var result = $"{tableDescription}.{FieldDefinition.Description}";
            return result;
            //return base.GetReportBeginTextPrintMode(lookupDefinition);
        }

        public override string GetReportText(LookupDefinitionBase lookupDefinition, bool printMode)
        {
            var result = string.Empty;
            if (printMode)
            {
                result += GetReportBeginTextPrintMode(lookupDefinition) + " ";
            }
            result += GetConditionText(Condition) + " ";
            var setUserValue = ValueType != ValueTypes.DateTime;

            switch (Condition)
            {
                case Conditions.Equals:
                case Conditions.NotEquals:
                    if (setUserValue)
                    {
                        if (FieldDefinition != null) result += FieldDefinition.GetUserValue(Value);
                    }
                    else
                    {
                        result += GetDateReportText();
                    }
                    break;
                case Conditions.EqualsNull:
                case Conditions.NotEqualsNull:
                    result = result.Trim();
                    break;
                default:
                    if (ValueType == ValueTypes.DateTime)
                    {
                        var dateSearchValue = GetDateReportText();
                        result += dateSearchValue;
                    }
                    else
                    {
                        result += Value;
                    }

                    break;
            }

            return result;
        }

        public override bool LoadFromEntity(AdvancedFindFilter entity, LookupDefinitionBase lookupDefinition,
            string path = "")
        {
            Path = entity.Path;
            Condition = (Conditions)entity.Operand;
            BaseLookup = lookupDefinition;

            if (FieldDefinition != null)
            {
                var newPath = GetNewPath();

                if (!newPath.IsNullOrEmpty())
                {
                    var treeViewItem = lookupDefinition.AdvancedFindTree.ProcessFoundTreeViewItem(newPath);
                    if (treeViewItem != null)
                    {
                        FieldDefinition = treeViewItem.FieldDefinition;
                        ProcessFoundTreeItem(treeViewItem);
                    }
                }

                var value = entity.SearchForValue;
                if (value.IsNullOrEmpty())
                {
                    switch (Condition)
                    {
                        case Conditions.EqualsNull:
                        case Conditions.NotEqualsNull:
                            break;
                        default:
                            return false;
                    }
                }
            }

            var result = base.LoadFromEntity(entity, lookupDefinition, Path);
            if (result)
            {
                SetTableDescription();
                if (DateFilterType != DateFilterTypes.SpecificDate)
                {
                    Value = ConvertDate(Value);
                }
            }
            return result;
        }

        public override void SaveToEntity(AdvancedFindFilter entity)
        {
            entity.Operand = (byte)Condition;
            entity.TableName = FieldDefinition.TableDefinition.Description;
            entity.FieldName = FieldDefinition.Description;
            base.SaveToEntity(entity);
        }

        public override string LoadFromFilterReturn(AdvancedFilterReturn filterReturn, TreeViewItem treeViewItem)
        {
            Condition = filterReturn.Condition;
            Value = filterReturn.SearchValue;

            ProcessFoundTreeItem(treeViewItem);

            var result = base.LoadFromFilterReturn(filterReturn, treeViewItem);
            Value = result;

            return result;
        }

        private void ProcessFoundTreeItem(TreeViewItem treeViewItem)
        {
            Path = treeViewItem.MakePath();
            ProcessIncludeResult includeResult = null;
            if (treeViewItem.Parent != null)
            {
                includeResult =
                    treeViewItem.BaseTree.MakeIncludes(treeViewItem.Parent);

                JoinDefinition = includeResult.LookupJoin.JoinDefinition;
            }

            switch (Condition)
            {
                case Conditions.Equals:
                case Conditions.NotEquals:
                case Conditions.EqualsNull:
                case Conditions.NotEqualsNull:
                    FormulaToSearch = null;
                    //FieldToSearch = null;
                    if (treeViewItem.Parent == null)
                    {
                        JoinDefinition = null;
                    }
                    else
                    {
                        if (includeResult != null) JoinDefinition = includeResult.LookupJoin.JoinDefinition;
                    }

                    break;
                default:
                    if (FieldDefinition.ParentJoinForeignKeyDefinition != null)
                    {
                        var textColumn = FieldDefinition.ParentJoinForeignKeyDefinition.PrimaryTable
                            .LookupDefinition.InitialSortColumnDefinition;

                        var fieldToSearch = textColumn.GetFieldForColumn();
                        if (fieldToSearch == null)
                        {
                            SetJoinDefinition(treeViewItem, treeViewItem.FieldDefinition);

                            FormulaToSearch = textColumn.GetFormulaForColumn();
                        }
                        else
                        {
                            SetJoinDefinition(treeViewItem, fieldToSearch);

                            FieldToSearch = fieldToSearch;
                        }
                    }

                    break;
            }
        }

        private void SetJoinDefinition(TreeViewItem treeViewItem, FieldDefinition fieldToSearch)
        {
            var newTreeViewItem = treeViewItem.BaseTree.FindFieldInTree(treeViewItem.Items, fieldToSearch);
            if (newTreeViewItem == null)
            {
                var includeResult = treeViewItem.BaseTree.MakeIncludes(treeViewItem);

                JoinDefinition = includeResult.LookupJoin.JoinDefinition;
            }
            else
            {
                var includeResult = treeViewItem.BaseTree.MakeIncludes(newTreeViewItem);

                JoinDefinition = includeResult.LookupJoin.JoinDefinition;
            }
        }

        public override void SaveToFilterReturn(AdvancedFilterReturn filterReturn)
        {
            filterReturn.Condition = Condition;
            filterReturn.FieldDefinition = FieldDefinition;
            filterReturn.SearchValue = Value;
            base.SaveToFilterReturn(filterReturn);
        }

        public override string ToString()
        {
            return FieldDefinition.ToString();
        }

        protected internal override string ConvertDate(string value)
        {
            value = base.ConvertDate(value);
            //if (FieldDefinition is DateFieldDefinition dateField)
            //{
            //    if (dateField.ConvertToLocalTime || SystemGlobals.ConvertAllDatesToUniversalTime)
            //    {
            //        var date = value.ToDate();
            //        if (date != null)
            //        {
            //            date = date.Value.ToUniversalTime();
            //            return date.Value.FormatDateValue(dateField.DateType);
            //        }
            //    }

            //}

            return value;
        }

        public override FilterItemDefinition GetNewFilterItemDefinition()
        {
            var result = new FieldFilterDefinition(TableFilterDefinition);
            
            return result;
        }

        internal override string GetNewPath()
        {
            if (!Path.IsNullOrEmpty())
            {
                return Path;
            }
            //var path = FieldDefinition.MakePath();
            var path = string.Empty;
            var parentObject = JoinDefinition?.ParentObject;
            while (parentObject != null)
            {
                path = parentObject.MakePath() + path;
                parentObject = parentObject.ParentObject;
            }
            var test = this;
            if (FieldDefinition.TableDefinition != TableFilterDefinition.TableDefinition)
            {
                if (JoinDefinition != null)
                {
                    path += JoinDefinition.ForeignKeyDefinition.FieldJoins[0].ForeignField.MakePath();
                }
            }

            if (path.IsNullOrEmpty())
            {
                path = Path;
            }
            return path;// + FieldDefinition.MakePath();
        }

        protected internal override DateTime ConvertToUniversalTime(DateTime date)
        {
            if (FieldDefinition is DateFieldDefinition dateField)
            {
                if (dateField.ConvertToLocalTime)
                {
                    date = date.ToUniversalTime();
                }
            }
            return base.ConvertToUniversalTime(date);
        }

        public override void SetTableDescription()
        {
            if (Path.IsNullOrEmpty())
            {
                base.SetTableDescription();
            }
            else if (BaseLookup != null)
            {
                var foundTreeItem = BaseLookup.AdvancedFindTree.ProcessFoundTreeViewItem(Path);
                if (foundTreeItem != null)
                {
                    if (foundTreeItem.Parent == null)
                    {
                        TableDescription = FieldDefinition.TableDefinition.Description;
                    }
                    else
                    {
                        TableDescription = foundTreeItem.Parent.Name;
                    }
                }
            }
        }

        public override Expression GetMauiFilter<TEntity>(ParameterExpression param)
        {
            if (FormulaToSearch != null)
            {
                return null;
            }

            DbDateTypes? dateType = null;
            if (FieldDefinition is DateFieldDefinition dateFieldDefinition)
            {
                dateType = dateFieldDefinition.DateType;
            }

            var stringValue = Value;
            var field = FieldDefinition;
            if (FieldToSearch != null)
            {
                field = FieldToSearch;
            }

            var value = stringValue.GetPropertyFilterValue(field.FieldDataType, field.FieldType);

            return GetBinaryExpression<TEntity>(param, PropertyName, Condition, field.FieldType, value);
        }
    }
}
