namespace CompetitiveTennis.Identity.Services;

using CompetitiveTennis.Models;
using Data.Models;
using Models;

public interface IIdentityService
{
    Task<Result<User>> Register(RegisterInputModel registerInput);

    Task<Result<FullUserOutputModel>> Login(UserInputModel userInputModel);

    Task<Result> ChangePassword(string userId, ChangePasswordInputModel changePasswordInput);

    Task<Result> ChangeNames(string userId, ChangeNamesInputModel changeNamesInput);
}