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
using RingSoft.DbLookup.AutoFill;
using RingSoft.DbLookup.DataProcessor;

namespace RingSoft.DbLookup.Lookup
{
    public class LookupWindowReturnArgs
    {
        public LookupDataMauiBase LookupData { get; internal set; }
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

                if (value is LookupFormulaColumnDefinition)
                {
                    var message = "Initial sort column cannot be a formula";
                    throw new ArgumentException( message );
                }


                _initialSortLookupColumnDefinition = value;
                InitialOrderByColumn =value;
            }
        }

        /// <summary>
        /// Gets and sets the initial type of the order by.
        /// </summary>
        /// <value>
        /// The initial type of the order by.
        /// </value>
        public OrderByTypes InitialOrderByType { get; set; }

        public LookupColumnDefinitionBase InitialOrderByColumn { get; set; }

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

        public event EventHandler<LookupWindowReturnArgs> WindowClosed;

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
            //tableDefinition.Context.Initialize();
            TableDefinition = tableDefinition;
            FilterDefinition = new TableFilterDefinitionBase(tableDefinition);
        }

        public LookupDefinitionBase(int advancedFindId, LookupRefresher lookupRefresher)
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

            lookupRefresher.LoadFromAdvFind(Entity);
            //if (Entity.RedAlert != null) lookupRefresher.RedAlert = Entity.RedAlert.Value;
            //if (Entity.Disabled != null) lookupRefresher.Disabled = Entity.Disabled.Value;
            //if (Entity.RefreshCondition != null)
            //    lookupRefresher.RefreshCondition = (Conditions)Entity.RefreshCondition.Value;
            //if (Entity.RefreshRate != null)
            //{
            //    lookupRefresher.RefreshRate = (RefreshRate)Entity.RefreshRate.Value;
            //}
            //else
            //{
            //    lookupRefresher.RefreshRate = RefreshRate.None;
            //}
            //if (Entity.RefreshValue != null) lookupRefresher.RefreshValue = Entity.RefreshValue.Value;
            //if (Entity.YellowAlert != null) lookupRefresher.YellowAlert = Entity.YellowAlert.Value;
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
            var initialOrderByIndex = source.VisibleColumns.ToList().IndexOf(source.InitialOrderByColumn);
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
            InitialOrderByColumn = VisibleColumns[initialOrderByIndex];  
            FromFormula = source.FromFormula;
            ReadOnlyMode = source.ReadOnlyMode;
            ParentObject = source.ParentObject;
            ChildField = source.ChildField;
            AllowAddOnTheFly = source.AllowAddOnTheFly;
            AdvancedFindTree = source.AdvancedFindTree;
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
                                var newColumn = AddHiddenColumn(formulaColumn.FormulaObject, formulaColumn.DataType);
                                newColumn.CopyFrom(columnType);
                            }
                            else
                            {
                                var newColumn = AddVisibleColumnDefinition(formulaColumn.Caption, formulaColumn.FormulaObject,
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

        internal LookupFieldColumnDefinition AddHiddenColumn(FieldDefinition fieldDefinition, string alias = "")
        {
            var isPrimaryKey = fieldDefinition.TableDefinition.PrimaryKeyFields.Contains(fieldDefinition);
            if (!isPrimaryKey)
            {
                ValidateNonPrimaryKeyDistinctColumns();
            }

            var columnDefinition = new LookupFieldColumnDefinition(fieldDefinition)
            {
                LookupDefinition = this,
                JoinQueryTableAlias = alias
            };

            _hiddenColumns.Add(columnDefinition);
            return columnDefinition;
        }

        internal LookupFormulaColumnDefinition AddHiddenColumn(ILookupFormula lookupFormula, FieldDataTypes dataType, string alias = "")
        {
            ValidateNonPrimaryKeyDistinctColumns();

            var columnDefinition = new LookupFormulaColumnDefinition(lookupFormula, dataType)
            {
                LookupDefinition = this,
                JoinQueryTableAlias = alias
            };

            _hiddenColumns.Add(columnDefinition);
            return columnDefinition;
        }

        public LookupFieldColumnDefinition AddVisibleColumnDefinition(string caption, FieldDefinition fieldDefinition
            , double percentWidth, string alias)
        {
            if (VisibleColumns.Count > 1 && TableDefinition.TableName == "AdvancedFinds")
            {
                
            }
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
                JoinQueryTableAlias = alias,
            };

            //ProcessVisibleColumnDefinition(column);
            //_visibleColumns.Add(column);
            AddVisibleColumnDefinition(column);
            
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

        public LookupFormulaColumnDefinition AddVisibleColumnDefinition(string caption, ILookupFormula lookupFormula,
            double percentWidth, FieldDataTypes dataType, string alias, bool allowNulls = false)
        {
            ValidateNonPrimaryKeyDistinctColumns();

            var column = new LookupFormulaColumnDefinition(lookupFormula, dataType)
            {
                Caption = caption,
                LookupDefinition = this,
                PercentWidth = percentWidth,
                AllowNulls = allowNulls,
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
            AddVisibleColumnDefinition(column);
            return column;
        }

        internal void AddVisibleColumnDefinition(LookupColumnDefinitionBase lookupColumn)
        {
            ProcessVisibleColumnDefinition(lookupColumn);
            if (lookupColumn.ColumnIndexToAdd >= 0)
            {
                _visibleColumns.Insert(lookupColumn.ColumnIndexToAdd, lookupColumn);
            }
            else
            {
                _visibleColumns.Add(lookupColumn);
            }
        }

        internal void ProcessVisibleColumnDefinition(LookupColumnDefinitionBase columnDefinition)
        {
            if (InitialSortColumnDefinition == null || columnDefinition.ColumnIndexToAdd == 0)
            {
                InitialSortColumnDefinition = columnDefinition;
            }
        }

        internal TableFieldJoinDefinition AddJoin(TableFieldJoinDefinition lookupFieldJoin)
        {
            if (_joinsList.All(p => p.Alias != lookupFieldJoin.Alias))
            {
                if (!_joinsList.Any(p => p.ForeignKeyDefinition.IsEqualTo(lookupFieldJoin.ForeignKeyDefinition)))
                {
                    _joinsList.Add(lookupFieldJoin);
                    return lookupFieldJoin;
                }
            }

            return null;
        }

        public TableFieldJoinDefinition AddCopyJoin(TableFieldJoinDefinition lookupFieldJoin)
        {
            var result = new TableFieldJoinDefinition();
            result.CopyFrom(lookupFieldJoin);
            AddJoin(result);
            return result;
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
            if (lookupJoin.JoinDefinition != null) lookupJoin.JoinDefinition.ParentObject = null;
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
            LookupColumnDefinitionBase lookupColumn = null;
            if (entity.Formula.IsNullOrEmpty())
            {
                lookupColumn = new LookupFieldColumnDefinition();
            }
            else
            {
                lookupColumn = new LookupFormulaColumnDefinition();
            }
            lookupColumn.LoadFromEntity(entity, this);
            return lookupColumn;
            //var tableDefinition =
            //    TableDefinition.Context.TableDefinitions.FirstOrDefault(p =>
            //        p.EntityName == entity.TableName);
            
            //FieldDefinition fieldDefinition = null;
            //var fieldDescription = string.Empty;
            //if (!entity.FieldName.IsNullOrEmpty())
            //{
            //    fieldDefinition =
            //        tableDefinition.FieldDefinitions.FirstOrDefault(p => p.FieldName == entity.FieldName);
            //    fieldDescription = fieldDefinition.Description;
            //}
            //else if (!entity.Formula.IsNullOrEmpty())
            //{
            //    fieldDescription = "<Formula>";
            //}

            //TableDefinitionBase primaryTable = null;
            //FieldDefinition primaryField = null;
            //if (!entity.PrimaryTableName.IsNullOrEmpty() && !entity.PrimaryFieldName.IsNullOrEmpty())
            //{
            //    primaryTable =
            //        TableDefinition.Context.TableDefinitions.FirstOrDefault(p =>
            //            p.EntityName == entity.PrimaryTableName);

            //    primaryField =
            //        primaryTable.FieldDefinitions.FirstOrDefault(p => p.FieldName == entity.PrimaryFieldName);

            //    if (fieldDefinition == null)
            //    {
            //        tableDefinition = primaryTable;
            //        fieldDefinition = primaryField;
            //    }
            //}

            //var fieldToProcess = fieldDefinition;
            //if (primaryField != null)
            //{
            //    fieldToProcess = primaryField;
            //}

            //TreeViewItem foundTreeViewItem = null;
            //if (!entity.Path.IsNullOrEmpty())
            //{
            //    var type = TreeViewType.Field;
            //    if (!entity.Formula.IsNullOrEmpty() && fieldToProcess == null)
            //    {
            //        type = TreeViewType.Formula;
            //    }
            //    foundTreeViewItem = AdvancedFindTree.ProcessFoundTreeViewItem(entity.Path, type);
            //}
            //else
            //{
            //    foundTreeViewItem = AdvancedFindTree.ProcessFoundTreeViewItem(entity.Formula, fieldToProcess,
            //        (FieldDataTypes)entity.FieldDataType, (DecimalEditFormatTypes)entity.DecimalFormatType);
            //}

            //if (!entity.Formula.IsNullOrEmpty())
            //{
            //    foundTreeViewItem.FormulaData = new TreeViewFormulaData
            //    {
            //        Formula = entity.Formula,
            //        DataType = (FieldDataTypes)entity.FieldDataType,
            //        DecimalFormatType = (DecimalEditFormatTypes)entity.DecimalFormatType,
            //    };

            //}
            //var result = AdvancedFindTree.MakeIncludes(foundTreeViewItem, entity.Caption).ColumnDefinition;
            //result.FieldDescription = fieldDescription;
            //result.PercentWidth = entity.PercentWidth * 100;

            //if (result is LookupFormulaColumnDefinition lookupFormulaColumn)
            //{
            //    lookupFormulaColumn.HasDataType((FieldDataTypes)entity.FieldDataType);
            //    if (entity.DecimalFormatType > 0)
            //    {
            //        lookupFormulaColumn.DecimalFieldType = (DecimalFieldTypes)entity.DecimalFormatType;
            //    }
            //}
            //return result;
        }

        public FilterItemDefinition LoadFromAdvFindFilter(AdvancedFindFilter entity, bool addFilterToLookup = true,
            TreeViewItem parentTreeItem = null, string path = "")
        {
            FilterItemDefinition result = null;

            if (entity.SearchForAdvancedFindId.HasValue)
            {
                var advancedFindFilter = new AdvancedFindFilterDefinition(FilterDefinition);
                result = advancedFindFilter;
            }
            else if (entity.Formula.IsNullOrEmpty())
            {
                var fieldFilter = new FieldFilterDefinition(FilterDefinition);
                var newPath = path + entity.Path;
                if (!newPath.IsNullOrEmpty())
                {
                    var foundItem = AdvancedFindTree.ProcessFoundTreeViewItem(newPath, TreeViewType.Field);
                    if (foundItem != null)
                    {
                        fieldFilter.FieldDefinition = foundItem.FieldDefinition;
                        var lookupJoin = AdvancedFindTree.MakeIncludes(foundItem).LookupJoin;
                        fieldFilter.JoinDefinition = lookupJoin?.JoinDefinition;
                    }
                }
                result = fieldFilter;
            }
            else
            {
                var formulaFilter = new FormulaFilterDefinition(FilterDefinition);
                result = formulaFilter;
            }

            if (result != null && result.LoadFromEntity(entity, this, path))
            {
                if (addFilterToLookup)
                {
                    FilterDefinition.AddUserFilter(result);
                }
            }
            //if (entity.SearchForAdvancedFindId != null)
            //{
            //    var advancedFindFilterDefinition = new AdvancedFindFilterDefinition(FilterDefinition);
            //    advancedFindFilterDefinition.LoadFromEntity(entity, this);
            //    result.FilterItemDefinition = advancedFindFilterDefinition;
            //    return result;
            //}

            //FilterItemDefinition filterItemDefinition = null;
            //if (entity.SearchForAdvancedFindId != null)
            //{
            //    if (addFilterToLookup)
            //    {
            //        filterItemDefinition =
            //            FilterDefinition.AddUserFilter(entity.SearchForAdvancedFindId.Value, this, entity.Path);
            //        var afTableDefinition =
            //            GetTableFieldForFilter(entity, out var afFieldDefinition, out var afFilterField);
            //        TreeViewItem afItem = null;
            //        if (afFieldDefinition != null)
            //        {
            //            if (entity.Path.IsNullOrEmpty())
            //            {
            //                afItem = AdvancedFindTree.ProcessFoundTreeViewItem(string.Empty, afFieldDefinition);
            //            }
            //            else
            //            {
            //                afItem = AdvancedFindTree.ProcessFoundTreeViewItem(entity.Path, TreeViewType.AdvancedFind);
            //            }
            //        }
            //        SetFilterProperties(entity, filterItemDefinition, afItem, true);
            //        //filterItemDefinition.TableDescription = SystemGlobals.AdvancedFindDbProcessor
            //        //    .GetAdvancedFind(entity.AdvancedFindId).Table;
            //        result.FilterItemDefinition = filterItemDefinition;
            //    }
            //    return result;
            //}
            //var tableDefinition = GetTableFieldForFilter(entity, out var fieldDefinition, out var filterField);

            //if (tableDefinition == null && entity.Formula.IsNullOrEmpty())
            //{
            //    var message = $"Advanced Find Id {entity.AdvancedFindId} Filter Id {entity.FilterId} is corrupt.";
            //    throw new Exception(message);
            //}
            //TreeViewItem foundTreeViewItem = null;
            //var type = TreeViewType.Field;
            //if (!entity.Formula.IsNullOrEmpty())
            //{
            //    type = TreeViewType.Formula;
            //}
            //if (entity.Path.IsNullOrEmpty())
            //{
            //    foundTreeViewItem = AdvancedFindTree.ProcessFoundTreeViewItem(entity.Formula, fieldDefinition);
            //}
            //else
            //{
            //    foundTreeViewItem = AdvancedFindTree.ProcessFoundTreeViewItem(entity.Path, type, parentTreeItem);
            //}

            //var includeResult = AdvancedFindTree.MakeIncludes(foundTreeViewItem, string.Empty, false);
            //var formula = entity.Formula;
            //var lookupField = fieldDefinition;
            //var condition = (Conditions)entity.Operand;

            //if (fieldDefinition != null)
            //{
            //    if (fieldDefinition.ParentJoinForeignKeyDefinition != null)
            //    {
            //        switch (condition)
            //        {
            //            case Conditions.Equals:
            //            case Conditions.NotEquals:
            //            case Conditions.EqualsNull:
            //            case Conditions.NotEqualsNull:
            //                if (fieldDefinition.ParentJoinForeignKeyDefinition != null)
            //                {
            //                    lookupField = fieldDefinition.ParentJoinForeignKeyDefinition.FieldJoins[0].PrimaryField;
            //                }

            //                break;
            //            default:
            //                var lookupColumn = fieldDefinition.ParentJoinForeignKeyDefinition
            //                    .PrimaryTable.LookupDefinition.InitialSortColumnDefinition;
            //                if (lookupColumn is LookupFormulaColumnDefinition lookupFormulaColumn)
            //                {
            //                    formula = lookupFormulaColumn.OriginalFormula;
            //                }
            //                else if (lookupColumn is LookupFieldColumnDefinition lookupFieldColumn)
            //                {
            //                    lookupField = lookupFieldColumn.FieldDefinition;
            //                }
            //                break;
            //        }
            //    }
            //}

            //if (foundTreeViewItem != null && foundTreeViewItem.Parent == null)
            //{
            //    switch (condition)
            //    {
            //        case Conditions.EqualsNull:
            //        case Conditions.NotEqualsNull:
            //            includeResult.LookupJoin = null;
            //            lookupField = foundTreeViewItem.FieldDefinition;
            //            filterField = foundTreeViewItem.FieldDefinition;
            //            break;
            //    }
            //}

            //var searchValue = entity.SearchForValue;
            //if (foundTreeViewItem != null && foundTreeViewItem.FieldDefinition is DateFieldDefinition dateField)
            //{
            //    searchValue = ProcessSearchValue(searchValue, (DateFilterTypes)entity.DateFilterType);
            //}
            //if (addFilterToLookup)
            //{
            //    if (formula.IsNullOrEmpty())
            //    {
            //        filterItemDefinition = FilterDefinition.AddUserFilter(lookupField, (Conditions)entity.Operand,
            //            searchValue);
            //    }
            //    else
            //    {
            //        searchValue = ProcessSearchValue(searchValue, (DateFilterTypes)entity.DateFilterType);
            //        var alias = includeResult.LookupJoin?.JoinDefinition.Alias;
            //        if (alias.IsNullOrEmpty())
            //        {
            //            alias = TableDefinition.TableName;
            //        }
            //        filterItemDefinition = FilterDefinition.AddUserFilter(formula, (Conditions)entity.Operand,
            //            searchValue, alias, (FieldDataTypes)entity.FormulaDataType);
            //    }
            //}
            //else
            //{
            //    if (formula.IsNullOrEmpty())
            //    {
            //        filterItemDefinition = FilterDefinition.CreateFieldFilter(lookupField, (Conditions)entity.Operand,
            //            searchValue);
            //    }
            //    else
            //    {
            //        searchValue = ProcessSearchValue(searchValue, (DateFilterTypes)entity.DateFilterType);
            //        var alias = includeResult.LookupJoin?.JoinDefinition.Alias;
            //        if (alias.IsNullOrEmpty())
            //        {
            //            alias = TableDefinition.TableName;
            //        }
            //        filterItemDefinition = FilterDefinition.CreateFormulaFilter(formula, (FieldDataTypes)entity.FormulaDataType, (Conditions)entity.Operand,
            //            searchValue, alias);
            //    }
            //}

            //if (includeResult.LookupJoin != null)
            //{
            //    filterItemDefinition.JoinDefinition = includeResult.LookupJoin.JoinDefinition;
            //}

            //if (foundTreeViewItem.Parent != null)
            //{
            //    filterItemDefinition.TableDescription = foundTreeViewItem.Parent.Name;
            //    filterField = foundTreeViewItem.FieldDefinition;
            //}
            //else
            //{
            //    if (tableDefinition == null)
            //    {
            //        filterItemDefinition.TableDescription = TableDefinition.Description;
            //    }
            //    else
            //    {
            //        filterItemDefinition.TableDescription = tableDefinition.Description;
            //        if (filterItemDefinition is FormulaFilterDefinition formulaFilter)
            //        {
            //            filterItemDefinition.TableDescription = foundTreeViewItem.Name;
            //        }
            //    }
            //}

            //SetFilterProperties(entity, filterItemDefinition, foundTreeViewItem);
            //result.FilterItemDefinition = filterItemDefinition;
            //if (entity.Formula.IsNullOrEmpty())
            //{
            //    result.FieldDefinition = filterField;
            //}
            //else
            //{
            //    if (filterItemDefinition is FormulaFilterDefinition formulaFilter)
            //    {
            //        formulaFilter.ReportDescription = entity.FormulaDisplayValue + " Formula";
            //    }
            //}

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
                filterItemDefinition.ReportDescription = foundItem.Name;
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
                        if (filterItemDefinition is FormulaFilterDefinition formulaFilter)
                        {
                            if (!entity.Path.IsNullOrEmpty())
                            {
                                filterItemDefinition.TableDescription = foundItem.Name;
                            }
                        }
                        else
                        {
                            filterItemDefinition.TableDescription = TableDefinition.Description;
                        }
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

        

        public void ShowAddOnTheFlyWindow(PrimaryKeyValue selectedPrimaryKeyValue = null
            , object addViewParameter = null, object ownerWindow = null)
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

        public static string ProcessSearchValue(string searchValue, DateFilterTypes filterType)
        {
            var result = searchValue;
            var date = DateTime.Now;
            var searchUnits = 0;
            switch (filterType)
            {
                case DateFilterTypes.SpecificDate:
                    return result;
                default:
                    searchUnits = searchValue.ToInt();
                    break;
            }

            searchUnits = -searchUnits;
            switch (filterType)
            {
                case DateFilterTypes.SpecificDate:
                    break;
                case DateFilterTypes.Days:
                    date = DateTime.Today;
                    date = date.AddDays(searchUnits);
                    break;
                case DateFilterTypes.Weeks:
                    date = DateTime.Today;
                    date = date.AddDays(searchUnits * 7);
                    break;
                case DateFilterTypes.Months:
                    date = DateTime.Today;
                    date = date.AddMonths(searchUnits);
                    break;
                case DateFilterTypes.Years:
                    date = DateTime.Today;
                    date = date.AddYears(searchUnits);
                    break;
                case DateFilterTypes.Hours:
                    date = date.AddHours(searchUnits);
                    break;
                case DateFilterTypes.Minutes:
                    date = date.AddMinutes(searchUnits);
                    break;
                case DateFilterTypes.Seconds:
                    date = date.AddSeconds(searchUnits);
                    break;
                default:
                    throw new ArgumentOutOfRangeException(nameof(filterType), filterType, null);
            }
            result = date.ToString();
            return result;
        }

        public void AddAllFieldsAsHiddenColumns(bool copyData = false)
        {
            foreach (var fieldDefinition in TableDefinition.FieldDefinitions)
            {
                if (!copyData && fieldDefinition.SkipPrint)
                {
                    continue;
                }
                LookupColumnDefinitionBase column = null;
                if (fieldDefinition.ParentJoinForeignKeyDefinition != null
                    && fieldDefinition.ParentJoinForeignKeyDefinition.FieldJoins.Count == 1 && !copyData)
                {
                    var join = Joins.FirstOrDefault(p =>
                        p.ForeignKeyDefinition == fieldDefinition.ParentJoinForeignKeyDefinition);

                    if (join == null)
                    {
                        join = new TableFieldJoinDefinition
                        {

                            ForeignKeyDefinition = fieldDefinition.ParentJoinForeignKeyDefinition,
                            ParentObject = null,
                            JoinType = JoinTypes.InnerJoin
                        };
                        if (fieldDefinition.AllowNulls)
                        {
                            join.JoinType = JoinTypes.LeftOuterJoin;
                        }

                        AddJoin(join);
                    }
                    var lookupColumn = join.ForeignKeyDefinition.FieldJoins[0].PrimaryField.TableDefinition
                        .LookupDefinition.InitialSortColumnDefinition;

                    if (lookupColumn is LookupFieldColumnDefinition lookupFieldColumn)
                    {
                        column = AddHiddenColumn(lookupFieldColumn.FieldDefinition, join.Alias);
                    }
                    else if (lookupColumn is LookupFormulaColumnDefinition lookupFormulaColumn)
                    {
                        column = AddHiddenColumn(lookupFormulaColumn.FormulaObject, lookupFormulaColumn.DataType,
                            join.Alias);
                    }

                }
                else
                {
                    column = AddHiddenColumn(fieldDefinition);
                }

                if (column != null)
                {
                    column.Caption = fieldDefinition.Description;
                }
            }
        }

        public void FireCloseEvent(LookupDataMauiBase lookupData)
        {
            WindowClosed?.Invoke(this, new LookupWindowReturnArgs(){LookupData = lookupData});
        }

        public virtual LookupDataMauiBase GetLookupDataMaui(LookupDefinitionBase lookupDefinition, bool inputMode)
        {
            return null;
        }

        public virtual AutoFillDataMauiBase GetAutoFillDataMaui(AutoFillSetup setup, IAutoFillControl control)
        {
            return null;
        }

        public virtual AutoFillValue GetAutoFillValue(PrimaryKeyValue primaryKey)
        {
            return null;
        }

        public virtual SelectQueryMauiBase GetSelectQueryMaui()
        {
            return null;
        }
    }
}
