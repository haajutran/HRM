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
            IQueryable<Department> departments = context.Departments
                .Include(ta => ta.DepartmentTasks)
                    .ThenInclude(e => e.Employee)
                .Include(ti => ti.DepartmentTitles)
                .AsNoTracking();
            return await departments.ToListAsync();
        }

        public async Task<Department> SearchAsync(int departmentCode)
        {
            return await context.Departments.FirstOrDefaultAsync(p => p.DepartmentCode == departmentCode);
        }

        public async Task<DepartmentTitle> SearchTitleAsync(int departmentTitleID)
        {
            return await context.DepartmentTitles
                .SingleOrDefaultAsync(t => t.DepartmentTitleID == departmentTitleID);
        }
    }
}
