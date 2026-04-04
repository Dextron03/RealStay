using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Persistence.Configurations
{
    public class PropertyImprovementConfiguration : IEntityTypeConfiguration<PropertyImprovement>
    {
        public void Configure(EntityTypeBuilder<PropertyImprovement> builder)
        {
            builder.ToTable("PropertyImprovementsTypes"); // To distinguish from the junction table

            builder.HasKey(pi => pi.Id);

            builder.Property(pi => pi.Id)
                .HasMaxLength(36);

            builder.Property(pi => pi.Name)
                .HasMaxLength(100)
                .IsRequired();

            builder.Property(pi => pi.Description)
                .HasMaxLength(255);
        }
    }
}
