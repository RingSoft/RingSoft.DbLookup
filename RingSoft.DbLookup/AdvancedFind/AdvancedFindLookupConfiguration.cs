using RingSoft.DbLookup.Lookup;

namespace RingSoft.DbLookup.AdvancedFind
{
    public class AdvancedFindLookupConfiguration
    {
        private IAdvancedFindLookupContext _lookupContext;

        public AdvancedFindLookupConfiguration(IAdvancedFindLookupContext lookupContext)
        {
            _lookupContext = lookupContext;
        }

        public void ConfigureLookups()
        {
            var advancedFindLookup = new LookupDefinition<AdvancedFindLookup, AdvancedFind>(_lookupContext.AdvancedFinds);
            advancedFindLookup.AddVisibleColumnDefinition(p => p.Name, "Name"
                , p => p.Name, 95);

            _lookupContext.AdvancedFindLookup = advancedFindLookup;

            _lookupContext.AdvancedFinds.HasLookupDefinition(advancedFindLookup);
        }

        public void InitializeModel()
        {
            _lookupContext.AdvancedFinds.RecordDescription = "Advanced Find";
        }
    }
}
