using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Persistence.Configurations
{
    public class PropertyImprovementsConfiguration : IEntityTypeConfiguration<PropertyImprovements>
    {
        public void Configure(EntityTypeBuilder<PropertyImprovements> builder)
        {
            builder.ToTable("PropertyImprovements");

            builder.HasKey(pi => new { pi.PropertyId, pi.ImprovementId });

            builder.HasOne(pi => pi.Property)
                .WithMany(p => p.Improvements)
                .HasForeignKey(pi => pi.PropertyId)
                .OnDelete(DeleteBehavior.Cascade);

            builder.HasOne(pi => pi.Improvement)
                .WithMany(i => i.PropertyImprovements)
                .HasForeignKey(pi => pi.ImprovementId)
                .OnDelete(DeleteBehavior.Restrict);
        }
    }
}
