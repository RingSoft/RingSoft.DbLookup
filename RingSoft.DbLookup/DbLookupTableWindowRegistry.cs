using RingSoft.DbLookup.Lookup;
using RingSoft.DbLookup.ModelDefinition;

namespace RingSoft.DbLookup
{
    public abstract class DbLookupTableWindowRegistry
    {
        public DbLookupTableWindowRegistry()
        {
            ActivateRegistry();
        }

        public virtual void ActivateRegistry()
        {
            SystemGlobals.TableRegistry = this;
        }

        public abstract bool IsTableRegistered(TableDefinitionBase tableDefinition);

        public abstract void ShowAddOntheFlyWindow(
            TableDefinitionBase tableDefinition
            , LookupAddViewArgs addViewArgs = null
            , object inputParameter = null);

        public abstract void ShowWindow(TableDefinitionBase tableDefinition);
    }
}
