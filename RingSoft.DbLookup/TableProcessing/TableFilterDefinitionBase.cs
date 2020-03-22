using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using RingSoft.DbLookupCore.ModelDefinition.FieldDefinitions;
using RingSoft.DbLookupCore.QueryBuilder;

namespace RingSoft.DbLookupCore.TableProcessing
{
    /// <summary>
    /// Arguments sent with the FilterCopied event.
    /// </summary>
    public class TableFilterCopiedArgs
    {
        /// <summary>
        /// Gets the source.
        /// </summary>
        /// <value>
        /// The source.
        /// </value>
        public TableFilterDefinitionBase Source { get; }

        /// <summary>
        /// Initializes a new instance of the <see cref="TableFilterCopiedArgs"/> class.
        /// </summary>
        /// <param name="source">The source.</param>
        public TableFilterCopiedArgs(TableFilterDefinitionBase source)
        {
            Source = source;
        }
    }
    /// <summary>
    /// A table filter definition not based on an entity.
    /// </summary>
    public class TableFilterDefinitionBase
    {
        /// <summary>
        /// Gets the fixed filters.
        /// </summary>
        /// <value>
        /// The fixed filters.
        /// </value>
        public IReadOnlyList<FieldFilterDefinition> FixedFilters => _fixedFilterDefinitions;

        /// <summary>
        /// Gets the user filters.
        /// </summary>
        /// <value>
        /// The user filters.
        /// </value>
        public IReadOnlyList<FieldFilterDefinition> UserFilters => _userFilterDefinitions;

        /// <summary>
        /// Gets the joins.
        /// </summary>
        /// <value>
        /// The joins.
        /// </value>
        public IReadOnlyList<TableFieldJoinDefinition> Joins => _joinDefinitions;

        /// <summary>
        /// Occurs when this filter is copied from a source table filter definition.  Used by subscribers (like AutoFill) to synchronize filters.
        /// </summary>
        public event EventHandler<TableFilterCopiedArgs> FilterCopied;

        private readonly List<FieldFilterDefinition> _fixedFilterDefinitions = new List<FieldFilterDefinition>();
        private readonly List<FieldFilterDefinition> _userFilterDefinitions = new List<FieldFilterDefinition>();
        private readonly List<TableFieldJoinDefinition> _joinDefinitions = new List<TableFieldJoinDefinition>();

        /// <summary>
        /// Clears the fixed filters.
        /// </summary>
        public void ClearFixedFilters()
        {
            _fixedFilterDefinitions.Clear();
        }

        /// <summary>
        /// Clears this filter and copies the source filter data to this object.
        /// </summary>
        /// <param name="source">The source.</param>
        public void CopyFrom(TableFilterDefinitionBase source)
        {
            _joinDefinitions.Clear();
            CopyFilters(source.FixedFilters, _fixedFilterDefinitions);
            CopyFilters(source.UserFilters, _userFilterDefinitions);
            OnTableFilterCopied(new TableFilterCopiedArgs(source));
        }

        protected void OnTableFilterCopied(TableFilterCopiedArgs e)
        {
            FilterCopied?.Invoke(this, e);
        }

        private void CopyFilters(IReadOnlyList<FieldFilterDefinition> sourceFilters, List<FieldFilterDefinition> destinationFilters)
        {
            destinationFilters.Clear();
            foreach (var sourceFilter in sourceFilters)
            {
                var fieldFilter = new FieldFilterDefinition
                {
                    TableFilterDefinition = this
                };
                fieldFilter.CopyFrom(sourceFilter);
                destinationFilters.Add(fieldFilter);
            }
        }

        protected FieldFilterDefinition CreateFieldFilter(FieldDefinition fieldDefinition, Conditions condition,
            string value)
        {
            var fieldFilter = new FieldFilterDefinition
            {
                TableFilterDefinition = this,
                FieldDefinition = fieldDefinition,
                Condition = condition,
                Value = value
            };

            return fieldFilter;
        }

        private FieldFilterDefinition CreateAddFixedFilter(FieldDefinition fieldDefinition, Conditions condition,
            string value)
        {
            var fieldFilter = CreateFieldFilter(fieldDefinition, condition, value);
            _fixedFilterDefinitions.Add(fieldFilter);
            return fieldFilter;
        }

        /// <summary>
        /// Adds a fixed filter.
        /// </summary>
        /// <param name="fieldDefinition">The field definition.</param>
        /// <param name="condition">The condition.</param>
        /// <param name="value">The value.</param>
        /// <returns>The added filter.</returns>
        public FieldFilterDefinition AddFixedFilter(StringFieldDefinition fieldDefinition, Conditions condition,
            string value)
        {
            return CreateAddFixedFilter(fieldDefinition, condition, value);
        }

        /// <summary>
        /// Adds a fixed filter.
        /// </summary>
        /// <param name="fieldDefinition">The field definition.</param>
        /// <param name="condition">The condition.</param>
        /// <param name="value">The value.</param>
        /// <returns>The added filter.</returns>
        public FieldFilterDefinition AddFixedFilter(IntegerFieldDefinition fieldDefinition, Conditions condition,
            int value)
        {
            return CreateAddFixedFilter(fieldDefinition, condition, value.ToString());
        }

        /// <summary>
        /// Adds a fixed filter.
        /// </summary>
        /// <param name="fieldDefinition">The field definition.</param>
        /// <param name="condition">The condition.</param>
        /// <param name="value">The value.</param>
        /// <returns>The added filter.</returns>
        public FieldFilterDefinition AddFixedFilter(DecimalFieldDefinition fieldDefinition, Conditions condition,
            double value)
        {
            return CreateAddFixedFilter(fieldDefinition, condition, value.ToString(CultureInfo.CurrentCulture));
        }

        /// <summary>
        /// Adds a fixed filter.
        /// </summary>
        /// <param name="fieldDefinition">The field definition.</param>
        /// <param name="condition">The condition.</param>
        /// <param name="value">The value.</param>
        /// <returns>The added filter.</returns>
        public FieldFilterDefinition AddFixedFilter(DecimalFieldDefinition fieldDefinition, Conditions condition,
            decimal value)
        {
            return CreateAddFixedFilter(fieldDefinition, condition, value.ToString(CultureInfo.CurrentCulture));
        }

        /// <summary>
        /// Adds a fixed filter.
        /// </summary>
        /// <param name="fieldDefinition">The field definition.</param>
        /// <param name="condition">The condition.</param>
        /// <param name="value">The value.</param>
        /// <returns>The added filter.</returns>
        public FieldFilterDefinition AddFixedFilter(DateFieldDefinition fieldDefinition, Conditions condition,
            DateTime value)
        {
            return CreateAddFixedFilter(fieldDefinition, condition, value.ToString(CultureInfo.CurrentCulture));
        }

        /// <summary>
        /// Adds a fixed filter.
        /// </summary>
        /// <param name="fieldDefinition">The field definition.</param>
        /// <param name="condition">The condition.</param>
        /// <param name="value">if set to <c>true</c> [value].</param>
        /// <returns>The added filter.</returns>
        public FieldFilterDefinition AddFixedFilter(BoolFieldDefinition fieldDefinition, Conditions condition,
            bool value)
        {
            return CreateAddFixedFilter(fieldDefinition, condition, SelectQuery.BoolToString(value));
        }

        /// <summary>
        /// Adds a fixed filter.
        /// </summary>
        /// <param name="fieldDefinition">The field definition.</param>
        /// <param name="condition">The condition.</param>
        /// <param name="value">The value.</param>
        /// <returns>The added filter.</returns>
        public FieldFilterDefinition AddFixedFilter(EnumFieldDefinition fieldDefinition, Conditions condition,
            Enum value)
        {
            var numValue = Convert.ToInt32(value);
            return CreateAddFixedFilter(fieldDefinition, condition, numValue.ToString());
        }

        /// <summary>
        /// Adds a fixed filter.
        /// </summary>
        /// <param name="fieldDefinition">The field definition.</param>
        /// <param name="condition">The condition.</param>
        /// <param name="value">The value.</param>
        /// <returns>The added filter.</returns>
        public FieldFilterDefinition AddFixedFilter(EnumFieldDefinition fieldDefinition, Conditions condition,
            string value)
        {
            var result = CreateAddFixedFilter(fieldDefinition, condition, value);
            result.CastEnumValueAsInt = false;
            return result;
        }

        internal void AddJoin(TableFieldJoinDefinition foreignKeyDefinition)
        {
            if (!_joinDefinitions.Contains(foreignKeyDefinition))
                _joinDefinitions.Add(foreignKeyDefinition);
        }

        /// <summary>
        /// Adds a user filter.
        /// </summary>
        /// <param name="fieldDefinition">The field definition.</param>
        /// <param name="condition">The condition.</param>
        /// <param name="value">The value.</param>
        /// <returns>The added filter.</returns>
        public FieldFilterDefinition AddUserFilter(FieldDefinition fieldDefinition, Conditions condition,
            string value)
        {
            var fieldFilter = CreateFieldFilter(fieldDefinition, condition, value);
            _userFilterDefinitions.Add(fieldFilter);
            return fieldFilter;
        }

        internal void ProcessQuery(SelectQuery query)
        {
            ProcessFieldJoins(query, Joins);
            ProcessFilters(query, FixedFilters);
            ProcessFilters(query, UserFilters);
        }

        private void ProcessFilters(SelectQuery query, IReadOnlyList<FieldFilterDefinition> filters)
        {
            WhereItem firstWhere = null, lastWhere = null;
            foreach (var fieldFilterDefinition in filters)
            {
                var queryTable = GetQueryTableForFieldFilter(query, fieldFilterDefinition);
                var value = fieldFilterDefinition.Value;

                var dateType = DbDateTypes.DateOnly;
                if (fieldFilterDefinition.FieldDefinition.FieldDataType == FieldDataTypes.DateTime)
                {
                    var dateField = fieldFilterDefinition.FieldDefinition as DateFieldDefinition;
                    if (dateField != null)
                        dateType = dateField.DateType;
                }

                if (fieldFilterDefinition.FieldDefinition.FieldDataType == FieldDataTypes.Enum && !fieldFilterDefinition.CastEnumValueAsInt)
                {
                    var enumField = fieldFilterDefinition.FieldDefinition as EnumFieldDefinition;
                    if (enumField != null)
                        lastWhere = query.AddWhereItemEnum(queryTable, enumField.FieldName,
                            fieldFilterDefinition.Condition, value, enumField.EnumTranslation);
                }
                else
                {
                    lastWhere = query.AddWhereItem(queryTable, fieldFilterDefinition.FieldDefinition.FieldName,
                        fieldFilterDefinition.Condition, value, fieldFilterDefinition.FieldDefinition.ValueType, dateType);
                }

                if (lastWhere != null)
                {
                    lastWhere.SetEndLogic(fieldFilterDefinition.EndLogic)
                        .SetLeftParenthesesCount(fieldFilterDefinition.LeftParenthesesCount)
                        .SetRightParenthesesCount(fieldFilterDefinition.RightParenthesesCount)
                        .IsCaseSensitive(fieldFilterDefinition.CaseSensitive);

                    if (firstWhere == null)
                        firstWhere = lastWhere;
                }
            }

            if (firstWhere != null)
            {
                firstWhere.SetLeftParenthesesCount(firstWhere.LeftParenthesesCount++);
                if (lastWhere != null)
                {
                    lastWhere.SetRightParenthesesCount(lastWhere.RightParenthesesCount++);
                    lastWhere.SetEndLogic(EndLogics.And);
                }
            }
        }

        internal static void ProcessFieldJoins(SelectQuery query, IReadOnlyList<TableFieldJoinDefinition> joins)
        {
            foreach (var tableFieldJoinDefinition in joins)
            {
                var joinType = JoinTypes.InnerJoin;
                if (tableFieldJoinDefinition.ForeignKeyDefinition.FieldJoins[0].ForeignField.AllowNulls)
                    joinType = JoinTypes.LeftOuterJoin;

                QueryTable foreignTable =
                    query.JoinTables.FirstOrDefault(f => f.Alias == tableFieldJoinDefinition.ParentAlias);
                if (foreignTable == null)
                    foreignTable = query.BaseTable;

                if (query.JoinTables.All(a => a.Alias != tableFieldJoinDefinition.Alias))
                {
                    var joinTable = query.AddPrimaryJoinTable(joinType, foreignTable,
                        tableFieldJoinDefinition.ForeignKeyDefinition.PrimaryTable.TableName,
                        tableFieldJoinDefinition.Alias);

                    foreach (var foreignKeyFieldJoin in tableFieldJoinDefinition.ForeignKeyDefinition.FieldJoins)
                    {
                        joinTable.AddJoinField(foreignKeyFieldJoin.PrimaryField.FieldName,
                            foreignKeyFieldJoin.ForeignField.FieldName);
                    }
                }
            }
        }

        private QueryTable GetQueryTableForFieldFilter(SelectQuery query, FieldFilterDefinition fieldFilterDefinition)
        {
            if (fieldFilterDefinition.JoinDefinition != null)
            {
                QueryTable foreignTable =
                    query.JoinTables.FirstOrDefault(f => f.Alias == fieldFilterDefinition.JoinDefinition.Alias);
                if (foreignTable != null)
                    return foreignTable;
            }

            return query.BaseTable;
        }

    }
}
