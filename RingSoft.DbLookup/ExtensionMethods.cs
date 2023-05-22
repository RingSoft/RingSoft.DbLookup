﻿using RingSoft.DataEntryControls.Engine;
using RingSoft.DbLookup.ModelDefinition.FieldDefinitions;
using RingSoft.DbLookup.QueryBuilder;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Linq.Expressions;
using RingSoft.DbLookup.AutoFill;
using RingSoft.DbLookup.ModelDefinition;
using RingSoft.DbLookup.DataProcessor;
using RingSoft.DbLookup.Lookup;

namespace RingSoft.DbLookup
{
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

        public static bool IsValid(this AutoFillValue autoFillValue)
        {
            return autoFillValue?.PrimaryKeyValue != null && autoFillValue.PrimaryKeyValue.IsValid;
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
            where TEntity : new()
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
            TableDefinition<TEntity> tableDefinition) where TEntity : new()
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
        public static AutoFillValue GetAutoFillValue<TEntity>(this TEntity entity) where TEntity : new()
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
    }
}
