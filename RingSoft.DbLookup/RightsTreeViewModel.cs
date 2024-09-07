// ***********************************************************************
// Assembly         : RingSoft.DbLookup
// Author           : petem
// Created          : 05-21-2024
//
// Last Modified By : petem
// Last Modified On : 05-21-2024
// ***********************************************************************
// <copyright file="RightsTreeViewModel.cs" company="Peter Ringering">
//     2024
// </copyright>
// <summary></summary>
// ***********************************************************************
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using RingSoft.DataEntryControls.Engine;
using RingSoft.DbLookup.ModelDefinition;

namespace RingSoft.DbLookup
{
    /// <summary>
    /// Interface IRightsTreeControl
    /// </summary>
    public interface IRightsTreeControl
    {
        /// <summary>
        /// Sets the data changed.
        /// </summary>
        void SetDataChanged();
    }
    /// <summary>
    /// Class RightTreeViewItem.
    /// Implements the <see cref="INotifyPropertyChanged" />
    /// </summary>
    /// <seealso cref="INotifyPropertyChanged" />
    public class RightTreeViewItem : INotifyPropertyChanged
    {
        /// <summary>
        /// Gets or sets the parent.
        /// </summary>
        /// <value>The parent.</value>
        public RightTreeViewItem Parent { get; set; }

        /// <summary>
        /// Gets or sets the text.
        /// </summary>
        /// <value>The text.</value>
        public string Text { get; set; }

        /// <summary>
        /// Gets a value indicating whether [setting check].
        /// </summary>
        /// <value><c>true</c> if [setting check]; otherwise, <c>false</c>.</value>
        public bool SettingCheck { get; private set; }

        /// <summary>
        /// Gets or sets the type of the right.
        /// </summary>
        /// <value>The type of the right.</value>
        public RightTypes RightType { get; set; }

        /// <summary>
        /// Gets or sets the table definition.
        /// </summary>
        /// <value>The table definition.</value>
        public TableDefinitionBase TableDefinition { get; set; }

        /// <summary>
        /// Gets or sets the special right.
        /// </summary>
        /// <value>The special right.</value>
        public SpecialRight SpecialRight { get; set; }

        /// <summary>
        /// Returns a <see cref="System.String" /> that represents this instance.
        /// </summary>
        /// <returns>A <see cref="System.String" /> that represents this instance.</returns>
        public override string ToString()
        {
            return Text;
        }

        /// <summary>
        /// The is checked
        /// </summary>
        private bool? _isChecked;

        /// <summary>
        /// Gets or sets a value indicating whether this instance is checked.
        /// </summary>
        /// <value><c>null</c> if [is checked] contains no value, <c>true</c> if [is checked]; otherwise, <c>false</c>.</value>
        public bool? IsChecked
        {
            get => _isChecked;
            set
            {
                if (_isChecked == value)
                {
                    return;
                }

                RightsTreeViewModel?.SetDataChanged();
                ThreeState = value == null;
                _isChecked = value;
                OnPropertyChanged();
                CheckChanged?.Invoke(this, EventArgs.Empty);
                if (_isChecked != null)
                {
                    if (Items.Any())
                    {
                        SettingCheck = true;
                        foreach (var treeViewItem in Items)
                        {
                            treeViewItem.IsChecked = _isChecked;
                        }
                    }
                    if (Parent != null && !Parent.SettingCheck)
                    {
                        CheckParent();
                    }
                    SettingCheck = false;
                }
                else
                {
                    if (Parent != null)
                    {
                        Parent.IsChecked = _isChecked;
                    }
                }
            }
        }

        /// <summary>
        /// The three state
        /// </summary>
        private bool _threeState;

        /// <summary>
        /// Gets or sets a value indicating whether [three state].
        /// </summary>
        /// <value><c>true</c> if [three state]; otherwise, <c>false</c>.</value>
        public bool ThreeState
        {
            get => _threeState;
            set
            {
                if (_threeState == value)
                {
                    return;
                }
                _threeState = value;
                OnPropertyChanged();
            }
        }

        /// <summary>
        /// The allows edit
        /// </summary>
        private bool _allowsEdit;

        /// <summary>
        /// Gets or sets a value indicating whether [allows edit].
        /// </summary>
        /// <value><c>true</c> if [allows edit]; otherwise, <c>false</c>.</value>
        public bool AllowsEdit
        {
            get => _allowsEdit;
            set
            {
                if (_allowsEdit == value)
                {
                    return;
                }
                _allowsEdit = value;
                OnPropertyChanged();
            }
        }

        /// <summary>
        /// Gets the rights TreeView model.
        /// </summary>
        /// <value>The rights TreeView model.</value>
        public RightsTreeViewModel RightsTreeViewModel { get; }

        /// <summary>
        /// Occurs when [check changed].
        /// </summary>
        public event EventHandler CheckChanged;

        /// <summary>
        /// Initializes a new instance of the <see cref="RightTreeViewItem"/> class.
        /// </summary>
        /// <param name="text">The text.</param>
        /// <param name="isChecked">if set to <c>true</c> [is checked].</param>
        /// <param name="parent">The parent.</param>
        /// <param name="rightsTreeViewModel">The rights TreeView model.</param>
        public RightTreeViewItem(string text, bool? isChecked, RightTreeViewItem parent, RightsTreeViewModel rightsTreeViewModel)
        {
            Parent = parent;
            Text = text;
            IsChecked = isChecked;
            RightsTreeViewModel = rightsTreeViewModel;
        }

        /// <summary>
        /// Checks the parent.
        /// </summary>
        public void CheckParent()
        {
            var checkedItems = Parent.Items.Where(p => p.IsChecked == true
                                                       || p.ThreeState == true);

            var threeStateItems = Parent.Items.Where(p => p.ThreeState == true);

            var anyChecked = checkedItems.Any();
            if (anyChecked)
            {
                if (checkedItems.Count() == Parent.Items.Count)
                {
                    if (threeStateItems.Any())
                    {
                        Parent.IsChecked = null;
                    }
                    else
                    {
                        Parent.IsChecked = true;
                    }
                }
                else
                {
                    Parent.IsChecked = null;
                }
            }
            else
            {
                Parent.IsChecked = false;
            }
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="RightTreeViewItem"/> class.
        /// </summary>
        public RightTreeViewItem()
        {
            AllowsEdit = true;
        }

        /// <summary>
        /// Sets the read only mode.
        /// </summary>
        /// <param name="readOnlyMode">if set to <c>true</c> [read only mode].</param>
        public void SetReadOnlyMode(bool readOnlyMode)
        {
            AllowsEdit = !readOnlyMode;
            foreach (var treeViewItem in Items)
            {
                treeViewItem.SetReadOnlyMode(readOnlyMode);
            }
        }

        /// <summary>
        /// Gets or sets the items.
        /// </summary>
        /// <value>The items.</value>
        public ObservableCollection<RightTreeViewItem> Items { get; set; } = new ObservableCollection<RightTreeViewItem>();

        /// <summary>
        /// Occurs when a property value changes.
        /// </summary>
        public event PropertyChangedEventHandler? PropertyChanged;

        /// <summary>
        /// Called when [property changed].
        /// </summary>
        /// <param name="propertyName">Name of the property.</param>
        protected virtual void OnPropertyChanged([CallerMemberName] string? propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
    /// <summary>
    /// Class RightsTreeViewModel.
    /// Implements the <see cref="INotifyPropertyChanged" />
    /// </summary>
    /// <seealso cref="INotifyPropertyChanged" />
    public class RightsTreeViewModel : INotifyPropertyChanged
    {
        /// <summary>
        /// The tree root
        /// </summary>
        private ObservableCollection<RightTreeViewItem> _treeRoot;

        /// <summary>
        /// Gets or sets the tree root.
        /// </summary>
        /// <value>The tree root.</value>
        public ObservableCollection<RightTreeViewItem> TreeRoot
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
        /// The rights items
        /// </summary>
        private List<RightTreeViewItem> _rightsItems = new List<RightTreeViewItem>();
        /// <summary>
        /// The read only mode
        /// </summary>
        private bool? _readOnlyMode;
        /// <summary>
        /// The initialized
        /// </summary>
        private bool _initialized;
        /// <summary>
        /// The loaded rights
        /// </summary>
        private string _loadedRights;
        /// <summary>
        /// The rights loaded
        /// </summary>
        private bool _rightsLoaded;

        /// <summary>
        /// Gets the rights.
        /// </summary>
        /// <value>The rights.</value>
        public ItemRights Rights { get; }

        /// <summary>
        /// Gets the control.
        /// </summary>
        /// <value>The control.</value>
        public IRightsTreeControl Control { get; private set; }

        /// <summary>
        /// Initializes a new instance of the <see cref="RightsTreeViewModel"/> class.
        /// </summary>
        /// <exception cref="System.Exception">SystemGlobals.ItemRightsFactory not set.</exception>
        public RightsTreeViewModel()
        {
            if (SystemGlobals.ItemRightsFactory == null)
            {
                throw new Exception("SystemGlobals.ItemRightsFactory not set.");
            }
            Rights= SystemGlobals.ItemRightsFactory.GetNewItemRights();
        }

        /// <summary>
        /// Initializes the specified control.
        /// </summary>
        /// <param name="control">The control.</param>
        public void Initialize(IRightsTreeControl control)
        {
            _rightsLoaded = false;
            Control = control;
            TreeRoot = new ObservableCollection<RightTreeViewItem>();
            foreach (var category in Rights.Categories)
            {
                var categoryItem = new RightTreeViewItem(category.Category, false, null, this);
                TreeRoot.Add(categoryItem);

                foreach (var rightCategoryItem in category.Items)
                {
                    var right = Rights.GetRight(rightCategoryItem.TableDefinition);
                    var item = new RightTreeViewItem(rightCategoryItem.Description, false, categoryItem, this);
                    categoryItem.Items.Add(item);

                    var viewItem = new RightTreeViewItem("Allow View", right.AllowView, item, this)
                        {RightType = RightTypes.AllowView, TableDefinition = right.TableDefinition};
                    viewItem.CheckChanged += (sender, args) => right.AllowView = viewItem.IsChecked.Value;
                    right.AllowViewChanged += (sender, args) => viewItem.IsChecked = right.AllowView;
                    _rightsItems.Add(viewItem);
                    item.Items.Add(viewItem);

                    foreach (var rightSpecialRight in right.SpecialRights)
                    {
                        var specialRightItem = new RightTreeViewItem(rightSpecialRight.Description,
                            rightSpecialRight.HasRight, item, this);
                        specialRightItem.SpecialRight = rightSpecialRight;
                        right.AllowViewChanged += (sender, args) =>
                        {
                            if (!right.AllowView)
                            {
                                specialRightItem.IsChecked = right.AllowView;
                            }
                        };
                        right.AllowEditChanged += (sender, args) =>
                        {
                            specialRightItem.IsChecked = right.AllowEdit;
                        };
                        specialRightItem.CheckChanged += (sender, args) =>
                        {
                            specialRightItem.SpecialRight.HasRight = specialRightItem.IsChecked.Value;
                            if (specialRightItem.IsChecked.Value)
                            {
                                right.AllowView = true;
                            }
                        };
                        _rightsItems.Add(specialRightItem);
                        item.Items.Add(specialRightItem);
                    }

                    var editItem = new RightTreeViewItem("Allow Edit", right.AllowEdit, item, this)
                        {RightType = RightTypes.AllowEdit, TableDefinition = right.TableDefinition};
                    editItem.CheckChanged += (sender, args) => right.AllowEdit = editItem.IsChecked.Value;
                    right.AllowEditChanged += (sender, args) => editItem.IsChecked = right.AllowEdit;
                    _rightsItems.Add(editItem);
                    item.Items.Add(editItem);

                    var addItem = new RightTreeViewItem("Allow Add", right.AllowAdd, item, this) 
                        {RightType = RightTypes.AllowAdd, TableDefinition = right.TableDefinition};
                    addItem.CheckChanged += (sender, args) => right.AllowAdd = addItem.IsChecked.Value;
                    right.AllowAddChanged += (sender, args) => addItem.IsChecked = right.AllowAdd;
                    _rightsItems.Add(addItem);
                    item.Items.Add(addItem);

                    var deleteItem = new RightTreeViewItem("Allow Delete", right.AllowDelete, item, this)
                        {RightType = RightTypes.AllowDelete, TableDefinition = right.TableDefinition};
                    deleteItem.CheckChanged += (sender, args) => right.AllowDelete = deleteItem.IsChecked.Value;
                    right.AllowDeleteChanged += (sender, args) => deleteItem.IsChecked = right.AllowDelete;
                    _rightsItems.Add(deleteItem);
                    item.Items.Add(deleteItem);
                }
            }
            if (_readOnlyMode != null)
            {
                SetReadOnlyMode(_readOnlyMode.Value);
            }
            _initialized = true;
            if (!_loadedRights.IsNullOrEmpty())
            {
                LoadRights(_loadedRights);
            }

            _rightsLoaded = true;
        }

        /// <summary>
        /// Sets the read only mode.
        /// </summary>
        /// <param name="readOnlyValue">if set to <c>true</c> [read only value].</param>
        public void SetReadOnlyMode(bool readOnlyValue = true)
        {
            if (TreeRoot == null)
            {
                _readOnlyMode = readOnlyValue;
                return;
            }
            foreach (var treeViewItem in TreeRoot)
            {
                treeViewItem.SetReadOnlyMode(readOnlyValue);
            }
        }

        /// <summary>
        /// Resets this instance.
        /// </summary>
        public void Reset()
        {
            Rights.Reset();
            foreach (var rightsRight in Rights.Rights)
            {
                foreach (var specialRight in rightsRight.SpecialRights)
                {
                    var tableSpecialRight = _rightsItems.FirstOrDefault(p =>
                        p.SpecialRight == specialRight);
                    if (tableSpecialRight != null) tableSpecialRight.IsChecked = specialRight.HasRight;
                }
            }
        }

        /// <summary>
        /// Loads the rights.
        /// </summary>
        /// <param name="rightsString">The rights string.</param>
        /// <exception cref="System.ArgumentOutOfRangeException"></exception>
        public void LoadRights(string rightsString)
        {
            if (!_initialized)
            {
                _loadedRights = rightsString;
                return;
            }
            _rightsLoaded = false;
            Rights.LoadRights(rightsString);
            foreach (var rightsRight in Rights.Rights)
            {
                var tableRights = _rightsItems.Where(p => p.TableDefinition == rightsRight.TableDefinition);
                foreach (var tableRight in tableRights)
                {
                    switch (tableRight.RightType)
                    {
                        case RightTypes.AllowView:
                            tableRight.IsChecked = rightsRight.AllowView;
                            break;
                        case RightTypes.AllowAdd:
                            tableRight.IsChecked = rightsRight.AllowAdd;
                            break;
                        case RightTypes.AllowEdit:
                            tableRight.IsChecked = rightsRight.AllowEdit;
                            break;
                        case RightTypes.AllowDelete:
                            tableRight.IsChecked = rightsRight.AllowDelete;
                            break;
                        default:
                            throw new ArgumentOutOfRangeException();
                    }

                    foreach (var specialRight in rightsRight.SpecialRights)
                    {
                        var tableSpecialRight = _rightsItems.FirstOrDefault(p =>
                            p.SpecialRight == specialRight);
                        if (tableSpecialRight != null) tableSpecialRight.IsChecked = specialRight.HasRight;
                    }
                }
            }
            _rightsLoaded = true;
        }

        /// <summary>
        /// Sets the data changed.
        /// </summary>
        public void SetDataChanged()
        {
            if (_rightsLoaded)
            {
                Control.SetDataChanged();
            }
        }

        /// <summary>
        /// Occurs when a property value changes.
        /// </summary>
        public event PropertyChangedEventHandler? PropertyChanged;

        /// <summary>
        /// Called when [property changed].
        /// </summary>
        /// <param name="propertyName">Name of the property.</param>
        protected virtual void OnPropertyChanged([CallerMemberName] string? propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
