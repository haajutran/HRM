using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using HRM.Data;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace HRM.Models
{
    public static class RolesData
    {
        private static readonly string[] Roles = new string[] { "Master", "HRDepartment", "HRDepartmentManager", "ITDepartment", "ITDepartmentManager", "FinanceDepartment", "FinanceDepartmentManager", "ITDepartmentDeputy", "FinanceDepartmentDeputy", "HRDepartmentDeputy" };

        public static async Task SeedRoles(IServiceProvider serviceProvider)
        {
            using (var serviceScope = serviceProvider.GetRequiredService<IServiceScopeFactory>().CreateScope())
            {
                var dbContext = serviceScope.ServiceProvider.GetService<ApplicationDbContext>();

                if (!dbContext.UserRoles.Any())
                {
                    var roleManager = serviceProvider.GetRequiredService<RoleManager<IdentityRole>>();

                    foreach (var role in Roles)
                    {
                        if (!await roleManager.RoleExistsAsync(role))
                        {
                            await roleManager.CreateAsync(new IdentityRole(role));
                        }
                    }
                }

            }
        }
    }
}
