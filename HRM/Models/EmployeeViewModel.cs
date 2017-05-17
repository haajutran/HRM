using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HRM.Models
{
    public class EmployeeViewModel
    {
        public Employee Employee { get; set; }

        public IQueryable<FamilyRelation> FamilyRelation { get; set; }
    }
}
