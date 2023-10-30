using RingSoft.DbLookup.ModelDefinition;
using RingSoft.DbLookup.ModelDefinition.FieldDefinitions;
using RingSoft.DbLookup.TableProcessing;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using RingSoft.DataEntryControls.Engine;
using RingSoft.DbLookup.AutoFill;
using RingSoft.DbLookup.DataProcessor;
using RingSoft.DbLookup.QueryBuilder;
using System.Text.RegularExpressions;

namespace RingSoft.DbLookup.Lookup
{
    /// <summary>
    /// Contains all the data necessary to show a lookup.
    /// </summary>
    /// <typeparam name="TLookupEntity">The type of the lookup entity.</typeparam>
    /// <typeparam name="TEntity">The type of entity used by the Entity Framework platform.</typeparam>
    /// <seealso cref="LookupDefinitionBase" />
    public class LookupDefinition<TLookupEntity, TEntity> : LookupDefinitionBase
        where TLookupEntity : new() where TEntity : class, new()
    {
        /// <summary>
        /// Gets the filter definition.
        /// </summary>
        /// <value>
        /// The filter definition.
        /// </value>
        public new TableFilterDefinition<TEntity> FilterDefinition { get; }

        /// <summary>
        /// Gets the table definition.
        /// </summary>
        /// <value>
        /// The table definition.
        /// </value>
        public new TableDefinition<TEntity> TableDefinition { get; }

        /// <summary>
        /// Initializes a new instance of the <see cref="LookupDefinition{TLookupEntity, TEntity}"/> class.
        /// </summary>
        /// <param name="tableDefinition">The table definition.</param>
        public LookupDefinition(TableDefinition<TEntity> tableDefinition) : base(tableDefinition)
        {
            LookupEntityName = typeof(TLookupEntity).Name;
            TableDefinition = tableDefinition;
            base.FilterDefinition = FilterDefinition = new TableFilterDefinition<TEntity>(TableDefinition);
        }

        public LookupDefinition(TableDefinition<TEntity> tableDefinition, int advancedFindId
            , LookupRefresher lookupRefresher) : base(advancedFindId, lookupRefresher)
        {
            if (base.TableDefinition.EntityName != tableDefinition.EntityName)
            {
                var message = "Invalid Table";
                throw new Exception(message);
            }
        }

        protected override LookupDefinitionBase BaseClone()
        {
            var clone = new LookupDefinition<TLookupEntity, TEntity>(TableDefinition);
            clone.CopyLookupData(this);
            return clone;
        }

        /// <summary>
        /// Clones this instance.
        /// </summary>
        /// <returns>A copy of this object.</returns>
        public new LookupDefinition<TLookupEntity, TEntity> Clone()
        {
            return BaseClone() as LookupDefinition<TLookupEntity, TEntity>;
        }

        /// <summary>
        /// Adds a visible column definition.  Should only be used with the WPF LookupControl.
        /// </summary>
        /// <param name="lookupEntityProperty">The lookup entity property.</param>
        /// <param name="entityProperty">The entity property.</param>
        /// <returns></returns>
        public LookupFieldColumnDefinition AddVisibleColumnDefinition(
            Expression<Func<TLookupEntity, object>> lookupEntityProperty,
            Expression<Func<TEntity, object>> entityProperty)
        {
            return AddVisibleColumnDefinition(lookupEntityProperty, string.Empty, entityProperty, 0);
        }

        public LookupFieldColumnDefinition AddVisibleColumnDefinition(
            Expression<Func<TLookupEntity, object>> lookupEntityProperty,
            string caption, Expression<Func<TEntity, object>> entityProperty, double percentWidth)
        {
            return AddVisibleColumnDefinition(lookupEntityProperty, caption, entityProperty, percentWidth, "");
        }

        /// <summary>
        /// Adds the visible column definition.
        /// </summary>
        /// <param name="lookupEntityProperty">The lookup entity property.</param>
        /// <param name="caption">The caption.</param>
        /// <param name="entityProperty">The entity property.</param>
        /// <param name="percentWidth">The percent of the lookup's total width.</param>
        /// <returns></returns>
        public LookupFieldColumnDefinition AddVisibleColumnDefinition(Expression<Func<TLookupEntity, object>> lookupEntityProperty,
            string caption, Expression<Func<TEntity, object>> entityProperty, double percentWidth, string alias)
        {
            var field = TableDefinition.GetPropertyField(entityProperty.GetFullPropertyName());
            var result = AddVisibleColumnDefinition(lookupEntityProperty, caption, field, percentWidth, alias);
            return result;
        }

        /// <summary>
        /// Adds a visible formula column definition.
        /// </summary>
        /// <param name="lookupEntityProperty">The lookup entity property.</param>
        /// <param name="formula">The formula.</param>
        /// <returns></returns>
        public LookupFormulaColumnDefinition AddVisibleColumnDefinition(
            Expression<Func<TLookupEntity, object>> lookupEntityProperty, ILookupFormula lookupFormula, string alias)
        {
            return AddVisibleColumnDefinition(lookupEntityProperty, string.Empty, lookupFormula, 0, alias);
        }

        /// <summary>
        /// Adds a visible formula column definition.
        /// </summary>
        /// <param name="lookupEntityProperty">The lookup entity property.</param>
        /// <param name="caption">The caption.</param>
        /// <param name="formula">The formula.</param>
        /// <param name="percentWidth">The percent of the lookup's total width.</param>
        /// <returns></returns>
        public LookupFormulaColumnDefinition AddVisibleColumnDefinition(
            Expression<Func<TLookupEntity, object>> lookupEntityProperty, string caption, ILookupFormula lookupFormula,
            double percentWidth, string alias)
        {
            var columnName = caption;
            if (columnName.IsNullOrEmpty())
                columnName = lookupEntityProperty.GetFullPropertyName();

            ValidateProperty(lookupEntityProperty, false, columnName);
            var column = base.AddVisibleColumnDefinition(caption, lookupFormula, percentWidth,
                GetFieldDataTypeForProperty(lookupEntityProperty), alias);
            column.PropertyName = lookupEntityProperty.GetFullPropertyName();
            return column;
        }

        /// <summary>
        /// Includes the specified related property.
        /// </summary>
        /// <typeparam name="TRelatedEntity">The type of the related entity.</typeparam>
        /// <param name="relatedProperty">The related property.</param>
        /// <returns></returns>
        public LookupJoinTableEntity<TLookupEntity, TEntity, TRelatedEntity> Include<TRelatedEntity>(
            Expression<Func<TEntity, TRelatedEntity>> relatedProperty)
            where TRelatedEntity : class

        {
            var returnEntity = new LookupJoinTableEntity<TLookupEntity, TEntity, TRelatedEntity>(this,
                ((LookupDefinitionBase) this).TableDefinition, relatedProperty.GetFullPropertyName(),
                relatedProperty.ReturnType.Name);
            
            returnEntity.ParentField = returnEntity.JoinDefinition.ForeignKeyDefinition.ForeignKeyFieldJoins[0].ForeignField;

            returnEntity.ParentObject = null;
            if (returnEntity.JoinDefinition != null) returnEntity.JoinDefinition.ParentObject = null;

            //ChildField =
            //    returnEntity.JoinDefinition.ForeignKeyDefinition.ForeignKeyFieldJoins[0].ForeignField;
            return returnEntity;
        }

        /// <summary>
        /// Adds a hidden column.
        /// </summary>
        /// <param name="lookupEntityProperty">The lookup entity property.</param>
        /// <param name="entityProperty">The entity property.</param>
        /// <returns></returns>
        public LookupFieldColumnDefinition AddHiddenColumn(Expression<Func<TLookupEntity, object>> lookupEntityProperty,
            Expression<Func<TEntity, object>> entityProperty)
        {
            var field = TableDefinition.GetPropertyField(entityProperty.GetFullPropertyName());
            return AddHiddenColumn(lookupEntityProperty, field);
        }

        /// <summary>
        /// Adds a hidden column.
        /// </summary>
        /// <param name="lookupEntityProperty">The lookup entity property.</param>
        /// <param name="fieldDefinition">The field definition.</param>
        /// <returns></returns>
        private LookupFieldColumnDefinition AddHiddenColumn(Expression<Func<TLookupEntity, object>> lookupEntityProperty, FieldDefinition fieldDefinition)
        {
            ValidateProperty(lookupEntityProperty, true, string.Empty);

            var column = base.AddHiddenColumn(fieldDefinition);
            column.PropertyName = lookupEntityProperty.GetFullPropertyName();
            return column;
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

        public LookupFieldColumnDefinition GetFieldColumnDefinition(Expression<Func<TLookupEntity, object>> lookupEntityProperty)
        {
            var column = GetColumnDefinition(lookupEntityProperty);
            return column as LookupFieldColumnDefinition;
        }

        private LookupFieldColumnDefinition AddVisibleColumnDefinition(
            Expression<Func<TLookupEntity, object>> lookupEntityProperty, string caption,
            FieldDefinition fieldDefinition,
            double percentWidth, string alias)
        {
            var columnName = caption;
            if (columnName.IsNullOrEmpty())
                columnName = lookupEntityProperty.GetFullPropertyName();

            ValidateProperty(lookupEntityProperty, false, columnName);
            var column = base.AddVisibleColumnDefinition(caption, fieldDefinition, percentWidth, alias);
            column.PropertyName = lookupEntityProperty.GetFullPropertyName();
            return column;
        }

        private FieldDataTypes GetFieldDataTypeForProperty(Expression<Func<TLookupEntity, object>> lookupEntityProperty)
        {
            var propertyType = GetTypeFromExpression(lookupEntityProperty);
            return GblMethods.GetFieldDataTypeForType(propertyType);
        }


        // ReSharper disable once ParameterOnlyUsedForPreconditionCheck.Local
        private void ValidateProperty(Expression<Func<TLookupEntity, object>> lookupEntityProperty, bool hiddenProperty, string columnName)
        {
            var propertyType = GetTypeFromExpression(lookupEntityProperty);
            if (propertyType == typeof(string)
                || propertyType == typeof(DateTime)
                || propertyType == typeof(double)
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
        /// Creates a new record for this lookup's table.  Calls LookupContext's ShowAddOnTheFly method to show the appropriate database maintenance window for this table.
        /// </summary>
        /// <param name="keyText">The key text.</param>
        /// <param name="ownerWindow">The owner window.</param>
        /// <param name="newRecordPrimaryKeyValue">The new record primary key value.</param>
        /// <param name="addViewParameter">The add-on-the-fly window's input parameter.</param>
        /// <returns>
        /// The new Primary Key Value and new lookup entity.
        /// </returns>
        public NewAddOnTheFlyResult ShowAddOnTheFlyWindow(string keyText, object ownerWindow,
            PrimaryKeyValue newRecordPrimaryKeyValue = null, object addViewParameter = null, PrimaryKeyValue selectedPrimaryKeyValue = null)
        {
            var addNewRecordProcessor =
                new AddOnTheFlyProcessor<TLookupEntity, TEntity>(this, keyText, ownerWindow, newRecordPrimaryKeyValue,
                    selectedPrimaryKeyValue)
                {
                    AddViewParameter = addViewParameter
                };

            return addNewRecordProcessor.ShowAddOnTheFlyWindow();
        }

        public override LookupDataMauiBase GetLookupDataMaui(LookupDefinitionBase lookupDefinition, bool inputMode)
        {
            if (inputMode)
            {
                return new LookupDataMaui<TEntity>(lookupDefinition);
            }
            else
            {
                
                return TableDefinition.Context.GetLookupDataMaui<TEntity>(lookupDefinition);
            }
        }

        public override AutoFillDataMauiBase GetAutoFillDataMaui(AutoFillSetup setup, IAutoFillControl control)
        {
            return new AutoFillDataMaui<TEntity>(setup, control);
        }

        public override SelectQueryMauiBase GetSelectQueryMaui()
        {
            return new SelectQueryMaui<TEntity>(this);
        }

        public override AutoFillValue GetAutoFillValue(PrimaryKeyValue primaryKey)
        {
            var query = TableDefinition.Context
                .GetQueryable<TEntity>(this);

            var param = GblMethods.GetParameterExpression<TEntity>();
            Expression fullExpr = null;
            foreach (var keyValueField in primaryKey.KeyValueFields)
            {
                var pkExpr = FilterItemDefinition.GetBinaryExpression<TEntity>
                (param
                    , keyValueField.FieldDefinition.PropertyName
                    , Conditions.Equals
                    , keyValueField.FieldDefinition.FieldType
                    , keyValueField.Value
                        .GetPropertyFilterValue(keyValueField.FieldDefinition.FieldDataType
                        , keyValueField.FieldDefinition.FieldType));
                if (fullExpr == null)
                {
                    fullExpr = pkExpr;
                }
                else
                {
                    fullExpr = FilterItemDefinition.AppendExpression(fullExpr, pkExpr, EndLogics.And);
                }
            }

            query = FilterItemDefinition.FilterQuery(query, param, fullExpr);
            var entity = query.Take(1).FirstOrDefault();
            var autoFillValue = entity.GetAutoFillValue();
            return autoFillValue;
        }

        public override string CopyDataTo(DbDataProcessor destinationProcessor, int tableIndex)
        {
            if (TableDefinition.TempTable)
            {
                return string.Empty;
            }

            var context = SystemGlobals.DataRepository.GetDataContext();

            var table = context.GetTable<TEntity>();

            var destinationContext = SystemGlobals.DataRepository.GetDataContext(destinationProcessor);

            var totalRecords = table.Count();

            var tableDef = TableDefinition as TableDefinition<TEntity>;

            var identity = TableDefinition.IsIdentity();
            var recordIndex = 0;
            var batch = new List<TEntity>();
            foreach (var entity in table)
            {
                TEntity newEntity = tableDef.GetEntity() as TEntity;
                foreach (var fieldDefinition in TableDefinition.FieldDefinitions)
                {
                    var value = GblMethods.GetPropertyValue(entity, fieldDefinition.PropertyName);
                    if (value.IsNullOrEmpty() && !fieldDefinition.AllowNulls)
                    {

                    }
                    if (!value.IsNullOrEmpty())
                    {
                        GblMethods.SetPropertyValue(newEntity, fieldDefinition.PropertyName, value);
                    }
                }

                batch.Add(newEntity);

                //destinationContext.SaveEntity(newEntity, "Copying Data");
                recordIndex++;
                if (recordIndex % 10 == 0)
                {
                    destinationContext.AddRange(batch);
                    if (identity)
                    {
                        destinationContext.SetIdentityInsert(destinationProcessor, TableDefinition, true, true);
                    }

                    if (!destinationContext.Commit("Copying Data", true))
                    {
                        var lastError = GblMethods.LastError;
                        if (identity)
                        {
                            destinationContext.SetIdentityInsert(destinationProcessor, TableDefinition, false, false);
                        }

                        return lastError;
                    }
                    batch.Clear();
                    if (identity)
                    {
                        destinationContext.SetIdentityInsert(destinationProcessor, TableDefinition, true, false);
                    }
                    var args = new CopyProcedureArgs
                    {
                        TableDefinitionBeingProcessed = TableDefinition,
                        TableIdBeingProcessed = tableIndex,
                        TotalRecords = totalRecords,
                        RecordBeingProcessed = recordIndex,
                    };
                    TableDefinition.Context.FireCopyProcedureEvent(args);
                }
            }

            if (batch.Any())
            {
                if (identity)
                {
                    destinationContext.SetIdentityInsert(destinationProcessor, TableDefinition, true, true);
                }
                destinationContext.AddRange(batch);
                if (!destinationContext.Commit("Copying Data", true))
                {
                    var lastError = GblMethods.LastError;
                    if (identity)
                    {
                        destinationContext.SetIdentityInsert(destinationProcessor, TableDefinition, false, false);
                    }

                    return lastError;
                }
            }
            if (identity)
            {
                destinationContext.SetIdentityInsert(destinationProcessor, TableDefinition, true, false);
            }

            var args1 = new CopyProcedureArgs
            {
                TableDefinitionBeingProcessed = TableDefinition,
                TableIdBeingProcessed = tableIndex,
                TotalRecords = totalRecords,
                RecordBeingProcessed = recordIndex,
            };
            TableDefinition.Context.FireCopyProcedureEvent(args1);


            return base.CopyDataTo(destinationProcessor, tableIndex);
        }
    }
}
