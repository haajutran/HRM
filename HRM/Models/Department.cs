using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HRM.Models
{
    public class Department
    {
        public int DepartmentID { get; set; }
        public int DepartmentCode { get; set; }
        public string DepartmentName { get; set; }
        public string Description { get; set; }
        public ICollection<DepartmentTask> DepartmentTasks { get; set; }
        public ICollection<DepartmentTitle> DepartmentTitles { get; set; }
    }
}
