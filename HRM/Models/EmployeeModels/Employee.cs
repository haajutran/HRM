using System.ComponentModel.DataAnnotations;
using System;
using System.Collections.Generic;
using System.Linq;

namespace HRM.Models
{
    public class Employee
    {
        [Key]
        public int EmployeeID { get; set; }
        public int EmployeeCode { get; set; }
        [Required(ErrorMessage = "Không để trống tên.")]
        public string FullName { get; set; }
        public string Gender { get; set; }
        public string PhoneNumber { get; set; }
        [Required(ErrorMessage = "Không để trống email.")]
        public string Email { get; set; }
        [Required(ErrorMessage = "Không để trống ngày sinh.")]
        public string DateOfBirth { get; set; }
        public string PlaceOfBirth { get; set; }
        public string HomeTown { get; set; }
        public string City { get; set; }
        public string Region { get; set; }
        [Required(ErrorMessage = "Không để trống CMND.")]
        [MinLength(9, ErrorMessage = "CMND phải có 9 số")]
        [MaxLength(9, ErrorMessage = "CMND phải có 9 số")]
        [RegularExpression("^[0-9]*$", ErrorMessage = "Chứng minh nhân dân phải là số!")]
        public string CitizenID { get; set; }
        public string PlaceOfProvide { get; set; }
        public string Address { get; set; }
        [Required(ErrorMessage = "Không để trống địa chỉ tạm trú.")]
        public string TempAddress { get; set; }
        public string Avatar { get; set; }
        public bool Active { get; set; }
        public int DepartmentCode { get; set; }
        public string DepartmentTitle { get; set; }
        [Required(ErrorMessage = "Không để trống ngày tham gia.")]
        public DateTime DateOfJoining { get; set; }
        public DateTime ExitDate { get; set; }
        public ICollection<Salary> SalaryRecords { get; set; }
        public ICollection<DepartmentTitle> DepartmentTitles { get; set; }
        public ICollection<DepartmentTask> DepartmentTasks { get; set; }
        public ICollection<FamilyRelation> FamilyRelations { get; set; }
        public ICollection<Department> Departments { get; set; }

        public static implicit operator Employee(int v)
        {
            throw new NotImplementedException();
        }
    }
}
