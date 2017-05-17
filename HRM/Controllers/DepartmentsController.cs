using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using HRM.Data;
using HRM.Models;
using Microsoft.AspNetCore.Hosting;
using HRM.Models.ViewModels;

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

    }
}
