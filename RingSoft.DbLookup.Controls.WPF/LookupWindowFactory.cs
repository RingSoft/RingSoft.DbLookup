﻿using RingSoft.DbLookup.Lookup;

namespace RingSoft.DbLookup.Controls.WPF
{
    public class LookupWindowFactory
    {
        public virtual LookupWindow CreateLookupWindow(LookupDefinitionBase lookupDefinition, bool allowAdd,
            bool allowView, string initialSearchFor, PrimaryKeyValue initialPrimaryKeyValue = null, PrimaryKeyValue readOnlyPrimaryKeyValue = null)
        {
            return new LookupWindow(lookupDefinition, allowAdd, allowView, initialSearchFor, readOnlyPrimaryKeyValue)
                {InitialSearchForPrimaryKeyValue = initialPrimaryKeyValue};
        }
    }
}
