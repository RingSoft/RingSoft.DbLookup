using System;
using System.Linq.Expressions;
using RingSoft.DbLookupCore.ModelDefinition.FieldDefinitions;

namespace RingSoft.DbLookupCore.Lookup
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
        /// Adds a visible column definition.
        /// </summary>
        /// <param name="lookupEntityProperty">The lookup entity property.</param>
        /// <param name="fieldDefinition">The field definition.</param>
        /// <returns></returns>
        public LookupFieldColumnDefinition AddVisibleColumnDefinition(
            Expression<Func<TLookupEntity, object>> lookupEntityProperty,
            FieldDefinition fieldDefinition)
        {
            return AddVisibleColumnDefinition(lookupEntityProperty, string.Empty, fieldDefinition, 0);
        }

        /// <summary>
        /// Adds a visible column definition.
        /// </summary>
        /// <param name="lookupEntityProperty">The lookup entity property.</param>
        /// <param name="caption">The caption.</param>
        /// <param name="fieldDefinition">The field definition.</param>
        /// <param name="percentWidth">The percent of the lookup's total width.</param>
        /// <returns></returns>
        public LookupFieldColumnDefinition AddVisibleColumnDefinition(Expression<Func<TLookupEntity, object>> lookupEntityProperty,
            string caption, FieldDefinition fieldDefinition, double percentWidth)
        {
            var columnDefinition = AddVisibleColumnDefinition(caption, fieldDefinition, percentWidth);
            columnDefinition.PropertyName = lookupEntityProperty.GetFullPropertyName();

            return columnDefinition;
        }

        /// <summary>
        /// Adds a hidden column.
        /// </summary>
        /// <param name="lookupEntityProperty">The lookup entity property.</param>
        /// <param name="fieldDefinition">The field definition.</param>
        /// <returns></returns>
        public LookupFieldColumnDefinition AddHiddenColumn(Expression<Func<TLookupEntity, object>> lookupEntityProperty,
            FieldDefinition fieldDefinition)
        {
            var columnDefinition = AddHiddenColumn(fieldDefinition);
            columnDefinition.PropertyName = lookupEntityProperty.GetFullPropertyName();

            return columnDefinition;
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
