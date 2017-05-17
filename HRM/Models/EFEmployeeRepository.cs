using Microsoft.EntityFrameworkCore;
using HRM.Data;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HRM.Models
{
    public class EFEmployeeRepository : IEmployeeRepository
    {
        private ApplicationDbContext context;

        public EFEmployeeRepository(ApplicationDbContext ctx)
        {
            context = ctx;
        }

        public async Task<IEnumerable<Employee>> EmployeesAsync()
        {
            IQueryable<Employee> employees = context.Employees;
            return await employees.ToListAsync();
        }

        public async Task<IEnumerable<Employee>> EmployeesAsync(int departmentCode)
        {
            IQueryable<Employee> employees = context.Employees.Where(e => e.DepartmentCode == departmentCode);          
            return await employees.ToListAsync();
        }

        public async Task<Employee> SearchAsync(int employeeID)
        {
            return await context.Employees.FirstOrDefaultAsync(p => p.EmployeeID == employeeID);
        }

    }
}
