using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Persistence.Configurations
{
    public class TypeSaleConfiguration : IEntityTypeConfiguration<TypeSale>
    {
        public void Configure(EntityTypeBuilder<TypeSale> builder)
        {
            builder.ToTable("TypeSales");

            builder.HasKey(ts => ts.Id);

            builder.Property(ts => ts.Id)
                .HasMaxLength(36);

            builder.Property(ts => ts.Name)
                .HasMaxLength(100)
                .IsRequired();

            builder.Property(ts => ts.Description)
                .HasMaxLength(255);
        }
    }
}
