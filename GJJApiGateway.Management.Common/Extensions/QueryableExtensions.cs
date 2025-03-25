namespace GJJApiGateway.Management.Common.Extensions;

// 扩展方法（用于条件过滤）
public static class QueryableExtensions
{
    public static IEnumerable<T> WhereIf<T>(this IEnumerable<T> source, bool condition, Func<T, bool> predicate)
    {
        return condition ? source.Where(predicate) : source;
    }
}