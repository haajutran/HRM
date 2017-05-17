using HRM.Data;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HRM.Models
{
    public class EFDepartmentRepository : IDepartmentRepository
    {
        private ApplicationDbContext context;

        public EFDepartmentRepository(ApplicationDbContext ctx)
        {
            context = ctx;
        }

        public async Task<IEnumerable<Department>> DepartmentsAsync()
        {
            IQueryable<Department> departments = context.Departments;
            return await departments.ToListAsync();
        }

        public async Task<Department> SearchAsync(int departmentCode)
        {
            return await context.Departments.FirstOrDefaultAsync(p => p.DepartmentCode == departmentCode);
        }

    }
}
