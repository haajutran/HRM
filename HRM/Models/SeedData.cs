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

            if (context.Departments.Any())
            {
                return;
            }
                    
            context.Departments.AddRange(
                new Department { DepartmentCode = 1, DepartmentName = "Phòng nhân sự" },
                new Department { DepartmentCode = 2, DepartmentName = "Phòng tài chính" },
                new Department { DepartmentCode = 3, DepartmentName = "Phòng kỹ thuật" }
                );

            context.SaveChanges();
        }
    }
}

