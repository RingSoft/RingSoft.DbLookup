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
    public class DeleteRecordItemViewModel : INotifyPropertyChanged
    {
		private bool _deleteAllRecords;

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
                OnPropertyChanged();
            }
		}

        private LookupDefinitionBase _lookupDefinition;

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

        private LookupCommand _lookupCommand;

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

        public void Initialize(DeleteTable deleteTable)
        {
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
                if (deleteTable.ParentField == null)
                {
                    foreach (var fieldJoin in deleteTable.ChildField.ParentJoinForeignKeyDefinition.FieldJoins)
                    {
                        var keyValueField =
                            deleteTable.Parent.PrimaryKeyValue.KeyValueFields.FirstOrDefault(p =>
                                p.FieldDefinition == fieldJoin.PrimaryField);

                        if (keyValueField != null)
                        {
                            LookupDefinition.FilterDefinition.AddFixedFieldFilter(fieldJoin.ForeignField,
                                Conditions.Equals,
                                keyValueField.Value);
                        }
                    }
                }
                else
                {
                    var joins = new List<TableFieldJoinDefinition>();

                    var parentJoin = ProcessFieldJoins(deleteTable, joins);
                    var topTable = deleteTable;
                    var tableTree = new List<DeleteTable>();
                    while (topTable.ParentDeleteTable != null)
                    {
                        tableTree.Add(topTable);
                        topTable = topTable.ParentDeleteTable;
                    }

                    if (tableTree.Count > 1)
                    {
                        var first = true;
                        foreach (var table in tableTree)
                        {
                            if (!first)
                            {
                                parentJoin = ProcessFieldJoins(table, joins, parentJoin);
                                //LookupDefinition.AddJoin(parentJoin);
                            }
                            first = false;
                        }
                    }


                    
                    foreach (var foreignKeyFieldJoin in deleteTable.RootField.ParentJoinForeignKeyDefinition.FieldJoins)
                    {
                        var keyValueField =
                            deleteTable.Parent.PrimaryKeyValue.KeyValueFields.FirstOrDefault(p =>
                                p.FieldDefinition == foreignKeyFieldJoin.PrimaryField);

                        if (keyValueField != null)
                        {
                            while (deleteTable.ParentDeleteTable != null)
                            {
                                deleteTable = deleteTable.ParentDeleteTable;
                            }

                            var fieldFilter = LookupDefinition.FilterDefinition.AddFixedFieldFilter(deleteTable.ChildField,
                                Conditions.Equals, keyValueField.Value);
                            fieldFilter.JoinDefinition = joins[joins.Count - 1];
                        }
                    }
                    var parentIndex = joins.Count - 1;
                    var joinIndex = 0;
                    var firstJoin = true;
                    //foreach (var tableFieldJoinDefinition in LookupDefinition.FilterDefinition.Joins)
                    //{
                    //    joinIndex++;
                    //    if (!firstJoin)
                    //    {
                    //        parentIndex--;
                    //        tableFieldJoinDefinition.ParentAlias = joins[parentIndex].Alias;
                    //    }
                    //    firstJoin = false;
                    //}

                }

                LookupCommand = new LookupCommand(LookupCommands.Refresh);
            }
        }

        private LookupJoin ProcessFieldJoins(DeleteTable deleteTable, List<TableFieldJoinDefinition> joins, LookupJoin parentJoin = null)
        {
            LookupJoin join = null;
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
                var tableFieldJoinDefinition = new TableFieldJoinDefinition
                {
                    ForeignKeyDefinition = fieldJoin.ForeignField.ParentJoinForeignKeyDefinition,
                };
                joins.Add(tableFieldJoinDefinition);
                LookupDefinition.FilterDefinition.AddJoin(tableFieldJoinDefinition);
            }

            if (joins.Count > 1)
            {
                var joinIndex = joins.Count - 1;
                joins[joinIndex].ParentAlias = joins[joinIndex - 1].Alias;
            }
            return join;
        }

        public event PropertyChangedEventHandler PropertyChanged;

        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        protected bool SetField<T>(ref T field, T value, [CallerMemberName] string propertyName = null)
        {
            if (EqualityComparer<T>.Default.Equals(field, value)) return false;
            field = value;
            OnPropertyChanged(propertyName);
            return true;
        }
    }
}
