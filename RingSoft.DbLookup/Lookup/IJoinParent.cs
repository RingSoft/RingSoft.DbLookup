using System;
using System.Collections.Generic;
using System.Text;
using RingSoft.DbLookup.ModelDefinition.FieldDefinitions;
using RingSoft.DbLookup.QueryBuilder;

namespace RingSoft.DbLookup.Lookup
{
    public interface IJoinParent
    {
        IJoinParent ParentObject { get; set; }

        FieldDefinition ChildField { get; set; }

        FieldDefinition ParentField { get; set; }

        LookupJoin MakeInclude(LookupDefinitionBase lookupDefinition, FieldDefinition childField = null);

        LookupColumnDefinitionBase AddVisibleColumnDefinitionField(string caption, FieldDefinition fieldDefinition,
            double percentWidth);

        string MakePath();
    }
}
