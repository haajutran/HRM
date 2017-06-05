using HRM.Data;
using HRM.Models;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.DependencyInjection;

namespace HRM.Model
{
    public static class IdentitySeedData
    {   
        private const string adminUser = "master";
        private const string adminEmail = "master@gmail.com";
        private const string adminPassword = "Master@123";
        public static async void EnsurePopulated(IApplicationBuilder app)
        {
            UserManager<AppUser> userManager = app.ApplicationServices.GetRequiredService<UserManager<AppUser>>();
            AppUser user = await userManager.FindByIdAsync(adminUser);
            if (user == null)
            {
                user = new AppUser { UserName = adminUser, Email = adminEmail };
                await userManager.CreateAsync(user, adminPassword);
                await userManager.AddToRoleAsync(user, "Master");
            }
        }
    }
}