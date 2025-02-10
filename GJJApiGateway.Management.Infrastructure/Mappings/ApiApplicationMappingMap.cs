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
    public class ApiApplicationMappingMap : IEntityTypeConfiguration<ApiApplicationMapping>
    {
        public void Configure(EntityTypeBuilder<ApiApplicationMapping> builder)
        {
            builder.ToTable("ApiApplicationMapping");
            builder.HasKey(a => a.Id);
        }
    }
}
