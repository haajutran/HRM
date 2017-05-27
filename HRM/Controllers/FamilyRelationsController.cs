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
    public class FamilyRelationsController : Controller
    {
        private readonly ApplicationDbContext _context;

        public FamilyRelationsController(ApplicationDbContext context)
        {
            _context = context;    
        }

        // GET: FamilyRelations
        public async Task<IActionResult> Index()
        {
            var famRel = await _context.FamilyRelations
                .Include(e => e.Employee)
                .ToListAsync();
            return View(famRel);
        }

        // GET: FamilyRelations/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var familyRelation = await _context.FamilyRelations
                .Include(e => e.Employee)
                .SingleOrDefaultAsync(m => m.FamilyRelationId == id);
            if (familyRelation == null)
            {
                return NotFound();
            }

            return View(familyRelation);
        }

        // GET: FamilyRelations/Create
        public IActionResult Create(int employeeID)
        {
            FamilyRelation familyRelation = new FamilyRelation();
            familyRelation.EmployeeId = employeeID;

            return View(familyRelation);
        }

        // POST: FamilyRelations/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("FamilyRelationId,EmployeeId,Name,DateOfBirth,Relation,Occupation,Address,WorkPlace,PhoneNumber,Description")] FamilyRelation familyRelation)
        {
            if (ModelState.IsValid)
            {
                _context.Add(familyRelation);
                await _context.SaveChangesAsync();
                return RedirectToAction("Index");
            }
            return View(familyRelation);
        }

        // GET: FamilyRelations/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var familyRelation = await _context.FamilyRelations
                .Include(e => e.Employee)
                .SingleOrDefaultAsync(m => m.FamilyRelationId == id);
            if (familyRelation == null)
            {
                return NotFound();
            }
            return View(familyRelation);
        }

        // POST: FamilyRelations/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("FamilyRelationId,EmployeeId,Name,DateOfBirth,Relation,Occupation,Address,WorkPlace,PhoneNumber,Description")] FamilyRelation familyRelation)
        {
            if (id != familyRelation.FamilyRelationId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(familyRelation);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!FamilyRelationExists(familyRelation.FamilyRelationId))
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
            return View(familyRelation);
        }

        // GET: FamilyRelations/Delete/5
        public async Task<IActionResult> Delete(int? familyRelationID)
        {
            if (familyRelationID == null)
            {
                return NotFound();
            }

            var familyRelation = await _context.FamilyRelations
                .Include(e => e.Employee)
                .SingleOrDefaultAsync(m => m.FamilyRelationId == familyRelationID);
            if (familyRelation == null)
            {
                return NotFound();
            }

            return View(familyRelation);
        }

        // POST: FamilyRelations/Delete/5
        [HttpPost, ActionName("DeleteConfirmed")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var familyRelation = await _context.FamilyRelations
                .Include(e => e.Employee)
                .SingleOrDefaultAsync(m => m.FamilyRelationId == id);
            _context.FamilyRelations.Remove(familyRelation);
            await _context.SaveChangesAsync();
            return RedirectToAction("Index");
        }

        private bool FamilyRelationExists(int id)
        {
            return _context.FamilyRelations.Any(e => e.FamilyRelationId == id);
        }
    }
}
