using RingSoft.DbLookup.Lookup;

// ReSharper disable once CheckNamespace
namespace RingSoft.DbLookup.Controls.WPF
{
    public class LookupControlSearchForFactory
    {
        public LookupSearchForControl CreateSearchForControl(LookupColumnDefinitionBase column)
        {
            LookupSearchForControl searchForControl = new LookupSearchForStringControl(column);
            searchForControl.InternalInitialize();

            return searchForControl;
        }
    }
}
