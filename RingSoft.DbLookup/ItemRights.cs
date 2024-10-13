// ***********************************************************************
// Assembly         : RingSoft.DbLookup
// Author           : petem
// Created          : 05-18-2024
//
// Last Modified By : petem
// Last Modified On : 05-18-2024
// ***********************************************************************
// <copyright file="ItemRights.cs" company="Peter Ringering">
//     2024
// </copyright>
// <summary></summary>
// ***********************************************************************
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
    /// <summary>
    /// Enum RightTypes
    /// </summary>
    public enum RightTypes
    {
        /// <summary>
        /// The allow view
        /// </summary>
        AllowView = 0,
        /// <summary>
        /// The allow edit
        /// </summary>
        AllowEdit = 1,
        /// <summary>
        /// The allow add
        /// </summary>
        AllowAdd = 2,
        /// <summary>
        /// The allow delete
        /// </summary>
        AllowDelete = 3,
    }


    /// <summary>
    /// Class SpecialRight.
    /// </summary>
    public class SpecialRight
    {
        /// <summary>
        /// Gets the table definition.
        /// </summary>
        /// <value>The table definition.</value>
        public TableDefinitionBase TableDefinition { get; internal set; }

        /// <summary>
        /// Gets the description.
        /// </summary>
        /// <value>The description.</value>
        public string Description { get; internal set; }

        /// <summary>
        /// Gets the right identifier.
        /// </summary>
        /// <value>The right identifier.</value>
        public int RightId { get; internal set; }

        /// <summary>
        /// The has right
        /// </summary>
        private bool _hasRight;

        /// <summary>
        /// Gets or sets a value indicating whether this instance has right.
        /// </summary>
        /// <value><c>true</c> if this instance has right; otherwise, <c>false</c>.</value>
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

        /// <summary>
        /// Initializes a new instance of the <see cref="SpecialRight"/> class.
        /// </summary>
        public SpecialRight()
        {

        }
    }
    /// <summary>
    /// Class Right.
    /// Implements the <see cref="INotifyPropertyChanged" />
    /// </summary>
    /// <seealso cref="INotifyPropertyChanged" />
    public class Right : INotifyPropertyChanged
    {
        /// <summary>
        /// Gets or sets the table definition.
        /// </summary>
        /// <value>The table definition.</value>
        public TableDefinitionBase TableDefinition { get; set; }

        /// <summary>
        /// Gets the special rights.
        /// </summary>
        /// <value>The special rights.</value>
        public List<SpecialRight> SpecialRights { get; private set; } = new List<SpecialRight>();

        /// <summary>
        /// The allow delete
        /// </summary>
        private bool _allowDelete;

        /// <summary>
        /// Gets or sets a value indicating whether [allow delete].
        /// </summary>
        /// <value><c>true</c> if [allow delete]; otherwise, <c>false</c>.</value>
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

        /// <summary>
        /// The allow add
        /// </summary>
        private bool _allowAdd;

        /// <summary>
        /// Gets or sets a value indicating whether [allow add].
        /// </summary>
        /// <value><c>true</c> if [allow add]; otherwise, <c>false</c>.</value>
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
        /// <summary>
        /// The allow edit
        /// </summary>
        private bool _allowEdit;

        /// <summary>
        /// Gets or sets a value indicating whether [allow edit].
        /// </summary>
        /// <value><c>true</c> if [allow edit]; otherwise, <c>false</c>.</value>
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

        /// <summary>
        /// The allow view
        /// </summary>
        private bool _allowView;

        /// <summary>
        /// Gets or sets a value indicating whether [allow view].
        /// </summary>
        /// <value><c>true</c> if [allow view]; otherwise, <c>false</c>.</value>
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

        /// <summary>
        /// Occurs when [allow view changed].
        /// </summary>
        public event EventHandler AllowViewChanged;
        /// <summary>
        /// Occurs when [allow edit changed].
        /// </summary>
        public event EventHandler AllowEditChanged;
        /// <summary>
        /// Occurs when [allow add changed].
        /// </summary>
        public event EventHandler AllowAddChanged;
        /// <summary>
        /// Occurs when [allow delete changed].
        /// </summary>
        public event EventHandler AllowDeleteChanged;

        /// <summary>
        /// Initializes a new instance of the <see cref="Right"/> class.
        /// </summary>
        public Right()
        {
            AllowDelete = true;
        }

        /// <summary>
        /// Adds the special right.
        /// </summary>
        /// <param name="specialRight">The special right.</param>
        public void AddSpecialRight(SpecialRight specialRight)
        {
            specialRight.TableDefinition = TableDefinition;
            specialRight.HasRight = true;
            SpecialRights.Add(specialRight);
        }
        /// <summary>
        /// Returns a <see cref="System.String" /> that represents this instance.
        /// </summary>
        /// <returns>A <see cref="System.String" /> that represents this instance.</returns>
        public override string ToString()
        {
            return TableDefinition.ToString();
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

    /// <summary>
    /// Class RightCategory.
    /// </summary>
    public class RightCategory
    {
        /// <summary>
        /// Gets the category.
        /// </summary>
        /// <value>The category.</value>
        public string Category { get; private set; }

        /// <summary>
        /// Gets the menu category identifier.
        /// </summary>
        /// <value>The menu category identifier.</value>
        public int MenuCategoryId { get; private set; }

        /// <summary>
        /// Gets the items.
        /// </summary>
        /// <value>The items.</value>
        public List<RightCategoryItem> Items { get; private set; } = new List<RightCategoryItem>();

        /// <summary>
        /// Initializes a new instance of the <see cref="RightCategory"/> class.
        /// </summary>
        /// <param name="name">The name.</param>
        /// <param name="menuCategoryId">The menu category identifier.</param>
        public RightCategory(string name, int menuCategoryId)
        {
            MenuCategoryId = menuCategoryId;
            Category = name;
        }
    }

    /// <summary>
    /// Class RightCategoryItem.
    /// </summary>
    public class RightCategoryItem
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="RightCategoryItem"/> class.
        /// </summary>
        /// <param name="item">The item.</param>
        /// <param name="tableDefinition">The table definition.</param>
        public RightCategoryItem(string item, TableDefinitionBase tableDefinition)
        {
            Description = item;
            TableDefinition = tableDefinition;
        }

        /// <summary>
        /// Gets or sets the description.
        /// </summary>
        /// <value>The description.</value>
        public string Description { get; set; }

        /// <summary>
        /// Gets or sets the table definition.
        /// </summary>
        /// <value>The table definition.</value>
        public TableDefinitionBase TableDefinition { get; set; }
    }

    /// <summary>
    /// Class ItemRights.
    /// </summary>
    public abstract class ItemRights
    {
        /// <summary>
        /// Gets or sets the rights.
        /// </summary>
        /// <value>The rights.</value>
        public ObservableCollection<Right> Rights { get; set; }

        /// <summary>
        /// Gets the special rights.
        /// </summary>
        /// <value>The special rights.</value>
        public List<SpecialRight> SpecialRights { get; private set; } = new List<SpecialRight>();


        /// <summary>
        /// Gets or sets the categories.
        /// </summary>
        /// <value>The categories.</value>
        public List<RightCategory> Categories { get; set; } = new List<RightCategory>();

        /// <summary>
        /// Initializes a new instance of the <see cref="ItemRights"/> class.
        /// </summary>
        public ItemRights()
        {
            Categories = new List<RightCategory>();
            SetupRightsTree();

            Initialize();
        }

        /// <summary>
        /// Setups the rights tree.
        /// </summary>
        public abstract void SetupRightsTree();

        /// <summary>
        /// Initializes this instance.
        /// </summary>
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

        /// <summary>
        /// Adds the special right.
        /// </summary>
        /// <param name="specialRightId">The special right identifier.</param>
        /// <param name="description">The description.</param>
        /// <param name="tableDefinition">The table definition.</param>
        public void AddSpecialRight(int specialRightId, string description, TableDefinitionBase tableDefinition)
        {
            SpecialRights.Add(new SpecialRight
            {
                TableDefinition = tableDefinition,
                RightId = specialRightId,
                Description = description,
            });
        }
        /// <summary>
        /// Resets this instance.
        /// </summary>
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

        /// <summary>
        /// Gets the right.
        /// </summary>
        /// <param name="tableDefinition">The table definition.</param>
        /// <returns>Right.</returns>
        public Right GetRight(TableDefinitionBase tableDefinition)
        {
            var right = Rights.FirstOrDefault(p => p.TableDefinition == tableDefinition);
            return right;
        }

        /// <summary>
        /// Gets the special right.
        /// </summary>
        /// <param name="tableDefinition">The table definition.</param>
        /// <param name="rightType">Type of the right.</param>
        /// <returns>SpecialRight.</returns>
        public SpecialRight GetSpecialRight(TableDefinitionBase tableDefinition, int rightType)
        {
            var right = SpecialRights.FirstOrDefault(p => p.TableDefinition == tableDefinition
                                                          && p.RightId == rightType);
            return right;
        }

        /// <summary>
        /// Determines whether the specified table definition has right.
        /// </summary>
        /// <param name="tableDefinition">The table definition.</param>
        /// <param name="rightType">Type of the right.</param>
        /// <returns><c>true</c> if the specified table definition has right; otherwise, <c>false</c>.</returns>
        /// <exception cref="System.ArgumentOutOfRangeException">rightType - null</exception>
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

        /// <summary>
        /// Determines whether [has special right] [the specified table definition].
        /// </summary>
        /// <param name="tableDefinition">The table definition.</param>
        /// <param name="rightType">Type of the right.</param>
        /// <returns><c>true</c> if [has special right] [the specified table definition]; otherwise, <c>false</c>.</returns>
        public bool HasSpecialRight(TableDefinitionBase tableDefinition, int rightType)
        {
            var right = GetSpecialRight(tableDefinition, rightType);
            if (right != null)
            {
                return right.HasRight;
            }
            return false;
        }

        /// <summary>
        /// Gets the rights string.
        /// </summary>
        /// <returns>System.String.</returns>
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

        /// <summary>
        /// Loads the rights.
        /// </summary>
        /// <param name="rightsString">The rights string.</param>
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

    /// <summary>
    /// Class AppRights.
    /// </summary>
    public class AppRights
    {
        /// <summary>
        /// Gets or sets the user rights.
        /// </summary>
        /// <value>The user rights.</value>
        public ItemRights UserRights { get; set; }

        /// <summary>
        /// Gets the group rights.
        /// </summary>
        /// <value>The group rights.</value>
        public List<ItemRights> GroupRights { get; private set; }

        /// <summary>
        /// Initializes a new instance of the <see cref="AppRights"/> class.
        /// </summary>
        /// <param name="userRights">The user rights.</param>
        public AppRights(ItemRights userRights)
        {
            UserRights = userRights;

            GroupRights = new List<ItemRights>();
        }

        /// <summary>
        /// Determines whether the specified table definition has right.
        /// </summary>
        /// <param name="tableDefinition">The table definition.</param>
        /// <param name="rightType">Type of the right.</param>
        /// <returns><c>true</c> if the specified table definition has right; otherwise, <c>false</c>.</returns>
        public bool HasRight(TableDefinitionBase tableDefinition, RightTypes rightType)
        {
            return UserRights.HasRight(tableDefinition, rightType) ||
                   GroupRights.Any(p => p.HasRight(tableDefinition, rightType));
        }

        /// <summary>
        /// Determines whether [has special right] [the specified table definition].
        /// </summary>
        /// <param name="tableDefinition">The table definition.</param>
        /// <param name="rightType">Type of the right.</param>
        /// <returns><c>true</c> if [has special right] [the specified table definition]; otherwise, <c>false</c>.</returns>
        public bool HasSpecialRight(TableDefinitionBase tableDefinition, int rightType)
        {
            return UserRights.HasSpecialRight(tableDefinition, rightType) ||
                   GroupRights.Any(p => p.HasSpecialRight(tableDefinition, rightType));
        }
    }
}
