using ECommerece.ServiceAbstraction;
using ECommerece.Shared.IdentityDTOS;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace ECommerece.presentation.Controllers;

public class AuthenticationController : ApiBaseController
{
    private readonly IAuthenticationService _authenticationService;

    public AuthenticationController(IAuthenticationService authenticationService)
    {
        _authenticationService = authenticationService;
    }
    //login
    //post :baseUrl/api/authentication/login
    [HttpPost("Login")]
    public async Task<ActionResult<UserDto>> Login(LoginDto loginDto)
    {
        var result = await _authenticationService.LoginAsync(loginDto);
        return HandleResult(result);
    }
    //register
    [HttpPost("Register")]
    public async Task<ActionResult<UserDto>> Register(RegisterDto registerDto)
    {
        var result = await _authenticationService.RegisterAsync(registerDto);
        return HandleResult(result);
    }
    //check email
    [HttpGet("CheckEmail")]
    public async Task<ActionResult<bool>> CheckEmail(string email)
    {
        var result = await _authenticationService.CheckEmailAsync(email);
        return Ok(result);
    }

    //get user by email
    [HttpGet("CurrentUser")]
    [Authorize]
    public async Task<ActionResult<UserDto>> GetCurrentUser()
    {
        var email =  User.FindFirstValue(ClaimTypes.Email);
        var result = await _authenticationService.GetUserByEmailAsync(email!);
        return HandleResult(result);
    }
}
