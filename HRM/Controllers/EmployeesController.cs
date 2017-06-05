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
    [Authorize(Roles = "Manager, Staff")]
    public class EmployeesController : Controller
    {

        private ApplicationDbContext _context;
        private IEmployeeRepository _employeeRepository;
        private IDepartmentRepository _departmentRepository;
        private IHostingEnvironment _environment;
        string url = "EditEmployee?employeeID=";
        public EmployeesController(IEmployeeRepository repo, ApplicationDbContext context, IHostingEnvironment environment, IDepartmentRepository departmentRepository)
        {
            _employeeRepository = repo;
            _context = context;
            _environment = environment;
            _departmentRepository = departmentRepository;
        }

        #region Employee Management

        public async Task<ViewResult> Index()
        {
            return View(await _employeeRepository.EmployeesAsync());
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
            DepartmentTitlesDropDownList();
            ContractsDropDownList();
            return View(new Employee());
        }



        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> AddEmployee([Bind("EmployeeID, FullName, Address, DateOfBirth, PhoneNumber, Email, HomeTown, City, CitizenID, PlaceOfProvide, TempAddress, DepartmentCode, DateOfJoining, DepartmentTitle, ContractID")] Employee employee, string gender)
        {

            if (ModelState.IsValid)
            {
                var department = await _departmentRepository.SearchAsync(employee.DepartmentCode);

                employee.EmployeeCode = CodeGenerator(employee, department);
                employee.Active = true;
                employee.Gender = gender;
                employee.Region = "Việt Nam";
                employee.Contract = await _context.Contracts.SingleOrDefaultAsync(c => c.ContractID == employee.ContractID);

                _context.Employees.Add(new Employee() {
                    FullName = employee.FullName,
                    Address = employee.Address,
                    DateOfBirth = employee.DateOfBirth,
                    PhoneNumber = employee.PhoneNumber,
                    Email = employee.Email,
                    HomeTown = employee.HomeTown,
                    City = employee.City,
                    CitizenID = employee.CitizenID,
                    TempAddress = employee.TempAddress,
                    DepartmentCode = employee.DepartmentCode,
                    DateOfJoining = employee.DateOfJoining,
                    DepartmentTitle = employee.DepartmentTitle,
                    ContractID = employee.ContractID,
                    EmployeeCode = employee.EmployeeCode,
                    Active = employee.Active,
                    Gender = employee.Gender,
                    Region = employee.Region,
                    Contract = employee.Contract
                });

                _context.SaveChanges();

                var theNewEmployee = await _employeeRepository.SearchCodeAsync(employee.EmployeeCode);

                DepartmentTitle departmentTitle = new DepartmentTitle()
                {
                    Employee = theNewEmployee,
                    Department = department,
                    Title = employee.DepartmentTitle
                };
                _context.Add(departmentTitle);

                await _context.SaveChangesAsync();
                return RedirectToAction("Index", "Employees");
            }
            DepartmentsDropDownList();
            DepartmentTitlesDropDownList();
            ContractsDropDownList();

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
            DepartmentTitlesDropDownList();
            ContractsDropDownList();
            var employee = await _employeeRepository.SearchAsync(employeeID);
            employee.ContractID = employee.Contract.ContractID;
            return View(employee);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> EditEmployee(int? employeeID, string gender)
        {

            if (employeeID == null) { return NotFound(); }

            var employeeToUpdate = await _context.Employees.Include(f => f.FamilyRelations).SingleOrDefaultAsync(e => e.EmployeeID == employeeID);

            if (await TryUpdateModelAsync<Employee>(employeeToUpdate, ""))
            {
                try
                {
                    employeeToUpdate.Gender = gender;
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateException /* ex */)
                {            //Log the error (uncomment ex variable name and write a log.)          
                    ModelState.AddModelError("", "Unable to save changes. " +
                        "Try again, and if the problem persists, " +
                        "see your system administrator.");
                }
                return Redirect("Index");
            }
            DepartmentsDropDownList();
            DepartmentTitlesDropDownList();
            ContractsDropDownList();
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

        [HttpPost, ActionName("DeleteConfirmed")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int employeeID)
        {
            var employee = await _employeeRepository.SearchAsync(employeeID);

            if (employee == null)
            {
                return Redirect("Index");
            }

            try
            {
                _context.Employees.Remove(employee);
                await _context.SaveChangesAsync();
                //TempData["message"] = $"{employee.Name} đã được xóa.";
                return Redirect("Index");
            }
            catch (DbUpdateException /* ex */)
            {
                //Log the error (uncomment ex variable name and write a log.)
                return RedirectToAction("DeleteEmployee", new { id = employeeID, saveChangesError = true });
            }
        }
        #endregion -------------------------------------- --------------------------------------

        #endregion --------------------------------------

        #region Employees' Family Management

        public async Task<IActionResult> FamilyDetails(int? id)
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

        public IActionResult AddFamily(int employeeID)
        {
            FamilyRelation familyRelation = new FamilyRelation()
            {
                EmployeeId = employeeID
            };
            return View(familyRelation);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> AddFamily([Bind("FamilyRelationId,EmployeeId,Name,DateOfBirth,Relation,Occupation,Address,WorkPlace,PhoneNumber,Description")] FamilyRelation familyRelation, int employeeID)
        {
            var employee = await _context.Employees.Include(f => f.FamilyRelations).FirstOrDefaultAsync(e => e.EmployeeID == employeeID);
            if (ModelState.IsValid)
            {
                familyRelation.Employee = employee;
                employee.FamilyRelations.Add(familyRelation);
                _context.Add(familyRelation);
                await _context.SaveChangesAsync();
                return Redirect(url + employeeID);
            }
            return View(familyRelation);
        }


        public async Task<IActionResult> EditFamily(int? id)
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


        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> EditFamily(int id, [Bind("FamilyRelationId,EmployeeId,Name,DateOfBirth,Relation,Occupation,Address,WorkPlace,PhoneNumber,Description")] FamilyRelation familyRelation)
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
                var employeeID = familyRelation.EmployeeId;
                return Redirect(url + employeeID);
            }
            return View(familyRelation);
        }

        public async Task<IActionResult> DeleteFamily(int? familyRelationID)
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

        [HttpPost, ActionName("DeleteFamilyConfirmed")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteFamilyConfirmed(int id)
        {
            var familyRelation = await _context.FamilyRelations
                .Include(e => e.Employee)
                .SingleOrDefaultAsync(m => m.FamilyRelationId == id);
            _context.FamilyRelations.Remove(familyRelation);
            var employeeID = familyRelation.EmployeeId;
            await _context.SaveChangesAsync();
            var returnUrl = "Employees/EditEmployee?employeeID=" + employeeID;
            return Redirect(url + employeeID);
        }

        private bool FamilyRelationExists(int id)
        {
            return _context.FamilyRelations.Any(e => e.FamilyRelationId == id);
        }
        #endregion

        #region Methods --------------------------------------

        #region Departments Drop Down List
        private void DepartmentsDropDownList()
        {
            var departmentsQuery = from d in _context.Departments
                                   orderby d.DepartmentName
                                   select d;

            ViewData["Departments"] = new SelectList(departmentsQuery.AsNoTracking(), "DepartmentCode", "DepartmentName");
        }
        #endregion

        #region Department Titles Drop Down List
        private void DepartmentTitlesDropDownList()
        {
            var titlesQuery = from d in _context.DepartmentTitles
                                            orderby d.Title
                                            select d.Title;

            ViewData["DepartmentTitles"] = new SelectList(titlesQuery.Distinct().AsNoTracking());

        }
        #endregion

        #region Contracts Drop Down List
        private void ContractsDropDownList()
        {
            var contractsQuery = from d in _context.Contracts
                              orderby d.Title
                              select d;

            ViewData["Contracts"] = new SelectList(contractsQuery.Distinct().AsNoTracking(), "ContractID", "Title");

        }
        #endregion

        private bool EmployeeExists(int employeeID)
        {
            return _context.Employees.Any(e => e.EmployeeID == employeeID);
        }

        private int CodeGenerator(Employee employee, Department department)
        {
            var left = (10 + department.DepartmentCode).ToString();
            var right = (10000 + department.DepartmentTitles.Count);
            var right2 = right.ToString().Substring(1, 4);
            var employeeCode = int.Parse(left + right2);
            return employeeCode;
        }

        #endregion
    }
}