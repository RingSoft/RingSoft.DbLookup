using System.Data.Entity.ModelConfiguration;
using RingSoft.DbLookup.App.Library.MegaDb.Model;

namespace RingSoft.DbLookup.App.Library.Ef6.MegaDb.Configurations
{
    public class StockConfiguration : EntityTypeConfiguration<StockMaster>
    {
        public StockConfiguration()
        {
            HasKey(p => new {p.StockNumber, p.Location});
            Property(p => p.StockNumber).IsRequired();
            Property(p => p.Location).IsRequired();
        }
    }
}
