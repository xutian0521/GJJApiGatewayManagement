namespace GJJApiGateway.Management.Application.Module.Validation;


public interface IUserNameCheckModule
{
    Task<UserName_ValidationResult> ValidateUserAsync(string userName);
    bool IsUserNameEmpty(string userName);
    bool IsUserNameFormatValid(string userName);
    Task<bool> DoesUserExistAsync(string userName);
    // Task<bool> IsUserLockedOrDisabledAsync(string userName);
}
