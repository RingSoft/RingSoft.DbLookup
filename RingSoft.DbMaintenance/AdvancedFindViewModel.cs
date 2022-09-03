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
    public interface IAdvancedFindView : IDbMaintenanceView
    {
        bool ShowFormulaEditor(TreeViewItem formulaTreeViewItem);

        bool ShowFromFormulaEditor(ref string fromFormula);

        AdvancedFilterReturn ShowAdvancedFilterWindow(TreeViewItem treeViewItem, LookupDefinitionBase lookupDefinition);

        bool NotifyFromFormulaExists { get; set; }

        void ApplyToLookup();
    }

    public enum TreeViewType
    {
        Field = 0,
        AdvancedFind = 1,
        Formula = 2,
        ForeignTable = 3
    }

    public class TreeViewFormulaData
    {
        public string Formula { get; set; }

        public FieldDataTypes DataType { get; set; }

        public DecimalEditFormatTypes DecimalFormatType { get; set; }
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
        public TreeViewFormulaData FormulaData { get; set; }

        private bool _isSelected;

        public bool IsSelected
        {
            get { return _isSelected; }
            set
            {
                if (_isSelected != value)
                {
                    _isSelected = value;
                    OnPropertyChanged();
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

        private AdvancedFindFiltersManager _filtersManager;

        public AdvancedFindFiltersManager FiltersManager
        {
            get => _filtersManager;
            set
            {
                if (_filtersManager == value)
                {
                    return;
                }

                _filtersManager = value;
                OnPropertyChanged();
            }
        }



        public RelayCommand AddColumnCommand { get; set; }

        public RelayCommand AddFilterCommand { get; set; }

        public RelayCommand FromFormulaCommand { get; set; }

        public RelayCommand ImportDefaultLookupCommand { get; set; }

        public RelayCommand ApplyToLookupCommand { get; set; }

        public TreeViewItem SelectedTreeViewItem { get; set; }

        public IAdvancedFindView View { get; set; }

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

            TableComboBoxSetup = new TextComboBoxControlSetup();
            var index = 0;
            foreach (var contextTableDefinition in SystemGlobals.AdvancedFindLookupContext.AdvancedFinds.Context
                         .TableDefinitions.OrderBy(p => p.Description))
            {
                if (!contextTableDefinition.Description.IsNullOrEmpty())
                {
                    TableComboBoxSetup.Items.Add(new TextComboBoxItem()
                        {NumericValue = index, TextValue = contextTableDefinition.Description});
                }

                index++;
            }

            ColumnsManager = new AdvancedFindColumnsManager(this);
            FiltersManager = new AdvancedFindFiltersManager(this);

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

        public void CreateCommands()
        {
            AddColumnCommand = new RelayCommand(AddColumn);

            AddFilterCommand = new RelayCommand(ShowFilterWindow);

            FromFormulaCommand = new RelayCommand(ShowFromFormulaEditor);

            ImportDefaultLookupCommand = new RelayCommand(ImportDefaultLookup);

            ApplyToLookupCommand = new RelayCommand(ApplyToLookup);
        }

        protected override AdvancedFind PopulatePrimaryKeyControls(AdvancedFind newEntity,
            PrimaryKeyValue primaryKeyValue)
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

            if (!entity.FromFormula.IsNullOrEmpty())
            {
                LookupDefinition.HasFromFormula(entity.FromFormula);
                View.NotifyFromFormulaExists = true;
            }
            else
            {
                View.NotifyFromFormulaExists = false;
            }

            ColumnsManager.LoadGrid(entity.Columns);
            FiltersManager.LoadGrid(entity.Filters);

            ResetLookup();
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
                    if (field.ParentJoinForeignKeyDefinition != null &&
                        field.ParentJoinForeignKeyDefinition.PrimaryTable != null)
                        AddTreeItem(field.ParentJoinForeignKeyDefinition.PrimaryTable, treeRoot.Items,
                            field.ParentJoinForeignKeyDefinition, treeRoot);
                }

                AddFormulaToTree(treeItems, null);

                LookupCommand = GetLookupCommand(LookupCommands.Clear);

            }

            TreeRoot = treeItems;
            FromFormulaCommand.IsEnabled = true;
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

            AddFormulaToTree(treeItems, parent);
        }

        private void AddFormulaToTree(ObservableCollection<TreeViewItem> treeItems, TreeViewItem parent)
        {
            var formulaTreeItem = new TreeViewItem
            {
                Name = "<Formula>",
                Type = TreeViewType.Formula,
                ViewModel = this,
                Parent = parent
            };
            treeItems.Add(formulaTreeItem);
        }

        public void OnTreeViewItemSelected(TreeViewItem treeViewItem)
        {
            SelectedTreeViewItem = treeViewItem;
            if (treeViewItem != null)
            {
                AddColumnCommand.IsEnabled = AddFilterCommand.IsEnabled = true;
            }
        }

        protected override bool ValidateEntity(AdvancedFind entity)
        {
            if (SelectedTableBoxItem == null)
            {
                var message = "You must select a table before saving.";
                var caption = "Invalid Table";
                //ControlsGlobals.UserInterface.ShowMessageBox(message, caption, RsMessageBoxIcons.Exclamation);
                View.OnValidationFail(
                    SystemGlobals.AdvancedFindLookupContext.AdvancedFinds.GetFieldDefinition(p => p.Table)
                    , message, caption);
                return false;
            }
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

            advancedFind.FromFormula = LookupDefinition?.FromFormula;

            advancedFind.Table = TableDefinition.Context.TableDefinitions
                .FirstOrDefault(p => p.Description == SelectedTableBoxItem?.TextValue)
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
            View.NotifyFromFormulaExists = false;

            if (LookupDefinition != null)
            {
                ResetLookup();
            }

            ColumnsManager.SetupForNewRecord();
            FiltersManager.SetupForNewRecord();
            AddColumnCommand.IsEnabled =
                AddFilterCommand.IsEnabled = false;
            FromFormulaCommand.IsEnabled = SelectedTableBoxItem != null;

            //LoadTree();
        }

        public void CreateLookupDefinition()
        {
            if (SelectedTableBoxItem != null)
            {
                var tableDefinition =
                    SystemGlobals.AdvancedFindLookupContext.AdvancedFinds.Context.TableDefinitions.FirstOrDefault(p =>
                        p.Description == SelectedTableBoxItem.TextValue);

                var oldLookup = LookupDefinition;
                LookupDefinition = new LookupDefinitionBase(tableDefinition);
                FiltersManager?.LoadFromLookupDefinition(oldLookup, true);
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
                FiltersManager.GetEntityList());
        }

        protected override bool DeleteEntity()
        {
            return SystemGlobals.AdvancedFindDbProcessor.DeleteAdvancedFind(AdvancedFindId);
        }

        private void AddColumn()
        {
            switch (SelectedTreeViewItem.Type)
            {
                case TreeViewType.Field:
                    var column = MakeIncludes(SelectedTreeViewItem, SelectedTreeViewItem.Name).ColumnDefinition;
                    ColumnsManager.LoadFromColumnDefinition(column);
                    break;
                case TreeViewType.Formula:
                    if (View.ShowFormulaEditor(SelectedTreeViewItem))
                    {
                        var formulaColumn =
                            MakeIncludes(SelectedTreeViewItem, SelectedTreeViewItem.Name).ColumnDefinition as
                                LookupFormulaColumnDefinition;
                        formulaColumn.UpdateCaption("<No Caption>");
                        formulaColumn.HasDataType(SelectedTreeViewItem.FormulaData.DataType);
                        formulaColumn.HasDecimalFieldType(
                            (DecimalFieldTypes) (int) SelectedTreeViewItem.FormulaData.DecimalFormatType);
                        ColumnsManager.LoadFromColumnDefinition(formulaColumn);
                    }

                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }

            ResetLookup();
        }

        public ProcessIncludeResult MakeIncludes(TreeViewItem selectedItem, string columnCaption = "",
            bool createColumn = true)
        {
            var result = new ProcessIncludeResult();
            var childNodes = new List<TreeViewItem>();

            LookupJoin includeJoin = null;
            var parentTreeItem = selectedItem;
            while (parentTreeItem.Parent != null)
            {
                parentTreeItem = parentTreeItem.Parent;
                childNodes.Insert(0, parentTreeItem);
            }

            if (childNodes.IndexOf(selectedItem) == -1 && selectedItem?.FieldDefinition?.ParentJoinForeignKeyDefinition != null)
            {
                childNodes.Add(selectedItem);
            }
            
            if (childNodes.Any() == false)
            {
                if (createColumn)
                {
                    switch (selectedItem.Type)
                    {
                        case TreeViewType.Field:
                            var processResult =
                                SelectColumnDescription(selectedItem, selectedItem, null, columnCaption);
                            includeJoin = ProcessInclude(includeJoin, processResult.LookupJoin);
                            result.LookupJoin = includeJoin;
                            result.ColumnDefinition = processResult.ColumnDefinition;
                            break;
                        case TreeViewType.Formula:
                            var column = LookupDefinition.AddVisibleColumnDefinition(columnCaption,
                                selectedItem.FormulaData.Formula, 20, selectedItem.FormulaData.DataType, "");
                            result.ColumnDefinition = column;

                            break;
                        default:
                            throw new ArgumentOutOfRangeException();
                    }


                    if (result.ColumnDefinition is LookupFormulaColumnDefinition formulaColumn)
                    {
                        if (result.LookupJoin == null)
                        {
                            formulaColumn.PrimaryTable = LookupDefinition.TableDefinition;
                        }
                        else
                        {
                            formulaColumn.PrimaryTable =
                                result.LookupJoin.JoinDefinition.ForeignKeyDefinition.ForeignTable;

                            formulaColumn.PrimaryField =
                                result.LookupJoin.JoinDefinition.ForeignKeyDefinition.FieldJoins[0].ForeignField;
                        }
                    }
                }
            }

            foreach (var child in childNodes)
            {
                if (childNodes.IndexOf(child) == 0)
                {
                    var newInclude = LookupDefinition.Include(child.FieldDefinition);
                    includeJoin = ProcessInclude(includeJoin, newInclude);
                    result.LookupJoin = includeJoin;
                }

                if (childNodes.IndexOf(child) == childNodes.Count - 1)
                {
                    if (childNodes.Count > 1)
                    {
                        var newInclude = includeJoin.Include(child.FieldDefinition);
                        includeJoin = ProcessInclude(includeJoin, newInclude);
                        result.LookupJoin = includeJoin;
                    }

                    if (createColumn)
                    {
                        switch (selectedItem.Type)
                        {
                            case TreeViewType.Field:
                                var processResult =
                                    SelectColumnDescription(selectedItem, child, includeJoin, columnCaption);
                                includeJoin = ProcessInclude(includeJoin, processResult.LookupJoin);
                                result.LookupJoin = includeJoin; 
                                result.ColumnDefinition = processResult.ColumnDefinition;
                                break;
                            case TreeViewType.Formula:
                                 var  column= includeJoin.AddVisibleColumnDefinition(columnCaption,
                                    selectedItem.FormulaData.Formula, 20, selectedItem.FormulaData.DataType);
                                result.ColumnDefinition = column;

                                break;
                            default:
                                throw new ArgumentOutOfRangeException();
                        }

                        if (result.ColumnDefinition is LookupFormulaColumnDefinition formulaColumn)
                        {
                            formulaColumn.PrimaryTable =
                                includeJoin.JoinDefinition.ForeignKeyDefinition.ForeignTable;

                            formulaColumn.PrimaryField =
                                includeJoin.JoinDefinition.ForeignKeyDefinition.FieldJoins[0].ForeignField;
                        }
                    }
                }
                else if (childNodes.IndexOf(child) != 0)
                {
                    var newInclude = includeJoin.Include(child.FieldDefinition);
                    includeJoin = ProcessInclude(includeJoin, newInclude);
                    result.LookupJoin = includeJoin;
                }
            }

            return result;
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

        private ProcessIncludeResult SelectColumnDescription(TreeViewItem selectedTreeViewItem, TreeViewItem child,
            LookupJoin includeJoin, string caption)
        {
            LookupColumnDefinitionBase column = null;
            if (selectedTreeViewItem.FieldDefinition.ParentJoinForeignKeyDefinition != null)
            {
                var textField = selectedTreeViewItem.FieldDefinition.ParentJoinForeignKeyDefinition
                    .FieldJoins[0].ForeignField;

                //if (includeJoin != null)
                //    includeJoin = includeJoin.Include(textField);
                //else
                //{
                //    includeJoin = LookupDefinition.Include(textField);
                //}

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
                    var formula = lookupFormulaColumn.OriginalFormula;
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
                        column = includeJoin.AddVisibleColumnDefinition(caption, selectedTreeViewItem.FieldDefinition,
                            20);
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
            if (!lookupDefinition.FromFormula.IsNullOrEmpty())
            {
                LookupDefinition.HasFromFormula(lookupDefinition.FromFormula);
                View.NotifyFromFormulaExists = true;
            }
            else
            {
                View.NotifyFromFormulaExists = false;
            }

            var parentObjects = new List<IJoinParent>();
            foreach (var visibleColumn in lookupDefinition.VisibleColumns)
            {
                var parent = visibleColumn.ParentObject;
                var lookupFieldColumn = visibleColumn as LookupFieldColumnDefinition;
                var lookupFormulaColumn = visibleColumn as LookupFormulaColumnDefinition;
                TreeViewItem foundTreeItem = null;
                var createColumn = true;
                switch (visibleColumn.ColumnType)
                {
                    case LookupColumnTypes.Field:
                        foundTreeItem = FindFieldInTree(TreeRoot, lookupFieldColumn.FieldDefinition);
                        break;
                    case LookupColumnTypes.Formula:
                        if (parent == null)
                        {
                            var newFormulaColumn = LookupDefinition.AddVisibleColumnDefinition(visibleColumn.Caption,
                                lookupFormulaColumn.OriginalFormula, visibleColumn.PercentWidth,
                                lookupFormulaColumn.DataType, "");
                            createColumn = false;
                            newFormulaColumn.PrimaryTable = LookupDefinition.TableDefinition;
                        }
                        else
                        {
                            if (parent is LookupJoin lookupJoin)
                            {
                                foundTreeItem = FindFieldInTree(TreeRoot,
                                    lookupJoin.JoinDefinition.ForeignKeyDefinition.FieldJoins[0].ForeignField);
                            }

                        }
                        break;
                    default:
                        throw new ArgumentOutOfRangeException();
                }

                if (createColumn)
                    MakeIncludes(foundTreeItem, visibleColumn.Caption, createColumn);
            }
            //    LookupFormulaColumnDefinition newLookupFormulaColumnDefinition = null;
            //    LookupColumnDefinitionBase newLookupColumnDefinition = null;

            //    if (parent == null)
            //    {
            //        if (lookupFieldColumn != null)
            //        {
            //            newLookupColumnDefinition = LookupDefinition.AddVisibleColumnDefinition(visibleColumn.Caption,
            //                lookupFieldColumn.FieldDefinition, visibleColumn.PercentWidth, "");
            //        }
            //        else if (lookupFormulaColumn != null)
            //        {
            //            newLookupFormulaColumnDefinition = LookupDefinition.AddVisibleColumnDefinition(
            //                visibleColumn.Caption, lookupFormulaColumn.OriginalFormula,
            //                visibleColumn.PercentWidth, lookupFormulaColumn.DataType,
            //                lookupFormulaColumn.JoinQueryTableAlias);

            //            newLookupFormulaColumnDefinition.PrimaryTable = LookupDefinition.TableDefinition;

            //        }
            //    }
            //    else
            //    {
            //        while (parent != null)
            //        {
            //            parentObjects.Insert(0, parent);
            //            parent = parent.ParentObject;
            //        }

            //        LookupJoin include = null;
            //        var index = 0;
            //        foreach (var parentObject in parentObjects)
            //        {
            //            if (index == 0)
            //            {
            //                include = LookupDefinition.MakeInclude(LookupDefinition,
            //                    visibleColumn.ParentField);
            //            }

            //            if (index == parentObjects.Count - 1)
            //            {
            //                if (lookupFieldColumn != null)
            //                {
            //                    newLookupColumnDefinition = include.AddVisibleColumnDefinitionField(
            //                        visibleColumn.Caption,
            //                        lookupFieldColumn.FieldDefinition, visibleColumn.PercentWidth);

            //                }
            //                else if (lookupFormulaColumn != null)
            //                {
            //                    newLookupFormulaColumnDefinition = include.AddVisibleColumnDefinition(
            //                        visibleColumn.Caption, lookupFormulaColumn.OriginalFormula,
            //                        visibleColumn.PercentWidth, lookupFormulaColumn.DataType);

            //                    newLookupFormulaColumnDefinition.PrimaryTable =
            //                        include.JoinDefinition.ForeignKeyDefinition.ForeignTable;

            //                    newLookupFormulaColumnDefinition.PrimaryField =
            //                        include.JoinDefinition.ForeignKeyDefinition.FieldJoins[0].ForeignField;

            //                }

            //            }
            //            else if (index > 0)
            //            {
            //                if (include == null)
            //                {
            //                    include = parentObject.MakeInclude(LookupDefinition);
            //                }
            //                else
            //                {
            //                    include = include.MakeInclude(LookupDefinition);
            //                }
            //            }

            //            index++;
            //        }
            //    }

            //    if (newLookupFormulaColumnDefinition != null && lookupFormulaColumn != null)
            //    {
            //        newLookupColumnDefinition = newLookupFormulaColumnDefinition;
            //        newLookupFormulaColumnDefinition.DecimalFieldType = lookupFormulaColumn.DecimalFieldType;
            //    }

            //    if (visibleColumn.ContentTemplateId != null)
            //        newLookupColumnDefinition.HasContentTemplateId((int) visibleColumn.ContentTemplateId);
            //}

            FiltersManager.LoadFromLookupDefinition(lookupDefinition);
            ColumnsManager.LoadFromLookupDefinition(LookupDefinition);
            AddColumnCommand.IsEnabled = AddFilterCommand.IsEnabled = false;
            ResetLookup();
        }

        public void ResetLookup()
        {
            if (FiltersManager.ValidateParentheses())
                LookupCommand = GetLookupCommand(LookupCommands.Reset, null, _input?.InputParameter);
        }

        private void ShowFromFormulaEditor()
        {
            var fromFormula = LookupDefinition.FromFormula;
            if (View.ShowFromFormulaEditor(ref fromFormula))
            {
                View.NotifyFromFormulaExists = !fromFormula.IsNullOrEmpty();
                LookupDefinition.HasFromFormula(fromFormula);
                ResetLookup();
            }
        }

        private void ShowFilterWindow()
        {
            var result = View.ShowAdvancedFilterWindow(SelectedTreeViewItem, LookupDefinition);
            if (result != null)
            {
                FiltersManager.LoadNewUserFilter(result);
            }
        }

        public TreeViewItem FindFieldInTree(ObservableCollection<TreeViewItem> items, FieldDefinition fieldDefinition,
            bool searchForRootFormula = false, TreeViewItem parentItem = null)
        {
            TreeViewItem foundTreeViewItem = null;
            if (!items.Any())
            {
                return parentItem;
            }

            foreach (var treeViewItem in items)
            {
                if (treeViewItem.Type == TreeViewType.Field)
                {
                    if (treeViewItem.FieldDefinition == fieldDefinition)
                    {
                        return treeViewItem;
                    }
                    else
                    {
                        foundTreeViewItem = FindFieldInTree(treeViewItem.Items, fieldDefinition);
                        if (foundTreeViewItem != null)
                        {
                            return foundTreeViewItem;
                        }

                    }
                }
                else if (treeViewItem.Type == TreeViewType.Formula && searchForRootFormula)
                {
                    return treeViewItem;
                }

            }

            return foundTreeViewItem;
        }

        public TreeViewItem ProcessFoundTreeViewItem(string formula, FieldDefinition fieldDefinition,
            FieldDataTypes? fieldDataType = null, DecimalEditFormatTypes? decimalEditFormat = null)
        {
            var items = TreeRoot;
            var foundTreeViewItem = FindFieldInTree(items, fieldDefinition);
            var alreadySearchedRoot = false;
            if (!formula.IsNullOrEmpty())
            {
                if (foundTreeViewItem == null)
                {
                    foundTreeViewItem = FindFieldInTree(items, fieldDefinition, true);
                    alreadySearchedRoot = true;
                }

                foundTreeViewItem.FormulaData = new TreeViewFormulaData();
                foundTreeViewItem.FormulaData.Formula = formula;
                if (fieldDataType != null)
                {
                    foundTreeViewItem.FormulaData.DataType = (FieldDataTypes) fieldDataType;
                }

                if (decimalEditFormat != null)
                {
                    foundTreeViewItem.FormulaData.DecimalFormatType = (DecimalEditFormatTypes) decimalEditFormat;
                }
            }

            if (!formula.IsNullOrEmpty() && !alreadySearchedRoot)
            {
                foundTreeViewItem =
                    FindFieldInTree(foundTreeViewItem.Items, fieldDefinition, true, foundTreeViewItem);

                foundTreeViewItem.FormulaData = new TreeViewFormulaData();
                foundTreeViewItem.FormulaData.Formula = formula;
                if (fieldDataType != null)
                {
                    foundTreeViewItem.FormulaData.DataType = (FieldDataTypes) fieldDataType;
                    foundTreeViewItem.FormulaData.DecimalFormatType = (DecimalEditFormatTypes) decimalEditFormat;
                }
            }

            return foundTreeViewItem;
        }

        private void ImportDefaultLookup()
        {
            if (SelectedTableBoxItem != null)
            {
                CreateLookupDefinition();
                var lookupDefinition= LookupDefinition.TableDefinition.LookupDefinition;
                LoadFromLookupDefinition(lookupDefinition);
            }
        }

        private void ApplyToLookup()
        {
            LookupDefinition.TableDefinition.HasLookupDefinition(LookupDefinition);
            View.ApplyToLookup();
        }
    }
}