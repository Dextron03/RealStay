using System;
using Domain.Entities;
using Infrastructure.Identity.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Persistence.Configurations
{
    public class WishListConfiguration : IEntityTypeConfiguration<WishList>
    {
        public void Configure(EntityTypeBuilder<WishList> builder)
        {
            builder.ToTable("WishLists");

            builder.HasKey(w => w.Id);

            builder.Property(w => w.Id)
                .HasMaxLength(36);

            builder.Property(w => w.DateRegistration)
                .IsRequired();

            // Client relationship (AppUser)
            builder.HasOne<AppUser>()
                .WithMany(u => u.WishLists)
                .HasForeignKey(w => w.ClientId)
                .OnDelete(DeleteBehavior.Restrict);

            // Property relationship
            builder.HasOne(w => w.Property)
                .WithMany()
                .HasForeignKey(w => w.PropertyId)
                .OnDelete(DeleteBehavior.Cascade);
        }
    }
}
