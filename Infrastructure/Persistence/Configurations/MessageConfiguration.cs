using System;
using Domain.Entities;
using Infrastructure.Identity.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Persistence.Configurations
{
    public class MessageConfiguration : IEntityTypeConfiguration<Message>
    {
        public void Configure(EntityTypeBuilder<Message> builder)
        {
            builder.ToTable("Messages"); // Consistent naming (Plural)

            builder.HasKey(m => m.Id);

            builder.Property(m => m.Id)
                .HasMaxLength(36);

            builder.Property(m => m.Content)
                .IsRequired();
            
            builder.Property(m => m.DateSend)
                .IsRequired();

            // Property relationship
            builder.HasOne(m => m.Property)
                .WithMany(p => p.Messages)
                .HasForeignKey(m => m.PropertyId)
                .OnDelete(DeleteBehavior.Cascade);

            // Sender relationship (AppUser is in Infrastructure.Identity.Entities)
            builder.HasOne<AppUser>()
                .WithMany(u => u.SentMessages)
                .HasForeignKey(m => m.SenderId)
                .OnDelete(DeleteBehavior.Restrict); // Important: No Cascade for multiple paths

            // Receiver relationship
            builder.HasOne<AppUser>()
                .WithMany(u => u.ReceivedMessages)
                .HasForeignKey(m => m.ReceiverId)
                .OnDelete(DeleteBehavior.Restrict); // Important: No Cascade
        }
    }
}
