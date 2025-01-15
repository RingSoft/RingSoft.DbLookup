// ***********************************************************************
// Assembly         : RingSoft.DbLookup
// Author           : petem
// Created          : 12-19-2022
//
// Last Modified By : petem
// Last Modified On : 09-03-2024
// ***********************************************************************
// <copyright file="FilterItemDefinition.cs" company="Peter Ringering">
//     Copyright (c) 2023. All rights reserved.
// </copyright>
// <summary></summary>
// ***********************************************************************
using RingSoft.DataEntryControls.Engine;
using RingSoft.DbLookup.AdvancedFind;
using RingSoft.DbLookup.Lookup;
using RingSoft.DbLookup.ModelDefinition;
using RingSoft.DbLookup.ModelDefinition.FieldDefinitions;
using RingSoft.DbLookup.QueryBuilder;
using System;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;

namespace RingSoft.DbLookup.TableProcessing
{
    /// <summary>
    /// Enum FilterItemTypes
    /// </summary>
    public enum FilterItemTypes
    {
        /// <summary>
        /// The field
        /// </summary>
        Field = 0,
        /// <summary>
        /// The formula
        /// </summary>
        Formula = 1,
        /// <summary>
        /// The advanced find
        /// </summary>
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
        /// <value>The type.</value>
        public abstract FilterItemTypes Type { get; }

        /// <summary>
        /// Gets the table filter definition.
        /// </summary>
        /// <value>The table filter definition.</value>
        public TableFilterDefinitionBase TableFilterDefinition { get; internal set; }

        /// <summary>
        /// Gets the left parentheses count.
        /// </summary>
        /// <value>The left parentheses count.</value>
        public int LeftParenthesesCount { get; set; }

        /// <summary>
        /// Gets the right parentheses count.
        /// </summary>
        /// <value>The right parentheses count.</value>
        public int RightParenthesesCount { get; set; }

        /// <summary>
        /// Gets the end logic.
        /// </summary>
        /// <value>The end logic.</value>
        public EndLogics EndLogic { get; set; }

        /// <summary>
        /// Gets the join definition.
        /// </summary>
        /// <value>
        /// The join definition.
        /// </value>

        private TableFieldJoinDefinition _joinDefinition;

        /// <summary>
        /// Gets or sets the join definition.
        /// </summary>
        /// <value>The join definition.</value>
        public TableFieldJoinDefinition JoinDefinition
        {
            get { return _joinDefinition; }
            set
            {
                _joinDefinition = value;
                SetTableDescription();
            }
        }


        /// <summary>
        /// Gets or sets the table description.
        /// </summary>
        /// <value>The table description.</value>
        public string TableDescription { get; set; }

        /// <summary>
        /// Gets or sets the field description.
        /// </summary>
        /// <value>The field description.</value>
        public string FieldDescription { get; set; }

        /// <summary>
        /// Gets or sets the report description.
        /// </summary>
        /// <value>The report description.</value>
        public string ReportDescription { get; set; }

        /// <summary>
        /// Gets the path.
        /// </summary>
        /// <value>The path.</value>
        public string Path { get; internal set; }

        /// <summary>
        /// Gets the type of the TreeView.
        /// </summary>
        /// <value>The type of the TreeView.</value>
        public abstract TreeViewType TreeViewType { get; }

        /// <summary>
        /// Gets the name of the property.
        /// </summary>
        /// <value>The name of the property.</value>
        public abstract string PropertyName { get; internal set; }

        /// <summary>
        /// Gets a value indicating whether this instance is fixed.
        /// </summary>
        /// <value><c>true</c> if this instance is fixed; otherwise, <c>false</c>.</value>
        public bool IsFixed { get; internal set; }

        /// <summary>
        /// Gets the value to filter.
        /// </summary>
        /// <value>The value.</value>
        public string Value { get; internal set; }

        /// <summary>
        /// The display value
        /// </summary>
        private string _displayValue;

        /// <summary>
        /// Gets or sets the display value.
        /// </summary>
        /// <value>The display value.</value>
        public string DisplayValue
        {
            get
            {
                if (_displayValue.IsNullOrEmpty())
                {
                    return Value;
                }
                return _displayValue;
            }
            set { _displayValue = value; }
        }



        /// <summary>
        /// Gets the type of the date filter.
        /// </summary>
        /// <value>The type of the date filter.</value>
        public DateFilterTypes DateFilterType { get; private set; }

        /// <summary>
        /// Gets the date filter value.
        /// </summary>
        /// <value>The date filter value.</value>
        public int DateFilterValue { get; private set; }

        /// <summary>
        /// Gets or sets the type of the value.
        /// </summary>
        /// <value>The type of the value.</value>
        public ValueTypes ValueType { get; set; }

        /// <summary>
        /// The date type
        /// </summary>
        private DbDateTypes? _dateType;

        /// <summary>
        /// Gets the type of the date.
        /// </summary>
        /// <value>The type of the date.</value>
        public DbDateTypes DateType
        {
            get
            {
                if (_dateType == null)
                {
                    if (SystemGlobals.ConvertAllDatesToUniversalTime)
                    {
                        return DbDateTypes.DateTime;
                    }
                    return DbDateTypes.DateOnly;
                }

                return _dateType.Value;
            }
            internal set => _dateType = value;
        }

        /// <summary>
        /// Gets or sets the lookup column.
        /// </summary>
        /// <value>The lookup column.</value>
        public LookupColumnDefinitionBase LookupColumn { get; set; }

        /// <summary>
        /// Gets a value indicating whether this <see cref="FilterItemDefinition" /> is processed.
        /// </summary>
        /// <value><c>true</c> if processed; otherwise, <c>false</c>.</value>
        public bool Processed { get; private set; }

        /// <summary>
        /// Initializes a new instance of the <see cref="FilterItemDefinition" /> class.
        /// </summary>
        /// <param name="tableFilterDefinition">The table filter definition.</param>
        public FilterItemDefinition(TableFilterDefinitionBase tableFilterDefinition)
        {
            TableFilterDefinition = tableFilterDefinition;
            TableDescription = tableFilterDefinition.TableDefinition.Description;
        }

        /// <summary>
        /// Gets the property value.
        /// </summary>
        /// <typeparam name="TEntity">The type of the t entity.</typeparam>
        /// <param name="entity">The entity.</param>
        /// <returns>System.String.</returns>
        public string GetPropertyValue<TEntity>(TEntity entity) where TEntity : class, new()
        {
            if (LookupColumn != null)
            {
                return LookupColumn.GetDatabaseValue(entity);
            }
            if (PropertyName.IsNullOrEmpty())
            {
                return GblMethods.GetPropertyValue(entity, PropertyName);
            }

            return Value;
        }

        /// <summary>
        /// Copies from.
        /// </summary>
        /// <param name="source">The source.</param>
        internal virtual void CopyFrom(FilterItemDefinition source)
        {
            LeftParenthesesCount = source.LeftParenthesesCount;
            RightParenthesesCount = source.RightParenthesesCount;
            TableFilterDefinition = source.TableFilterDefinition;
            JoinDefinition = source.JoinDefinition;
            EndLogic = source.EndLogic;
            TableDescription = source.TableDescription;
            ReportDescription = source.ReportDescription;
            ValueType = source.ValueType;
            DateFilterType = source.DateFilterType;
            DateFilterValue = source.DateFilterValue;
            IsFixed = source.IsFixed;
            LookupColumn = source.LookupColumn;
        }

        /// <summary>
        /// Sets the table description.
        /// </summary>
        public virtual void SetTableDescription()
        {
            if (JoinDefinition != null && JoinDefinition.ForeignKeyDefinition != null)
            {
                TableDescription = JoinDefinition.ForeignKeyDefinition.FieldJoins[0].ForeignField.Description;
            }

        }


        /// <summary>
        /// Gets the report text.
        /// </summary>
        /// <param name="lookupDefinition">The lookup definition.</param>
        /// <param name="printMode">if set to <c>true</c> [print mode].</param>
        /// <returns>System.String.</returns>
        public abstract string GetReportText(LookupDefinitionBase lookupDefinition, bool printMode);

        /// <summary>
        /// Gets the print text.
        /// </summary>
        /// <param name="lookupDefinition">The lookup definition.</param>
        /// <returns>System.String.</returns>
        public string GetPrintText(LookupDefinitionBase lookupDefinition)
        {
            var lParen = GblMethods.StringDuplicate("(", LeftParenthesesCount);
            var result = GetReportText(lookupDefinition, true);
            var rParen = GblMethods.StringDuplicate(")", RightParenthesesCount);
            result = lParen + result + rParen;

            return result;
        }

        /// <summary>
        /// Prints the end logic text.
        /// </summary>
        /// <returns>System.String.</returns>
        public string PrintEndLogicText()
        {
            var enumTranslation = new EnumFieldTranslation();
            enumTranslation.LoadFromEnum<EndLogics>();

            var result = " " + enumTranslation.TypeTranslations
                .FirstOrDefault(p => p.NumericValue == (int)EndLogic).TextValue + "\r\n";

            return result;
        }

        /// <summary>
        /// Gets the date report text.
        /// </summary>
        /// <returns>System.String.</returns>
        public string GetDateReportText()
        {
            var result = string.Empty;
            if (ValueType == ValueTypes.DateTime)
            {
                if (DateFilterType == DateFilterTypes.SpecificDate)
                {
                    var date = DisplayValue.ToDate();
                    if (date.HasValue)
                    {
                        return date.Value.FormatDateValue(DateType, false);
                    }
                }
                var enumTrans = new EnumFieldTranslation();
                enumTrans.LoadFromEnum<DateFilterTypes>();
                var typeTranslation = enumTrans.TypeTranslations.FirstOrDefault(p => p.NumericValue == (int)DateFilterType);
                if (typeTranslation != null)
                {
                    return $"{DateFilterValue} {typeTranslation.TextValue}";
                }
            }
            return result;
        }

        /// <summary>
        /// Gets the condition text.
        /// </summary>
        /// <param name="condition">The condition.</param>
        /// <returns>System.String.</returns>
        /// <exception cref="System.ArgumentOutOfRangeException"></exception>
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

        /// <summary>
        /// Loads from entity.
        /// </summary>
        /// <param name="entity">The entity.</param>
        /// <param name="lookupDefinition">The lookup definition.</param>
        /// <param name="path">The path.</param>
        /// <returns><c>true</c> if XXXX, <c>false</c> otherwise.</returns>
        public virtual bool LoadFromEntity(AdvancedFindFilter entity, LookupDefinitionBase lookupDefinition, string path = "")
        {
            LeftParenthesesCount = entity.LeftParentheses;
            RightParenthesesCount = entity.RightParentheses;
            EndLogic = (EndLogics)entity.EndLogic;
            DateFilterType = (DateFilterTypes)entity.DateFilterType;
            Value = entity.SearchForValue;
            if (ValueType == ValueTypes.DateTime && DateFilterType != DateFilterTypes.SpecificDate)
            {
                DateFilterValue = Value.ToInt();
            }
            Path = entity.Path;
            //var process = false;
            //if (entity.Path.IsNullOrEmpty())
            //{
            //    process = true;
            //}
            //else
            //{
            //    var newPath = entity.Path;
            //    if (entity.Path != path)
            //    {
            //        newPath = path + entity.Path;
            //    }
            //    var treeViewItem =
            //        lookupDefinition.AdvancedFindTree.ProcessFoundTreeViewItem(newPath, TreeViewType);
            //    process = treeViewItem != null;
            //}
            //if (!process)
            //{
            //    var message = "This advanced find is corrupt. Please delete it.";
            //    var caption = "Corrupt Advanced Find.";
            //    ControlsGlobals.UserInterface.ShowMessageBox(message, caption, RsMessageBoxIcons.Error);
            //}
            return true;
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

        /// <summary>
        /// Saves to entity.
        /// </summary>
        /// <param name="entity">The entity.</param>
        public virtual void SaveToEntity(AdvancedFindFilter entity)
        {
            entity.Path = Path;
            entity.LeftParentheses = (byte)LeftParenthesesCount;
            entity.RightParentheses = (byte)RightParenthesesCount;
            entity.EndLogic = (byte)EndLogic;
            entity.DateFilterType = (byte)DateFilterType;
            if (DateFilterType != DateFilterTypes.SpecificDate)
            {
                entity.SearchForValue = DateFilterValue.ToString();
            }
            else
            {
                entity.SearchForValue = DisplayValue;
            }
        }

        /// <summary>
        /// Loads from filter return.
        /// </summary>
        /// <param name="filterReturn">The filter return.</param>
        /// <param name="treeViewItem">The tree view item.</param>
        /// <returns>System.String.</returns>
        public virtual string LoadFromFilterReturn(AdvancedFilterReturn filterReturn, TreeViewItem treeViewItem)
        {
            var searchValue = filterReturn.SearchValue;
            DateFilterType = filterReturn.DateFilterType;

            if (ValueType == ValueTypes.DateTime && DateFilterType != DateFilterTypes.SpecificDate)
            {
                DateFilterValue = searchValue.ToInt();
            }
            //searchValue = GetSearchValue(searchValue);

            return searchValue;

        }

        /// <summary>
        /// Converts to universal time.
        /// </summary>
        /// <param name="date">The date.</param>
        /// <returns>DateTime.</returns>
        protected internal virtual DateTime ConvertToUniversalTime(DateTime date)
        {
            return date;
        }

        /// <summary>
        /// Gets the search value.
        /// </summary>
        /// <param name="searchValue">The search value.</param>
        /// <returns>System.String.</returns>
        public virtual string GetSearchValue(string searchValue)
        {
            if (ValueType == ValueTypes.DateTime)
            {
                DisplayValue = searchValue;
                searchValue = ConvertDate(searchValue);
                if (SystemGlobals.ConvertAllDatesToUniversalTime)
                {
                    var date = searchValue.ToDate();
                    if (date != null)
                    {
                        date = ConvertToUniversalTime(date.Value);
                        searchValue = date.Value.FormatDateValue(DbDateTypes.Millisecond);
                    }

                }

            }

            return searchValue;
        }

        /// <summary>
        /// Saves to filter return.
        /// </summary>
        /// <param name="filterReturn">The filter return.</param>
        public virtual void SaveToFilterReturn(AdvancedFilterReturn filterReturn)
        {
            filterReturn.Path = Path;
            filterReturn.DateFilterType = DateFilterType;
            if (DateFilterType != DateFilterTypes.SpecificDate)
            {
                filterReturn.SearchValue = DateFilterValue.ToString();
            }
        }

        /// <summary>
        /// Gets the table field for filter.
        /// </summary>
        /// <param name="lookupDefinition">The lookup definition.</param>
        /// <param name="entity">The entity.</param>
        /// <param name="fieldDefinition">The field definition.</param>
        /// <param name="filterField">The filter field.</param>
        /// <returns>TableDefinitionBase.</returns>
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

        /// <summary>
        /// Converts the date.
        /// </summary>
        /// <param name="value">The value.</param>
        /// <returns>System.String.</returns>
        protected internal virtual string ConvertDate(string value)
        {
            switch (DateFilterType)
            {
                case DateFilterTypes.SpecificDate:
                    break;
                default:
                    DateFilterValue = value.ToInt();
                    var result = LookupDefinitionBase.ProcessSearchValue(value, DateFilterType);
                    return result;
            }
            return value;
        }

        /// <summary>
        /// Gets the report begin text print mode.
        /// </summary>
        /// <param name="lookupDefinition">The lookup definition.</param>
        /// <returns>System.String.</returns>
        protected internal virtual string GetReportBeginTextPrintMode(LookupDefinitionBase lookupDefinition)
        {
            var result = string.Empty;
            TreeViewItem foundItem = null;

            if (!Path.IsNullOrEmpty())
            {
                foundItem = lookupDefinition.AdvancedFindTree.ProcessFoundTreeViewItem(Path, TreeViewType);
            }

            if (foundItem != null)
            {
                if (foundItem.Parent != null)
                {
                    result += $"{foundItem.Parent.Name}.";
                }
                else
                {
                    result += $"{foundItem.FieldDefinition.TableDefinition.Description}.";
                }
                result += $"{foundItem.Name} ";
            }
            return result;
        }

        /// <summary>
        /// Gets the new filter item definition.
        /// </summary>
        /// <returns>FilterItemDefinition.</returns>
        public abstract FilterItemDefinition GetNewFilterItemDefinition();

        /// <summary>
        /// Gets the new path.
        /// </summary>
        /// <returns>System.String.</returns>
        internal abstract string GetNewPath();

        /// <summary>
        /// Copies to new filter.
        /// </summary>
        /// <param name="lookupDefinition">The lookup definition.</param>
        public void CopyToNewFilter(LookupDefinitionBase lookupDefinition)
        {
            var newFilter = GetNewFilterItemDefinition();
            if (newFilter != null)
            {
                newFilter.CopyFrom(this);
                lookupDefinition.FilterDefinition.AddFixedFilter(newFilter);
                if (Path.IsNullOrEmpty())
                {
                    //if (JoinDefinition != null)
                    //{
                    //    TableDescription = JoinDefinition.ForeignKeyDefinition.FieldJoins[0].ForeignField.Description;
                    //}
                    Path = GetNewPath();
                }
                GetJoins(lookupDefinition, newFilter);
            }
        }

        /// <summary>
        /// Gets the joins.
        /// </summary>
        /// <param name="lookupDefinition">The lookup definition.</param>
        /// <param name="newFilter">The new filter.</param>
        private void GetJoins(LookupDefinitionBase lookupDefinition, FilterItemDefinition newFilter)
        {
            var foundItem = lookupDefinition.AdvancedFindTree.ProcessFoundTreeViewItem(Path);
            if (foundItem != null)
            {
                var index = lookupDefinition.FilterDefinition.Joins.Count;
                var join = lookupDefinition.AdvancedFindTree.MakeIncludes(foundItem).LookupJoin;
                if (join != null)
                {
                    newFilter.JoinDefinition = join.JoinDefinition;
                }

                //TableDescription = foundItem.Name;
            }
        }

        /// <summary>
        /// Gets the maui filter.
        /// </summary>
        /// <typeparam name="TEntity">The type of the t entity.</typeparam>
        /// <param name="param">The parameter.</param>
        /// <returns>Expression.</returns>
        public virtual Expression GetMauiFilter<TEntity>(ParameterExpression param)
        {
            Processed = true;
            return null;
        }

        /// <summary>
        /// Gets the string expression.
        /// </summary>
        /// <typeparam name="TEntity">The type of the t entity.</typeparam>
        /// <param name="param">The parameter.</param>
        /// <param name="property">The property.</param>
        /// <param name="condition">The condition.</param>
        /// <param name="value">The value.</param>
        /// <returns>Expression.</returns>
        /// <exception cref="System.ArgumentOutOfRangeException">condition - null</exception>
        public static Expression GetStringExpression<TEntity>(
            ParameterExpression param, string property, Conditions condition, string value)
        {
            var newWhereMethod = GetWhereMethod<TEntity>();
            var returnExpression = GetPropertyExpression(property, param);
            Expression<Func<string>> idLambda = () => value;

            MethodInfo callMethod = null;
            Expression callExpr = null;

            Expression result = null;
            Expression expr = param;

            string[] props = property.Split('.');
            System.Type type = typeof(TEntity);
            PropertyInfo pi = null;

            var lastType = type;
            foreach (string prop in props)
            {
                // use reflection (not ComponentModel) to mirror LINQ
                pi = type.GetProperty(prop);
                if (props.LastOrDefault() != prop)
                {
                    expr = Expression.Property(expr, pi);
                }
                type = pi.PropertyType;
            }

            MemberExpression m = null;
            ConstantExpression c = null;
            MethodInfo mi = null;
            Expression call = null;

            switch (condition)
            {
                case Conditions.Equals:
                case Conditions.NotEquals:
                case Conditions.GreaterThan:
                case Conditions.GreaterThanEquals:
                case Conditions.LessThan:
                case Conditions.LessThanEquals:
                    callMethod = typeof(string).GetMethod("CompareTo"
                        , new[] { typeof(string) });
                    callExpr = Expression.Call(returnExpression, callMethod, idLambda.Body);
                    result = GetBinaryExpression(callExpr, condition, Expression.Constant(0));
                    break;
                case Conditions.Contains:
                    m = Expression.MakeMemberAccess(expr, pi);
                    c = Expression.Constant(value, typeof(string));
                    mi = typeof(string).GetMethod("Contains"
                        , new System.Type[] { typeof(string) });
                    call = Expression.Call(m, mi, c);
                    result = call;
                    break;
                case Conditions.NotContains:
                    m = Expression.MakeMemberAccess(expr, pi);
                    c = Expression.Constant(value, typeof(string));
                    mi = typeof(string).GetMethod("Contains", new System.Type[] { typeof(string) });
                    call = Expression.Call(m, mi, c);
                    var not = Expression.Not(call);
                    result = not;
                    break;
                case Conditions.BeginsWith:
                    m = Expression.MakeMemberAccess(expr, pi);
                    c = Expression.Constant(value, typeof(string));
                    mi = typeof(string).GetMethod("StartsWith", new System.Type[] { typeof(string) });
                    call = Expression.Call(m, mi, c);
                    result = call;
                    break;
                case Conditions.EndsWith:
                    m = Expression.MakeMemberAccess(expr, pi);
                    c = Expression.Constant(value, typeof(string));
                    mi = typeof(string).GetMethod("EndsWith", new System.Type[] { typeof(string) });
                    call = Expression.Call(m, mi, c);
                    result = call;

                    break;
                default:
                    throw new ArgumentOutOfRangeException(nameof(condition), condition, null);
            }

            return result;
        }

        /// <summary>
        /// Gets the binary expression.
        /// </summary>
        /// <param name="returnExpression">The return expression.</param>
        /// <param name="condition">The condition.</param>
        /// <param name="constantExpression">The constant expression.</param>
        /// <returns>BinaryExpression.</returns>
        /// <exception cref="System.ArgumentOutOfRangeException">condition - null</exception>
        public static BinaryExpression GetBinaryExpression(Expression returnExpression, Conditions condition,
            ConstantExpression constantExpression)
        {
            BinaryExpression result = null;
            switch (condition)
            {
                case Conditions.Equals:
                    result = Expression.Equal(returnExpression, constantExpression);
                    break;
                case Conditions.NotEquals:
                    result = Expression.NotEqual(returnExpression, constantExpression);
                    break;
                case Conditions.GreaterThan:
                    result = Expression.GreaterThan(returnExpression, constantExpression);
                    break;
                case Conditions.GreaterThanEquals:
                    result = Expression.GreaterThanOrEqual(returnExpression, constantExpression);
                    break;
                case Conditions.LessThan:
                    result = Expression.LessThan(returnExpression, constantExpression);
                    break;
                case Conditions.LessThanEquals:
                    result = Expression.LessThanOrEqual(returnExpression, constantExpression);
                    break;
                case Conditions.Contains:
                    break;
                case Conditions.NotContains:
                    break;
                case Conditions.EqualsNull:
                    break;
                case Conditions.NotEqualsNull:
                    break;
                case Conditions.BeginsWith:
                    break;
                case Conditions.EndsWith:
                    break;
                default:
                    throw new ArgumentOutOfRangeException(nameof(condition), condition, null);
            }
            return result;
        }

        /// <summary>
        /// Gets the binary expression.
        /// </summary>
        /// <typeparam name="TEntity">The type of the t entity.</typeparam>
        /// <param name="param">The parameter.</param>
        /// <param name="property">The property.</param>
        /// <param name="condition">The condition.</param>
        /// <param name="fieldType">Type of the field.</param>
        /// <param name="value">The value.</param>
        /// <returns>Expression.</returns>
        /// <exception cref="System.ArgumentNullException">Value cannot be null.</exception>
        /// <exception cref="System.ArgumentOutOfRangeException">condition - null</exception>
        public static Expression GetBinaryExpression<TEntity>(ParameterExpression param, string property, Conditions condition, System.Type fieldType, object value = null)
        {
            Expression result = null;
            var returnExpression = GetPropertyExpression(property, param);

            ConstantExpression expressionValue = null;

            Expression expr = param;

            string[] props = property.Split('.');
            System.Type type = typeof(TEntity);
            PropertyInfo pi = null;

            var lastType = type;
            foreach (string prop in props)
            {
                // use reflection (not ComponentModel) to mirror LINQ
                pi = type.GetProperty(prop);
                if (props.LastOrDefault() != prop)
                {
                    expr = Expression.Property(expr, pi);
                }
                type = pi.PropertyType;
            }
            Expression propertyAccess = Expression.Property(expr, pi);
            var propType = GblMethods.GetPropertyType<TEntity>(property);

            switch (condition)
            {
                case Conditions.Equals:
                case Conditions.NotEquals:
                case Conditions.GreaterThan:
                case Conditions.GreaterThanEquals:
                case Conditions.LessThan:
                case Conditions.LessThanEquals:
                case Conditions.Contains:
                case Conditions.NotContains:
                case Conditions.BeginsWith:
                case Conditions.EndsWith:
                    if (value == null)
                    {
                        throw new ArgumentNullException("Value cannot be null.");
                    }

                    if (fieldType.IsEnum)
                    {
                        fieldType = typeof(int);
                    }
                    expressionValue = Expression.Constant(value, fieldType);
                    if (fieldType == typeof(string))
                    {
                        return GetStringExpression<TEntity>(param, property, condition, value.ToString());
                    }
                    else
                    {
                        result = GetBinaryExpression(returnExpression, condition, expressionValue);
                    }
                    break;
                case Conditions.EqualsNull:
                    if (!type.IsNullable())
                    {
                        return null;
                    }
                    result = Expression.Equal(propertyAccess, Expression.Constant(null, type));
                    break;
                case Conditions.NotEqualsNull:
                    if (!type.IsNullable())
                    {
                        return null;
                    }
                    result = Expression.NotEqual(propertyAccess, Expression.Constant(null, type));
                    break;
                default:
                    throw new ArgumentOutOfRangeException(nameof(condition), condition, null);
            }

            return result;
        }

        /// <summary>
        /// Creates the null propagation expression.
        /// </summary>
        /// <param name="o">The o.</param>
        /// <param name="property">The property.</param>
        /// <returns>Expression.</returns>
        public static Expression CreateNullPropagationExpression(Expression o, string property)
        {
            Expression propertyAccess = Expression.Property(o, property);

            var propertyType = propertyAccess.Type;

            if (propertyType.IsValueType && Nullable.GetUnderlyingType(propertyType) == null)
                propertyAccess = Expression.Convert(
                    propertyAccess, typeof(Nullable<>).MakeGenericType(propertyType));

            var nullResult = Expression.Default(propertyAccess.Type);

            var condition = Expression.Equal(o, Expression.Constant(null, o.Type));

            return Expression.Condition(condition, nullResult, propertyAccess);
        }

        /// <summary>
        /// Gets the where method.
        /// </summary>
        /// <typeparam name="TEntity">The type of the t entity.</typeparam>
        /// <returns>MethodInfo.</returns>
        public static MethodInfo GetWhereMethod<TEntity>()
        {
            var methods = typeof(Queryable).GetMethods();
            var whereMethod = methods
                .Where(method => method.Name == "Where"
                                 && method.IsGenericMethodDefinition
                                 && method.GetGenericArguments().Length == 1
                                 && method.GetParameters().Length == 2)
                .FirstOrDefault();
            var newWhereMethod = whereMethod.MakeGenericMethod(typeof(TEntity));
            return newWhereMethod;
        }


        /// <summary>
        /// Gets the property expression.
        /// </summary>
        /// <param name="property">The property.</param>
        /// <param name="param">The parameter.</param>
        /// <returns>Expression.</returns>
        public static Expression GetPropertyExpression(string property, ParameterExpression param)
        {
            var first = true;
            Expression returnExpression = null;
            var properties = property.Split('.');
            foreach (var newProperty in properties)
            {
                if (first)
                {
                    first = false;
                    returnExpression = Expression.Property(param, newProperty);
                }
                else
                {
                    returnExpression = Expression.Property(returnExpression, newProperty);
                }
            }

            return returnExpression;
        }

        /// <summary>
        /// Filters the query.
        /// </summary>
        /// <typeparam name="TEntity">The type of the t entity.</typeparam>
        /// <param name="source">The source.</param>
        /// <param name="param">The parameter.</param>
        /// <param name="expression">The expression.</param>
        /// <returns>IQueryable&lt;TEntity&gt;.</returns>
        public static IQueryable<TEntity> FilterQuery<TEntity>(IQueryable<TEntity> source, ParameterExpression param, Expression expression)
        {
            if (expression == null)
            {
                return source;
            }

            var origCursor = ControlsGlobals.UserInterface.GetWindowCursor();
            ControlsGlobals.UserInterface.SetWindowCursor(WindowCursorTypes.Wait);

            var whereLambda = Expression.Lambda<Func<TEntity, bool>>(expression, param);

            var whereMethod = GetWhereMethod<TEntity>();

            object whereResult = whereMethod
                .Invoke(null, new object[] { source, whereLambda });
            var whereQueryable = (IQueryable<TEntity>)whereResult;

            ControlsGlobals.UserInterface.SetWindowCursor(origCursor);
            return whereQueryable;
        }

        /// <summary>
        /// Appends the expression.
        /// </summary>
        /// <param name="left">The left.</param>
        /// <param name="right">The right.</param>
        /// <param name="endLogic">The end logic.</param>
        /// <returns>Expression.</returns>
        /// <exception cref="System.ArgumentOutOfRangeException">endLogic - null</exception>
        public static Expression AppendExpression(Expression left, Expression right, EndLogics endLogic)
        {
            var result = left;

            if (right == null)
            {
                return left;
            }

            if (left == null)
            {
                return right;
            }
            switch (endLogic)
            {
                case EndLogics.And:
                    result = Expression.AndAlso(left, right);
                    break;
                case EndLogics.Or:
                    result = Expression.OrElse(left, right);
                    break;
                default:
                    throw new ArgumentOutOfRangeException(nameof(endLogic), endLogic, null);
            }

            return result;
        }
    }
}
