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

        public string RecordDate { get; set; }

        public long Earned { get; set; }

        public Employee Employee { get; set; }
    }
}
