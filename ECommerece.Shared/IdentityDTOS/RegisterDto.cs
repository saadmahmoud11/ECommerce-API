using System.ComponentModel.DataAnnotations;

namespace ECommerece.Shared.IdentityDTOS;

public record RegisterDto([EmailAddress]string Email, string DisplayName,string UserName,
    string Password, [Phone]string PhoneNumber);

