using GJJApiGateway.Management.Infrastructure.Repositories.Interfaces;

namespace GJJApiGateway.Management.Application.Module.Validation;

/// <summary>
/// 用户名校验模块，封装所有与用户名验证相关的逻辑。
/// </summary>
public class UserNameCheckModule: IUserNameCheckModule
{
    private readonly ISysUserInfoRepository _userRepository;  // 数据库访问依赖 (通过构造函数注入)

    public UserNameCheckModule(ISysUserInfoRepository userRepository)
    {
        _userRepository = userRepository;
    }

    /// <summary>
    /// 总体校验用户名的方法，依次调用其他校验方法。
    /// </summary>
    public async Task<UserName_ValidationResult> ValidateUserAsync(string userName)
    {
        // 1. 检查用户名是否为空
        if (IsUserNameEmpty(userName))
        {
            return new UserName_ValidationResult(false, "用户名不能为空");
        }

        // 2. 检查用户名格式是否有效（如邮箱或手机号格式）
        if (!IsUserNameFormatValid(userName))
        {
            return new UserName_ValidationResult(false, "用户名格式不正确");
        }

        // 3. 检查用户是否存在于数据库
        bool exists = await DoesUserExistAsync(userName);
        if (!exists)
        {
            return new UserName_ValidationResult(false, "用户不存在");
        }
        
        // // 4. 检查用户是否被锁定或禁用
        // bool isLockedOrDisabled = await IsUserLockedOrDisabledAsync(userName);
        // if (isLockedOrDisabled)
        // {
        //     return new UserName_ValidationResult(false, "用户已被锁定或禁用");
        // }

        // 所有校验通过
        return new UserName_ValidationResult(true, null);
    }

    /// <summary>
    /// 检查用户名是否为空或仅包含空白。
    /// </summary>
    public bool IsUserNameEmpty(string userName)
    {
        return string.IsNullOrWhiteSpace(userName);
    }

    /// <summary>
    /// 检查用户名格式是否有效（例如是否为邮箱地址或手机号）。
    /// </summary>
    public bool IsUserNameFormatValid(string userName)
    {
        // 简化的格式校验示例：包含 '@' 则视为邮箱，全为数字则视为手机号。
        bool looksLikeEmail = userName.Contains("@");
        bool looksLikePhone = userName.All(char.IsDigit);
        return looksLikeEmail || looksLikePhone;
    }

    /// <summary>
    /// 异步检查用户是否存在（查询数据库）。
    /// </summary>
    public async Task<bool> DoesUserExistAsync(string userName)
    {
        // 调用用户仓储或数据库检查用户名是否存在
        return await _userRepository.ExistsByUsernameAsync(userName);
    }
    
    // /// <summary>
    // /// 异步检查用户是否被锁定或禁用。
    // /// </summary>
    // public async Task<bool> IsUserLockedOrDisabledAsync(string userName)
    // {
    //     // 查询用户详细信息，然后检查锁定/禁用状态
    //     var user = await _userRepository.FindByUsernameAsync(userName);
    //     if (user == null) return false;  // 用户不存在的情况已在 DoesUserExistAsync 处理
    //     return user.IsLocked || user.IsDisabled;
    // }
}

/// <summary>
/// 表示校验结果的简单类。
/// </summary>
public class UserName_ValidationResult
{
    public bool IsValid { get; }
    public string ErrorMessage { get; }

    public UserName_ValidationResult(bool isValid, string errorMessage)
    {
        IsValid = isValid;
        ErrorMessage = errorMessage;
    }
}