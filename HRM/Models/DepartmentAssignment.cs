using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HRM.Models
{
    public class DepartmentAssignment
    {
        public int DepartmentAssignmentID { get; set; }
        public int DepartmentID { get; set; }
        public int EmployeeID { get; set; }
        public Department Department { get; set; }
        public Employee Employee { get; set; }
    }
}
