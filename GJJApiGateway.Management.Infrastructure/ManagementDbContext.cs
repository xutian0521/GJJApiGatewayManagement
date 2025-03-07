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


        public DbSet<ApiInfo> ApiInfos { get; set; }
        public DbSet<ApiApplication> ApiApplications { get; set; }
        public DbSet<ApiApplicationMapping> ApiApplicationMappings { get; set; }
        public DbSet<SysUserInfo> SysUserInfos { get; set; }
        public DbSet<SysMenu> SysMenus { get; set; }
        public DbSet<SysRoleMenu> SysRoleMenus { get; set; }
        public DbSet<SysRole> SysRoles { get; set; }
        public DbSet<SysDataDictionary> SysDataDictionarys { get; set; }

        
        /// <summary>
        /// 重写 OnModelCreating 方法，应用实体映射配置。
        /// </summary>
        /// <param name="modelBuilder">模型生成器。</param>
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {

            modelBuilder.ApplyConfiguration(new ApiInfoMapping());
            modelBuilder.ApplyConfiguration(new ApiApplicationMap());
            modelBuilder.ApplyConfiguration(new ApiApplicationMappingMap());
            modelBuilder.ApplyConfiguration(new SysUserInfoMapping());
            modelBuilder.ApplyConfiguration(new SysMenuMapping());
            modelBuilder.ApplyConfiguration(new SysRoleMenuMapping());
            modelBuilder.ApplyConfiguration(new SysRoleMapping());
            modelBuilder.ApplyConfiguration(new SysDataDictionaryMapping());
        }
    }
}
