using System.ComponentModel.DataAnnotations;
using System;
using System.Collections.Generic;
using System.Linq;

namespace HRM.Models
{
    public class Contract
    {
        public int ContractID { get; set; }

        public string Title { get; set; }

        public long PayPerHour { get; set; }

        public ICollection<Employee> Employees { get; set; }
    }
}