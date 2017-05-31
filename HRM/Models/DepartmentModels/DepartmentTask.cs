using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace HRM.Models
{
    public class DepartmentTask
    {
        [Key]
        public int DepartmentTaskID { get; set; }
        [Required(ErrorMessage = "Không để trống.")]
        public string Title { get; set; }
        [Display(Name="Mô tả")]
        public string Description { get; set; }

        public int WorkHours { get; set; }

        public Employee Employee { get; set; }

        public Department Department { get; set; }
    }
}
