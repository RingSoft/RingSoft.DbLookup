using RingSoft.DbLookup.Lookup;
using RingSoft.DbLookup.ModelDefinition;

namespace RingSoft.SimpleDemo.WPF
{
    public interface IDemoLookupDefinition
    {
        string TopHeader { get; set; }
    }

    public class DemoLookupDefinition<TLookupEntity, TEntity> : LookupDefinition<TLookupEntity, TEntity>, IDemoLookupDefinition
    where TLookupEntity : new() where TEntity : new()
    {
        public string TopHeader { get; set; }

        public DemoLookupDefinition(TableDefinition<TEntity> tableDefinition) : base(tableDefinition)
        {
        }

        protected override LookupDefinitionBase BaseClone()
        {
            var clone = new DemoLookupDefinition<TLookupEntity, TEntity>(TableDefinition);
            clone.CopyLookupData(this);
            return clone;
        }

        /// <summary>
        /// Clones this instance.
        /// </summary>
        /// <returns>A copy of this object.</returns>
        public new DemoLookupDefinition<TLookupEntity, TEntity> Clone()
        {
            return BaseClone() as DemoLookupDefinition<TLookupEntity, TEntity>;
        }

        public override void CopyLookupData(LookupDefinitionBase source)
        {
            if (source is DemoLookupDefinition<TLookupEntity, TEntity> demoLookupDefinition)
            {
                TopHeader = demoLookupDefinition.TopHeader;
            }
            base.CopyLookupData(source);
        }
    }
}
