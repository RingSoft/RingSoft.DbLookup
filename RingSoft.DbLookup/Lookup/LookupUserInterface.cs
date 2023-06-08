using RingSoft.DataEntryControls.Engine;
using RingSoft.DbLookup.DataProcessor;

namespace RingSoft.DbLookup.Lookup
{
    public class LookupUserInterface : ILookupControl
    {
        public int PageSize { get; set; } = 20;
        public LookupSearchTypes SearchType { get; set; } = LookupSearchTypes.Equals;
        public string SearchText { get; set; }
        public int SelectedIndex => 0;
    }
}
