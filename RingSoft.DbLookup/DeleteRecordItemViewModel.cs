using System.Collections.Generic;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using RingSoft.DbLookup.Lookup;

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
                LookupDefinition = deleteTable.ChildField.TableDefinition.LookupDefinition;
            }

            if (LookupDefinition != null)
            {
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
