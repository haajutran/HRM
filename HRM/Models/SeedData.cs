using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using HRM.Data;
using System.Linq;
using System;

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
                new Department { DepartmentCode = 1, DepartmentName = "Bộ phận nhân sự",    Description = "Chịu trách nhiệm về mảng nhân sự"    },
                new Department { DepartmentCode = 2, DepartmentName = "Bộ phận tài chính",  Description = "Chịu trách nhiệm về mảng tài chính"  },
                new Department { DepartmentCode = 3, DepartmentName = "Bộ phận kỹ thuật",   Description = "Chịu trách nhiệm về mảng kỹ thuật"   }
                );

            if (context.DepartmentTitles.Any())
            {
                return;
            }

            context.DepartmentTitles.AddRange(
                new DepartmentTitle { Title = "Trưởng phòng"},
                new DepartmentTitle { Title = "Thư ký"},
                new DepartmentTitle { Title = "Phòng viên"},
                new DepartmentTitle { Title = "Thử việc" }
                );

            context.SaveChanges();
        }
    }
}

