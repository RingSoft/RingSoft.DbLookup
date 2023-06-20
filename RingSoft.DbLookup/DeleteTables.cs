using RingSoft.DbLookup.ModelDefinition.FieldDefinitions;
using System.Collections.Generic;
using RingSoft.DbLookup.Lookup;
using RingSoft.DbLookup.ModelDefinition;
using RingSoft.DbLookup.QueryBuilder;

namespace RingSoft.DbLookup
{
    public class DeleteTables
    {
        public List<DeleteTable> Tables { get; private set; } = new List<DeleteTable>();

        public PrimaryKeyValue PrimaryKeyValue { get; set; }

        public bool DeleteAllData { get; set; }

        public IDbContext Context { get; set; }
    }

    public class DeleteTable
    {
        private FieldDefinition _childField;

        public FieldDefinition ChildField
        {
            get
            {
                if (_childField == null)
                {
                    
                }
                return _childField;
            }
            set
            {
                if (value == null)
                {
                    
                }
                _childField = value;
        }
    }

        public DeleteTables Parent { get; set; }

        public FieldDefinition ParentField { get; set; }

        public DeleteTable ParentDeleteTable { get; set; }

        public FieldDefinition RootField { get; set; }
        
        public SelectQueryMauiBase Query { get; set; }

        public bool DeleteAllData { get; set; }
        
        public bool NullAllData { get; set; }

        public bool Processed { get; set; }

        public string Description { get; set; }

        public LookupFieldColumnDefinition Column { get; set; }

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
