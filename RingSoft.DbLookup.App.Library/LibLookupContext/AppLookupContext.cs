using System;
using RingSoft.DbLookup.DataProcessor;
using RingSoft.DbLookup.ModelDefinition;

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

        public LookupContextBase LookupContext => this;

        public override DbDataProcessor DataProcessor => LookupContextConfiguration.DataProcessor;

        public bool ValidateRegistryDbConnectionSettings(RegistrySettings registrySettings)
        {
            return true;
        }

        public event EventHandler<CanProcessTableArgs> CanViewTableEvent;
    }
}
