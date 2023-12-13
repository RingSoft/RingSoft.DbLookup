// ***********************************************************************
// Assembly         : RingSoft.DbLookup
// Author           : petem
// Created          : 06-08-2023
//
// Last Modified By : petem
// Last Modified On : 12-04-2023
// ***********************************************************************
// <copyright file="AutoFillDataMaui.cs" company="Peter Ringering">
//     Copyright (c) . All rights reserved.
// </copyright>
// <summary></summary>
// ***********************************************************************
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using RingSoft.DataEntryControls.Engine;
using RingSoft.DbLookup.Lookup;
using RingSoft.DbLookup.ModelDefinition;
using RingSoft.DbLookup.QueryBuilder;
using RingSoft.DbLookup.TableProcessing;

namespace RingSoft.DbLookup.AutoFill
{
    /// <summary>
    /// Class AutoFillOutputData.
    /// </summary>
    public class AutoFillOutputData
    {
        /// <summary>
        /// Gets the contains data.
        /// </summary>
        /// <value>The contains data.</value>
        public List<string> ContainsData { get; }

        /// <summary>
        /// Gets the automatic fill value.
        /// </summary>
        /// <value>The automatic fill value.</value>
        public AutoFillValue AutoFillValue { get; }

        /// <summary>
        /// Gets the begin text.
        /// </summary>
        /// <value>The begin text.</value>
        public string BeginText { get; }

        /// <summary>
        /// Initializes a new instance of the <see cref="AutoFillOutputData"/> class.
        /// </summary>
        /// <param name="containsData">The contains data.</param>
        /// <param name="autoFillValue">The automatic fill value.</param>
        /// <param name="beginText">The begin text.</param>
        public AutoFillOutputData(List<string> containsData, AutoFillValue autoFillValue, string beginText = "")
        {
            ContainsData = containsData;
            AutoFillValue = autoFillValue;
            BeginText = beginText;
        }
    }
    /// <summary>
    /// Class AutoFillDataMauiBase.
    /// </summary>
    public abstract class AutoFillDataMauiBase
    {
        /// <summary>
        /// Gets the setup.
        /// </summary>
        /// <value>The setup.</value>
        public AutoFillSetup Setup { get; }

        /// <summary>
        /// Gets the control.
        /// </summary>
        /// <value>The control.</value>
        public IAutoFillControl Control { get; }

        /// <summary>
        /// Occurs when [output data changed].
        /// </summary>
        public event EventHandler<AutoFillOutputData> OutputDataChanged;

        /// <summary>
        /// Initializes a new instance of the <see cref="AutoFillDataMauiBase"/> class.
        /// </summary>
        /// <param name="setup">The setup.</param>
        /// <param name="control">The control.</param>
        public AutoFillDataMauiBase(AutoFillSetup setup, IAutoFillControl control)
        {
            SystemGlobals.LookupContext.Initialize();
            Setup = setup;
            Control = control;
        }

        /// <summary>
        /// Called when [output automatic fill data].
        /// </summary>
        /// <param name="outputData">The output data.</param>
        protected internal void OnOutputAutoFillData(AutoFillOutputData outputData)
        {
            OutputDataChanged?.Invoke(this, outputData);
        }

        /// <summary>
        /// Called when [key character pressed].
        /// </summary>
        /// <param name="keyChar">The key character.</param>
        public abstract void OnKeyCharPressed(char keyChar);

        /// <summary>
        /// Called when [backspace key down].
        /// </summary>
        public abstract void OnBackspaceKeyDown();

        /// <summary>
        /// Called when [delete key down].
        /// </summary>
        public abstract void OnDeleteKeyDown();

        /// <summary>
        /// Called when [lookup select].
        /// </summary>
        /// <param name="primaryKey">The primary key.</param>
        public abstract void OnLookupSelect(PrimaryKeyValue primaryKey);

        /// <summary>
        /// Sets the value.
        /// </summary>
        /// <param name="primaryKeyValue">The primary key value.</param>
        /// <param name="text">The text.</param>
        /// <param name="refreshContainsList">if set to <c>true</c> [refresh contains list].</param>
        public abstract void SetValue(PrimaryKeyValue primaryKeyValue, string text, bool refreshContainsList);

        /// <summary>
        /// Gets the automatic fill contains item.
        /// </summary>
        /// <param name="text">The text.</param>
        /// <param name="beginText">The begin text.</param>
        /// <returns>AutoFillContainsItem.</returns>
        public abstract AutoFillContainsItem GetAutoFillContainsItem(string text, string beginText);

        /// <summary>
        /// Called when [ListBox change].
        /// </summary>
        /// <param name="item">The item.</param>
        /// <returns>AutoFillValue.</returns>
        public abstract AutoFillValue OnListBoxChange(AutoFillContainsItem item);

        /// <summary>
        /// Called when [paste].
        /// </summary>
        /// <param name="text">The text.</param>
        /// <returns>AutoFillValue.</returns>
        public abstract AutoFillValue OnPaste(string text);
    }

    /// <summary>
    /// Class AutoFillDataMaui.
    /// Implements the <see cref="RingSoft.DbLookup.AutoFill.AutoFillDataMauiBase" />
    /// </summary>
    /// <typeparam name="TEntity">The type of the t entity.</typeparam>
    /// <seealso cref="RingSoft.DbLookup.AutoFill.AutoFillDataMauiBase" />
    public class AutoFillDataMaui<TEntity> : AutoFillDataMauiBase where TEntity : class, new()
    {
        /// <summary>
        /// Gets the table definition.
        /// </summary>
        /// <value>The table definition.</value>
        public TableDefinition<TEntity> TableDefinition { get; }

        /// <summary>
        /// Initializes a new instance of the <see cref="AutoFillDataMaui{TEntity}"/> class.
        /// </summary>
        /// <param name="setup">The setup.</param>
        /// <param name="control">The control.</param>
        public AutoFillDataMaui(AutoFillSetup setup, IAutoFillControl control) : base(setup, control)
        {
            TableDefinition = GblMethods.GetTableDefinition<TEntity>();
        }

        /// <summary>
        /// Called when [key character pressed].
        /// </summary>
        /// <param name="keyChar">The key character.</param>
        public override void OnKeyCharPressed(char keyChar)
        {
            switch (keyChar)
            {
                case '\b':
                    OnBackspaceKeyDown();
                    return;
                case '\u001b':  //Escape
                case '\t':
                case '\r':
                case '\n':
                    return;
            }

            var text = Control.EditText;
            var selectionStart = Control.SelectionStart;
            var selectionLength = Control.SelectionLength;

            var rightText = GetRightText(text, selectionStart, selectionLength);
            var beginText = text.LeftStr(selectionStart) + keyChar + rightText;
            var newText = GetNewText(beginText);

            if (newText.IsNullOrEmpty())
            {
                //GetContainsBoxDataTable(beginText);
                Control.EditText = beginText;
                Control.SelectionStart = selectionStart + 1;
                Control.SelectionLength = 0;
            }
            else
            {
                Control.EditText = newText;
                Control.SelectionStart = selectionStart + 1;
                Control.SelectionLength = newText.Length - Control.SelectionStart;
            }

            OnOutput(newText, null, beginText);

        }

        /// <summary>
        /// Called when [backspace key down].
        /// </summary>
        public override void OnBackspaceKeyDown()
        {
            var text = Control.EditText;
            var selectionStart = Control.SelectionStart;
            var selectionLength = Control.SelectionLength;

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

            //GetContainsBoxDataTable(newText);
            Control.EditText = newText;
            Control.SelectionStart = newSelectStart;
            Control.SelectionLength = 0;
            //ClearPrimaryKeyValue();

            OnOutput(newText);

        }

        /// <summary>
        /// Called when [delete key down].
        /// </summary>
        public override void OnDeleteKeyDown()
        {
            var text = Control.EditText;
            if (text.IsNullOrEmpty() || Control.SelectionStart > text.Length - 1)
            {
                return;
            }

            var newText = string.Empty;
            var setText = true;
            if (Control.SelectionLength == text.Length)
            {
                setText = false;
            }
            var selectionStart = Control.SelectionStart;
            var selectionLength = Control.SelectionLength;

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

            //GetContainsBoxDataTable(newText);
            Control.EditText = newText;
            Control.SelectionStart = selectionStart;
            Control.SelectionLength = 0;

            OnOutput(newText);

        }

        /// <summary>
        /// Called when [lookup select].
        /// </summary>
        /// <param name="primaryKey">The primary key.</param>
        public override void OnLookupSelect(PrimaryKeyValue primaryKey)
        {
            var entity = TableDefinition.GetEntityFromPrimaryKeyValue(primaryKey);
            if (entity != null)
            {
                if (Setup.LookupDefinition.InitialSortColumnDefinition is LookupFieldColumnDefinition fieldColumn)
                {
                    //var primaryKeyField = TableDefinition.PrimaryKeyFields.FirstOrDefault();
                    var filter = new TableFilterDefinition<TEntity>(TableDefinition);
                    foreach (var keyValueField in primaryKey.KeyValueFields)
                    {
                        filter.AddFixedFieldFilter(keyValueField.FieldDefinition, Conditions.Equals,
                            keyValueField.Value);
                    }
                    var param = GblMethods.GetParameterExpression<TEntity>();
                    if (param != null)
                    {
                        //var query = SystemGlobals.DataRepository.GetDataContext().GetTable<TEntity>();
                        var query = Setup
                            .LookupDefinition
                            .TableDefinition
                            .Context
                            .GetQueryable<TEntity>(Setup.LookupDefinition);
                        var expr = filter.GetWhereExpresssion<TEntity>(param);
                        var filterQuery = FilterItemDefinition.FilterQuery(query, param, expr);
                        entity = filterQuery.FirstOrDefault();

                        if (entity != null)
                        {
                            var autoFill = entity.GetAutoFillValue(Setup.LookupDefinition);
                            OnOutputAutoFillData(
                                new AutoFillOutputData(
                                    null
                                    , autoFill));
                        }
                    }
                }
            }
        }

        /// <summary>
        /// Gets the right text.
        /// </summary>
        /// <param name="text">The text.</param>
        /// <param name="selectionStart">The selection start.</param>
        /// <param name="selectionLength">Length of the selection.</param>
        /// <returns>System.String.</returns>
        private string GetRightText(string text, int selectionStart, int selectionLength)
        {
            if (text.IsNullOrEmpty())
                return string.Empty;

            return text.RightStr(text.Length - (selectionLength + selectionStart));
        }

        /// <summary>
        /// Gets the new text.
        /// </summary>
        /// <param name="beginText">The begin text.</param>
        /// <returns>System.String.</returns>
        public string GetNewText(string beginText)
        {
            ControlsGlobals.UserInterface.SetWindowCursor(WindowCursorTypes.Wait);
            var result = string.Empty;

            var query = TableDefinition.Context.GetQueryable<TEntity>(Setup.LookupDefinition);
            var param = GblMethods.GetParameterExpression<TEntity>();
            var baseExpr = Setup.LookupDefinition.FilterDefinition.GetWhereExpresssion<TEntity>(param);

            var property = Setup.LookupDefinition.InitialSortColumnDefinition.GetPropertyJoinName();

            var autoFillExpr =
                FilterItemDefinition.GetBinaryExpression<TEntity>
                    (param, property, Conditions.BeginsWith, typeof(string), beginText);

            Expression newFilter = null;
            if (baseExpr == null)
            {
                newFilter = autoFillExpr;
            }
            else
            {
                newFilter = FilterItemDefinition.AppendExpression(baseExpr, autoFillExpr, EndLogics.And);
            }

            var resultQuery = FilterItemDefinition.FilterQuery(query, param, newFilter);

            resultQuery = GblMethods.ApplyOrder(resultQuery, OrderMethods.OrderBy, property);

            AutoFillValue newValue = null;
            TEntity first = null;
            try
            {
                first = resultQuery.FirstOrDefault();
            }
            catch (Exception e)
            {
                ControlsGlobals.UserInterface.SetWindowCursor(WindowCursorTypes.Default);
                Console.WriteLine(e);
                ControlsGlobals.UserInterface.ShowMessageBox(e.Message, "Error!", RsMessageBoxIcons.Error);
                return string.Empty;
            }
            
            if (first == null)
            {
                //result = beginText;
            }
            else
            {
                result = Setup.LookupDefinition.InitialSortColumnDefinition.GetDatabaseValue(first);

                newValue = first.GetAutoFillValue();
            }
            if (newValue == null)
            {
                newValue = GetAutoFillValue(beginText);
            }

            //var outputData = new AutoFillOutputData(null, newValue); ;
            //OnOutputAutoFillData(outputData);
            OnOutput(result, newValue, beginText);
            ControlsGlobals.UserInterface.SetWindowCursor(WindowCursorTypes.Default);
            return result;
        }

        /// <summary>
        /// Gets the contains list.
        /// </summary>
        /// <param name="beginText">The begin text.</param>
        /// <returns>List&lt;System.String&gt;.</returns>
        private List<string> GetContainsList(string beginText)
        {
            var param = GblMethods.GetParameterExpression<TEntity>();
            var result = new List<string>();
            if (Setup.LookupDefinition.InitialSortColumnDefinition is LookupFieldColumnDefinition fieldColumn)
            {
                var containsProperty = Setup.LookupDefinition.InitialSortColumnDefinition.GetPropertyJoinName();

                var lookupExpr = Setup.LookupDefinition.FilterDefinition.GetWhereExpresssion<TEntity>(param);
                var containsExpr = FilterItemDefinition.GetBinaryExpression<TEntity>(
                    param
                    , containsProperty
                    , Conditions.Contains
                    , fieldColumn.FieldDefinition.FieldType
                    , beginText);
                containsExpr = FilterItemDefinition.AppendExpression(
                    lookupExpr
                    , containsExpr
                    , EndLogics.And);
                var containsQuery = Setup.LookupDefinition.TableDefinition.Context
                    .GetQueryable<TEntity>(Setup.LookupDefinition);
                containsQuery = FilterItemDefinition.FilterQuery(containsQuery, param, containsExpr);
                
                containsQuery = GblMethods.ApplyOrder(containsQuery, OrderMethods.OrderBy, containsProperty);
                containsQuery = containsQuery.Take(5);

                try
                {
                    foreach (var entity in containsQuery)
                    {
                        result.Add(fieldColumn.GetDatabaseValue(entity));
                    }
                }
                catch (Exception e)
                {
                    Console.WriteLine(e);
                    return result;
                }
            }
            return result;
        }

        /// <summary>
        /// Called when [output].
        /// </summary>
        /// <param name="newText">The new text.</param>
        /// <param name="autoFillValue">The automatic fill value.</param>
        /// <param name="beginText">The begin text.</param>
        private void OnOutput(string newText, AutoFillValue autoFillValue = null, string beginText = "")
        {
            List<string> containsList = null;
            var containsText = beginText;
            if (containsText.IsNullOrEmpty())
            {
                containsText = newText;
            }if (!containsText.IsNullOrEmpty())
            {
                containsList = GetContainsList(containsText);
            }

            if (autoFillValue == null)
            {
                autoFillValue = GetAutoFillValue();
            }

            var outputData = new AutoFillOutputData(containsList
                , autoFillValue, containsText);

            OnOutputAutoFillData(outputData);
        }

        /// <summary>
        /// Gets the automatic fill value.
        /// </summary>
        /// <param name="editText">The edit text.</param>
        /// <returns>AutoFillValue.</returns>
        private AutoFillValue GetAutoFillValue(string editText = "")
        {
            if (editText.IsNullOrEmpty())
            {
                editText = Control.EditText;
            }
            var autoFillValue = GetAutoFillData();
            if (autoFillValue == null)
            {
                if (!Control.EditText.IsNullOrEmpty())
                {
                    var primaryKey = new PrimaryKeyValue(Setup.LookupDefinition.TableDefinition);
                    autoFillValue = new AutoFillValue(primaryKey, editText);
                    autoFillValue.FromLookup = false;
                }
            }

            return autoFillValue;
        }

        /// <summary>
        /// Gets the automatic fill data.
        /// </summary>
        /// <returns>AutoFillValue.</returns>
        private AutoFillValue GetAutoFillData()
        {
            if (Setup.LookupDefinition.InitialSortColumnDefinition is LookupFieldColumnDefinition lookupFieldColumn)
            {
                var param = GblMethods.GetParameterExpression<TEntity>();
                var query = TableDefinition.Context.GetQueryable<TEntity>(Setup.LookupDefinition);
                var property = Setup.LookupDefinition.InitialSortColumnDefinition.GetPropertyJoinName();
                var autoFillExpr = FilterItemDefinition.GetBinaryExpression<TEntity>(param
                    , property
                    , Conditions.Equals
                    , lookupFieldColumn.FieldDefinition.FieldType
                    , Control.EditText);

                var filterExpr = Setup.LookupDefinition.FilterDefinition.GetWhereExpresssion<TEntity>(param);

                autoFillExpr = FilterItemDefinition.AppendExpression(autoFillExpr, filterExpr, EndLogics.And);

                query = FilterItemDefinition.FilterQuery(query
                    , param
                    , autoFillExpr);
                AutoFillValue autoFillValue = null;
                try
                {
                    autoFillValue = query.FirstOrDefault().GetAutoFillValue();
                }
                catch (Exception e)
                {
                    Console.WriteLine(e);
                    ControlsGlobals.UserInterface.SetWindowCursor(WindowCursorTypes.Default);
                }
                return autoFillValue;
            }
            return null;
        }

        /// <summary>
        /// Sets the value.
        /// </summary>
        /// <param name="primaryKeyValue">The primary key value.</param>
        /// <param name="text">The text.</param>
        /// <param name="refreshContainsList">if set to <c>true</c> [refresh contains list].</param>
        public override void SetValue(PrimaryKeyValue primaryKeyValue, string text, bool refreshContainsList)
        {
            if (refreshContainsList)
            {
                //GetContainsBoxDataTable(text);
            } 

            Control.EditText = text.IsNullOrEmpty() ? string.Empty : text;
            Control.SelectionStart = 0;
            Control.SelectionLength = Control.EditText.Length;
        }
        /// <summary>
        /// Gets the automatic fill contains item.
        /// </summary>
        /// <param name="text">The text.</param>
        /// <param name="beginText">The begin text.</param>
        /// <returns>AutoFillContainsItem.</returns>
        public override AutoFillContainsItem GetAutoFillContainsItem(string text, string beginText)
        {
            var firstIndex = text.IndexOf(beginText, StringComparison.OrdinalIgnoreCase);
            var prefix = text.LeftStr(firstIndex);
            var containsText = text.MidStr(firstIndex, beginText.Length);
            var suffix = text.RightStr(text.Length - (firstIndex + beginText.Length));
            return new AutoFillContainsItem
            {
                PrefixText = prefix,
                ContainsText = containsText,
                SuffixText = suffix
            };
        }

        /// <summary>
        /// Called when [ListBox change].
        /// </summary>
        /// <param name="item">The item.</param>
        /// <returns>AutoFillValue.</returns>
        public override AutoFillValue OnListBoxChange(AutoFillContainsItem item)
        {
            Control.EditText = item.ToString();
            Control.SelectionStart = Control.EditText.Length;
            Control.SelectionLength = 0;
            return GetAutoFillData();
        }

        /// <summary>
        /// Called when [paste].
        /// </summary>
        /// <param name="text">The text.</param>
        /// <returns>AutoFillValue.</returns>
        public override AutoFillValue OnPaste(string text)
        {
            Control.EditText = text;
            var result = GetAutoFillValue();
            Control.SelectionStart = Control.EditText.Length;
            Control.SelectionLength = 0;
            return result;
        }
    }
}
