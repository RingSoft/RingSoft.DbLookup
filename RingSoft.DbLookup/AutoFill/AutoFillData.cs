using RingSoft.DataEntryControls.Engine;
using System;
using System.Data;
using System.Linq;
using RingSoft.DbLookup.DataProcessor;
using RingSoft.DbLookup.Lookup;
using RingSoft.DbLookup.ModelDefinition.FieldDefinitions;
using RingSoft.DbLookup.QueryBuilder;

namespace RingSoft.DbLookup.AutoFill
{
    /// <summary>
    /// Contains all the data to process during data entry in an AutoFill control.
    /// </summary>
    public class AutoFillData
    {
        /// <summary>
        /// Gets the AutoFill definition which defines how this class will operate.
        /// </summary>
        /// <value>
        /// The AutoFill definition.
        /// </value>
        public AutoFillDefinitionBase AutoFillDefinition { get; private set; }

        /// <summary>
        /// Gets the IAutoFillControl interface that gets and displays the data.
        /// </summary>
        /// <value>
        /// The AutoFill control.
        /// </value>
        public IAutoFillControl AutoFillControl { get; }

        /// <summary>
        /// Gets the current PrimaryKeyValue.
        /// </summary>
        /// <value>
        /// The primary key value.
        /// </value>
        public PrimaryKeyValue PrimaryKeyValue { get; private set; }

        /// <summary>
        /// Gets or sets the maximum number of rows in the contains box.
        /// </summary>
        /// <value>
        /// The contains box maximum rows.
        /// </value>
        public int ContainsBoxMaxRows { get; set; } = 5;

        /// <summary>
        /// Gets or sets a value indicating whether to show the contains box.
        /// </summary>
        /// <value>
        ///   <c>true</c> if show contains box; otherwise, <c>false</c>.
        /// </value>
        public bool ShowContainsBox { get; set; } = true;

        /// <summary>
        /// Occurs when this object's data changes.
        /// </summary>
        public event EventHandler<AutoFillDataChangedArgs> AutoFillDataChanged;

        private const string AutoFillTableName = "AutoFill";
        private const string ContainsTableName = "Contains";

        private string _containsText;
        private DataTable _containsBoxDataTable;

        /// <summary>
        /// Initializes a new instance of this class.
        /// </summary>
        /// <param name="autoFillControl">The AutoFill control interface.</param>
        /// <param name="lookupDefinition">The lookup definition used to create the AutoFillDefinition based on the initial sort column definition.</param>
        /// <param name="isDistinct">Set to true if there should only be distinct values.</param>
        /// <exception cref="ArgumentException">Lookup definition does not have any visible columns defined or its initial sort column is null.</exception>
        /// <exception cref="ArgumentOutOfRangeException"></exception>
        public AutoFillData(IAutoFillControl autoFillControl, LookupDefinitionBase lookupDefinition, bool isDistinct)
        {
            if (lookupDefinition.InitialSortColumnDefinition == null)
                throw new ArgumentException(
                    "Lookup definition does not have any visible columns defined or its initial sort column is null.");

            AutoFillControl = autoFillControl;

            AutoFillDefinitionBase autoFillDefinition = null;
            if (lookupDefinition.InitialSortColumnDefinition is LookupFieldColumnDefinition initialFieldColumnDefinition)
            {
                if (initialFieldColumnDefinition.FieldDefinition.TableDefinition != lookupDefinition.TableDefinition)
                {
                    var stringField =
                        lookupDefinition.TableDefinition.FieldDefinitions.FirstOrDefault(p =>
                            p.FieldDataType == FieldDataTypes.String);
                    var columnDefinition = lookupDefinition.AddHiddenColumn(stringField);
                    lookupDefinition.InitialSortColumnDefinition = columnDefinition;
                }
            }
            //else if (lookupDefinition.InitialSortColumnDefinition is LookupFormulaColumnDefinition initialFormulaColumnDefinition)
            //{
            //    var stringField =
            //        lookupDefinition.TableDefinition.FieldDefinitions.FirstOrDefault(p =>
            //            p.FieldDataType == FieldDataTypes.String);
            //    var columnDefinition = lookupDefinition.AddHiddenColumn(stringField);
            //    lookupDefinition.InitialSortColumnDefinition = columnDefinition;
            //}
            switch (lookupDefinition.InitialSortColumnDefinition.ColumnType)
            {
                case LookupColumnTypes.Field:
                    if (lookupDefinition.InitialSortColumnDefinition is LookupFieldColumnDefinition lookupFieldColumn)
                    {
                        AutoFillFieldDefinition autoFillFieldDefinition = null;
                        switch (lookupFieldColumn.FieldDefinition.FieldDataType)
                        {
                            case FieldDataTypes.String:
                                var stringField = lookupFieldColumn.FieldDefinition as StringFieldDefinition;
                                autoFillFieldDefinition = new AutoFillFieldDefinition(stringField);
                                autoFillFieldDefinition.IsDistinct(lookupFieldColumn.Distinct || isDistinct);
                                autoFillDefinition = autoFillFieldDefinition;
                                break;
                            case FieldDataTypes.Integer:
                                break;
                            case FieldDataTypes.Decimal:
                                break;
                            case FieldDataTypes.DateTime:
                                var dateField = lookupFieldColumn.FieldDefinition as DateFieldDefinition;
                                autoFillFieldDefinition = new AutoFillFieldDefinition(dateField);
                                autoFillFieldDefinition.IsDistinct(lookupFieldColumn.Distinct || isDistinct);
                                autoFillDefinition = autoFillFieldDefinition;

                                break;
                            case FieldDataTypes.Bool:
                                break;
                            default:
                                throw new ArgumentOutOfRangeException();
                        }

                    }
                    break;
                case LookupColumnTypes.Formula:
                    if (lookupDefinition.InitialSortColumnDefinition is LookupFormulaColumnDefinition lookupFormulaColumn)
                    {
                        lookupFormulaColumn.JoinQueryTableAlias = null;
                        autoFillDefinition = new AutoFillFormulaDefinition(lookupDefinition.TableDefinition,
                            lookupFormulaColumn.Formula);
                    }
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }

            lookupDefinition.FilterDefinition.FilterCopied += (sender, args) =>
            {
                autoFillDefinition?.FilterDefinition.CopyFrom(args.Source);
            };
            autoFillDefinition?.FilterDefinition.CopyFrom(lookupDefinition.FilterDefinition);

            if (autoFillDefinition != null)
                autoFillDefinition.FromFormula = lookupDefinition.FromFormula;

            Initialize(autoFillDefinition);
        }

        /// <summary>
        /// Initializes a new instance of this class.
        /// </summary>
        /// <param name="autoFillControl">The AutoFill control interface.</param>
        /// <param name="autoFillDefinition">The AutoFillDefinition.</param>
        /// <exception cref="ArgumentException">AutoFill's Field definition cannot be a memo field.</exception>
        /// <exception cref="ArgumentOutOfRangeException"></exception>
        public AutoFillData(IAutoFillControl autoFillControl, AutoFillDefinitionBase autoFillDefinition)
        {
            AutoFillControl = autoFillControl;

            Initialize(autoFillDefinition);
        }

        private void Initialize(AutoFillDefinitionBase autoFillDefinition)
        {
            switch (autoFillDefinition.Type)
            {
                case AutoFillTypes.Field:
                    if (autoFillDefinition is AutoFillFieldDefinition autoFillFieldDefinition)
                    {
                        if (autoFillFieldDefinition.FieldDefinition is StringFieldDefinition stringField)
                        {
                            if (stringField.MemoField)
                                throw new ArgumentException("AutoFill's Field definition cannot be a memo field.");
                        }
                    }
                    break;
                case AutoFillTypes.Formula:
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }

            AutoFillDefinition = autoFillDefinition;
            PrimaryKeyValue = new PrimaryKeyValue(autoFillDefinition.TableDefinition);
        }

        /// <summary>
        /// Called when a keyboard character is pressed.
        /// </summary>
        /// <param name="keyChar">The key character that was pressed.</param>
        /// <returns>True if the character was processed</returns>
        public bool OnKeyCharPressed(char keyChar)
        {
            switch (keyChar)
            {
                case '\b':
                    OnBackspaceKeyDown();
                    return true;
                case '\u001b':  //Escape
                case '\t':
                case '\r':
                case '\n':
                    return false;
            }

            var text = AutoFillControl.EditText;
            var selectionStart = AutoFillControl.SelectionStart;
            var selectionLength = AutoFillControl.SelectionLength;

            var rightText = GetRightText(text, selectionStart, selectionLength);
            var beginText = text.LeftStr(selectionStart) + keyChar + rightText;
            var newText = GetNewText(beginText);

            if (newText.IsNullOrEmpty())
            {
                GetContainsBoxDataTable(beginText);
                AutoFillControl.EditText = beginText;
                AutoFillControl.SelectionStart = selectionStart + 1;
                AutoFillControl.SelectionLength = 0;
            }
            else
            {
                AutoFillControl.EditText = newText;
                AutoFillControl.SelectionStart = selectionStart + 1;
                AutoFillControl.SelectionLength = newText.Length - AutoFillControl.SelectionStart;
            }

            OnOutput();
            return true;
        }

        public void OnTextChanged()
        {
            GetContainsBoxDataTable(AutoFillControl.EditText);
            OnOutput();
        }

        /// <summary>
        /// Called when the delete key is pressed.
        /// </summary>
        public void OnDeleteKeyDown()
        {
            var text = AutoFillControl.EditText;
            if (text.IsNullOrEmpty() || AutoFillControl.SelectionStart > text.Length - 1)
            {
                return;
            }

            var newText = string.Empty;
            var setText = true;
            if (AutoFillControl.SelectionLength == text.Length)
            {
                setText = false;
            }
            var selectionStart = AutoFillControl.SelectionStart;
            var selectionLength = AutoFillControl.SelectionLength;

            if (setText)
            {
                var leftText = text.LeftStr(selectionStart);
                var selectedText = text.MidStr(selectionStart, selectionLength);
                var rightStart = selectionStart + selectionLength;
                if (selectionLength == 0)
                {
                    rightStart++;
                }
                var rightText = text.GetRightText(rightStart, 0);
                newText = leftText + rightText;
            }

            GetContainsBoxDataTable(newText);
            AutoFillControl.EditText = newText;
            AutoFillControl.SelectionStart = selectionStart;
            AutoFillControl.SelectionLength = 0;
            ClearPrimaryKeyValue();

            OnOutput();
        }

        /// <summary>
        /// Called when the backspace key is pressed.
        /// </summary>
        public void OnBackspaceKeyDown()
        {
            var text = AutoFillControl.EditText;
            var selectionStart = AutoFillControl.SelectionStart;
            var selectionLength = AutoFillControl.SelectionLength;

            if (selectionStart == 0)
            {
                return;
            }

            var newSelectStart = selectionStart - 1;
            var newText = text.LeftStr(selectionStart - 1);
            var rightText = GetRightText(text, selectionStart, selectionLength);

            if (selectionLength > 0)
            {
                newText = text.LeftStr(selectionStart);
                newSelectStart = selectionStart;
            }
            newText += rightText;

            GetContainsBoxDataTable(newText);
            AutoFillControl.EditText = newText;
            AutoFillControl.SelectionStart = newSelectStart;
            AutoFillControl.SelectionLength = 0;
            ClearPrimaryKeyValue();

            OnOutput();
        }

        private string GetRightText(string text, int selectionStart, int selectionLength)
        {
            if (text.IsNullOrEmpty())
                return string.Empty;

            return text.RightStr(text.Length - (selectionLength + selectionStart));
        }

        private void OnOutput(bool refreshContainsList = true)
        {
            var args = new AutoFillDataChangedArgs()
            {
                RefreshContainsList =  refreshContainsList,
                ContainsBoxDataTable = _containsBoxDataTable
            };
            AutoFillDataChanged?.Invoke(this, args);
        }

        private string GetNewText(string beginText)
        {
            var newText = string.Empty;
            var autoFillQuery = GetBaseQuery().SetMaxRecords(1);
            autoFillQuery.BaseTable.Formula = AutoFillDefinition.FromFormula;

            switch (AutoFillDefinition.Type)
            {
                case AutoFillTypes.Field:
                    if (AutoFillDefinition is AutoFillFieldDefinition autoFillField)
                    {
                        autoFillQuery.AddWhereItem(autoFillField.FieldDefinition.FieldName, Conditions.BeginsWith, beginText);
                    }
                    break;
                case AutoFillTypes.Formula:
                    if (AutoFillDefinition is AutoFillFormulaDefinition autoFillFormula)
                    {
                        autoFillQuery.AddWhereItemFormula(autoFillFormula.Formula, Conditions.BeginsWith, beginText);
                    }
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }
            
            QuerySet querySet = new QuerySet();
            querySet.AddQuery(autoFillQuery, AutoFillTableName);

            if (ShowContainsBox)
            {
                var containsQuery = GetContainsDataTableQuery(beginText);
                querySet.AddQuery(containsQuery, ContainsTableName);
            }

            var result = AutoFillDefinition.TableDefinition.Context.DataProcessor.GetData(querySet);
            if (result.ResultCode == GetDataResultCodes.Success)
            {
                ClearPrimaryKeyValue();
                var autoFillTextDataTable = result.DataSet.Tables[AutoFillTableName];
                if (autoFillTextDataTable.Rows.Count > 0)
                {
                    var dataRow = autoFillTextDataTable.Rows[0];
                    newText = dataRow.GetRowValue(AutoFillDefinition.SelectSqlAlias);
                    PrimaryKeyValue = new PrimaryKeyValue(AutoFillDefinition.TableDefinition);
                    PrimaryKeyValue.PopulateFromDataRow(dataRow);
                }

                if (ShowContainsBox)
                {
                    _containsText = beginText;
                    _containsBoxDataTable = result.DataSet.Tables[ContainsTableName];
                }
            }

            return newText;
        }

        private SelectQuery GetBaseQuery()
        {
            var query = new SelectQuery(AutoFillDefinition.TableDefinition.TableName);
            query.BaseTable.Formula = AutoFillDefinition.FromFormula;

            AutoFillFieldDefinition autoFillField = null;
            switch (AutoFillDefinition.Type)
            {
                case AutoFillTypes.Field:
                    if (AutoFillDefinition is AutoFillFieldDefinition)
                    {
                        autoFillField = AutoFillDefinition as AutoFillFieldDefinition;
                        if (autoFillField != null)
                            query.AddSelectColumn(autoFillField.FieldDefinition.FieldName,
                                    autoFillField.SelectSqlAlias, autoFillField.Distinct)
                                .AddOrderBySegment(autoFillField.FieldDefinition.FieldName,
                                    OrderByTypes.Ascending);
                    }
                    break;
                case AutoFillTypes.Formula:
                    if (AutoFillDefinition is AutoFillFormulaDefinition autoFillFormula)
                    {
                        query.AddSelectFormulaColumn(autoFillFormula.SelectSqlAlias, autoFillFormula.Formula)
                            .AddOrderByFormulaSegment(autoFillFormula.Formula, OrderByTypes.Ascending);
                    }
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }

            var distinct = false;
            if (autoFillField != null)
                distinct = autoFillField.Distinct;

            if (distinct)
            {
                query.AddSelectColumn(autoFillField.FieldDefinition.FieldName);
            }
            else
            {
                foreach (var primaryKeyField in AutoFillDefinition.TableDefinition.PrimaryKeyFields)
                {
                    query.AddSelectColumn(primaryKeyField.FieldName);
                }
            }

            AutoFillDefinition.FilterDefinition.ProcessQuery(query);

            return query;
        }

        private void GetContainsBoxDataTable(string text)
        {
            var autoFillQuery = GetBaseQuery();

            switch (AutoFillDefinition.Type)
            {
                case AutoFillTypes.Field:
                    if (AutoFillDefinition is AutoFillFieldDefinition autoFillField)
                    {
                        autoFillQuery.AddWhereItem(autoFillField.FieldDefinition.FieldName, Conditions.Equals, text);
                    }
                    break;
                case AutoFillTypes.Formula:
                    if (AutoFillDefinition is AutoFillFormulaDefinition autoFillFormula)
                    {
                        autoFillQuery.AddWhereItemFormula(autoFillFormula.Formula, Conditions.Equals, text);
                    }
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }

            QuerySet querySet = new QuerySet();
            querySet.AddQuery(autoFillQuery, AutoFillTableName);

            if (ShowContainsBox && !text.IsNullOrEmpty())
            {
                var containsQuery = GetContainsDataTableQuery(text);
                querySet.AddQuery(containsQuery, ContainsTableName);
            }

            var result = AutoFillDefinition.TableDefinition.Context.DataProcessor.GetData(querySet);
            if (result.ResultCode == GetDataResultCodes.Success)
            {
                ClearPrimaryKeyValue();
                var autoFillTextDataTable = result.DataSet.Tables[AutoFillTableName];
                if (autoFillTextDataTable.Rows.Count > 0)
                {
                    PrimaryKeyValue.PopulateFromDataRow(autoFillTextDataTable.Rows[0]);
                }

                if (ShowContainsBox)
                {
                    _containsText = text;
                    _containsBoxDataTable = result.DataSet.Tables[ContainsTableName];
                }
            }
        }

        private SelectQuery GetContainsDataTableQuery(string text)
        {
            var query = GetBaseQuery().SetMaxRecords(ContainsBoxMaxRows);
            switch (AutoFillDefinition.Type)
            {
                case AutoFillTypes.Field:
                    if (AutoFillDefinition is AutoFillFieldDefinition autoFillField)
                    {
                        query.AddWhereItem(autoFillField.FieldDefinition.FieldName, Conditions.Contains, text);
                    }
                    break;
                case AutoFillTypes.Formula:
                    if (AutoFillDefinition is AutoFillFormulaDefinition autoFillFormula)
                    {
                        query.AddWhereItemFormula(autoFillFormula.Formula, Conditions.Contains, text);
                    }
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }

            return query;
        }

        /// <summary>
        /// Called when the contains listbox selected index changes.
        /// </summary>
        /// <param name="newContainsBoxIndex">New index of the contains box.</param>
        public void OnChangeContainsIndex(int newContainsBoxIndex)
        {
            if (_containsBoxDataTable != null && newContainsBoxIndex >= 0 && newContainsBoxIndex < _containsBoxDataTable.Rows.Count)
            {
                var dataRow = _containsBoxDataTable.Rows[newContainsBoxIndex];
                AutoFillControl.EditText = dataRow.GetRowValue(AutoFillDefinition.SelectSqlAlias);
                PrimaryKeyValue = new PrimaryKeyValue(AutoFillDefinition.TableDefinition);
                PrimaryKeyValue.PopulateFromDataRow(dataRow);
                AutoFillControl.SelectionStart = AutoFillControl.EditText.Length;
                AutoFillControl.SelectionLength = 0;
                OnOutput(false);
            }
        }

        /// <summary>
        /// Sets the primary key value and text.
        /// </summary>
        /// <param name="primaryKeyValue">The primary key value.</param>
        /// <param name="text">The text.</param>
        /// <param name="refreshContainsList">if set to true then refresh the contains listbox.</param>
        public void SetValue(PrimaryKeyValue primaryKeyValue, string text, bool refreshContainsList)
        {
            if (refreshContainsList)
            {
                GetContainsBoxDataTable(text);
            }
            ClearPrimaryKeyValue();
            PrimaryKeyValue.CopyFromPrimaryKeyValue(primaryKeyValue);
            AutoFillControl.EditText = text.IsNullOrEmpty()?string.Empty:text;
            AutoFillControl.SelectionStart = 0;
            AutoFillControl.SelectionLength = AutoFillControl.EditText.Length;
            OnOutput(refreshContainsList);
        }

        /// <summary>
        /// Clears the value.
        /// </summary>
        public void ClearValue()
        {
            ClearPrimaryKeyValue();
            AutoFillControl.EditText = string.Empty;
            AutoFillControl.SelectionStart = AutoFillControl.SelectionLength = 0;
            _containsBoxDataTable = null;
            OnOutput();
        }

        /// <summary>
        /// Gets the AutoFill contains item to put into the contains list box.  Value is in pieces so the contains text is bolded.
        /// </summary>
        /// <param name="containsDataRow">The contains data row.</param>
        /// <returns>An Auto Fill Contains object that splits the text into prefix, contains(bold) and suffix.</returns>
        public AutoFillContainsItem GetAutoFillContainsItem(DataRow containsDataRow)
        {
            var text = containsDataRow.GetRowValue(AutoFillDefinition.SelectSqlAlias);
            var firstIndex = text.IndexOf(_containsText, StringComparison.OrdinalIgnoreCase);
            var prefix = text.LeftStr(firstIndex);
            var containsText = text.MidStr(firstIndex, _containsText.Length);
            var suffix = text.RightStr(text.Length - (firstIndex + _containsText.Length));
            return new AutoFillContainsItem
            {
                PrefixText = prefix,
                ContainsText = containsText,
                SuffixText = suffix
            };
        }

        private void ClearPrimaryKeyValue()
        {
            PrimaryKeyValue = new PrimaryKeyValue(AutoFillDefinition.TableDefinition);
        }

        /// <summary>
        /// Refreshes the data.
        /// </summary>
        /// <param name="refreshContainsList">If set to true refresh the contains list.</param>
        public void RefreshData(bool refreshContainsList)
        {
            if (!AutoFillControl.EditText.IsNullOrEmpty())
            {
                ClearPrimaryKeyValue();
                GetContainsBoxDataTable(AutoFillControl.EditText);
                OnOutput(refreshContainsList);
            }
        }
    }
}
