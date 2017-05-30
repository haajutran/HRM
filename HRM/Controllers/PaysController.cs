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
    public class PaysController : Controller
    {
        private readonly ApplicationDbContext _context;

        public PaysController(ApplicationDbContext context)
        {
            _context = context;    
        }

        // GET: Pays
        public async Task<IActionResult> Index()
        {
            return View(await _context.Pays.ToListAsync());
        }

        // GET: Pays/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var pay = await _context.Pays
                .SingleOrDefaultAsync(m => m.PayID == id);
            if (pay == null)
            {
                return NotFound();
            }

            return View(pay);
        }

        // GET: Pays/Create
        public IActionResult Create(int departmentTaskID)
        {
            var pay = new Pay();
            pay.DepartmentTask = new DepartmentTask();
            pay.DepartmentTask.Description = departmentTaskID.ToString();
            return View(pay);
        }

        // POST: Pays/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("PayID,RecordDate,Checked,DepartmentTask")] Pay pay)
        {
            if (ModelState.IsValid)
            {
                var p = await _context.DepartmentTasks.SingleOrDefaultAsync(x => x.DepartmentTaskID == int.Parse(pay.DepartmentTask.Description));
                pay.DepartmentTask = p;
                pay.RecordDate = DateTime.Now.ToString();
                _context.Add(pay);
                await _context.SaveChangesAsync();
                return RedirectToAction("Index");
            }
            return View(pay);
        }

        // GET: Pays/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var pay = await _context.Pays.SingleOrDefaultAsync(m => m.PayID == id);
            if (pay == null)
            {
                return NotFound();
            }
            return View(pay);
        }

        // POST: Pays/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("PayID,RecordDate,Checked")] Pay pay)
        {
            if (id != pay.PayID)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(pay);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!PayExists(pay.PayID))
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
            return View(pay);
        }

        // GET: Pays/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var pay = await _context.Pays
                .SingleOrDefaultAsync(m => m.PayID == id);
            if (pay == null)
            {
                return NotFound();
            }

            return View(pay);
        }

        // POST: Pays/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var pay = await _context.Pays.SingleOrDefaultAsync(m => m.PayID == id);
            _context.Pays.Remove(pay);
            await _context.SaveChangesAsync();
            return RedirectToAction("Index");
        }

        private bool PayExists(int id)
        {
            return _context.Pays.Any(e => e.PayID == id);
        }
    }
}
