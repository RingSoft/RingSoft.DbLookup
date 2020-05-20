using RingSoft.DbLookup.ModelDefinition;

namespace RingSoft.DbLookup.Lookup
{
    /// <summary>
    /// A lookup definition that has no generic entity but a generic lookup entity.
    /// </summary>
    /// <typeparam name="TLookupEntity">The type of the lookup entity.</typeparam>
    /// <seealso cref="LookupDefinitionBase" />
    // ReSharper disable once UnusedTypeParameter
    public class LookupEntityDefinition<TLookupEntity> : LookupDefinitionBase where TLookupEntity : new()
    {
        internal LookupEntityDefinition(TableDefinitionBase tableDefinition) : base(tableDefinition)
        {
        }

    }
}
