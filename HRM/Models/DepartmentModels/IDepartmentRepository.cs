using System.Collections.Generic;
using System.Threading.Tasks;

namespace HRM.Models
{
    public interface IDepartmentRepository
    {
        Task<IEnumerable<Department>> DepartmentsAsync();
        Task<Department> SearchAsync(int departmentID);
        Task<DepartmentTitle> SearchTitleAsync(int departmentTitleID);
        Task<DepartmentTask> SearchTaskAsync(int departmentTaskID);
    }
}
