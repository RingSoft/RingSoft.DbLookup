using System;
using RingSoft.DbLookup.DataProcessor;
using RingSoft.DbLookup.Lookup;
using RingSoft.DbLookup.ModelDefinition;

namespace RingSoft.DbLookup.App.Library.LibLookupContext
{
    public interface IAppLookupContext
    {
        DataProcessorTypes DataProcessorType { get; set; }

        event EventHandler<LookupAddViewArgs> LookupAddView;

        bool Initialized { get; }

        AppLookupContextConfiguration LookupContextConfiguration { get; }

        DbDataProcessor DataProcessor { get; }

        bool ValidateRegistryDbConnectionSettings(RegistrySettings registrySettings);

        void Initialize();
    }
}
