// ***********************************************************************
// Assembly         : RingSoft.DbMaintenance
// Author           : petem
// Created          : 12-19-2022
//
// Last Modified By : petem
// Last Modified On : 09-06-2024
// ***********************************************************************
// <copyright file="AdvancedFindViewModel.cs" company="Peter Ringering">
//     Copyright (c) 2023. All rights reserved.
// </copyright>
// <summary></summary>
// ***********************************************************************
using RingSoft.DataEntryControls.Engine;
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
    /// <summary>
    /// Interface IAdvancedFindView
    /// Extends the <see cref="RingSoft.DbMaintenance.IDbMaintenanceView" />
    /// </summary>
    /// <seealso cref="RingSoft.DbMaintenance.IDbMaintenanceView" />
    public interface IAdvancedFindView : IDbMaintenanceView
    {
        /// <summary>
        /// Shows the advanced filter window.
        /// </summary>
        /// <param name="treeViewItem">The tree view item.</param>
        /// <param name="lookupDefinition">The lookup definition.</param>
        /// <returns>AdvancedFilterReturn.</returns>
        AdvancedFilterReturn ShowAdvancedFilterWindow(TreeViewItem treeViewItem, LookupDefinitionBase lookupDefinition);

        /// <summary>
        /// Shows the filters ellipse.
        /// </summary>
        /// <param name="showFiltersEllipse">if set to <c>true</c> [show filters ellipse].</param>
        void ShowFiltersEllipse(bool showFiltersEllipse = true);

        /// <summary>
        /// Shows the refresh settings.
        /// </summary>
        /// <param name="advancedFind">The advanced find.</param>
        /// <returns><c>true</c> if XXXX, <c>false</c> otherwise.</returns>
        bool ShowRefreshSettings(AdvancedFind advancedFind);

        /// <summary>
        /// Sets the alert level.
        /// </summary>
        /// <param name="level">The level.</param>
        /// <param name="message">The message.</param>
        /// <param name="showCount">if set to <c>true</c> [show count].</param>
        /// <param name="recordCount">The record count.</param>
        void SetAlertLevel(AlertLevels level, string message, bool showCount, int recordCount);

        //void LockTable(bool lockValue);

        /// <summary>
        /// Gets the record count.
        /// </summary>
        /// <param name="showRecordCount">if set to <c>true</c> [show record count].</param>
        /// <returns>System.Int32.</returns>
        int GetRecordCount(bool showRecordCount);

        /// <summary>
        /// Sets the add on fly focus.
        /// </summary>
        void SetAddOnFlyFocus();

        /// <summary>
        /// Prints the output.
        /// </summary>
        /// <param name="printerSetup">The printer setup.</param>
        void PrintOutput(PrinterSetupArgs printerSetup);

        /// <summary>
        /// Checks the table is focused.
        /// </summary>
        void CheckTableIsFocused();

        /// <summary>
        /// Selects the filters tab.
        /// </summary>
        void SelectFiltersTab();

        /// <summary>
        /// Resets the lookup.
        /// </summary>
        void ResetLookup();

        PrimaryKeyValue GetSelectedPrimaryKeyValue();
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

    /// <summary>
    /// Class AdvancedFindInput.
    /// </summary>
    public class AdvancedFindInput
    {
        /// <summary>
        /// Gets or sets the input parameter.
        /// </summary>
        /// <value>The input parameter.</value>
        public object InputParameter { get; set; }
        /// <summary>
        /// Gets or sets the lock table.
        /// </summary>
        /// <value>The lock table.</value>
        public TableDefinitionBase LockTable { get; set; }
        /// <summary>
        /// Gets or sets the lookup definition.
        /// </summary>
        /// <value>The lookup definition.</value>
        public LookupDefinitionBase LookupDefinition { get; set; }
        /// <summary>
        /// Gets or sets the width of the lookup.
        /// </summary>
        /// <value>The width of the lookup.</value>
        public double LookupWidth { get; set; }

        public LookupDataMauiBase LookupData { get; set; }
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


    /// <summary>
    /// Class AdvancedFindViewModel.
    /// Implements the <see cref="RingSoft.DbMaintenance.DbMaintenanceViewModel{RingSoft.DbLookup.AdvancedFind.AdvancedFind}" />
    /// </summary>
    /// <seealso cref="RingSoft.DbMaintenance.DbMaintenanceViewModel{RingSoft.DbLookup.AdvancedFind.AdvancedFind}" />
    public class AdvancedFindViewModel : DbMaintenanceViewModel<AdvancedFind>
    {
        /// <summary>
        /// The advanced find identifier
        /// </summary>
        private int _advancedFindId;

        /// <summary>
        /// Gets or sets the advanced find identifier.
        /// </summary>
        /// <value>The advanced find identifier.</value>
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

        /// <summary>
        /// The table row
        /// </summary>
        private ListControlDataSourceRow _tableRow;

        /// <summary>
        /// Gets or sets the table row.
        /// </summary>
        /// <value>The table row.</value>
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

        /// <summary>
        /// The table setup
        /// </summary>
        private ListControlSetup _tableSetup;

        /// <summary>
        /// Gets or sets the table setup.
        /// </summary>
        /// <value>The table setup.</value>
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

        /// <summary>
        /// The table data source
        /// </summary>
        private ListControlDataSource _tableDataSource;

        /// <summary>
        /// Gets or sets the table data source.
        /// </summary>
        /// <value>The table data source.</value>
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


        /// <summary>
        /// The tree root
        /// </summary>
        private ObservableCollection<TreeViewItem> _treeRoot;

        /// <summary>
        /// Gets or sets the tree root.
        /// </summary>
        /// <value>The tree root.</value>
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

        /// <summary>
        /// The lookup definition
        /// </summary>
        private LookupDefinitionBase _lookupDefinition;

        /// <summary>
        /// Gets or sets the lookup definition.
        /// </summary>
        /// <value>The lookup definition.</value>
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

        /// <summary>
        /// The columns manager
        /// </summary>
        private AdvancedFindColumnsManager _columnsManager;

        /// <summary>
        /// Gets or sets the columns manager.
        /// </summary>
        /// <value>The columns manager.</value>
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

        /// <summary>
        /// The filters manager
        /// </summary>
        private AdvancedFindFiltersManager _filtersManager;

        /// <summary>
        /// Gets or sets the filters manager.
        /// </summary>
        /// <value>The filters manager.</value>
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



        /// <summary>
        /// Gets or sets the add column command.
        /// </summary>
        /// <value>The add column command.</value>
        public RelayCommand AddColumnCommand { get; set; }

        /// <summary>
        /// Gets or sets the add filter command.
        /// </summary>
        /// <value>The add filter command.</value>
        public RelayCommand AddFilterCommand { get; set; }

        /// <summary>
        /// Gets or sets the import default lookup command.
        /// </summary>
        /// <value>The import default lookup command.</value>
        public RelayCommand ImportDefaultLookupCommand { get; set; }

        /// <summary>
        /// Gets or sets the refresh settings command.
        /// </summary>
        /// <value>The refresh settings command.</value>
        public RelayCommand RefreshSettingsCommand { get; set; }

        /// <summary>
        /// Gets or sets the refresh now command.
        /// </summary>
        /// <value>The refresh now command.</value>
        public RelayCommand RefreshNowCommand { get; set; }

        /// <summary>
        /// Gets or sets the print lookup output command.
        /// </summary>
        /// <value>The print lookup output command.</value>
        public RelayCommand PrintLookupOutputCommand { get; set; }

        public RelayCommand SelectLookupRowCommand { get; }

        /// <summary>
        /// Gets or sets the table UI command.
        /// </summary>
        /// <value>The table UI command.</value>
        public UiCommand TableUiCommand { get; set; }

        /// <summary>
        /// Gets or sets the selected TreeView item.
        /// </summary>
        /// <value>The selected TreeView item.</value>
        public TreeViewItem SelectedTreeViewItem { get; set; }

        /// <summary>
        /// Gets the view.
        /// </summary>
        /// <value>The view.</value>
        public IAdvancedFindView View { get; set; }

        /// <summary>
        /// Gets or sets the advanced find input.
        /// </summary>
        /// <value>The advanced find input.</value>
        public AdvancedFindInput AdvancedFindInput { get; set; }

        /// <summary>
        /// Gets or sets the advanced find tree.
        /// </summary>
        /// <value>The advanced find tree.</value>
        public AdvancedFindTree AdvancedFindTree { get; set; }

        /// <summary>
        /// Gets the lookup refresher.
        /// </summary>
        /// <value>The lookup refresher.</value>
        public LookupRefresher LookupRefresher { get; private set; }

        public UiCommand SelectLookupRowUiCommand { get; }

        /// <summary>
        /// Gets a value indicating whether this <see cref="AdvancedFindViewModel" /> is clearing.
        /// </summary>
        /// <value><c>true</c> if clearing; otherwise, <c>false</c>.</value>
        public bool Clearing { get; private set; }

        public event EventHandler LookupCreated;

        /// <summary>
        /// The record count
        /// </summary>
        private int _recordCount;

        /// <summary>
        /// Initializes a new instance of the <see cref="AdvancedFindViewModel" /> class.
        /// </summary>
        public AdvancedFindViewModel()
        {
            SelectLookupRowUiCommand = new UiCommand();
            SelectLookupRowCommand = new RelayCommand(SelectLookupRow);
            TablesToDelete.Add(SystemGlobals.AdvancedFindLookupContext.AdvancedFindColumns);
            TablesToDelete.Add(SystemGlobals.AdvancedFindLookupContext.AdvancedFindFilters);

            CreateCommands();
            TableUiCommand = new UiCommand();
            MapFieldToUiCommand(TableUiCommand, TableDefinition.GetFieldDefinition(p => p.Table));
        }

        /// <summary>
        /// Initializes this instance.  Executed after the view is loaded.
        /// </summary>
        protected override void Initialize()
        {
            var origCursor = ControlsGlobals.UserInterface.GetWindowCursor();
            ControlsGlobals.UserInterface.SetWindowCursor(WindowCursorTypes.Wait);

            if (InputParameter != null)
            {
                if (InputParameter is AdvancedFindInput advancedFindInput)
                {
                    AdvancedFindInput = advancedFindInput;
                }
            }

            if (base.View is IAdvancedFindView view)
            {
                View = view;
            }

            //TableComboBoxSetup = new TextComboBoxControlSetup();
            //var index = 0;
            TableSetup = new ListControlSetup();
            TableDataSource = new ListControlDataSource();
            var dataColumn = TableSetup.AddColumn(1, "Table", FieldDataTypes.String, 95);
            var index = 1;

            foreach (var contextTableDefinition in SystemGlobals
                         .AdvancedFindLookupContext
                         .AdvancedFinds.Context
                         .TableDefinitions)
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

            //View.SetAlertLevel(AlertLevels.Green, "", true, 0);
            base.Initialize();

            SelectLookupRowUiCommand.Visibility = UiVisibilityTypes.Collapsed;
            SelectLookupRowCommand.IsEnabled = false;
            if (AdvancedFindInput != null)
            {
                if (AdvancedFindInput.LookupData != null)
                {
                    if (AdvancedFindInput.LookupData.LookupWindow != null
                        && !AdvancedFindInput.LookupData.LookupWindow.ReadOnlyMode)
                    {
                        SelectLookupRowUiCommand.Visibility = UiVisibilityTypes.Visible;
                    }
                }
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
                    View.SetAddOnFlyFocus();
                    RecordDirty = false;
                }
                else
                {
                    LockTableRow();
                }
            }
            ControlsGlobals.UserInterface.SetWindowCursor(origCursor);
        }

        /// <summary>
        /// Locks the table row.
        /// </summary>
        private void LockTableRow()
        {
            if (AdvancedFindInput != null)
            {
                TableRow = TableDataSource.Items.FirstOrDefault(p =>
                    p.DataCells[0].TextValue == AdvancedFindInput.LockTable.Description);
            }
        }

        /// <summary>
        /// Handles the RefreshRecordCountEvent event of the LookupRefresher control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="EventArgs" /> instance containing the event data.</param>
        private void LookupRefresher_RefreshRecordCountEvent(object sender, EventArgs e)
        {
            ProcessRefresh();
        }

        /// <summary>
        /// Lookups the refresher set alert level event.
        /// </summary>
        /// <param name="sender">The sender.</param>
        /// <param name="e">The e.</param>
        private void LookupRefresher_SetAlertLevelEvent(object sender, RefreshAlertLevelArgs e)
        {

            var message = LookupRefresher.GetRecordCountMessage(_recordCount, KeyAutoFillValue?.Text);
            View.SetAlertLevel(e.AlertLevel, message, LookupRefresher.RefreshRate != RefreshRate.None, _recordCount);
        }

        /// <summary>
        /// Creates the commands.
        /// </summary>
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

        /// <summary>
        /// Populates the primary key controls.  This is executed during record save and retrieval operations.
        /// </summary>
        /// <param name="newEntity">The entity containing just the primary key values.</param>
        /// <param name="primaryKeyValue">The primary key value.</param>
        /// <returns>An entity populated from the database.</returns>
        protected override void PopulatePrimaryKeyControls(AdvancedFind newEntity, PrimaryKeyValue primaryKeyValue)
        {
            AdvancedFindId = newEntity.Id;
        }

        /// <summary>
        /// Gets the entity from database.
        /// </summary>
        /// <param name="newEntity">The new entity.</param>
        /// <param name="primaryKeyValue">The primary key value.</param>
        /// <returns>AdvancedFind.</returns>
        /// <exception cref="System.ApplicationException"></exception>
        protected override AdvancedFind GetEntityFromDb(AdvancedFind newEntity,
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
            ReadOnlyMode = false;
            TableUiCommand.IsEnabled = false;
            RefreshSettingsCommand.IsEnabled = RefreshNowCommand.IsEnabled = true;
            return advancedFind;
        }

        /// <summary>
        /// Loads this view model from the entity generated from PopulatePrimaryKeyControls.  This is executed only during record retrieval operations.
        /// </summary>
        /// <param name="entity">The entity that was loaded from the database by PopulatePrimaryKeyControls.</param>
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

            View.ShowFiltersEllipse(FiltersManager.HasData);

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

        /// <summary>
        /// Loads the refresh settings.
        /// </summary>
        /// <param name="entity">The entity.</param>
        private void LoadRefreshSettings(AdvancedFind entity)
        {
            LookupRefresher.LoadFromAdvFind(entity);
            //ProcessRefresh();
            LookupRefresher.ResetTimer();
        }

        /// <summary>
        /// Loads the tree.
        /// </summary>
        /// <param name="tableName">Name of the table.</param>
        private void LoadTree(string tableName)
        {
            CreateLookupDefinition();

            ImportDefaultLookupCommand.IsEnabled = true;

            AdvancedFindTree.LoadTree(tableName);
            this.LookupDefinition.AdvancedFindTree = AdvancedFindTree;
            TreeRoot = AdvancedFindTree.TreeRoot;

            LookupDefinition.SetCommand(GetLookupCommand(LookupCommands.Clear));
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

        /// <summary>
        /// Called when [TreeView item selected].
        /// </summary>
        /// <param name="treeViewItem">The tree view item.</param>
        public void OnTreeViewItemSelected(TreeViewItem treeViewItem)
        {
            SelectedTreeViewItem = treeViewItem;
            if (treeViewItem != null)
            {
                AddColumnCommand.IsEnabled = SelectedTreeViewItem.Type != TreeViewType.AdvancedFind;
                AddFilterCommand.IsEnabled = true;
            }
        }

        /// <summary>
        /// Validates the entity.
        /// </summary>
        /// <param name="entity">The entity.</param>
        /// <returns><c>true</c> if XXXX, <c>false</c> otherwise.</returns>
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

        /// <summary>
        /// Gets the entity data.
        /// </summary>
        /// <returns>AdvancedFind.</returns>
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

        /// <summary>
        /// Clears the data.
        /// </summary>
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
            //var command = GetLookupCommand(LookupCommands.Reset, null, AdvancedFindInput?.InputParameter);
            //command.ClearColumns = true;
            //LookupDefinition.SetCommand(command);
            //}
            View.ResetLookup();

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

        /// <summary>
        /// Locks the table.
        /// </summary>
        private void LockTable()
        {
            var lockValue = AdvancedFindInput?.LockTable != null;
            //View.LockTable(lockValue);
            TableUiCommand.IsEnabled = !lockValue;
        }

        /// <summary>
        /// Clears the refresh.
        /// </summary>
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

        /// <summary>
        /// Creates the lookup definition.
        /// </summary>
        public void CreateLookupDefinition()
        {
            if (TableRow != null)
            {
                var tableDefinition =
                    SystemGlobals.AdvancedFindLookupContext.AdvancedFinds.Context.TableDefinitions.FirstOrDefault(p =>
                        p.Description == TableRow.GetCellItem(0));

                var oldLookup = LookupDefinition;
                LookupDefinition = new LookupDefinitionBase(tableDefinition);
                LookupCreated?.Invoke(this, EventArgs.Empty);
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

        /// <summary>
        /// Saves the entity.
        /// </summary>
        /// <param name="entity">The entity.</param>
        /// <returns><c>true</c> if XXXX, <c>false</c> otherwise.</returns>
        protected override bool SaveEntity(AdvancedFind entity)
        {
            var context = SystemGlobals.DataRepository.GetDataContext();
            var result = context.SaveEntity(entity, "Saving Advanced Find");
            if (result)
            {
                ColumnsManager.SaveNoCommitData(entity, context);
                //FiltersManager.SaveNoCommitData(entity, context);
                var table = context.GetTable<AdvancedFindFilter>();
                var existingFilters = table
                    .Where(p => p.AdvancedFindId == entity.Id);

                var newFilters = FiltersManager.GetEntityList();
                foreach (var filter in newFilters)
                {
                    filter.AdvancedFindId = entity.Id;
                }

                context.RemoveRange(existingFilters);
                context.AddRange(newFilters);

                result = context.Commit("Saving Advanced Find Details");
            }
            return result;
        }

        /// <summary>
        /// Deletes the entity.
        /// </summary>
        /// <returns><c>true</c> if XXXX, <c>false</c> otherwise.</returns>
        protected override bool DeleteEntity()
        {
            var context = SystemGlobals.DataRepository.GetDataContext();
            var table = context.GetTable<AdvancedFind>();
            var advFind = table
                .FirstOrDefault(p => p.Id == AdvancedFindId);
            if (advFind != null)
            {
                ColumnsManager.DeleteNoCommitData(advFind, context);
                FiltersManager.DeleteNoCommitData(advFind, context);
                return context.DeleteEntity(advFind, "Deleting Advanced Find");
            }
            return true;
        }

        /// <summary>
        /// Adds the column.
        /// </summary>
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

        /// <summary>
        /// Makes the includes.
        /// </summary>
        /// <param name="selectedItem">The selected item.</param>
        /// <returns>ProcessIncludeResult.</returns>
        public ProcessIncludeResult MakeIncludes(TreeViewItem selectedItem)
        {
            return LookupDefinition.AdvancedFindTree.MakeIncludes(selectedItem);
        }


        /// <summary>
        /// Loads from lookup definition.
        /// </summary>
        /// <param name="lookupDefinition">The lookup definition.</param>
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

        /// <summary>
        /// Resets the lookup.
        /// </summary>
        /// <param name="validate">if set to <c>true</c> [validate].</param>
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
                LookupDefinition.SetCommand(GetLookupCommand(LookupCommands.Reset, null, AdvancedFindInput?.InputParameter));
                ProcessRefresh();
                PrintLookupOutputCommand.IsEnabled = RefreshNowCommand.IsEnabled = true;
            }
        }

        /// <summary>
        /// Refreshes the lookup.
        /// </summary>
        /// <param name="refreshCount">if set to <c>true</c> [refresh count].</param>
        public void RefreshLookup(bool refreshCount = true)
        {
            if (ValidateLookup())
            {
                LookupDefinition.SetCommand(GetLookupCommand(LookupCommands.Refresh, null, AdvancedFindInput?.InputParameter));
                if (refreshCount)
                {
                    ProcessRefresh();
                }
            }
        }

        /// <summary>
        /// Validates the lookup.
        /// </summary>
        /// <returns><c>true</c> if XXXX, <c>false</c> otherwise.</returns>
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

        /// <summary>
        /// Clears the lookup.
        /// </summary>
        /// <param name="clearColumns">if set to <c>true</c> [clear columns].</param>
        public void ClearLookup(bool clearColumns = true)
        {
            if (clearColumns)
            {
                var command = GetLookupCommand(LookupCommands.Reset, null, AdvancedFindInput?.InputParameter);
                command.ClearColumns = true;
                command.ResetSearchFor = true;
                LookupDefinition.SetCommand(command);
            }
            else
            {
                LookupDefinition.SetCommand(GetLookupCommand(LookupCommands.Clear));
            }
        }

        /// <summary>
        /// Adds the filter.
        /// </summary>
        /// <exception cref="System.ArgumentOutOfRangeException"></exception>
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
        /// <summary>
        /// Shows the filter window.
        /// </summary>
        private void ShowFilterWindow()
        {
            var result = View.ShowAdvancedFilterWindow(SelectedTreeViewItem, LookupDefinition);
            if (result != null)
            {
                FiltersManager.LoadNewUserFilter(result);
                RecordDirty = true;
            }
        }

        /// <summary>
        /// Finds the field in tree.
        /// </summary>
        /// <param name="items">The items.</param>
        /// <param name="fieldDefinition">The field definition.</param>
        /// <param name="searchForRootFormula">if set to <c>true</c> [search for root formula].</param>
        /// <param name="parentItem">The parent item.</param>
        /// <returns>TreeViewItem.</returns>
        public TreeViewItem FindFieldInTree(ObservableCollection<TreeViewItem> items, FieldDefinition fieldDefinition,
            bool searchForRootFormula = false, TreeViewItem parentItem = null)
        {
            return LookupDefinition.AdvancedFindTree.FindFieldInTree(items, fieldDefinition, searchForRootFormula, parentItem);
        }

        /// <summary>
        /// Processes the found TreeView item.
        /// </summary>
        /// <param name="formula">The formula.</param>
        /// <param name="fieldDefinition">The field definition.</param>
        /// <param name="fieldDataType">Type of the field data.</param>
        /// <param name="decimalEditFormat">The decimal edit format.</param>
        /// <returns>TreeViewItem.</returns>
        public TreeViewItem ProcessFoundTreeViewItem(string formula, FieldDefinition fieldDefinition,
            FieldDataTypes? fieldDataType = null, DecimalEditFormatTypes? decimalEditFormat = null)
        {
            return LookupDefinition.AdvancedFindTree.ProcessFoundTreeViewItem(formula, fieldDefinition, fieldDataType, decimalEditFormat);
        }

        /// <summary>
        /// Imports the default lookup.
        /// </summary>
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

        /// <summary>
        /// Applies to lookup.
        /// </summary>
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

        /// <summary>
        /// Shows the SQL.
        /// </summary>
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

        /// <summary>
        /// Shows the refresh settings.
        /// </summary>
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

        /// <summary>
        /// Refreshes the now.
        /// </summary>
        private void RefreshNow()
        {
            if (ValidateLookup())
            {
                LookupDefinition.SetCommand(GetLookupCommand(LookupCommands.Refresh, new PrimaryKeyValue(LookupDefinition.TableDefinition)));
                ProcessRefresh();
            }
        }

        /// <summary>
        /// Processes the refresh.
        /// </summary>
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

        /// <summary>
        /// called when the user is trying to close the view.
        /// </summary>
        /// <param name="e">The <see cref="CancelEventArgs" /> instance containing the event data.</param>
        public override void OnWindowClosing(CancelEventArgs e)
        {
            LookupRefresher.Dispose();
            
            base.OnWindowClosing(e);
        }

        private void SelectLookupRow()
        {
            if (!CheckDirty())
            {
                return;
            }
            var primaryKey = View.GetSelectedPrimaryKeyValue();

            Processor.CloseWindow();

            AdvancedFindInput.LookupData.SelectPrimaryKey(primaryKey);
            AdvancedFindInput.LookupData.LookupWindow.OnSelectButtonClick();
        }
    }
}