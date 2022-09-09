using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Threading;
using System.Threading.Tasks;
using System.Timers;
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

        void ShowSqlStatement();

        bool ShowRefreshSettings(AdvancedFind advancedFind);

        void SetAlertLevel(AlertLevels level);

        int GetRecordCount(bool showRecordCount);

        void LockTable(bool lockValue);
    }

    //public class TreeViewFormulaData
    //{
    //    public string Formula { get; set; }

    //    public FieldDataTypes DataType { get; set; }

    //    public DecimalEditFormatTypes DecimalFormatType { get; set; }
    //}

    //public class ProcessIncludeResult
    //{
    //    public LookupJoin LookupJoin { get; set; }
    //    public LookupColumnDefinitionBase ColumnDefinition { get; set; }
    //}

    public class AdvancedFindInput
    {
        public object InputParameter { get; set; }
        public TableDefinitionBase LockTable { get; set; }
        public LookupDefinitionBase LookupDefinition { get; set; }
        public double LookupWidth { get; set; }
    }


    //public class TreeViewItem : INotifyPropertyChanged
    //{
    //    public string Name { get; set; }
    //    public TreeViewType Type { get; set; }
    //    public FieldDefinition FieldDefinition { get; set; }
    //    public ObservableCollection<TreeViewItem> Items { get; set; } = new ObservableCollection<TreeViewItem>();
    //    public ForeignKeyDefinition ParentJoin { get; set; }
    //    public AdvancedFindViewModel ViewModel { get; set; }
    //    public LookupJoin Include { get; set; }
    //    public TreeViewItem Parent { get; set; }
    //    public TreeViewFormulaData FormulaData { get; set; }

    //    private bool _isSelected;

    //    public bool IsSelected
    //    {
    //        get { return _isSelected; }
    //        set
    //        {
    //            if (_isSelected != value)
    //            {
    //                _isSelected = value;
    //                OnPropertyChanged();
    //                if (_isSelected)
    //                {
    //                    SelectedTreeItem = this;
    //                }
    //            }
    //        }
    //    }

    //    private TreeViewItem _selectedTreeItem;

    //    public TreeViewItem SelectedTreeItem
    //    {
    //        get => _selectedTreeItem;
    //        set
    //        {
    //            _selectedTreeItem = value;
    //            ViewModel.OnTreeViewItemSelected(_selectedTreeItem);
    //        }
    //    }

    //    public override string ToString()
    //    {
    //        return Name;
    //    }

    //    public event PropertyChangedEventHandler PropertyChanged;

    //    protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
    //    {
    //        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
    //    }
    //}


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
                OnPropertyChanged(null, false);
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
                    var table = TableDefinition.Context.TableDefinitions.FirstOrDefault(p =>
                        p.Description == _selectedTableBoxItem.TextValue);
                    LoadTree(table.TableName);
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
                OnPropertyChanged(null, false);
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

        public RelayCommand ShowSqlCommand { get; set; }

        public RelayCommand RefreshSettingsCommand { get; set; }

        public RelayCommand RefreshNowCommand { get; set; }

        public TreeViewItem SelectedTreeViewItem { get; set; }

        public IAdvancedFindView View { get; set; }

        public AdvancedFindInput AdvancedFindInput { get; set; }

        public AdvancedFindTree AdvancedFindTree { get; set; }

        public byte? RefreshRate { get; set; }
        public int? RefreshValue { get; set; }
        public byte? RefreshCondition { get; set; }
        public int? YellowAlert { get; set; }
        public int? RedAlert { get; set; }

        private RefreshRate _refreshRate;
        private int _refreshValue;
        private System.Timers.Timer _timer;
        private int _interval = 0;

        protected override void Initialize()
        {
            if (LookupAddViewArgs != null)
            {
                if (LookupAddViewArgs.InputParameter is AdvancedFindInput advancedFindInput)
                {
                    AdvancedFindInput = advancedFindInput;
                }
            }
            _timer = new System.Timers.Timer(1000);
            _timer.Elapsed += _timer_Elapsed;

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

            AdvancedFindTree = new AdvancedFindTree(LookupDefinition);
            AdvancedFindTree.SelectedTreeItemChanged += (sender, item) => OnTreeViewItemSelected(item);

            ColumnsManager = new AdvancedFindColumnsManager(this);
            FiltersManager = new AdvancedFindFiltersManager(this);

            if (AdvancedFindInput != null)
            {
                TableIndex = TableComboBoxSetup.Items.FindIndex(p => p.TextValue == AdvancedFindInput.LockTable.Description);
                ViewLookupDefinition.FilterDefinition.AddFixedFilter(
                    SystemGlobals.AdvancedFindLookupContext.AdvancedFinds.GetFieldDefinition(p =>
                        p.Table), Conditions.Equals,
                    AdvancedFindInput.LockTable.EntityName);
                if (AdvancedFindInput.LookupDefinition != null)
                    LoadFromLookupDefinition(AdvancedFindInput.LookupDefinition);
            }
            View.SetAlertLevel(AlertLevels.Green);
            base.Initialize();
        }

        public void CreateCommands()
        {
            AddColumnCommand = new RelayCommand(AddColumn);

            AddFilterCommand = new RelayCommand(AddFilter);

            FromFormulaCommand = new RelayCommand(ShowFromFormulaEditor);

            ImportDefaultLookupCommand = new RelayCommand(ImportDefaultLookup);

            ApplyToLookupCommand = new RelayCommand(ApplyToLookup);

            ShowSqlCommand = new RelayCommand(ShowSql);

            RefreshSettingsCommand = new RelayCommand(ShowRefreshSettings);

            RefreshNowCommand = new RelayCommand(RefreshNow);
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
            ClearRefresh();
            LoadRefreshSettings(entity);
            ProcessRefresh(true);

            ColumnsManager.LoadGrid(entity.Columns);
            FiltersManager.LoadGrid(entity.Filters);

            ResetLookup();
            View.LockTable(true);

            ApplyToLookupCommand.IsEnabled = RefreshNowCommand.IsEnabled =
                ShowSqlCommand.IsEnabled = RefreshSettingsCommand.IsEnabled = true;
        }

        private void LoadRefreshSettings(AdvancedFind entity)
        {
            RefreshRate = entity.RefreshRate;
            RefreshValue = entity.RefreshValue;
            RefreshCondition = entity.RefreshCondition;
            YellowAlert = entity.YellowAlert;
            RedAlert = entity.RedAlert;

            if (RefreshRate.HasValue)
            {
                _refreshRate = (DbLookup.AdvancedFind.RefreshRate) RefreshRate.Value;
            }

            if (RefreshValue.HasValue)
            {
                _refreshValue = RefreshValue.Value;
            }
        }

        private void LoadTree(string tableName)
        {
            CreateLookupDefinition();

            ImportDefaultLookupCommand.IsEnabled = FromFormulaCommand.IsEnabled = true;

            AdvancedFindTree.LoadTree(tableName);

            TreeRoot = AdvancedFindTree.TreeRoot;

            LookupCommand = GetLookupCommand(LookupCommands.Clear);
        }

        //private void AddTreeItem(TableDefinitionBase table,
        //    ObservableCollection<TreeViewItem> treeItems,
        //    ForeignKeyDefinition join, TreeViewItem parent, AdvancedFindTree baseTree)
        //{
        //    foreach (var tableFieldDefinition in table.FieldDefinitions.OrderBy(p => p.Description))
        //    {
        //        var treeChildItem = new TreeViewItem();
        //        treeChildItem.Name = tableFieldDefinition.Description;
        //        treeChildItem.Type = TreeViewType.Field;
        //        treeChildItem.FieldDefinition = tableFieldDefinition;
        //        //treeChildItem.ViewModel = this;
        //        treeChildItem.BaseTree = baseTree;
        //        treeChildItem.Parent = parent;
        //        if (tableFieldDefinition.ParentJoinForeignKeyDefinition != null)
        //        {
        //            join = tableFieldDefinition.ParentJoinForeignKeyDefinition;
        //        }

        //        treeChildItem.ParentJoin = join;
        //        treeItems.Add(treeChildItem);

        //        if (tableFieldDefinition.ParentJoinForeignKeyDefinition != null &&
        //            tableFieldDefinition.ParentJoinForeignKeyDefinition.PrimaryTable != null)
        //        {
        //            //treeChildItem.PrimaryFieldDefinition = tableFieldDefinition.ParentJoinForeignKeyDefinition
        //            //    .FieldJoins[0].PrimaryField;

        //            if (tableFieldDefinition.AllowRecursion)
        //                AddTreeItem(tableFieldDefinition.ParentJoinForeignKeyDefinition.PrimaryTable,
        //                    treeChildItem.Items, join, treeChildItem, baseTree);
        //        }
        //    }

        //    AddFormulaToTree(treeItems, parent, baseTree);
        //    AddAdvancedFindToTree(treeItems, parent, baseTree);
        //}

        //private void TreeChildItem_SelectedTreeItemChanged(object sender, TreeViewItem e)
        //{
        //    OnTreeViewItemSelected(e);
        //}

        //private void AddFormulaToTree(ObservableCollection<TreeViewItem> treeItems, TreeViewItem parent, AdvancedFindTree baseTree)
        //{
        //    var formulaTreeItem = new TreeViewItem
        //    {
        //        Name = "<Formula>",
        //        Type = TreeViewType.Formula,
        //        Parent = parent
        //    };
        //    formulaTreeItem.BaseTree = baseTree;
        //    treeItems.Add(formulaTreeItem);
        //}

        //private void AddAdvancedFindToTree(ObservableCollection<TreeViewItem> treeViewItems, TreeViewItem parent, AdvancedFindTree baseTree)
        //{
        //    var result = new TreeViewItem
        //    {
        //        Name = "<Advanced Find>",
        //        Type = TreeViewType.AdvancedFind,
        //        //ViewModel = this,
        //        Parent = parent
        //    };
        //    result.BaseTree = baseTree;

        //    treeViewItems.Add(result);
        //}

        public void OnTreeViewItemSelected(TreeViewItem treeViewItem)
        {
            SelectedTreeViewItem = treeViewItem;
            if (treeViewItem != null)
            {
                AddColumnCommand.IsEnabled = SelectedTreeViewItem.Type != TreeViewType.AdvancedFind;
                AddFilterCommand.IsEnabled = true;
            }
        }

        protected override bool ValidateEntity(AdvancedFind entity)
        {
            if (!ValidateLookup())
                return false;

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

            advancedFind.RefreshRate = RefreshRate;
            advancedFind.RefreshValue = RefreshValue;
            advancedFind.RefreshCondition = RefreshCondition;
            advancedFind.YellowAlert = YellowAlert;
            advancedFind.RedAlert = RedAlert;

            return advancedFind;
        }

        protected override void ClearData()
        {
            AdvancedFindId = 0;
            if (AdvancedFindInput == null || AdvancedFindInput.LockTable == null)
            {
                TableIndex = -1;
                SelectedTableBoxItem = null;
                SelectedTreeViewItem = null;
                TreeRoot?.Clear();
            }

            CreateLookupDefinition();
            View.NotifyFromFormulaExists = false;

            //if (LookupDefinition != null)
            //{
            //ResetLookup();
            var command = GetLookupCommand(LookupCommands.Reset, null, AdvancedFindInput?.InputParameter);
            command.ClearColumns = true;
            LookupCommand = command;
            //}

            ColumnsManager.SetupForNewRecord();
            FiltersManager.SetupForNewRecord();
            AddColumnCommand.IsEnabled = 
                AddFilterCommand.IsEnabled = ApplyToLookupCommand.IsEnabled = RefreshNowCommand.IsEnabled =
                    ShowSqlCommand.IsEnabled = RefreshSettingsCommand.IsEnabled = false;

            FromFormulaCommand.IsEnabled = ImportDefaultLookupCommand.IsEnabled = SelectedTableBoxItem != null;

            ClearRefresh();
            LockTable();

            //LoadTree();
        }

        private void LockTable()
        {
            var lockValue = AdvancedFindInput?.LockTable != null;
            View.LockTable(lockValue);
        }

        private void ClearRefresh()
        {
            RefreshRate = null;
            RefreshValue = null;
            RefreshCondition = null;
            YellowAlert = null;
            RedAlert = null;
            View.SetAlertLevel(AlertLevels.Green);
            _refreshValue = 0;
            ProcessRefresh(true);
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
            AdvancedFindTree.LookupDefinition = LookupDefinition;
            ColumnsManager?.SetupForNewRecord();
            FiltersManager?.SetupForNewRecord();
            RefreshNowCommand.IsEnabled = RefreshSettingsCommand.IsEnabled =
                ApplyToLookupCommand.IsEnabled = ShowSqlCommand.IsEnabled = false;
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
            return AdvancedFindTree.MakeIncludes(selectedItem, columnCaption, createColumn);
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

            foreach (var visibleColumn in lookupDefinition.VisibleColumns)
            {
                var parent = visibleColumn.ParentObject;
                var lookupFieldColumn = visibleColumn as LookupFieldColumnDefinition;
                var lookupFormulaColumn = visibleColumn as LookupFormulaColumnDefinition;
                TreeViewItem foundTreeItem = null;
                var createColumn = true;
                LookupFormulaColumnDefinition newLookupFormulaColumnDefinition = null;
                LookupColumnDefinitionBase newLookupColumnDefinition = null;
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
                            newLookupFormulaColumnDefinition = newFormulaColumn;
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
                {
                    var includeResult = MakeIncludes(foundTreeItem, visibleColumn.Caption, createColumn);
                    newLookupColumnDefinition = includeResult.ColumnDefinition;
                }


                if (newLookupFormulaColumnDefinition != null && lookupFormulaColumn != null)
                {
                    newLookupColumnDefinition = newLookupFormulaColumnDefinition;
                    newLookupFormulaColumnDefinition.DecimalFieldType = lookupFormulaColumn.DecimalFieldType;
                }

                if (visibleColumn.ContentTemplateId != null)
                    newLookupColumnDefinition.HasContentTemplateId((int)visibleColumn.ContentTemplateId);
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
            AddColumnCommand.IsEnabled = AddFilterCommand.IsEnabled = 
                AddFilterCommand.IsEnabled = ApplyToLookupCommand.IsEnabled = RefreshNowCommand.IsEnabled =
                    ShowSqlCommand.IsEnabled = RefreshSettingsCommand.IsEnabled = false;
            ResetLookup();
        }

        public void ResetLookup()
        {
            if (ValidateLookup())
            {
                LookupCommand = GetLookupCommand(LookupCommands.Reset, null, AdvancedFindInput?.InputParameter);
                ProcessRefresh(true);
                ApplyToLookupCommand.IsEnabled = RefreshNowCommand.IsEnabled =
                    ShowSqlCommand.IsEnabled = RefreshSettingsCommand.IsEnabled = true;
            }
        }

        private bool ValidateLookup()
        {
            if (FiltersManager.ValidateParentheses())
            {
                if (!FiltersManager.ValidateAdvancedFind())
                {
                    var command = GetLookupCommand(LookupCommands.Reset, null, AdvancedFindInput?.InputParameter);
                    command.ClearColumns = true;
                    command.ResetSearchFor = true;
                    LookupCommand = command;
                    return false;
                }

                return true;
            }

            return false;
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

        private void AddFilter()
        {
            switch (SelectedTreeViewItem.Type)
            {
                case TreeViewType.AdvancedFind:
                    FiltersManager.AddAdvancedFindFilterRow(SelectedTreeViewItem.Parent?.FieldDefinition);
                    break;
                case TreeViewType.Field:
                case TreeViewType.Formula:
                    ShowFilterWindow();
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
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
            return AdvancedFindTree.FindFieldInTree(items, fieldDefinition, searchForRootFormula, parentItem);
        }

        public TreeViewItem ProcessFoundTreeViewItem(string formula, FieldDefinition fieldDefinition,
            FieldDataTypes? fieldDataType = null, DecimalEditFormatTypes? decimalEditFormat = null)
        {
            return AdvancedFindTree.ProcessFoundTreeViewItem(formula, fieldDefinition, fieldDataType, decimalEditFormat);
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
            if (ValidateLookup())
            {
                LookupDefinition.TableDefinition.HasLookupDefinition(LookupDefinition);
                View.ApplyToLookup();
            }
        }

        private void ShowSql()
        {
            if (ValidateLookup())
            {
                View.ShowSqlStatement();

            }
        }

        private void ShowRefreshSettings()
        {
            var refreshSettings = GetEntityData();
            if (View.ShowRefreshSettings(refreshSettings))
            {
                LoadRefreshSettings(refreshSettings);
                View.GetRecordCount(true);
                ResetLookup();
            }
        }

        private void RefreshNow()
        {
            if (ValidateLookup())
            {
                LookupCommand = GetLookupCommand(LookupCommands.Refresh);
                ProcessRefresh(true);
            }
        }

        private void ProcessRefresh(bool resetTimer)
        {
            if (RefreshCondition.HasValue)
            {
                var recordCount = View.GetRecordCount(true);
                var yellowAlert = YellowAlert.Value;
                var redAlert = RedAlert.Value;
                var refreshCondition = (Conditions)RefreshCondition.Value;

                switch (refreshCondition)
                {
                    case Conditions.Equals:
                        if (recordCount == yellowAlert)
                        {
                            View.SetAlertLevel(AlertLevels.Yellow);
                        }
                        if (recordCount == redAlert)
                        {
                            View.SetAlertLevel(AlertLevels.Red);
                        }
                        if (recordCount != yellowAlert && recordCount != redAlert)
                        {
                            View.SetAlertLevel(AlertLevels.Green);
                        }
                        break;
                    case Conditions.NotEquals:
                        if (recordCount != yellowAlert)
                        {
                            View.SetAlertLevel(AlertLevels.Yellow);
                        }
                        else if (recordCount != redAlert)
                        {
                            View.SetAlertLevel(AlertLevels.Red);
                        }
                        if (recordCount == yellowAlert && recordCount == redAlert)
                        {
                            View.SetAlertLevel(AlertLevels.Green);
                        }
                        break;
                    case Conditions.GreaterThan:
                        if (recordCount > yellowAlert)
                        {
                            View.SetAlertLevel(AlertLevels.Yellow);
                        }
                        if (recordCount > redAlert)
                        {
                            View.SetAlertLevel(AlertLevels.Red);
                        }
                        if(recordCount < yellowAlert && recordCount < redAlert)
                        {
                            View.SetAlertLevel(AlertLevels.Green);
                        }
                        break;
                    case Conditions.GreaterThanEquals:
                        if (recordCount >= yellowAlert)
                        {
                            View.SetAlertLevel(AlertLevels.Yellow);
                        }
                        if (recordCount >= redAlert)
                        {
                            View.SetAlertLevel(AlertLevels.Red);
                        }
                        if (recordCount <= yellowAlert && recordCount <= redAlert)
                        {
                            View.SetAlertLevel(AlertLevels.Green);
                        }
                        break;
                    case Conditions.LessThan:
                        if (recordCount < yellowAlert)
                        {
                            View.SetAlertLevel(AlertLevels.Yellow);
                        }
                        else if (recordCount < redAlert)
                        {
                            View.SetAlertLevel(AlertLevels.Red);
                        }
                        if (recordCount > yellowAlert && recordCount > redAlert)
                        {
                            View.SetAlertLevel(AlertLevels.Green);
                        }
                        break;
                    case Conditions.LessThanEquals:
                        if (recordCount <= yellowAlert)
                        {
                            View.SetAlertLevel(AlertLevels.Yellow);
                        }
                        else if (recordCount <= redAlert)
                        {
                            View.SetAlertLevel(AlertLevels.Red);
                        }
                        if (recordCount >= yellowAlert && recordCount >= redAlert)
                        {
                            View.SetAlertLevel(AlertLevels.Green);
                        }
                        break;
                    default:
                        throw new ArgumentOutOfRangeException();
                }

                if (resetTimer)
                {
                    ResetTimer();
                }
            }
            else
            {
                _refreshValue = 0;
                View.GetRecordCount(false);
            }
        }

        private void ResetTimer()
        {
            _interval = 0;
            if (_refreshValue <= 0)
            {
                _timer.Enabled = false;
            }
            else
            {
                _timer.Enabled = true;
            }

        }

        private void _timer_Elapsed(object sender, ElapsedEventArgs e)
        {
            _interval++ ;
            switch (_refreshRate)
            {
                case DbLookup.AdvancedFind.RefreshRate.Hours:
                    if (_interval == (_refreshValue * 60) * 60)
                    {
                        _interval = 0;
                        TimerRefresh();
                    }
                    break;
                case DbLookup.AdvancedFind.RefreshRate.Minutes:
                    if (_interval == _refreshValue * 60)
                    {
                        _interval = 0;
                        TimerRefresh();
                    }
                    break;
                case DbLookup.AdvancedFind.RefreshRate.Seconds:
                    if (_interval == _refreshValue)
                    {
                        _interval = 0;
                        TimerRefresh();
                    }
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }

        }

        private void TimerRefresh()
        {
            _timer.Enabled = false;
            if (ValidateLookup())
            {
                LookupCommand = GetLookupCommand(LookupCommands.Refresh);
                ProcessRefresh(false);
            }
            _timer.Enabled = true;

        }

        public override void OnWindowClosing(CancelEventArgs e)
        {
            _timer.Enabled = false;
            _timer.Stop();
            _interval = 0;
            View.SetAlertLevel(AlertLevels.Green);
            base.OnWindowClosing(e);
        }
    }
}