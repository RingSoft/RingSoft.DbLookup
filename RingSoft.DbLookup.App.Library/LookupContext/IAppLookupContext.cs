using System;
using RingSoft.DbLookup;
using RingSoft.DbLookup.GetDataProcessor;
using RingSoft.DbLookup.Lookup;
using RSDbLookupApp.Library.LookupContext;

namespace RSDbLookupApp.Library
{
    public interface IAppLookupContext
    {
        DataProcessorTypes DataProcessorType { get; set; }

        event EventHandler<LookupAddViewArgs> LookupAddView;

        event EventHandler<TableProcessingArgs> TableProcessing;

        bool Initialized { get; }

        AppLookupContextConfiguration LookupContextConfiguration { get; }

        DbDataProcessor DataProcessor { get; }

        bool ValidateRegistryDbConnectionSettings(RegistrySettings registrySettings);
    }
}
