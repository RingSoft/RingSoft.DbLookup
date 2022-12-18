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
                    foreach (var fieldJoin in deleteTable.ChildField.ParentJoinForeignKeyDefinition.FieldJoins)
                    {
                        LookupDefinition.Include(fieldJoin.ForeignField);
                        {
                            var tableFieldJoinDefinition = new TableFieldJoinDefinition
                            {
                                ForeignKeyDefinition = fieldJoin.ForeignField.ParentJoinForeignKeyDefinition,
                            };
                            joins.Add(tableFieldJoinDefinition);
                            LookupDefinition.FilterDefinition.AddJoin(tableFieldJoinDefinition);
                        }
                    }
                    var fieldIndex = 0;
                    foreach (var foreignKeyFieldJoin in deleteTable.RootField.ParentJoinForeignKeyDefinition.FieldJoins)
                    {
                        var keyValueField =
                            deleteTable.Parent.PrimaryKeyValue.KeyValueFields.FirstOrDefault(p =>
                                p.FieldDefinition == foreignKeyFieldJoin.PrimaryField);

                        if (keyValueField != null)
                        {
                            var fieldFilter = LookupDefinition.FilterDefinition.AddFixedFieldFilter(foreignKeyFieldJoin.ForeignField,
                                Conditions.Equals, keyValueField.Value);
                            fieldFilter.JoinDefinition = joins[fieldIndex];
                        }

                        fieldIndex++;
                    }
                }

                LookupCommand = new LookupCommand(LookupCommands.Refresh);
            }
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
