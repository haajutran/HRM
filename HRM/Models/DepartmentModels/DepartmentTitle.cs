using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace HRM.Models
{
    public class DepartmentTitle
    {   
        [Key]
        public int DepartmentTitleID { get; set; }
        [Required]
        [Display(Name="Chức danh")]
        public string Title { get; set; }
        [Display(Name = "Mô tả")]
        public string Description { get; set; }
        public Department Department { get; set; }
        public Employee Employee { get; set; }
    }
}
