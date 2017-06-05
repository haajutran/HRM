using System.Collections.Generic;
using System.Threading.Tasks;

namespace HRM.Models
{
    public interface IDepartmentRepository
    {
        Task<IEnumerable<Department>> DepartmentsAsync();
        Task<Department> SearchAsync(int departmentCode);
        Task<DepartmentTitle> SearchTitleAsync(int departmentTitleID);
        Task<DepartmentTask> SearchTaskAsync(int departmentTaskID);
        Task<Department> SearchByIDAsync(int departmentID);
        Task<Department> SearchByIDAsync(int? departmentID);
    }
}
