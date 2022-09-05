using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using RingSoft.DataEntryControls.Engine;
using RingSoft.DbLookup.Lookup;
using RingSoft.DbLookup.ModelDefinition;
using RingSoft.DbLookup.ModelDefinition.FieldDefinitions;

namespace RingSoft.DbLookup.AdvancedFind
{
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

    public class TreeViewItem : INotifyPropertyChanged
    {
        public string Name { get; set; }
        public TreeViewType Type { get; set; }
        public FieldDefinition FieldDefinition { get; set; }
        public ObservableCollection<TreeViewItem> Items { get; set; } = new ObservableCollection<TreeViewItem>();
        public ForeignKeyDefinition ParentJoin { get; set; }
        //public AdvancedFindViewModel ViewModel { get; set; }
        public LookupJoin Include { get; set; }
        public TreeViewItem Parent { get; set; }
        public TreeViewFormulaData FormulaData { get; set; }
        public AdvancedFindTree BaseTree  { get; set; }

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
                BaseTree.OnSelectedTreeItemChanged(_selectedTreeItem);
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


    public class AdvancedFindTree
    {
        public LookupDefinitionBase LookupDefinition { get; set; }

        public ObservableCollection<TreeViewItem> TreeRoot { get; set; }

        public event EventHandler<TreeViewItem> SelectedTreeItemChanged;

        public AdvancedFindTree(LookupDefinitionBase lookupDefinition)
        {
            LookupDefinition = lookupDefinition;
        }

        internal void OnSelectedTreeItemChanged(TreeViewItem selectedItem)
        {
            SelectedTreeItemChanged?.Invoke(this, selectedItem);
        }

        public void LoadTree(string tableName)
        {
            var treeItems = new ObservableCollection<TreeViewItem>();

            if (!tableName.IsNullOrEmpty())
            {
                var table = SystemGlobals.AdvancedFindLookupContext.AdvancedFinds.Context.TableDefinitions
                    .FirstOrDefault(
                        f => f.TableName == tableName);
                var fields = table.FieldDefinitions;

                foreach (var field in fields.OrderBy(p => p.Description))
                {
                    var treeRoot = new TreeViewItem();
                    treeRoot.Name = field.Description;
                    treeRoot.Type = TreeViewType.Field;
                    treeRoot.FieldDefinition = field;
                    treeRoot.BaseTree = this;
                    treeItems.Add(treeRoot);
                    if (field.ParentJoinForeignKeyDefinition != null &&
                        field.ParentJoinForeignKeyDefinition.PrimaryTable != null)
                        AddTreeItem(field.ParentJoinForeignKeyDefinition.PrimaryTable, treeRoot.Items,
                            field.ParentJoinForeignKeyDefinition, treeRoot);
                }

                AddFormulaToTree(treeItems, null);
                AddAdvancedFindToTree(treeItems, null);

                

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
                //treeChildItem.ViewModel = this;
                treeChildItem.BaseTree = this;
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
            AddAdvancedFindToTree(treeItems, parent);
        }


        private void AddFormulaToTree(ObservableCollection<TreeViewItem> treeItems, TreeViewItem parent)
        {
            var formulaTreeItem = new TreeViewItem
            {
                Name = "<Formula>",
                Type = TreeViewType.Formula,
                Parent = parent
            };
            formulaTreeItem.BaseTree = this;
            treeItems.Add(formulaTreeItem);
        }

        private void AddAdvancedFindToTree(ObservableCollection<TreeViewItem> treeViewItems, TreeViewItem parent)
        {
            var result = new TreeViewItem
            {
                Name = "<Advanced Find>",
                Type = TreeViewType.AdvancedFind,
                //ViewModel = this,
                Parent = parent
            };
            result.BaseTree = this;

            treeViewItems.Add(result);
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

    }
}
