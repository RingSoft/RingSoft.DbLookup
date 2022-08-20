using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using RingSoft.DataEntryControls.Engine;
using RingSoft.DbLookup;
using RingSoft.DbLookup.AdvancedFind;
using RingSoft.DbLookup.AutoFill;
using RingSoft.DbLookup.DataProcessor;
using RingSoft.DbLookup.Lookup;
using RingSoft.DbLookup.ModelDefinition;
using RingSoft.DbLookup.ModelDefinition.FieldDefinitions;
using RingSoft.DbLookup.QueryBuilder;
using RingSoft.DbLookup.TableProcessing;

namespace RingSoft.DbMaintenance
{
    public enum TreeViewType
    {
        Field = 0,
        AdvancedFind = 1,
        Formula = 2,
        ForeignTable = 3
    }

    public class ProcessIncludeResult
    {
        public LookupJoin LookupJoin { get; set; }
        public LookupColumnDefinitionBase ColumnDefinition { get; set; }
    }
    public class AdvancedFindInput
    {
        public object InputParameter { get; set; }
        public TableDefinitionBase LockTable { get; set; }
        public LookupDefinitionBase LookupDefinition { get; set; }
        public double LookupWidth { get; set; }
    }


    public class TreeViewItem : INotifyPropertyChanged
    {
        public string Name { get; set; }
        public TreeViewType Type { get; set; }
        public FieldDefinition FieldDefinition { get; set; }
        public ObservableCollection<TreeViewItem> Items { get; set; } = new ObservableCollection<TreeViewItem>();
        public ForeignKeyDefinition ParentJoin { get; set; }
        public AdvancedFindViewModel ViewModel { get; set; }
        public LookupJoin Include { get; set; }
        public TreeViewItem Parent { get; set; }

        private bool _isSelected;
        public bool IsSelected
        {
            get { return _isSelected; }
            set
            {
                if (_isSelected != value)
                {
                    _isSelected = value;
                    OnPropertyChanged("IsSelected");
                    if (_isSelected)
                    {
                        SelectedTreeItem = this;
                    }
                }
            }
        }

        private TreeViewItem _selectedTreeItem;

        public TreeViewItem SelectedTreeItem
        {
            get => _selectedTreeItem;
            set
            {
                if (_selectedTreeItem == value)
                {
                    return;
                }
                _selectedTreeItem = value;
                ViewModel.OnTreeViewItemSelected(_selectedTreeItem);
            }
        }

        public override string ToString()
        {
            return Name;
        }

        public event PropertyChangedEventHandler PropertyChanged;

        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }

    public class TreeViewItems : List<TreeViewItem>
    {
        public string Name { get; set; }
        public TreeViewType Type { get; set; }
        public FieldDefinition FieldDefinition { get; set; }
        public ObservableCollection<TreeViewItem> Items { get; set; } = new ObservableCollection<TreeViewItem>();
    }

    public class AdvancedFindViewModel : DbMaintenanceViewModel<AdvancedFind>
    {
        public override TableDefinition<AdvancedFind> TableDefinition =>
            SystemGlobals.AdvancedFindLookupContext.AdvancedFinds;

        private int _advancedFindId;

        public int AdvancedFindId
        {
            get => _advancedFindId;
            set
            {
                if (_advancedFindId == value)
                {
                    return;
                }
                _advancedFindId = value;
                OnPropertyChanged();
            }
        }

        private TextComboBoxControlSetup _tableComboBoxSetup;

        public TextComboBoxControlSetup TableComboBoxSetup
        {
            get => _tableComboBoxSetup;
            set
            {
                if (_tableComboBoxSetup == value)
                {
                    return;
                }
                _tableComboBoxSetup = value;
                OnPropertyChanged();
            }
        }

        private int _tableIndex;

        public int TableIndex
        {
            get => _tableIndex;
            set
            {
                if (_tableIndex == value)
                {
                    return;
                }
                _tableIndex = value;
                OnPropertyChanged();
            }
        }

        private TextComboBoxItem _selectedTableBoxItem;

        public TextComboBoxItem SelectedTableBoxItem
        {
            get => _selectedTableBoxItem;
            set
            {
                if (_selectedTableBoxItem == value)
                {
                    return;
                }
                _selectedTableBoxItem = value;
                if (_selectedTableBoxItem != null)
                {
                    LoadTree();
                }
                OnPropertyChanged();
            }
        }

        private ObservableCollection<TreeViewItem> _treeRoot;

        public ObservableCollection<TreeViewItem> TreeRoot
        {
            get => _treeRoot;
            set
            {
                if (_treeRoot == value)
                {
                    return;
                }
                _treeRoot = value;
                OnPropertyChanged();
            }
        }

        private LookupDefinitionBase _lookupDefinition;

        public LookupDefinitionBase LookupDefinition
        {
            get => _lookupDefinition;
            set
            {
                if (_lookupDefinition == value)
                    return;

                _lookupDefinition = value;
                OnPropertyChanged();
            }
        }

        private LookupCommand _lookupCommand;

        public LookupCommand LookupCommand
        {
            get => _lookupCommand;
            set
            {
                if (value == _lookupCommand)
                    return;

                _lookupCommand = value;
                OnPropertyChanged();
            }
        }

        private AdvancedFindColumnsManager _columnsManager;

        public AdvancedFindColumnsManager ColumnsManager
        {
            get => _columnsManager;
            set
            {
                if (_columnsManager == value)
                {
                    return;
                }
                _columnsManager = value;
                OnPropertyChanged();
            }
        }


        public RelayCommand AddColumnCommand { get; set; }

        public RelayCommand AddFilterCommand { get; set; }

        public TreeViewItem SelectedTreeViewItem { get; set; }

        private List<LookupJoin> _includes = new List<LookupJoin>();
        private AdvancedFindInput _input = null;

        protected override void Initialize()
        {
            if (LookupAddViewArgs != null)
            {
                if (LookupAddViewArgs.InputParameter is AdvancedFindInput advancedFindInput)
                {
                    _input = advancedFindInput;
                }
            }
            AddColumnCommand = new RelayCommand(AddColumn);

            TableComboBoxSetup = new TextComboBoxControlSetup();
            var index = 0;
            foreach (var contextTableDefinition in SystemGlobals.AdvancedFindLookupContext.AdvancedFinds.Context
                         .TableDefinitions.OrderBy(p => p.Description))
            {
                if (!contextTableDefinition.Description.IsNullOrEmpty())
                {
                    TableComboBoxSetup.Items.Add(new TextComboBoxItem() { NumericValue = index, TextValue = contextTableDefinition.Description });
                }
                index++;
            }

            ColumnsManager = new AdvancedFindColumnsManager(this);

            if (_input != null)
            {
                TableIndex = TableComboBoxSetup.Items.FindIndex(p => p.TextValue == _input.LockTable.Description);
                ViewLookupDefinition.FilterDefinition.AddFixedFilter(
                    SystemGlobals.AdvancedFindLookupContext.AdvancedFinds.GetFieldDefinition(p =>
                        p.Table), Conditions.Equals,
                    _input.LockTable.EntityName);
                LoadFromLookupDefinition(_input.LookupDefinition);
            }
            

            base.Initialize();
        }

        protected override AdvancedFind PopulatePrimaryKeyControls(AdvancedFind newEntity, PrimaryKeyValue primaryKeyValue)
        {
            var advancedFind = SystemGlobals.AdvancedFindDbProcessor.GetAdvancedFind(newEntity.Id);
            AdvancedFindId = advancedFind.Id;

            KeyAutoFillValue = new AutoFillValue(primaryKeyValue, advancedFind.Name);

            return advancedFind;
        }

        protected override void LoadFromEntity(AdvancedFind entity)
        {
            AdvancedFindId = entity.Id;
            
            var tableDefinition =
                TableDefinition.Context.TableDefinitions.FirstOrDefault(p => p.EntityName == entity.Table);
            var comboItem = TableComboBoxSetup.Items.FirstOrDefault(p => p.TextValue == tableDefinition.Description);
            TableIndex = TableComboBoxSetup.Items.IndexOf(comboItem);

            CreateLookupDefinition();
            ColumnsManager.LoadGrid(entity.Columns);

            LookupCommand = GetLookupCommand(LookupCommands.Reset);
        }

        private void LoadTree()
        {
            CreateLookupDefinition();
            var treeItems = new ObservableCollection<TreeViewItem>();
            
            if (TableIndex >= 0)
            {
                var table = SystemGlobals.AdvancedFindLookupContext.AdvancedFinds.Context.TableDefinitions
                    .FirstOrDefault(
                        f => f.Description == TableComboBoxSetup.Items[TableIndex].TextValue);
                var fields = table.FieldDefinitions;

                foreach (var field in fields.OrderBy(p => p.Description))
                {
                    var treeRoot = new TreeViewItem();
                    treeRoot.Name = field.Description;
                    treeRoot.Type = TreeViewType.Field;
                    treeRoot.FieldDefinition = field;
                    treeRoot.ViewModel = this;
                    treeItems.Add(treeRoot);
                    if (field.ParentJoinForeignKeyDefinition != null && field.ParentJoinForeignKeyDefinition.PrimaryTable != null)
                        AddTreeItem(field.ParentJoinForeignKeyDefinition.PrimaryTable, treeRoot.Items, field.ParentJoinForeignKeyDefinition, treeRoot);
                }

                LookupCommand = GetLookupCommand(LookupCommands.Clear);

            }
            TreeRoot = treeItems;
        }

        private void AddTreeItem(TableDefinitionBase table,
            ObservableCollection<TreeViewItem> treeItems,
            ForeignKeyDefinition join, TreeViewItem parent)
        {
            foreach (var tableFieldDefinition in table.FieldDefinitions.OrderBy(p => p.Description))
            {
                var treeChildItem = new TreeViewItem();
                treeChildItem.Name = tableFieldDefinition.Description;
                treeChildItem.Type = TreeViewType.Field;
                treeChildItem.FieldDefinition = tableFieldDefinition;
                treeChildItem.ViewModel = this;
                treeChildItem.Parent = parent;
                if (tableFieldDefinition.ParentJoinForeignKeyDefinition != null)
                {
                    join = tableFieldDefinition.ParentJoinForeignKeyDefinition;
                }
                treeChildItem.ParentJoin = join;
                treeItems.Add(treeChildItem);

                if (tableFieldDefinition.ParentJoinForeignKeyDefinition != null &&
                    tableFieldDefinition.ParentJoinForeignKeyDefinition.PrimaryTable != null)
                {
                    //treeChildItem.PrimaryFieldDefinition = tableFieldDefinition.ParentJoinForeignKeyDefinition
                    //    .FieldJoins[0].PrimaryField;

                    if (tableFieldDefinition.AllowRecursion)
                        AddTreeItem(tableFieldDefinition.ParentJoinForeignKeyDefinition.PrimaryTable,
                            treeChildItem.Items, join, treeChildItem);
                }
            }
        }

        public void OnTreeViewItemSelected(TreeViewItem treeViewItem)
        {
            SelectedTreeViewItem = treeViewItem;
        }

        protected override bool ValidateEntity(AdvancedFind entity)
        {
            return base.ValidateEntity(entity);
        }

        protected override AdvancedFind GetEntityData()
        {
            var advancedFind = new AdvancedFind();
            advancedFind.Id = AdvancedFindId;
            if (KeyAutoFillValue != null)
            {
                advancedFind.Name = KeyAutoFillValue.Text;
            }
            
            advancedFind.Table = TableDefinition.Context.TableDefinitions
                .FirstOrDefault(p => p.Description == TableComboBoxSetup.Items[TableIndex].TextValue)
                ?.EntityName;
            return advancedFind;
        }

        protected override void ClearData()
        {
            AdvancedFindId = 0;
            if (_input == null || _input.LockTable == null)
            {
                TableIndex = -1;
                SelectedTableBoxItem = null;
                SelectedTreeViewItem = null;
                TreeRoot?.Clear();
            }

            CreateLookupDefinition();

            if (LookupDefinition != null)
            {
                LookupCommand = GetLookupCommand(LookupCommands.Reset);
            }
            
            ColumnsManager.SetupForNewRecord();


            //LoadTree();
        }

        private void CreateLookupDefinition()
        {
            if (SelectedTableBoxItem != null)
            {
                var tableDefinition =
                    SystemGlobals.AdvancedFindLookupContext.AdvancedFinds.Context.TableDefinitions.FirstOrDefault(p =>
                        p.Description == SelectedTableBoxItem.TextValue);

                LookupDefinition = new LookupDefinitionBase(tableDefinition);
            }
            else
            {
                LookupDefinition = null;
            }
            _includes.Clear();
        }

        protected override bool SaveEntity(AdvancedFind entity)
        {
            return SystemGlobals.AdvancedFindDbProcessor.SaveAdvancedFind(entity, ColumnsManager.GetEntityList(),
                new List<AdvancedFindFilter>());
        }

        protected override bool DeleteEntity()
        {
            return SystemGlobals.AdvancedFindDbProcessor.DeleteAdvancedFind(AdvancedFindId);
        }

        private void AddColumn()
        {
            var column = MakeIncludes(SelectedTreeViewItem, SelectedTreeViewItem.Name);
            ColumnsManager.LoadFromColumnDefinition(column);

            LookupCommand = GetLookupCommand(LookupCommands.Reset, null, _input?.InputParameter);
        }

        public LookupColumnDefinitionBase MakeIncludes(TreeViewItem selectedItem, string columnCaption, bool createColumn = true)
        {
            LookupColumnDefinitionBase column = null;
            var childNodes = new List<TreeViewItem>();

            LookupJoin includeJoin = null;
            var parentTreeItem = selectedItem;
            while (parentTreeItem.Parent != null)
            {
                parentTreeItem = parentTreeItem.Parent;
                childNodes.Insert(0, parentTreeItem);
            }

            if (childNodes.Any() == false)
            {
                if (createColumn)
                {
                    var processResult = SelectColumnDescription(selectedItem, selectedItem, null, columnCaption);
                    includeJoin = ProcessInclude(includeJoin, processResult.LookupJoin);
                    column = processResult.ColumnDefinition;
                }
            }

            foreach (var child in childNodes)
            {
                if (childNodes.IndexOf(child) == 0)
                {
                    var newInclude = LookupDefinition.Include(child.FieldDefinition);
                    includeJoin = ProcessInclude(includeJoin, newInclude);
                }

                if (childNodes.IndexOf(child) == childNodes.Count - 1)
                {
                    if (childNodes.Count > 1)
                    {
                        var newInclude = includeJoin.Include(child.FieldDefinition);
                        includeJoin = ProcessInclude(includeJoin, newInclude);
                    }

                    if (createColumn)
                    {
                        var processResult = SelectColumnDescription(selectedItem, child, includeJoin, columnCaption);
                        includeJoin = ProcessInclude(includeJoin, processResult.LookupJoin);
                        column = processResult.ColumnDefinition;
                    }
                }
                else if (childNodes.IndexOf(child) != 0)
                {
                    var newInclude = includeJoin.Include(child.FieldDefinition);
                    includeJoin = ProcessInclude(includeJoin, newInclude);
                }
            }

            return column;
        }

        private LookupJoin ProcessInclude(LookupJoin includeJoin, LookupJoin newInclude)
        {
            if (newInclude != null)
            {
                if (_includes.FirstOrDefault(p => p.JoinDefinition.Alias == newInclude.JoinDefinition.Alias) == null)
                {
                    _includes.Add(newInclude);
                    includeJoin = newInclude;
                }
                else
                {
                    includeJoin = _includes.FirstOrDefault(p =>
                        p.JoinDefinition.Alias == newInclude.JoinDefinition.Alias);
                }
            }

            return includeJoin;
        }

        private ProcessIncludeResult SelectColumnDescription(TreeViewItem selectedTreeViewItem, TreeViewItem child, LookupJoin includeJoin, string caption)
        {
            LookupColumnDefinitionBase column = null;
            if (selectedTreeViewItem.FieldDefinition.ParentJoinForeignKeyDefinition != null)
            {
                var textField = selectedTreeViewItem.FieldDefinition.ParentJoinForeignKeyDefinition
                    .FieldJoins[0].ForeignField;

                if (includeJoin != null)
                    includeJoin = includeJoin.Include(textField);
                else
                {
                    includeJoin = LookupDefinition.Include(textField);
                }

                if (selectedTreeViewItem.FieldDefinition.ParentJoinForeignKeyDefinition.FieldJoins[0]
                        .PrimaryField.TableDefinition.LookupDefinition
                        .InitialSortColumnDefinition is LookupFieldColumnDefinition lookupFieldColumn)
                {
                    textField = lookupFieldColumn.FieldDefinition;
                    column = includeJoin.AddVisibleColumnDefinition(caption, textField, 20);
                }
                else if (selectedTreeViewItem.FieldDefinition.ParentJoinForeignKeyDefinition.FieldJoins[0]
                             .PrimaryField.TableDefinition.LookupDefinition
                             .InitialSortColumnDefinition is LookupFormulaColumnDefinition lookupFormulaColumn)
                {
                    lookupFormulaColumn.JoinQueryTableAlias = includeJoin.JoinDefinition.Alias;
                    var formula = lookupFormulaColumn.Formula;
                    column = includeJoin.AddVisibleColumnDefinition(caption,
                        formula, 20, lookupFormulaColumn.DataType);
                }
            }
            else
            {
                if (includeJoin != null)
                {
                    column = includeJoin.AddVisibleColumnDefinition(caption,
                        selectedTreeViewItem.FieldDefinition,
                        20);
                }
                else
                {
                    if (child.FieldDefinition.ParentJoinForeignKeyDefinition != null)
                    {
                        includeJoin = LookupDefinition.Include(child.FieldDefinition.ParentJoinForeignKeyDefinition
                            .FieldJoins[0].ForeignField);
                        column = includeJoin.AddVisibleColumnDefinition(caption, selectedTreeViewItem.FieldDefinition, 20);
                    }
                    else
                    {
                        column = LookupDefinition.AddVisibleColumnDefinition(caption, child.FieldDefinition, 20, "");
                    }
                }
            }

            var processResult = new ProcessIncludeResult
            {
                ColumnDefinition = column,
                LookupJoin = includeJoin
            };
            return processResult;
        }

        public void LoadFromLookupDefinition(LookupDefinitionBase lookupDefinition)
        {
            var parentObjects = new List<IJoinParent>();
            foreach (var visibleColumn in lookupDefinition.VisibleColumns)
            {
                var parent = visibleColumn.ParentObject;
                var lookupFieldColumn = visibleColumn as LookupFieldColumnDefinition;
                var lookupFormulaColumn = visibleColumn as LookupFormulaColumnDefinition;

                if (parent == null)
                {
                    if (lookupFieldColumn != null)
                    {
                        LookupDefinition.AddVisibleColumnDefinition(visibleColumn.Caption,
                            lookupFieldColumn.FieldDefinition, visibleColumn.PercentWidth, "");
                    }
                    else if (lookupFormulaColumn != null)
                    {
                        LookupDefinition.AddVisibleColumnDefinition(visibleColumn.Caption, lookupFormulaColumn.Formula,
                            visibleColumn.PercentWidth, lookupFormulaColumn.DataType, lookupFormulaColumn.JoinQueryTableAlias);
                    }
                }
                else
                {
                    while (parent != null)
                    {
                        parentObjects.Insert(0, parent);
                        parent = parent.ParentObject;
                    }

                    LookupJoin include = null;
                    var index = 0;
                    foreach (var parentObject in parentObjects)
                    {
                        if (index == 0)
                        {
                            include = LookupDefinition.MakeInclude(LookupDefinition,
                                visibleColumn.ParentField);
                        }
                        if (index == parentObjects.Count - 1)
                        {
                            if (lookupFieldColumn != null)
                            {
                                include.AddVisibleColumnDefinitionField(visibleColumn.Caption,
                                    lookupFieldColumn.FieldDefinition, visibleColumn.PercentWidth);
                            }
                            else if (lookupFormulaColumn != null)
                            {
                                include.AddVisibleColumnDefinition(visibleColumn.Caption, lookupFormulaColumn.Formula,
                                    visibleColumn.PercentWidth, lookupFormulaColumn.DataType);
                            }

                        }
                        else if (index > 0)
                        {
                            if (include == null)
                            {
                                include = parentObject.MakeInclude(LookupDefinition);
                            }
                            else
                            {
                                include = include.MakeInclude(LookupDefinition);
                            }
                        }

                        index++;
                    }
                }
            }

            foreach (var fixedFilter in lookupDefinition.FilterDefinition.FixedFilters)
            {
                if (fixedFilter is FieldFilterDefinition fieldFilter)
                {
                    switch (fieldFilter.FieldDefinition.FieldDataType)
                    {
                        case FieldDataTypes.String:
                            if (fieldFilter.FieldDefinition is StringFieldDefinition stringField)
                                LookupDefinition.FilterDefinition.AddFixedFilter(stringField, fieldFilter.Condition,
                                    fieldFilter.Value);
                            break;
                        case FieldDataTypes.Integer:
                            if (fieldFilter.FieldDefinition is IntegerFieldDefinition integerField)
                                LookupDefinition.FilterDefinition.AddFixedFilter(integerField, fieldFilter.Condition,
                                    fieldFilter.Value.ToInt());
                            break;
                        case FieldDataTypes.Decimal:
                        case FieldDataTypes.DateTime:
                        case FieldDataTypes.Bool:
                            break;
                        default:
                            throw new ArgumentOutOfRangeException();
                    }
                    //LookupDefinition.FilterDefinition.AddFixedFilter(fieldFilter.FieldDefinition, fieldFilter.Condition,
                    //value);
                }
            }
            ColumnsManager.LoadFromLookupDefinition(LookupDefinition);
            LookupCommand = GetLookupCommand(LookupCommands.Reset, null, _input?.InputParameter);
        }

    }
}
