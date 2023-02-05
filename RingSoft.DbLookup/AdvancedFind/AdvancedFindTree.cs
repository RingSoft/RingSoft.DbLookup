using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using MySqlX.XDevAPI.Common;
using MySqlX.XDevAPI.Relational;
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

        public string MakePath()
        {
            var result = string.Empty;
            if (FieldDefinition != null)
            {
                result = FieldDefinition.MakePath();
            }
            
            var parent = Parent;
            while (parent != null)
            {
                result = parent.FieldDefinition.MakePath() + result;
                parent = parent.Parent;
            }
            return result;
        }

        public LookupColumnDefinitionBase CreateColumn()
        {
            LookupColumnDefinitionBase result = null;
            switch (Type)
            {
                case TreeViewType.Field:
                    result = new LookupFieldColumnDefinition(FieldDefinition);
                    break;
                case TreeViewType.Formula:
                    var newFormula = string.Empty;
                    var formatType = FieldDataTypes.String;
                    if (FormulaData != null)
                    {
                        newFormula = FormulaData.Formula;
                        formatType = FormulaData.DataType;

                    }
                    result = new LookupFormulaColumnDefinition(newFormula, formatType);
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }
            var formula = result.LoadFromTreeViewItem(this);
            if (!formula.IsNullOrEmpty())
            {
                result = new LookupFormulaColumnDefinition(formula, FieldDataTypes.String);
                result.LoadFromTreeViewItem(this);
            }
            return result;
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
                        field.ParentJoinForeignKeyDefinition.PrimaryTable != null && field.AllowRecursion
                        && field.ParentJoinForeignKeyDefinition.PrimaryTable.CanViewTable)
                    {
                        if (field.ParentJoinForeignKeyDefinition.FieldJoins.Count == 1)
                        {
                            AddTreeItem(field.ParentJoinForeignKeyDefinition.PrimaryTable, treeRoot.Items,
                                field.ParentJoinForeignKeyDefinition, treeRoot);
                        }
                    }
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

                    if (tableFieldDefinition.AllowRecursion && tableFieldDefinition.TableDefinition.CanViewTable)
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

        public TreeViewItem FindAfInTree(FieldDefinition parentFieldDefinition)
        {
            return null;
        }

        public TreeViewItem ProcessFoundTreeViewItem(string path, TreeViewType type = TreeViewType.Field, TreeViewItem root = null)
        {
            TreeViewItem result = null;
            if (root != null)
            {
                result = root;
            }
            while (!path.IsNullOrEmpty())
            {
                var sepPos = path.IndexOf(";");
                var filePath = path.LeftStr(sepPos);

                var fieldDefinition = GetFieldDefinition(filePath);
                if (fieldDefinition != null)
                {
                    if (result == null)
                    {
                        result = TreeRoot.FirstOrDefault(p => p.FieldDefinition == fieldDefinition);
                    }
                    else
                    {
                        result = result.Items.FirstOrDefault(p => p.FieldDefinition == fieldDefinition);
                    }
                }
                path = path.RightStr(path.Length - (sepPos + 1));
            }

            switch (type)
            {
                case TreeViewType.Field:
                    break;
                default:
                    if (result.FieldDefinition.AllowRecursion)
                    {
                        result = result.Items.FirstOrDefault(p => p.Type == type);
                    }
                    break;
            }
            return result;
        }

        private FieldDefinition GetFieldDefinition(string path)
        {
            var sepPos = path.IndexOf("@");
            if (sepPos >= 0)
            {
                var tableName = path.LeftStr(sepPos);
                var fieldName = path.RightStr(path.Length - (sepPos + 1));

                var tableDefinition =
                    LookupDefinition.TableDefinition.Context.TableDefinitions.FirstOrDefault(p =>
                        p.TableName == tableName);

                if (tableDefinition != null)
                {
                    var fieldDefinition =
                        tableDefinition.FieldDefinitions.FirstOrDefault(p => p.FieldName == fieldName);
                    if (fieldDefinition != null)
                    {
                        return fieldDefinition;
                    }
                }
            }

            throw new Exception($"Could not find path '{path}'");
        }

        public TreeViewItem ProcessFoundTreeViewItem(string formula, FieldDefinition fieldDefinition,
            FieldDataTypes? fieldDataType = null, DecimalEditFormatTypes? decimalEditFormat = null)
        {
            var items = TreeRoot;
            if (!items.Any())
            {
                return null;
            }
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

        public ProcessIncludeResult MakeIncludes(TreeViewItem selectedItem)
        {
            var result = new ProcessIncludeResult();
            var childNodes = new List<TreeViewItem>();

            LookupJoin includeJoin = null;
            var parentTreeItem = selectedItem;
            while (parentTreeItem?.Parent != null)
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
                //if (createColumn && selectedItem != null)
                //{
                //    switch (selectedItem.Type)
                //    {
                //        case TreeViewType.Field:
                //            var processResult =
                //                SelectColumnDescription(selectedItem, selectedItem, null, columnCaption, percentWidth);
                //            includeJoin = ProcessInclude(includeJoin, processResult.LookupJoin);
                //            result.LookupJoin = includeJoin;
                //            result.ColumnDefinition = processResult.ColumnDefinition;
                //            break;
                //        case TreeViewType.Formula:
                //            var column = LookupDefinition.AddVisibleColumnDefinition(columnCaption,
                //                selectedItem.FormulaData.Formula, percentWidth, selectedItem.FormulaData.DataType, "");
                //            result.ColumnDefinition = column;

                //            break;
                //        default:
                //            throw new ArgumentOutOfRangeException();
                //    }


                //    if (result.ColumnDefinition is LookupFormulaColumnDefinition formulaColumn)
                //    {
                //        if (result.LookupJoin == null)
                //        {
                //            formulaColumn.PrimaryTable = LookupDefinition.TableDefinition;
                //        }
                //        else
                //        {
                //            formulaColumn.PrimaryTable =
                //                result.LookupJoin.JoinDefinition.ForeignKeyDefinition.ForeignTable;

                //            formulaColumn.PrimaryField =
                //                result.LookupJoin.JoinDefinition.ForeignKeyDefinition.FieldJoins[0].ForeignField;
                //        }
                //    }
                //}
            }

            foreach (var child in childNodes)
            {
                if (childNodes.IndexOf(child) == 0)
                {
                    if (child.FieldDefinition.ParentJoinForeignKeyDefinition != null)
                    {
                        if (child.FieldDefinition.ParentJoinForeignKeyDefinition.FieldJoins.Count == 1)
                        {
                            var newInclude = LookupDefinition.Include(child.FieldDefinition);
                            includeJoin = ProcessInclude(includeJoin, newInclude);
                            result.LookupJoin = includeJoin;
                        }
                    }
                }

                if (childNodes.IndexOf(child) == childNodes.Count - 1)
                {
                    if (childNodes.Count > 1)
                    {
                        var newInclude = includeJoin.Include(child.FieldDefinition);
                        includeJoin = ProcessInclude(includeJoin, newInclude);
                        result.LookupJoin = includeJoin;
                    }

                    //if (createColumn)
                    //{
                    //    switch (selectedItem.Type)
                    //    {
                    //        case TreeViewType.Field:
                    //            var processResult =
                    //                SelectColumnDescription(selectedItem, child, includeJoin, columnCaption, percentWidth);
                    //            includeJoin = ProcessInclude(includeJoin, processResult.LookupJoin);
                    //            result.LookupJoin = includeJoin;
                    //            result.ColumnDefinition = processResult.ColumnDefinition;
                    //            break;
                    //        case TreeViewType.Formula:
                    //            var column = includeJoin.AddVisibleColumnDefinition(columnCaption,
                    //               selectedItem.FormulaData.Formula, percentWidth, selectedItem.FormulaData.DataType);
                    //            result.ColumnDefinition = column;
                    //            break;
                    //        default:
                    //            throw new ArgumentOutOfRangeException();
                    //    }

                    //    if (result.ColumnDefinition is LookupFormulaColumnDefinition formulaColumn)
                    //    {
                    //        formulaColumn.PrimaryTable =
                    //            includeJoin.JoinDefinition.ForeignKeyDefinition.ForeignTable;

                    //        formulaColumn.PrimaryField =
                    //            includeJoin.JoinDefinition.ForeignKeyDefinition.FieldJoins[0].ForeignField;
                    //    }
                    //}
                }
                else if (childNodes.IndexOf(child) != 0)
                {
                    var newInclude = includeJoin.Include(child.FieldDefinition);
                    includeJoin = ProcessInclude(includeJoin, newInclude);
                    result.LookupJoin = includeJoin;
                }
            }

            //if (createColumn)
            //{
            //    SetTableField(selectedItem, result);
            //}
            return result;
        }

        private void SetTableField(TreeViewItem selectedItem, ProcessIncludeResult result)
        {
            result.ColumnDefinition.Path = selectedItem.MakePath();
            if (selectedItem.FieldDefinition != null && selectedItem.FieldDefinition.ParentJoinForeignKeyDefinition != null)
            {
                result.ColumnDefinition.FieldDescription = selectedItem.FieldDefinition.ParentJoinForeignKeyDefinition
                    .PrimaryTable.LookupDefinition.InitialSortColumnDefinition.Caption;
                result.ColumnDefinition.TableDescription = selectedItem.Name;
            }
            else
            {
                result.ColumnDefinition.FieldDescription = selectedItem.Name;
            }
            if (selectedItem.Parent == null)
            {
                if (result.ColumnDefinition.TableDescription.IsNullOrEmpty())
                {
                    result.ColumnDefinition.TableDescription = LookupDefinition.TableDefinition.Description;
                }
            }
            else
            {
                if (result.ColumnDefinition.TableDescription.IsNullOrEmpty())
                {
                    result.ColumnDefinition.TableDescription = selectedItem.Parent.Name;
                }
            }
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
            LookupJoin includeJoin, string caption, double percentWidth)
        {
            LookupColumnDefinitionBase column = null;
            if (selectedTreeViewItem.FieldDefinition.ParentJoinForeignKeyDefinition != null && selectedTreeViewItem.FieldDefinition.FieldDataType != FieldDataTypes.String)
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
                    column = includeJoin.AddVisibleColumnDefinition(caption, textField, percentWidth);
                }
                else if (selectedTreeViewItem.FieldDefinition.ParentJoinForeignKeyDefinition.FieldJoins[0]
                             .PrimaryField.TableDefinition.LookupDefinition
                             .InitialSortColumnDefinition is LookupFormulaColumnDefinition lookupFormulaColumn)
                {
                    //Corrupts column formula/alias
                    //lookupFormulaColumn.JoinQueryTableAlias = includeJoin.JoinDefinition.Alias;
                    var formula = lookupFormulaColumn.OriginalFormula;
                    column = includeJoin.AddVisibleColumnDefinition(caption,
                        formula, percentWidth, lookupFormulaColumn.DataType);
                }
            }
            else
            {
                if (includeJoin != null)
                {
                    column = includeJoin.AddVisibleColumnDefinition(caption,
                        selectedTreeViewItem.FieldDefinition,
                        percentWidth);
                }
                else
                {
                    var addColumnFromJoin = false;
                    if (child.FieldDefinition.ParentJoinForeignKeyDefinition != null)
                    {
                        if (child.FieldDefinition.ParentJoinForeignKeyDefinition.FieldJoins.Count == 1)
                        {
                            includeJoin = LookupDefinition.Include(child.FieldDefinition.ParentJoinForeignKeyDefinition
                                .FieldJoins[0].ForeignField);
                        }
                        else
                        {
                            addColumnFromJoin = false;
                        }

                        if (includeJoin != null)
                        {
                            column = includeJoin.AddVisibleColumnDefinition(caption, selectedTreeViewItem.FieldDefinition,
                                percentWidth);
                            addColumnFromJoin = true;
                        }
                    }
                    if (!addColumnFromJoin)
                    {
                        column = LookupDefinition.AddVisibleColumnDefinition(caption, child.FieldDefinition, percentWidth, "");
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
