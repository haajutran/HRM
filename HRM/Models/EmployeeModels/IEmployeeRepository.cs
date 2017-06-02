using System.Collections.Generic;
using System.Threading.Tasks;

namespace HRM.Models
{
    public interface IEmployeeRepository
    {
        Task<IEnumerable<Employee>> EmployeesAsync();
        Task<IEnumerable<Employee>> EmployeesAsync(int departmentCode);
        Task<Employee> SearchAsync(int employeeID);
        Task<Employee> SearchAsync(int? employeeID);
        Task<Employee> SearchCodeAsync(int? employeeCode);
    }
}
