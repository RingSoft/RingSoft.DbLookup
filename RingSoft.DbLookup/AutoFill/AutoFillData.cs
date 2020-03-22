﻿using System;
using System.Data;
using RingSoft.DbLookup.GetDataProcessor;
using RingSoft.DbLookup.Lookup;
using RingSoft.DbLookup.ModelDefinition.FieldDefinitions;
using RingSoft.DbLookup.QueryBuilder;

namespace RingSoft.DbLookup.AutoFill
{
    /// <summary>
    /// Argument sent in the AutoFillChanged event.
    /// </summary>
    public class AutoFillDataChangedArgs
    {
        /// <summary>
        /// Gets a value indicating whether to refresh the contains list.
        /// </summary>
        /// <value>
        ///   <c>true</c> if refresh contains list; otherwise, <c>false</c>.
        /// </value>
        public bool RefreshContainsList { get; }

        public AutoFillDataChangedArgs(bool refreshContainsList)
        {
            RefreshContainsList = refreshContainsList;
        }
    }
    /// <summary>
    /// Contains all the data to output during data entry in an AutoFill control.
    /// </summary>
    public class AutoFillData
    {
        /// <summary>
        /// Gets the AutoFill definition.
        /// </summary>
        /// <value>
        /// The AutoFill definition.
        /// </value>
        public AutoFillBase AutoFillDefinition { get; private set; }

        /// <summary>
        /// Gets the search for contains text.
        /// </summary>
        /// <value>
        /// The contains text.
        /// </value>
        public string ContainsText { get; private set; }

        /// <summary>
        /// Gets the text result.
        /// </summary>
        /// <value>
        /// The text result.
        /// </value>
        public string TextResult { get; private set; }

        /// <summary>
        /// Gets the start index of the cursor.
        /// </summary>
        /// <value>
        /// The start index of the cursor.
        /// </value>
        public int CursorStartIndex { get; private set; }

        /// <summary>
        /// Gets the length of the text select.
        /// </summary>
        /// <value>
        /// The length of the text select.
        /// </value>
        public int TextSelectLength { get; private set; }

        /// <summary>
        /// Gets the primary key value.
        /// </summary>
        /// <value>
        /// The primary key value.
        /// </value>
        public PrimaryKeyValue PrimaryKeyValue { get; private set; }

        /// <summary>
        /// Gets the contains box data table.
        /// </summary>
        /// <value>
        /// The contains box data table.
        /// </value>
        public DataTable ContainsBoxDataTable { get; private set; }

        /// <summary>
        /// Gets or sets the contains box maximum rows.
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
        /// Occurs when AutoFill data is changed.
        /// </summary>
        public event EventHandler<AutoFillDataChangedArgs> AutoFillDataChanged;

        private const string AutoFillTableName = "AutoFill";
        private const string ContainsTableName = "Contains";

        /// <summary>
        /// Initializes a new instance of the <see cref="AutoFillData" /> class.
        /// </summary>
        /// <param name="lookupDefinition">The lookup definition.</param>
        /// <param name="isDistinct">if set to <c>true</c> is distinct.</param>
        /// <exception cref="ArgumentOutOfRangeException"></exception>
        public AutoFillData(LookupDefinitionBase lookupDefinition, bool isDistinct)
        {
            AutoFillBase autoFillDefinition = null;

            switch (lookupDefinition.InitialSortColumnDefinition.ColumnType)
            {
                case LookupColumnTypes.Field:
                    if (lookupDefinition.InitialSortColumnDefinition is LookupFieldColumnDefinition lookupFieldColumn)
                    {
                        var stringField = lookupFieldColumn.FieldDefinition as StringFieldDefinition;
                        var autoFillFieldDefinition = new AutoFillFieldDefinition(stringField);
                        autoFillFieldDefinition.IsDistinct(lookupFieldColumn.Distinct || isDistinct);
                        autoFillDefinition = autoFillFieldDefinition;
                    }
                    break;
                case LookupColumnTypes.Formula:
                    if (lookupDefinition.InitialSortColumnDefinition is LookupFormulaColumnDefinition lookupFormulaColumn)
                    {
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

            Initialize(autoFillDefinition);
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="AutoFillData"/> class.
        /// </summary>
        /// <param name="autoFillDefinition">The AutoFill definition.</param>
        /// <exception cref="ArgumentException">AutoFill's Field definition cannot be a memo field.</exception>
        /// <exception cref="ArgumentOutOfRangeException"></exception>
        public AutoFillData(AutoFillBase autoFillDefinition)
        {
            Initialize(autoFillDefinition);
        }

        private void Initialize(AutoFillBase autoFillDefinition)
        {
            switch (autoFillDefinition.Type)
            {
                case AutoFillTypes.Field:
                    if (autoFillDefinition is AutoFillFieldDefinition autoFillFieldDefinition)
                    {
                        if (autoFillFieldDefinition.StringFieldDefinition.MemoField)
                            throw new ArgumentException("AutoFill's Field definition cannot be a memo field.");
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
        /// <param name="keyChar">The key character.</param>
        /// <param name="text">The text.</param>
        /// <param name="selectionStart">The selection start.</param>
        /// <param name="selectionLength">Length of the selection.</param>
        /// <returns></returns>
        public bool OnKeyCharPressed(char keyChar, string text, int selectionStart, int selectionLength)
        {
            switch (keyChar)
            {
                case '\b':
                    OnBackspaceKeyDown(text, selectionStart, selectionLength);
                    return true;
                case '\u001b':  //Escape
                case '\t':
                case '\r':
                case '\n':
                    return false;
            }
            var rightText = GetRightText(text, selectionStart, selectionLength);
            var beginText = text.LeftStr(selectionStart) + keyChar + rightText;
            var newText = GetNewText(beginText);

            if (newText.IsNullOrEmpty())
            {
                GetContainsBoxDataTable(beginText);
                TextResult = beginText;
                CursorStartIndex = selectionStart + 1;
                TextSelectLength = 0;
            }
            else
            {
                TextResult = newText;
                CursorStartIndex = selectionStart + 1;
                TextSelectLength = newText.Length - CursorStartIndex;
            }

            OnOutput();
            return true;
        }

        /// <summary>
        /// Called when the delete key is pressed.
        /// </summary>
        /// <param name="text">The text.</param>
        /// <param name="selectionStart">The selection start.</param>
        /// <param name="selectionLength">Length of the selection.</param>
        public void OnDeleteKeyDown(string text, int selectionStart, int selectionLength)
        {
            var leftText = text.LeftStr(selectionStart);
            var rightText = GetRightText(text, selectionStart, selectionLength);
            var newText = leftText + rightText;

            GetContainsBoxDataTable(newText);
            TextResult = newText;
            CursorStartIndex = selectionStart;
            TextSelectLength = 0;
            ClearPrimaryKeyValue();

            OnOutput();
        }

        /// <summary>
        /// Called when the backspace key is pressed.
        /// </summary>
        /// <param name="text">The text.</param>
        /// <param name="selectionStart">The selection start.</param>
        /// <param name="selectionLength">Length of the selection.</param>
        public void OnBackspaceKeyDown(string text, int selectionStart, int selectionLength)
        {
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
            TextResult = newText;
            CursorStartIndex = newSelectStart;
            TextSelectLength = 0;
            ClearPrimaryKeyValue();

            OnOutput();
        }

        private string GetRightText(string text, int selectionStart, int selectionLength)
        {
            return text.RightStr(text.Length - (selectionLength + selectionStart));
        }

        private void OnOutput(bool refreshContainsList = true)
        {
            AutoFillDataChanged?.Invoke(this, new AutoFillDataChangedArgs(refreshContainsList));
        }

        private string GetNewText(string beginText)
        {
            var newText = string.Empty;
            var autoFillQuery = GetBaseQuery().SetMaxRecords(1);

            switch (AutoFillDefinition.Type)
            {
                case AutoFillTypes.Field:
                    if (AutoFillDefinition is AutoFillFieldDefinition autoFillField)
                    {
                        autoFillQuery.AddWhereItem(autoFillField.StringFieldDefinition.FieldName, Conditions.BeginsWith, beginText);
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
                    ContainsText = beginText;
                    ContainsBoxDataTable = result.DataSet.Tables[ContainsTableName];
                }
            }

            return newText;
        }

        private SelectQuery GetBaseQuery()
        {
            var query = new SelectQuery(AutoFillDefinition.TableDefinition.TableName);
            AutoFillFieldDefinition autoFillField = null;
            switch (AutoFillDefinition.Type)
            {
                case AutoFillTypes.Field:
                    if (AutoFillDefinition is AutoFillFieldDefinition)
                    {
                        autoFillField = AutoFillDefinition as AutoFillFieldDefinition;
                        if (autoFillField != null)
                            query.AddSelectColumn(autoFillField.StringFieldDefinition.FieldName,
                                    autoFillField.SelectSqlAlias, autoFillField.Distinct)
                                .AddOrderBySegment(autoFillField.StringFieldDefinition.FieldName,
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
                query.AddSelectColumn(autoFillField.StringFieldDefinition.FieldName);
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
                        autoFillQuery.AddWhereItem(autoFillField.StringFieldDefinition.FieldName, Conditions.Equals, text);
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
                    ContainsText = text;
                    ContainsBoxDataTable = result.DataSet.Tables[ContainsTableName];
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
                        query.AddWhereItem(autoFillField.StringFieldDefinition.FieldName, Conditions.Contains, text);
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
            if (ContainsBoxDataTable != null && newContainsBoxIndex >= 0 && newContainsBoxIndex < ContainsBoxDataTable.Rows.Count)
            {
                var dataRow = ContainsBoxDataTable.Rows[newContainsBoxIndex];
                TextResult = dataRow.GetRowValue(AutoFillDefinition.SelectSqlAlias);
                PrimaryKeyValue = new PrimaryKeyValue(AutoFillDefinition.TableDefinition);
                PrimaryKeyValue.PopulateFromDataRow(dataRow);
                CursorStartIndex = TextResult.Length;
                TextSelectLength = 0;
                OnOutput(false);
            }
        }

        /// <summary>
        /// Sets the value.
        /// </summary>
        /// <param name="primaryKeyValue">The primary key value.</param>
        /// <param name="text">The text.</param>
        /// <param name="refreshContainsList">if set to <c>true</c> then refresh the contains listbox.</param>
        public void SetValue(PrimaryKeyValue primaryKeyValue, string text, bool refreshContainsList)
        {
            if (refreshContainsList)
            {
                GetContainsBoxDataTable(text);
            }
            ClearPrimaryKeyValue();
            PrimaryKeyValue.CopyFromPrimaryKeyValue(primaryKeyValue);
            TextResult = text.IsNullOrEmpty()?string.Empty:text;
            CursorStartIndex = 0;
            TextSelectLength = TextResult.Length;
            OnOutput(refreshContainsList);
        }

        /// <summary>
        /// Clears the value.
        /// </summary>
        public void ClearValue()
        {
            ClearPrimaryKeyValue();
            TextResult = string.Empty;
            CursorStartIndex = TextSelectLength = 0;
            ContainsBoxDataTable = null;
            OnOutput();
        }

        /// <summary>
        /// Gets the AutoFill contains item to put into the contains list box.  Value is in pieces so the contains text is bolded.
        /// </summary>
        /// <param name="containsDataRow">The contains data row.</param>
        /// <returns></returns>
        public AutoFillContainsItem GetAutoFillContainsItem(DataRow containsDataRow)
        {
            var text = containsDataRow.GetRowValue(AutoFillDefinition.SelectSqlAlias);
            var firstIndex = text.IndexOf(ContainsText, StringComparison.OrdinalIgnoreCase);
            var prefix = text.LeftStr(firstIndex);
            var containsText = text.MidStr(firstIndex, ContainsText.Length);
            var suffix = text.RightStr(text.Length - (firstIndex + ContainsText.Length));
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
        /// <param name="refreshContainsList">if set to <c>true</c> refresh the contains list.</param>
        public void RefreshData(bool refreshContainsList)
        {
            if (!TextResult.IsNullOrEmpty())
            {
                ClearPrimaryKeyValue();
                GetContainsBoxDataTable(TextResult);
                OnOutput(refreshContainsList);
            }
        }
    }
}
