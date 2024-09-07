// ***********************************************************************
// Assembly         : RingSoft.DbLookup
// Author           : petem
// Created          : 12-19-2022
//
// Last Modified By : petem
// Last Modified On : 12-20-2023
// ***********************************************************************
// <copyright file="AdvancedFindTree.cs" company="Peter Ringering">
//     Copyright (c) 2023. All rights reserved.
// </copyright>
// <summary></summary>
// ***********************************************************************
using RingSoft.DataEntryControls.Engine;
using RingSoft.DbLookup.Lookup;
using RingSoft.DbLookup.ModelDefinition;
using RingSoft.DbLookup.ModelDefinition.FieldDefinitions;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;

namespace RingSoft.DbLookup.AdvancedFind
{
    /// <summary>
    /// Enum TreeViewType
    /// </summary>
    public enum TreeViewType
    {
        /// <summary>
        /// The field
        /// </summary>
        Field = 0,
        /// <summary>
        /// The advanced find
        /// </summary>
        AdvancedFind = 1,
        /// <summary>
        /// The formula
        /// </summary>
        Formula = 2,
        /// <summary>
        /// The foreign table
        /// </summary>
        ForeignTable = 3
    }

    /// <summary>
    /// Class TreeViewFormulaData.
    /// </summary>
    public class TreeViewFormulaData
    {
        /// <summary>
        /// Gets or sets the formula.
        /// </summary>
        /// <value>The formula.</value>
        public string Formula { get; set; }

        /// <summary>
        /// Gets or sets the type of the data.
        /// </summary>
        /// <value>The type of the data.</value>
        public FieldDataTypes DataType { get; set; }

        /// <summary>
        /// Gets or sets the type of the decimal format.
        /// </summary>
        /// <value>The type of the decimal format.</value>
        public DecimalEditFormatTypes DecimalFormatType { get; set; }
    }

    /// <summary>
    /// Class ProcessIncludeResult.
    /// </summary>
    public class ProcessIncludeResult
    {
        /// <summary>
        /// Gets or sets the lookup join.
        /// </summary>
        /// <value>The lookup join.</value>
        public LookupJoin LookupJoin { get; set; }
        /// <summary>
        /// Gets or sets the column definition.
        /// </summary>
        /// <value>The column definition.</value>
        public LookupColumnDefinitionBase ColumnDefinition { get; set; }
    }

    /// <summary>
    /// Class TreeViewItem.
    /// Implements the <see cref="INotifyPropertyChanged" />
    /// </summary>
    /// <seealso cref="INotifyPropertyChanged" />
    public class TreeViewItem : INotifyPropertyChanged
    {
        /// <summary>
        /// Gets or sets the name.
        /// </summary>
        /// <value>The name.</value>
        public string Name { get; set; }
        /// <summary>
        /// Gets or sets the type.
        /// </summary>
        /// <value>The type.</value>
        public TreeViewType Type { get; set; }
        /// <summary>
        /// Gets or sets the field definition.
        /// </summary>
        /// <value>The field definition.</value>
        public FieldDefinition FieldDefinition { get; set; }
        /// <summary>
        /// Gets or sets the items.
        /// </summary>
        /// <value>The items.</value>
        public ObservableCollection<TreeViewItem> Items { get; set; } = new ObservableCollection<TreeViewItem>();
        /// <summary>
        /// Gets or sets the parent join.
        /// </summary>
        /// <value>The parent join.</value>
        public ForeignKeyDefinition ParentJoin { get; set; }
        //public AdvancedFindViewModel ViewModel { get; set; }
        /// <summary>
        /// Gets or sets the include.
        /// </summary>
        /// <value>The include.</value>
        public LookupJoin Include { get; set; }
        /// <summary>
        /// Gets or sets the parent.
        /// </summary>
        /// <value>The parent.</value>
        public TreeViewItem Parent { get; set; }
        /// <summary>
        /// Gets or sets the formula data.
        /// </summary>
        /// <value>The formula data.</value>
        public TreeViewFormulaData FormulaData { get; set; }
        /// <summary>
        /// Gets or sets the base tree.
        /// </summary>
        /// <value>The base tree.</value>
        public AdvancedFindTree BaseTree  { get; set; }

        /// <summary>
        /// The is selected
        /// </summary>
        private bool _isSelected;

        /// <summary>
        /// Gets or sets a value indicating whether this instance is selected.
        /// </summary>
        /// <value><c>true</c> if this instance is selected; otherwise, <c>false</c>.</value>
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

        /// <summary>
        /// The selected tree item
        /// </summary>
        private TreeViewItem _selectedTreeItem;

        /// <summary>
        /// Gets or sets the selected tree item.
        /// </summary>
        /// <value>The selected tree item.</value>
        public TreeViewItem SelectedTreeItem
        {
            get => _selectedTreeItem;
            set
            {
                _selectedTreeItem = value;
                BaseTree.OnSelectedTreeItemChanged(_selectedTreeItem);
            }
        }

        /// <summary>
        /// Returns a <see cref="System.String" /> that represents this instance.
        /// </summary>
        /// <returns>A <see cref="System.String" /> that represents this instance.</returns>
        public override string ToString()
        {
            return Name;
        }

        /// <summary>
        /// Makes the path.
        /// </summary>
        /// <returns>System.String.</returns>
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

        /// <summary>
        /// Creates the column.
        /// </summary>
        /// <param name="index">The index.</param>
        /// <returns>LookupColumnDefinitionBase.</returns>
        /// <exception cref="System.ArgumentOutOfRangeException"></exception>
        public LookupColumnDefinitionBase CreateColumn(int index = -1)
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
                    result = new LookupFormulaColumnDefinition(null, formatType);
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }
            result.ColumnIndexToAdd = index;
            var formula = result.LoadFromTreeViewItem(this);
            if (!formula.IsNullOrEmpty())
            {
                result = new LookupFormulaColumnDefinition(null, FieldDataTypes.String);
                result.ColumnIndexToAdd = index;
                result.LoadFromTreeViewItem(this);
            }
            return result;
        }
        /// <summary>
        /// Occurs when a property value changes.
        /// </summary>
        public event PropertyChangedEventHandler PropertyChanged;

        /// <summary>
        /// Called when [property changed].
        /// </summary>
        /// <param name="propertyName">Name of the property.</param>
        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }

    /// <summary>
    /// Class TreeViewItems.
    /// Implements the <see cref="System.Collections.Generic.List{RingSoft.DbLookup.AdvancedFind.TreeViewItem}" />
    /// </summary>
    /// <seealso cref="System.Collections.Generic.List{RingSoft.DbLookup.AdvancedFind.TreeViewItem}" />
    public class TreeViewItems : List<TreeViewItem>
    {
        /// <summary>
        /// Gets or sets the name.
        /// </summary>
        /// <value>The name.</value>
        public string Name { get; set; }
        /// <summary>
        /// Gets or sets the type.
        /// </summary>
        /// <value>The type.</value>
        public TreeViewType Type { get; set; }
        /// <summary>
        /// Gets or sets the field definition.
        /// </summary>
        /// <value>The field definition.</value>
        public FieldDefinition FieldDefinition { get; set; }
        /// <summary>
        /// Gets or sets the items.
        /// </summary>
        /// <value>The items.</value>
        public ObservableCollection<TreeViewItem> Items { get; set; } = new ObservableCollection<TreeViewItem>();
    }


    /// <summary>
    /// Class AdvancedFindTree.
    /// </summary>
    public class AdvancedFindTree
    {
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
                _lookupDefinition = value;
                _includes.Clear();
            }
        }

        /// <summary>
        /// Gets or sets the tree root.
        /// </summary>
        /// <value>The tree root.</value>
        public ObservableCollection<TreeViewItem> TreeRoot { get; set; }

        /// <summary>
        /// Occurs when [selected tree item changed].
        /// </summary>
        public event EventHandler<TreeViewItem> SelectedTreeItemChanged;

        /// <summary>
        /// The includes
        /// </summary>
        private List<LookupJoin> _includes = new List<LookupJoin>();

        /// <summary>
        /// Initializes a new instance of the <see cref="AdvancedFindTree" /> class.
        /// </summary>
        /// <param name="lookupDefinition">The lookup definition.</param>
        public AdvancedFindTree(LookupDefinitionBase lookupDefinition)
        {
            LookupDefinition = lookupDefinition;
        }

        /// <summary>
        /// Called when [selected tree item changed].
        /// </summary>
        /// <param name="selectedItem">The selected item.</param>
        internal void OnSelectedTreeItemChanged(TreeViewItem selectedItem)
        {
            SelectedTreeItemChanged?.Invoke(this, selectedItem);
        }

        /// <summary>
        /// Loads the tree.
        /// </summary>
        /// <param name="tableName">Name of the table.</param>
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
                        field.ParentJoinForeignKeyDefinition.PrimaryTable != null
                        && field.ParentJoinForeignKeyDefinition.PrimaryTable.CanViewTable)
                    {
                        if (field.ParentJoinForeignKeyDefinition.FieldJoins.Count == 1)
                        {
                            AddTreeItem(field.ParentJoinForeignKeyDefinition.PrimaryTable, treeRoot.Items,
                                field.ParentJoinForeignKeyDefinition, treeRoot);
                        }
                    }
                }

                //AddFormulaToTree(treeItems, null);
                AddAdvancedFindToTree(treeItems, null);

                

            }

            TreeRoot = treeItems;

        }

        /// <summary>
        /// Adds the tree item.
        /// </summary>
        /// <param name="table">The table.</param>
        /// <param name="treeItems">The tree items.</param>
        /// <param name="join">The join.</param>
        /// <param name="parent">The parent.</param>
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
                    tableFieldDefinition.ParentJoinForeignKeyDefinition.PrimaryTable != null
                    && tableFieldDefinition.ParentJoinForeignKeyDefinition.PrimaryTable != table)
                {
                    //treeChildItem.PrimaryFieldDefinition = tableFieldDefinition.ParentJoinForeignKeyDefinition
                    //    .FieldJoins[0].PrimaryField;

                    if (tableFieldDefinition.AllowRecursion && tableFieldDefinition.TableDefinition.CanViewTable)
                        AddTreeItem(tableFieldDefinition.ParentJoinForeignKeyDefinition.PrimaryTable,
                            treeChildItem.Items, join, treeChildItem);
                }
            }

            //AddFormulaToTree(treeItems, parent);
            //AddAdvancedFindToTree(treeItems, parent);
        }


        /// <summary>
        /// Adds the formula to tree.
        /// </summary>
        /// <param name="treeItems">The tree items.</param>
        /// <param name="parent">The parent.</param>
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

        /// <summary>
        /// Adds the advanced find to tree.
        /// </summary>
        /// <param name="treeViewItems">The tree view items.</param>
        /// <param name="parent">The parent.</param>
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
        /// <summary>
        /// Finds the field in tree.
        /// </summary>
        /// <param name="items">The items.</param>
        /// <param name="fieldDefinition">The field definition.</param>
        /// <param name="searchForRootFormula">if set to <c>true</c> [search for root formula].</param>
        /// <param name="parentItem">The parent item.</param>
        /// <param name="searchSubs">if set to <c>true</c> [search subs].</param>
        /// <returns>TreeViewItem.</returns>
        public TreeViewItem FindFieldInTree(ObservableCollection<TreeViewItem> items, FieldDefinition fieldDefinition,
            bool searchForRootFormula = false, TreeViewItem parentItem = null, bool searchSubs = true)
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
                    else if (searchSubs)
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

        /// <summary>
        /// Finds the table in tree.
        /// </summary>
        /// <param name="tableDefinition">The table definition.</param>
        /// <param name="parentItem">The parent item.</param>
        /// <param name="checkKeys">if set to <c>true</c> [check keys].</param>
        /// <param name="parentField">The parent field.</param>
        /// <param name="fiterField">The fiter field.</param>
        /// <returns>TreeViewItem.</returns>
        public TreeViewItem FindTableInTree(TableDefinitionBase tableDefinition
            , TreeViewItem parentItem, bool checkKeys = true, FieldDefinition parentField = null
            , FieldDefinition fiterField = null)
        {
            TreeViewItem result = null;
            var items = parentItem.Items.ToList();
            foreach (var parentItemItem in items)
            {
                if (parentItemItem.Type == TreeViewType.Field)
                {
                    if (parentItemItem.FieldDefinition.TableDefinition == tableDefinition)
                    {
                        if (parentField != null)
                        {
                            if (parentItemItem.FieldDefinition != parentField)
                            {
                                continue;
                            }
                        }
                        return parentItemItem;
                    }

                    else if (parentItemItem.FieldDefinition.ParentJoinForeignKeyDefinition != null && checkKeys)
                    {
                        if (parentItemItem
                            .FieldDefinition
                            .ParentJoinForeignKeyDefinition
                            .PrimaryTable == tableDefinition)
                        {
                            if (parentField != null)
                            {
                                if (parentItemItem.FieldDefinition != parentField)
                                {
                                    continue;
                                }

                            }

                            return parentItemItem;
                        }
                    }
                }
            }

            if (result == null)
            {
                var subItems = parentItem.Items.ToList();
                foreach (var parentItemItem in subItems)
                {
                    if (fiterField != null)
                    {
                        if (parentItemItem.Type == TreeViewType.Field)
                        {
                            if (parentItemItem.FieldDefinition.ParentJoinForeignKeyDefinition != null
                                && fiterField.ParentJoinForeignKeyDefinition != null)
                            {
                                if (parentItemItem.FieldDefinition.ParentJoinForeignKeyDefinition.PrimaryTable
                                    == fiterField.ParentJoinForeignKeyDefinition.PrimaryTable)
                                {
                                    if (fiterField != parentItemItem.FieldDefinition)
                                    {
                                        continue;
                                    }
                                }
                            }
                        }
                    }

                    result = FindTableInTree(tableDefinition, parentItemItem, checkKeys, parentField, fiterField);
                    if (result != null)
                    {
                        return result;
                    }
                }
            }
            return null;
        }

        /// <summary>
        /// Finds the table in tree.
        /// </summary>
        /// <param name="tableDefinition">The table definition.</param>
        /// <returns>TreeViewItem.</returns>
        public TreeViewItem FindTableInTree(TableDefinitionBase tableDefinition)
        {
            TreeViewItem result = null;
            foreach (var parentItemItem in TreeRoot)
            {
                if (parentItemItem.Type == TreeViewType.Field)
                {
                    if (parentItemItem.FieldDefinition.TableDefinition == tableDefinition)
                    {
                        return parentItemItem;
                    }

                    else if (parentItemItem.FieldDefinition.ParentJoinForeignKeyDefinition != null)
                    {
                        if (parentItemItem
                                .FieldDefinition
                                .ParentJoinForeignKeyDefinition
                                .PrimaryTable == tableDefinition)
                        {
                            return parentItemItem;
                        }
                    }
                }
            }

            if (result == null)
            {
                foreach (var parentItemItem in TreeRoot)
                {
                    result = FindTableInTree(tableDefinition, parentItemItem);
                    if (result != null)
                    {
                        return result;
                    }
                }
            }
            return null;
        }


        /// <summary>
        /// Finds the af in tree.
        /// </summary>
        /// <param name="parentFieldDefinition">The parent field definition.</param>
        /// <returns>TreeViewItem.</returns>
        public TreeViewItem FindAfInTree(FieldDefinition parentFieldDefinition)
        {
            return null;
        }

        /// <summary>
        /// Processes the found TreeView item.
        /// </summary>
        /// <param name="path">The path.</param>
        /// <param name="type">The type.</param>
        /// <param name="root">The root.</param>
        /// <returns>TreeViewItem.</returns>
        public TreeViewItem ProcessFoundTreeViewItem(string path, TreeViewType type = TreeViewType.Field, TreeViewItem root = null)
        {
            TreeViewItem result = null;
            if (path == "Employees@ReportsTo;Employees@FullName;")
            {
                
            }

            if (path == "Order Details@OrderID;Orders@OrderName;")
            {
                
            }
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

                    if (fieldDefinition.ParentJoinForeignKeyDefinition == null)
                    {
                        if (fieldDefinition.PropertyName == "ReportsTo")
                        {

                        }

                        if (result != null && result.Parent != null)
                        {
                            var lookupJoin = new LookupJoin(LookupDefinition);
                            lookupJoin.ParentObject = result.Parent.Include;
                            result.Include = lookupJoin;
                        }

                    }
                }
                if (fieldDefinition != null && !fieldDefinition.AllowRecursion)
                {
                    return result;
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

        /// <summary>
        /// Gets the field definition.
        /// </summary>
        /// <param name="path">The path.</param>
        /// <returns>FieldDefinition.</returns>
        /// <exception cref="System.Exception">Could not find path '{path}'</exception>
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

            //throw new Exception($"Could not find path '{path}'");
            return null;
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

        /// <summary>
        /// Makes the includes.
        /// </summary>
        /// <param name="selectedItem">The selected item.</param>
        /// <returns>ProcessIncludeResult.</returns>
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

        /// <summary>
        /// Sets the table field.
        /// </summary>
        /// <param name="selectedItem">The selected item.</param>
        /// <param name="result">The result.</param>
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

        /// <summary>
        /// Processes the include.
        /// </summary>
        /// <param name="includeJoin">The include join.</param>
        /// <param name="newInclude">The new include.</param>
        /// <returns>LookupJoin.</returns>
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
        /// <summary>
        /// Selects the column description.
        /// </summary>
        /// <param name="selectedTreeViewItem">The selected TreeView item.</param>
        /// <param name="child">The child.</param>
        /// <param name="includeJoin">The include join.</param>
        /// <param name="caption">The caption.</param>
        /// <param name="percentWidth">Width of the percent.</param>
        /// <returns>ProcessIncludeResult.</returns>
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
                        lookupFormulaColumn.FormulaObject, percentWidth, lookupFormulaColumn.DataType);
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
