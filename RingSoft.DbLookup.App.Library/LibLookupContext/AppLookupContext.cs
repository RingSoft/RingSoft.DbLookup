using RingSoft.DbLookup.GetDataProcessor;

namespace RingSoft.DbLookup.App.Library.LibLookupContext
{
    public abstract class AppLookupContext : LookupContextBase, IAppLookupContext
    {
        public abstract AppLookupContextConfiguration LookupContextConfiguration { get; }

        public DataProcessorTypes DataProcessorType
        {
            get { return LookupContextConfiguration.DataProcessorType; }
            set { LookupContextConfiguration.DataProcessorType = value; }
        }

        public override DbDataProcessor DataProcessor => LookupContextConfiguration.DataProcessor;

        public bool ValidateRegistryDbConnectionSettings(RegistrySettings registrySettings)
        {
            return true;
        }
    }
}
