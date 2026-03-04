using ECommerece.Shared.CommonResult;
using ECommerece.Shared.IdentityDTOS;

namespace ECommerece.ServiceAbstraction;

public interface IAuthenticationService
{
    //login => token, displayName, email
    Task<Result<UserDto>> LoginAsync(LoginDto loginDto);
    //register
    Task<Result<UserDto>> RegisterAsync(RegisterDto registerDto);
    Task<bool> CheckEmailAsync(string email);
    Task<Result<UserDto>> GetUserByEmailAsync(string email);
}
