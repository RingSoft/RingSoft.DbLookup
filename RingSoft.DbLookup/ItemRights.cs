using RingSoft.DataEntryControls.Engine;
using RingSoft.DbLookup.ModelDefinition;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System;
using System.Linq;

namespace RingSoft.DbLookup
{
    public enum RightTypes
    {
        AllowView = 0,
        AllowAdd = 1,
        AllowEdit = 2,
        AllowDelete = 3,
    }


    public class SpecialRight
    {
        public TableDefinitionBase TableDefinition { get; internal set; }

        public string Description { get; internal set; }

        public int RightId { get; internal set; }

        private bool _hasRight;

        public bool HasRight
        {
            get => _hasRight;
            set
            {
                if (_hasRight == value)
                {
                    return;
                }
                _hasRight = value;
            }
        }

        public SpecialRight()
        {

        }
    }
    public class Right : INotifyPropertyChanged
    {
        public TableDefinitionBase TableDefinition { get; set; }

        public List<SpecialRight> SpecialRights { get; private set; } = new List<SpecialRight>();

        private bool _allowDelete;

        public bool AllowDelete
        {
            get => _allowDelete;
            set
            {
                if (_allowDelete == value)
                    return;

                _allowDelete = value;
                OnPropertyChanged();
                AllowDeleteChanged?.Invoke(this, EventArgs.Empty);
                if (AllowDelete)
                {
                    AllowAdd = AllowDelete;
                }
            }
        }

        private bool _allowAdd;

        public bool AllowAdd
        {
            get => _allowAdd;
            set
            {
                if (_allowAdd == value)
                {
                    return;
                }
                _allowAdd = value;
                OnPropertyChanged();
                AllowAddChanged?.Invoke(this, EventArgs.Empty);
                if (AllowAdd)
                {
                    AllowEdit = AllowAdd;
                }
                else
                {
                    AllowDelete = AllowAdd;
                }
            }
        }
        private bool _allowEdit;

        public bool AllowEdit
        {
            get => _allowEdit;
            set
            {
                if (AllowEdit == value)
                {
                    return;
                }
                _allowEdit = value;
                OnPropertyChanged();
                AllowEditChanged?.Invoke(this, EventArgs.Empty);
                if (AllowEdit)
                {
                    AllowView = AllowEdit;
                }
                else
                {
                    AllowAdd = AllowEdit;
                }
            }
        }

        private bool _allowView;

        public bool AllowView
        {
            get => _allowView;
            set
            {
                if (_allowView == value)
                {
                    return;
                }
                _allowView = value;
                OnPropertyChanged();
                AllowViewChanged?.Invoke(this, EventArgs.Empty);
                if (!AllowView)
                {
                    AllowEdit = AllowView;
                }
            }
        }

        public event EventHandler AllowViewChanged;
        public event EventHandler AllowEditChanged;
        public event EventHandler AllowAddChanged;
        public event EventHandler AllowDeleteChanged;

        public Right()
        {
            AllowDelete = true;
        }

        public void AddSpecialRight(SpecialRight specialRight)
        {
            specialRight.TableDefinition = TableDefinition;
            specialRight.HasRight = true;
            SpecialRights.Add(specialRight);
        }
        public override string ToString()
        {
            return TableDefinition.ToString();
        }

        public event PropertyChangedEventHandler? PropertyChanged;

        protected virtual void OnPropertyChanged([CallerMemberName] string? propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }

    public class RightCategory
    {
        public string Category { get; private set; }

        public int MenuCategoryId { get; private set; }

        public List<RightCategoryItem> Items { get; private set; } = new List<RightCategoryItem>();

        public RightCategory(string name, int menuCategoryId)
        {
            MenuCategoryId = menuCategoryId;
            Category = name;
        }
    }

    public class RightCategoryItem
    {
        public RightCategoryItem(string item, TableDefinitionBase tableDefinition)
        {
            Description = item;
            TableDefinition = tableDefinition;
        }

        public string Description { get; set; }

        public TableDefinitionBase TableDefinition { get; set; }
    }

    public abstract class ItemRights
    {
        public ObservableCollection<Right> Rights { get; set; }

        public List<SpecialRight> SpecialRights { get; private set; } = new List<SpecialRight>();


        public List<RightCategory> Categories { get; set; } = new List<RightCategory>();

        public ItemRights()
        {
            Categories = new List<RightCategory>();
            SetupRightsTree();

            Initialize();
        }

        public abstract void SetupRightsTree();

        private void Initialize()
        {
            Rights = new ObservableCollection<Right>();
            var tables = SystemGlobals.LookupContext.TableDefinitions
                .OrderByDescending(p => p.IsAdvancedFind).ToList();
            foreach (var tableDefinition in tables)
            {
                if (tableDefinition.PrimaryKeyFields[0].ParentJoinForeignKeyDefinition == null)
                {
                    var right = new Right
                    {
                        TableDefinition = tableDefinition,
                    };
                    var specialRights = SpecialRights.Where(p => p.TableDefinition == tableDefinition);
                    foreach (var specialRight in specialRights)
                    {
                        right.AddSpecialRight(specialRight);
                    }
                    Rights.Add(right);
                }
            }
        }

        public void AddSpecialRight(int specialRightId, string description, TableDefinitionBase tableDefinition)
        {
            SpecialRights.Add(new SpecialRight
            {
                TableDefinition = tableDefinition,
                RightId = specialRightId,
                Description = description,
            });
        }
        public void Reset()
        {
            foreach (var right in Rights)
            {
                right.AllowDelete = true;
                foreach (var specialRight in right.SpecialRights)
                {
                    specialRight.HasRight = true;
                }
            }
        }

        public Right GetRight(TableDefinitionBase tableDefinition)
        {
            var right = Rights.FirstOrDefault(p => p.TableDefinition == tableDefinition);
            return right;
        }

        public SpecialRight GetSpecialRight(TableDefinitionBase tableDefinition, int rightType)
        {
            var right = SpecialRights.FirstOrDefault(p => p.TableDefinition == tableDefinition
                                                          && p.RightId == rightType);
            return right;
        }

        public bool HasRight(TableDefinitionBase tableDefinition, RightTypes rightType)
        {
            if (tableDefinition.PrimaryKeyFields[0].ParentJoinForeignKeyDefinition != null)
            {
                tableDefinition = tableDefinition.PrimaryKeyFields[0].ParentJoinForeignKeyDefinition.PrimaryTable;
            }
            var right = GetRight(tableDefinition);
            switch (rightType)
            {
                case RightTypes.AllowView:
                    return right.AllowView;
                case RightTypes.AllowAdd:
                    return right.AllowAdd;
                case RightTypes.AllowEdit:
                    return right.AllowEdit;
                case RightTypes.AllowDelete:
                    return right.AllowDelete;
                default:
                    throw new ArgumentOutOfRangeException(nameof(rightType), rightType, null);
            }
        }

        public bool HasSpecialRight(TableDefinitionBase tableDefinition, int rightType)
        {
            var right = GetSpecialRight(tableDefinition, rightType);
            if (right != null)
            {
                return right.HasRight;
            }
            return false;
        }

        public string GetRightsString()
        {
            var specialRightsBits = string.Empty;

            var result = string.Empty;

            foreach (var right in Rights)
            {
                result += $"@{right.TableDefinition.TableName}";

                var rightBit = "0";
                if (right.AllowDelete)
                {
                    rightBit = "1";
                }
                result += rightBit;

                rightBit = "0";
                if (right.AllowAdd)
                {
                    rightBit = "1";
                }
                result += rightBit;

                rightBit = "0";
                if (right.AllowEdit)
                {
                    rightBit = "1";
                }
                result += rightBit;

                rightBit = "0";
                if (right.AllowView)
                {
                    rightBit = "1";
                }
                result += rightBit;

                foreach (var specialRight in right.SpecialRights)
                {
                    rightBit = specialRight.HasRight ? "1" : "0";
                    specialRightsBits += rightBit;
                }
                result += specialRightsBits;
                specialRightsBits = string.Empty;
            }
            return result;
        }

        public void LoadRights(string rightsString)
        {
            Reset();
            if (rightsString.IsNullOrEmpty())
            {
                return;
            }

            foreach (var right in Rights)
            {
                var tableRights = rightsString;
                var tableRightsPrefix = $"@{right.TableDefinition.TableName}";
                var tableRightsPos = rightsString.IndexOf(tableRightsPrefix);

                if (tableRightsPos >= 0)
                {
                    var endRight = rightsString.IndexOf("@", tableRightsPos + 1);
                    if (endRight >= 0)
                    {
                        tableRights = rightsString.Substring(tableRightsPos + tableRightsPrefix.Length
                            , endRight - (tableRightsPos + tableRightsPrefix.Length));
                    }
                    else
                    {
                        tableRights = rightsString.Substring(tableRightsPos + tableRightsPrefix.Length);
                    }
                }
                else
                {
                    continue;
                }

                if (tableRights.Length < 4)
                {
                    continue;
                }

                //var rightIndex = Rights.IndexOf(right);
                //var rightStringIndex = 0;
                //if (rightIndex > 0)
                //{
                //    rightStringIndex = rightIndex * 4;
                //}

                //if (rightStringIndex > tableRights.Length - 1)
                //{
                //    return;
                //}
                var counter = 0;
                while (counter < 4)
                {
                    var rightBit = tableRights[counter].ToString();
                    if (counter == 0)
                    {
                        right.AllowDelete = rightBit.ToBool();
                    }
                    else if (counter == 1)
                    {
                        right.AllowAdd = rightBit.ToBool();
                    }
                    else if (counter == 2)
                    {
                        right.AllowEdit = rightBit.ToBool();
                    }
                    else if (counter == 3)
                    {
                        right.AllowView = rightBit.ToBool();
                    }

                    counter++;
                }

                var specialBitIndex = counter;
                if (tableRights.Length < right.SpecialRights.Count + counter)
                {
                    continue;
                }
                foreach (var specialRight in right.SpecialRights)
                {
                    //var search = $"@{right.TableDefinition.TableName}";
                    //var beginningPos = rightsString.IndexOf(search);
                    //if (beginningPos != -1)
                    {
                        //var beginningRight = rightsString.GetRightText(beginningPos + 1, 0);
                        //var endingPos = beginningRight.IndexOf("@");
                        //if (endingPos != -1)
                        //{
                        //    beginningRight = beginningRight.LeftStr(endingPos);
                        //}
                        //var tablePos = beginningRight.IndexOf(right.TableDefinition.TableName);
                        //beginningRight = beginningRight.GetRightText(tablePos
                        //    , right.TableDefinition.TableName.Length);
                        //if (specialBitIndex < beginningRight.Length)
                        {
                            var specialRightChar = tableRights[specialBitIndex];
                            specialRight.HasRight = specialRightChar.ToString().ToBool();
                        }

                        specialBitIndex++;
                    }
                }

            }
        }
    }

    public class AppRights
    {
        public ItemRights UserRights { get; set; }

        public List<ItemRights> GroupRights { get; private set; }

        public AppRights(ItemRights userRights)
        {
            UserRights = userRights;

            GroupRights = new List<ItemRights>();
        }

        public bool HasRight(TableDefinitionBase tableDefinition, RightTypes rightType)
        {
            return UserRights.HasRight(tableDefinition, rightType) ||
                   GroupRights.Any(p => p.HasRight(tableDefinition, rightType));
        }

        public bool HasSpecialRight(TableDefinitionBase tableDefinition, int rightType)
        {
            return UserRights.HasSpecialRight(tableDefinition, rightType) ||
                   GroupRights.Any(p => p.HasSpecialRight(tableDefinition, rightType));
        }
    }
}
