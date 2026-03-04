using System.ComponentModel.DataAnnotations;

namespace ECommerece.Shared.IdentityDTOS;

public record LoginDto([EmailAddress]string Email, string Password);

