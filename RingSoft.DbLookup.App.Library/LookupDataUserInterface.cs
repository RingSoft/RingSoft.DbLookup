﻿using RingSoft.DbLookup.Lookup;

namespace RingSoft.DbLookup.App.Library
{
    public class LookupDataUserInterface : ILookupControl
    {
        public int PageSize { get; set; } = 15;
        public LookupSearchTypes SearchType { get; set; } = LookupSearchTypes.Equals;
        public string SearchText { get; set; } = string.Empty;
        public int SelectedIndex => 0;
        public void SetLookupIndex(int index)
        {
            
        }
    }
}
