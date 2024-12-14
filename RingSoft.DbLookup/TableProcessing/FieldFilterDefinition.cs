// ***********************************************************************
// Assembly         : RingSoft.DbLookup
// Author           : petem
// Created          : 12-19-2022
//
// Last Modified By : petem
// Last Modified On : 05-28-2024
// ***********************************************************************
// <copyright file="FieldFilterDefinition.cs" company="Peter Ringering">
//     Copyright (c) 2023. All rights reserved.
// </copyright>
// <summary></summary>
// ***********************************************************************

using RingSoft.DataEntryControls.Engine;
using RingSoft.DbLookup.AdvancedFind;
using RingSoft.DbLookup.Lookup;
using RingSoft.DbLookup.ModelDefinition.FieldDefinitions;
using RingSoft.DbLookup.QueryBuilder;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq.Expressions;

namespace RingSoft.DbLookup.TableProcessing
{
    /// <summary>
    /// Enum DateFilterTypes
    /// </summary>
    public enum DateFilterTypes
    {
        /// <summary>
        /// The specific date
        /// </summary>
        [Description("Specific Date")]
        SpecificDate = 0,
        /// <summary>
        /// The days
        /// </summary>
        [Description("Day(s) Ago")]
        Days = 1,
        /// <summary>
        /// The weeks
        /// </summary>
        [Description("Week(s) Ago")]
        Weeks = 2,
        /// <summary>
        /// The months
        /// </summary>
        [Description("Month(s) Ago")]
        Months = 3,
        /// <summary>
        /// The years
        /// </summary>
        [Description("Year(s) Ago")]
        Years = 4,
        /// <summary>
        /// The hours
        /// </summary>
        [Description("Hour(s) Ago")]
        Hours = 5,
        /// <summary>
        /// The minutes
        /// </summary>
        [Description("Minute(s) Ago")]
        Minutes = 6,
        /// <summary>
        /// The seconds
        /// </summary>
        [Description("Second(s) Ago")]
        Seconds = 7,
    }

    /// <summary>
    /// Represents a filter field item in a table filter definition.
    /// </summary>
    public class FieldFilterDefinition : FilterItemType<FieldFilterDefinition>
    {
        /// <summary>
        /// Gets the type.
        /// </summary>
        /// <value>The type.</value>
        public override FilterItemTypes Type => FilterItemTypes.Field;


        /// <summary>
        /// Gets the field definition.
        /// </summary>
        /// <value>
        /// The field definition.
        /// </value>
        private FieldDefinition _fieldDefinition;

        /// <summary>
        /// Gets the field definition.
        /// </summary>
        /// <value>The field definition.</value>
        public FieldDefinition FieldDefinition
        {
            get => _fieldDefinition;
            internal set
            {
                _fieldDefinition = value;
                if (value.Description == "Name")
                {
                    
                }
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
        /// <value>The condition.</value>
        public Conditions Condition { get; set; }

        /// <summary>
        /// Gets a value indicating whether the search is case sensitive.
        /// </summary>
        /// <value>
        ///   <c>true</c> if case sensitive; otherwise, <c>false</c>.
        /// </value>
        private bool _caseSensitive;

        /// <summary>
        /// Gets a value indicating whether [case sensitive].
        /// </summary>
        /// <value><c>true</c> if [case sensitive]; otherwise, <c>false</c>.</value>
        /// <exception cref="System.ArgumentOutOfRangeException"></exception>
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
        /// <value><c>true</c> if cast enum value as int; otherwise, <c>false</c>.</value>
        public bool CastEnumValueAsInt { get; internal set; } = true;

        /// <summary>
        /// The parent field
        /// </summary>
        private FieldDefinition _parentField;

        /// <summary>
        /// Gets or sets the parent field.
        /// </summary>
        /// <value>The parent field.</value>
        public FieldDefinition ParentField
        {
            get => _parentField;
            set { _parentField = value; }
        }

        /// <summary>
        /// The field to search
        /// </summary>
        private FieldDefinition _fieldToSearch;

        /// <summary>
        /// Gets the field to search.
        /// </summary>
        /// <value>The field to search.</value>
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

        /// <summary>
        /// Gets the formula to search.
        /// </summary>
        /// <value>The formula to search.</value>
        public ILookupFormula FormulaToSearch { get; internal set; }

        /// <summary>
        /// Gets the base lookup.
        /// </summary>
        /// <value>The base lookup.</value>
        public LookupDefinitionBase BaseLookup { get; internal set; }

        /// <summary>
        /// Gets a value indicating whether this instance is primary key.
        /// </summary>
        /// <value><c>true</c> if this instance is primary key; otherwise, <c>false</c>.</value>
        public bool IsPrimaryKey { get; internal set; }

        /// <summary>
        /// Gets the navigation properties.
        /// </summary>
        /// <value>The navigation properties.</value>
        public List<JoinInfo> NavigationProperties { get; internal set; }

        /// <summary>
        /// Initializes a new instance of the <see cref="FieldFilterDefinition" /> class.
        /// </summary>
        /// <param name="tableFilterDefinition">The table filter definition.</param>
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

        /// <summary>
        /// Gets the type of the TreeView.
        /// </summary>
        /// <value>The type of the TreeView.</value>
        public override TreeViewType TreeViewType => TreeViewType.Field;

        /// <summary>
        /// Gets or sets the name of the set property.
        /// </summary>
        /// <value>The name of the set property.</value>
        public string SetPropertyName { get; set; }

        /// <summary>
        /// Gets the name of the property.
        /// </summary>
        /// <value>The name of the property.</value>
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

                    var result = JoinDefinition.GetPropertyJoinName(field.PropertyName);
                    return result;
                }

                return SetPropertyName;
            }

            internal set
            {
                SetPropertyName = value;
            }
        }

        /// <summary>
        /// Copies from.
        /// </summary>
        /// <param name="source">The source.</param>
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
            NavigationProperties = fieldFilterDefinition.NavigationProperties;

            base.CopyFrom(source);
        }

        /// <summary>
        /// Gets the report begin text print mode.
        /// </summary>
        /// <param name="lookupDefinition">The lookup definition.</param>
        /// <returns>System.String.</returns>
        protected internal override string GetReportBeginTextPrintMode(LookupDefinitionBase lookupDefinition)
        {
            var tableDescription = TableDescription;

            var result = $"{tableDescription}.{FieldDefinition.Description}";
            return result;
            //return base.GetReportBeginTextPrintMode(lookupDefinition);
        }

        /// <summary>
        /// Gets the report text.
        /// </summary>
        /// <param name="lookupDefinition">The lookup definition.</param>
        /// <param name="printMode">if set to <c>true</c> [print mode].</param>
        /// <returns>System.String.</returns>
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
                        //Peter Ringering - 12/10/2024 12:44:06 PM - E-62
                        if (FieldDefinition != null)
                        {
                            result += FieldDefinition.GetUserValue(Value, true);
                        }
                        else
                        {
                            result += Value;
                        }
                    }

                    break;
            }

            return result;
        }

        /// <summary>
        /// Loads from entity.
        /// </summary>
        /// <param name="entity">The entity.</param>
        /// <param name="lookupDefinition">The lookup definition.</param>
        /// <param name="path">The path.</param>
        /// <returns><c>true</c> if XXXX, <c>false</c> otherwise.</returns>
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
                        //SetFieldToDisplay();
                        ProcessFoundTreeItem(treeViewItem);
                        SetPropertyName = FieldDefinition.PropertyName;
                        if (JoinDefinition != null)
                        {
                            NavigationProperties = JoinDefinition.GetNavigationProperties(true);
                            if (FieldToSearch != null)
                            {
                                PropertyName = JoinDefinition.GetPropertyJoinName(FieldToSearch.PropertyName);
                            }
                            else
                            {
                                PropertyName = JoinDefinition.GetPropertyJoinName(FieldDefinition.PropertyName);
                            }
                        }
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
                if (DateFilterType == DateFilterTypes.SpecificDate)
                {
                    SetDateDisplayValue(Value);
                }
                else
                {
                    Value = ConvertDate(Value);
                }
            }
            return result;
        }

        /// <summary>
        /// Saves to entity.
        /// </summary>
        /// <param name="entity">The entity.</param>
        public override void SaveToEntity(AdvancedFindFilter entity)
        {
            entity.Operand = (byte)Condition;
            entity.TableName = FieldDefinition.TableDefinition.Description;
            entity.FieldName = FieldDefinition.Description;
            base.SaveToEntity(entity);
        }

        /// <summary>
        /// Loads from filter return.
        /// </summary>
        /// <param name="filterReturn">The filter return.</param>
        /// <param name="treeViewItem">The tree view item.</param>
        /// <returns>System.String.</returns>
        public override string LoadFromFilterReturn(AdvancedFilterReturn filterReturn, TreeViewItem treeViewItem)
        {
            Condition = filterReturn.Condition;
            Value = filterReturn.SearchValue;

            ProcessFoundTreeItem(treeViewItem);

            var result = base.LoadFromFilterReturn(filterReturn, treeViewItem);

            //if (DateFilterType != DateFilterTypes.SpecificDate)
            {
                Value = ConvertDate(Value);
            }
            //else
            //{
            //    Value = result;
            //}


            return result;
        }

        /// <summary>
        /// Processes the found tree item.
        /// </summary>
        /// <param name="treeViewItem">The tree view item.</param>
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

        /// <summary>
        /// Sets the join definition.
        /// </summary>
        /// <param name="treeViewItem">The tree view item.</param>
        /// <param name="fieldToSearch">The field to search.</param>
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

        /// <summary>
        /// Saves to filter return.
        /// </summary>
        /// <param name="filterReturn">The filter return.</param>
        public override void SaveToFilterReturn(AdvancedFilterReturn filterReturn)
        {
            filterReturn.Condition = Condition;
            filterReturn.FieldDefinition = FieldDefinition;
            filterReturn.SearchValue = Value;
            base.SaveToFilterReturn(filterReturn);
        }

        /// <summary>
        /// Returns a <see cref="System.String" /> that represents this instance.
        /// </summary>
        /// <returns>A <see cref="System.String" /> that represents this instance.</returns>
        public override string ToString()
        {
            var result = GetReportBeginTextPrintMode(null);

            result += $"{GetConditionText(Condition)} {Value}";

            return result;
        }

        /// <summary>
        /// Converts the date.
        /// </summary>
        /// <param name="value">The value.</param>
        /// <returns>System.String.</returns>
        protected internal override string ConvertDate(string value)
        {
            value = base.ConvertDate(value);
            if (FieldDefinition is DateFieldDefinition dateField)
            {
                if (dateField.ConvertToLocalTime || SystemGlobals.ConvertAllDatesToUniversalTime)
                {
                    var date = value.ToDate();
                    if (date != null)
                    {
                        date = date.Value.ToUniversalTime();
                        SetDateDisplayValue(date.Value.FormatDateValue(dateField.DateType));
                        return date.Value.FormatDateValue(dateField.DateType);
                    }
                }

            }

            return value;
        }

        /// <summary>
        /// Sets the date display value.
        /// </summary>
        /// <param name="value">The value.</param>
        public void SetDateDisplayValue(string value)
        {
            if (FieldDefinition is DateFieldDefinition dateField)
            {
                if (dateField.ConvertToLocalTime || SystemGlobals.ConvertAllDatesToUniversalTime)
                {
                    var date = value.ToDate();
                    if (date != null)
                    {
                        date = date.Value.ToLocalTime();
                        DisplayValue = date.Value.FormatDateValue(dateField.DateType);
                    }
                }

            }

        }

        /// <summary>
        /// Gets the new filter item definition.
        /// </summary>
        /// <returns>FilterItemDefinition.</returns>
        public override FilterItemDefinition GetNewFilterItemDefinition()
        {
            var result = new FieldFilterDefinition(TableFilterDefinition);
            
            return result;
        }

        /// <summary>
        /// Gets the new path.
        /// </summary>
        /// <returns>System.String.</returns>
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

        /// <summary>
        /// Converts to universal time.
        /// </summary>
        /// <param name="date">The date.</param>
        /// <returns>DateTime.</returns>
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

        /// <summary>
        /// Sets the table description.
        /// </summary>
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

        /// <summary>
        /// Gets the maui filter.
        /// </summary>
        /// <typeparam name="TEntity">The type of the t entity.</typeparam>
        /// <param name="param">The parameter.</param>
        /// <returns>Expression.</returns>
        public override Expression GetMauiFilter<TEntity>(ParameterExpression param)
        {
            if (FormulaToSearch != null)
            {
                return null;
            }

            Expression nullExpr = null;
            DbDateTypes? dateType = null;
            if (FieldDefinition is DateFieldDefinition dateFieldDefinition)
            {
                dateType = dateFieldDefinition.DateType;
            }

            if (IsNullableFilter() && Value.IsNullOrEmpty() && Condition != Conditions.NotEqualsNull)
            {
                var propertyName = PropertyName;
                if (LookupColumn != null)
                {
                    var useDbField = false;
                    if (FieldDefinition.AllowNulls)
                    {
                        useDbField = true;
                    }
                    propertyName = LookupColumn.GetPropertyJoinName(useDbField);
                }
                nullExpr = FilterItemDefinition
                    .GetBinaryExpression<TEntity>(param, propertyName, Conditions.EqualsNull
                        , FieldDefinition.FieldType);
                return nullExpr;
            }
            var stringValue = Value;
            var field = FieldDefinition;
            if (FieldToSearch != null)
            {
                field = FieldToSearch;
            }

            var test = this;
            var value = stringValue.GetPropertyFilterValue(field.FieldDataType, field.FieldType);
            
            var result = GetBinaryExpression<TEntity>(param, PropertyName, Condition, field.FieldType, value);
            return result;
        }

        /// <summary>
        /// Determines whether [is nullable filter].
        /// </summary>
        /// <returns><c>true</c> if [is nullable filter]; otherwise, <c>false</c>.</returns>
        public bool IsNullableFilter()
        {
            var result = FieldDefinition.AllowNulls;
            if (!result)
            {
                if (NavigationProperties != null)
                {
                    foreach (var property in NavigationProperties)
                    {
                        if (property.ParentJoin != null)
                        {
                            if (property
                                .ParentJoin
                                .ForeignKeyDefinition
                                .FieldJoins[0]
                                .ForeignField.AllowNulls)
                            {
                                return true;
                            }
                        }
                    }
                }
            }
            return result;
        }

        /// <summary>
        /// Determines whether [is null filter].
        /// </summary>
        /// <returns><c>true</c> if [is null filter]; otherwise, <c>false</c>.</returns>
        public bool IsNullFilter()
        {
            var result = false;
            if (IsNullableFilter() && Value.IsNullOrEmpty())
            {
                result = true;
            }
            return result;
        }

        /// <summary>
        /// Sets the field to display.
        /// </summary>
        /// <param name="condition">The condition.</param>
        public void SetFieldToDisplay(Conditions condition)
        {
            if (FieldDefinition.ParentJoinForeignKeyDefinition != null)
            {
                switch (condition)
                {
                    case Conditions.Equals:
                    case Conditions.EqualsNull:
                    case Conditions.NotEquals:
                    case Conditions.NotEqualsNull:
                        break;
                    default:
                        var primaryLookup = FieldDefinition
                            .ParentJoinForeignKeyDefinition
                            .PrimaryTable
                            .LookupDefinition;
                        if (primaryLookup != null)
                        {
                            var sortColumn = primaryLookup.InitialSortColumnDefinition;
                            if (sortColumn is LookupFieldColumnDefinition fieldColumn)
                            {
                                FieldToSearch = fieldColumn.FieldToDisplay;
                            }
                        }
                        break;
                }
            }
        }

    }
}
