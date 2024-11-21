using Microsoft.AspNetCore.Identity;
using Homework19_ASPNET.Auth;

namespace Homework19_ASPNET.Models
{
    public class RoleInitializer
    {
        public static async Task InitializeAsync(UserManager<User> userManager, RoleManager<IdentityRole> roleManager)
        {
            //string adminName = "firstAdmin";
            //string password = "_Aa123456";
            if (await roleManager.FindByNameAsync("admin") == null)
            {
                await roleManager.CreateAsync(new IdentityRole("admin"));
            }
            if (await roleManager.FindByNameAsync("simpleUser") == null)
            {
                await roleManager.CreateAsync(new IdentityRole("simpleUser"));
            }
            
            if (await userManager.FindByNameAsync("firstAdmin") == null)
            {
                User admin = new User { UserName = "firstAdmin" };
                IdentityResult result = await userManager.CreateAsync(admin, "_Aa123456");
                if (result.Succeeded)
                {
                    await userManager.AddToRoleAsync(admin, "admin");
                }
            }

            if (await userManager.FindByNameAsync("firstUser") == null)
            {
                User user = new User { UserName = "firstUser" };
                IdentityResult result = await userManager.CreateAsync(user, "Qwerty1!");
                if (result.Succeeded)
                {
                    await userManager.AddToRoleAsync(user, "simpleUser");
                }
            }
        }
    }
}
