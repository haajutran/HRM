using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using HRM.Data;
using System.Linq;

namespace HRM.Models
{
    public static class SeedData
    {

        public static void EnsurePopulated(IApplicationBuilder app)
        {
            ApplicationDbContext context = app.ApplicationServices.GetRequiredService<ApplicationDbContext>();

            context.Database.EnsureCreated();

            if (context.Employees.Any())
            {
                return;
            }

            context.SaveChanges();
        }
    }
}

