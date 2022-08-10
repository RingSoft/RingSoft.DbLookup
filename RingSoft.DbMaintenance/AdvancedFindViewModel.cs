using RingSoft.DbLookup;
using RingSoft.DbLookup.AdvancedFind;
using RingSoft.DbLookup.ModelDefinition;

namespace RingSoft.DbMaintenance
{
    public class AdvancedFindViewModel : DbMaintenanceViewModel<AdvancedFind>
    {
        public override TableDefinition<AdvancedFind> TableDefinition =>
            SystemGlobals.AdvancedFindLookupContext.AdvancedFinds;
        protected override AdvancedFind PopulatePrimaryKeyControls(AdvancedFind newEntity, PrimaryKeyValue primaryKeyValue)
        {
            var advancedFind = SystemGlobals.AdvancedFindDbProcessor.GetAdvancedFind(newEntity.Id);

            return advancedFind;
        }

        protected override void LoadFromEntity(AdvancedFind entity)
        {
            
        }

        protected override AdvancedFind GetEntityData()
        {
            return new AdvancedFind();
        }

        protected override void ClearData()
        {
            
        }

        protected override bool SaveEntity(AdvancedFind entity)
        {
            return true;
        }

        protected override bool DeleteEntity()
        {
            return true;
        }
    }
}
