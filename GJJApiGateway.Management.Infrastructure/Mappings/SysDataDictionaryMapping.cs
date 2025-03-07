using GJJApiGateway.Management.Model.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace GJJApiGateway.Management.Infrastructure.Mappings;

public class SysDataDictionaryMapping: IEntityTypeConfiguration<SysDataDictionary>
{
    public void Configure(EntityTypeBuilder<SysDataDictionary> builder)
    {
        builder.ToTable("SysDataDictionary");
        builder.HasKey(a => a.Id);
    }
}