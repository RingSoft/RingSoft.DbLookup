// ***********************************************************************
// Assembly         : RingSoft.DbLookup
// Author           : petem
// Created          : 12-19-2022
//
// Last Modified By : petem
// Last Modified On : 06-19-2023
// ***********************************************************************
// <copyright file="DeleteRecordItemViewModel.cs" company="Peter Ringering">
//     Copyright (c) . All rights reserved.
// </copyright>
// <summary></summary>
// ***********************************************************************
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using RingSoft.DbLookup.DataProcessor;
using RingSoft.DbLookup.Lookup;
using RingSoft.DbLookup.ModelDefinition.FieldDefinitions;
using RingSoft.DbLookup.ModelDefinition;
using RingSoft.DbLookup.QueryBuilder;
using RingSoft.DbLookup.TableProcessing;

namespace RingSoft.DbLookup
{
    /// <summary>
    /// Class DeleteRecordItemViewModel.
    /// Implements the <see cref="INotifyPropertyChanged" />
    /// </summary>
    /// <seealso cref="INotifyPropertyChanged" />
    public class DeleteRecordItemViewModel : INotifyPropertyChanged
    {
        /// <summary>
        /// The delete all records
        /// </summary>
        private bool _deleteAllRecords;

        /// <summary>
        /// Gets or sets a value indicating whether [delete all records].
        /// </summary>
        /// <value><c>true</c> if [delete all records]; otherwise, <c>false</c>.</value>
        public bool DeleteAllRecords
		{
			get => _deleteAllRecords;
            set
            {
                if (_deleteAllRecords == value)
                {
                    return;
                }
                _deleteAllRecords = value;
                DeleteTable.DeleteAllData = value;
                OnPropertyChanged();
            }
		}

        /// <summary>
        /// The null all records
        /// </summary>
        private bool _nullAllRecords;

        /// <summary>
        /// Gets or sets a value indicating whether [null all records].
        /// </summary>
        /// <value><c>true</c> if [null all records]; otherwise, <c>false</c>.</value>
        public bool NullAllRecords
        {
            get => _nullAllRecords;
            set
            {
                if (_nullAllRecords == value)
                {
                    return;
                }
                _nullAllRecords = value;
                DeleteTable.NullAllData = value;
                OnPropertyChanged();
            }
        }


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
                if (_lookupDefinition == value)
                {
                    return;
                }
                _lookupDefinition = value;
                OnPropertyChanged();
            }
        }

        /// <summary>
        /// The lookup command
        /// </summary>
        private LookupCommand _lookupCommand;

        /// <summary>
        /// Gets or sets the lookup command.
        /// </summary>
        /// <value>The lookup command.</value>
        public LookupCommand LookupCommand
        {
            get => _lookupCommand;
            set
            {
                if (_lookupCommand == value)
                    return;

                _lookupCommand = value;
                OnPropertyChanged();
            }
        }

        /// <summary>
        /// Gets the delete table.
        /// </summary>
        /// <value>The delete table.</value>
        public DeleteTable DeleteTable { get; private set; }

        /// <summary>
        /// Initializes the specified delete table.
        /// </summary>
        /// <param name="deleteTable">The delete table.</param>
        public void Initialize(DeleteTable deleteTable)
        {
            DeleteTable = deleteTable;
            DeleteAllRecords = deleteTable.DeleteAllData;
            NullAllRecords = deleteTable.NullAllData;
            if (deleteTable.ChildField.TableDefinition.LookupDefinition != null)
            {
                LookupDefinition = deleteTable.ChildField.TableDefinition.LookupDefinition.Clone();
            }
            else
            {
                LookupDefinition = new LookupDefinitionBase(deleteTable.ChildField.TableDefinition);
                var stringFields =
                    deleteTable.ChildField.TableDefinition.FieldDefinitions.Where(p =>
                        p.FieldDataType == FieldDataTypes.String);
                foreach (var stringField in stringFields)
                {
                    var width = (100 / stringFields.Count());
                    LookupDefinition.AddVisibleColumnDefinition(stringField.Description, stringField, width, "");
                }
            }

            if (LookupDefinition != null)
            {
                var fieldFilters = deleteTable
                    .Query
                    .Filter
                    .FixedFilters
                    .OfType<FieldFilterDefinition>();

                foreach (var fieldFilter in fieldFilters)
                {
                    var newFilter = LookupDefinition.FilterDefinition.AddFixedFilter(
                        fieldFilter.FieldDefinition
                        , fieldFilter.Condition
                        , fieldFilter.Value);
                    newFilter.PropertyName = fieldFilter.PropertyName;
                    newFilter.LookupColumn = fieldFilter.LookupColumn;
                }
                //if (deleteTable.ParentField == null)
                //{
                //    foreach (var fieldJoin in deleteTable.ChildField.ParentJoinForeignKeyDefinition.FieldJoins)
                //    {
                //        var keyValueField =
                //            deleteTable.Parent.PrimaryKeyValue.KeyValueFields.FirstOrDefault(p =>
                //                p.FieldDefinition == fieldJoin.PrimaryField);

                //        if (keyValueField != null)
                //        {
                //            LookupDefinition.FilterDefinition.AddFixedFieldFilter(fieldJoin.ForeignField,
                //                Conditions.Equals,
                //                keyValueField.Value);
                //        }
                //    }
                //}
                //else
                //{
                //    var joins = new List<TableFieldJoinDefinition>();

                //    var parentJoin = ProcessFieldJoins(deleteTable, joins);
                //    var topTable = deleteTable;
                //    var tableTree = new List<DeleteTable>();
                //    while (topTable.ParentDeleteTable != null)
                //    {
                //        tableTree.Add(topTable);
                //        topTable = topTable.ParentDeleteTable;
                //    }

                //    if (tableTree.Count > 1)
                //    {
                //        var first = true;
                //        foreach (var table in tableTree)
                //        {
                //            if (!first)
                //            {
                //                parentJoin = ProcessFieldJoins(table, joins, parentJoin);
                //                //LookupDefinition.AddJoin(parentJoin);
                //            }
                //            first = false;
                //        }
                //    }


                    
                //    foreach (var foreignKeyFieldJoin in deleteTable.RootField.ParentJoinForeignKeyDefinition.FieldJoins)
                //    {
                //        var keyValueField =
                //            deleteTable.Parent.PrimaryKeyValue.KeyValueFields.FirstOrDefault(p =>
                //                p.FieldDefinition == foreignKeyFieldJoin.PrimaryField);

                //        if (keyValueField != null)
                //        {
                //            while (deleteTable.ParentDeleteTable != null)
                //            {
                //                deleteTable = deleteTable.ParentDeleteTable;
                //            }

                //            if (deleteTable.ChildField == null)
                //            {
                //                deleteTable.ChildField = deleteTable.Column.FieldDefinition;
                //            }
                //            var fieldFilter = LookupDefinition.FilterDefinition.AddFixedFieldFilter(deleteTable.ChildField,
                //                Conditions.Equals, keyValueField.Value);
                //            fieldFilter.JoinDefinition = joins[joins.Count - 1];
                //        }
                //    }
                //}

                LookupCommand = new LookupCommand(LookupCommands.Refresh);
            }
        }

        /// <summary>
        /// Processes the field joins.
        /// </summary>
        /// <param name="deleteTable">The delete table.</param>
        /// <param name="joins">The joins.</param>
        /// <param name="parentJoin">The parent join.</param>
        /// <returns>LookupJoin.</returns>
        private LookupJoin ProcessFieldJoins(DeleteTable deleteTable, List<TableFieldJoinDefinition> joins, LookupJoin parentJoin = null)
        {
            LookupJoin join = null;
            var joinIndex = 0;
            foreach (var fieldJoin in deleteTable.ChildField.ParentJoinForeignKeyDefinition.FieldJoins)
            {
                if (parentJoin == null)
                {
                    join = LookupDefinition.Include(fieldJoin.ForeignField);
                }
                else
                {
                    join = parentJoin.Include(fieldJoin.ForeignField);
                }
                if (joinIndex == 0)
                {
                    var tableFieldJoinDefinition = new TableFieldJoinDefinition
                    {
                        ForeignKeyDefinition = fieldJoin.ForeignField.ParentJoinForeignKeyDefinition,
                        ParentObject = parentJoin
                    };

                    joins.Add(tableFieldJoinDefinition);
                    LookupDefinition.FilterDefinition.AddJoin(tableFieldJoinDefinition);
                }
                joinIndex++;
            }

            if (joins.Count > 1)
            {
                joinIndex = joins.Count - 1;
                joins[joinIndex].ParentAlias = joins[joinIndex - 1].Alias;
            }
            return join;
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

        /// <summary>
        /// Sets the field.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="field">The field.</param>
        /// <param name="value">The value.</param>
        /// <param name="propertyName">Name of the property.</param>
        /// <returns><c>true</c> if XXXX, <c>false</c> otherwise.</returns>
        protected bool SetField<T>(ref T field, T value, [CallerMemberName] string propertyName = null)
        {
            if (EqualityComparer<T>.Default.Equals(field, value)) return false;
            field = value;
            OnPropertyChanged(propertyName);
            return true;
        }
    }
}
