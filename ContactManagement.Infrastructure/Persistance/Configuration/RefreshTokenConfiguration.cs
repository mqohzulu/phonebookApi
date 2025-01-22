using ContactManagement.Domain.Entities;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ContactManagement.Infrastructure.Persistance.Configuration
{
    public class RefreshTokenConfiguration : IEntityTypeConfiguration<RefreshToken>
    {
        public void Configure(EntityTypeBuilder<RefreshToken> builder)
        {
            builder.HasKey(r => r.Id);

            builder.Property(r => r.Token)
                .IsRequired()
                .HasMaxLength(256);

            builder.Property(r => r.UserId)
                .IsRequired();

            builder.Property(r => r.ExpiresAt)
                .IsRequired();

            builder.Property(r => r.CreatedAt)
                .IsRequired();

            builder.Property(r => r.RevokedBy)
                .HasMaxLength(100);

            builder.Property(r => r.ReasonRevoked)
                .HasMaxLength(200);

            builder.Property(r => r.ReplacedByToken)
                .HasMaxLength(256);

            builder.HasIndex(r => r.Token)
                .IsUnique();

            builder.HasIndex(r => r.UserId);
        }
    }
}
