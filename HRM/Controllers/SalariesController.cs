using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using HRM.Data;
using HRM.Models;

namespace HRM.Controllers
{
    public class SalariesController : Controller
    {
        private readonly ApplicationDbContext _context;
        private IEmployeeRepository _employeeRepository;
        private IDepartmentRepository _departmentRepository;
        public SalariesController(ApplicationDbContext context, IEmployeeRepository employeeRepository, IDepartmentRepository departmentRepository)
        {
            _context = context;
            _employeeRepository = employeeRepository;
            _departmentRepository = departmentRepository;
        }

        // GET: Salaries
        public async Task<IActionResult> Index()
        {
            return View(await _context.SalaryRecords.ToListAsync());
        }

        // GET: Salaries/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var salary = await _context.SalaryRecords
                .SingleOrDefaultAsync(m => m.SalaryID == id);
            if (salary == null)
            {
                return NotFound();
            }

            return View(salary);
        }

        // GET: Salaries/Create
        public async Task<IActionResult> AddSalary(int employeeID)
        {
            Salary salary = new Salary();
            salary.EmployeeID = employeeID;
            var emp = await _employeeRepository.SearchAsync(employeeID);
            salary.PayPerHour = emp.Contract.PayPerHour;
            long workHoursSum = 0;
            var tasks = _context.DepartmentTasks
                .Include(d => d.Employee)
                .Select(e => e).Where(e => e.Employee.EmployeeID == employeeID);
            foreach (var t in tasks)
            {
                workHoursSum += t.WorkHours;
            }

            salary.Earned = workHoursSum*emp.Contract.PayPerHour;
            ViewData["Contract"] = emp.Contract.Title;

            return View(salary);
        }

        // POST: Salaries/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> AddSalary([Bind("SalaryID,RecordDate,Earned,PayPerHour")] Salary salary, int employeeID)
        {
            if (ModelState.IsValid)
            {
                salary.Employee = await _employeeRepository.SearchAsync(employeeID);
                salary.RecordDate = DateTime.Now.ToString();
                _context.Add(salary);

                await _context.SaveChangesAsync();
                return RedirectToAction("EditEmployee", "Employees", new { employeeID = employeeID });
            }
            return View(salary);
        }

        // GET: Salaries/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var salary = await _context.SalaryRecords.SingleOrDefaultAsync(m => m.SalaryID == id);
            if (salary == null)
            {
                return NotFound();
            }
            return View(salary);
        }

        // POST: Salaries/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("SalaryID,RecordDate,Earned")] Salary salary)
        {
            if (id != salary.SalaryID)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(salary);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!SalaryExists(salary.SalaryID))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction("Index");
            }
            return View(salary);
        }

        // GET: Salaries/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var salary = await _context.SalaryRecords
                .SingleOrDefaultAsync(m => m.SalaryID == id);
            if (salary == null)
            {
                return NotFound();
            }

            return View(salary);
        }

        // POST: Salaries/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var salary = await _context.SalaryRecords.SingleOrDefaultAsync(m => m.SalaryID == id);
            _context.SalaryRecords.Remove(salary);
            await _context.SaveChangesAsync();
            return RedirectToAction("Index");
        }

        private bool SalaryExists(int id)
        {
            return _context.SalaryRecords.Any(e => e.SalaryID == id);
        }
    }
}
