﻿using RingSoft.DataEntryControls.Engine;
using RingSoft.DbLookup;
using RingSoft.DbLookup.AdvancedFind;
using RingSoft.DbLookup.AutoFill;
using RingSoft.DbLookup.Lookup;
using RingSoft.DbLookup.ModelDefinition;
using RingSoft.DbLookup.ModelDefinition.FieldDefinitions;
using RingSoft.DbLookup.QueryBuilder;
using RingSoft.DbLookup.TableProcessing;
using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;

namespace RingSoft.DbMaintenance
{
    public interface IAdvancedFindView : IDbMaintenanceView
    {
        AdvancedFilterReturn ShowAdvancedFilterWindow(TreeViewItem treeViewItem, LookupDefinitionBase lookupDefinition);

        void ShowFiltersEllipse(bool showFiltersEllipse = true);

        bool ShowRefreshSettings(AdvancedFind advancedFind);

        void SetAlertLevel(AlertLevels level, string message, bool showCount, int recordCount);

        void LockTable(bool lockValue);

        int GetRecordCount(bool showRecordCount);

        void SetAddOnFlyFocus();

        void PrintOutput(PrinterSetupArgs printerSetup);

        void CheckTableIsFocused();
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

        //private TextComboBoxControlSetup _tableComboBoxSetup;

        //public TextComboBoxControlSetup TableComboBoxSetup
        //{
        //    get => _tableComboBoxSetup;
        //    set
        //    {
        //        if (_tableComboBoxSetup == value)
        //        {
        //            return;
        //        }

        //        _tableComboBoxSetup = value;
        //        OnPropertyChanged();
        //    }
        //}

        //private int _tableIndex;

        //public int TableIndex
        //{
        //    get => _tableIndex;
        //    set
        //    {
        //        if (_tableIndex == value)
        //        {
        //            return;
        //        }

        //        _tableIndex = value;
        //        OnPropertyChanged();
        //    }
        //}

        //private TextComboBoxItem _selectedTableBoxItem;

        //public TextComboBoxItem SelectedTableBoxItem
        //{
        //    get => _selectedTableBoxItem;
        //    set
        //    {
        //        if (_selectedTableBoxItem == value)
        //        {
        //            return;
        //        }

        //        _selectedTableBoxItem = value;
        //        if (_selectedTableBoxItem != null)
        //        {
        //            var table = TableDefinition.Context.TableDefinitions.FirstOrDefault(p =>
        //                p.Description == _selectedTableBoxItem.TextValue);
        //            LoadTree(table.TableName);
        //        }

        //        OnPropertyChanged();
        //    }
        //}

        private ListControlDataSourceRow _tableRow;

        public ListControlDataSourceRow TableRow
        {
            get => _tableRow;
            set
            {
                if (_tableRow == value)
                    return;

                _tableRow = value;
                if (TableRow != null)
                {
                    var table = TableDefinition.Context.TableDefinitions.FirstOrDefault(p =>
                        p.Description == TableRow.GetCellItem(0));
                    LoadTree(table.TableName);
                }


                OnPropertyChanged();
            }
        }

        private ListControlSetup _tableSetup;

        public ListControlSetup TableSetup
        {
            get => _tableSetup;
            set
            {
                if (_tableSetup == value) 
                    return;

                _tableSetup = value;
                OnPropertyChanged();
            }
        }

        private ListControlDataSource _tableDataSource;

        public ListControlDataSource TableDataSource
        {
            get => _tableDataSource;
            set
            {
                if (_tableDataSource == value)
                    return;

                _tableDataSource = value;
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

        public RelayCommand ImportDefaultLookupCommand { get; set; }

        public RelayCommand RefreshSettingsCommand { get; set; }

        public RelayCommand RefreshNowCommand { get; set; }

        public RelayCommand PrintLookupOutputCommand { get; set; }

        public UiCommand TableUiCommand { get; set; }

        public TreeViewItem SelectedTreeViewItem { get; set; }

        public IAdvancedFindView View { get; set; }

        public AdvancedFindInput AdvancedFindInput { get; set; }

        public AdvancedFindTree AdvancedFindTree { get; set; }

        public LookupRefresher LookupRefresher { get; private set; }

        public bool Clearing { get; private set; }

        private int _recordCount;

        public AdvancedFindViewModel()
        {
            TablesToDelete.Add(SystemGlobals.AdvancedFindLookupContext.AdvancedFindColumns);
            TablesToDelete.Add(SystemGlobals.AdvancedFindLookupContext.AdvancedFindFilters);

            TableUiCommand = new UiCommand();
            MapFieldToUiCommand(TableUiCommand, TableDefinition.GetFieldDefinition(p => p.Table));
        }

        protected override void Initialize()
        {
            if (LookupAddViewArgs != null)
            {
                if (LookupAddViewArgs.InputParameter is AdvancedFindInput advancedFindInput)
                {
                    AdvancedFindInput = advancedFindInput;
                }
            }

            //TableComboBoxSetup = new TextComboBoxControlSetup();
            //var index = 0;
            TableSetup = new ListControlSetup();
            TableDataSource = new ListControlDataSource();
            var dataColumn = TableSetup.AddColumn(1, "Table", FieldDataTypes.String, 95);
            var index = 1;

            foreach (var contextTableDefinition in SystemGlobals.AdvancedFindLookupContext.AdvancedFinds.Context.TableDefinitions)
            {
                if (contextTableDefinition.LookupDefinition != null
                    && contextTableDefinition.CanViewTable
                    && contextTableDefinition.IsAdvancedFind)
                {
                    var tableRow = new ListControlDataSourceRow();
                    tableRow.AddColumn(dataColumn, contextTableDefinition.Description);

                    TableDataSource.AddRow(tableRow);
                    index++;
                }
            }

            LookupRefresher = new LookupRefresher();
            LookupRefresher.StartRefresh();

            AdvancedFindTree = new AdvancedFindTree(LookupDefinition);
            AdvancedFindTree.SelectedTreeItemChanged += (sender, item) => OnTreeViewItemSelected(item);
            LookupRefresher.SetAlertLevelEvent += LookupRefresher_SetAlertLevelEvent;
            LookupRefresher.RefreshRecordCountEvent += LookupRefresher_RefreshRecordCountEvent;

            ColumnsManager = new AdvancedFindColumnsManager(this);
            FiltersManager = new AdvancedFindFiltersManager(this);

            if (AdvancedFindInput != null)
            {
                //TableIndex = TableComboBoxSetup.Items.FindIndex(p => p.TextValue == AdvancedFindInput.LockTable.Description);
                ViewLookupDefinition.FilterDefinition.AddFixedFilter(
                    SystemGlobals.AdvancedFindLookupContext.AdvancedFinds.GetFieldDefinition(p =>
                        p.Table), Conditions.Equals,
                    AdvancedFindInput.LockTable.EntityName);
                if (AdvancedFindInput.LookupDefinition != null)
                {
                    TableRow = TableDataSource.Items.FirstOrDefault(p =>
                        p.DataCells[0].TextValue == AdvancedFindInput.LookupDefinition.TableDefinition.Description);
                    CreateLookupDefinition();
                    LoadFromLookupDefinition(AdvancedFindInput.LookupDefinition);
                }
                else
                {
                    LockTableRow();
                }
            }
            //View.SetAlertLevel(AlertLevels.Green, "", true, 0);
            if (LookupAddViewArgs != null && LookupAddViewArgs.LookupFormMode == LookupFormModes.View)
            {
                View.SetAddOnFlyFocus();
            }
            base.Initialize();
        }

        private void LockTableRow()
        {
            if (AdvancedFindInput != null)
            {
                TableRow = TableDataSource.Items.FirstOrDefault(p =>
                    p.DataCells[0].TextValue == AdvancedFindInput.LockTable.Description);
            }
        }

        private void LookupRefresher_RefreshRecordCountEvent(object sender, EventArgs e)
        {
            ProcessRefresh();
        }

        private void LookupRefresher_SetAlertLevelEvent(object sender, RefreshAlertLevelArgs e)
        {

            var message = LookupRefresher.GetRecordCountMessage(_recordCount, KeyAutoFillValue?.Text);
            View.SetAlertLevel(e.AlertLevel, message, LookupRefresher.RefreshRate != RefreshRate.None, _recordCount);
        }

        public void CreateCommands()
        {
            AddColumnCommand = new RelayCommand(AddColumn);

            AddFilterCommand = new RelayCommand(AddFilter);

            AddColumnCommand.IsEnabled = AddFilterCommand.IsEnabled = SelectedTreeViewItem != null;

            ImportDefaultLookupCommand = new RelayCommand(ImportDefaultLookup);

            RefreshSettingsCommand = new RelayCommand(ShowRefreshSettings);

            RefreshNowCommand = new RelayCommand(RefreshNow);

            PrintLookupOutputCommand = new RelayCommand((() =>
            {
                var advFindPrintProcessor = new AdvancedFindPrinterProcessor(this);
                advFindPrintProcessor.PrintOutput();
            }));
        }

        protected override AdvancedFind PopulatePrimaryKeyControls(AdvancedFind newEntity,
            PrimaryKeyValue primaryKeyValue)
        {
            if (SystemGlobals.AdvancedFindDbProcessor == null)
            {
                throw new ApplicationException(
                    $"{nameof(SystemGlobals)}.{nameof(SystemGlobals.AdvancedFindDbProcessor)} not set.");
            }
            var advancedFind = SystemGlobals.AdvancedFindDbProcessor.GetAdvancedFind(newEntity.Id);
            if (advancedFind != null)
            {
                var tableDefinition =
                    TableDefinition.Context.TableDefinitions.FirstOrDefault(p => p.EntityName == advancedFind.Table);
                if (!tableDefinition.CanViewTable)
                {
                    return null;
                }

                var items = TableDataSource.Items;
                TableRow = items.FirstOrDefault(p => p.GetCellItem(0)
                                                                     == tableDefinition.Description);

            }
            AdvancedFindId = advancedFind.Id;

            KeyAutoFillValue = new AutoFillValue(primaryKeyValue, advancedFind.Name);

            ReadOnlyMode = false;
            View.LockTable(true);
            RefreshSettingsCommand.IsEnabled = RefreshNowCommand.IsEnabled = true;
            return advancedFind;
        }

        protected override void LoadFromEntity(AdvancedFind entity)
        {
            Clearing = true;
            AdvancedFindId = entity.Id;

            var tableDefinition =
                TableDefinition.Context.TableDefinitions.FirstOrDefault(p => p.EntityName == entity.Table);
            //var comboItem = TableComboBoxSetup.Items.FirstOrDefault(p => p.TextValue == tableDefinition.Description);
            //TableIndex = TableComboBoxSetup.Items.IndexOf(comboItem);
            CreateLookupDefinition();

            
            ClearRefresh();
            LoadRefreshSettings(entity);
            
            ColumnsManager.LoadGrid(entity.Columns);
            FiltersManager.LoadGrid(entity.Filters);

            if (entity.Filters.Any())
            {
                View.ShowFiltersEllipse(true);
            }

            if (tableDefinition.LookupDefinition != null)
            {
                if (tableDefinition.LookupDefinition.InitialOrderByField != null
                    && ColumnsManager.IsSortColumnInitialSortColumn())
                {
                    LookupDefinition.InitialOrderByField = tableDefinition.LookupDefinition.InitialOrderByField;
                }
            }

            //ProcessRefresh();

            ResetLookup();

            PrintLookupOutputCommand.IsEnabled =  RefreshNowCommand.IsEnabled = true;
            RefreshSettingsCommand.IsEnabled = true;

            Clearing = false;
        }

        private void LoadRefreshSettings(AdvancedFind entity)
        {
            LookupRefresher.LoadFromAdvFind(entity);
            //ProcessRefresh();
            LookupRefresher.ResetTimer();
        }

        private void LoadTree(string tableName)
        {
            CreateLookupDefinition();

            ImportDefaultLookupCommand.IsEnabled = true;

            AdvancedFindTree.LoadTree(tableName);
            this.LookupDefinition.AdvancedFindTree = AdvancedFindTree;
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

            if (!FiltersManager.ValidateGrid())
            {
                return false;
            }
            if (TableRow == null)
            {
                var message = "You must select a table before saving.";
                var caption = "Invalid Table";
                ControlsGlobals.UserInterface.ShowMessageBox(message, caption, RsMessageBoxIcons.Exclamation);
                TableUiCommand.SetFocus();
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
                .FirstOrDefault(p => p.Description == TableRow?.GetCellItem(0))
                ?.EntityName;

            advancedFind.RefreshRate = (byte)LookupRefresher.RefreshRate;
            advancedFind.RefreshValue = LookupRefresher.RefreshValue;
            advancedFind.RefreshCondition = (byte)LookupRefresher.RefreshCondition;
            advancedFind.YellowAlert = LookupRefresher.YellowAlert;
            advancedFind.RedAlert = LookupRefresher.RedAlert;
            advancedFind.Disabled = LookupRefresher.Disabled;

            return advancedFind;
        }

        protected override void ClearData()
        {
            Clearing = true;
            AdvancedFindId = 0;
            if (AdvancedFindInput == null || AdvancedFindInput.LockTable == null)
            {
                SelectedTreeViewItem = null;
                TreeRoot?.Clear();
                TableRow = null;
            }

            ReadOnlyMode = false;

            CreateLookupDefinition();

            //if (LookupDefinition != null)
            //{
            //ResetLookup();
            var command = GetLookupCommand(LookupCommands.Reset, null, AdvancedFindInput?.InputParameter);
            command.ClearColumns = true;
            LookupCommand = command;
            //}

            ColumnsManager.SetupForNewRecord();
            FiltersManager.SetupForNewRecord();
            PrintLookupOutputCommand.IsEnabled = AddColumnCommand.IsEnabled = 
                AddFilterCommand.IsEnabled =  RefreshNowCommand.IsEnabled = false;

            ImportDefaultLookupCommand.IsEnabled = TableRow != null;

            ClearRefresh();
            LockTable();
            View.ShowFiltersEllipse(false);
            Clearing = false;

            //LoadTree();
        }

        private void LockTable()
        {
            var lockValue = AdvancedFindInput?.LockTable != null;
            View.LockTable(lockValue);
        }

        private void ClearRefresh()
        {
            LookupRefresher.RefreshRate = RefreshRate.None;
            LookupRefresher.RefreshValue = 0;
            LookupRefresher.RefreshCondition = Conditions.Equals;
            LookupRefresher.YellowAlert = 0;
            LookupRefresher.RedAlert = 0;
            LookupRefresher.Disabled = false;
            View.SetAlertLevel(AlertLevels.Green, "", true, 0);
            //ProcessRefresh(true);
            LookupRefresher.ResetTimer();
        }

        public void CreateLookupDefinition()
        {
            if (TableRow != null)
            {
                var tableDefinition =
                    SystemGlobals.AdvancedFindLookupContext.AdvancedFinds.Context.TableDefinitions.FirstOrDefault(p =>
                        p.Description == TableRow.GetCellItem(0));

                var oldLookup = LookupDefinition;
                LookupDefinition = new LookupDefinitionBase(tableDefinition);
                LookupDefinition.AdvancedFindTree = AdvancedFindTree;
                //FiltersManager?.LoadFromLookupDefinition(oldLookup, true);
                if (oldLookup != null)
                    LookupDefinition.FilterDefinition.LoadFixedFromLookup(oldLookup.FilterDefinition);
            }
            else
            {
                LookupDefinition = null;
            }
            AdvancedFindTree.LookupDefinition = LookupDefinition;
            ColumnsManager?.SetupForNewRecord();
            FiltersManager?.SetupForNewRecord();
            PrintLookupOutputCommand.IsEnabled = RefreshNowCommand.IsEnabled = RefreshSettingsCommand.IsEnabled = false;
        }

        protected override bool SaveEntity(AdvancedFind entity)
        {
            if (SystemGlobals.AdvancedFindDbProcessor == null)
            {
                throw new ApplicationException("SystemGlobals.AdvancedFindDbProcessor has not been set.");
            }
            var result = SystemGlobals.AdvancedFindDbProcessor.SaveAdvancedFind(entity, ColumnsManager.GetEntityList(),
                FiltersManager.GetEntityList());
            if (result)
            {
                View.CheckTableIsFocused();
            }
            return result;
        }

        protected override bool DeleteEntity()
        {
            return SystemGlobals.AdvancedFindDbProcessor.DeleteAdvancedFind(AdvancedFindId);
        }

        private void AddColumn()
        {
            var startIndex = ColumnsManager.GetNewColumnIndex();
            var column = SelectedTreeViewItem.CreateColumn(startIndex);
            column.AdjustColumnWidth = false;
            ColumnsManager.LoadFromColumnDefinition(column);
            //switch (SelectedTreeViewItem.Type)
            //{
            //    case TreeViewType.Field:
            //        var column = MakeIncludes(SelectedTreeViewItem, SelectedTreeViewItem.Name).ColumnDefinition;
            //        ColumnsManager.LoadFromColumnDefinition(column);
            //        RecordDirty = true;
            //        break;
            //    case TreeViewType.Formula:
            //        if (View.ShowFormulaEditor(SelectedTreeViewItem))
            //        {
            //            RecordDirty = true;
            //            var formulaColumn =
            //                MakeIncludes(SelectedTreeViewItem, SelectedTreeViewItem.Name).ColumnDefinition as
            //                    LookupFormulaColumnDefinition;
            //            formulaColumn.UpdateCaption("<No Caption>");
            //            formulaColumn.HasDataType(SelectedTreeViewItem.FormulaData.DataType);
            //            formulaColumn.HasDecimalFieldType(
            //                (DecimalFieldTypes) (int) SelectedTreeViewItem.FormulaData.DecimalFormatType);
            //            ColumnsManager.LoadFromColumnDefinition(formulaColumn);
            //        }

            //        break;
            //    default:
            //        throw new ArgumentOutOfRangeException();
            //}

            ResetLookup();
        }

        public ProcessIncludeResult MakeIncludes(TreeViewItem selectedItem)
        {
            return LookupDefinition.AdvancedFindTree.MakeIncludes(selectedItem);
        }


        public void LoadFromLookupDefinition(LookupDefinitionBase lookupDefinition)
        {

            LookupDefinition.InitialOrderByField = lookupDefinition.InitialOrderByField;
            foreach (var visibleColumn in lookupDefinition.VisibleColumns)
            {
                visibleColumn.AddNewColumnDefinition(LookupDefinition);
            }

            //LookupDefinition.FilterDefinition.ClearFixedFilters();
            foreach (var fixedFilter in lookupDefinition.FilterDefinition.FixedFilters)
            {
                fixedFilter.CopyToNewFilter(LookupDefinition);
            }
            //if (!lookupDefinition.FromFormula.IsNullOrEmpty())
            //{
            //    LookupDefinition.HasFromFormula(lookupDefinition.FromFormula);
            //    View.NotifyFromFormulaExists = true;
            //}
            //else
            //{
            //    View.NotifyFromFormulaExists = false;
            //}

            //foreach (var visibleColumn in lookupDefinition.VisibleColumns)
            //{
            //    var parent = visibleColumn.ParentObject;
            //    var lookupFieldColumn = visibleColumn as LookupFieldColumnDefinition;
            //    var lookupFormulaColumn = visibleColumn as LookupFormulaColumnDefinition;
            //    TreeViewItem foundTreeItem = null;
            //    var createColumn = true;
            //    LookupFormulaColumnDefinition newLookupFormulaColumnDefinition = null;
            //    LookupColumnDefinitionBase newLookupColumnDefinition = null;
            //    switch (visibleColumn.ColumnType)
            //    {
            //        case LookupColumnTypes.Field:
            //            if (lookupFieldColumn.Path.IsNullOrEmpty())
            //            {
            //                foundTreeItem = FindFieldInTree(TreeRoot, lookupFieldColumn.FieldDefinition);
            //            }
            //            else
            //            {
            //                foundTreeItem = AdvancedFindTree.ProcessFoundTreeViewItem(lookupFieldColumn.Path);
            //            }
            //            break;
            //        case LookupColumnTypes.Formula:
            //            if (parent == null)
            //            {
            //                var newFormulaColumn = LookupDefinition.AddVisibleColumnDefinition(visibleColumn.Caption,
            //                    lookupFormulaColumn.OriginalFormula, visibleColumn.PercentWidth,
            //                    lookupFormulaColumn.DataType, "");
            //                newLookupFormulaColumnDefinition = newFormulaColumn;
            //                createColumn = false;
            //                newFormulaColumn.PrimaryTable = LookupDefinition.TableDefinition;
            //                newFormulaColumn.HasConvertToLocalTime(lookupFormulaColumn
            //                    .ConvertToLocalTime);
            //                newFormulaColumn.HasDateType(lookupFormulaColumn.DateType);
            //                newFormulaColumn.HasDateFormatString(string.Empty);
            //            }
            //            else
            //            {
            //                if (parent is LookupJoin lookupJoin)
            //                {
            //                    foundTreeItem = FindFieldInTree(TreeRoot,
            //                        lookupJoin.JoinDefinition.ForeignKeyDefinition.FieldJoins[0].ForeignField);
            //                }

            //            }

            //            break;
            //        default:
            //            throw new ArgumentOutOfRangeException();
            //    }

            //    if (createColumn)
            //    {
            //        if (lookupFormulaColumn != null)
            //        {
            //            var includeResult = MakeIncludes(foundTreeItem, visibleColumn.Caption, false);
            //            if (includeResult != null && includeResult.LookupJoin != null)
            //            {
            //                newLookupColumnDefinition = includeResult.LookupJoin.AddVisibleColumnDefinition(lookupFormulaColumn.Caption,
            //                    lookupFormulaColumn.Formula, lookupFormulaColumn.PercentWidth,
            //                    lookupFormulaColumn.DataType);
            //                newLookupFormulaColumnDefinition = newLookupColumnDefinition as LookupFormulaColumnDefinition;
            //            }
            //        }
            //        else
            //        {
            //            var includeResult = MakeIncludes(foundTreeItem, visibleColumn.Caption, createColumn, visibleColumn.PercentWidth);
            //            newLookupColumnDefinition = includeResult.ColumnDefinition;
            //        }
            //    }


            //    if (newLookupFormulaColumnDefinition != null && lookupFormulaColumn != null)
            //    {
            //        newLookupColumnDefinition = newLookupFormulaColumnDefinition;
            //        newLookupFormulaColumnDefinition.DecimalFieldType = lookupFormulaColumn.DecimalFieldType;

            //    }

            //    if (visibleColumn.ContentTemplateId != null)
            //        newLookupColumnDefinition.HasContentTemplateId((int)visibleColumn.ContentTemplateId);

            //    newLookupColumnDefinition.DoShowNegativeValuesInRed(visibleColumn.ShowNegativeValuesInRed);
            //    newLookupColumnDefinition.DoShowPositiveValuesInGreen(visibleColumn.ShowPositiveValuesInGreen);
            //}
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

            //if (lookupDefinition.InitialOrderByColumn != lookupDefinition.InitialSortColumnDefinition)
            //{
            //    var initialSortColumnIndex = lookupDefinition.VisibleColumns.ToList()
            //        .IndexOf(lookupDefinition.InitialOrderByColumn);
            //    if (initialSortColumnIndex != -1)
            //    {
            //        LookupDefinition.InitialOrderByColumn = LookupDefinition.VisibleColumns[initialSortColumnIndex];
            //    }
            //}
            //LookupDefinition.InitialOrderByType = lookupDefinition.InitialOrderByType;
            //AddColumnCommand.IsEnabled = AddFilterCommand.IsEnabled = 
            //    AddFilterCommand.IsEnabled = ApplyToLookupCommand.IsEnabled = RefreshNowCommand.IsEnabled =
            //        ShowSqlCommand.IsEnabled = RefreshSettingsCommand.IsEnabled = false;
            ResetLookup();
        }

        public void ResetLookup(bool validate = true)
        {
            var valResult = true;
            if (validate)
            {
                valResult = ValidateLookup();
            }
            if (valResult)
            {
                var test = LookupDefinition;
                LookupCommand = GetLookupCommand(LookupCommands.Reset, null, AdvancedFindInput?.InputParameter);
                ProcessRefresh();
                PrintLookupOutputCommand.IsEnabled = RefreshNowCommand.IsEnabled = true;
            }
        }

        public void RefreshLookup(bool refreshCount = true)
        {
            if (ValidateLookup())
            {
                LookupCommand = GetLookupCommand(LookupCommands.Refresh, null, AdvancedFindInput?.InputParameter);
                if (refreshCount)
                {
                    ProcessRefresh();
                }
            }
        }

        private bool ValidateLookup()
        {
            if (Clearing)
                return true;

            if (FiltersManager.ValidateParentheses())
            {
                if (!FiltersManager.ValidateAdvancedFind())
                {
                    ClearLookup(false);
                    return false;
                }

                return true;
            }
            else
            {
                ClearLookup(false);
            }

            return false;
        }

        public void ClearLookup(bool clearColumns = true)
        {
            if (clearColumns)
            {
                var command = GetLookupCommand(LookupCommands.Reset, null, AdvancedFindInput?.InputParameter);
                command.ClearColumns = true;
                command.ResetSearchFor = true;
                LookupCommand = command;
            }
            else
            {
                LookupCommand = GetLookupCommand(LookupCommands.Clear);
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
                RecordDirty = true;
            }
        }

        public TreeViewItem FindFieldInTree(ObservableCollection<TreeViewItem> items, FieldDefinition fieldDefinition,
            bool searchForRootFormula = false, TreeViewItem parentItem = null)
        {
            return LookupDefinition.AdvancedFindTree.FindFieldInTree(items, fieldDefinition, searchForRootFormula, parentItem);
        }

        public TreeViewItem ProcessFoundTreeViewItem(string formula, FieldDefinition fieldDefinition,
            FieldDataTypes? fieldDataType = null, DecimalEditFormatTypes? decimalEditFormat = null)
        {
            return LookupDefinition.AdvancedFindTree.ProcessFoundTreeViewItem(formula, fieldDefinition, fieldDataType, decimalEditFormat);
        }

        private void ImportDefaultLookup()
        {
            var keyDown = Processor.IsMaintenanceKeyDown(MaintenanceKey.Alt);
            if (TableRow != null)
            {
                CreateLookupDefinition();
                var lookupDefinition= LookupDefinition.TableDefinition.LookupDefinition;
                if (lookupDefinition != null)
                {
                    RecordDirty = true;
                    LoadFromLookupDefinition(lookupDefinition);
                }
                else
                {
                    ControlsGlobals.UserInterface.ShowMessageBox("No default lookup for table set.",
                        "No Default Lookup", RsMessageBoxIcons.Exclamation);
                }
            }

            if (!keyDown)
            {
                View.ResetViewForNewRecord();
            }
        }

        private void ApplyToLookup()
        {
        //    var keyDown = Processor.IsMaintenanceKeyDown(MaintenanceKey.Alt);
        //    if (ValidateLookup())
        //    {
        //        LookupDefinition.TableDefinition.HasLookupDefinition(LookupDefinition);
        //        View.ApplyToLookup();
        //    }

        //    if (!keyDown)
        //    {
        //        View.ResetViewForNewRecord();
        //    }
        }

        private void ShowSql()
        {
        //    var keyDown = Processor.IsMaintenanceKeyDown(MaintenanceKey.Alt);
        //    if (ValidateLookup())
        //    {
        //        View.ShowSqlStatement();

        //    }

        //    if (!keyDown)
        //    {
        //        View.ResetViewForNewRecord();
        //        View.ResetViewForNewRecord();
        //    }
        }

        private void ShowRefreshSettings()
        {
            var keyDown = Processor.IsMaintenanceKeyDown(MaintenanceKey.Alt);
            var refreshSettings = GetEntityData();
            if (View.ShowRefreshSettings(refreshSettings))
            {
                RecordDirty = true;
                LoadRefreshSettings(refreshSettings);
                ResetLookup();
            }

            if (!keyDown)
            {
                View.ResetViewForNewRecord();
            }
        }

        private void RefreshNow()
        {
            if (ValidateLookup())
            {
                LookupCommand = GetLookupCommand(LookupCommands.Refresh, new PrimaryKeyValue(LookupDefinition.TableDefinition));
                ProcessRefresh();
            }
        }

        private void ProcessRefresh()
        {
            //var lookupMaui = LookupDefinition.TableDefinition.LookupDefinition
            //    .GetLookupDataMaui(LookupDefinition, 10);
            //lookupMaui.GetInitData();

            if (LookupRefresher.RefreshRate == RefreshRate.None)
            {
                return;
            }

            var lookupUi = new LookupUserInterface
            {
                PageSize = 1,
            };
            var lookupData = LookupDefinition.TableDefinition.LookupDefinition.GetLookupDataMaui(
                LookupDefinition, false);

            _recordCount = lookupData.GetRecordCount();
            if (_recordCount == 0 && LookupRefresher.RefreshRate == RefreshRate.None)
            {
                return;
            }

            LookupRefresher.UpdateRecordCount(_recordCount);
            RefreshLookup(false);
        }

        public override void OnWindowClosing(CancelEventArgs e)
        {
            LookupRefresher.Dispose();
            
            base.OnWindowClosing(e);
        }
    }
}