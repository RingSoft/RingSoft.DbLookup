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

    internal class AddOnTheFlyProcessor : ILookupControl
    {
        public int PageSize => 1;
        public LookupSearchTypes SearchType => LookupSearchTypes.Equals;
        public string SearchText { get; set; }
        public object AddViewParameter { get; set; }

        public object OwnerWindow { get; set; }

        public PrimaryKeyValue NewPrimaryKeyValue { get; set; }

        public PrimaryKeyValue SelectedPrimaryKeyValue { get; set; }

        public LookupDefinitionBase LookupDefinition { get; private set; }

        public AddOnTheFlyProcessor(LookupDefinitionBase lookupDefinition)
        {
            LookupDefinition = lookupDefinition;
        }

        public LookupAddViewArgs SetupProcessor(LookupDataMauiBase lookupData)
        {
            LookupAddViewArgs args = null;
            if (SelectedPrimaryKeyValue == null)
            {
                args = new LookupAddViewArgs(lookupData, false, LookupFormModes.Add,
                    SearchText, OwnerWindow)
                {
                    NewRecordPrimaryKeyValue = NewPrimaryKeyValue,
                    InputParameter = AddViewParameter
                };
            }
            else
            {
                args = new LookupAddViewArgs(lookupData, false, LookupFormModes.View,
                    string.Empty, OwnerWindow)
                {
                    SelectedPrimaryKeyValue = SelectedPrimaryKeyValue,
                    InputParameter = AddViewParameter
                };

            }

            args.CallBackToken.RefreshData += (sender, eventArgs) =>
            {
                LookupDefinition.FireCloseEvent(lookupData);
                if (lookupData.SelectedPrimaryKeyValue == null || !lookupData.SelectedPrimaryKeyValue.IsValid)
                {
                    lookupData.RefreshData();
                }
            };
            return args;
        }

        public void ShowAddOnTheFlyWindow()
        {
            //var lookupData = new LookupDataBase(LookupDefinition, this);
            var lookupData = LookupDefinition.TableDefinition.LookupDefinition.GetLookupDataMaui(LookupDefinition, true);
            //lookupData.LookupView += (sender, viewArgs) =>
            //{
            //    viewArgs.Handled = true;
            //};
            var args = SetupProcessor(lookupData);
            LookupDefinition.TableDefinition.Context.OnAddViewLookup(args);
        }
    }

    internal class AddOnTheFlyProcessor<TLookupEntity, TEntity> : AddOnTheFlyProcessor
        where TLookupEntity : new() where TEntity : class, new()
    {
        private LookupDefinition<TLookupEntity, TEntity> _lookupDefinition;
        public AddOnTheFlyProcessor(LookupDefinition<TLookupEntity, TEntity> lookupDefinition, string newText,
            object ownerWindow, PrimaryKeyValue newRecordPrimaryKeyValue = null,
            PrimaryKeyValue selectedPrimaryKeyValue = null) : base(lookupDefinition)
        {
            SearchText = newText;
            OwnerWindow = ownerWindow;
            NewPrimaryKeyValue = newRecordPrimaryKeyValue;
            SelectedPrimaryKeyValue = selectedPrimaryKeyValue;
            _lookupDefinition = lookupDefinition;
        }

        public NewAddOnTheFlyResult<TLookupEntity> ShowAddOnTheFlyWindow()
        {
            //var lookupData = new LookupData<TLookupEntity, TEntity>(_lookupDefinition, this);
            //lookupData.LookupView += (sender, viewArgs) =>
            //{
            //    viewArgs.Handled = true;
            //};
            //var args = SetupProcessor(lookupData);
            //_lookupDefinition.TableDefinition.Context.OnAddViewLookup(args);

            //var result = new NewAddOnTheFlyResult<TLookupEntity>(lookupData.SelectedItem, lookupData.SelectedPrimaryKeyValue);
            //return result;
            return null;
        }
    }
}
