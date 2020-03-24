using RingSoft.DbLookup.App.Library.LookupContext;
using RingSoft.DbLookup.GetDataProcessor;

namespace RingSoft.DbLookup.App.Library.EfCore
{
    public abstract class AppLookupContextEfCore : DbLookup.EfCore.LookupContext
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
