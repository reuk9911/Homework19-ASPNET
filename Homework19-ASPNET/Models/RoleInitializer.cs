using Microsoft.AspNetCore.Identity;

namespace Homework19_ASPNET.Models
{
    public class RoleInitializer
    {
        public static async Task InitializeAsync(UserManager<User> userManager, RoleManager<IdentityRole> roleManager)
        {
            string adminName = "firstAdmin";
            string password = "_Aa123456";
            if (await roleManager.FindByNameAsync("admin") == null)
            {
                await roleManager.CreateAsync(new IdentityRole("admin"));
            }
            if (await roleManager.FindByNameAsync("simpleUser") == null)
            {
                await roleManager.CreateAsync(new IdentityRole("simpleUser"));
            }
            if (await userManager.FindByNameAsync(adminName) == null)
            {
                User admin = new User { UserName = adminName };
                IdentityResult result = await userManager.CreateAsync(admin, password);
                if (result.Succeeded)
                {
                    await userManager.AddToRoleAsync(admin, "admin");
                }
            }
        }
    }
}
