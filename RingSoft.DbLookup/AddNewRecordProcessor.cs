using System;
using RingSoft.DbLookup.Lookup;
using RingSoft.DbLookup.ModelDefinition;

namespace RingSoft.DbLookup
{
    public class NewLookupRow<TLookupEntity> where TLookupEntity : new()
    {
        public TLookupEntity NewLookupEntity { get; private set; }

        public PrimaryKeyValue NewPrimaryKeyValue { get; private set; }

        internal NewLookupRow(TLookupEntity newLookupEntity, PrimaryKeyValue newPrimaryKeyValue)
        {
            NewLookupEntity = newLookupEntity;
            NewPrimaryKeyValue = newPrimaryKeyValue;
        }
    }

    internal class AddNewRecordProcessor<TLookupEntity, TEntity> : ILookupControl
        where TLookupEntity : new() where TEntity : new()
    {
        public int PageSize => 1;
        public LookupSearchTypes SearchType => LookupSearchTypes.Equals;
        public string SearchText => string.Empty;

        private LookupDefinition<TLookupEntity, TEntity> _lookupDefinition;
        private string _newText;
        private object _ownerWindow;
        private PrimaryKeyValue _newPrimaryKeyValue;

        public AddNewRecordProcessor(LookupDefinition<TLookupEntity, TEntity> lookupDefinition, string newText,
            object ownerWindow, PrimaryKeyValue newRecordPrimaryKeyValue = null)
        {
            _lookupDefinition = lookupDefinition;
            _newText = newText;
            _ownerWindow = ownerWindow;
            _newPrimaryKeyValue = newRecordPrimaryKeyValue;
        }

        public NewLookupRow<TLookupEntity> AddNewRow()
        {
            var lookupData = new LookupData<TLookupEntity, TEntity>(_lookupDefinition, this);
            lookupData.LookupView += (sender, viewArgs) =>
            {
                viewArgs.Handled = true;
            };
            var args = new LookupAddViewArgs(lookupData, false, LookupFormModes.Add,
                _newText, _ownerWindow) { NewRecordPrimaryKeyValue = _newPrimaryKeyValue};
            _lookupDefinition.TableDefinition.Context.OnAddViewLookup(args);

            var newRecord = new NewLookupRow<TLookupEntity>(lookupData.SelectedItem, lookupData.SelectedPrimaryKeyValue);
            return newRecord;
        }
    }
}
