using RingSoft.DataEntryControls.Engine;
using RingSoft.DbLookup.ModelDefinition.FieldDefinitions;
using RingSoft.DbLookup.QueryBuilder;
using System;
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
    public class JoinInfo
    {
        public TableFieldJoinDefinition ParentJoin { get; set; }

        public TableFieldJoinDefinition ChildJoin { get; set; }
    }

    public static class ExtensionMethods
    {

        public static string TrimRight(this string value, string trimChars)
        {
            return value.LeftStr(value.Length - trimChars.Length);
        }

        #region Property Name

        /// <summary>
        /// Gets the full name of the property. (u => u.UserId returns "UserId")
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

        private static bool IsConversion(Expression exp)
        {
            return (
                exp.NodeType == ExpressionType.Convert ||
                exp.NodeType == ExpressionType.ConvertChecked
                );
        }

        #endregion

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

        public static bool IsValid(this PrimaryKeyValue pkValue)
        {
            if (pkValue == null)
            {
                return false;
            }

            return pkValue.IntIsValid;
        }
        public static bool IsValid(this AutoFillValue autoFillValue)
        {
            if (autoFillValue == null)
            {
                return false;
            }
            return autoFillValue.PrimaryKeyValue.IsValid();
        }

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

        public static AutoFillValue GetAutoFillValue(this TableDefinitionBase tableDefinition, string primaryKeyString)
        {
            return tableDefinition.Context.OnAutoFillTextRequest(tableDefinition, primaryKeyString);
        }

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

        public static DateTime? ToDate(this string value)
        {
            DateTime? result = null;
            if (DateTime.TryParse(value, null, out var newDate))
            {
                result = newDate;
            }
            return result;
        }

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

        public static void FillOutEntity<TEntity>(this TEntity entity) where TEntity : class, new()
        {
            var table = SystemGlobals.LookupContext.TableDefinitions
                .FirstOrDefault(p => p.EntityName == entity.GetType().Name);
            if (table is TableDefinition<TEntity> fullTable)
            {
                fullTable.FillOutEntity(entity);
            }
        }
        public static AutoFillValue GetAutoFillValue<TEntity>(this TEntity entity) where TEntity : class, new()
        {
            if (entity == null)
            {
                return null;
            }
            var table = SystemGlobals.LookupContext.TableDefinitions
                .FirstOrDefault(p => p.EntityName == entity.GetType().Name);
            if (table is TableDefinition<TEntity> fullTable)
            {
                return fullTable.GetAutoFillValue(entity);
            }

            var primaryKey = new PrimaryKeyValue(table);
            return new AutoFillValue(primaryKey, string.Empty);
        }

        public static TEntity FillOutProperties<TEntity>(this TEntity entity) where TEntity : class, new()
        {
            if (entity == null)
            {
                return null;
            }
            var tableDefinition = SystemGlobals
                .LookupContext
                .TableDefinitions
                .FirstOrDefault(p => p.EntityName == entity.GetType().Name);
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

                    var context = SystemGlobals.DataRepository.GetDataContext();
                    var table = context.GetTable<TEntity>();
                    var param = GblMethods.GetParameterExpression<TEntity>();
                    var expr = filter.GetWhereExpresssion<TEntity>(param);
                    var query = FilterItemDefinition.FilterQuery(table, param, expr);
                    if (query.Any())
                    {
                        var result = query.FirstOrDefault();
                        return result;
                    }
                }
            }

            return entity;
        }


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
            return result;
        }

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

            var children = joins.Where(p => p.ParentObject != null).OfType<TableFieldJoinDefinition>();

            foreach (var child in children)
            {
                var type = child.ParentObject.GetType();
                if (child.ParentObject is LookupJoin lookupJoin)
                {
                    var parentLookupJoin = result.FirstOrDefault(p => p.ChildJoin == lookupJoin.JoinDefinition);
                    if (parentLookupJoin != null)
                    {
                        var joinInfo = new JoinInfo()
                        {
                            ParentJoin = parentLookupJoin.ChildJoin,
                            ChildJoin = child
                        };
                        result.Add(joinInfo);
                    }
                }
            }

            return result;
        }

        public static List<string> GetAllIncludePropertiesFromNavProperties(this List<JoinInfo> joinsInfos)
        {
            var result = new List<string>();
            foreach (var joinInfo in joinsInfos)
            {
                result.Add(joinInfo.GetIncludePropertyFromNavProperty(joinsInfos));
            }
            return result;
        }

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

        public static Expression AppendExpression(this BinaryExpression left, BinaryExpression right, EndLogics endLogic)
        {
            return FilterItemDefinition.AppendExpression(left, right, endLogic);
        }
    }
}
