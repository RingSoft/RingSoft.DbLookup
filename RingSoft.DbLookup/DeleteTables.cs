using RingSoft.DbLookup.ModelDefinition.FieldDefinitions;
using System.Collections.Generic;

namespace RingSoft.DbLookup
{
    public class DeleteTables
    {
        public List<DeleteTable> Tables { get; private set; } = new List<DeleteTable>();
    }

    public class DeleteTable
    {
        public FieldDefinition ChildField { get; set; }

        public DeleteTables Parent { get; set; }

        public PrimaryKeyValue PrimaryKeyValue { get; set; }
    }
}
