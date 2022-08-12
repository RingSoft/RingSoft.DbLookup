using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using RingSoft.DataEntryControls.Engine;
using RingSoft.DbLookup;
using RingSoft.DbLookup.AdvancedFind;
using RingSoft.DbLookup.AutoFill;
using RingSoft.DbLookup.Lookup;
using RingSoft.DbLookup.ModelDefinition;
using RingSoft.DbLookup.ModelDefinition.FieldDefinitions;

namespace RingSoft.DbMaintenance
{
    public enum TreeViewType
    {
        Field = 0,
        AdvancedFind = 1,
        Formula = 2,
        ForeignTable = 3
    }

    public class TreeViewItem : INotifyPropertyChanged
    {
        public string Name { get; set; }
        public TreeViewType Type { get; set; }
        public FieldDefinition FieldDefinition { get; set; }
        public ObservableCollection<TreeViewItem> Items { get; set; } = new ObservableCollection<TreeViewItem>();

        public AdvancedFindViewModel ViewModel { get; set; }

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
                ViewModel.OnTreeViewItemSelected(this);
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
                LoadTree();
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

        public RelayCommand AddColumnCommand { get; set; }

        public RelayCommand AddFilterCommand { get; set; }

        public TreeViewItem SelectedTreeViewItem { get; set; }

        protected override void Initialize()
        {
            AddColumnCommand = new RelayCommand(AddColumn);

            TableComboBoxSetup = new TextComboBoxControlSetup();
            var index = 0;
            foreach (var contextTableDefinition in SystemGlobals.AdvancedFindLookupContext.AdvancedFinds.Context
                         .TableDefinitions.OrderBy(p => p.EntityName))
            {
                TableComboBoxSetup.Items.Add(new TextComboBoxItem(){NumericValue = index, TextValue = contextTableDefinition.EntityName});
                index++;
            }
            base.Initialize();
        }

        protected override AdvancedFind PopulatePrimaryKeyControls(AdvancedFind newEntity, PrimaryKeyValue primaryKeyValue)
        {
            var advancedFind = SystemGlobals.AdvancedFindDbProcessor.GetAdvancedFind(newEntity.Id);

            KeyAutoFillValue = new AutoFillValue(primaryKeyValue, advancedFind.Name);

            return advancedFind;
        }

        protected override void LoadFromEntity(AdvancedFind entity)
        {
            AdvancedFindId = entity.Id;
            TableIndex =
                TableComboBoxSetup.Items.IndexOf(
                    TableComboBoxSetup.Items.FirstOrDefault(p => p.TextValue == entity.Table));
        }

        private void LoadTree()
        {
            if (SelectedTableBoxItem != null)
            {
                var tableDefinition =
                    SystemGlobals.AdvancedFindLookupContext.AdvancedFinds.Context.TableDefinitions.FirstOrDefault(p =>
                        p.EntityName == SelectedTableBoxItem.TextValue);

                LookupDefinition = new LookupDefinitionBase(tableDefinition);

            }

            var treeItems = new ObservableCollection<TreeViewItem>();
            if (TableIndex >= 0)
            {
                var table = SystemGlobals.AdvancedFindLookupContext.AdvancedFinds.Context.TableDefinitions
                    .FirstOrDefault(
                        f => f.EntityName == TableComboBoxSetup.Items[TableIndex].TextValue);
                var fields = table.FieldDefinitions;

                foreach (var field in fields)
                {
                    var treeRoot = new TreeViewItem();
                    treeRoot.Name = field.FieldName;
                    treeRoot.Type = TreeViewType.Field;
                    treeRoot.FieldDefinition = field;
                    treeRoot.ViewModel = this;
                    treeItems.Add(treeRoot);
                    if (field.ParentJoinForeignKeyDefinition != null && field.ParentJoinForeignKeyDefinition.PrimaryTable != null)
                        AddTreeItem(field.ParentJoinForeignKeyDefinition.PrimaryTable, treeRoot.Items);
                }

                LookupCommand = GetLookupCommand(LookupCommands.Refresh);

            }
            TreeRoot = treeItems;
        }

        private void AddTreeItem(TableDefinitionBase table,
            ObservableCollection<TreeViewItem> treeItems)
        {

            foreach (var tableFieldDefinition in table.FieldDefinitions)
            {
                var treeChildItem = new TreeViewItem();
                treeChildItem.Name = tableFieldDefinition.FieldName;
                treeChildItem.Type = TreeViewType.Field;
                treeChildItem.FieldDefinition = tableFieldDefinition;
                treeChildItem.ViewModel = this;
                treeItems.Add(treeChildItem);

                if (tableFieldDefinition.ParentJoinForeignKeyDefinition != null &&
                    tableFieldDefinition.ParentJoinForeignKeyDefinition.PrimaryTable != null)
                {
                    if(tableFieldDefinition.AllowRecursion)
                        AddTreeItem(tableFieldDefinition.ParentJoinForeignKeyDefinition.PrimaryTable, treeChildItem.Items);
                }
            }
        }

        public void OnTreeViewItemSelected(TreeViewItem treeViewItem)
        {
            SelectedTreeViewItem = treeViewItem;
        }


        protected override AdvancedFind GetEntityData()
        {
            var advancedFind = new AdvancedFind();
            advancedFind.Id = AdvancedFindId;
            advancedFind.Name = KeyAutoFillValue.Text;
            advancedFind.Table = TableComboBoxSetup.Items[TableIndex].TextValue;
            return advancedFind;
        }

        protected override void ClearData()
        {
            AdvancedFindId = 0;
            TableIndex = -1;
        }

        protected override bool SaveEntity(AdvancedFind entity)
        {
            return SystemGlobals.AdvancedFindDbProcessor.SaveAdvancedFind(entity, new List<AdvancedFindColumn>(),
                new List<AdvancedFindFilter>());
        }

        protected override bool DeleteEntity()
        {
            return SystemGlobals.AdvancedFindDbProcessor.DeleteAdvancedFind(AdvancedFindId);
        }

        private void AddColumn()
        {

            LookupDefinition.AddVisibleColumnDefinition(SelectedTreeViewItem.Name, SelectedTreeViewItem.FieldDefinition,
                20);

            LookupCommand = GetLookupCommand(LookupCommands.Reset);
        }
    }
}
