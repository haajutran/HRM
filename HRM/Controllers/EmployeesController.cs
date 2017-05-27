using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using HRM.Data;
using HRM.Models;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Authorization;
using System.Collections.Generic;
using HRM.Models.ViewModels;
using Microsoft.AspNetCore.Mvc.Rendering;
using System;

// For more information on enabling MVC for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace HRM.Controllers
{
    public class EmployeesController : Controller
    {

        private ApplicationDbContext _context;
        private IEmployeeRepository _employeeRepository;
        private IHostingEnvironment _environment;

        public EmployeesController(IEmployeeRepository repo, ApplicationDbContext context, IHostingEnvironment environment)
        {
            _employeeRepository = repo;
            _context = context;
            _environment = environment;
        }

        #region Employee Management

        public async Task<ViewResult> Index()
        {
            return View(new EmployeesListViewModel
            {
                Employees = await _employeeRepository.EmployeesAsync()

            });
        }

        #region Employee Profile

        /*--- Employee Profile ---*/
        public async Task<IActionResult> EmployeeProfile(int employeeID)
        {
            Employee employee = new Employee();

            employee = await _context.Employees
                .Include(e => e.Departments)
                    .ThenInclude(i => i.DepartmentTitles)
                .Include(e => e.Departments)
                    .ThenInclude(a => a.DepartmentTasks)
                        .ThenInclude(p => p.Pay)
                .Include(f => f.FamilyRelations)
                .Include(s => s.SalaryRecords)
                .SingleOrDefaultAsync(m => m.EmployeeID == employeeID);

            return View(employee);
        }

        #endregion

        #region Add Employee

        //[Authorize(Roles = "Manager")]
        public IActionResult AddEmployee()
        {
            DepartmentsDropDownList();

            return View();
        }



        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> AddEmployee([Bind("EmployeeCode, FullName, Address, DateOfBirth,PlaceOfBirth, Gender, PhoneNumber, Email, HomeTown, City, CitizenID, PlaceOfProvide, TempAddress, Avatar, DepartmentCode, OutOfWork")] Employee employee, string gender)
        {

            if (ModelState.IsValid)
            {
                employee.Gender = gender;
                //employee.FamilyRelations = _context.FamilyRelations.Select(f => f).Where(x => x.EmployeeId == employee.EmployeeID);
                //Department department = await _context.Departments
                //    .Include(d => d.Employees)
                //    .SingleOrDefaultAsync(m => m.DepartmentCode == employee.DepartmentCode);
                //employee.Departments = department;
                //department.Employees.Add(employee);
                _context.Add(employee);
                await _context.SaveChangesAsync();
                return RedirectToAction("Index", "Employees");
            }
            DepartmentsDropDownList();

            return View(employee);
        }

        #endregion

        #region Edit Employee
        //[Authorize(Roles = "Manager")]
        public async Task<IActionResult> EditEmployee(int? employeeID)
        {
            if (employeeID == null)
            {
                return NotFound();
            }
            DepartmentsDropDownList();
            var employee = await _context.Employees
                .SingleOrDefaultAsync(m => m.EmployeeID == employeeID);

           


            return View(employee);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> EditEmployee(int? employeeID, string gender)
        {

            if (employeeID == null) { return NotFound(); }

            var employeeToUpdate = await _context.Employees.SingleOrDefaultAsync(e => e.EmployeeID == employeeID);

            if (await TryUpdateModelAsync<Employee>(employeeToUpdate, ""))
            {
                try {
                    employeeToUpdate.Gender = gender;
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateException /* ex */)
                {            //Log the error (uncomment ex variable name and write a log.)          
                    ModelState.AddModelError("", "Unable to save changes. " +  
                        "Try again, and if the problem persists, " +        
                        "see your system administrator.");
                }        return RedirectToAction("Index");
            }
            DepartmentsDropDownList();
            return View(employeeToUpdate); 

            //        if (ModelState.IsValid)
            //{
            //    Department department = await _context.Departments
            //     .Include(d => d.Employees)
            //     .FirstOrDefaultAsync(m => m.DepartmentCode == employee.DepartmentCode);
            //    employee.Department = department;
            //    employee.Gender = gender;
            //    _context.Update(employee);
            //    await _context.SaveChangesAsync();
            //    return RedirectToAction("Index", "Employees");
            //}
            //DepartmentsDropDownList();

            //return View(employee);
        }


        #endregion --------------------------------------

        #region Delete Employee
        [Authorize(Roles = "Manager")]
        public async Task<IActionResult> DeleteEmployee(int? employeeID, bool? saveChangesError = false)
        {
            if (employeeID == null)
            {
                return NotFound();
            }
            var employee = await _context.Employees.SingleOrDefaultAsync(m => m.EmployeeID == employeeID);
            if (employee == null)
            {
                return NotFound();
            }

            if (saveChangesError.GetValueOrDefault())
            {
                ViewData["ErrorMessage"] =
                    "Failed!";
            }

            return View(employee);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int employeeID)
        {
            var employee = await _employeeRepository.SearchAsync(employeeID);
            if (employee == null)
            {
                return RedirectToAction("Index", "Employees");
            }

            try
            {
                _context.Employees.Remove(employee);
                await _context.SaveChangesAsync();
                //TempData["message"] = $"{employee.Name} đã được xóa.";
                return RedirectToAction("Index", "Employees");
            }
            catch (DbUpdateException /* ex */)
            {
                //Log the error (uncomment ex variable name and write a log.)
                return RedirectToAction("DeleteEmployee", new { id = employeeID, saveChangesError = true });
            }
        }
        #endregion -------------------------------------- --------------------------------------

        #endregion --------------------------------------

        #region Methods --------------------------------------

        private void DepartmentsDropDownList()
        {
            var departmentsQuery = from d in _context.Departments
                                   orderby d.DepartmentName
                                   select d;

            ViewData["Departments"] = new SelectList(departmentsQuery.AsNoTracking(), "DepartmentCode", "DepartmentName");
        }

        private bool EmployeeExists(int employeeID)
        {
            return _context.Employees.Any(e => e.EmployeeID == employeeID);
        }

        #endregion
    }
}
