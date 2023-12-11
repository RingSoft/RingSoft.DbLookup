// ***********************************************************************
// Assembly         : RingSoft.DbLookup
// Author           : petem
// Created          : 12-19-2022
//
// Last Modified By : petem
// Last Modified On : 07-23-2023
// ***********************************************************************
// <copyright file="LookupFormulaColumnDefinition.cs" company="Peter Ringering">
//     Copyright (c) . All rights reserved.
// </copyright>
// <summary></summary>
// ***********************************************************************
using RingSoft.DbLookup.QueryBuilder;
using System;
using System.ComponentModel;
using System.Globalization;
using System.Linq;
using RingSoft.DataEntryControls.Engine;
using RingSoft.DbLookup.AdvancedFind;
using RingSoft.DbLookup.ModelDefinition;
using RingSoft.DbLookup.ModelDefinition.FieldDefinitions;
using RingSoft.DbLookup.DataProcessor;
using RingSoft.DbLookup.TableProcessing;

namespace RingSoft.DbLookup.Lookup
{
    /// <summary>
    /// Interface ILookupFormula
    /// </summary>
    public interface ILookupFormula
    {
        /// <summary>
        /// Gets the identifier.
        /// </summary>
        /// <value>The identifier.</value>
        int Id { get; }

        /// <summary>
        /// Gets the database value.
        /// </summary>
        /// <param name="entity">The entity.</param>
        /// <returns>System.String.</returns>
        string GetDatabaseValue(object entity);
    }
    /// <summary>
    /// A lookup column based on a formula.
    /// </summary>
    /// <seealso cref="LookupColumnDefinitionBase" />
    public class LookupFormulaColumnDefinition : LookupColumnDefinitionType<LookupFormulaColumnDefinition>
    {
        /// <summary>
        /// Gets the type of the column.
        /// </summary>
        /// <value>The type of the column.</value>
        public override LookupColumnTypes ColumnType => LookupColumnTypes.Formula;

        /// <summary>
        /// Gets the type of the data.
        /// </summary>
        /// <value>The type of the data.</value>
        public override FieldDataTypes DataType => _dataType;

        /// <summary>
        /// Gets the type of the value.
        /// </summary>
        /// <value>The type of the value.</value>
        public ValueTypes ValueType => GblMethods.GetValueTypeForFieldDataType(DataType);

        /// <summary>
        /// Gets the select SQL alias.
        /// </summary>
        /// <value>The select SQL alias.</value>
        public override string SelectSqlAlias => _selectSqlAlias;

        /// <summary>
        /// Gets the number format string.
        /// </summary>
        /// <value>The number format string.</value>
        public string NumberFormatString { get; internal set; }

        /// <summary>
        /// Gets the date format string.
        /// </summary>
        /// <value>The date format string.</value>
        public string DateFormatString { get; private set; }

        /// <summary>
        /// Gets the formula.
        /// </summary>
        /// <value>
        /// The formula.
        /// </value>

        private string _formula;

        /// <summary>
        /// Gets or sets the formula.
        /// </summary>
        /// <value>The formula.</value>
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

        /// <summary>
        /// Gets the original formula.
        /// </summary>
        /// <value>The original formula.</value>
        public string OriginalFormula { get; private set; }

        /// <summary>
        /// The column culture
        /// </summary>
        private CultureInfo _columnCulture;

        /// <summary>
        /// Gets the culture.
        /// </summary>
        /// <value>The culture.</value>
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
        /// Gets the number of digits to the right of the double point.
        /// </summary>
        /// <value>The double count.</value>
        public int DecimalCount { get; internal set; }

        /// <summary>
        /// Gets the type of the double field.
        /// </summary>
        /// <value>The type of the double field.</value>
        public DecimalFieldTypes DecimalFieldType { get; set; }

        /// <summary>
        /// Gets the type of the date.
        /// </summary>
        /// <value>The type of the date.</value>
        public DbDateTypes DateType { get; set; }

        /// <summary>
        /// Gets the description.
        /// </summary>
        /// <value>The description.</value>
        public string Description { get; private set; }

        /// <summary>
        /// Gets or sets the primary table.
        /// </summary>
        /// <value>The primary table.</value>
        public TableDefinitionBase PrimaryTable { get; set; }

        /// <summary>
        /// Gets or sets the primary field.
        /// </summary>
        /// <value>The primary field.</value>
        public FieldDefinition PrimaryField { get; set; }

        /// <summary>
        /// Gets a value indicating whether [convert to local time].
        /// </summary>
        /// <value><c>true</c> if [convert to local time]; otherwise, <c>false</c>.</value>
        public bool ConvertToLocalTime { get; private set; }

        /// <summary>
        /// Gets the formula object.
        /// </summary>
        /// <value>The formula object.</value>
        public ILookupFormula FormulaObject { get; private set; }

        /// <summary>
        /// Gets a value indicating whether [allow nulls].
        /// </summary>
        /// <value><c>true</c> if [allow nulls]; otherwise, <c>false</c>.</value>
        public bool AllowNulls { get; internal set; }

        /// <summary>
        /// The select SQL alias
        /// </summary>
        private readonly string _selectSqlAlias;
        /// <summary>
        /// The data type
        /// </summary>
        private FieldDataTypes _dataType;


        /// <summary>
        /// Initializes a new instance of the <see cref="LookupFormulaColumnDefinition" /> class.
        /// </summary>
        /// <param name="formulaObject">The formula object.</param>
        /// <param name="dataType">Type of the data.</param>
        internal LookupFormulaColumnDefinition(ILookupFormula formulaObject, FieldDataTypes dataType)
        {
            _selectSqlAlias = Guid.NewGuid().ToString().Replace("-", "").ToUpper();
            _dataType = dataType;

            DateType = DbDateTypes.DateTime;
            //DateFormatString = LookupDefaults.DefaultDateCulture.DateTimeFormat.ShortDatePattern;
            DecimalCount = LookupDefaults.DefaultDecimalCount;

            SetupColumn();
            FormulaObject = formulaObject;
        }

        /// <summary>
        /// Processes the new visible column.
        /// </summary>
        /// <param name="columnDefinition">The column definition.</param>
        /// <param name="lookupDefinition">The lookup definition.</param>
        /// <param name="copyFrom">if set to <c>true</c> [copy from].</param>
        protected internal override void ProcessNewVisibleColumn(LookupColumnDefinitionBase columnDefinition, LookupDefinitionBase lookupDefinition,
            bool copyFrom = true)
        {
            base.ProcessNewVisibleColumn(columnDefinition, lookupDefinition, copyFrom);
            LookupDefinition.TableDefinition.Context.RegisterLookupFormula(FormulaObject);
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="LookupFormulaColumnDefinition"/> class.
        /// </summary>
        internal LookupFormulaColumnDefinition()
        {
            _selectSqlAlias = Guid.NewGuid().ToString().Replace("-", "").ToUpper();
        }

        /// <summary>
        /// Gets the type of the TreeView.
        /// </summary>
        /// <value>The type of the TreeView.</value>
        public override TreeViewType TreeViewType => TreeViewType.Formula;

        /// <summary>
        /// Copies from.
        /// </summary>
        /// <param name="source">The source.</param>
        internal override void CopyFrom(LookupColumnDefinitionBase source)
        {
            if (source is LookupFormulaColumnDefinition formulaSource)
            {
                Formula = formulaSource._formula;
                FormulaObject = formulaSource.FormulaObject;
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
                AllowNulls = formulaSource.AllowNulls;
                if (formulaSource.Caption == "Difference")
                {
                    
                }
            }
            base.CopyFrom(source);
        }

        /// <summary>
        /// Updates the formula.
        /// </summary>
        /// <param name="formula">The formula.</param>
        /// <returns>LookupFormulaColumnDefinition.</returns>
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
        /// <exception cref="System.ArgumentException">Invalid format string.</exception>
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
        /// <exception cref="System.ArgumentException">Invalid date format string.</exception>
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
        /// <returns>LookupFormulaColumnDefinition.</returns>
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
        /// <returns>The formatted value.</returns>
        /// <exception cref="System.ArgumentOutOfRangeException"></exception>
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
                    var convertToLocalTime = ConvertToLocalTime;
                    if (!convertToLocalTime)
                    {
                        convertToLocalTime = SystemGlobals.ConvertAllDatesToUniversalTime;
                    }
                    return DateFieldDefinition.FormatDateValue(value, DateFormatString, DateType, ColumnCulture, convertToLocalTime);
                case FieldDataTypes.Bool:
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }

            return GblMethods.FormatValue(DataType, value);
        }

        /// <summary>
        /// Gets the text for column.
        /// </summary>
        /// <param name="primaryKeyValue">The primary key value.</param>
        /// <returns>System.String.</returns>
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
        /// Sets the number of digits to the right of the double point.
        /// </summary>
        /// <param name="value">The new digits value.</param>
        /// <returns>This object.</returns>
        public LookupFormulaColumnDefinition HasDecimalCount(int value)
        {
            DecimalCount = value;
            return this;
        }

        /// <summary>
        /// Sets the type of this double field.
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

        /// <summary>
        /// Determines whether the specified value has description.
        /// </summary>
        /// <param name="value">The value.</param>
        /// <returns>LookupFormulaColumnDefinition.</returns>
        public LookupFormulaColumnDefinition HasDescription(string value)
        {
            Description = value;
            return this;
        }

        /// <summary>
        /// Determines whether [has data type] [the specified value].
        /// </summary>
        /// <param name="value">The value.</param>
        /// <returns>LookupFormulaColumnDefinition.</returns>
        public LookupFormulaColumnDefinition HasDataType(FieldDataTypes value)
        {
            _dataType = value;
            return this;
        }

        /// <summary>
        /// Determines whether [has convert to local time] [the specified value].
        /// </summary>
        /// <param name="value">if set to <c>true</c> [value].</param>
        /// <returns>LookupFormulaColumnDefinition.</returns>
        public LookupFormulaColumnDefinition HasConvertToLocalTime(bool value = true)
        {
            ConvertToLocalTime = value;
            return this;
        }

        /// <summary>
        /// Gets the formula for column.
        /// </summary>
        /// <returns>ILookupFormula.</returns>
        internal override ILookupFormula GetFormulaForColumn()
        {
            return FormulaObject;
        }

        /// <summary>
        /// Adds the new column definition.
        /// </summary>
        /// <param name="lookupDefinition">The lookup definition.</param>
        public override void AddNewColumnDefinition(LookupDefinitionBase lookupDefinition)
        {
            var newColumn = new LookupFormulaColumnDefinition(FormulaObject, DataType);

            newColumn.LookupDefinition = lookupDefinition;
            newColumn.JoinQueryTableAlias = JoinQueryTableAlias;
            ProcessNewVisibleColumn(newColumn, lookupDefinition);
            var test = this;

            base.AddNewColumnDefinition(lookupDefinition);
        }

        /// <summary>
        /// Loads from TreeView item.
        /// </summary>
        /// <param name="item">The item.</param>
        /// <returns>System.String.</returns>
        internal override string LoadFromTreeViewItem(TreeViewItem item)
        {
            if (item.FieldDefinition != null)
            {
                FormulaObject = item.FieldDefinition.FormulaObject;
                AllowNulls = item.FieldDefinition.AllowNulls;
            }
            var result = base.LoadFromTreeViewItem(item);
            if (item.FieldDefinition != null && !item.FieldDefinition.AllowRecursion)
            {
                TableDescription = item.Name;
            }

            return result;
        }

        /// <summary>
        /// Returns a <see cref="System.String" /> that represents this instance.
        /// </summary>
        /// <returns>A <see cref="System.String" /> that represents this instance.</returns>
        public override string ToString()
        {
            if (Caption.IsNullOrEmpty())
            {
                return Formula;
            }
            return base.ToString();
        }

        /// <summary>
        /// Saves to entity.
        /// </summary>
        /// <param name="entity">The entity.</param>
        public override void SaveToEntity(AdvancedFindColumn entity)
        {
            entity.Formula = FormulaObject.Id.ToString();
            entity.FieldDataType = (byte)DataType;
            entity.PrimaryFieldName = Description;
            var dateTypeInt = (int)DateType;
            entity.PrimaryTableName = dateTypeInt.ToString();
            entity.DecimalFormatType = (byte)DecimalFieldType;

            base.SaveToEntity(entity);
        }

        /// <summary>
        /// Loads from entity.
        /// </summary>
        /// <param name="entity">The entity.</param>
        /// <param name="lookupDefinition">The lookup definition.</param>
        /// <exception cref="System.ArgumentOutOfRangeException"></exception>
        internal override void LoadFromEntity(AdvancedFindColumn entity, LookupDefinitionBase lookupDefinition)
        {
            var formulaId = entity.Formula.ToInt();
            if (formulaId == 0)
            {

            }
            else
            {
                FormulaObject = lookupDefinition.TableDefinition.Context.FormulaRegistry
                    .FirstOrDefault(p => p.Id == formulaId);
            }

            _dataType = (FieldDataTypes)entity.FieldDataType;
            Description = entity.PrimaryFieldName;
            DateType = (DbDateTypes)entity.PrimaryTableName.ToInt();
            DecimalFieldType = (DecimalFieldTypes)entity.DecimalFormatType;
            switch (_dataType)
            {
                case FieldDataTypes.Decimal:
                    switch (DecimalFieldType)
                    {
                        case DecimalFieldTypes.Decimal:
                            DecimalCount = CultureInfo.CurrentCulture.NumberFormat.NumberDecimalDigits;
                            break;
                        case DecimalFieldTypes.Currency:
                            DecimalCount = CultureInfo.CurrentCulture.NumberFormat.CurrencyDecimalDigits;
                            break;
                        case DecimalFieldTypes.Percent:
                            DecimalCount = CultureInfo.CurrentCulture.NumberFormat.PercentDecimalDigits;
                            break;
                        default:
                            throw new ArgumentOutOfRangeException();
                    }
                    break;
            }
            Path = entity.Path;
            if (Path.IsNullOrEmpty())
            {
                var foundItem = lookupDefinition.AdvancedFindTree.TreeRoot
                    .FirstOrDefault(p => p.Type == TreeViewType.Formula);

                if (foundItem != null)
                {
                    LoadFromTreeViewItem(foundItem);
                }
            }

            var test = this;
            base.LoadFromEntity(entity, lookupDefinition);
            ProcessNewVisibleColumn(this, lookupDefinition, false);
            switch (_dataType)
            {
                case FieldDataTypes.Decimal:
                    HasHorizontalAlignmentType(LookupColumnAlignmentTypes.Right);
                    break;
            }
        }

        /// <summary>
        /// Gets the name of the property join.
        /// </summary>
        /// <param name="useDbField">if set to <c>true</c> [use database field].</param>
        /// <returns>System.String.</returns>
        public override string GetPropertyJoinName(bool useDbField = false)
        {
            return string.Empty;
        }

        /// <summary>
        /// Gets the database value.
        /// </summary>
        /// <typeparam name="TEntity">The type of the t entity.</typeparam>
        /// <param name="entity">The entity.</param>
        /// <returns>System.String.</returns>
        public override string GetDatabaseValue<TEntity>(TEntity entity)
        {
            var result = string.Empty;
            var propertyObject = GetPropertyObject(entity);

            if (FormulaObject != null)
            {
                //if (propertyObject != null)
                //{
                //    result = FormulaObject.GetDatabaseValue(propertyObject);
                //}
                //else
                //{
                //    if (!AllowNulls)
                //    {
                //        result = FormulaObject.GetDatabaseValue(entity);
                //    }
                //}
                result = FormulaObject.GetDatabaseValue(entity);
            }

            return result;
        }

        /// <summary>
        /// Gets the formatted value.
        /// </summary>
        /// <typeparam name="TEntity">The type of the t entity.</typeparam>
        /// <param name="entity">The entity.</param>
        /// <returns>System.String.</returns>
        public override string GetFormattedValue<TEntity>(TEntity entity)
        {
            var value = GetDatabaseValue(entity);
            return FormatValue(value);
        }
    }
}
