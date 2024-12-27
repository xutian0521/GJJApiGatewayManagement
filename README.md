GJJApiGatewayManagement.sln
│
├── ApiGateway.Management.API                         // 职责：作为管理系统的后端 API 项目，负责处理与授权管理相关的 HTTP 请求，如创建、更新、删除授权规则。
│   ├── Controllers                                   // 包含处理 HTTP 请求的控制器。
│   │   └── AuthorizationController.cs                // 处理授权相关的 API 请求，如创建授权规则、更新授权规则等。
│   ├── DTOs                                          // 定义数据传输对象，用于控制器与服务之间的数据交换。
│   │   ├── CreateAuthorizationDto.cs
│   │   ├── UpdateAuthorizationDto.cs
│   │   └── AuthorizationDto.cs
│   ├── Services                                      // 包含业务逻辑服务，调用 Application 层的服务接口。
│   │   └── AuthorizationService.cs
│   ├── Mapping                                       // 配置 AutoMapper，用于 DTO 和实体/视图模型之间的映射。
│   │   └── MappingProfile.cs
│   ├── ApiGateway.Management.API.csproj              // 项目文件
│   └── appsettings.json                              // 配置文件
│
├── ApiGateway.Management.Application                 // 职责：处理管理系统的业务逻辑，包含服务接口和实现。
│   ├── Interfaces                                    // 定义服务接口。
│   │   └── IAuthorizationService.cs                  // 定义授权管理的服务接口。
│   ├── Services                                      // 实现服务接口，包含具体的业务逻辑。
│   │   └── AuthorizationService.cs                   // 实现 IAuthorizationService，包含具体的业务逻辑，如验证授权规则的有效性、调用基础设施层的仓储接口进行数据操作等。
│   ├── DTOs                                          // 数据传输对象（如果有其他业务逻辑 DTO）
│   ├── Mapping                                       // AutoMapper 配置
│   └── ApiGateway.Management.Application.csproj      // 项目文件
│
├── ApiGateway.Management.Model                       // 职责：存放管理系统的核心实体和视图模型。
│   ├── Entities                                      // 包含核心业务实体。
│   │   └── Authorization.cs                          // 定义授权规则的实体类，包含如 Id、Role、AllowedEndpoints 等属性。
│   ├── ViewModels                                    // 定义用于前端展示的数据结构。
│   │   └── AuthorizationViewModel.cs                 // 定义用于前端展示的授权规则视图模型。
│   └── ApiGateway.Management.Model.csproj            // 项目文件
│
├── ApiGateway.Management.Infrastructure              // 职责：负责数据访问，包括与数据库的交互，仓储模式的实现等。
│   ├── Repositories                                  // 数据仓储接口和实现
│   │   ├── Interfaces                                // 定义数据仓储接口。
│   │   │   └── IAuthorizationRepository.cs           // 定义授权规则的仓储接口。
│   │   └── AuthorizationRepository.cs                // 实现 IAuthorizationRepository，包含具体的数据访问逻辑。
│   ├── DbContext                                     // 定义数据库上下文，包含 DbSet<Authorization>。
│   │   └── ManagementDbContext.cs                    // 定义数据库上下文，配置 Authorization 实体。
│   ├── Mappings                                      // 配置实体与数据库表的映射
│   │   └── AuthorizationMapping.cs                   // 定义 Authorization 实体的数据库映射配置。
│   └── ApiGateway.Management.Infrastructure.csproj   // 项目文件
│
├── ApiGateway.Management.Common                      // 职责：存放管理系统的通用工具、扩展方法和常量。
│   ├── Extensions                                    // 包含扩展方法，如服务注册扩展方法。
│   │   └── ServiceCollectionExtensions.cs            // 定义服务注册的扩展方法，用于在 Program.cs 中注册服务。
│   ├── Utilities                                     // 包含通用工具类，如日志记录工具。
│   │   └── Logger.cs                                 // 实现统一的日志记录功能。
│   ├── Constants                                     // 包含全局常量定义。
│   │   └── AppConstants.cs                           // 定义全局常量，如角色名称、默认配置等。
│   └── ApiGateway.Management.Common.csproj           // 项目文件
│
│
└── README.md                                         // 整体解决方案文档


├── ApiGateway.Management.UI                          // 职责：作为管理系统的前端项目，提供用户界面用于管理 API 授权和配置。
│   ├── public                                        // 存放静态资源，如 index.html、图片等。
│   ├── index.html
│   ├── src                                           // 源代码
│   │   ├── components                                // 组件(可复用的 UI 组件。)
│   │   ├── AuthorizationForm.js                      // 实现授权管理的页面逻辑和 UI。
│   │   ├── pages                                     // 页面(不同的页面，如授权管理页面、日志查看页面等。)
│   │   ├── AuthorizationManagementPage.js            // 封装与管理 API 后端交互的函数。
│   │   ├── services                                  // 与后端 API 交互的服务(与后端 API 交互的服务，如调用授权管理 API 的函数。)
│   │   ├── authorizationService.js
│   │   ├── App.js                                    // 主应用组件 (主应用组件，配置路由和全局状态。)
│   │   └── index.js                                  // 入口文件 (前端应用的入口文件。)
│   ├── package.json                                  // 定义前端依赖和脚本。
│   └── README.md                                     // 前端项目文档
