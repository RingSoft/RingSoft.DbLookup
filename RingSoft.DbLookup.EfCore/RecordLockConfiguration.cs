using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using RingSoft.DbLookup.RecordLocking;

namespace RingSoft.DbLookup.EfCore
{
    public class RecordLockConfiguration : IEntityTypeConfiguration<RecordLock>
    {
        public void Configure(EntityTypeBuilder<RecordLock> builder)
        {
            builder.Property(p => p.Table).HasColumnType(DbConstants.StringColumnType);
            builder.Property(p => p.PrimaryKey).HasColumnType(DbConstants.StringColumnType);
            builder.Property(p => p.LockDateTime).HasColumnType(DbConstants.DateColumnType);
            builder.Property(p => p.User).HasColumnType(DbConstants.StringColumnType);

            builder.HasKey(p => new {p.Table, p.PrimaryKey});
        }
    }
}
