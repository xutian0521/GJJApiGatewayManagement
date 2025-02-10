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
    public class ApiInfoMapping : IEntityTypeConfiguration<ApiInfo>
    {
        public void Configure(EntityTypeBuilder<ApiInfo> builder)
        {
            builder.ToTable("ApiInfo");
            builder.HasKey(a => a.Id);
        }
    }
}
