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
    public class SysUserInfoMapping : IEntityTypeConfiguration<SysUserInfo>
    {
        public void Configure(EntityTypeBuilder<SysUserInfo> builder)
        {
            builder.ToTable("SysUserInfo");
            builder.HasKey(a => a.ID);
        }
    }
}
