using Newtonsoft.Json;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using GJJApiGateway.Management.Common.DTOs;


namespace GJJApiGateway.Management.Common.Utilities
{
    /// <summary>
    /// ApiRegistrationHelper 提供扫描程序集生成 API 注册信息的公共方法。
    /// 此部分与业务逻辑无关，放在 Common 层。
    /// </summary>
    public static class ApiRegistrationHelper
    {
        /// <summary>
        /// 扫描当前程序集，识别带 EnableAPIRegister 特性的 Controller 方法，并生成需要新增的 CommonApiInfoDto 列表。
        /// </summary>
        /// <param name="existingApis">已有的 API 注册信息（CommonApiInfoDto 列表）</param>
        /// <param name="targetNamespace">要扫描的 Controller 命名空间</param>
        /// <returns>返回需要新增的 CommonApiInfoDto 列表</returns>
        public static List<CommonApiInfoDto> ScanControllersForApiInfo(IEnumerable<CommonApiInfoDto> existingApis, string targetNamespace)
        {
            List<CommonApiInfoDto> addList = new List<CommonApiInfoDto>();

            // 获取当前执行的程序集
            Assembly assembly = Assembly.GetExecutingAssembly();
            // 获取所有类型
            Type[] types = assembly.GetTypes();
            // 筛选出继承自 ControllerBase 且在指定命名空间中的类
            var controllerTypes = types.Where(type =>
                typeof(Microsoft.AspNetCore.Mvc.ControllerBase).IsAssignableFrom(type) &&
                type.Namespace == targetNamespace);

            foreach (Type controller in controllerTypes)
            {
                // 检查控制器是否带有 EnableAPIRegister 特性
                var enableAttr = controller.GetCustomAttribute<GJJApiGateway.Management.Common.Attributes.EnableAPIRegisterAttribute>();
                if (enableAttr == null)
                {
                    continue;
                }
                Console.WriteLine($"Controller: {controller.Name}");
                // 获取该 Controller 中所有公共方法（只获取声明在该类中的方法）
                MethodInfo[] methods = controller.GetMethods(BindingFlags.Instance | BindingFlags.Public | BindingFlags.DeclaredOnly);
                foreach (MethodInfo method in methods)
                {
                    Console.WriteLine($"-- Method: {method.Name}");
                    // 使用 CommonApiInfoDto 替代 ApiInfo 实体
                    CommonApiInfoDto apiInfo = new CommonApiInfoDto
                    {
                        ApiPath = $"api/{controller.Name.Replace("Controller", "")}/{method.Name}",
                        ApiChineseName = method.Name,
                        ApiType = "",
                        Body = "",
                        Parameter = "",
                        Description = "",
                        ResultStruct = "",
                        ServiceStatus = 0,
                        CreateTime = DateTime.Now
                    };

                    // 处理方法参数
                    ParameterInfo[] parameters = method.GetParameters();
                    foreach (var parameter in parameters)
                    {
                        if (typeof(Microsoft.AspNetCore.Http.IFormFile).IsAssignableFrom(parameter.ParameterType) ||
                            typeof(Microsoft.AspNetCore.Http.IFormFileCollection).IsAssignableFrom(parameter.ParameterType))
                        {
                            apiInfo.Body += $"{parameter.Name}: {parameter.ParameterType.Name}, ";
                        }
                        else
                        {
                            var fromBodyAttr = parameter.GetCustomAttribute<Microsoft.AspNetCore.Mvc.FromBodyAttribute>();
                            var fromQueryAttr = parameter.GetCustomAttribute<Microsoft.AspNetCore.Mvc.FromQueryAttribute>();
                            if (fromBodyAttr != null)
                            {
                                if (!parameter.ParameterType.IsPrimitive && parameter.ParameterType != typeof(string))
                                {
                                    var exampleInstance = Activator.CreateInstance(parameter.ParameterType);
                                    var serializedExample = JsonConvert.SerializeObject(exampleInstance);
                                    apiInfo.Body += $"{parameter.Name}: {serializedExample}, ";
                                }
                                else
                                {
                                    apiInfo.Body += $"{parameter.Name}: {parameter.ParameterType.Name}, ";
                                }
                            }
                            else if (fromQueryAttr != null)
                            {
                                apiInfo.Parameter += $"{parameter.Name}: {parameter.ParameterType.Name}, ";
                            }
                        }
                    }

                    // 处理返回类型（通过 [ProducesResponseType] 特性）
                    var responseTypes = method.GetCustomAttributes<Microsoft.AspNetCore.Mvc.ProducesResponseTypeAttribute>(inherit: false);
                    foreach (var responseType in responseTypes)
                    {
                        if (responseType.Type != null)
                        {
                            object instance = CreateInstanceOfType(responseType.Type);
                            apiInfo.ResultStruct += JsonConvert.SerializeObject(instance, Formatting.Indented) + "; ";
                        }
                    }

                    apiInfo.Body = apiInfo.Body.TrimEnd(',', ' ');
                    apiInfo.Parameter = apiInfo.Parameter.TrimEnd(',', ' ');

                    // 如果已有列表中没有相同 ApiPath，则新增
                    if (!existingApis.Any(x => x.ApiPath.Contains(apiInfo.ApiPath)))
                    {
                        addList.Add(apiInfo);
                    }
                }
            }
            return addList;
        }



        private static object CreateInstanceOfType(Type type, HashSet<Type> visitedTypes = null)
        {
            visitedTypes ??= new HashSet<Type>();
            if (visitedTypes.Contains(type))
            {
                return null;
            }
            visitedTypes.Add(type);
            if (type == typeof(string) || type.IsValueType)
            {
                return type.IsValueType ? Activator.CreateInstance(type) : null;
            }
            else if (type.IsGenericType)
            {
                return CreateInstanceOfGenericType(type, visitedTypes);
            }
            else if (type.IsClass)
            {
                var instance = Activator.CreateInstance(type);
                foreach (var property in type.GetProperties(BindingFlags.Public | BindingFlags.Instance))
                {
                    if (property.CanWrite && property.GetSetMethod(true) != null && !visitedTypes.Contains(property.PropertyType))
                    {
                        var propertyValue = CreateInstanceOfType(property.PropertyType, new HashSet<Type>(visitedTypes));
                        property.SetValue(instance, propertyValue);
                    }
                }
                return instance;
            }
            return null;
        }

        private static object CreateInstanceOfGenericType(Type type, HashSet<Type> visitedTypes)
        {
            Type[] genericArguments = type.GetGenericArguments();
            Type genericTypeDefinition = type.GetGenericTypeDefinition();
            object[] genericArgsInstances = genericArguments
                .Select(arg => CreateInstanceOfType(arg, new HashSet<Type>(visitedTypes)))
                .ToArray();

            // 这里不再针对 s_ApiResult<> 特殊处理，因为 s_ApiResult<> 由上面的 CreateControllerReturnValue 专门处理
            object instance = Activator.CreateInstance(type);
            var contentProperty = type.GetProperty("Content");
            if (contentProperty != null && genericArgsInstances.Length > 0)
            {
                contentProperty.SetValue(instance, genericArgsInstances[0]);
            }
            return instance;
        }
    }
}
