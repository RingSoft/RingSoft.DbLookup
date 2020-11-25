using RingSoft.DbLookup;
using RingSoft.DbLookup.Controls.WPF;
using RingSoft.DbLookup.Lookup;

namespace RingSoft.SimpleDemo.WPF
{
    public class DemoLookupWindowFactory : LookupWindowFactory
    {
        public override LookupWindow CreateLookupWindow(LookupDefinitionBase lookupDefinition, bool allowAdd, bool allowView,
            string initialSearchFor, PrimaryKeyValue initialPrimaryKeyValue = null)
        {
            return new DemoLookupWindow(lookupDefinition, allowAdd, allowView, initialSearchFor)
                {InitialSearchForPrimaryKeyValue = initialPrimaryKeyValue};
        }
    }
}
