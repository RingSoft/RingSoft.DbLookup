using System;
using System.Collections.Generic;
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
    public class AutoFillOutputData
    {
        public List<string> ContainsData { get; }

        public AutoFillValue AutoFillValue { get; }


        public AutoFillOutputData(List<string> containsData, AutoFillValue autoFillValue)
        {
            ContainsData = containsData;
            AutoFillValue = autoFillValue;
        }
    }
    public abstract class AutoFillDataMauiBase
    {
        public AutoFillSetup Setup { get; }

        public IAutoFillControl Control { get; }

        public event EventHandler<AutoFillOutputData> OutputDataChanged;

        public AutoFillDataMauiBase(AutoFillSetup setup, IAutoFillControl control)
        {
            Setup = setup;
            Control = control;
        }

        protected internal void OnOutputAutoFillData(AutoFillOutputData outputData)
        {
            OutputDataChanged?.Invoke(this, outputData);
        }

        public abstract void OnKeyCharPressed(char keyChar);

        public abstract void OnBackspaceKeyDown();

        public abstract void OnDeleteKeyDown();

        public abstract void OnLookupSelect(PrimaryKeyValue primaryKey);

        public abstract void SetValue(PrimaryKeyValue primaryKeyValue, string text, bool refreshContainsList);
    }

    public class AutoFillDataMaui<TEntity> : AutoFillDataMauiBase where TEntity : class, new()
    {
        public TableDefinition<TEntity> TableDefinition { get; }

        public AutoFillDataMaui(AutoFillSetup setup, IAutoFillControl control) : base(setup, control)
        {
            TableDefinition = GblMethods.GetTableDefinition<TEntity>();
        }

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

            OnOutput(beginText);

        }

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

        public override void OnLookupSelect(PrimaryKeyValue primaryKey)
        {
            var entity = TableDefinition.GetEntityFromPrimaryKeyValue(primaryKey);
            if (entity != null)
            {
                if (Setup.LookupDefinition.InitialSortColumnDefinition is LookupFieldColumnDefinition fieldColumn)
                {
                    var primaryKeyField = TableDefinition.PrimaryKeyFields.FirstOrDefault();
                    var property = primaryKeyField.PropertyName;
                    var param = GblMethods.GetParameterExpression<TEntity>();
                    if (param != null)
                    {
                        var query = SystemGlobals.DataRepository.GetDataContext().GetTable<TEntity>();

                        var value = GblMethods.GetPropertyValue(entity, property);
                        var filterValue =
                            value.GetPropertyFilterValue(primaryKeyField.FieldDataType, primaryKeyField.FieldType);

                        var filter = FilterItemDefinition.GetBinaryExpression<TEntity>(param, property,
                            Conditions.Equals, primaryKeyField.FieldType, filterValue);

                        var filterQuery = FilterItemDefinition.FilterQuery(query, param, filter);
                        entity = filterQuery.FirstOrDefault();

                        if (entity != null)
                        {
                            OnOutputAutoFillData(new AutoFillOutputData(null, entity.GetAutoFillValue()));
                        }
                    }
                }
            }
        }

        private string GetRightText(string text, int selectionStart, int selectionLength)
        {
            if (text.IsNullOrEmpty())
                return string.Empty;

            return text.RightStr(text.Length - (selectionLength + selectionStart));
        }

        public string GetNewText(string beginText)
        {
            var result = string.Empty;

            var query = TableDefinition.Context.GetQueryable<TEntity>(Setup.LookupDefinition);
            var param = GblMethods.GetParameterExpression<TEntity>();
            var baseExpr = Setup.LookupDefinition.FilterDefinition.GetWhereExpresssion<TEntity>(param);

            var property = Setup.LookupDefinition.InitialSortColumnDefinition.GetPropertyJoinName();

            var autoFillExpr =
                FilterItemDefinition.GetBinaryExpression<TEntity>(param, property, Conditions.BeginsWith, typeof(string), beginText);

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

            var first = resultQuery.FirstOrDefault();
            if (first == null)
            {
                result = beginText;
            }
            else
            {
                result = Setup.LookupDefinition.InitialSortColumnDefinition.GetDatabaseValue(first);

                var outputData = new AutoFillOutputData(null, first.GetAutoFillValue()); ;
                OnOutputAutoFillData(outputData);
            }
            return result;
        }
        private void OnOutput(string newText)
        {
            var autoFillValue = GetAutoFillData();
            if (autoFillValue == null)
            {
                var primaryKey = new PrimaryKeyValue(Setup.LookupDefinition.TableDefinition);
                autoFillValue = new AutoFillValue(primaryKey, Control.EditText);
            }

            var outputData = new AutoFillOutputData(null
                , autoFillValue);
            OnOutputAutoFillData(outputData);
        }

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
                query = FilterItemDefinition.FilterQuery(query
                    , param
                    , autoFillExpr);
                var autoFillValue = query.FirstOrDefault().GetAutoFillValue();
                return autoFillValue;
            }
            return null;
        }

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

    }
}
