using System.ComponentModel.DataAnnotations;
using System;
using System.Collections.Generic;
using System.Linq;

namespace HRM.Models
{
    public class Employee
    {
        public int EmployeeID { get; set; }
        public int EmployeeCode { get; set; }
        public string FullName { get; set; }
        public string Gender { get; set; }
        public string PhoneNumber { get; set; }
        public string Email { get; set; }
        public string DateOfBirth { get; set; }
        public string PlaceOfBirth { get; set; }
        public string HomeTown { get; set; }
        public string City { get; set; }
        public string Region { get; set; }
        public string CitizenID { get; set; }
        public string PlaceOfProvide { get; set; }
        public string Address { get; set; }
        public string TempAddress { get; set; }
        public string Avatar { get; set; }
        public int OutOfWork { get; set; }
        public int DepartmentCode { get; set; }
        public ICollection<Pay> Pays { get; set; }
        public ICollection<Salary> SalaryRecords { get; set; }
        public ICollection<DepartmentTitle> DepartmentTitles { get; set; }
        public ICollection<DepartmentTask> DepartmentTasks { get; set; }
        public ICollection<FamilyRelation> FamilyRelations { get; set; }
        public ICollection<Department> Departments { get; set; }
    }
}
