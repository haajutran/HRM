using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace HRM.Models
{
    public class Salary
    {
        public int SalaryID { get; set; }
        [DataType(DataType.DateTime)]
        public string RecordDate { get; set; }
        [Display(Name = "Lương Hưởng")]
        public long Earned { get; set; }
        [Display(Name = "Lương giờ theo hợp đồng")]
        [Required]
        public long PayPerHour { get; set; }
        [Display(Name = "Ghi chú")]
        public string Comment { get; set; }

        public int? EmployeeID { get; set; }

        public Employee Employee { get; set; }
    }
}
