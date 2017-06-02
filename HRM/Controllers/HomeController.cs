using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using HRM.Models.ViewModels;
using HRM.Models;
using HRM.Data;
using Microsoft.AspNetCore.Authorization;

namespace HRM.Controllers
{
    [Authorize(Roles = "Manager")]
    public class HomeController : Controller
    {
        private IEmployeeRepository _employeeRepository;
        private ApplicationDbContext _context;

        public HomeController(IEmployeeRepository repo, ApplicationDbContext context)
        {
            _employeeRepository = repo;
            _context = context;
        }
        public IActionResult Index()
        {
            return View();
        }

        public async Task<ViewResult> Employees()
        {
            return View(new EmployeesListViewModel
            {
                Employees = await _employeeRepository.EmployeesAsync()
            });
        }


        public IActionResult About()
        {
            ViewData["Message"] = "Your application description page.";

            return View();
        }

        public IActionResult Contact()
        {
            ViewData["Message"] = "Your contact page.";

            return View();
        }

        public IActionResult Error()
        {
            return View();
        }
    }
}
