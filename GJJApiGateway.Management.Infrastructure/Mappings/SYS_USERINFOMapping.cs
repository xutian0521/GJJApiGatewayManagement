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
    public class SYS_USERINFOMapping : IEntityTypeConfiguration<SYS_USERINFO>
    {
        public void Configure(EntityTypeBuilder<SYS_USERINFO> builder)
        {
            builder.ToTable("SYS_USERINFO");
            builder.HasKey(a => a.ID);
        }
    }
}
