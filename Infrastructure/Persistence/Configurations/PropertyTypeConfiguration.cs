using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Persistence.Configurations
{
    public class PropertyTypeConfiguration : IEntityTypeConfiguration<PropertyType>
    {
        public void Configure(EntityTypeBuilder<PropertyType> builder)
        {
            builder.ToTable("PropertyTypes");

            builder.HasKey(pt => pt.Id);

            builder.Property(pt => pt.Id)
                .HasMaxLength(36);

            builder.Property(pt => pt.Name)
                .HasMaxLength(100)
                .IsRequired();

            builder.Property(pt => pt.Description)
                .HasMaxLength(255);
        }
    }
}
