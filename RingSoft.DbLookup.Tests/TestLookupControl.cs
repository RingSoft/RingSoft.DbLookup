using RingSoft.DbLookup.Lookup;

namespace RingSoft.DbLookup.Tests
{
    public class TestLookupControl: ILookupControl
    {
        public int PageSize => _pageSize;
        public LookupSearchTypes SearchType => _searchType;
        public string SearchText => _searchText;
        public int SelectedIndex => _selectedIndex;

        private int _pageSize = 10;
        private LookupSearchTypes _searchType = LookupSearchTypes.Equals;
        private string _searchText;
        private int _selectedIndex;

        public void SetLookupIndex(int index)
        {
            _selectedIndex = index;
        }

        public void SetPageSize(int value)
        {
            _pageSize = value;
        }

        public void SetSearchType(LookupSearchTypes searchType)
        {
            _searchType = searchType;
        }

        public void SetSearchText(string text)
        {
            _searchText = text;
        }
    }
}
