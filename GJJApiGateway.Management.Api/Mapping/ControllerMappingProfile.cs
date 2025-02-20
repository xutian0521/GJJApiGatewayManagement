using AutoMapper;
using GJJApiGateway.Management.Api.DTOs;
using GJJApiGateway.Management.Api.ViewModel;
using GJJApiGateway.Management.Application.DTOs;       // 控制器层的 DTO 或 ViewModel，例如 ApiInfoViewModel

namespace GJJApiGateway.Management.Api.Mappings
{
    /// <summary>
    /// ControllerMappingProfile 定义了业务层 DTO 与控制器层 ViewModel（或 API 层 DTO）之间的映射关系。
    /// 此配置文件位于 API 层，Application 层不引用此配置。
    /// </summary>
    public class ControllerMappingProfile : Profile
    {
        public ControllerMappingProfile()
        {
            // 将业务层 A_ApiInfoDto 映射为 API 层 ApiInfoViewModel
            CreateMap<A_ApiInfoDto, ApiInfoVM>().ReverseMap();

            // 将业务层 A_ApiApplicationDto 映射为 API 层 ApiApplicationViewModel
            CreateMap<A_ApiApplicationDto, ApiApplicationVM>().ReverseMap();

            // 其它 DTO 映射
            CreateMap<A_ApiConfigurationDto, C_ApiConfigurationDto>().ReverseMap();
            CreateMap<A_ApiAuthorizationRequestDto, C_ApiAuthorizationRequestDto>().ReverseMap();
            CreateMap<A_ApiInfoDto, C_ApiInfoDto>().ReverseMap();
            CreateMap<A_ApiApplicationDto, C_ApiApplicationDto>().ReverseMap();
            CreateMap<A_LoginRequestDto, C_LoginRequestDto>().ReverseMap();
            CreateMap<A_LoginResponseDto, LoginResponseVM>().ReverseMap();
            CreateMap<A_SysUserInfoDto, SysUserInfoVM>().ReverseMap();
            CreateMap<A_SysMenuDto, C_SysMenuDto>().ReverseMap();
            CreateMap<A_SysMenuDto, SysMenuVM>().ReverseMap();
            CreateMap<A_ApiApplicationDto, ApiApplicationVM>().ReverseMap();
            
            CreateMap<PageResult<A_ApiApplicationDto>, Pager<ApiApplicationVM>>()
            .ConvertUsing<PageResultToPagerConverter<A_ApiApplicationDto, ApiApplicationVM>>();
            CreateMap<PageResult<A_ApiInfoDto>, Pager<ApiInfoVM>>()
                .ConvertUsing<PageResultToPagerConverter<A_ApiInfoDto, ApiInfoVM>>();
        }
    }

    /// <summary>
    /// 将业务层分页 DTO (PageResult<TSource>) 转换为控制器层分页 ViewModel (Pager<TDestination>)，
    /// 使用 AutoMapper 将 TSource 转换为 TDestination。
    /// </summary>
    public class PageResultToPagerConverter<TSource, TDestination> : ITypeConverter<PageResult<TSource>, Pager<TDestination>>
    {
        public Pager<TDestination> Convert(PageResult<TSource> source, Pager<TDestination> destination, ResolutionContext context)
        {
            var list = context.Mapper.Map<IEnumerable<TDestination>>(source.Items);
            return new Pager<TDestination>
            {
                List = list.ToList(),
                Total = source.Total
            };
        }
    }
}
