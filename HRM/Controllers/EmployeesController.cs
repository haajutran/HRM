using System.ComponentModel.DataAnnotations;
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
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.DependencyInjection;
using HRM.Models.AccountViewModels;
using System;

// For more information on enabling MVC for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace HRM.Controllers
{
    [Authorize]
    public class EmployeesController : Controller
    {
        private readonly UserManager<AppUser> _userManager;
        private ApplicationDbContext _context;
        private IEmployeeRepository _employeeRepository;
        private IHostingEnvironment _environment;
        string url = "EditEmployee?employeeID=";
        public EmployeesController(UserManager<AppUser> userManager, IEmployeeRepository repo, ApplicationDbContext context, IHostingEnvironment environment)
        {
            _userManager = userManager;
            _employeeRepository = repo;
            _context = context;
            _environment = environment;
        }

        #region Employee Management

        #region Index
        public async Task<ViewResult> Index()
        {
            return View(await _employeeRepository.EmployeesAsync());
        }
        #endregion

        #region Employee Profile
        [Authorize(Roles = "Master, HRDepartment, ITDepartment, FinanceDepartment")]
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


        [Authorize(Roles = "Master, HRDepartmentManager, ITDepartmentManager, FinanceDepartmentManager, HRDepartment, HRDepartmentDeputy")]
        //[Authorize(Roles = "Manager")]
        public async Task<IActionResult> AddEmployee()
        {
            var currentUser = await GetCurrentUserAsync();
            var currentUserRoles = await _userManager.GetRolesAsync(currentUser);

            DepartmentsDropDownList();
            DepartmentTitlesDropDownList();
            ContractsDropDownList();
            return View();
        }

        [Authorize(Roles = "Master, HRDepartmentManager, ITDepartmentManager, FinanceDepartmentManager, HRDepartment, HRDepartmentDeputy")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> AddEmployee([Bind("FullName, ContractID, Address, DateOfBirth, PhoneNumber, Email, HomeTown, City, CitizenID, PlaceOfProvide, TempAddress, DepartmentCode, DateOfJoining, DepartmentTitle")] Employee employee, string gender)
        {

            if (ModelState.IsValid)
            {
                if (EmailExist(employee))
                {
                    ViewData["ErrorMessage"] = "Email đã bị trùng.";
                    ContractsDropDownList();
                    DepartmentsDropDownList();
                    DepartmentTitlesDropDownList();
                    return View(employee);
                }
                var department = _context.Departments.Include(d => d.DepartmentTitles).SingleOrDefault(d => d.DepartmentCode == employee.DepartmentCode);

                var currentUser = await GetCurrentUserAsync();
                var currentUserRoles = await _userManager.GetRolesAsync(currentUser);

                if (employee.DepartmentTitle == "Trưởng phòng")
                {

                    if (!currentUserRoles.Contains("HRDepartmentManager")
                        && !currentUserRoles.Contains("Master"))
                    {
                        TempData["ErrorMessage"] = "Bạn không có quyền thêm chức vụ Trưởng Phòng.";
                        return Redirect("Error");
                    }
                    var departmentManagerCount = 0;
                    foreach (var dpm in department.DepartmentTitles)
                    {
                        if (dpm.Title == "Trưởng phòng")
                        {
                            departmentManagerCount++;
                        }
                    }
                    if (departmentManagerCount >= 1)
                    {
                        ViewData["ErrorMessage"] = "Số lượng chức vụ trưởng phòng trong một phòng ban không thể lớn hơn 2.";
                        ContractsDropDownList();
                        DepartmentsDropDownList();
                        DepartmentTitlesDropDownList();
                        return View(employee);
                    }
                }

                else if (employee.DepartmentTitle.Equals("Phó phòng"))
                {

                    if (!currentUserRoles.Contains("HRDepartmentManager")
                      && !currentUserRoles.Contains("Master"))
                    {
                        var employeeUser = _context.Employees
                          .Include(d => d.DepartmentAssignments)
                          .SingleOrDefault(u => u.UserId == currentUser.Id);
                        var hasDepartment = false;
                        foreach (var dad in employeeUser.DepartmentAssignments)
                        {
                            if (dad.Department == department)
                            {
                                hasDepartment = true;
                                break;
                            }
                        }
                        if (hasDepartment == false)
                        {
                            TempData["ErrorMessage"] = "Bạn không có quyền thêm chức vụ Phó Phòng.";
                            return Redirect("Error");
                        }
                    }
                    var departmentManagerCount = 0;
                    foreach (var dpm in department.DepartmentTitles)
                    {
                        if (dpm.Title == "Phó phòng")
                        {
                            departmentManagerCount++;
                        }
                    }
                    if (departmentManagerCount >= 2)
                    {
                        TempData["ErrorMessage"] = "Số lượng chức vụ phó phòng trong một phòng ban không thể lớn hơn 3.";
                        return Redirect("Error");
                    }
                }

                DepartmentTitle departmentTitle = new DepartmentTitle()
                {
                    Employee = employee,
                    Department = department,
                    Title = employee.DepartmentTitle
                };

                #region Add
                _context.DepartmentTitles.Add(departmentTitle);
                employee.EmployeeCode = CodeGenerator(employee, department);
                employee.Active = true;
                //employee.Departments =  departments;
                employee.Gender = gender;
                employee.Region = "Việt Nam";
                department.DepartmentTitles.Add(departmentTitle);
                _context.Add(employee);

                string username = employee.EmployeeCode + "";
                string email = employee.Email;
                string password = "User@123";

                var user = new AppUser { UserName = username, Email = email };
                await _userManager.CreateAsync(user, password);
                if (departmentTitle.Title.Equals("Trưởng phòng"))
                {
                    await _userManager.AddToRoleAsync(user, department.Role + "Manager");
                }
                else if (departmentTitle.Title.Equals("Phó phòng"))
                {
                    await _userManager.AddToRoleAsync(user, department.Role + "Deputy");
                }
                else
                {
                    await _userManager.AddToRoleAsync(user, department.Role);
                }

                #endregion

                employee.UserId = user.Id;
                await _context.SaveChangesAsync();
                DepartmentAssignment deA = new DepartmentAssignment { Department = department, Employee = _context.Employees.FirstOrDefault(e => e.EmployeeCode == employee.EmployeeCode) };
                _context.Add(deA);
                await _context.SaveChangesAsync();
                return RedirectToAction("Index", "Employees");
            }
            ContractsDropDownList();
            DepartmentsDropDownList();
            DepartmentTitlesDropDownList();

            return View(employee);
        }



        #endregion

        #region Edit Employee

        #region Edit Employee View
        [Authorize]
        public async Task<IActionResult> EditEmployee(int? employeeID)
        {
            if (employeeID == null)
            {
                return NotFound();
            }

            ContractsDropDownList();
            DepartmentsDropDownList();
            DepartmentTitlesDropDownList();

            var employee = await _employeeRepository.SearchAsync(employeeID);
            //var titles = employee.DepartmentTitles();
            var userId = employee.UserId;
            AppUser user = await _userManager.FindByIdAsync(userId);
            var userRoles = await _userManager.GetRolesAsync(user);

            var currentUser = await GetCurrentUserAsync();
            var currentUserRoles = await _userManager.GetRolesAsync(currentUser);

            if (userRoles.Contains("Master"))
            {
                if (!currentUserRoles.Contains("Master"))
                {
                    return RedirectToAction("AccessDenied", "Account");
                }

            }

            //else if()
            if (!currentUserRoles.Contains("Master") && !currentUserRoles.Contains("HRDepartmentManager"))
            {
                bool tf = false;
                foreach (var item in currentUserRoles)
                {
                    if (item.Contains("Manager"))
                    {
                        tf = true;
                        break;
                    }
                }
                if (tf == false)
                {
                    if (!currentUser.UserName.Equals(employee.EmployeeCode.ToString()))
                    {
                        return RedirectToAction("AccessDenied", "Account");
                    }
                    else
                    {
                        ViewData["Limited"] = "true";
                    }
                }
              
            }





            return View(employee);
        }
        #endregion

        [HttpPost]
        [Authorize]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> EditEmployee(int? employeeID, string gender, string email)
        {

            if (employeeID == null) { return NotFound(); }

            var employeeToUpdate = await _context.Employees
                .Include(f => f.FamilyRelations)
                .Include(d => d.Departments)
                .SingleOrDefaultAsync(e => e.EmployeeID == employeeID);


            if (await TryUpdateModelAsync<Employee>(employeeToUpdate, ""))
            {
                try
                {

                    var de = _context.Departments.Include(d => d.DepartmentTitles).SingleOrDefault(d => d.DepartmentCode == employeeToUpdate.DepartmentCode);

                    var userId = employeeToUpdate.UserId;
                    AppUser user = await _userManager.FindByIdAsync(userId);
                    foreach (var dep in employeeToUpdate.Departments)
                    {
                        await _userManager.RemoveFromRoleAsync(user, dep.Role);
                    }

                    if (EmailExistEdit(employeeToUpdate))
                    {
                        ViewData["ErrorMessage"] = "Email đã bị trùng.";
                        ContractsDropDownList();
                        DepartmentsDropDownList();
                        DepartmentTitlesDropDownList();
                        return View(employeeToUpdate);
                    }

                    var ed = employeeToUpdate.DepartmentTitle.Trim();
                    var department = _context.Departments.Include(d => d.DepartmentTitles).SingleOrDefault(d => d.DepartmentCode == employeeToUpdate.DepartmentCode);
                    var currentUser = await GetCurrentUserAsync();
                    var currentUserRoles = await _userManager.GetRolesAsync(currentUser);

                    if (ed.Equals("Trưởng phòng"))
                    {
                        var departmentManagerCount = 0;
                        foreach (var dpm in de.DepartmentTitles)
                        {
                            if (dpm.Title == "Trưởng phòng")
                            {
                                departmentManagerCount++;
                            }
                        }
                        if (departmentManagerCount >= 1)
                        {
                            TempData["ErrorMessage"] = "Số lượng chức vụ trưởng phòng trong một phòng ban không thể lớn hơn 2.";
                            return Redirect("Error");
                        }
                    }

                    else if (ed.Equals("Phó phòng"))
                    {
                        if (!currentUserRoles.Contains("HRDepartmentManager")
                            && !currentUserRoles.Contains("Master"))
                        {
                            var employeeUser = _context.Employees
                              .Include(d => d.DepartmentAssignments)
                              .SingleOrDefault(u => u.UserId == currentUser.Id);
                            var hasDepartment = false;
                            foreach (var dad in employeeUser.DepartmentAssignments)
                            {
                                if (dad.Department == department)
                                {
                                    hasDepartment = true;
                                    break;
                                }
                            }
                            if (hasDepartment == false)
                            {
                                TempData["ErrorMessage"] = "Bạn không có quyền thêm chức vụ Phó Phòng.";
                                return Redirect("Error");
                            }
                        }
                        var departmentManagerCount = 0;
                        foreach (var dpm in de.DepartmentTitles)
                        {
                            if (dpm.Title == "Phó phòng")
                            {
                                departmentManagerCount++;
                            }
                        }
                        if (departmentManagerCount >= 2)
                        {
                            TempData["ErrorMessage"] = "Số lượng chức vụ phó phòng trong một phòng ban không thể lớn hơn 3.";
                            return Redirect("Error");
                        }
                    }

                    if (employeeToUpdate.Active == false)
                    {
                        if (employeeToUpdate.ExitDate < employeeToUpdate.DateOfJoining)
                        {
                            ViewData["ErrorMessage"] = "Ngày nghỉ không thể truớc Ngày tham gia.";
                            ContractsDropDownList();
                            DepartmentsDropDownList();
                            DepartmentTitlesDropDownList();
                            return View(employeeToUpdate);
                        }

                        foreach (var department2 in employeeToUpdate.Departments)
                        {
                            await _userManager.RemoveFromRoleAsync(user, department2.Role);
                        }
                    }

                    else
                    {
                        foreach (var department2 in employeeToUpdate.Departments)
                        {
                            await _userManager.AddToRoleAsync(user, department2.Role);
                        }

                    }

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
            ContractsDropDownList();
            DepartmentsDropDownList();
            DepartmentTitlesDropDownList();
            return View(employeeToUpdate);
        }



        #endregion --------------------------------------

        #region Delete Employee

        #region Delete Employee View
        [Authorize(Roles = "Master, HRDepartmentManager")]
        public async Task<IActionResult> DeleteEmployee(int? employeeID, bool? saveChangesError = false)
        {

            if (employeeID == null)
            {
                return NotFound();
            }

            var employee = await _context.Employees
                .Include(da => da.DepartmentAssignments)
                    .ThenInclude(d => d.Department)
                .SingleOrDefaultAsync(m => m.EmployeeID == employeeID);
            /////////////----------------------------
            var userId = employee.UserId;
            AppUser user = await _userManager.FindByIdAsync(userId);
            foreach (var role in employee.DepartmentAssignments)
            {
                if (role.Department.Role == "Master")
                {
                    TempData["ErrorMessage"] = "Không thể xóa user Master";
                    return Redirect("Error");
                }
            }


            /////////----------------------------
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
        #endregion

        #region Delete Confirmed

        [Authorize(Roles = "Master, HRDepartmentManager")]
        public async Task<IActionResult> DeleteConfirmed(int employeeID)
        {
            var employee = await _employeeRepository.SearchAsync(employeeID);

            if (employee == null)
            {
                return Redirect("Index");
            }
            _context.Employees.Remove(employee);
            await _context.SaveChangesAsync();
            try
            {

                var userId = employee.UserId;
                AppUser user = await _userManager.FindByIdAsync(userId);
                IdentityResult result = await _userManager.DeleteAsync(user);
                if (result.Succeeded)
                {

                    ViewData["ErrorMessage"] = "Tài khoản " + user.UserName + " đã được xóa.";
                    return RedirectToAction("Index");
                }
                else
                {
                    ViewData["ErrorMessage"] = "Lỗi trong quá trình xóa tài khoản.";
                    return Redirect("Index");
                }
                //TempData["message"] = $"{employee.Name} đã được xóa.";        
            }
            catch (DbUpdateException /* ex */)
            {
                //Log the error (uncomment ex variable name and write a log.)
                return RedirectToAction("DeleteEmployee", new { id = employeeID, saveChangesError = true });
            }
        }
        #endregion

        #endregion -------------------------------------- --------------------------------------

        #endregion --------------------------------------

        #region Employees' Family Management

        [Authorize(Roles = "Master, HRDepartment")]
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

        [Authorize(Roles = "Master, HRDepartment")]
        public IActionResult AddFamily(int employeeID)
        {
            FamilyRelation familyRelation = new FamilyRelation()
            {
                EmployeeId = employeeID
            };
            return View(familyRelation);
        }

        [Authorize(Roles = "Master, HRDepartment")]
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

        [Authorize(Roles = "Master, HRDepartment")]
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

        [Authorize(Roles = "Master, HRDepartment")]
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

        [Authorize(Roles = "Master, HRDepartment")]
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

        [Authorize(Roles = "Master, HRDepartment")]
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
        private bool EmailExist(Employee employee)
        {
            return _context.Employees.Any(e => e.Email == employee.Email);
        }
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

        #region Error View
        public IActionResult Error()
        {
            return View();
        }
        #endregion

        #region Email Exist Edit
        private bool EmailExistEdit(Employee employeeToUpdate)
        {
            var email = employeeToUpdate.Email;
            var employees = _context.Employees;
            int count = 0;
            foreach (var employee in employees)
            {
                if (employee.Email == email)
                {
                    count++;
                    if (count == 2)
                    {
                        return true;
                    }
                }
            }
            return false;
        }
        #endregion

        #region Employee Exist
        private bool EmployeeExists(int employeeCode)
        {
            return _context.Employees.Any(e => e.EmployeeCode == employeeCode);
        }
        #endregion

        #region Code Generator
        private int CodeGenerator(Employee employee, Department department)
        {
            var left = (10 + department.DepartmentCode).ToString();
            var right = (10000 + department.DepartmentTitles.Count);
            var right2 = right.ToString().Substring(1, 4);
            var employeeCode = int.Parse(left + right2);
            return employeeCode;
        }
        #endregion

        #region Get Current User
        private Task<AppUser> GetCurrentUserAsync()
        {
            return _userManager.GetUserAsync(HttpContext.User);
        }
        #endregion

        #endregion

    }
}