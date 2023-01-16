using RingSoft.DbLookup.ModelDefinition.FieldDefinitions;
using System.Collections.Generic;
using RingSoft.DbLookup.ModelDefinition;
using RingSoft.DbLookup.QueryBuilder;

namespace RingSoft.DbLookup
{
    public class DeleteTables
    {
        public List<DeleteTable> Tables { get; private set; } = new List<DeleteTable>();

        public PrimaryKeyValue PrimaryKeyValue { get; set; }

        public bool DeleteAllData { get; set; }
    }

    public class DeleteTable
    {
        public FieldDefinition ChildField { get; set; }

        public DeleteTables Parent { get; set; }

        public FieldDefinition ParentField { get; set; }

        public DeleteTable ParentDeleteTable { get; set; }

        public FieldDefinition RootField { get; set; }
        
        public SelectQuery Query { get; set; }

        public bool DeleteAllData { get; set; }
        
        public bool NullAllData { get; set; }

        public bool Processed { get; set; }

        public string Description { get; set; }

        public override string ToString()
        {
            if (ChildField != null)
            {
                return ChildField.ToString();
            }
            return base.ToString();
        }
    }
}
