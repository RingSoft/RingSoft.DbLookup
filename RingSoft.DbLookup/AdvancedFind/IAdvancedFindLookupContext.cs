using RingSoft.DbLookup.Lookup;
using RingSoft.DbLookup.ModelDefinition;
using RingSoft.DbLookup.RecordLocking;

namespace RingSoft.DbLookup.AdvancedFind
{
    public interface IAdvancedFindLookupContext
    {
        LookupContextBase Context { get; }

        TableDefinition<RecordLock> RecordLocks { get; set; }

        TableDefinition<AdvancedFind> AdvancedFinds { get; set; }

        TableDefinition<AdvancedFindColumn> AdvancedFindColumns { get; set; }

        TableDefinition<AdvancedFindFilter> AdvancedFindFilters { get; set; }

        LookupDefinition<AdvancedFindLookup, AdvancedFind> AdvancedFindLookup { get; set; }

        LookupDefinition<RecordLockingLookup, RecordLock> RecordLockingLookup { get; set; }
    }
}
