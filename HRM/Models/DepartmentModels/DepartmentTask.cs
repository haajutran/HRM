using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HRM.Models
{
    public class DepartmentTask
    {
        public int DepartmentTaskID { get; set; }

        public string Title { get; set; }

        public string Description { get; set; }

        public int WorkHours { get; set; }

        public Employee Employee { get; set; }

        public Department Department { get; set; }

        public Pay Pay { get; set; }
    }
}
