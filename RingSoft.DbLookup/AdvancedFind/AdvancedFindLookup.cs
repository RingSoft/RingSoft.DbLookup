using System;
using System.Collections.Generic;
using System.Text;

namespace RingSoft.DbLookup.AdvancedFind
{
    public class AdvancedFindLookup
    {
        public string Name { get; set; }

        public string TableName { get; set; }
    }

    public class AdvFindFilterLookup
    {
        public string TableName { get; set; }

        public string FieldName { get; set; }
    }
}
