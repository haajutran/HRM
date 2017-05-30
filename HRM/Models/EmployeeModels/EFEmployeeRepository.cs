﻿using Microsoft.EntityFrameworkCore;
using HRM.Data;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System;

namespace HRM.Models
{
    public class EFEmployeeRepository : IEmployeeRepository
    {
        private ApplicationDbContext context;

        public EFEmployeeRepository(ApplicationDbContext ctx)
        {
            context = ctx;
        }

        public async Task<IEnumerable<Employee>> EmployeesAsync()
        {
            IQueryable<Employee> employees = context.Employees
                .Include(d => d.Departments)
                    .ThenInclude(d => d.DepartmentTasks)
                .Include(d => d.Departments)
                    .ThenInclude(d => d.DepartmentTitles)
                .Include(f => f.FamilyRelations);

            return await employees.ToListAsync();
        }

        public async Task<IEnumerable<Employee>> EmployeesAsync(int departmentCode)
        {
            IQueryable<Employee> employees = context.Employees.Where(e => e.DepartmentCode == departmentCode);          
            return await employees.ToListAsync();
        }

        public async Task<Employee> SearchAsync(int employeeID)
        {
            var employee = context.Employees
                .Include(d => d.Departments)
                    .ThenInclude(d => d.DepartmentTasks)
                .Include(d => d.Departments)
                    .ThenInclude(d => d.DepartmentTitles)
                        .ThenInclude(d => d.Department)
                .Include(f => f.FamilyRelations)
                .SingleOrDefaultAsync(m => m.EmployeeID == employeeID);

            return await employee;
        }

        public async Task<Employee> SearchAsync(int? employeeID)
        {
            var employee = context.Employees
                .Include(d => d.Departments)
                    .ThenInclude(d => d.DepartmentTasks)
                .Include(d => d.Departments)
                    .ThenInclude(d => d.DepartmentTitles)
                        .ThenInclude(d => d.Department)
                .Include(f => f.FamilyRelations)
                .SingleOrDefaultAsync(m => m.EmployeeID == employeeID);

            return await employee;
        }
    }
}
