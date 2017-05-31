using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace HRM.Models
{
    public class FamilyRelation
    {
        [Key]
        public int FamilyRelationId { get; set; }

        [Required]
        [Display(Name = "Mã Nhân viên")]
        public int EmployeeId { get; set; }

        [Required]
        [Display(Name = "Tên")]
        public string Name { get; set; }

        [Display(Name = "Ngày sinh")]
        public string DateOfBirth { get; set; }

        [Display(Name = "Quan hệ")]
        public string Relation { get; set; }

        [Display(Name = "Nghề nghiệp")]
        public string Occupation { get; set; }

        [Display(Name = "Địa chỉ")]
        public string Address { get; set; }

        [Display(Name = "Nơi làm")]
        public string WorkPlace { get; set; }

        [Display(Name = "ĐT")]
        public string PhoneNumber { get; set; }

        [Display(Name = "Ghi chú")]
        public string Description { get; set; }

        public Employee Employee { get; set; }
    }
}
