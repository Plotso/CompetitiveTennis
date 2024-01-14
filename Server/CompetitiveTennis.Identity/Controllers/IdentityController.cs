namespace CompetitiveTennis.Identity.Controllers;

using CompetitiveTennis.Controllers;
using CompetitiveTennis.Models;
using CompetitiveTennis.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Models;
using Services;

public class IdentityController : ApiController
{
    private readonly IIdentityService _identity;
    private readonly ICurrentUserService _currentUser;

    public IdentityController(IIdentityService identity, ICurrentUserService currentUser)
    {
        _identity = identity;
        _currentUser = currentUser;
    }

    [HttpPost]
    [Route(nameof(Register))]
    public async Task<ActionResult<Result<FullUserOutputModel>>> Register(RegisterInputModel input)
    {
        var result = await _identity.Register(input);
        var userInputModel = result.IsSuccess
            ? new UserInputModel()
            {
                LoginInfo = input.Username ?? input.Email,
                Password = input.Password,
                EmailLogin = input.Username == null
            }
            : null;
        return result.IsSuccess ? await Login(userInputModel) : BadRequest(result.Errors);
    }

    [HttpPost]
    [Route(nameof(Login))]
    public async Task<ActionResult<Result<FullUserOutputModel>>> Login(UserInputModel input)
    {
        var result = await _identity.Login(input);
        return result.IsSuccess ? result : Result<FullUserOutputModel>.Failure(result.Errors);
    }

    [HttpPut]
    [Authorize]
    [Route(nameof(ChangePassword))]
    public async Task<ActionResult> ChangePassword(ChangePasswordInputModel input)
        => await _identity.ChangePassword(_currentUser.UserId, input);

    [HttpPut]
    [Authorize]
    [Route(nameof(ChangeNames))]
    public async Task<ActionResult> ChangeNames(ChangeNamesInputModel input)
        => await _identity.ChangeNames(_currentUser.UserId, input);
}