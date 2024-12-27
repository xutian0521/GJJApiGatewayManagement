using GJJApiGateway.Management.Model.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace GJJApiGateway.Management.Infrastructure.Mappings
{
    /// <summary>
    /// 授权规则实体的数据库映射配置。
    /// </summary>
    public class AuthorizationMapping : IEntityTypeConfiguration<Authorization>
    {
        /// <summary>
        /// 配置授权规则实体的数据库映射。
        /// </summary>
        /// <param name="builder">实体类型构建器。</param>
        public void Configure(EntityTypeBuilder<Authorization> builder)
        {
            // 指定映射到 "Authorizations" 表
            builder.ToTable("Authorizations");

            // 设置主键
            builder.HasKey(a => a.Id);

            // 配置 Role 属性为必填，最大长度为100
            builder.Property(a => a.Role)
                   .IsRequired()
                   .HasMaxLength(100);

            // 配置 AllowedEndpoints 属性为必填，最大长度为1000
            builder.Property(a => a.AllowedEndpoints)
                   .IsRequired()
                   .HasMaxLength(1000);

            // 配置 CreatedAt 属性为必填
            builder.Property(a => a.CreatedAt)
                   .IsRequired();

            // 配置 UpdatedAt 属性为必填
            builder.Property(a => a.UpdatedAt)
                   .IsRequired();
        }
    }
}
