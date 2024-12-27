using GJJApiGateway.Management.Model.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace GJJApiGateway.Management.Infrastructure.Mappings
{
    public class AuthorizationMapping : IEntityTypeConfiguration<Authorization>
    {
        public void Configure(EntityTypeBuilder<Authorization> builder)
        {
            builder.ToTable("Authorizations");
            builder.HasKey(a => a.Id);
            builder.Property(a => a.Role)
                   .IsRequired()
                   .HasMaxLength(100);
            builder.Property(a => a.AllowedEndpoints)
                   .IsRequired()
                   .HasMaxLength(1000);
            builder.Property(a => a.CreatedAt)
                   .IsRequired();
            builder.Property(a => a.UpdatedAt)
                   .IsRequired();
        }
    }
}
