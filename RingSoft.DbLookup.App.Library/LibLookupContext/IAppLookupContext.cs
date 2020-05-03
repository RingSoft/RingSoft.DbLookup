using System;
using RingSoft.DbLookup.GetDataProcessor;
using RingSoft.DbLookup.Lookup;

namespace RingSoft.DbLookup.App.Library.LibLookupContext
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
