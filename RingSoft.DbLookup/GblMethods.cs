using System;
using System.Collections.Generic;
using System.Data;
using System.Diagnostics;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;
using Newtonsoft.Json;
using RingSoft.DbLookup.QueryBuilder;
using RingSoft.DataEntryControls.Engine;
using RingSoft.DbLookup.ModelDefinition;
using RingSoft.Printing.Interop;
using Google.Protobuf.WellKnownTypes;
using RingSoft.DbLookup.DataProcessor;
using Enum = System.Enum;
using Type = System.Type;

namespace RingSoft.DbLookup
{
    public enum FieldDataTypes
    {
        String = 0,
        Integer = 1,
        Decimal = 2,
        //Enum = 3,
        DateTime = 4,
        Bool = 5,
        Memo = 6
    }

    public static class GblMethods
    {
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
        /// <param name="decimals">The number of digits to the right of the decimal point.</param>
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
        /// <returns></returns>
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

        public static string FormatDateValue(this DateTime value, DbDateTypes dateType, bool fullString = true)
        {
            string formatString;
            if (fullString)
            {
                switch (dateType)
                {
                    case DbDateTypes.DateOnly:
                        formatString = "MM/dd/yyyy";
                        break;
                    case DbDateTypes.DateTime:
                        formatString = "MM/dd/yyyy hh:mm:ss tt";
                        break;
                    case DbDateTypes.Millisecond:
                        formatString = "MM/dd/yyyy hh:mm:ss tt.fff";
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
                        formatString = "M/d/yyyy";
                        break;
                    case DbDateTypes.DateTime:
                        formatString = "M/d/yyyy hh:mm:ss tt";
                        break;
                    case DbDateTypes.Millisecond:
                        formatString = "M/d/yyyy hh:mm:ss tt.fff";
                        break;
                    default:
                        throw new ArgumentOutOfRangeException(nameof(value), value, null);

                }
            }

            return value.ToString(formatString);
        }

        /// <summary>
        /// Formats the value.
        /// </summary>
        /// <param name="dataType">Type of the data.</param>
        /// <param name="value">The value to format.</param>
        /// <param name="formatString">The format string.</param>
        /// <param name="culture">The culture.</param>
        /// <returns></returns>
        /// <exception cref="ArgumentOutOfRangeException"></exception>
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
        /// <exception cref="ArgumentException">Column {columnName} does not exist in the DataRow.</exception>
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
                else if (property.PropertyType == typeof(decimal)
                         || property.PropertyType == typeof(decimal?))
                {
                    decimal checkValue;
                    if (Decimal.TryParse(value, out checkValue))
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

                else if (property.PropertyType == typeof(bool))
                {
                    bool checkValue;
                    if (Boolean.TryParse(value, out checkValue))
                        property.SetValue(model, checkValue);
                }
            }
        }

        public static string GetPropertyValue<T>(T model, string propertyName) where T : new()
        {
            var property =
                model.GetType().GetProperties().FirstOrDefault(f => f.Name == propertyName);
            if (property != null)
            {
                var value = property.GetValue(model);
                if (value != null)
                    return property.GetValue(model).ToString();
            }

            return String.Empty;
        }

        public static FieldDataTypes GetFieldDataTypeForType(Type type)
        {
            if (type == typeof(DateTime)
                || type == typeof(DateTime?))
            {
                return FieldDataTypes.DateTime;
            }
            else if (type == typeof(decimal)
                     || type == typeof(decimal?)
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

        public static bool ValidatePrintingFile()
        {
            if (GetPrintingInputExeFileName().IsNullOrEmpty())
            {
                var message =
                    "The RingSoft printing app has not been installed. Would you like to download and install it?";
                var caption = "Printing";
                if (ControlsGlobals.UserInterface.ShowYesNoMessageBox(message, caption, true) ==
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

        public static bool DoRecordLock(PrimaryKeyValue primaryKey)
        {
            var result = true;

            var recordLockTableField =
                SystemGlobals.AdvancedFindLookupContext.RecordLocks.GetFieldDefinition(p => p.Table);
            var recordLockPkField =
                SystemGlobals.AdvancedFindLookupContext.RecordLocks.GetFieldDefinition(p => p.PrimaryKey);
            var recordLockDateField =
                SystemGlobals.AdvancedFindLookupContext.RecordLocks.GetFieldDefinition(p => p.LockDateTime);
            var recordLockUserField = SystemGlobals.AdvancedFindLookupContext.RecordLocks.GetFieldDefinition(p => p.User);

            var sqlDatas = new List<SqlData>();
            var sqlData = new SqlData(recordLockTableField.FieldName,
                primaryKey.TableDefinition.TableName, ValueTypes.String);
            sqlDatas.Add(sqlData);
            sqlData = new SqlData(recordLockPkField.FieldName, primaryKey.KeyString, ValueTypes.String);
            sqlDatas.Add(sqlData);
            sqlData = new SqlData(recordLockDateField.FieldName, GetNowDateText(), ValueTypes.DateTime,
                DbDateTypes.Millisecond);
            sqlDatas.Add(sqlData);
            sqlData = new SqlData(recordLockUserField.FieldName, SystemGlobals.UserName, ValueTypes.String);
            sqlDatas.Add(sqlData);

            var selectQuery = new SelectQuery(recordLockTableField.TableDefinition.TableName);
            selectQuery.AddWhereItem(recordLockTableField.FieldName, Conditions.Equals,
                primaryKey.TableDefinition.TableName);

            selectQuery.AddWhereItem(recordLockPkField.FieldName, Conditions.Equals, primaryKey.KeyString);

            var getDataResult = primaryKey.TableDefinition.Context.DataProcessor.GetData(selectQuery);
            if (getDataResult.ResultCode == GetDataResultCodes.Success)
            {
                var sql = string.Empty;
                if (getDataResult.DataSet.Tables[0].Rows.Count >= 1)
                {
                    var dataRow = getDataResult.DataSet.Tables[0].Rows[0];
                    var recordLockPrimaryKey = new PrimaryKeyValue(SystemGlobals.AdvancedFindLookupContext.RecordLocks);
                    recordLockPrimaryKey.PopulateFromDataRow(dataRow);
                    var updateStatement = new UpdateDataStatement(recordLockPrimaryKey);
                    foreach (var data in sqlDatas)
                    {
                        updateStatement.AddSqlData(data);
                    }

                    sql = primaryKey.TableDefinition.Context.DataProcessor.SqlGenerator.GenerateUpdateSql(
                        updateStatement);
                }
                else
                {
                    var insertStatement = new InsertDataStatement(recordLockPkField.TableDefinition);
                    foreach (var data in sqlDatas)
                    {
                        insertStatement.AddSqlData(data);
                    }

                    sql = primaryKey.TableDefinition.Context.DataProcessor.SqlGenerator.GenerateInsertSqlStatement(
                        insertStatement);
                }
                result = primaryKey.TableDefinition.Context.DataProcessor.ExecuteSql(sql).ResultCode == GetDataResultCodes.Success;
            }
            
            return result;
        }

        public static string GetNowDateText()
        {
            var newDate = DateTime.Now.ToUniversalTime();
            var dateText = newDate.ToString("yyyy-MM-dd HH:mm:ss.fff");
            return dateText;
        }

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
    }
}
