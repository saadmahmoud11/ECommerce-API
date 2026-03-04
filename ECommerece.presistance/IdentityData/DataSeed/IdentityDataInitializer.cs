using ECommerece.Domain.Contracts;
using ECommerece.Domain.IdentityModule;
using Microsoft.AspNetCore.Identity;

namespace ECommerece.presistance.IdentityData.DataSeed;

public class IdentityDataInitializer : IDataInitializer
{
    private readonly UserManager<ApplicationUser> _userManager;
    private readonly RoleManager<IdentityRole> _roleManager;

    public IdentityDataInitializer(UserManager<ApplicationUser> userManager, RoleManager<IdentityRole> roleManager)
    {
        _userManager = userManager;
        _roleManager = roleManager;
    }


    public async Task InitializeAsync()
    {
        try
        {
            if (!_roleManager.Roles.Any())
            {
                await _roleManager.CreateAsync(new IdentityRole("Admin"));
                await _roleManager.CreateAsync(new IdentityRole("SuperAdmin"));
            }
            if (!_userManager.Users.Any())
            {
                var user01 = new ApplicationUser()
                {
                    DisplayName = "Saad Mahmoud",
                    UserName = "SaadMahmoud",
                    Email = "saad@gmail.com",
                    PhoneNumber = "1234567890",
                };
                var user02 = new ApplicationUser()
                {
                    DisplayName = "Ali Mahmoud",
                    UserName = "AliMahmoud",
                    Email = "Ali@gmail.com",
                    PhoneNumber = "1234567666",
                };
                await _userManager.CreateAsync(user01,"P@ssw0rd");
                await _userManager.CreateAsync(user02, "P@ssw0rd");

                await _userManager.AddToRoleAsync(user01, "SuperAdmin");
                await _userManager.AddToRoleAsync(user02, "Admin");
            }
        }
        catch(Exception ex)
        {
            Console.WriteLine($"Error While seeding{ex} ");
        }
    }
}
