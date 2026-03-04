using ECommerece.Domain.IdentityModule;
using ECommerece.ServiceAbstraction;
using ECommerece.Shared.CommonResult;
using ECommerece.Shared.IdentityDTOS;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace ECommerece.Service;

public class AuthenticationService : IAuthenticationService
{
    private readonly UserManager<ApplicationUser> _userManager;
    private readonly IConfiguration _configuration;

    public AuthenticationService(UserManager<ApplicationUser> userManager, IConfiguration configuration)
    {
        _userManager = userManager;
        _configuration = configuration;
    }

    public async Task<bool> CheckEmailAsync(string email)
    {
        var user = await _userManager.FindByEmailAsync(email);
        return user != null;
    }

    public async Task<Result<UserDto>> GetUserByEmailAsync(string email)
    {
        var user = await _userManager.FindByEmailAsync(email);
        if (user is null)
        {
            return Error.NotFound("User.NotFound");
        }
        return new UserDto(user.Email!, user.DisplayName, await CreateTokenAsync(user));
    }

    public async Task<Result<UserDto>> LoginAsync(LoginDto loginDto)
    {
        var user = await _userManager.FindByEmailAsync(loginDto.Email);
        if (user is null)
        {
            return Error.InvalidCredentials("User.InvalidCredentials");
        }
        var isPasswordValid = await _userManager.CheckPasswordAsync(user, loginDto.Password);
        if (!isPasswordValid)
        {
            return Error.InvalidCredentials("User.InvalidCredentials");

        }
        var token = await CreateTokenAsync(user);
        return new UserDto(user.Email!, user.DisplayName, token);

    }

    public async Task<Result<UserDto>> RegisterAsync(RegisterDto registerDto)
    {
        var user = new ApplicationUser()
        {
            Email = registerDto.Email,
            DisplayName = registerDto.DisplayName,
            PhoneNumber = registerDto.PhoneNumber,
            UserName = registerDto.UserName,
        };
        var identityResult = await _userManager.CreateAsync(user, registerDto.Password);
        if (identityResult.Succeeded)
        {
            var token = await CreateTokenAsync(user);
            return new UserDto(user.Email!, user.DisplayName, token);
        }
        return identityResult.Errors.Select(e => Error.Validation(e.Code, e.Description)).ToList();
    }

    private async Task<string> CreateTokenAsync(ApplicationUser user)
    {
        //token [issuer, audience, claims, expires, signingCredentials]
        var claims = new List<Claim>()
        {
            new Claim(JwtRegisteredClaimNames.Email, user.Email!),
            new Claim(JwtRegisteredClaimNames.Name, user.UserName!),
        };
        var roles = await _userManager.GetRolesAsync(user);
        foreach (var role in roles)
        {
            claims.Add(new Claim(ClaimTypes.Role, role));
        }
        var secretKey = _configuration["JwtOptions:SecretKey"];
        var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(secretKey));
        var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);
        var token = new JwtSecurityToken(
            issuer: _configuration["JwtOptions:Issuer"],
            audience: _configuration["JwtOptions:Audience"],
            expires: DateTime.UtcNow.AddHours(1),
            claims: claims,
            signingCredentials: creds
            );
        return new JwtSecurityTokenHandler().WriteToken(token);
    }
}