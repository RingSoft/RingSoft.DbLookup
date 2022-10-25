using RingSoft.DataEntryControls.Engine;
using RingSoft.DbLookup.Lookup;
using RingSoft.DbLookup.QueryBuilder;

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
            _lookupContext.AdvancedFinds.GetFieldDefinition(p => p.FromFormula).IsMemo();
            _lookupContext.AdvancedFinds.RecordDescription = "Advanced Find";
            _lookupContext.AdvancedFinds.PriorityLevel = 10;
            _lookupContext.AdvancedFinds.IsAdvancedFind = true;

            _lookupContext.AdvancedFindColumns.GetFieldDefinition(p => p.Formula).IsMemo();

            _lookupContext.AdvancedFindColumns.GetFieldDefinition(p => p.DecimalFormatType)
                .IsEnum<DecimalEditFormatTypes>();

            _lookupContext.AdvancedFindColumns.GetFieldDefinition(p => p.FieldDataType)
                .IsEnum<FieldDataTypes>();
            _lookupContext.AdvancedFindColumns.IsAdvancedFind = true;

            _lookupContext.AdvancedFindFilters.GetFieldDefinition(p => p.Formula).IsMemo();

            _lookupContext.AdvancedFindFilters.GetFieldDefinition(p => p.EndLogic)
                .IsEnum<EndLogics>();

            _lookupContext.AdvancedFindFilters.GetFieldDefinition(p => p.FormulaDataType)
                .IsEnum<FieldDataTypes>();

            _lookupContext.AdvancedFindFilters.GetFieldDefinition(p => p.Operand)
                .HasDescription("Condition").IsEnum<Conditions>();
            _lookupContext.AdvancedFindFilters.IsAdvancedFind = true;

            _lookupContext.RecordLocks.GetFieldDefinition(p => p.LockDateTime).HasDateType(DbDateTypes.DateTime);
        }
    }
}
