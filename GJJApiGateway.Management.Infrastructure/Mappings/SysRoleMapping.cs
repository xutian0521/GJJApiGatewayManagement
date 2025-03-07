using GJJApiGateway.Management.Model.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace GJJApiGateway.Management.Infrastructure.Mappings;

public class SysRoleMapping: IEntityTypeConfiguration<SysRole>
{
    public void Configure(EntityTypeBuilder<SysRole> builder)
    {
        builder.ToTable("SysRole");
        builder.HasKey(a => a.ID);
    }
}