using RingSoft.DbLookup.ModelDefinition;
using RingSoft.DbLookup.ModelDefinition.FieldDefinitions;
using RingSoft.DbLookup.TableProcessing;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using RingSoft.DbLookup.QueryBuilder;

namespace RingSoft.DbLookup.Lookup
{
    /// <summary>
    /// Contains all the data necessary for a lookup.
    /// </summary>
    public class LookupDefinitionBase
    {
        /// <summary>
        /// Gets the table definition.
        /// </summary>
        /// <value>
        /// The table definition.
        /// </value>
        public TableDefinitionBase TableDefinition { get; set; }

        /// <summary>
        /// Gets the visible columns.
        /// </summary>
        /// <value>
        /// The visible columns.
        /// </value>
        public IReadOnlyList<LookupColumnDefinitionBase> VisibleColumns => _visibleColumns;
        
        /// <summary>
        /// Gets the hidden columns.
        /// </summary>
        /// <value>
        /// The hidden columns.
        /// </value>
        public IReadOnlyList<LookupColumnDefinitionBase> HiddenColumns => _hiddenColumns;

        /// <summary>
        /// Gets the joins.
        /// </summary>
        /// <value>
        /// The joins.
        /// </value>
        public IReadOnlyList<TableFieldJoinDefinition> Joins => _joinsList;

        private LookupColumnDefinitionBase _initialSortLookupColumnDefinition;

        /// <summary>
        /// Gets and sets the initial sort column definition.
        /// </summary>
        /// <value>
        /// The initial sort column definition.
        /// </value>
        public LookupColumnDefinitionBase InitialSortColumnDefinition
        {
            get => _initialSortLookupColumnDefinition;
            set
            {
                if (value.LookupDefinition != this)
                    throw new ArgumentException($"Sort column {value.PropertyName}'s lookup definition does not match this.");

                _initialSortLookupColumnDefinition = value;
            }
        }

        /// <summary>
        /// Gets and sets the initial type of the order by.
        /// </summary>
        /// <value>
        /// The initial type of the order by.
        /// </value>
        public OrderByTypes InitialOrderByType { get; set; }

        /// <summary>
        /// Gets the name of the lookup entity.
        /// </summary>
        /// <value>
        /// The name of the lookup entity.
        /// </value>
        public string LookupEntityName { get; internal set; }

        /// <summary>
        /// Gets the filter definition.
        /// </summary>
        /// <value>
        /// The filter definition.
        /// </value>
        public TableFilterDefinitionBase FilterDefinition { get; internal set; }

        /// <summary>
        /// Gets or sets the title that shows on the lookup window.
        /// </summary>
        /// <value>
        /// The title.
        /// </value>
        public string Title { get; set; }

        public bool ReadOnlyMode { get; set; }

        public string FromFormula { get; internal set; }

        public LookupDefinitionBase HasFromFormula(string value)
        {
            FromFormula = value;
            return this;
        }

        public void ClearVisibleColumns()
        {
            _visibleColumns.Clear();            
        }


        private readonly List<LookupColumnDefinitionBase> _visibleColumns = new List<LookupColumnDefinitionBase>();
        private readonly List<LookupColumnDefinitionBase> _hiddenColumns = new List<LookupColumnDefinitionBase>();
        private readonly List<TableFieldJoinDefinition> _joinsList = new List<TableFieldJoinDefinition>();

        public LookupDefinitionBase(TableDefinitionBase tableDefinition)
        {
            tableDefinition.Context.Initialize();
            TableDefinition = tableDefinition;
            FilterDefinition = new TableFilterDefinitionBase();
        }

        protected virtual LookupDefinitionBase BaseClone()
        {
            var clone = new LookupDefinitionBase(TableDefinition);
            clone.CopyLookupData(this);
            return clone;
        }

        /// <summary>
        /// Clones this instance.
        /// </summary>
        /// <returns>A copy of this object.</returns>
        public LookupDefinitionBase Clone()
        {
            return BaseClone();
        }

        public virtual void CopyLookupData(LookupDefinitionBase source)
        {
            CopyColumns(source.VisibleColumns, false);
            CopyColumns(source.HiddenColumns, true);

            foreach (var joinDefinition in source.Joins)
            {
                var newJoin = new TableFieldJoinDefinition();
                newJoin.CopyFrom(joinDefinition);
                _joinsList.Add(newJoin);
            }

            FilterDefinition.CopyFrom(source.FilterDefinition);
            InitialOrderByType = source.InitialOrderByType;
            FromFormula = source.FromFormula;
            ReadOnlyMode = source.ReadOnlyMode;
            ParentObject = source.ParentObject;
            ChildField = source.ChildField;
        }

        private void CopyColumns(IReadOnlyList<LookupColumnDefinitionBase> sourceColumnList, bool hidden)
        {
            foreach (var columnType in sourceColumnList)
            {
                switch (columnType.ColumnType)
                {
                    case LookupColumnTypes.Field:
                        if (columnType is LookupFieldColumnDefinition fieldColumn)
                        {
                            if (hidden)
                            {
                                var newColumn = AddHiddenColumn(fieldColumn.FieldDefinition);
                                newColumn.CopyFrom(columnType);
                            }
                            else
                            {
                                var newColumn = AddVisibleColumnDefinition(fieldColumn.Caption,
                                    fieldColumn.FieldDefinition,
                                    columnType.PercentWidth,
                                    fieldColumn.JoinQueryTableAlias);
                                newColumn.CopyFrom(columnType);
                            }
                        }

                        break;
                    case LookupColumnTypes.Formula:
                        if (columnType is LookupFormulaColumnDefinition formulaColumn)
                        {
                            if (hidden)
                            {
                                var newColumn = AddHiddenColumn(formulaColumn.Formula, formulaColumn.DataType);
                                newColumn.CopyFrom(columnType);
                            }
                            else
                            {
                                var newColumn = AddVisibleColumnDefinition(formulaColumn.Caption, formulaColumn.Formula,
                                    formulaColumn.PercentWidth, formulaColumn.DataType, formulaColumn.JoinQueryTableAlias);
                                newColumn.CopyFrom(columnType);
                            }
                        }
                        break;
                    default:
                        throw new ArgumentOutOfRangeException();
                }
            }
        }

        internal LookupFieldColumnDefinition AddHiddenColumn(FieldDefinition fieldDefinition)
        {
            var isPrimaryKey = fieldDefinition.TableDefinition.PrimaryKeyFields.Contains(fieldDefinition);
            if (!isPrimaryKey)
            {
                ValidateNonPrimaryKeyDistinctColumns();
            }

            var columnDefinition = new LookupFieldColumnDefinition(fieldDefinition)
            {
                LookupDefinition = this
            };

            _hiddenColumns.Add(columnDefinition);
            return columnDefinition;
        }

        internal LookupFormulaColumnDefinition AddHiddenColumn(string formula, FieldDataTypes dataType)
        {
            ValidateNonPrimaryKeyDistinctColumns();

            var columnDefinition = new LookupFormulaColumnDefinition(formula, dataType)
            {
                LookupDefinition = this
            };

            _hiddenColumns.Add(columnDefinition);
            return columnDefinition;
        }

        public LookupFieldColumnDefinition AddVisibleColumnDefinition(string caption, FieldDefinition fieldDefinition, double percentWidth, string alias)
        {
            var isPrimaryKey = fieldDefinition.TableDefinition.PrimaryKeyFields.Contains(fieldDefinition);
            if (!isPrimaryKey)
            {
                ValidateNonPrimaryKeyDistinctColumns();
            }
            var column = new LookupFieldColumnDefinition(fieldDefinition)
            {
                Caption = caption,
                LookupDefinition = this,
                PercentWidth = percentWidth,
                JoinQueryTableAlias = alias
            };

            ProcessVisibleColumnDefinition(column);
            _visibleColumns.Add(column);
            column.ChildField = fieldDefinition.TableDefinition.PrimaryKeyFields[0];
            return column;
        }

        private void ValidateNonPrimaryKeyDistinctColumns()
        {
            if (GetDistinctColumns().Any())
                throw new ArgumentException(
                    "Adding non-primary key fields to lookup definitions with distinct columns is not allowed.");
        }

        internal List<LookupFieldColumnDefinition> GetDistinctColumns()
        {
            var visibleDistinctColumns = GetDistinctColumns(VisibleColumns);
            var hiddenDistinctColumns = GetDistinctColumns(HiddenColumns);

            var result = new List<LookupFieldColumnDefinition>();

            if (visibleDistinctColumns.Any())
            {
                foreach (var column in visibleDistinctColumns)
                {
                    result.Add(column);
                }
            }

            if (hiddenDistinctColumns.Any())
            {
                foreach (var column in hiddenDistinctColumns)
                {
                    result.Add(column);
                }
            }

            return result;
        }

        internal List<LookupFieldColumnDefinition> GetDistinctColumns(IReadOnlyList<LookupColumnDefinitionBase> columns)
        {
            var result = new List<LookupFieldColumnDefinition>();
            var fieldColumns = columns.Where(w => w.ColumnType == LookupColumnTypes.Field);
            foreach (var column in fieldColumns)
            {
                if (column is LookupFieldColumnDefinition fieldColumn)
                {
                    if (fieldColumn.Distinct)
                        result.Add(fieldColumn);
                }
            }

            return result;
        }

        public LookupFormulaColumnDefinition AddVisibleColumnDefinition(string caption, string formula,
            double percentWidth, FieldDataTypes dataType, string alias)
        {
            ValidateNonPrimaryKeyDistinctColumns();

            var column = new LookupFormulaColumnDefinition(formula, dataType)
            {
                Caption = caption,
                LookupDefinition = this,
                PercentWidth = percentWidth
            };

            //if (fieldDefinition != null)
            {
                //column.FieldDefinition = fieldDefinition;
                //column.JoinQueryTableAlias = fieldDefinition.ParentJoinForeignKeyDefinition.Alias;

                column.JoinQueryTableAlias = alias;
            }
            ProcessVisibleColumnDefinition(column);

            //if (join != null)
            //    column.JoinQueryTableAlias = join.Alias;
            _visibleColumns.Add(column);
            return column;
        }

        internal void ProcessVisibleColumnDefinition(LookupColumnDefinitionBase columnDefinition)
        {
            if (InitialSortColumnDefinition == null)
                InitialSortColumnDefinition = columnDefinition;
        }

        internal void AddJoin(TableFieldJoinDefinition lookupFieldJoin)
        {
            if (_joinsList.All(p => p.ForeignKeyDefinition.Alias != lookupFieldJoin.ForeignKeyDefinition.Alias))
                _joinsList.Add(lookupFieldJoin);

            foreach (var column in VisibleColumns)
            {
                //column.JoinQueryTableAlias = lookupFieldJoin.Alias;
            }
        }

        /// <summary>
        /// Gets the index of visible column.
        /// </summary>
        /// <param name="visibleColumnDefinition">The visible column definition.</param>
        /// <returns></returns>
        public int GetIndexOfVisibleColumn(LookupColumnDefinitionBase visibleColumnDefinition)
        {
            return _visibleColumns.IndexOf(visibleColumnDefinition);
        }

        public LookupJoin Include(FieldDefinition foreignFieldDefinition)
        {
            var lookupJoin = new LookupJoin(this, foreignFieldDefinition);
            return lookupJoin;
        }

        public IJoinParent ParentObject { get; set; }
        public FieldDefinition ChildField { get; set; }
        public LookupJoin MakeInclude(LookupDefinitionBase lookupDefinition, FieldDefinition childField = null)
        {
            if (childField == null)
            {
                childField = ChildField;
            }
            return Include(childField);
        }

        public LookupColumnDefinitionBase AddVisibleColumnDefinitionField(string caption, FieldDefinition fieldDefinition,
            double percentWidth)
        {
            return AddVisibleColumnDefinitionField(caption, fieldDefinition, percentWidth);
        }
    }
}
