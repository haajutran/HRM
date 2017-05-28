using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HRM.Models
{
    public class DepartmentTitle
    {
        public int DepartmentTitleID { get; set; }

        public string Title { get; set; }

        public string Description { get; set; }

        Department Department { get; set; }

        Employee Employee { get; set; }
    }
}
