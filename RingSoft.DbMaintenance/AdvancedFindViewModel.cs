using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using RingSoft.DataEntryControls.Engine;
using RingSoft.DbLookup;
using RingSoft.DbLookup.AdvancedFind;
using RingSoft.DbLookup.AutoFill;
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

    public class TreeViewItem
    {
        public string Name { get; set; }
        public TreeViewType Type { get; set; }
        public FieldDefinition FieldDefinition { get; set; }
        public ObservableCollection<TreeViewItem> Items { get; set; } = new ObservableCollection<TreeViewItem>();
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
                LoadTree();
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
                OnPropertyChanged();
            }
        }


        //private ObservableCollection<TreeViewItem> _treeViewItems;

        //public ObservableCollection<TreeViewItem> TreeViewItems
        //{
        //    get => _treeViewItems;
        //    set
        //    {
        //        if (_treeViewItems == value)
        //        {
        //            return;
        //        }
        //        _treeViewItems = value;
        //        OnPropertyChanged();
        //    }
        //}

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


        protected override void Initialize()
        {
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
                    treeItems.Add(treeRoot);
                    if (field.ParentJoinForeignKeyDefinition != null && field.ParentJoinForeignKeyDefinition.PrimaryTable != null)
                        AddTreeItem(field.ParentJoinForeignKeyDefinition.PrimaryTable, treeRoot.Items);
                }

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
                treeItems.Add(treeChildItem);

                if (tableFieldDefinition.ParentJoinForeignKeyDefinition != null &&
                    tableFieldDefinition.ParentJoinForeignKeyDefinition.PrimaryTable != null)
                {
                    if(tableFieldDefinition.AllowRecursion)
                        AddTreeItem(tableFieldDefinition.ParentJoinForeignKeyDefinition.PrimaryTable, treeChildItem.Items);
                }
            }
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
    }
}
