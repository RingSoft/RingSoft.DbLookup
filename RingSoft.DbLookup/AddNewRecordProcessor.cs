using System;
using RingSoft.DbLookup.Lookup;
using RingSoft.DbLookup.ModelDefinition;

namespace RingSoft.DbLookup
{
    internal class AddNewRecordProcessor : ILookupControl
    {
        public int PageSize => 1;
        public LookupSearchTypes SearchType => LookupSearchTypes.Equals;
        public string SearchText => string.Empty;

        private TableDefinitionBase _tableDefinition;
        private string _newText;
        private object _ownerWindow;

        public AddNewRecordProcessor(TableDefinitionBase tableDefinition, string newText, object ownerWindow)
        {
            if (tableDefinition.LookupDefinition == null)
                throw new Exception($"Lookup Definition for table '{tableDefinition.EntityName}' is not defined.");

            _tableDefinition = tableDefinition;
            _newText = newText;
            _ownerWindow = ownerWindow;
        }

        public PrimaryKeyValue GetNewRecord()
        {
            var lookupData = new LookupDataBase(_tableDefinition.LookupDefinition, this);
            lookupData.LookupView += (sender, viewArgs) =>
            {
                viewArgs.Handled = true;
            };
            var args = new LookupAddViewArgs(lookupData, false, LookupFormModes.Add,
                _newText, _ownerWindow);
            _tableDefinition.Context.OnAddViewLookup(args);
            return lookupData.SelectedPrimaryKeyValue;
        }
    }
}
