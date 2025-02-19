using GJJApiGateway.Management.Model.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GJJApiGateway.Management.Infrastructure.Mappings
{
    public class SysRoleMenuMapping : IEntityTypeConfiguration<SysRoleMenu>
    {
        public void Configure(EntityTypeBuilder<SysRoleMenu> builder)
        {
            builder.ToTable("SysRoleMenu");
            builder.HasKey(a => a.ID);
        }
    }
}
