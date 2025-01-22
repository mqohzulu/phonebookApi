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

    public class RoleConfiguration : IEntityTypeConfiguration<Role>
    {
        public void Configure(EntityTypeBuilder<Role> builder)
        {
            builder.HasKey(r => r.Id);

            builder.Property(r => r.Name)
                .HasMaxLength(50)
                .IsRequired();

            builder.Property(r => r.Description)
                .HasMaxLength(200);

            builder.Property(r => r.IsDefault)
                .HasDefaultValue(false);

            builder.HasIndex(r => r.Name)
                .IsUnique();

            builder.HasData(
                Role.Create("Admin", "System Administrator", true).Value,
                Role.Create("User", "Regular User", true).Value
            );
        }
    }
}
