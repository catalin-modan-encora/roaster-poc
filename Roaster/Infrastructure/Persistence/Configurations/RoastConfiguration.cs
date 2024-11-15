using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Roaster.Infrastructure.Persistence.Models;

namespace Roaster.Infrastructure.Persistence.Configurations
{
    internal sealed class RoastConfiguration : IEntityTypeConfiguration<Roast>
    {
        public void Configure(EntityTypeBuilder<Roast> builder)
        {
            builder.HasKey(b => b.Id);

            builder
                .Property(b => b.Name)
                .HasMaxLength(64)
                .IsRequired();

            builder.HasIndex(b => b.Name).IsClustered(true).IncludeProperties("Name");
        }
    }
}
