using System.Collections.Generic;
using System.Threading.Tasks;

namespace HRM.Models
{
    public interface IEmployeeRepository
    {
        Task<IEnumerable<Employee>> EmployeesAsync();
        Task<Employee> SearchAsync(int employeeID);
    }
}
