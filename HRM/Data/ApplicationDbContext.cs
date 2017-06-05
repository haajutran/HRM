using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using HRM.Models;

namespace HRM.Data
{
    public class ApplicationDbContext : IdentityDbContext<AppUser>
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);
            builder.Entity<Employee>().ToTable("Employee");
            builder.Entity<Department>().ToTable("Department");
            builder.Entity<FamilyRelation>().ToTable("FamilyRelation");
            builder.Entity<DepartmentTask>().ToTable("DepartmentTask");
            builder.Entity<DepartmentTitle>().ToTable("DepartmentTitle");
            builder.Entity<Salary>().ToTable("Salary");
            builder.Entity<Contract>().ToTable("Contract");
        }
        public DbSet<Employee> Employees { get; set; }
        public DbSet<Department> Departments { get; set; }
        public DbSet<FamilyRelation> FamilyRelations { get; set; }
        public DbSet<DepartmentTask> DepartmentTasks { get; set; }
        public DbSet<DepartmentTitle> DepartmentTitles { get; set; }
        public DbSet<Salary> SalaryRecords { get; set; }
        public DbSet<Contract> Contracts { get; set; }

    }
}
