using RingSoft.DbLookup.ModelDefinition;

namespace RingSoft.DbLookup.AdvancedFind
{
    public interface IAdvancedFindLookupContext
    {
        TableDefinition<AdvancedFind> AdvancedFinds { get; set; }

        TableDefinition<AdvancedFindColumn> AdvancedFindColumns { get; set; }

        TableDefinition<AdvancedFindFilter> AdvancedFindFilters { get; set; }
    }
}
