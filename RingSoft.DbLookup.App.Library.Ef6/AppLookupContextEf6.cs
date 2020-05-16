using RingSoft.DbLookup.App.Library.LibLookupContext;
using RingSoft.DbLookup.DataProcessor;
using RingSoft.DbLookup.Ef6;

namespace RingSoft.DbLookup.App.Library.Ef6
{
    public abstract class AppLookupContextEf6 : LookupContext
    {
        public abstract AppLookupContextConfiguration LookupContextConfiguration { get; }

        public DataProcessorTypes DataProcessorType
        {
            get { return LookupContextConfiguration.DataProcessorType; }
            set { LookupContextConfiguration.DataProcessorType = value; }
        }

        public override DbDataProcessor DataProcessor => LookupContextConfiguration.DataProcessor;
    }
}
