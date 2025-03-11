using AutoMapper;
using GJJApiGateway.Management.Api.Controllers.Account.DTOs;
using GJJApiGateway.Management.Api.Controllers.Account.ViewModels;
using GJJApiGateway.Management.Api.Controllers.APIAuth.DTOs;
using GJJApiGateway.Management.Api.Controllers.APIAuth.ViewModels;
using GJJApiGateway.Management.Api.Controllers.Shared.ViewModels;
using GJJApiGateway.Management.Application.AccountService.DTOs;
using GJJApiGateway.Management.Application.APIAuthService.DTOs;
using GJJApiGateway.Management.Application.RuleService.DTOs;
using GJJApiGateway.Management.Application.Shared.DTOs;

namespace GJJApiGateway.Management.Api.Controllers.APIAuth.Mappings;

public class C_APIAuthControllerMappingProfile: Profile
{
    public C_APIAuthControllerMappingProfile()
    {
        CreateMap<A_ApiInfoDto, ApiInfoVM>().ReverseMap();
        CreateMap<A_ApiAuthorizationRequestDto, C_ApiAuthorizationRequestDto>().ReverseMap();
        CreateMap<A_ApiInfoDto, C_ApiInfoDto>().ReverseMap();
        CreateMap<A_ApiApplicationDto, C_ApiApplicationDto>().ReverseMap();
        CreateMap<A_ApiApplicationDto, ApiApplicationVM>().ReverseMap();
        
        CreateMap<PageResult<A_ApiApplicationDto>, Pager<ApiApplicationVM>>()
            .ConvertUsing<PageResultToPagerConverter<A_ApiApplicationDto, ApiApplicationVM>>();
        CreateMap<PageResult<A_ApiInfoDto>, Pager<ApiInfoVM>>()
            .ConvertUsing<PageResultToPagerConverter<A_ApiInfoDto, ApiInfoVM>>();
    }
    
    /// <summary>
    /// 将业务层分页 DTO (PageResult<TSource>) 转换为控制器层分页 ViewModel (Pager<TDestination>)，
    /// 使用 AutoMapper 将 TSource 转换为 TDestination。
    /// </summary>
    public class PageResultToPagerConverter<TSource, TDestination> : ITypeConverter<PageResult<TSource>, Pager<TDestination>>
    {
        public Pager<TDestination> Convert(PageResult<TSource> source, Pager<TDestination> destination, ResolutionContext context)
        {
            var list = context.Mapper.Map<IEnumerable<TDestination>>(source.List);
            return new Pager<TDestination>
            {
                List = list.ToList(),
                Total = source.Total
            };
        }
    }
}