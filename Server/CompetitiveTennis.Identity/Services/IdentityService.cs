namespace CompetitiveTennis.Identity.Services;

using CompetitiveTennis.Models;
using Data.Models;
using Microsoft.AspNetCore.Identity;
using Models;

public class IdentityService : IIdentityService
{
    private const string InvalidErrorMessage = "Invalid credentials.";

    private readonly UserManager<User> _userManager;
    private readonly ITokenGenerator _tokenGenerator;

    public IdentityService(UserManager<User> userManager, ITokenGenerator tokenGenerator)
    {
        _userManager = userManager;
        _tokenGenerator = tokenGenerator;
    }
    public async Task<Result<User>> Register(RegisterInputModel registerInput)
    {
        if (registerInput.Email == Constants.SystemUser)
            return Result<User>.Failure(new []{ $"{Constants.SystemUser} is preserved email for the system."});
        
        var user = new User
        {
            Email = registerInput.Email,
            UserName = registerInput.Username,
            FirstName = registerInput.FirstName,
            LastName = registerInput.LastName
        };

        var identityResult = await _userManager.CreateAsync(user, registerInput.Password);

        return identityResult.Succeeded
            ? Result<User>.SuccessWith(user)
            : Result<User>.Failure(identityResult.Errors.Select(e => e.Description));
    }

    public async Task<Result<UserOutputModel>> Login(UserInputModel userInput)
    {
        var user = await GetUser(userInput);
        if (user == null)
            return InvalidErrorMessage;

        var passwordValid = await _userManager.CheckPasswordAsync(user, userInput.Password);
        if (!passwordValid)
            return InvalidErrorMessage;

        var roles = await _userManager.GetRolesAsync(user);
        var isAdministrator = roles.Any(r => r == Constants.AdministratorRoleName);

        var token = _tokenGenerator.GenerateToken(user, roles);

        return new UserOutputModel(token, user.UserName ?? user.Email, isAdministrator);
    }

    public async Task<Result> ChangePassword(string userId, ChangePasswordInputModel changePasswordInput)
    {
        var user = await _userManager.FindByIdAsync(userId);
        if (user == null)
            return InvalidErrorMessage;

        var identityResult = await _userManager.ChangePasswordAsync(
            user,
            changePasswordInput.CurrentPassword,
            changePasswordInput.NewPassword);

        return identityResult.Succeeded
            ? Result.Success
            : Result.Failure(identityResult.Errors.Select(e => e.Description));
    }

    public async Task<Result> ChangeNames(string userId, ChangeNamesInputModel changeNamesInput)
    {
        var user = await _userManager.FindByIdAsync(userId);
        if (user == null)
            return InvalidErrorMessage;

        //ToDo: Check for XSS or sql injection
        user.FirstName = changeNamesInput.FirstName;
        user.LastName = changeNamesInput.LastName;

        var identityResult = await _userManager.UpdateAsync(user);

        return identityResult.Succeeded
            ? Result.Success
            : Result.Failure(identityResult.Errors.Select(e => e.Description));
    }
    
    private async Task<User?> GetUser(UserInputModel inputModel) 
        => inputModel.EmailLogin ?
            await _userManager.FindByEmailAsync(inputModel.LoginInfo) :
            await _userManager.FindByNameAsync(inputModel.LoginInfo);
}