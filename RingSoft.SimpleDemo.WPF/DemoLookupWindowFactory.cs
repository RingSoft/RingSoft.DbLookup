using RingSoft.DbLookup.Controls.WPF;
using RingSoft.DbLookup.Lookup;

namespace RingSoft.SimpleDemo.WPF
{
    public class DemoLookupWindowFactory : LookupWindowFactory
    {
        public override LookupWindow CreateLookupWindow(LookupDefinitionBase lookupDefinition, bool allowAdd, bool allowView,
            string initialSearchFor)
        {
            return new DemoLookupWindow(lookupDefinition, allowAdd, allowView, initialSearchFor);
        }
    }
}
