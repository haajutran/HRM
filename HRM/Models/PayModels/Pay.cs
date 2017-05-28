using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HRM.Models
{
    public class Pay
    {
        public int PayID { get; set; }

        public string RecordDate { get; set; }

        public int Checked { get; set; }

        DepartmentTask DepartmentTask { get; set; }
    }
}
