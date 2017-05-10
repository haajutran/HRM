using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using HRM.Data;
using HRM.Models;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using System;
using Microsoft.AspNetCore.Authorization;

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

        /*--- Employee Profile ---*/
        public async Task<IActionResult> EmployeeProfile(int employeeID)
        {
            var employee = await _context.Employees.SingleOrDefaultAsync(m => m.EmployeeID == employeeID);
            return View(employee);
        }

        #region Add Employee

        //[Authorize(Roles = "Manager")]
        public IActionResult AddEmployee()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> AddEmployee([Bind("EmployeeCode, FullName, DateOfBirth,PlaceOfBirth, Family, Gender, PhoneNumber, Email, HomeTown, City, CitizenID, PlaceOfProvide, TempAddress, Avatar, DepartmentID, OutOfWork")] Employee employee, string gender)
        {
            ModifyEmployee(employee);

            if (ModelState.IsValid)
            {
                employee.Gender = gender;
                _context.Add(employee);
                await _context.SaveChangesAsync();
                return RedirectToAction("Employees", "Home");
            }
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

            var employee = await _context.Employees.SingleOrDefaultAsync(m => m.EmployeeID == employeeID);

            if (employeeID == null)
            {
                return NotFound();
            }

            return View(employee);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> EditEmployee([Bind("EmployeeCode, FullName, DateOfBirth,PlaceOfBirth, Family, Gender, PhoneNumber, Email, HomeTown, City, CitizenID, PlaceOfProvide, TempAddress, Avatar, DepartmentID, OutOfWork")] Employee employee, string gender)
        {

            ModifyEmployee(employee);


            employee.Gender = gender;
            _context.Update(employee);
            await _context.SaveChangesAsync();


            return View(employee);
        }


        #endregion

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
                return RedirectToAction("EmployeesManagement", "Main");
            }

            try
            {
                _context.Employees.Remove(employee);
                await _context.SaveChangesAsync();
                //TempData["message"] = $"{employee.Name} đã được xóa.";
                return RedirectToAction("EmployeesManagement", "Main");
            }
            catch (DbUpdateException /* ex */)
            {
                //Log the error (uncomment ex variable name and write a log.)
                return RedirectToAction("DeleteEmployee", new { id = employeeID, saveChangesError = true });
            }
        }
        #endregion

        #endregion

        private bool EmployeeExists(int employeeID)
        {
            return _context.Employees.Any(e => e.EmployeeID == employeeID);
        }

        private void ModifyEmployee(Employee employee)
        {

        }
    }
}
