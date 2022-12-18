using RingSoft.DbLookup.ModelDefinition.FieldDefinitions;
using System.Collections.Generic;
using RingSoft.DbLookup.ModelDefinition;

namespace RingSoft.DbLookup
{
    public class DeleteTables
    {
        public List<DeleteTable> Tables { get; private set; } = new List<DeleteTable>();

        public PrimaryKeyValue PrimaryKeyValue { get; set; }
    }

    public class DeleteTable
    {
        public FieldDefinition ChildField { get; set; }

        public DeleteTables Parent { get; set; }

        public FieldDefinition ParentField { get; set; }

        public FieldDefinition RootField { get; set; }
    }
}
