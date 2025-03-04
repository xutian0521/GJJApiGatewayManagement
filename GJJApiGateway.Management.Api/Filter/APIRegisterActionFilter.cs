using GJJApiGateway.Management.Common.Attributes;
using GJJApiGateway.Management.Common.Constants;
using GJJApiGateway.Management.Model.Entities;
using JWT.Serializers;
using JWT;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore.SqlServer.Storage.Internal;
using Microsoft.Extensions.Caching.Memory;
using GJJApiGateway.Management.Application.APIAuthService.Interfaces;

namespace GJJApiGateway.Management.Api.Filter
{
    /// <summary>
    /// API注册过滤器
    /// </summary>
    public class APIRegisterActionFilter : IActionFilter
    {
        private readonly IAuthorizationService _apiAuthorizationService;

        public APIRegisterActionFilter(IAuthorizationService apiAuthorizationService)
        {
            _apiAuthorizationService = apiAuthorizationService;
        }

        public void OnActionExecuted(ActionExecutedContext context)
        {
            // 过滤器执行后的逻辑（目前为空）
        }

        public async void OnActionExecuting(ActionExecutingContext context)
        {
            // 需要API注册验证
            if (context.ActionDescriptor.EndpointMetadata.Any(x => x.GetType() == typeof(EnableAPIRegisterAttribute)))
            {
                // 获取控制器名称
                var controllerName = context.ActionDescriptor.RouteValues["controller"];
                // 获取方法名称
                var actionName = context.ActionDescriptor.RouteValues["action"];

                // 拼接ApiURL
                string apiPath = $"api/{controllerName}/{actionName}";
                context.HttpContext.Items.Add("ApiPath", apiPath);

                // 获取API信息
                var apiInfo = _apiAuthorizationService.GetApiInfoByApiPathAsync(apiPath).Result;
                if (apiInfo == null)
                {
                    context.Result = new ContentResult
                    {
                        Content = "API注册未初始化该接口",
                        StatusCode = 401
                    };
                    return;
                }

                // 校验API状态
                if (apiInfo.Status != ApiOnlineStatusConst.已上线)
                {
                    context.Result = new ContentResult
                    {
                        Content = "API未上线",
                        StatusCode = 401
                    };
                    return;
                }

                if (!apiInfo.IsEnabled)
                {
                    context.Result = new ContentResult
                    {
                        Content = "API未启用",
                        StatusCode = 401
                    };
                    return;
                }

                if (context.HttpContext.Items.TryGetValue("JwtToken", out var jwtToken))
                {
                    // 校验JWT Token
                    if (_apiAuthorizationService.ValidateJwtToken(jwtToken?.ToString(), out var payload))
                    {
                        // 校验Token有效性
                        if (payload.exp < (DateTime.UtcNow - new DateTime(1970, 1, 1)).TotalSeconds)
                        {
                            context.Result = new ContentResult
                            {
                                Content = "Token 已过期",
                                StatusCode = 401
                            };
                            return;
                        }

                        //// 校验API授权
                        //int applicationId = payload.applicationId;
                        //if (!await _apiAuthorizationService.IsApiAuthorized(applicationId, apiPath, payload))
                        //{
                        //    context.Result = new ContentResult
                        //    {
                        //        Content = $"API未授权: {apiPath}",
                        //        StatusCode = 401
                        //    };
                        //    return;
                        //}
                    }
                    else
                    {
                        context.Result = new ContentResult
                        {
                            Content = "JWT验证失败",
                            StatusCode = 401
                        };
                        return;
                    }
                }
            }
        }
    }



}
