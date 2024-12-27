using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Emit;
using System.Text;
using System.Threading.Tasks;
using GJJApiGateway.Management.Infrastructure.Mappings;
using GJJApiGateway.Management.Model.Entities;
using Microsoft.EntityFrameworkCore;

namespace GJJApiGateway.Management.Infrastructure
{
    /// <summary>
    /// 管理后台的数据库上下文，负责与数据库的交互。
    /// </summary>
    public class ManagementDbContext : DbContext
    {
        /// <summary>
        /// 构造函数，注入数据库上下文选项。
        /// </summary>
        /// <param name="options">数据库上下文选项。</param>
        public ManagementDbContext(DbContextOptions<ManagementDbContext> options)
            : base(options)
        {
        }

        /// <summary>
        /// 授权规则实体集，对应数据库中的 "Authorizations" 表。
        /// </summary>
        public DbSet<Authorization> Authorizations { get; set; }

        /// <summary>
        /// 重写 OnModelCreating 方法，应用实体映射配置。
        /// </summary>
        /// <param name="modelBuilder">模型生成器。</param>
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            // 应用授权规则实体的映射配置
            modelBuilder.ApplyConfiguration(new AuthorizationMapping());

            // 应用其他实体的映射配置
            // 例如：
            // modelBuilder.ApplyConfiguration(new AnotherEntityMapping());
        }
    }
}
