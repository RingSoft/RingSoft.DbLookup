﻿using RingSoft.DbLookup.ModelDefinition.FieldDefinitions;

namespace RingSoft.DbLookup.Lookup
{
    /// <summary>
    /// A lookup entity join  definition to a foreign table definition.
    /// </summary>
    /// <typeparam name="TLookupEntity">The type of the lookup entity.</typeparam>
    /// <seealso cref="LookupJoin" />
    public class LookupJoinEntity<TLookupEntity> : LookupJoin where TLookupEntity : new()
    {
        private LookupEntityDefinition<TLookupEntity> _lookupEntityDefinition;

        public LookupJoinEntity(LookupEntityDefinition<TLookupEntity> lookupEntityDefinition, FieldDefinition foreignFieldDefinition)
            : base(lookupEntityDefinition, foreignFieldDefinition)
        {
            _lookupEntityDefinition = lookupEntityDefinition;
        }

        protected internal LookupJoinEntity(LookupEntityDefinition<TLookupEntity> lookupEntityDefinition) : base(lookupEntityDefinition)
        {
            _lookupEntityDefinition = lookupEntityDefinition;
        }

        /// <summary>
        /// Includes the specified foreign field definition.
        /// </summary>
        /// <param name="foreignFieldDefinition">The foreign field definition.</param>
        /// <returns></returns>
        public new LookupJoinEntity<TLookupEntity> Include(FieldDefinition foreignFieldDefinition)
        {
            var lookupJoin = new LookupJoinEntity<TLookupEntity>(_lookupEntityDefinition);
            lookupJoin.JoinDefinition = JoinDefinition;
            lookupJoin.SetJoinDefinition(foreignFieldDefinition);
            return lookupJoin;
        }
    }
}
