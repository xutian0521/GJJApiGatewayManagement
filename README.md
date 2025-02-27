GJJApiGatewayManagement.sln
��
������ ApiGateway.Management.API                         // ְ����Ϊ����ϵͳ�ĺ�� API ��Ŀ������������Ȩ������ص� HTTP �����紴�������¡�ɾ����Ȩ����
��   ������ Controllers                                   // �������� HTTP ����Ŀ�������
��   ��   ������ AuthorizationController.cs                // ������Ȩ��ص� API �����紴����Ȩ���򡢸�����Ȩ����ȡ�
��   ������ DTOs                                          // �������ݴ���������ڿ����������֮������ݽ�����
��   ��   ������ CreateAuthorizationDto.cs
��   ��   ������ UpdateAuthorizationDto.cs
��   ��   ������ AuthorizationDto.cs
��   ������ Services                                      // ����ҵ���߼����񣬵��� Application ��ķ���ӿڡ�
��   ��   ������ AuthorizationService.cs
��   ������ Mapping                                       // ���� AutoMapper������ DTO ��ʵ��/��ͼģ��֮���ӳ�䡣
��   ��   ������ MappingProfile.cs
��   ������ ApiGateway.Management.API.csproj              // ��Ŀ�ļ�
��   ������ appsettings.json                              // �����ļ�
��
������ ApiGateway.Management.Application                 // ְ�𣺴�������ϵͳ��ҵ���߼�����������ӿں�ʵ�֡�
��   ������ Interfaces                                    // �������ӿڡ�
��   ��   ������ IAuthorizationService.cs                  // ������Ȩ�����ķ���ӿڡ�
��   ������ Services                                      // ʵ�ַ���ӿڣ����������ҵ���߼���
��   ��   ������ AuthorizationService.cs                   // ʵ�� IAuthorizationService�����������ҵ���߼�������֤��Ȩ�������Ч�ԡ����û�����ʩ��Ĳִ��ӿڽ������ݲ����ȡ�
��   ������ DTOs                                          // ���ݴ���������������ҵ���߼� DTO��
��   ������ Mapping                                       // AutoMapper ����
��   ������ ApiGateway.Management.Application.csproj      // ��Ŀ�ļ�
��
������ ApiGateway.Management.Model                       // ְ�𣺴�Ź���ϵͳ�ĺ���ʵ�����ͼģ�͡�
��   ������ Entities                                      // ��������ҵ��ʵ�塣
��   ��   ������ Authorization.cs                          // ������Ȩ�����ʵ���࣬������ Id��Role��AllowedEndpoints �����ԡ�
��   ������ ViewModels                                    // ��������ǰ��չʾ�����ݽṹ��
��   ��   ������ AuthorizationViewModel.cs                 // ��������ǰ��չʾ����Ȩ������ͼģ�͡�
��   ������ ApiGateway.Management.Model.csproj            // ��Ŀ�ļ�
��
������ ApiGateway.Management.Infrastructure              // ְ�𣺸������ݷ��ʣ����������ݿ�Ľ������ִ�ģʽ��ʵ�ֵȡ�
��   ������ Repositories                                  // ���ݲִ��ӿں�ʵ��
��   ��   ������ Interfaces                                // �������ݲִ��ӿڡ�
��   ��   ��   ������ IAuthorizationRepository.cs           // ������Ȩ����Ĳִ��ӿڡ�
��   ��   ������ AuthorizationRepository.cs                // ʵ�� IAuthorizationRepository��������������ݷ����߼���
��   ������ DbContext                                     // �������ݿ������ģ����� DbSet<Authorization>��
��   ��   ������ ManagementDbContext.cs                    // �������ݿ������ģ����� Authorization ʵ�塣
��   ������ Mappings                                      // ����ʵ�������ݿ����ӳ��
��   ��   ������ AuthorizationMapping.cs                   // ���� Authorization ʵ������ݿ�ӳ�����á�
��   ������ ApiGateway.Management.Infrastructure.csproj   // ��Ŀ�ļ�
��
������ ApiGateway.Management.Common                      // ְ�𣺴�Ź���ϵͳ��ͨ�ù��ߡ���չ�����ͳ�����
��   ������ Extensions                                    // ������չ�����������ע����չ������
��   ��   ������ ServiceCollectionExtensions.cs            // �������ע�����չ������������ Program.cs ��ע�����
��   ������ Utilities                                     // ����ͨ�ù����࣬����־��¼���ߡ�
��   ��   ������ Logger.cs                                 // ʵ��ͳһ����־��¼���ܡ�
��   ������ Constants                                     // ����ȫ�ֳ������塣
��   ��   ������ AppConstants.cs                           // ����ȫ�ֳ��������ɫ���ơ�Ĭ�����õȡ�
��   ������ ApiGateway.Management.Common.csproj           // ��Ŀ�ļ�
��
��
������ README.md                                         // �����������ĵ�


������ ApiGateway.Management.UI                          // ְ����Ϊ����ϵͳ��ǰ����Ŀ���ṩ�û��������ڹ��� API ��Ȩ�����á�
��   ������ public                                        // ��ž�̬��Դ���� index.html��ͼƬ�ȡ�
��   ������ index.html
��   ������ src                                           // Դ����
��   ��   ������ components                                // ���(�ɸ��õ� UI �����)
��   ��   ������ AuthorizationForm.js                      // ʵ����Ȩ������ҳ���߼��� UI��
��   ��   ������ pages                                     // ҳ��(��ͬ��ҳ�棬����Ȩ����ҳ�桢��־�鿴ҳ��ȡ�)
��   ��   ������ AuthorizationManagementPage.js            // ��װ����� API ��˽����ĺ�����
��   ��   ������ services                                  // ���� API �����ķ���(���� API �����ķ����������Ȩ���� API �ĺ�����)
��   ��   ������ authorizationService.js
��   ��   ������ App.js                                    // ��Ӧ����� (��Ӧ�����������·�ɺ�ȫ��״̬��)
��   ��   ������ index.js                                  // ����ļ� (ǰ��Ӧ�õ�����ļ���)
��   ������ package.json                                  // ����ǰ�������ͽű���
��   ������ README.md                                     // ǰ����Ŀ�ĵ�
