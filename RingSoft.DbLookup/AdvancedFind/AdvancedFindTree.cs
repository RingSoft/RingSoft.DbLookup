using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
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

        internal void OnSelectedTreeItemChanged(TreeViewItem selectedItem)
        {
            SelectedTreeItemChanged?.Invoke(this, selectedItem);
        }
    }
}
