// ***********************************************************************
// Assembly         : RingSoft.DbLookup
// Author           : petem
// Created          : 12-19-2022
//
// Last Modified By : petem
// Last Modified On : 07-24-2023
// ***********************************************************************
// <copyright file="LookupColumnDefinitionBase.cs" company="Peter Ringering">
//     Copyright (c) 2023. All rights reserved.
// </copyright>
// <summary></summary>
// ***********************************************************************
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using RingSoft.DataEntryControls.Engine;
using RingSoft.DbLookup.AdvancedFind;
using RingSoft.DbLookup.DataProcessor;
using RingSoft.DbLookup.ModelDefinition.FieldDefinitions;
using RingSoft.DbLookup.QueryBuilder;
using RingSoft.DbLookup.TableProcessing;

namespace RingSoft.DbLookup.Lookup
{
    /// <summary>
    /// Enum LookupColumnTypes
    /// </summary>
    public enum LookupColumnTypes
    {
        /// <summary>
        /// The field
        /// </summary>
        Field = 0,
        /// <summary>
        /// The formula
        /// </summary>
        Formula = 1,
    }

    /// <summary>
    /// Enum LookupColumnAlignmentTypes
    /// </summary>
    public enum LookupColumnAlignmentTypes
    {
        /// <summary>
        /// The left
        /// </summary>
        Left = 0,
        /// <summary>
        /// The center
        /// </summary>
        Center = 1,
        /// <summary>
        /// The right
        /// </summary>
        Right = 2
    }

    /// <summary>
    /// The lookup column definition base class.
    /// </summary>
    public abstract class LookupColumnDefinitionBase : IJoinParent
    {
        /// <summary>
        /// Gets the join query table alias.
        /// </summary>
        /// <value>
        /// The join query table alias.
        /// </value>
        private string _joinAlias;

        /// <summary>
        /// Gets or sets the join query table alias.
        /// </summary>
        /// <value>The join query table alias.</value>
        public string JoinQueryTableAlias
        {
            get
            {
                if (!_joinAlias.IsNullOrEmpty())
                {

                }

                return _joinAlias;
            }
            set
            {
                _joinAlias = value;
            }
        }


        /// <summary>
        /// Gets the type of the column.
        /// </summary>
        /// <value>The type of the column.</value>
        public abstract LookupColumnTypes ColumnType { get; }

        /// <summary>
        /// Gets the type of the data.
        /// </summary>
        /// <value>The type of the data.</value>
        public abstract FieldDataTypes DataType { get; }

        /// <summary>
        /// Gets the lookup definition.
        /// </summary>
        /// <value>The lookup definition.</value>
        public LookupDefinitionBase LookupDefinition { get; internal set; }


        /// <summary>
        /// Gets the select SQL alias.
        /// </summary>
        /// <value>The select SQL alias.</value>
        public abstract string SelectSqlAlias { get; }

        /// <summary>
        /// Gets the caption.
        /// </summary>
        /// <value>The caption.</value>
        public string Caption { get; internal set; }

        /// <summary>
        /// Gets the name of the property.
        /// </summary>
        /// <value>The name of the property.</value>
        public string PropertyName { get; internal set; }

        /// <summary>
        /// Gets the column's percent of the lookup's width.
        /// </summary>
        /// <value>The percent width of the column.</value>
        public double PercentWidth { get; internal set; }

        /// <summary>
        /// Gets the lookup control column identifier.
        /// </summary>
        /// <value>The lookup control column identifier.</value>
        public int LookupControlColumnId { get; internal set; }

        /// <summary>
        /// Gets the content template identifier.
        /// </summary>
        /// <value>The content template identifier.</value>
        public int? ContentTemplateId { get; internal set; }

        /// <summary>
        /// Gets a value indicating whether [keep null empty].
        /// </summary>
        /// <value><c>true</c> if [keep null empty]; otherwise, <c>false</c>.</value>
        public bool KeepNullEmpty { get; internal set; }

        /// <summary>
        /// Gets the navigation properties.
        /// </summary>
        /// <value>The navigation properties.</value>
        public List<JoinInfo>? NavigationProperties { get; internal set; }
        /// <summary>
        /// Gets the horizontal alignment type.
        /// </summary>
        /// <value>The horizontal alignment type.</value>
        public LookupColumnAlignmentTypes HorizontalAlignment { get; private set; }

        /// <summary>
        /// Gets the search for host identifier.
        /// </summary>
        /// <value>The search for host identifier.</value>
        public virtual int? SearchForHostId { get; internal set; }

        /// <summary>
        /// Gets a value indicating whether [show negative values in red].
        /// </summary>
        /// <value><c>true</c> if [show negative values in red]; otherwise, <c>false</c>.</value>
        public bool ShowNegativeValuesInRed { get; internal set; }

        /// <summary>
        /// Gets a value indicating whether [show positive values in green].
        /// </summary>
        /// <value><c>true</c> if [show positive values in green]; otherwise, <c>false</c>.</value>
        public bool ShowPositiveValuesInGreen { get; internal set; }

        /// <summary>
        /// Gets the table description.
        /// </summary>
        /// <value>The table description.</value>
        public string TableDescription { get; internal set; }

        /// <summary>
        /// Gets the field description.
        /// </summary>
        /// <value>The field description.</value>
        public string FieldDescription { get; internal set; }

        /// <summary>
        /// Gets the path.
        /// </summary>
        /// <value>The path.</value>
        public string Path { get; internal set; }

        /// <summary>
        /// Gets the type of the TreeView.
        /// </summary>
        /// <value>The type of the TreeView.</value>
        public abstract TreeViewType TreeViewType { get; }

        /// <summary>
        /// Gets the column index to add.
        /// </summary>
        /// <value>The column index to add.</value>
        public int ColumnIndexToAdd { get; internal set; } = -1;

        /// <summary>
        /// Gets or sets a value indicating whether [adjust column width].
        /// </summary>
        /// <value><c>true</c> if [adjust column width]; otherwise, <c>false</c>.</value>
        public bool AdjustColumnWidth { get; set; } = true;

        /// <summary>
        /// Gets a value indicating whether this instance has nav properties.
        /// </summary>
        /// <value><c>true</c> if this instance has nav properties; otherwise, <c>false</c>.</value>
        public bool HasNavProperties { get; internal set; }

        /// <summary>
        /// Gets a value indicating whether [set horizontal alignment].
        /// </summary>
        /// <value><c>true</c> if [set horizontal alignment]; otherwise, <c>false</c>.</value>
        public bool SetHorizontalAlignment { get; private set; }

        /// <summary>
        /// Setups the column.
        /// </summary>
        internal void SetupColumn()
        {
            HorizontalAlignment = SetupDefaultHorizontalAlignment();
        }

        /// <summary>
        /// Setups the default horizontal alignment.
        /// </summary>
        /// <returns>LookupColumnAlignmentTypes.</returns>
        protected virtual LookupColumnAlignmentTypes SetupDefaultHorizontalAlignment()
        {
            if (LookupDefinition == null)
            {
                return LookupColumnAlignmentTypes.Left;
            }
            switch (DataType)
            {
                case FieldDataTypes.Integer:
                case FieldDataTypes.Decimal:
                    return LookupColumnAlignmentTypes.Right;
                default:
                    return LookupColumnAlignmentTypes.Left;
            }
        }

        /// <summary>
        /// Copies from.
        /// </summary>
        /// <param name="source">The source.</param>
        internal virtual void CopyFrom(LookupColumnDefinitionBase source)
        {
            Caption = source.Caption;
            PropertyName = source.PropertyName;
            PercentWidth = source.PercentWidth;
            HorizontalAlignment = source.HorizontalAlignment;
            SearchForHostId = source.SearchForHostId;
            LookupControlColumnId = source.LookupControlColumnId;
            ShowNegativeValuesInRed = source.ShowNegativeValuesInRed;
            ShowPositiveValuesInGreen = source.ShowPositiveValuesInGreen;
            ContentTemplateId = source.ContentTemplateId;
            //JoinQueryTableAlias = source.JoinQueryTableAlias;
            ParentObject = source.ParentObject;
            ChildField = source.ChildField;
            ParentField = source.ParentField;
            TableDescription = source.TableDescription;
            FieldDescription = source.FieldDescription;
            Path = source.Path;
            SetHorizontalAlignment = source.SetHorizontalAlignment;
        }

        /// <summary>
        /// Formats the value to display in the lookup view.
        /// </summary>
        /// <param name="value">The value from the database.</param>
        /// <returns>The formatted value.</returns>
        public abstract string FormatValue(string value);

        /// <summary>
        /// Gets the text for column.
        /// </summary>
        /// <param name="primaryKeyValue">The primary key value.</param>
        /// <returns>System.String.</returns>
        public abstract string GetTextForColumn(PrimaryKeyValue primaryKeyValue);

        /// <summary>
        /// Sets the horizontal alignment type.
        /// </summary>
        /// <param name="alignmentType">The new horizontal alignment type.</param>
        public void HasHorizontalAlignmentType(LookupColumnAlignmentTypes alignmentType)
        {
            HorizontalAlignment = alignmentType;
            SetHorizontalAlignment = true;
        }

        /// <summary>
        /// Determines whether [has search for host identifier] [the specified host identifier].
        /// </summary>
        /// <param name="hostId">The host identifier.</param>
        public void HasSearchForHostId(int hostId)
        {
            SearchForHostId = hostId;
        }

        /// <summary>
        /// Determines whether [has lookup control column identifier] [the specified lookup control column identifier].
        /// </summary>
        /// <param name="lookupControlColumnId">The lookup control column identifier.</param>
        public void HasLookupControlColumnId(int lookupControlColumnId)
        {
            LookupControlColumnId = lookupControlColumnId;
        }

        /// <summary>
        /// Determines whether [has content template identifier] [the specified content template identifier].
        /// </summary>
        /// <param name="contentTemplateId">The content template identifier.</param>
        /// <exception cref="System.Exception">Custom Content Template can only be set on Integer fields.</exception>
        public void HasContentTemplateId(int contentTemplateId)
        {
            if (DataType != FieldDataTypes.Integer)
                throw new Exception("Custom Content Template can only be set on Integer fields.");

            ContentTemplateId = contentTemplateId;
            if (LookupControlColumnId == LookupDefaults.TextColumnId)
                LookupControlColumnId = LookupDefaults.CustomContentColumnId;
        }

        /// <summary>
        /// Determines whether [has keep null empty] [the specified value].
        /// </summary>
        /// <param name="value">if set to <c>true</c> [value].</param>
        public void HasKeepNullEmpty(bool value = true)
        {
            KeepNullEmpty = value;
        }

        /// <summary>
        /// Does the show negative values in red.
        /// </summary>
        /// <param name="value">if set to <c>true</c> [value].</param>
        public void DoShowNegativeValuesInRed(bool value = true)
        {
            ShowNegativeValuesInRed = value;
        }

        /// <summary>
        /// Does the show positive values in green.
        /// </summary>
        /// <param name="value">if set to <c>true</c> [value].</param>
        public void DoShowPositiveValuesInGreen(bool value = true)
        {
            ShowPositiveValuesInGreen = value;
        }

        /// <summary>
        /// Updates the width of the percent.
        /// </summary>
        /// <param name="newValue">The new value.</param>
        public void UpdatePercentWidth(double newValue)
        {
            PercentWidth = newValue;
        }

        /// <summary>
        /// Updates the caption.
        /// </summary>
        /// <param name="value">The value.</param>
        /// <returns>LookupColumnDefinitionBase.</returns>
        public LookupColumnDefinitionBase UpdateCaption(string value)
        {
            Caption = value;
            return this;
        }

        /// <summary>
        /// The parent
        /// </summary>
        private IJoinParent _parent;
        /// <summary>
        /// The join type
        /// </summary>
        private JoinTypes _joinType;

        /// <summary>
        /// Gets or sets the parent object.
        /// </summary>
        /// <value>The parent object.</value>
        public IJoinParent ParentObject
        {
            get => _parent;
            set
            {
                if (Caption == "Difference" && value == null)
                {

                }

                _parent = value;
            }
        }

        /// <summary>
        /// Gets or sets the name of the related table.
        /// </summary>
        /// <value>The name of the related table.</value>
        public string RelatedTableName { get; set; }
        /// <summary>
        /// Gets or sets the child field.
        /// </summary>
        /// <value>The child field.</value>
        public FieldDefinition ChildField { get; set; }
        /// <summary>
        /// Gets or sets the parent field.
        /// </summary>
        /// <value>The parent field.</value>
        public FieldDefinition ParentField { get; set; }
        /// <summary>
        /// Gets or sets the child join field.
        /// </summary>
        /// <value>The child join field.</value>
        public FieldDefinition ChildJoinField { get; set; }

        /// <summary>
        /// Makes the include.
        /// </summary>
        /// <param name="lookupDefinition">The lookup definition.</param>
        /// <param name="childField">The child field.</param>
        /// <returns>LookupJoin.</returns>
        /// <exception cref="System.NotImplementedException"></exception>
        public LookupJoin MakeInclude(LookupDefinitionBase lookupDefinition, FieldDefinition childField = null)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Adds the visible column definition field.
        /// </summary>
        /// <param name="caption">The caption.</param>
        /// <param name="fieldDefinition">The field definition.</param>
        /// <param name="percentWidth">Width of the percent.</param>
        /// <returns>LookupColumnDefinitionBase.</returns>
        public LookupColumnDefinitionBase AddVisibleColumnDefinitionField(string caption,
            FieldDefinition fieldDefinition,
            double percentWidth)
        {
            return null;
        }

        /// <summary>
        /// Makes the path.
        /// </summary>
        /// <returns>System.String.</returns>
        public string MakePath()
        {
            return string.Empty;
        }

        /// <summary>
        /// Gets the type of the join.
        /// </summary>
        /// <value>The type of the join.</value>
        JoinTypes IJoinParent.JoinType
        {
            get => _joinType;
            set => _joinType = value;
        }

        /// <summary>
        /// Returns a <see cref="System.String" /> that represents this instance.
        /// </summary>
        /// <returns>A <see cref="System.String" /> that represents this instance.</returns>
        public override string ToString()
        {
            return Caption;
        }

        /// <summary>
        /// Formats the column for header row key.
        /// </summary>
        /// <param name="dataRow">The data row.</param>
        /// <returns>System.String.</returns>
        public virtual string FormatColumnForHeaderRowKey(DataRow dataRow)
        {
            var key = dataRow.GetRowValue(SelectSqlAlias);
            key = GblMethods.FormatValueForPrinterRowKey(DataType, key);

            var primaryKeyValue = new PrimaryKeyValue(LookupDefinition.TableDefinition);
            primaryKeyValue.PopulateFromDataRow(dataRow);
            key += primaryKeyValue.KeyString;
            return key;
        }

        /// <summary>
        /// Formats the column for header row key.
        /// </summary>
        /// <typeparam name="TEntity">The type of the t entity.</typeparam>
        /// <param name="primaryKeyValue">The primary key value.</param>
        /// <param name="entity">The entity.</param>
        /// <returns>System.String.</returns>
        public virtual string FormatColumnForHeaderRowKey<TEntity>(PrimaryKeyValue primaryKeyValue, TEntity entity) where TEntity : new()
        {
            var key = GblMethods.GetPropertyValue(entity, GetPropertyJoinName());
            key = GblMethods.FormatValueForPrinterRowKey(DataType, key);

            key += primaryKeyValue.KeyString;
            return key;
        }


        /// <summary>
        /// Gets the formula for column.
        /// </summary>
        /// <returns>ILookupFormula.</returns>
        internal virtual ILookupFormula GetFormulaForColumn()
        {
            return null;
        }

        /// <summary>
        /// Gets the field for column.
        /// </summary>
        /// <returns>FieldDefinition.</returns>
        public virtual FieldDefinition GetFieldForColumn()
        {
            return null;
        }

        /// <summary>
        /// Adds the new column definition.
        /// </summary>
        /// <param name="lookupDefinition">The lookup definition.</param>
        public virtual void AddNewColumnDefinition(LookupDefinitionBase lookupDefinition)
        {

        }

        /// <summary>
        /// Processes the new visible column.
        /// </summary>
        /// <param name="columnDefinition">The column definition.</param>
        /// <param name="lookupDefinition">The lookup definition.</param>
        /// <param name="copyFrom">if set to <c>true</c> [copy from].</param>
        protected internal virtual void ProcessNewVisibleColumn(LookupColumnDefinitionBase columnDefinition
            , LookupDefinitionBase lookupDefinition, bool copyFrom = true)
        {
            columnDefinition.LookupDefinition = lookupDefinition;

            if (copyFrom)
            {
                columnDefinition.CopyFrom(this);
            }

            if (!Path.IsNullOrEmpty())
            {
                var foundTreeItem = lookupDefinition.AdvancedFindTree.ProcessFoundTreeViewItem(Path, TreeViewType);
                if (foundTreeItem != null)
                {
                    if (foundTreeItem.Parent == null)
                    {
                        if (foundTreeItem.FieldDefinition != null 
                            && !foundTreeItem.FieldDefinition.AllowRecursion
                            && columnDefinition.ColumnType == LookupColumnTypes.Formula)
                        {
                            columnDefinition.TableDescription = foundTreeItem.Name;
                        }
                        else
                        {
                            columnDefinition.TableDescription = lookupDefinition.TableDefinition.Description;
                        }
                    }
                    else
                    {
                        columnDefinition.TableDescription = foundTreeItem.Parent.Name;
                    }

                    columnDefinition.FieldDescription = foundTreeItem.Name;

                    var joinResult = lookupDefinition.AdvancedFindTree.MakeIncludes(foundTreeItem);
                    if (joinResult != null)
                    {
                        ParentObject = joinResult.LookupJoin;
                    }
                    if (joinResult != null && joinResult.LookupJoin != null)
                    {
                        columnDefinition.JoinQueryTableAlias = joinResult.LookupJoin.JoinDefinition.Alias;
                    }
                }
            }

            
            lookupDefinition.AddVisibleColumnDefinition(columnDefinition);
        }

        /// <summary>
        /// Loads from TreeView item.
        /// </summary>
        /// <param name="item">The item.</param>
        /// <returns>System.String.</returns>
        internal virtual string LoadFromTreeViewItem(TreeViewItem item)
        {
            Path = item.MakePath();
            Caption = item.Name;
            PercentWidth = 20;
            ProcessNewVisibleColumn(this, item.BaseTree.LookupDefinition, false);
            return string.Empty;
        }

        /// <summary>
        /// Gets the select formula.
        /// </summary>
        /// <returns>System.String.</returns>
        public virtual string GetSelectFormula()
        {
            return string.Empty;
        }

        /// <summary>
        /// Saves to entity.
        /// </summary>
        /// <param name="entity">The entity.</param>
        public virtual void SaveToEntity(AdvancedFindColumn entity)
        {
            entity.Path = Path;
            entity.Caption = Caption;
            entity.PercentWidth = PercentWidth;
        }

        /// <summary>
        /// Loads from entity.
        /// </summary>
        /// <param name="entity">The entity.</param>
        /// <param name="lookupDefinition">The lookup definition.</param>
        internal virtual void LoadFromEntity(AdvancedFindColumn entity, LookupDefinitionBase lookupDefinition)
        {
            Path = entity.Path;
            if (!Path.IsNullOrEmpty())
            {
                var foundItem = lookupDefinition.AdvancedFindTree.ProcessFoundTreeViewItem(Path, TreeViewType);
                if (foundItem != null)
                {
                    LoadFromTreeViewItem(foundItem);
                }
            }
            Caption = entity.Caption;
            PercentWidth = entity.PercentWidth;
            var test = this;
            HorizontalAlignment = SetupDefaultHorizontalAlignment();
            AdjustColumnWidth = false;
        }

        /// <summary>
        /// Formats the value for column map.
        /// </summary>
        /// <param name="value">The value.</param>
        /// <returns>System.String.</returns>
        public virtual string FormatValueForColumnMap(string value)
        {
            if (SearchForHostId != null)
            {
                var newValue = DbDataProcessor.UserInterface.FormatValue(value, SearchForHostId.Value);
                if (newValue == value)
                {
                    return FormatValue(value);
                }
            }
            else
            {
                return FormatValue(value);
            }
            return FormatValue(value);
        }

        /// <summary>
        /// Gets the property object.
        /// </summary>
        /// <typeparam name="TEntity">The type of the t entity.</typeparam>
        /// <param name="entity">The entity.</param>
        /// <returns>System.Object.</returns>
        public object GetPropertyObject<TEntity>(TEntity entity)
        {
            var properties = ParentObject.GetNavigationProperties();

            return GetPropertyObject(entity, properties);
        }

        /// <summary>
        /// Gets the property object.
        /// </summary>
        /// <typeparam name="TEntity">The type of the t entity.</typeparam>
        /// <param name="entity">The entity.</param>
        /// <param name="properties">The properties.</param>
        /// <returns>System.Object.</returns>
        public object GetPropertyObject<TEntity>(TEntity entity, List<JoinInfo> properties)
        {
            HasNavProperties = properties.Any();

            object propertyObject = null;

            var index = 0;
            foreach (var property in properties)
            {
                if (propertyObject == null)
                {
                    if (index > 0)
                    {
                        return null;
                    }
                    propertyObject = GblMethods.GetPropertyObject(entity, property
                        .ParentJoin.ForeignKeyDefinition.ForeignObjectPropertyName);
                }
                else
                {
                    propertyObject = GblMethods.GetPropertyObject(propertyObject, property
                        .ParentJoin.ForeignKeyDefinition.ForeignObjectPropertyName);
                }
                index++;
            }

            return propertyObject;
        }


        /// <summary>
        /// Gets the name of the property join.
        /// </summary>
        /// <param name="useDbField">if set to <c>true</c> [use database field].</param>
        /// <returns>System.String.</returns>
        public abstract string GetPropertyJoinName(bool useDbField = false);

        /// <summary>
        /// Gets the database value.
        /// </summary>
        /// <typeparam name="TEntity">The type of the t entity.</typeparam>
        /// <param name="entity">The entity.</param>
        /// <returns>System.String.</returns>
        public abstract string GetDatabaseValue<TEntity>(TEntity entity) where TEntity : new();

        /// <summary>
        /// Gets the formatted value.
        /// </summary>
        /// <typeparam name="TEntity">The type of the t entity.</typeparam>
        /// <param name="entity">The entity.</param>
        /// <returns>System.String.</returns>
        public abstract string GetFormattedValue<TEntity>(TEntity entity) where TEntity : new();
    }
}
