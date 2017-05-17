using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using HRM.Data;
using HRM.Models;
using Microsoft.AspNetCore.Hosting;
using HRM.Models.ViewModels;
using Microsoft.EntityFrameworkCore;

// For more information on enabling MVC for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace HRM.Controllers
{
    public class DepartmentsController : Controller
    {

        private ApplicationDbContext _context;
        private IDepartmentRepository _departmentRepository;
        private IHostingEnvironment _environment;

        public DepartmentsController(IDepartmentRepository repo, ApplicationDbContext context, IHostingEnvironment environment)
        {
            _departmentRepository = repo;
            _context = context;
            _environment = environment;
        }

        // GET: /<controller>/
        public async Task<ViewResult> Index()
        {
            return View(new DepartmentsListViewModel
            {
                Departments = await _departmentRepository.DepartmentsAsync()
            });
        }

        #region Add Department

        //[Authorize(Roles = "Manager")]
        public IActionResult AddDepartment()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> AddDepartment([Bind("DepartmentCode, DepartmentName")] Department department)
        {

            if (ModelState.IsValid)
            {
                _context.Add(department);
                await _context.SaveChangesAsync();
                return RedirectToAction("Index", "Departments");
            }
            return View(department);
        }

        #endregion

        #region Edit Department
        //[Authorize(Roles = "Manager")]
        public async Task<IActionResult> EditDepartment(int? departmentCode)
        {
            if (departmentCode == null)
            {
                return NotFound();
            }

            var department = await _context.Departments.SingleOrDefaultAsync(m => m.DepartmentCode == departmentCode);

            return View(department);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> EditDepartment([Bind("DepartmentCode, DepartmentName")] Department department)
        {

            _context.Update(department);
            await _context.SaveChangesAsync();


            return View(department);
        }


        #endregion

        #region Department Detail

        public IActionResult DepartmentDetail()
        {
            return View();
        }

        #endregion

        #region Delete Employee
        //[Authorize(Roles = "Manager")]
        //public async Task<IActionResult> DeleteEmployee(int? employeeID, bool? saveChangesError = false)
        //{
        //    if (employeeID == null)
        //    {
        //        return NotFound();
        //    }
        //    var employee = await _context.Employees.SingleOrDefaultAsync(m => m.EmployeeID == employeeID);
        //    if (employee == null)
        //    {
        //        return NotFound();
        //    }

        //    if (saveChangesError.GetValueOrDefault())
        //    {
        //        ViewData["ErrorMessage"] =
        //            "Failed!";
        //    }

        //    return View(employee);
        //}
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int departmentCode)
        {
            var department = await _departmentRepository.SearchAsync(departmentCode);
            if (department == null)
            {
                return RedirectToAction("EmployeesManagement", "Main");
            }

            try
            {
                _context.Departments.Remove(department);
                await _context.SaveChangesAsync();
                //TempData["message"] = $"{employee.Name} đã được xóa.";
                return RedirectToAction("Index", "Departments");
            }
            catch (DbUpdateException /* ex */)
            {
                //Log the error (uncomment ex variable name and write a log.)
                return RedirectToAction("DeleteEmployee", new { id = departmentCode, saveChangesError = true });
            }
        }
        #endregion

    }
}
