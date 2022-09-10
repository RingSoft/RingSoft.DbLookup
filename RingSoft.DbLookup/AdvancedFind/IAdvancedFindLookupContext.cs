using RingSoft.DbLookup.Lookup;
using RingSoft.DbLookup.ModelDefinition;

namespace RingSoft.DbLookup.AdvancedFind
{
    public interface IAdvancedFindLookupContext
    {
        LookupContextBase Context { get; }
        TableDefinition<AdvancedFind> AdvancedFinds { get; set; }

        TableDefinition<AdvancedFindColumn> AdvancedFindColumns { get; set; }

        TableDefinition<AdvancedFindFilter> AdvancedFindFilters { get; set; }

        LookupDefinition<AdvancedFindLookup, AdvancedFind> AdvancedFindLookup { get; set; }
    }
}
