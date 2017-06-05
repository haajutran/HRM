using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace HRM.Models
{
    public class Department
    {
        [Key]
        public int DepartmentID { get; set; }
        [Required(ErrorMessage = "Không để trống mã phòng ban.")]
        public int DepartmentCode { get; set; }
        [Required(ErrorMessage = "Không để trống tên phòng ban.")]
        public string DepartmentName { get; set; }
        public string Description { get; set; }
        public string Role { get; set; }
        public ICollection<DepartmentTask> DepartmentTasks { get; set; }
        public ICollection<DepartmentTitle> DepartmentTitles { get; set; }
        public ICollection<DepartmentAssignment> DepartmentAssignments { get; set; }
    }
}
