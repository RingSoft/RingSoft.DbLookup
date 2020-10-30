using RingSoft.DbLookup.Lookup;

namespace RingSoft.DbLookup
{
    public class NewAddOnTheFlyResult<TLookupEntity> where TLookupEntity : new()
    {
        public TLookupEntity NewLookupEntity { get; private set; }

        public PrimaryKeyValue NewPrimaryKeyValue { get; private set; }

        internal NewAddOnTheFlyResult(TLookupEntity newLookupEntity, PrimaryKeyValue newPrimaryKeyValue)
        {
            NewLookupEntity = newLookupEntity;
            NewPrimaryKeyValue = newPrimaryKeyValue;
        }
    }

    internal class AddOnTheFlyProcessor<TLookupEntity, TEntity> : ILookupControl
        where TLookupEntity : new() where TEntity : new()
    {
        public int PageSize => 1;
        public LookupSearchTypes SearchType => LookupSearchTypes.Equals;
        public string SearchText => string.Empty;

        public object AddViewParameter { get; set; }

        private LookupDefinition<TLookupEntity, TEntity> _lookupDefinition;
        private string _newText;
        private object _ownerWindow;
        private PrimaryKeyValue _newPrimaryKeyValue;

        public AddOnTheFlyProcessor(LookupDefinition<TLookupEntity, TEntity> lookupDefinition, string newText,
            object ownerWindow, PrimaryKeyValue newRecordPrimaryKeyValue = null)
        {
            _lookupDefinition = lookupDefinition;
            _newText = newText;
            _ownerWindow = ownerWindow;
            _newPrimaryKeyValue = newRecordPrimaryKeyValue;
        }

        public NewAddOnTheFlyResult<TLookupEntity> ShowAddOnTheFlyWindow()
        {
            var lookupData = new LookupData<TLookupEntity, TEntity>(_lookupDefinition, this);
            lookupData.LookupView += (sender, viewArgs) =>
            {
                viewArgs.Handled = true;
            };
            var args = new LookupAddViewArgs(lookupData, false, LookupFormModes.Add,
                _newText, _ownerWindow)
            {
                NewRecordPrimaryKeyValue = _newPrimaryKeyValue,
                InputParameter = AddViewParameter
            };
            _lookupDefinition.TableDefinition.Context.OnAddViewLookup(args);

            var result = new NewAddOnTheFlyResult<TLookupEntity>(lookupData.SelectedItem, lookupData.SelectedPrimaryKeyValue);
            return result;
        }
    }
}
