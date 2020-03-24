using RingSoft.DbLookup.App.Library.LookupContext;
using RingSoft.DbLookup.EfCore;
using RingSoft.DbLookup.GetDataProcessor;

namespace RSDbLookupApp.Library.EfCore
{
    public abstract class AppLookupContextEfCore : LookupContext
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
