using RingSoft.DbLookup.QueryBuilder;
using System;
using System.ComponentModel;
using System.Globalization;
using RingSoft.DataEntryControls.Engine;
using RingSoft.DbLookup.AdvancedFind;
using RingSoft.DbLookup.ModelDefinition;
using RingSoft.DbLookup.ModelDefinition.FieldDefinitions;
using RingSoft.DbLookup.DataProcessor;
using RingSoft.DbLookup.TableProcessing;

namespace RingSoft.DbLookup.Lookup
{
    /// <summary>
    /// A lookup column based on a formula.
    /// </summary>
    /// <seealso cref="LookupColumnDefinitionBase" />
    public class LookupFormulaColumnDefinition : LookupColumnDefinitionType<LookupFormulaColumnDefinition>
    {
        /// <summary>
        /// Gets the type of the column.
        /// </summary>
        /// <value>
        /// The type of the column.
        /// </value>
        public override LookupColumnTypes ColumnType => LookupColumnTypes.Formula;

        /// <summary>
        /// Gets the type of the data.
        /// </summary>
        /// <value>
        /// The type of the data.
        /// </value>
        public override FieldDataTypes DataType => _dataType;

        /// <summary>
        /// Gets the type of the value.
        /// </summary>
        /// <value>
        /// The type of the value.
        /// </value>
        public ValueTypes ValueType => GblMethods.GetValueTypeForFieldDataType(DataType);

        /// <summary>
        /// Gets the select SQL alias.
        /// </summary>
        /// <value>
        /// The select SQL alias.
        /// </value>
        public override string SelectSqlAlias => _selectSqlAlias;

        /// <summary>
        /// Gets the number format string.
        /// </summary>
        /// <value>
        /// The number format string.
        /// </value>
        public string NumberFormatString { get; internal set; }

        /// <summary>
        /// Gets the date format string.
        /// </summary>
        /// <value>
        /// The date format string.
        /// </value>
        public string DateFormatString { get; private set; }

        /// <summary>
        /// Gets the formula.
        /// </summary>
        /// <value>
        /// The formula.
        /// </value>

        private string _formula;

        public string Formula
        {
            get
            {
                var alias = JoinQueryTableAlias;
                if (alias.IsNullOrEmpty())
                {
                    alias = LookupDefinition.TableDefinition.TableName;
                }

                if (!_formula.IsNullOrEmpty())
                {
                    return _formula.Replace("{Alias}", alias);
                }
                return string.Empty;
            }
            set
            {
                _formula = value;
                OriginalFormula = value;
            }
        }

        public string OriginalFormula { get; private set; }

        private CultureInfo _columnCulture;

        /// <summary>
        /// Gets the culture.
        /// </summary>
        /// <value>
        /// The culture.
        /// </value>
        public CultureInfo ColumnCulture
        {
            get
            {
                if (_columnCulture == null)
                {
                    if (DataType == FieldDataTypes.DateTime)
                        return LookupDefaults.DefaultDateCulture;
                    else
                    {
                        return LookupDefaults.DefaultNumberCulture;
                    }
                }

                return _columnCulture;
            }
            private set => _columnCulture = value;
        }

        /// <summary>
        /// Gets the number of digits to the right of the decimal point.
        /// </summary>
        /// <value>
        /// The decimal count.
        /// </value>
        public int DecimalCount { get; internal set; }

        /// <summary>
        /// Gets the type of the decimal field.
        /// </summary>
        /// <value>
        /// The type of the decimal field.
        /// </value>
        public DecimalFieldTypes DecimalFieldType { get; set; }

        /// <summary>
        /// Gets the type of the date.
        /// </summary>
        /// <value>
        /// The type of the date.
        /// </value>
        public DbDateTypes DateType { get; set; }

        public string Description { get; private set; }

        public TableDefinitionBase PrimaryTable { get; set; }

        public FieldDefinition PrimaryField { get; set; }

        public bool ConvertToLocalTime { get; private set; }

        private readonly string _selectSqlAlias;
        private FieldDataTypes _dataType;

        /// <summary>
        /// Initializes a new instance of the <see cref="LookupFormulaColumnDefinition"/> class.
        /// </summary>
        /// <param name="formula">The formula.</param>
        /// <param name="dataType">Type of the data.</param>
        internal LookupFormulaColumnDefinition(string formula, FieldDataTypes dataType)
        {
            Formula = formula;
            _selectSqlAlias = Guid.NewGuid().ToString().Replace("-", "").ToUpper();
            _dataType = dataType;

            DateFormatString = LookupDefaults.DefaultDateCulture.DateTimeFormat.ShortDatePattern;
            DecimalCount = LookupDefaults.DefaultDecimalCount;

            SetupColumn();
        }

        public override TreeViewType TreeViewType => TreeViewType.Formula;

        internal override void CopyFrom(LookupColumnDefinitionBase source)
        {
            if (source is LookupFormulaColumnDefinition formulaSource)
            {
                Formula = formulaSource._formula;
                NumberFormatString = formulaSource.NumberFormatString;
                DateFormatString = formulaSource.DateFormatString;
                ColumnCulture = formulaSource.ColumnCulture;
                DecimalCount = formulaSource.DecimalCount;
                DecimalFieldType = formulaSource.DecimalFieldType;
                DateType = formulaSource.DateType;
                JoinQueryTableAlias = formulaSource.JoinQueryTableAlias;
                PrimaryTable = formulaSource.PrimaryTable;
                PrimaryField = formulaSource.PrimaryField;
                ParentObject =formulaSource.ParentObject;
                ConvertToLocalTime = formulaSource.ConvertToLocalTime;
                if (formulaSource.Caption == "Difference")
                {
                    
                }
            }
            base.CopyFrom(source);
        }

        public LookupFormulaColumnDefinition UpdateFormula(string formula)
        {
            Formula = formula;
            return this;
        }


        /// <summary>
        /// Sets the number format string.
        /// </summary>
        /// <param name="value">The number format string.</param>
        /// <returns>This object.</returns>
        public LookupFormulaColumnDefinition HasNumberFormatString(string value)
        {
            var number = 100000.12;
            var format = $"{{0:{value}}}";
            try
            {
                var checkFormat = string.Format(format, number);
                var unused = double.Parse(GblMethods.NumTextToString(checkFormat));
            }
            catch (Exception)
            {
                throw new ArgumentException("Invalid format string.");
            }

            NumberFormatString = value;
            return this;
        }

        /// <summary>
        /// Sets the date format string.
        /// </summary>
        /// <param name="value">The date format string.</param>
        /// <returns>This object.</returns>
        public LookupFormulaColumnDefinition HasDateFormatString(string value)
        {
            var date = new DateTime(2000, 01, 01);
            var format = $"{{0:{value}}}";
            try
            {
                var dateCheckFormat = string.Format(format, date);
                var unused = DateTime.Parse(dateCheckFormat);
            }
            catch (Exception)
            {
                throw new ArgumentException("Invalid date format string.");
            }
            DateFormatString = value;
            return this;
        }

        /// <summary>
        /// Sets the column culture identifier.
        /// </summary>
        /// <param name="cultureId">The culture identifier.</param>
        /// <returns></returns>
        public LookupFormulaColumnDefinition HasColumnCultureId(string cultureId)
        {
            ColumnCulture = new CultureInfo(cultureId);
            DecimalFieldDefinition.FormatCulture(ColumnCulture);
            return this;
        }

        /// <summary>
        /// Formats the value to display in the lookup view.
        /// </summary>
        /// <param name="value">The value from the database.</param>
        /// <returns>
        /// The formatted value.
        /// </returns>
        /// <exception cref="NotImplementedException"></exception>
        public override string FormatValue(string value)
        {
            switch (DataType)
            {
                case FieldDataTypes.String:
                    break;
                case FieldDataTypes.Integer:
                case FieldDataTypes.Decimal:
                    return DecimalFieldDefinition.FormatNumericValue(value, NumberFormatString, DecimalFieldType,
                        DecimalCount, ColumnCulture);
                case FieldDataTypes.DateTime:
                    return DateFieldDefinition.FormatDateValue(value, DateFormatString, DateType, ColumnCulture, ConvertToLocalTime);
                case FieldDataTypes.Bool:
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }

            return GblMethods.FormatValue(DataType, value);
        }

        public override string GetTextForColumn(PrimaryKeyValue primaryKeyValue)
        {
            var query = new SelectQuery(primaryKeyValue.TableDefinition.TableName);
            if (!LookupDefinition.FromFormula.IsNullOrEmpty())
            {
                query.BaseTable.Formula = LookupDefinition.FromFormula;
            }

            query.AddSelectFormulaColumn("Formula",
                OriginalFormula.Replace("{Alias}", primaryKeyValue.TableDefinition.TableName));
            var test = this;
            foreach (var primaryKeyField in primaryKeyValue.KeyValueFields)
            {
                query.AddWhereItem(primaryKeyField.FieldDefinition.FieldName, Conditions.Equals, primaryKeyField.Value);
            }

            var dataProcessResult = primaryKeyValue.TableDefinition.Context.DataProcessor.GetData(query);
            if (dataProcessResult.ResultCode == GetDataResultCodes.Success)
            {
                var result = dataProcessResult.DataSet.Tables[0].Rows[0].GetRowValue("Formula");
                if (DataType == FieldDataTypes.DateTime)
                {
                    var date = DateTime.MinValue;
                    if (DateTime.TryParse(result, out date))
                    {
                        result = FormatValue(date.ToString());
                    }
                }
                return result;
            }

            return "";
        }

        /// <summary>
        /// Sets the number of digits to the right of the decimal point.
        /// </summary>
        /// <param name="value">The new digits value.</param>
        /// <returns>This object.</returns>
        public LookupFormulaColumnDefinition HasDecimalCount(int value)
        {
            DecimalCount = value;
            return this;
        }

        /// <summary>
        /// Sets the type of this decimal field.
        /// </summary>
        /// <param name="value">The new DecimalFieldTypes value.</param>
        /// <returns>This object.</returns>
        public LookupFormulaColumnDefinition HasDecimalFieldType(DecimalFieldTypes value)
        {
            DecimalFieldType = value;
            return this;
        }

        /// <summary>
        /// Sets the type of the date.
        /// </summary>
        /// <param name="value">The new DbDateTypes value.</param>
        /// <returns>This object.</returns>
        public LookupFormulaColumnDefinition HasDateType(DbDateTypes value)
        {
            DateType = value;
            return this;
        }

        public LookupFormulaColumnDefinition HasDescription(string value)
        {
            Description = value;
            return this;
        }

        public LookupFormulaColumnDefinition HasDataType(FieldDataTypes value)
        {
            _dataType = value;
            return this;
        }

        public LookupFormulaColumnDefinition HasConvertToLocalTime(bool value = true)
        {
            ConvertToLocalTime = value;
            return this;
        }

        internal override string GetFormulaForColumn()
        {
            return OriginalFormula;
        }
    }
}
