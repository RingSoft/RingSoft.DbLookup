using RingSoft.DbLookup.ModelDefinition;
using RingSoft.DbLookup.ModelDefinition.FieldDefinitions;
using RingSoft.DbLookup.TableProcessing;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using RingSoft.DataEntryControls.Engine;
using RingSoft.DbLookup.QueryBuilder;
using RingSoft.DbLookup.AdvancedFind;
using RingSoft.DbLookup.DataProcessor;

namespace RingSoft.DbLookup.Lookup
{
    public class LookupFilterReturn
    {
        public FilterItemDefinition FilterItemDefinition { get; set; }

        public FieldDefinition FieldDefinition { get; set; }
    }

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
                    throw new ArgumentException(
                        $"Sort column {value.PropertyName}'s lookup definition does not match this.");

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

        public bool AllowAddOnTheFly { get; set; } = true;

        public string FromFormula { get; internal set; }

        public AdvancedFindTree AdvancedFindTree { get; set; }

        public AdvancedFind.AdvancedFind Entity { get; internal set; }

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
            FilterDefinition = new TableFilterDefinitionBase(tableDefinition);
        }

        public LookupDefinitionBase(int advancedFindId)
        {
            Entity = SystemGlobals.AdvancedFindDbProcessor.GetAdvancedFind(advancedFindId);
            TableDefinition =
                SystemGlobals.AdvancedFindLookupContext.AdvancedFinds.Context.TableDefinitions.FirstOrDefault(p =>
                    p.EntityName == Entity.Table);
            if (TableDefinition == null)
            {
                var message = "Invalid table";
                throw new Exception(message);
            }

            FilterDefinition = new TableFilterDefinitionBase(TableDefinition);
            AdvancedFindTree = new AdvancedFindTree(this);
            AdvancedFindTree.LoadTree(TableDefinition.TableName);

            FromFormula = Entity.FromFormula;
            foreach (var advancedFindColumn in Entity.Columns)
            {
                LoadFromAdvFindColumnEntity(advancedFindColumn);
            }

            foreach (var advancedFindFilter in Entity.Filters)
            {
                LoadFromAdvFindFilter(advancedFindFilter);
            }
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
            AllowAddOnTheFly = source.AllowAddOnTheFly;
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
                                var newColumn = AddVisibleColumnDefinition(formulaColumn.Caption, formulaColumn.OriginalFormula,
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

        public void DeleteVisibleColumn(LookupColumnDefinitionBase column)
        {
            _visibleColumns.Remove(column);
            if (column == InitialSortColumnDefinition && VisibleColumns.Any())
            {
                InitialSortColumnDefinition = VisibleColumns[0];
            }
        }

        public LookupColumnDefinitionBase LoadFromAdvFindColumnEntity(AdvancedFindColumn entity)
        {
            var tableDefinition =
                TableDefinition.Context.TableDefinitions.FirstOrDefault(p =>
                    p.EntityName == entity.TableName);
            
            FieldDefinition fieldDefinition = null;
            var fieldDescription = string.Empty;
            if (!entity.FieldName.IsNullOrEmpty())
            {
                fieldDefinition =
                    tableDefinition.FieldDefinitions.FirstOrDefault(p => p.FieldName == entity.FieldName);
                fieldDescription = fieldDefinition.Description;
            }

            TableDefinitionBase primaryTable = null;
            FieldDefinition primaryField = null;
            if (!entity.PrimaryTableName.IsNullOrEmpty() && !entity.PrimaryFieldName.IsNullOrEmpty())
            {
                primaryTable =
                    TableDefinition.Context.TableDefinitions.FirstOrDefault(p =>
                        p.EntityName == entity.PrimaryTableName);

                primaryField =
                    primaryTable.FieldDefinitions.FirstOrDefault(p => p.FieldName == entity.PrimaryFieldName);

                if (fieldDefinition == null)
                {
                    tableDefinition = primaryTable;
                    fieldDefinition = primaryField;
                }
            }

            var foundTreeViewItem = AdvancedFindTree.ProcessFoundTreeViewItem(entity.Formula, fieldDefinition,
                (FieldDataTypes)entity.FieldDataType, (DecimalEditFormatTypes)entity.DecimalFormatType);

            var result = AdvancedFindTree.MakeIncludes(foundTreeViewItem, entity.Caption).ColumnDefinition;
            result.TableDescription = tableDefinition.Description;
            result.FieldDescription = fieldDescription;
            result.PercentWidth = entity.PercentWidth * 100;

            if (result is LookupFormulaColumnDefinition lookupFormulaColumn)
            {
                lookupFormulaColumn.HasDataType((FieldDataTypes)entity.FieldDataType);
                if (entity.DecimalFormatType > 0)
                {
                    lookupFormulaColumn.DecimalFieldType = (DecimalFieldTypes)entity.DecimalFormatType;
                }
            }
            return result;
        }

        public LookupFilterReturn LoadFromAdvFindFilter(AdvancedFindFilter entity, bool addFilterToLookup = true)
        {
            var result = new LookupFilterReturn();
            FilterItemDefinition filterItemDefinition = null;
            if (entity.SearchForAdvancedFindId != null)
            {
                if (addFilterToLookup)
                {
                    filterItemDefinition = FilterDefinition.AddUserFilter(entity.SearchForAdvancedFindId.Value, this);
                    var afTableDefinition =
                        GetTableFieldForFilter(entity, out var afFieldDefinition, out var afFilterField);
                    TreeViewItem afItem = null;
                    if (afFieldDefinition != null)
                    {
                        afItem = AdvancedFindTree.ProcessFoundTreeViewItem(string.Empty, afFieldDefinition);
                    }
                    SetFilterProperties(entity, filterItemDefinition, afItem, true);
                    //filterItemDefinition.TableDescription = SystemGlobals.AdvancedFindDbProcessor
                    //    .GetAdvancedFind(entity.AdvancedFindId).Table;
                    result.FilterItemDefinition = filterItemDefinition;
                }
                return result;
            }
            var tableDefinition = GetTableFieldForFilter(entity, out var fieldDefinition, out var filterField);

            if (tableDefinition == null)
            {
                var message = $"Advanced Find Id {entity.AdvancedFindId} Filter Id {entity.FilterId} is corrupt.";
                throw new Exception(message);
            }
            var foundTreeViewItem = AdvancedFindTree.ProcessFoundTreeViewItem(entity.Formula, fieldDefinition);

            var includeResult = AdvancedFindTree.MakeIncludes(foundTreeViewItem, string.Empty, false);
            var formula = entity.Formula;
            var lookupField = fieldDefinition;
            
            if (fieldDefinition != null)
            {
                if (fieldDefinition.ParentJoinForeignKeyDefinition != null)
                {
                    var condition = (Conditions)entity.Operand;
                    switch (condition)
                    {
                        case Conditions.Equals:
                        case Conditions.NotEquals:
                            if (fieldDefinition.ParentJoinForeignKeyDefinition != null)
                            {
                                lookupField = fieldDefinition.ParentJoinForeignKeyDefinition.FieldJoins[0].PrimaryField;
                            }

                            break;
                        default:
                            var lookupColumn = fieldDefinition.ParentJoinForeignKeyDefinition
                                .PrimaryTable.LookupDefinition.InitialSortColumnDefinition;
                            if (lookupColumn is LookupFormulaColumnDefinition lookupFormulaColumn)
                            {
                                formula = lookupFormulaColumn.OriginalFormula;
                            }
                            else if (lookupColumn is LookupFieldColumnDefinition lookupFieldColumn)
                            {
                                lookupField = lookupFieldColumn.FieldDefinition;
                            }
                            break;
                    }
                }
            }

            if (addFilterToLookup)
            {
                if (formula.IsNullOrEmpty())
                {
                    filterItemDefinition = FilterDefinition.AddUserFilter(lookupField, (Conditions)entity.Operand,
                        entity.SearchForValue);
                }
                else
                {
                    var alias = includeResult.LookupJoin?.JoinDefinition.Alias;
                    filterItemDefinition = FilterDefinition.AddUserFilter(formula, (Conditions)entity.Operand,
                        entity.SearchForValue, alias, (FieldDataTypes)entity.FormulaDataType);
                }
            }
            else
            {
                if (formula.IsNullOrEmpty())
                {
                    filterItemDefinition = FilterDefinition.CreateFieldFilter(lookupField, (Conditions)entity.Operand,
                        entity.SearchForValue);
                }
                else
                {
                    var alias = includeResult.LookupJoin?.JoinDefinition.Alias;
                    filterItemDefinition = FilterDefinition.CreateFormulaFilter(formula, (FieldDataTypes)entity.FormulaDataType, (Conditions)entity.Operand,
                        entity.SearchForValue, alias);
                }
            }

            if (includeResult.LookupJoin != null)
            {
                filterItemDefinition.JoinDefinition = includeResult.LookupJoin.JoinDefinition;
            }

            if (foundTreeViewItem.Parent != null)
            {
                filterItemDefinition.TableDescription = foundTreeViewItem.Parent.Name;
            }
            else
            {
                filterItemDefinition.TableDescription = tableDefinition.Description;
            }

            SetFilterProperties(entity, filterItemDefinition, foundTreeViewItem);
            result.FilterItemDefinition = filterItemDefinition;
            if (entity.Formula.IsNullOrEmpty())
            {
                result.FieldDefinition = filterField;
            }
            
            return result;
        }

        private TableDefinitionBase GetTableFieldForFilter(AdvancedFindFilter entity, out FieldDefinition fieldDefinition,
            out FieldDefinition filterField)
        {
            var tableDefinition =
                TableDefinition.Context.TableDefinitions.FirstOrDefault(p =>
                    p.EntityName == entity.TableName);

            fieldDefinition = null;
            filterField = null;
            if (!entity.FieldName.IsNullOrEmpty())
            {
                filterField = fieldDefinition =
                    tableDefinition.FieldDefinitions.FirstOrDefault(p => p.FieldName == entity.FieldName);
            }

            TableDefinitionBase primaryTable = null;
            FieldDefinition primaryField = null;
            if (!entity.PrimaryTableName.IsNullOrEmpty() && !entity.PrimaryFieldName.IsNullOrEmpty())
            {
                primaryTable =
                    TableDefinition.Context.TableDefinitions.FirstOrDefault(p =>
                        p.TableName == entity.PrimaryTableName);

                primaryField =
                    primaryTable.FieldDefinitions.FirstOrDefault(p => p.FieldName == entity.PrimaryFieldName);

                if (fieldDefinition == null)
                {
                    tableDefinition = primaryTable;
                    fieldDefinition = primaryField;
                }
            }

            return tableDefinition;
        }

        private void SetFilterProperties(AdvancedFindFilter entity, 
            FilterItemDefinition filterItemDefinition, TreeViewItem foundItem, bool isAdvFind = false)
        {
            filterItemDefinition.LeftParenthesesCount = entity.LeftParentheses;
            filterItemDefinition.RightParenthesesCount = entity.RightParentheses;
            filterItemDefinition.EndLogic = (EndLogics)entity.EndLogic;
            if (foundItem != null)
            {
                if (foundItem.Parent != null)
                {
                    filterItemDefinition.TableDescription = foundItem.Parent.Name;
                }
                else
                {
                    if (isAdvFind)
                    {
                        filterItemDefinition.TableDescription = foundItem.Name;
                    }
                    else
                    {
                        filterItemDefinition.TableDescription = TableDefinition.Description;
                    }
                }
            }
            else
            {
                filterItemDefinition.TableDescription = TableDefinition.Description;
            }
        }

        public void GetCountQuery(QuerySet querySet, string name)
        {
            var lookupInterface = new LookupUserInterface
            {
                PageSize = 0
            };
            var lookupData = new LookupDataBase(this, lookupInterface);
            var query = lookupData.GetQuery();
            var countQuery = new CountQuery(query, name);
            querySet.AddQuery(countQuery, name);
        }

        public int GetCount(DataProcessResult countResult, string name)
        {
            if (countResult.ResultCode == GetDataResultCodes.Success)
            {
                var count = countResult.DataSet.Tables[name].Rows[0]
                    .GetRowValue(name)
                    .ToInt();
                return count;
            }
            return 0;
        }

        public void ShowAddOnTheFlyWindow(PrimaryKeyValue selectedPrimaryKeyValue = null, object addViewParameter = null, object ownerWindow = null)
        {
            var addNewRecordProcessor =
                new AddOnTheFlyProcessor(this)
                {
                    AddViewParameter = addViewParameter,
                    OwnerWindow = ownerWindow,
                    SelectedPrimaryKeyValue = selectedPrimaryKeyValue
                };

            addNewRecordProcessor.ShowAddOnTheFlyWindow();
        }

    }
}
