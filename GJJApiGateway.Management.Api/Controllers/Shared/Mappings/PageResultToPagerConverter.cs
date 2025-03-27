using AutoMapper;
using GJJApiGateway.Management.Api.Controllers.Shared.ViewModels;
using GJJApiGateway.Management.Application.Shared.DTOs;

namespace GJJApiGateway.Management.Api.Controllers.Shared.Mappings;

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