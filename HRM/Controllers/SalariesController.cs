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
        public SalariesController(ApplicationDbContext context, IEmployeeRepository employeeRepository)
        {
            _context = context;
            _employeeRepository = employeeRepository;
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
            var emp = await _employeeRepository.SearchAsync(employeeID);
            Salary salary = new Salary();
            salary.Employee = emp;

            foreach (var task in emp.DepartmentTasks)
            {
                salary.Earned += task.WorkHours;
            }

            return View(salary);
        }

        // POST: Salaries/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> AddSalary([Bind("SalaryID,RecordDate,Earned")] Salary salary, int employeeID)
        {
            if (ModelState.IsValid)
            {
                salary.Employee = await _employeeRepository.SearchAsync(employeeID);
                _context.Add(salary);


                await _context.SaveChangesAsync();
                return Redirect("Employees/EditEmployee?employeeID=" + employeeID);
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
