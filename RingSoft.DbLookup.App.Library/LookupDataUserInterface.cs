using RingSoft.DbLookup.Lookup;

namespace RSDbLookupApp.Library
{
    public class LookupDataUserInterface : ILookupUserInterface
    {
        public int PageSize { get; set; } = 15;
        public LookupSearchTypes SearchType { get; set; } = LookupSearchTypes.Equals;
        public string SearchText { get; set; } = string.Empty;
    }
}
