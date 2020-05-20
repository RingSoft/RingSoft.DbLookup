using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Linq.Expressions;
using RingSoft.DbLookup.ModelDefinition;
using RingSoft.DbLookup.ModelDefinition.FieldDefinitions;

namespace RingSoft.DbLookup.Lookup
{
    /// <summary>
    /// A lookup definition that has no generic entity but a generic lookup entity.
    /// </summary>
    /// <typeparam name="TLookupEntity">The type of the lookup entity.</typeparam>
    /// <seealso cref="LookupDefinitionBase" />
    public class LookupEntityDefinition<TLookupEntity> : LookupDefinitionBase where TLookupEntity : new()
    {
        internal LookupEntityDefinition(TableDefinitionBase tableDefinition) : base(tableDefinition)
        {
        }

        /// <summary>
        /// Clones this instance.
        /// </summary>
        /// <returns>
        /// An object that derives from this LookupEntityDefinition class.
        /// </returns>
        protected internal override LookupDefinitionBase BaseClone()
        {
            var clone = new LookupEntityDefinition<TLookupEntity>(TableDefinition);
            clone.CopyLookupData(this);
            return clone;
        }


        /// <summary>
        /// Adds a visible column definition.
        /// </summary>
        /// <param name="lookupEntityProperty">The lookup entity property.</param>
        /// <param name="caption">The caption.</param>
        /// <param name="fieldDefinition">The field definition.</param>
        /// <param name="percentWidth">The percent of the lookup's total width.</param>
        /// <returns></returns>
        internal LookupFieldColumnDefinition AddVisibleColumnDefinition(
            Expression<Func<TLookupEntity, object>> lookupEntityProperty, string caption,
            FieldDefinition fieldDefinition,
            double percentWidth)
        {
            var columnName = caption;
            if (columnName.IsNullOrEmpty())
                columnName = lookupEntityProperty.GetFullPropertyName();

            ValidateProperty(lookupEntityProperty, false, columnName);
            var column = base.AddVisibleColumnDefinition(caption, fieldDefinition, percentWidth);
            column.PropertyName = lookupEntityProperty.GetFullPropertyName();
            return column;
        }

        internal FieldDataTypes GetFieldDataTypeForProperty(Expression<Func<TLookupEntity, object>> lookupEntityProperty)
        {
            var propertyType = GetTypeFromExpression(lookupEntityProperty);
            return GblMethods.GetFieldDataTypeForType(propertyType);
        }

        /// <summary>
        /// Gets the column definition.
        /// </summary>
        /// <param name="lookupEntityProperty">The lookup entity property.</param>
        /// <returns></returns>
        public LookupColumnDefinitionBase GetColumnDefinition(Expression<Func<TLookupEntity, object>> lookupEntityProperty)
        {
            var propertyName = lookupEntityProperty.GetFullPropertyName();
            var column = VisibleColumns.FirstOrDefault(c => c.PropertyName == propertyName);
            if (column == null)
                column = HiddenColumns.FirstOrDefault(c => c.PropertyName == propertyName);
            return column;
        }

        /// <summary>
        /// Adds a hidden column.
        /// </summary>
        /// <param name="lookupEntityProperty">The lookup entity property.</param>
        /// <param name="fieldDefinition">The field definition.</param>
        /// <returns></returns>
        public LookupFieldColumnDefinition AddHiddenColumn(Expression<Func<TLookupEntity, object>> lookupEntityProperty, FieldDefinition fieldDefinition)
        {
            ValidateProperty(lookupEntityProperty, true, string.Empty);

            var column = base.AddHiddenColumn(fieldDefinition);
            column.PropertyName = lookupEntityProperty.GetFullPropertyName();
            return column;
        }

        public LookupFormulaColumnDefinition AddHiddenColumn(Expression<Func<TLookupEntity, object>> lookupEntityProperty, string formula)
        {
            ValidateProperty(lookupEntityProperty, true, lookupEntityProperty.GetFullPropertyName());
            var column = base.AddHiddenColumn(formula, GetFieldDataTypeForProperty(lookupEntityProperty));
            column.PropertyName = lookupEntityProperty.GetFullPropertyName();
            return column;
        }

        internal void ValidateProperty(Expression<Func<TLookupEntity, object>> lookupEntityProperty, bool hiddenProperty, string columnName)
        {
            var propertyType = GetTypeFromExpression(lookupEntityProperty);
            if (propertyType == typeof(string)
                || propertyType == typeof(DateTime)
                || propertyType == typeof(decimal)
                || propertyType == typeof(double)
                || propertyType == typeof(float)
                || propertyType == typeof(int)
                || propertyType == typeof(long)
                || propertyType == typeof(byte)
                || propertyType == typeof(short)
            )
            {
                //OK
            }
            else if (propertyType == typeof(bool))
            {
                if (!hiddenProperty)
                    throw new ArgumentException($"Visible bool column '{columnName}' will get converted to a string.  You must map this visible bool column to a string property.");
            }
            else if (propertyType.BaseType == typeof(Enum))
            {
                if (!hiddenProperty)
                    throw new ArgumentException($"Visible enumerator column '{columnName}' will get converted to a string.  You must map this visible enumerator column to a string property.");
            }
            else
            {
                throw new ArgumentException($"Property '{lookupEntityProperty.GetFullPropertyName()}' of type '{propertyType.Name}' is not supported.");
            }
        }

        private Type GetTypeFromExpression(Expression<Func<TLookupEntity, object>> expr)
        {
            if ((expr.Body.NodeType == ExpressionType.Convert) ||
                (expr.Body.NodeType == ExpressionType.ConvertChecked))
            {
                var unary = expr.Body as UnaryExpression;
                if (unary != null)
                    return unary.Operand.Type;
            }

            return expr.Body.Type;
        }

        /// <summary>
        /// Gets the lookup results list from lookup data.
        /// </summary>
        /// <param name="lookupData">The lookup data.</param>
        /// <returns></returns>
        public List<TLookupEntity> GetLookupResultsListFromLookupData(LookupDataBase lookupData)
        {
            var lookupResults = new List<TLookupEntity>();

            if (lookupData.LookupResultsDataTable != null)
            {
                foreach (DataRow dataRow in lookupData.LookupResultsDataTable.Rows)
                {
                    lookupResults.Add(GetEntityFromDataRow(dataRow));
                }
            }

            return lookupResults;
        }

        /// <summary>
        /// Gets the selected item from lookup data.
        /// </summary>
        /// <param name="lookupData">The lookup data.</param>
        /// <returns></returns>
        /// <exception cref="ArgumentException">The passed in lookup definition's entity does not match this lookup definition entity</exception>
        public TLookupEntity GetSelectedItemFromLookupData(LookupDataBase lookupData)
        {
            if (lookupData.LookupDefinition.LookupEntityName != LookupEntityName)
                throw new ArgumentException("The passed in lookup definition's entity does not match this lookup definition entity");

            if (lookupData.LookupResultsDataTable != null && lookupData.SelectedRowIndex >= 0 &&
                lookupData.SelectedRowIndex < lookupData.LookupResultsDataTable.Rows.Count)
            {
                return GetEntityFromDataRow(lookupData.LookupResultsDataTable.Rows[lookupData.SelectedRowIndex]);
            }

            return new TLookupEntity();
        }

        private TLookupEntity GetEntityFromDataRow(DataRow dataRow)
        {
            var entity = (TLookupEntity)Activator.CreateInstance(typeof(TLookupEntity));

            foreach (var lookupDefinitionVisibleColumn in VisibleColumns)
            {
                ProcessColumn(entity, dataRow, lookupDefinitionVisibleColumn);
            }

            foreach (var lookupDefinitionHiddenColumn in HiddenColumns)
            {
                ProcessColumn(entity, dataRow, lookupDefinitionHiddenColumn);
            }

            return entity;
        }

        private void ProcessColumn(TLookupEntity listItem, DataRow dataRow, LookupColumnDefinitionBase column)
        {
            if (column.PropertyName.IsNullOrEmpty())
                return;

            var value = dataRow.GetRowValue(column.SelectSqlAlias);
            GblMethods.SetPropertyValue(listItem, column.PropertyName, value);
        }
    }
}
