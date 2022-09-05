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
        private LookupDefinitionBase _lookupDefinition;

        public LookupDefinitionBase LookupDefinition
        {
            get => _lookupDefinition;
            set
            {
                _lookupDefinition = value;
                _includes.Clear();
            }
        }

        public ObservableCollection<TreeViewItem> TreeRoot { get; set; }

        public event EventHandler<TreeViewItem> SelectedTreeItemChanged;

        private List<LookupJoin> _includes = new List<LookupJoin>();

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
                    foundTreeViewItem.FormulaData.DataType = (FieldDataTypes)fieldDataType;
                }

                if (decimalEditFormat != null)
                {
                    foundTreeViewItem.FormulaData.DecimalFormatType = (DecimalEditFormatTypes)decimalEditFormat;
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
                    foundTreeViewItem.FormulaData.DataType = (FieldDataTypes)fieldDataType;
                    foundTreeViewItem.FormulaData.DecimalFormatType = (DecimalEditFormatTypes)decimalEditFormat;
                }
            }

            return foundTreeViewItem;
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
                                var column = includeJoin.AddVisibleColumnDefinition(columnCaption,
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
    }
}
