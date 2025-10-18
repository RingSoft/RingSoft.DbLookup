// ***********************************************************************
// Assembly         : RingSoft.DbLookup
// Author           : petem
// Created          : 12-19-2022
//
// Last Modified By : petem
// Last Modified On : 08-30-2024
// ***********************************************************************
// <copyright file="ExtensionMethods.cs" company="Peter Ringering">
//     Copyright (c) 2023. All rights reserved.
// </copyright>
// <summary></summary>
// ***********************************************************************
using RingSoft.DataEntryControls.Engine;
using RingSoft.DbLookup.ModelDefinition.FieldDefinitions;
using RingSoft.DbLookup.QueryBuilder;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Linq.Expressions;
using System.Runtime.CompilerServices;
using RingSoft.DbLookup.AutoFill;
using RingSoft.DbLookup.ModelDefinition;
using RingSoft.DbLookup.DataProcessor;
using RingSoft.DbLookup.Lookup;
using RingSoft.DbLookup.TableProcessing;

namespace RingSoft.DbLookup
{
    /// <summary>
    /// Class JoinInfo.
    /// </summary>
    /// <font color="red">Badly formed XML comment.</font>
    public class JoinInfo
    {
        /// <summary>
        /// Gets or sets the parent join.
        /// </summary>
        /// <value>The parent join.</value>
        public TableFieldJoinDefinition ParentJoin { get; set; }

        /// <summary>
        /// Gets or sets the child join.
        /// </summary>
        /// <value>The child join.</value>
        public TableFieldJoinDefinition ChildJoin { get; set; }
    }

    /// <summary>
    /// Class ExtensionMethods.
    /// </summary>
    public static class ExtensionMethods
    {

        /// <summary>
        /// Trims the right of a string.
        /// </summary>
        /// <param name="value">The value.</param>
        /// <param name="trimChars">The trim chars.</param>
        /// <returns>System.String.</returns>
        public static string TrimRight(this string value, string trimChars)
        {
            return value.LeftStr(value.Length - trimChars.Length);
        }

        #region Property Name

        /// <summary>
        /// Gets the full name of the property. (u =&gt; u.UserId returns "UserId")
        /// </summary>
        /// <typeparam name="T">The first parameter of the Func.</typeparam>
        /// <typeparam name="TProperty">The type of the property.</typeparam>
        /// <param name="exp">The expression.</param>
        /// <returns>The full name of the property.</returns>
        public static string GetFullPropertyName<T, TProperty>
            (this Expression<Func<T, TProperty>> exp)
        {
            MemberExpression memberExp;
            if (!TryFindMemberExpression(exp.Body, out memberExp))
                return string.Empty;

            var memberNames = new Stack<string>();
            do
            {
                memberNames.Push(memberExp.Member.Name);
            } while (TryFindMemberExpression(memberExp.Expression, out memberExp));

            return string.Join(".", memberNames.ToArray());
        }

        // code adjusted to prevent horizontal overflow
        /// <summary>
        /// Tries the find member expression.
        /// </summary>
        /// <param name="exp">The exp.</param>
        /// <param name="memberExp">The member exp.</param>
        /// <returns><c>true</c> if XXXX, <c>false</c> otherwise.</returns>
        private static bool TryFindMemberExpression
            (Expression exp, out MemberExpression memberExp)
        {
            memberExp = exp as MemberExpression;
            if (memberExp != null)
            {
                // heyo! that was easy enough
                return true;
            }

            // if the compiler created an automatic conversion,
            // it'll look something like...
            // obj => Convert(obj.Property) [e.g., int -> object]
            // OR:
            // obj => ConvertChecked(obj.Property) [e.g., int -> long]
            // ...which are the cases checked in IsConversion
            if (IsConversion(exp) && exp is UnaryExpression)
            {
                memberExp = ((UnaryExpression)exp).Operand as MemberExpression;
                if (memberExp != null)
                {
                    return true;
                }
            }

            return false;
        }

        /// <summary>
        /// Determines whether the specified exp is conversion.
        /// </summary>
        /// <param name="exp">The exp.</param>
        /// <returns><c>true</c> if the specified exp is conversion; otherwise, <c>false</c>.</returns>
        private static bool IsConversion(Expression exp)
        {
            return (
                exp.NodeType == ExpressionType.Convert ||
                exp.NodeType == ExpressionType.ConvertChecked
                );
        }

        #endregion

        /// <summary>
        /// Converts the type of the database date type to date format.
        /// </summary>
        /// <param name="dbDateType">Type of the database date.</param>
        /// <returns>DateFormatTypes.</returns>
        /// <exception cref="System.ArgumentOutOfRangeException">dbDateType - null</exception>
        public static DateFormatTypes ConvertDbDateTypeToDateFormatType(this DbDateTypes dbDateType)
        {
            switch (dbDateType)
            {
                case DbDateTypes.DateOnly:
                    return DateFormatTypes.DateOnly;
                case DbDateTypes.DateTime:
                case DbDateTypes.Millisecond:
                    return DateFormatTypes.DateTime;
                default:
                    throw new ArgumentOutOfRangeException(nameof(dbDateType), dbDateType, null);
            }
        }

        /// <summary>
        /// Converts the type of the decimal field type to decimal edit format.
        /// </summary>
        /// <param name="decimalFieldType">Type of the decimal field.</param>
        /// <returns>DecimalEditFormatTypes.</returns>
        /// <exception cref="System.ArgumentOutOfRangeException">decimalFieldType - null</exception>
        public static DecimalEditFormatTypes ConvertDecimalFieldTypeToDecimalEditFormatType(
            this DecimalFieldTypes decimalFieldType)
        {
            switch (decimalFieldType)
            {
                case DecimalFieldTypes.Decimal:
                    return DecimalEditFormatTypes.Number;
                case DecimalFieldTypes.Currency:
                    return DecimalEditFormatTypes.Currency;
                case DecimalFieldTypes.Percent:
                    return DecimalEditFormatTypes.Percent;
                default:
                    throw new ArgumentOutOfRangeException(nameof(decimalFieldType), decimalFieldType, null);
            }
        }

        /// <summary>
        /// Returns true if Primary Key Value is valid.
        /// </summary>
        /// <param name="pkValue">The pk value.</param>
        /// <returns><c>true</c> if the specified pk value is valid; otherwise, <c>false</c>.</returns>
        public static bool IsValid(this PrimaryKeyValue pkValue)
        {
            if (pkValue == null)
            {
                return false;
            }

            return pkValue.IntIsValid;
        }
        /// <summary>
        /// Returns true if Auto Fill Value is valid.
        /// </summary>
        /// <param name="autoFillValue">The automatic fill value.</param>
        /// <returns><c>true</c> if the specified automatic fill value is valid; otherwise, <c>false</c>.</returns>
        public static bool IsValid(this AutoFillValue autoFillValue, bool checkDb = false)
        {
            if (autoFillValue == null)
            {
                return false;
            }
            if (autoFillValue.PrimaryKeyValue.IsValid())
            {
                if (checkDb)
                {
                    return autoFillValue.PrimaryKeyValue.IsValidDb();
                }

                return true;
            }

            return false;
        }

        /// <summary>
        /// Converts the property name to description.
        /// </summary>
        /// <param name="propertyName">Name of the property.</param>
        /// <returns>System.String.</returns>
        public static string ConvertPropertyNameToDescription(this string propertyName)
        {
            var newDescription = string.Empty;
            var index = 0;
            var propertyNameCharArry = propertyName.ToCharArray();
            var previousChar = ' ';
            foreach (var c in propertyNameCharArry)
            {
                newDescription += c;
                if (char.IsUpper(c) && index > 0)
                {
                    var addSpace = !char.IsUpper(previousChar);

                    if (addSpace)
                    {
                        if (newDescription[index - 1] != ' ')
                        {
                            newDescription = newDescription.Insert(index, " ");
                        }

                        index++;
                    }
                }
                previousChar = c;
                index++;
            }

            return newDescription;
        }

        /// <summary>
        /// Converts the type of the field type into value.
        /// </summary>
        /// <param name="fieldDataType">Type of the field data.</param>
        /// <returns>ValueTypes.</returns>
        /// <exception cref="System.ArgumentOutOfRangeException"></exception>
        public static ValueTypes ConvertFieldTypeIntoValueType(this FieldDataTypes fieldDataType)
        {
            var valueType = ValueTypes.String;
            switch (fieldDataType)
            {
                case FieldDataTypes.String:
                    valueType = ValueTypes.String;
                    break;
                case FieldDataTypes.Integer:
                case FieldDataTypes.Decimal:
                    valueType = ValueTypes.Numeric;
                    break;
                case FieldDataTypes.DateTime:
                    valueType = ValueTypes.DateTime;
                    break;
                case FieldDataTypes.Bool:
                    valueType = ValueTypes.Bool;
                    break;
                case FieldDataTypes.Memo:
                    valueType = ValueTypes.Memo;
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }

            return valueType;

        }

        /// <summary>
        /// Gets the automatic fill value.
        /// </summary>
        /// <param name="tableDefinition">The table definition.</param>
        /// <param name="primaryKeyString">The primary key string.</param>
        /// <returns>AutoFillValue.</returns>
        public static AutoFillValue GetAutoFillValue(this TableDefinitionBase tableDefinition, string primaryKeyString)
        {
            return tableDefinition.Context.OnAutoFillTextRequest(tableDefinition, primaryKeyString);
        }

        /// <summary>
        /// Gets the entity.
        /// </summary>
        /// <typeparam name="TEntity">The type of the t entity.</typeparam>
        /// <param name="autoFillValue">The automatic fill value.</param>
        /// <returns>TEntity.</returns>
        public static TEntity GetEntity<TEntity>(this AutoFillValue autoFillValue)
            where TEntity : class, new()
        {
            if (!autoFillValue.IsValid())
            {
                return new TEntity();
            }
            var table = autoFillValue.PrimaryKeyValue.TableDefinition;
            if (table is TableDefinition<TEntity> fullTable)
            {
                return GetEntity<TEntity>(autoFillValue, fullTable);
            }

            return new TEntity();
        }

        /// <summary>
        /// Gets the entity.
        /// </summary>
        /// <typeparam name="TEntity">The type of the t entity.</typeparam>
        /// <param name="autoFillValue">The automatic fill value.</param>
        /// <param name="tableDefinition">The table definition.</param>
        /// <returns>TEntity.</returns>
        public static TEntity GetEntity<TEntity>(this AutoFillValue autoFillValue,
            TableDefinition<TEntity> tableDefinition) where TEntity : class, new()
        {
            var result = new TEntity();
            if (autoFillValue.IsValid())
            {
                result = tableDefinition.GetEntityFromPrimaryKeyValue(autoFillValue.PrimaryKeyValue);
            }
            
            return result;
        }

        /// <summary>
        /// Converts to date.
        /// </summary>
        /// <param name="value">The value.</param>
        /// <returns>System.Nullable&lt;DateTime&gt;.</returns>
        public static DateTime? ToDate(this string value)
        {
            DateTime? result = null;
            if (DateTime.TryParse(value, null, out var newDate))
            {
                result = newDate;
            }
            return result;
        }

        /// <summary>
        /// Converts to fielddatatype.
        /// </summary>
        /// <param name="valueType">Type of the value.</param>
        /// <returns>FieldDataTypes.</returns>
        /// <exception cref="System.ArgumentOutOfRangeException">valueType - null</exception>
        public static FieldDataTypes ToFieldDataType(this ValueTypes valueType)
        {
            var result = FieldDataTypes.String;

            switch (valueType)
            {
                case ValueTypes.String:
                    result = FieldDataTypes.String;
                    break;
                case ValueTypes.Numeric:
                    result = FieldDataTypes.Decimal;
                    break;
                case ValueTypes.DateTime:
                    result = FieldDataTypes.DateTime;
                    break;
                case ValueTypes.Bool:
                    result = FieldDataTypes.Bool;
                    break;
                case ValueTypes.Memo:
                    result = FieldDataTypes.Memo;
                    break;
                default:
                    throw new ArgumentOutOfRangeException(nameof(valueType), valueType, null);
            }

            return result;
        }

        /// <summary>
        /// Fills the out entity.
        /// </summary>
        /// <typeparam name="TEntity">The type of the t entity.</typeparam>
        /// <param name="entity">The entity.</param>
        public static void FillOutEntity<TEntity>(this TEntity entity) where TEntity : class, new()
        {
            var table = SystemGlobals.LookupContext.TableDefinitions
                .FirstOrDefault(p => p.EntityName == entity.GetType().Name);
            if (table is TableDefinition<TEntity> fullTable)
            {
                fullTable.FillOutEntity(entity);
            }
        }
        /// <summary>
        /// Gets the automatic fill value.
        /// </summary>
        /// <typeparam name="TEntity">The type of the t entity.</typeparam>
        /// <param name="entity">The entity.</param>
        /// <param name="lookupDefinition">The lookup definition.</param>
        /// <returns>AutoFillValue.</returns>
        public static AutoFillValue GetAutoFillValue<TEntity>(this TEntity entity
            , LookupDefinitionBase lookupDefinition = null) where TEntity : class, new()
        {
            if (entity == null)
            {
                return null;
            }
            var table = SystemGlobals.LookupContext.TableDefinitions
                .FirstOrDefault(p => p.EntityName == entity.GetType().Name);
            if (table is TableDefinition<TEntity> fullTable)
            {
                var primaryKey1 = fullTable.GetPrimaryKeyValueFromEntity(entity);
                return fullTable.GetAutoFillValue(entity, primaryKey1.KeyString, lookupDefinition);
            }

            var primaryKey = new PrimaryKeyValue(table);
            return new AutoFillValue(primaryKey, string.Empty);
        }

        /// <summary>
        /// Fills the out properties.
        /// </summary>
        /// <typeparam name="TEntity">The type of the t entity.</typeparam>
        /// <param name="entity">The entity.</param>
        /// <param name="gridTables">The grid tables.</param>
        /// <returns>TEntity.</returns>
        public static TEntity FillOutProperties<TEntity>(this TEntity entity, List<TableDefinitionBase> gridTables)
            where TEntity : class, new()
        {
            if (entity == null)
            {
                return null;
            }

            var tableDefinition = GblMethods.GetTableDefinition<TEntity>();
            if (tableDefinition is TableDefinition<TEntity> fullTable)
            {
                var filter = GetFullTableFilter(entity);
                var table = fullTable.Context.GetQueryableForTableGrid(fullTable, gridTables);
                if (ProcessTableFilterQuery(filter, table, out var fillOutProperties1))
                    return fillOutProperties1;
            }

            return null;
        }
        /// <summary>
        /// Fills the out properties.
        /// </summary>
        /// <typeparam name="TEntity">The type of the t entity.</typeparam>
        /// <param name="entity">The entity.</param>
        /// <param name="getRelatedEntities">if set to <c>true</c> [get related entities].</param>
        /// <returns>TEntity.</returns>
        public static TEntity FillOutProperties<TEntity>(this TEntity entity, bool getRelatedEntities) where TEntity : class, new()
        {
            if (entity == null)
            {
                return null;
            }

            var tableDefinition = GblMethods.GetTableDefinition<TEntity>();
            if (tableDefinition is TableDefinition<TEntity> fullTable)
            {
                var filter = GetFullTableFilter(entity);
                var table = fullTable.Context.GetQueryableTable(fullTable, getRelatedEntities);
                if (ProcessTableFilterQuery(filter, table, out var fillOutProperties1)) 
                    return fillOutProperties1;
            }

            return null;
        }

        /// <summary>
        /// Processes the table filter query.
        /// </summary>
        /// <typeparam name="TEntity">The type of the t entity.</typeparam>
        /// <param name="filter">The filter.</param>
        /// <param name="table">The table.</param>
        /// <param name="fillOutProperties1">The fill out properties1.</param>
        /// <returns><c>true</c> if XXXX, <c>false</c> otherwise.</returns>
        private static bool ProcessTableFilterQuery<TEntity>(TableFilterDefinition<TEntity> filter, IQueryable<TEntity> table,
            out TEntity fillOutProperties1) where TEntity : class, new()
        {
            var param = GblMethods.GetParameterExpression<TEntity>();
            var expr = filter.GetWhereExpresssion<TEntity>(param);
            var query = FilterItemDefinition.FilterQuery(table, param, expr);
            if (query.Any())
            {
                var result = query.FirstOrDefault();
                {
                    fillOutProperties1 = result;
                    return true;
                }
            }

            fillOutProperties1 = null;
            return false;
        }

        /// <summary>
        /// Gets the full table filter.
        /// </summary>
        /// <typeparam name="TEntity">The type of the t entity.</typeparam>
        /// <param name="entity">The entity.</param>
        /// <returns>TableFilterDefinition&lt;TEntity&gt;.</returns>
        public static TableFilterDefinition<TEntity> GetFullTableFilter<TEntity>(this TEntity entity) where TEntity : class, new()
        {
            var tableDefinition = GblMethods.GetTableDefinition<TEntity>();
            if (tableDefinition is TableDefinition<TEntity> fullTable)
            {
                var filter = new TableFilterDefinition<TEntity>(fullTable);
                var primaryKey = fullTable.GetPrimaryKeyValueFromEntity(entity);
                if (primaryKey.IsValid())
                {
                    foreach (var primaryKeyField in fullTable.PrimaryKeyFields)
                    {
                        var fieldValue = GblMethods.GetPropertyValue(entity, primaryKeyField.PropertyName);
                        filter.AddFixedFilter(primaryKeyField, Conditions.Equals, fieldValue);
                    }

                }

                return filter;
            }

            return null;
        }

        /// <summary>
        /// Converts the enumerable to data table.
        /// </summary>
        /// <typeparam name="TEntity">The type of the t entity.</typeparam>
        /// <param name="data">The data.</param>
        /// <returns>DataTable.</returns>
        public static DataTable ConvertEnumerableToDataTable<TEntity>(this IEnumerable<TEntity> data) where TEntity : class, new()
        {
            var entityName = typeof(TEntity).Name;
            var table = SystemGlobals.LookupContext.TableDefinitions
                .FirstOrDefault(p => p.EntityName == entityName);

            var result = new DataTable();
            foreach (var fieldDefinition in table.FieldDefinitions)
            {
                result.Columns.Add(fieldDefinition.FieldName);
            }

            foreach (var entity in data)
            {
                var row = result.NewRow();
                foreach (var fieldDefinition in table.FieldDefinitions)
                {
                    var value = GblMethods.GetPropertyValue(entity, fieldDefinition.PropertyName);
                    row[fieldDefinition.FieldName] = value;
                }
                result.Rows.Add(row);
            }

            return result;
        }

        /// <summary>
        /// Validates the automatic fill.
        /// </summary>
        /// <param name="autoFillValue">The automatic fill value.</param>
        /// <param name="autoFillSetup">The automatic fill setup.</param>
        /// <returns><c>true</c> if XXXX, <c>false</c> otherwise.</returns>
        public static bool ValidateAutoFill(this AutoFillValue autoFillValue, AutoFillSetup autoFillSetup)
        {
            var result = true;

            if (autoFillSetup.ForeignField == null)
            {
                return result;
            }

            if (autoFillValue == null)
            {
                return autoFillSetup.ForeignField.AllowNulls;
            }
            if (!autoFillValue.IsValid())
            {
                if (!autoFillValue.Text.IsNullOrEmpty())
                {
                    return false;
                }
                else
                {
                    return autoFillSetup.ForeignField.AllowNulls;
                }
            }

            if (!autoFillSetup
                .LookupDefinition
                .TableDefinition
                .ValidateAutoFillValue(autoFillValue))
            {
                result = false;
            }
            return result;
        }

        /// <summary>
        /// Gets the name of the property join.
        /// </summary>
        /// <param name="tableFieldJoinDefinition">The table field join definition.</param>
        /// <param name="propertyName">Name of the property.</param>
        /// <param name="useParent">if set to <c>true</c> [use parent].</param>
        /// <returns>System.String.</returns>
        public static string GetPropertyJoinName(this TableFieldJoinDefinition tableFieldJoinDefinition, string propertyName, bool useParent = false)
        {
            var result = string.Empty;

            if (tableFieldJoinDefinition == null)
            {
                return propertyName;
            }

            var properties = tableFieldJoinDefinition.GetNavigationProperties(useParent);

            foreach (var property in properties)
            {
                result += $"{property.ParentJoin.ForeignKeyDefinition.ForeignObjectPropertyName}.";
            }

            result += propertyName;

            return result;

        }

        /// <summary>
        /// Gets the navigation properties.
        /// </summary>
        /// <param name="parentJoin">The parent join.</param>
        /// <returns>List&lt;JoinInfo&gt;.</returns>
        public static List<JoinInfo> GetNavigationProperties(this IJoinParent parentJoin)
        {
            var result = new List<JoinInfo>();
            if (parentJoin is LookupJoin lookupJoin)
            {
                if (lookupJoin.JoinDefinition != null)
                {
                    var joinInfo = new JoinInfo
                    {
                        ParentJoin = lookupJoin.JoinDefinition,
                    };
                    result.Add(joinInfo);
                }
                if (parentJoin.ParentObject != null)
                {
                    var newProps = parentJoin.ParentObject.GetNavigationProperties();
                    result.InsertRange(0, newProps);
                }
            }

            return result;
        }

        /// <summary>
        /// Gets the navigation properties.
        /// </summary>
        /// <param name="parentJoin">The parent join.</param>
        /// <param name="returnParent">if set to <c>true</c> [return parent].</param>
        /// <returns>List&lt;JoinInfo&gt;.</returns>
        public static List<JoinInfo> GetNavigationProperties(this TableFieldJoinDefinition parentJoin, bool returnParent = false)
        {
            var result = new List<JoinInfo>();
            var joinInfo = new JoinInfo
            {
                ParentJoin = parentJoin,
            };
            result.Add(joinInfo);

            if (parentJoin.ParentObject != null)
            {
                var newProps = parentJoin.ParentObject.GetNavigationProperties();
                result.InsertRange(0, newProps);
            }

            if (returnParent)
            {
                if (parentJoin.ForeignKeyDefinition.FieldJoins[0].ForeignField.AllowRecursion)
                {
                    result.Remove(result.LastOrDefault());
                }
            }
            return result;
        }

        /// <summary>
        /// Gets all navigation properties.
        /// </summary>
        /// <param name="lookupDefinition">The lookup definition.</param>
        /// <returns>List&lt;JoinInfo&gt;.</returns>
        public static List<JoinInfo> GetAllNavigationProperties(this LookupDefinitionBase lookupDefinition)
        {
            var result = new List<JoinInfo>();
            var joins = lookupDefinition.Joins;

            var baseJoins = joins.Where(p => p.ParentObject == null);
            foreach (var joinDefinition in baseJoins)
            {
                var joinInfo = new JoinInfo()
                {
                    ChildJoin = joinDefinition,
                };
                result.Add(joinInfo);
            }

            var children = joins
                .Where(p => p.ParentObject != null).OfType<TableFieldJoinDefinition>();

            foreach (var child in children)
            {
                var type = child.ParentObject.GetType();
                if (child.ParentObject is LookupJoin lookupJoin)
                {
                    var parentLookupJoin = result
                        .FirstOrDefault(p => 
                            p.ChildJoin.Alias == lookupJoin.JoinDefinition.Alias);
                    if (parentLookupJoin != null)
                    {
                        var joinInfo = new JoinInfo()
                        {
                            ParentJoin = parentLookupJoin.ChildJoin,
                            ChildJoin = child
                        };
                        result.Add(joinInfo);
                    }
                    else
                    {
                        
                    }
                }
            }

            return result;
        }

        /// <summary>
        /// Gets all include properties from nav properties.
        /// </summary>
        /// <param name="joinsInfos">The joins infos.</param>
        /// <returns>List&lt;System.String&gt;.</returns>
        public static List<string> GetAllIncludePropertiesFromNavProperties(this List<JoinInfo> joinsInfos)
        {
            var result = new List<string>();
            foreach (var joinInfo in joinsInfos)
            {
                result.Add(joinInfo.GetIncludePropertyFromNavProperty(joinsInfos));
            }
            return result;
        }

        /// <summary>
        /// Gets the include property from nav property.
        /// </summary>
        /// <param name="joinInfo">The join information.</param>
        /// <param name="joinInfos">The join infos.</param>
        /// <returns>System.String.</returns>
        public static string GetIncludePropertyFromNavProperty(this JoinInfo joinInfo, List<JoinInfo> joinInfos)
        {
            var result = joinInfo.ChildJoin.ForeignKeyDefinition.ForeignObjectPropertyName;
            if (joinInfo.ParentJoin != null)
            {
                var parentJoin = joinInfos.FirstOrDefault(p => p.ChildJoin == joinInfo.ParentJoin);
                if (parentJoin != null)
                {
                    result = $"{GetIncludePropertyFromNavProperty(parentJoin, joinInfos)}.{result}";
                }
            }
            return result;
        }

        /// <summary>
        /// Gets the property filter value.
        /// </summary>
        /// <param name="value">The value.</param>
        /// <param name="dataType">Type of the data.</param>
        /// <param name="valType">Type of the value.</param>
        /// <returns>System.Object.</returns>
        /// <exception cref="System.ArgumentOutOfRangeException">dataType - null</exception>
        public static object GetPropertyFilterValue(this string value, FieldDataTypes dataType, Type valType)
        {
            var nullable = false;
            if (Nullable.GetUnderlyingType(valType) != null)
            {
                nullable = true;
            }
            object result = null;
            if (value == null)
            {
                return result;
            }
            switch (dataType)
            {
                case FieldDataTypes.String:
                case FieldDataTypes.Memo:
                    result = value;
                    break;
                case FieldDataTypes.Integer:
                    if (valType.IsEnum)
                    {
                        result = value.ToInt();
                        return result;
                    }
                    if (nullable)
                    {
                        int? intVal = int.Parse(value);
                        if (valType == typeof(Int16?))
                        {
                            result = (Int16?)Int16.Parse(value); }

                        if (valType == typeof(byte?))
                        {
                            result = (byte?)byte.Parse(value);
                        }

                        if (result == null)
                        {
                            result = intVal;
                        }
                    }
                    else
                    {
                        var intVal = int.Parse(value);
                        if (valType == typeof(Int16))
                        {
                            result = Int16.Parse(value);
                        }

                        if (valType == typeof(byte))
                        {
                            result = byte.Parse(value);
                        }

                        if (result == null)
                        {
                            result = intVal;
                        }
                    }

                    break;
                case FieldDataTypes.Decimal:
                    result = double.Parse(value);
                    break;
                case FieldDataTypes.DateTime:
                    if (value.IsNullOrEmpty())
                    {
                        result = (DateTime?)null;
                    }
                    else
                    {
                        result = DateTime.Parse(value);
                    }

                    break;
                case FieldDataTypes.Bool:
                    result = value.ToBool();
                    break;
                default:
                    throw new ArgumentOutOfRangeException(nameof(dataType), dataType, null);
            }
            return result;
        }

        /// <summary>
        /// Appends the expression.
        /// </summary>
        /// <param name="left">The left.</param>
        /// <param name="right">The right.</param>
        /// <param name="endLogic">The end logic.</param>
        /// <returns>Expression.</returns>
        public static Expression AppendExpression(this BinaryExpression left, BinaryExpression right, EndLogics endLogic)
        {
            return FilterItemDefinition.AppendExpression(left, right, endLogic);
        }

        /// <summary>
        /// Determines whether [is equal to] [the specified last].
        /// </summary>
        /// <typeparam name="TEntity">The type of the t entity.</typeparam>
        /// <param name="first">The first.</param>
        /// <param name="last">The last.</param>
        /// <returns><c>true</c> if [is equal to] [the specified last]; otherwise, <c>false</c>.</returns>
        public static bool IsEqualTo<TEntity>(this TEntity first, TEntity last) where TEntity : class, new()
        {
            var tableDef = GblMethods.GetTableDefinition<TEntity>();
            if (tableDef == null)
            {
                return false;
            }
            var firstPk = tableDef.GetPrimaryKeyValueFromEntity(first);
            var lastPk = tableDef.GetPrimaryKeyValueFromEntity(last);
            return firstPk.IsEqualTo(lastPk);
        }

        /// <summary>
        /// Uts the fill out entity.
        /// </summary>
        /// <typeparam name="TEntity">The type of the t entity.</typeparam>
        /// <param name="entity">The entity.</param>
        public static void UtFillOutEntity<TEntity>(this TEntity entity) where TEntity : class, new()
        {
            if (!SystemGlobals.UnitTestMode)
            {
                return;
            }

            var tableDef = GblMethods.GetTableDefinition<TEntity>();
            tableDef.FillOutEntity(entity);
        }

        /// <summary>
        /// Gets the table definition.
        /// </summary>
        /// <typeparam name="TEntity">The type of the t entity.</typeparam>
        /// <param name="entity">The entity.</param>
        /// <returns>TableDefinition&lt;TEntity&gt;.</returns>
        public static TableDefinition<TEntity> GetTableDefinition<TEntity>(this TEntity entity)
            where TEntity : class, new()
        {
            return GblMethods.GetTableDefinition<TEntity>();
        }

        /// <summary>
        /// Determines whether the specified right type has right.
        /// </summary>
        /// <param name="tableDefinition">The table definition.</param>
        /// <param name="rightType">Type of the right.</param>
        /// <returns><c>true</c> if the specified right type has right; otherwise, <c>false</c>.</returns>
        public static bool HasRight(this TableDefinitionBase tableDefinition, RightTypes rightType)
        {
            if (tableDefinition.TableRight != null)
            {
                var tableRightVal = (int)tableDefinition.TableRight;
                var checkRightVal = (int)rightType;
                return tableRightVal >= checkRightVal;
            }
            if (SystemGlobals.Rights == null)
            {
                return true;
            }
            return SystemGlobals.Rights.HasRight(tableDefinition, rightType);
        }

        /// <summary>
        /// Determines whether [has special right] [the specified right type].
        /// </summary>
        /// <param name="tableDefinition">The table definition.</param>
        /// <param name="rightType">Type of the right.</param>
        /// <returns><c>true</c> if [has special right] [the specified right type]; otherwise, <c>false</c>.</returns>
        public static bool HasSpecialRight(this TableDefinitionBase tableDefinition, int rightType)
        {
            return SystemGlobals.Rights.HasSpecialRight(tableDefinition, rightType);
        }

        public static string GetText(this AutoFillValue autoFillValue)
        {
            if (autoFillValue != null)
            {
                return autoFillValue.Text;
            }

            return string.Empty;
        }
    }
}
