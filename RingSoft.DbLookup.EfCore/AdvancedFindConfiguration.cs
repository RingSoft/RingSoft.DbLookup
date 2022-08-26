using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace RingSoft.DbLookup.EfCore
{
    public class AdvancedFindConfiguration : IEntityTypeConfiguration<AdvancedFind.AdvancedFind>
    {
        public void Configure(EntityTypeBuilder<AdvancedFind.AdvancedFind> builder)
        { 
        }
    }
}
