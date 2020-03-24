using RingSoft.DbLookup.App.Library.LookupContext;
using RingSoft.DbLookup.GetDataProcessor;

namespace RingSoft.DbLookup.App.Library.Ef6
{
    public abstract class AppLookupContextEf6 : DbLookup.Ef6.LookupContext
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
