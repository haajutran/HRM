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
    public class DepartmentTitlesController : Controller
    {
        private readonly ApplicationDbContext _context;

        public DepartmentTitlesController(ApplicationDbContext context)
        {
            _context = context;    
        }

        // GET: DepartmentTitles
        public async Task<IActionResult> Index()
        {
            var applicationDbContext = _context.DepartmentTitles.Include(d => d.Department);
            return View(await applicationDbContext.ToListAsync());
        }

        // GET: DepartmentTitles/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var departmentTitle = await _context.DepartmentTitles
                .Include(d => d.Department)
                .SingleOrDefaultAsync(m => m.DepartmentTitleID == id);
            if (departmentTitle == null)
            {
                return NotFound();
            }

            return View(departmentTitle);
        }

        // GET: DepartmentTitles/Create
        public IActionResult Create()
        {
            ViewData["DepartmentID"] = new SelectList(_context.Departments, "DepartmentID", "DepartmentName");
            return View();
        }

        // POST: DepartmentTitles/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("DepartmentTitleID,Title,Description,DepartmentID,EmployeeCode")] DepartmentTitle departmentTitle)
        {
            if (ModelState.IsValid)
            {
                _context.Add(departmentTitle);
                await _context.SaveChangesAsync();
                return RedirectToAction("Index");
            }
            ViewData["DepartmentID"] = new SelectList(_context.Departments, "DepartmentID", "DepartmentName", departmentTitle.DepartmentID);
            return View(departmentTitle);
        }

        // GET: DepartmentTitles/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var departmentTitle = await _context.DepartmentTitles.SingleOrDefaultAsync(m => m.DepartmentTitleID == id);
            if (departmentTitle == null)
            {
                return NotFound();
            }
            ViewData["DepartmentID"] = new SelectList(_context.Departments, "DepartmentID", "DepartmentName", departmentTitle.DepartmentID);
            return View(departmentTitle);
        }

        // POST: DepartmentTitles/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("DepartmentTitleID,Title,Description,DepartmentID,EmployeeCode")] DepartmentTitle departmentTitle)
        {
            if (id != departmentTitle.DepartmentTitleID)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(departmentTitle);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!DepartmentTitleExists(departmentTitle.DepartmentTitleID))
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
            ViewData["DepartmentID"] = new SelectList(_context.Departments, "DepartmentID", "DepartmentName", departmentTitle.DepartmentID);
            return View(departmentTitle);
        }

        // GET: DepartmentTitles/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var departmentTitle = await _context.DepartmentTitles
                .Include(d => d.Department)
                .SingleOrDefaultAsync(m => m.DepartmentTitleID == id);
            if (departmentTitle == null)
            {
                return NotFound();
            }

            return View(departmentTitle);
        }

        // POST: DepartmentTitles/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var departmentTitle = await _context.DepartmentTitles.SingleOrDefaultAsync(m => m.DepartmentTitleID == id);
            _context.DepartmentTitles.Remove(departmentTitle);
            await _context.SaveChangesAsync();
            return RedirectToAction("Index");
        }

        private bool DepartmentTitleExists(int id)
        {
            return _context.DepartmentTitles.Any(e => e.DepartmentTitleID == id);
        }
    }
}
