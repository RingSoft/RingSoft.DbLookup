// ***********************************************************************
// Assembly         : RingSoft.DbLookup
// Author           : petem
// Created          : 12-19-2022
//
// Last Modified By : petem
// Last Modified On : 09-09-2023
// ***********************************************************************
// <copyright file="GblMethods.cs" company="Peter Ringering">
//     Copyright (c) 2023. All rights reserved.
// </copyright>
// <summary></summary>
// ***********************************************************************
using RingSoft.DataEntryControls.Engine;
using RingSoft.DbLookup.ModelDefinition;
using RingSoft.DbLookup.QueryBuilder;
using RingSoft.DbLookup.RecordLocking;
using RingSoft.Printing.Interop;
using System;
using System.Collections.Generic;
using System.Data;
using System.Diagnostics;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using RingSoft.DbLookup.ModelDefinition.FieldDefinitions;
using Enum = System.Enum;
using Type = System.Type;

namespace RingSoft.DbLookup
{
    /// <summary>
    /// Enum FieldDataTypes
    /// </summary>
    public enum FieldDataTypes
    {
        /// <summary>
        /// The string
        /// </summary>
        String = 0,
        /// <summary>
        /// The integer
        /// </summary>
        Integer = 1,
        /// <summary>
        /// The decimal
        /// </summary>
        Decimal = 2,
        //Enum = 3,
        /// <summary>
        /// The date time
        /// </summary>
        DateTime = 4,
        /// <summary>
        /// The bool
        /// </summary>
        Bool = 5,
        /// <summary>
        /// The memo
        /// </summary>
        Memo = 6
    }

    /// <summary>
    /// Enum OrderMethods
    /// </summary>
    public enum OrderMethods
    {
        /// <summary>
        /// The order by
        /// </summary>
        OrderBy = 0,
        /// <summary>
        /// The then by
        /// </summary>
        ThenBy = 1,
        /// <summary>
        /// The order by descending
        /// </summary>
        OrderByDescending = 2,
        /// <summary>
        /// The then by descending
        /// </summary>
        ThenByDescending = 3,
    }

    /// <summary>
    /// Global Library Methods.
    /// </summary>
    public static class GblMethods
    {
        /// <summary>
        /// The search for enum host identifier
        /// </summary>
        public const int SearchForEnumHostId = 4;
        /// <summary>
        /// Gets or sets the last error.
        /// </summary>
        /// <value>The last error.</value>
        public static string LastError { get; set; }

        public static DateTime NowDate()
        {
            var nowDate = DateTime.Now;
            var result = DateTime.Today;
            result = result.AddHours(nowDate.Hour);
            result = result.AddMinutes(nowDate.Minute);
            result = result.AddSeconds(nowDate.Second);
            return result;
        }

        public static DateTime ScrubDateTime(DateTime input)
        {
            var result = DateTime.MinValue;
            result = result.AddYears(input.Year - DateTime.MinValue.Year);
            result = result.AddMonths(input.Month - DateTime.MinValue.Month);
            result = result.AddDays(input.Day - DateTime.MinValue.Day);
            result = result.AddHours(input.Hour);
            result = result.AddMinutes(input.Minute);
            result = result.AddSeconds(input.Second);
            return result;
        }

        /// <summary>
        /// Duplicates the passed-in string.
        /// </summary>
        /// <param name="dupString">The string to duplicate.</param>
        /// <param name="count">The number of times to duplicate.</param>
        /// <returns>The duplicated string.</returns>
        public static string StringDuplicate(this string dupString, int count)
        {
            StringBuilder stringBuilder = new StringBuilder();
            for (int counter = 0; counter < count; counter++)
                stringBuilder.Append(dupString);

            return stringBuilder.ToString();
        }

        /// <summary>
        /// Gets the number format string to use in formatting numeric values.
        /// </summary>
        /// <param name="decimals">The number of digits to the right of the double point.</param>
        /// <param name="isCurrency">if set to <c>true</c> [is currency].</param>
        /// <returns>A numeric format string.</returns>
        public static string GetNumFormat(int decimals, bool isCurrency)
        {
            if (isCurrency)
                return $"C{decimals}";

            return $"N{decimals}";
        }

        /// <summary>
        /// Gets the percent format string used in formatting percent values.
        /// </summary>
        /// <param name="decimals">The decimals.</param>
        /// <returns>System.String.</returns>
        public static string GetPercentFormat(int decimals)
        {
            return $"P{decimals}";
        }

        /// <summary>
        /// Removes all currency, percent, and thousands separator text from the text.
        /// </summary>
        /// <param name="text">The text to process.</param>
        /// <returns>Text without numeric symbols.</returns>
        public static string NumTextToString(this string text)
        {
            var stripText = NumberFormatInfo.CurrentInfo.CurrencyGroupSeparator;
            stripText += NumberFormatInfo.CurrentInfo.CurrencySymbol;
            stripText += NumberFormatInfo.CurrentInfo.PercentSymbol;
            return StripText(text, stripText);
        }

        /// <summary>
        /// Strips the text of all the characters in the stripString.
        /// </summary>
        /// <param name="text">The text to process.</param>
        /// <param name="stripString">The characters to strip.</param>
        /// <returns>The text without the characters in stripString.</returns>
        public static string StripText(this string text, string stripString)
        {
            if (text == "")
                return text;

            string returnString = text;
            foreach (char cChar in stripString)
                returnString = returnString.Replace(cChar.ToString(), "");

            return returnString;
        }

        /// <summary>
        /// Formats the date value.
        /// </summary>
        /// <param name="value">The value.</param>
        /// <param name="dateType">Type of the date.</param>
        /// <param name="fullString">if set to <c>true</c> [full string].</param>
        /// <returns>System.String.</returns>
        /// <exception cref="System.ArgumentOutOfRangeException">value - null</exception>
        public static string FormatDateValue(this DateTime value, DbDateTypes dateType, bool fullString = true)
        {
            string formatString;
            if (fullString)
            {
                switch (dateType)
                {
                    case DbDateTypes.DateOnly:
                    case DbDateTypes.Millisecond:
                        formatString = "MM/dd/yyyy";
                        break;
                    case DbDateTypes.DateTime:
                        formatString = "MM/dd/yyyy hh:mm:ss tt";
                        break;
                    default:
                        throw new ArgumentOutOfRangeException(nameof(value), value, null);
                }
            }
            else
            {
                switch (dateType)
                {
                    case DbDateTypes.DateOnly:
                    case DbDateTypes.Millisecond:
                        formatString = "M/d/yyyy";
                        break;
                    case DbDateTypes.DateTime:
                        formatString = "M/d/yyyy hh:mm:ss tt";
                        break;
                    default:
                        throw new ArgumentOutOfRangeException(nameof(value), value, null);

                }
            }

            var result = value.ToString(formatString);
            if (dateType == DbDateTypes.Millisecond)
            {
                result = $"{result} {value.TimeOfDay}";
            }
            return result;
        }

        /// <summary>
        /// Formats the value.
        /// </summary>
        /// <param name="dataType">Type of the data.</param>
        /// <param name="value">The value to format.</param>
        /// <param name="formatString">The format string.</param>
        /// <param name="culture">The culture.</param>
        /// <param name="convertToLocalTime">if set to <c>true</c> [convert to local time].</param>
        /// <returns>System.String.</returns>
        /// <exception cref="System.ArgumentOutOfRangeException"></exception>
        public static string FormatValue(FieldDataTypes dataType, string value, string formatString = "", CultureInfo culture = null, bool convertToLocalTime = false)
        {
           
            switch (dataType)
            {
                case FieldDataTypes.String:
                    break;
                case FieldDataTypes.Integer:
                    if (value.IsNullOrEmpty())
                        value = "0";
                    if (culture == null)
                        culture = LookupDefaults.DefaultNumberCulture;
                    if (Int32.TryParse(value, out var intValue))
                        return intValue.ToString(formatString, culture.NumberFormat);
                    break;
                case FieldDataTypes.Decimal:
                    if (culture == null)
                        culture = LookupDefaults.DefaultNumberCulture;
                    if (value.IsNullOrEmpty())
                        value = "0";
                    if (Decimal.TryParse(value, out var decimalValue))
                        return decimalValue.ToString(formatString, culture.NumberFormat);
                    break;
                case FieldDataTypes.DateTime:
                    if (culture == null)
                        culture = LookupDefaults.DefaultDateCulture;
                    if (DateTime.TryParse(value, out var dateValue))
                    {
                        if (convertToLocalTime)
                        {
                            dateValue = dateValue.ToLocalTime();
                        }
                        return dateValue.ToString(formatString, culture.DateTimeFormat);
                    }

                    break;
                case FieldDataTypes.Bool:
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }

            return value;
        }

        /// <summary>
        /// Gets the value in the DataRow for the column name.
        /// </summary>
        /// <param name="dataRow">The data row.</param>
        /// <param name="columnName">Name of the column.</param>
        /// <returns>The value for the column name.</returns>
        /// <exception cref="System.ArgumentException">Column {columnName} does not exist in the DataRow.</exception>
        public static string GetRowValue(this DataRow dataRow, string columnName)
        {
            try
            {
                return dataRow[columnName].ToString();
            }
            catch (Exception)
            {
                throw new ArgumentException($"Column {columnName} does not exist in the DataRow.");
            }
        }

        /// <summary>
        /// Sets the property value.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="model">The model.</param>
        /// <param name="propertyName">Name of the property.</param>
        /// <param name="value">The value.</param>
        public static void SetPropertyValue<T>(T model, string propertyName, string value) where T : new()
        {
            var property = model.GetType().GetProperties().FirstOrDefault(f => f.Name == propertyName);
            if (property != null)
            {
                var nullable = false;
                if (Nullable.GetUnderlyingType(property.PropertyType) != null)
                {
                    nullable = true;
                }

                if (nullable &&  value == null)
                {
                    property.SetValue(model, value);
                }

                if (property.PropertyType == typeof(string))
                {
                    property.SetValue(model, value);
                }
                else if (property.PropertyType == typeof(DateTime)
                         || property.PropertyType == typeof(DateTime?))
                {
                    DateTime checkValue;
                    if (DateTime.TryParse(value, out checkValue))
                        property.SetValue(model, checkValue);
                }
                else if (property.PropertyType == typeof(double)
                         || property.PropertyType == typeof(double?))
                {
                    double checkValue;
                    if (double.TryParse(value, out checkValue))
                        property.SetValue(model, checkValue);
                }
                else if (property.PropertyType == typeof(double)
                         || property.PropertyType == typeof(double?))
                {
                    double checkValue;
                    if (Double.TryParse(value, out checkValue))
                        property.SetValue(model, checkValue);
                }
                else if (property.PropertyType == typeof(float)
                         || property.PropertyType == typeof(float?))
                {
                    float checkValue;
                    if (Single.TryParse(value, out checkValue))
                        property.SetValue(model, checkValue);
                }
                else if (property.PropertyType == typeof(int) 
                         || property.PropertyType == typeof(int?)
                         || property.PropertyType.BaseType == typeof(Enum))
                {
                    int checkValue;
                    if (Int32.TryParse(value, out checkValue))
                        property.SetValue(model, checkValue);
                }
                else if (property.PropertyType == typeof(long)
                         || property.PropertyType == typeof(long?))
                {
                    long checkValue;
                    if (Int64.TryParse(value, out checkValue))
                        property.SetValue(model, checkValue);
                }
                else if (property.PropertyType == typeof(byte)
                         || property.PropertyType == typeof(byte?))
                {
                    byte checkValue;
                    if (Byte.TryParse(value, out checkValue))
                        property.SetValue(model, checkValue);
                }
                else if (property.PropertyType == typeof(short)
                         || property.PropertyType == typeof(short?))
                {
                    short checkValue;
                    if (Int16.TryParse(value, out checkValue))
                        property.SetValue(model, checkValue);
                }

                else if (property.PropertyType == typeof(bool)
                         || property.PropertyType == typeof(bool?))
                {
                    bool checkValue;
                    if (bool.TryParse(value, out checkValue))
                        property.SetValue(model, checkValue);
                }

                else
                {
                    property.SetValue(model,null);
                }
            }
        }

        /// <summary>
        /// Sets the property object.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="model">The model.</param>
        /// <param name="propertyName">Name of the property.</param>
        /// <param name="value">The value.</param>
        public static void SetPropertyObject<T>(T model, string propertyName, object value) where T : new()
        {
            var property = model.GetType().GetProperties().FirstOrDefault(f => f.Name == propertyName);
            if (property != null)
            {
                property.SetValue(model, value);
            }
        }

        /// <summary>
        /// Gets the property value.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="model">The model.</param>
        /// <param name="propertyName">Name of the property.</param>
        /// <param name="dateType">Type of the date.</param>
        /// <returns>System.String.</returns>
        public static string GetPropertyValue<T>(T model, string propertyName, DbDateTypes? dateType = null) where T : new()
        {
            var properties = model.GetType().GetProperties();
            var property =
                properties.FirstOrDefault(f => f.Name == propertyName);

            if (property != null)
            {
                var value = property.GetValue(model);
                if (property.PropertyType.IsEnum)
                {
                    value = (int)value;
                }
                if (dateType != null)
                {
                    switch (dateType.Value)
                    {
                        case DbDateTypes.DateTime:
                        case DbDateTypes.Millisecond:
                            if (value is DateTime date)
                            {
                                var dateValue = date.FormatDateValue(DbDateTypes.Millisecond);
                                return dateValue;
                            }
                            break;
                    }
                }

                if (value != null)
                {
                    return value.ToString();
                }
            }

            return string.Empty;
        }

        /// <summary>
        /// Gets the property object.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="model">The model.</param>
        /// <param name="propertyName">Name of the property.</param>
        /// <returns>System.Object.</returns>
        public static object GetPropertyObject<T>(T model, string propertyName)
        {
            var properties = model.GetType().GetProperties();
            var property =
                properties.FirstOrDefault(f => f.Name == propertyName);

            object value = property.GetValue(model, null);
            return value;
        }

        /// <summary>
        /// Gets the property.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="propertyName">Name of the property.</param>
        /// <returns>System.Object.</returns>
        public static object GetProperty<T>(string propertyName)
        {
            var properties = typeof(T).GetProperties();
            var property =
                properties.FirstOrDefault(f => f.Name == propertyName);
            if (property != null)
                return property;
            return null;
        }

        /// <summary>
        /// Gets the property.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="parentProperty">The parent property.</param>
        /// <param name="propertyName">Name of the property.</param>
        /// <returns>System.Object.</returns>
        public static object GetProperty<T>(T parentProperty, string propertyName)
        {
            var properties = typeof(T).GetProperties();
            var property =
                properties.FirstOrDefault(f => f.Name == propertyName);
            if (property != null)
                return property;
            return null;

        }

        /// <summary>
        /// Gets the type of the field data type for.
        /// </summary>
        /// <param name="type">The type.</param>
        /// <returns>FieldDataTypes.</returns>
        public static FieldDataTypes GetFieldDataTypeForType(Type type)
        {
            if (type == typeof(DateTime)
                || type == typeof(DateTime?))
            {
                return FieldDataTypes.DateTime;
            }
            else if (type == typeof(double)
                     || type == typeof(double?)
                     || type == typeof(double)
                     || type == typeof(double?)
                     || type == typeof(float)
                     || type == typeof(float?))
            {
                return FieldDataTypes.Decimal;
            }
            else if (type == typeof(int)
                     || type == typeof(int?)
                     || type == typeof(long)
                     || type == typeof(long?)
                     || type == typeof(byte)
                     || type == typeof(byte?)
                     || type == typeof(short)
                     || type == typeof(short?))
            {
                return FieldDataTypes.Integer;
            }
            else if (type == typeof(bool))
            {
                return FieldDataTypes.Bool;
            }

            return FieldDataTypes.String;
        }

        /// <summary>
        /// Gets the type of the value type for field data.
        /// </summary>
        /// <param name="fieldDataType">Type of the field data.</param>
        /// <returns>ValueTypes.</returns>
        /// <exception cref="System.ArgumentOutOfRangeException">fieldDataType - null</exception>
        public static ValueTypes GetValueTypeForFieldDataType(FieldDataTypes fieldDataType)
        {
            switch (fieldDataType)
            {
                case FieldDataTypes.String:
                    return ValueTypes.String;
                case FieldDataTypes.Integer:
                case FieldDataTypes.Decimal:
                    return ValueTypes.Numeric;
                case FieldDataTypes.DateTime:
                    return ValueTypes.DateTime;
                case FieldDataTypes.Bool:
                    return ValueTypes.Bool;
                default:
                    throw new ArgumentOutOfRangeException(nameof(fieldDataType), fieldDataType, null);
            }
        }

        /// <summary>
        /// Formats the value for printer row key.
        /// </summary>
        /// <param name="fieldDataType">Type of the field data.</param>
        /// <param name="value">The value.</param>
        /// <returns>System.String.</returns>
        public static string FormatValueForPrinterRowKey(FieldDataTypes fieldDataType, string value)
        {
            switch (fieldDataType)
            {
                case FieldDataTypes.Integer:
                    var numberFormat = GetNumFormat(0, false);
                    var intValue = value.ToInt();
                    return intValue.ToString("00000000");
                case FieldDataTypes.Decimal:
                    var decimalValue = value.ToDecimal();
                    return decimalValue.ToString("00000000N2");
                case FieldDataTypes.DateTime:
                    var date = value.ToDate();
                    return date?.ToString("O");
                case FieldDataTypes.Bool:
                    return value.ToBool().ToString();
            }
            return value;
        }

        /// <summary>
        /// Gets the name of the printing input executable file.
        /// </summary>
        /// <returns>System.String.</returns>
        public static string GetPrintingInputExeFileName()
        {
            var jsonFile = $"{PrintingInteropGlobals.ProgramDataFolder}{PrintingInteropGlobals.InitializeJsonFileName}";
            var fileInfo = new FileInfo(jsonFile);
            if (!fileInfo.Exists)
            {
                return string.Empty;
            }
            return jsonFile;
        }

        /// <summary>
        /// Validates the printing file.
        /// </summary>
        /// <returns><c>true</c> if XXXX, <c>false</c> otherwise.</returns>
        public static async Task<bool> ValidatePrintingFile()
        {
            if (GetPrintingInputExeFileName().IsNullOrEmpty())
            {
                var message =
                    "The RingSoft printing app has not been installed. Would you like to download and install it?";
                var caption = "Printing";
                if (await ControlsGlobals.UserInterface.ShowYesNoMessageBox(message, caption, true) ==
                    MessageBoxButtonsResult.Yes)
                {
                    Process.Start(new ProcessStartInfo
                    {
                        FileName = "http://ringsoft.site/printing/",
                        UseShellExecute = true
                    });
                }

                return false;
            }
            return true;
        }

        /// <summary>
        /// Does the record lock.
        /// </summary>
        /// <param name="primaryKey">The primary key.</param>
        /// <returns><c>true</c> if XXXX, <c>false</c> otherwise.</returns>
        public static bool DoRecordLock(PrimaryKeyValue primaryKey)
        {
            var result = true;
            var context = SystemGlobals.DataRepository.GetDataContext();
            var query = context.GetTable<RecordLock>();
            var recordLock = query.FirstOrDefault(p => p.Table == primaryKey.TableDefinition.TableName
                                                             
                                                       && p.PrimaryKey == primaryKey.KeyString);
            if (recordLock == null)
            {
                recordLock = new RecordLock()
                {
                    Table = primaryKey.TableDefinition.TableName,
                    LockDateTime = DateTime.Now.ToUniversalTime(),
                    PrimaryKey = primaryKey.KeyString,
                    User = SystemGlobals.UserName,
                };
                var list = new List<RecordLock>();
                list.Add(recordLock);
                context.AddRange(list);
                context.Commit("Saving Record Lock");
            }
            else
            {
                recordLock.LockDateTime = DateTime.Now.ToUniversalTime();
                recordLock.User = SystemGlobals.UserName;
                context.SaveEntity(recordLock, "Saving Record Lock");
            }

            //var recordLockTableField =
            //    SystemGlobals.AdvancedFindLookupContext.RecordLocks.GetFieldDefinition(p => p.Table);
            //var recordLockPkField =
            //    SystemGlobals.AdvancedFindLookupContext.RecordLocks.GetFieldDefinition(p => p.PrimaryKey);
            //var recordLockDateField =
            //    SystemGlobals.AdvancedFindLookupContext.RecordLocks.GetFieldDefinition(p => p.LockDateTime);
            //var recordLockUserField = SystemGlobals.AdvancedFindLookupContext.RecordLocks.GetFieldDefinition(p => p.User);

            //var sqlDatas = new List<SqlData>();
            //var sqlData = new SqlData(recordLockTableField.FieldName,
            //    primaryKey.TableDefinition.TableName, ValueTypes.String);
            //sqlDatas.Add(sqlData);
            //sqlData = new SqlData(recordLockPkField.FieldName, primaryKey.KeyString, ValueTypes.String);
            //sqlDatas.Add(sqlData);
            //sqlData = new SqlData(recordLockDateField.FieldName, GetNowDateText(), ValueTypes.DateTime,
            //    DbDateTypes.Millisecond);
            //sqlDatas.Add(sqlData);
            //sqlData = new SqlData(recordLockUserField.FieldName, SystemGlobals.UserName, ValueTypes.String);
            //sqlDatas.Add(sqlData);

            //var selectQuery = new SelectQuery(recordLockTableField.TableDefinition.TableName);
            //selectQuery.AddWhereItem(recordLockTableField.FieldName, Conditions.Equals,
            //    primaryKey.TableDefinition.TableName);

            //selectQuery.AddWhereItem(recordLockPkField.FieldName, Conditions.Equals, primaryKey.KeyString);

            //var getDataResult = primaryKey.TableDefinition.Context.DataProcessor.GetData(selectQuery);
            //if (getDataResult.ResultCode == GetDataResultCodes.Success)
            //{
            //    var sql = string.Empty;
            //    if (getDataResult.DataSet.Tables[0].Rows.Count >= 1)
            //    {
            //        var dataRow = getDataResult.DataSet.Tables[0].Rows[0];
            //        var recordLockPrimaryKey = new PrimaryKeyValue(SystemGlobals.AdvancedFindLookupContext.RecordLocks);
            //        recordLockPrimaryKey.PopulateFromDataRow(dataRow);
            //        var updateStatement = new UpdateDataStatement(recordLockPrimaryKey);
            //        foreach (var data in sqlDatas)
            //        {
            //            updateStatement.AddSqlData(data);
            //        }

            //        sql = primaryKey.TableDefinition.Context.DataProcessor.SqlGenerator.GenerateUpdateSql(
            //            updateStatement);
            //    }
            //    else
            //    {
            //        var insertStatement = new InsertDataStatement(recordLockPkField.TableDefinition);
            //        foreach (var data in sqlDatas)
            //        {
            //            insertStatement.AddSqlData(data);
            //        }

            //        sql = primaryKey.TableDefinition.Context.DataProcessor.SqlGenerator.GenerateInsertSqlStatement(
            //            insertStatement);
            //    }
            //    result = primaryKey.TableDefinition.Context.DataProcessor.ExecuteSql(sql).ResultCode == GetDataResultCodes.Success;
            //}

            return result;
        }

        /// <summary>
        /// Gets the now date text.
        /// </summary>
        /// <returns>System.String.</returns>
        public static string GetNowDateText()
        {
            var newDate = DateTime.Now.ToUniversalTime();
            var dateText = newDate.ToString("yyyy-MM-dd HH:mm:ss.fff");
            return dateText;
        }

        /// <summary>
        /// Gets the table definition.
        /// </summary>
        /// <typeparam name="TEntity">The type of the t entity.</typeparam>
        /// <returns>TableDefinition&lt;TEntity&gt;.</returns>
        public static TableDefinition<TEntity> GetTableDefinition<TEntity>() where TEntity : class, new()
        {
            TableDefinition<TEntity> tableDefinition = null;
            var entityName = typeof(TEntity).Name;
            var table = SystemGlobals.LookupContext.TableDefinitions
                .FirstOrDefault(p => p.EntityName == entityName);

            if (table is TableDefinition<TEntity> fullTable)
            {
                tableDefinition = fullTable;
            }

            return tableDefinition;
        }

        /// <summary>
        /// Gets the type.
        /// </summary>
        /// <typeparam name="TEntity">The type of the t entity.</typeparam>
        /// <param name="property">The property.</param>
        /// <returns>Type.</returns>
        private static Type GetType<TEntity>(string property)
        {
            Type type = typeof(TEntity);
            ParameterExpression arg = Expression.Parameter(type, "x");
            Expression expr = arg;

            string[] props = property.Split('.');
            foreach (string prop in props)
            {
                // use reflection (not ComponentModel) to mirror LINQ
                PropertyInfo pi = type.GetProperty(prop);
                expr = Expression.Property(expr, pi);
                type = pi.PropertyType;
            }
            return type;

        }


        /// <summary>
        /// Applies the order.
        /// </summary>
        /// <typeparam name="TEntity">The type of the t entity.</typeparam>
        /// <param name="source">The source.</param>
        /// <param name="methodType">Type of the method.</param>
        /// <param name="property">The property.</param>
        /// <returns>IQueryable&lt;TEntity&gt;.</returns>
        /// <exception cref="System.ArgumentOutOfRangeException">methodType - null</exception>
        /// <exception cref="System.ArgumentNullException">source</exception>
        public static IQueryable<TEntity> ApplyOrder<TEntity>(IQueryable<TEntity> source, OrderMethods methodType, string property)
        {
            var list = new List<TEntity>();
            var methodName = "OrderBy";
            switch (methodType)
            {
                case OrderMethods.OrderBy:
                    break;
                case OrderMethods.ThenBy:
                    methodName = "ThenBy";
                    break;
                case OrderMethods.OrderByDescending:
                    methodName = "OrderByDescending";
                    break;
                case OrderMethods.ThenByDescending:
                    methodName = "ThenByDescending";
                    break;
                default:
                    throw new ArgumentOutOfRangeException(nameof(methodType), methodType, null);
            }
            if (source == null)
                throw new ArgumentNullException("source");

            if (string.IsNullOrEmpty(property))
                return source;

            var lambda = GetLambda<TEntity>(property);

            object result = typeof(Queryable).GetMethods().Single(
                    method => method.Name == methodName
                              && method.IsGenericMethodDefinition
                              && method.GetGenericArguments().Length == 2
                              && method.GetParameters().Length == 2)
                .MakeGenericMethod(typeof(TEntity), GetType<TEntity>(property))
                .Invoke(null, new object[] { source, lambda });

            var queryAble = (IQueryable<TEntity>)result;
            return queryAble;
        }

        /// <summary>
        /// Gets the type of the property.
        /// </summary>
        /// <typeparam name="TEntity">The type of the t entity.</typeparam>
        /// <param name="property">The property.</param>
        /// <returns>Type.</returns>
        public static Type GetPropertyType<TEntity>(string property)
        {
            var type = typeof(TEntity);
            string[] props = property.Split('.');
            foreach (string prop in props)
            {
                // use reflection (not ComponentModel) to mirror LINQ
                PropertyInfo pi = type.GetProperty(prop);
                type = pi.PropertyType;
            }

            return type;
        }

        /// <summary>
        /// Gets the property information.
        /// </summary>
        /// <typeparam name="TEntity">The type of the t entity.</typeparam>
        /// <param name="property">The property.</param>
        /// <returns>PropertyInfo.</returns>
        public static PropertyInfo GetPropertyInfo<TEntity>(string property)
        {
            PropertyInfo result  = null;
            var type = typeof(TEntity);
            string[] props = property.Split('.');
            foreach (string prop in props)
            {
                // use reflection (not ComponentModel) to mirror LINQ
                PropertyInfo pi = type.GetProperty(prop);
                type = pi.PropertyType;
                result = pi;
            }
            return result;
        }

        /// <summary>
        /// Gets the type of the nullable.
        /// </summary>
        /// <param name="type">The type.</param>
        /// <returns>Type.</returns>
        public static Type GetNullableType(Type type)
        {
            // Use Nullable.GetUnderlyingType() to remove the Nullable<T> wrapper if type is already nullable.
            type = Nullable.GetUnderlyingType(type) ?? type; // avoid type becoming null
            if (type.IsValueType)
                return typeof(Nullable<>).MakeGenericType(type);
            else
                return type;
        }

        /// <summary>
        /// Gets the lambda.
        /// </summary>
        /// <typeparam name="TEntity">The type of the t entity.</typeparam>
        /// <param name="property">The property.</param>
        /// <returns>LambdaExpression.</returns>
        public static LambdaExpression GetLambda<TEntity>(string property)
        {
            System.Type type = typeof(TEntity);
            ParameterExpression arg = Expression.Parameter(type, "x");
            Expression expr = arg;

            string[] props = property.Split('.');
            foreach (string prop in props)
            {
                // use reflection (not ComponentModel) to mirror LINQ
                PropertyInfo pi = type.GetProperty(prop);
                expr = Expression.Property(expr, pi);
                type = pi.PropertyType;
            }

            if (type == typeof(double))
            {
                
            }
            System.Type delegateType = typeof(Func<,>).MakeGenericType(typeof(TEntity), type);
            LambdaExpression lambda = Expression.Lambda(delegateType, expr, arg);
            return lambda;
        }

        /// <summary>
        /// Gets the parameter expression.
        /// </summary>
        /// <typeparam name="TEntity">The type of the t entity.</typeparam>
        /// <returns>ParameterExpression.</returns>
        public static ParameterExpression GetParameterExpression<TEntity>()
        {
            var param = Expression.Parameter(typeof(TEntity), "p");

            return param;
        }

        public static HashSet<T> CreateList<T>(params T[] elements)
        {
            var list = new List<T>(elements);
            return new HashSet<T>(list);
        }
    }
}
