using HRM.Data;
using HRM.Models;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Authorization;

// For more information on enabling MVC for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace HRM.Controllers
{
    [Authorize(Roles = "Manager, Staff")]
    public class DepartmentsController : Controller
    {

        private ApplicationDbContext _context;
        private IDepartmentRepository _departmentRepository;
        private IEmployeeRepository _employeeRepository;
        private IHostingEnvironment _environment;

        public DepartmentsController(IDepartmentRepository repo, ApplicationDbContext context, IHostingEnvironment environment, IEmployeeRepository employeeRepository)
        {
            _departmentRepository = repo;
            _context = context;
            _environment = environment;
            _employeeRepository = employeeRepository;
        }

        // GET: /<controller>/
        public async Task<ViewResult> Index()
        {
            var departments = await _departmentRepository.DepartmentsAsync();
            return View(departments);
        }

        #region Manage Work Hours

        public IActionResult ManageWorkHours(int departmentID)
        {
            WorkHoursManagement whm = new WorkHoursManagement()
            {
                DepartmentTasks = _context.DepartmentTasks
                .Include(d => d.Department)
                .Include(d => d.Employee)
                .Select(a => a).Where(a => a.Department.DepartmentID == departmentID)
            };
            return View(whm);
        }

        public async Task<IActionResult> UpdateWorkHours(int departmentTaskID)
        {
            var dTask = await _departmentRepository.SearchTaskAsync(departmentTaskID);

            ViewBag.DepartmentName = (await _departmentRepository.SearchByIDAsync(dTask.Department.DepartmentID)).DepartmentName;

            return View(dTask);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> UpdateWorkHours([Bind("DepartmentTaskID, WorkHours")]DepartmentTask departmentTask)
        {
            var dT = await _departmentRepository.SearchTaskAsync(departmentTask.DepartmentTaskID);
            dT.WorkHours = departmentTask.WorkHours;
            _context.Update(dT);
            await _context.SaveChangesAsync();

            return View(dT);
        }

        #endregion

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
                var departmentTest = _context.Departments.SingleOrDefault(d => d.DepartmentCode == department.DepartmentCode);
                if(departmentTest != null)
                {
                    return Redirect("DepartmentCodeError");
                }
                _context.Add(department);
                await _context.SaveChangesAsync();
                return RedirectToAction("Index", "Departments");
            }
            return View(department);
        }

        public IActionResult DepartmentCodeError()
        {
            return View();
        }

        #endregion

        #region Add Title
        public IActionResult AddTitle(int employeeID)
        {
            ListOfDepartments();
            ListOfTitles();
            DepartmentTitle departmentTitle = new DepartmentTitle()
            {
                Employee = new Employee()
            };
            departmentTitle.Employee.EmployeeCode = employeeID;
            return View(departmentTitle);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> AddTitle([Bind("Title, Description, Department, Employee")] DepartmentTitle departmentTitle)
        {

            if (ModelState.IsValid)
            {
                var employee = await _employeeRepository.SearchAsync(departmentTitle.Employee.EmployeeCode);
                if (employee == null)
                {
                    return Redirect("EmployeeNull");
                }
                var dT = new DepartmentTitle()
                {
                    Title = departmentTitle.Title,
                    Description = departmentTitle.Description,
                    Employee = employee,
                    Department = await _departmentRepository.SearchAsync(departmentTitle.Department.DepartmentCode)
                };

                _context.Add(dT);
                await _context.SaveChangesAsync();
                return RedirectToAction("Index", "Departments");
            }
            ListOfDepartments();
            ListOfTitles();

            return View(departmentTitle);
        }
        #endregion

        #region Edit Task

        public async Task<IActionResult> EditTask(int departmentTaskID)
        {
            var departmentTask = await _departmentRepository.SearchTaskAsync(departmentTaskID);

            return View(departmentTask);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> EditTask(DepartmentTask departmentTask)
        {

            if (departmentTask == null) { return NotFound(); }

            if (await TryUpdateModelAsync<DepartmentTask>(departmentTask, ""))
            {
                _context.Update(departmentTask);
                await _context.SaveChangesAsync();
                return RedirectToAction("Index");
            }

            ListOfTitles();

            return View(departmentTask);
        }

        #endregion

        #region Add Task
        public IActionResult AddTask(int departmentID)
        {
            ListOfDepartments();
            ListOfTitles();
            //var dT = await _context.DepartmentTasks
            //    .Include(x => x.Department)
            //    .Include(y => y.Employee)
            //    .SingleOrDefaultAsync(d => d.DepartmentTaskID == departmentID);

            DepartmentTask departmentTask = new DepartmentTask();
            ViewBag.DepartmentID = departmentID;
            //departmentTask.Employee = new Employee();
            //departmentTask.Employee.EmployeeCode = dT.Employee.EmployeeID;

            //departmentTask.Department = new Department();
            //departmentTask.Department.DepartmentCode = dT.Department.DepartmentID;

            return View(departmentTask);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> AddTask([Bind("Title, Description")] DepartmentTask departmentTask, int employeeCode, int departmentID)
        {

            if (ModelState.IsValid)
            {
                var employee = _context.Employees.SingleOrDefault(e => e.EmployeeCode == employeeCode);
                var department = _context.Departments.SingleOrDefault(d => d.DepartmentID == departmentID);
                if (employee == null)
                {
                    return Redirect("EmployeeNull");
                }
                //var dT = new DepartmentTask()
                //{
                //    Title = departmentTask.Title,
                //    Description = departmentTask.Description,
                //    WorkHours = 0,
                //    Employee = await _employeeRepository.SearchAsync(departmentTask.Employee.EmployeeCode),
                //    Department = await _departmentRepository.SearchAsync(departmentTask.Department.DepartmentCode)
                //};
                departmentTask.WorkHours = 0;
                departmentTask.Employee = employee;
                departmentTask.Department = department;

                _context.Add(departmentTask);
                await _context.SaveChangesAsync();
                return RedirectToAction("Index", "Departments");
            }
            ListOfDepartments();
            ListOfTitles();

            return View(departmentTask);
        }
        #endregion

        #region Edit Department
        //[Authorize(Roles = "Manager")]
        public async Task<IActionResult> EditDepartment(int? departmentID)
        {
            if (departmentID == null)
            {
                return NotFound();
            }

            var department = await _context.Departments.SingleOrDefaultAsync(m => m.DepartmentID == departmentID);

            ListOfTitles();

            return View(department);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> EditDepartmentPost(int? departmentCode)
        {

            if (departmentCode == null) { return NotFound(); }

            var departmentToUpdate = await _context.Departments.SingleOrDefaultAsync(d => d.DepartmentCode == departmentCode);

            if (await TryUpdateModelAsync<Department>(departmentToUpdate, ""))
            {
                try
                {
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateException /* ex */)
                {            //Log the error (uncomment ex variable name and write a log.)          
                    ModelState.AddModelError("", "Unable to save changes. " +
                        "Try again, and if the problem persists, " +
                        "see your system administrator.");
                }
                return RedirectToAction("Index");
            }

            ListOfTitles();

            return View(departmentToUpdate);
        }


        #endregion

        #region Department Detail

        public async Task<ViewResult> DepartmentDetail(int departmentID)
        {
            Department department = await _context.Departments
                .Include(ta => ta.DepartmentTasks)
                    .ThenInclude(a => a.Employee)
                        .ThenInclude(d => d.DepartmentTitles)
                .Include(tt => tt.DepartmentTitles)
                    .ThenInclude(t => t.Employee)
                        .ThenInclude(d => d.DepartmentTasks)
                .SingleOrDefaultAsync(d => d.DepartmentID == departmentID);

            ViewData["DepartmentName"] = department.DepartmentName;

            return View(department);
        }

        #endregion

        #region Delete Employee
        [Authorize(Roles = "Manager")]
        public async Task<IActionResult> DeleteDepartment(int? departmentID, bool? saveChangesError = false)
        {
            if (departmentID == null)
            {
                return NotFound();
            }
            var department = await _context.Departments.SingleOrDefaultAsync(d => d.DepartmentID == departmentID);
            if (department == null)
            {
                return NotFound();
            }

            if (saveChangesError.GetValueOrDefault())
            {
                ViewData["ErrorMessage"] =
                    "Failed!";
            }

            return View(department);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int departmentID)
        {
            var department = await _context.Departments.SingleOrDefaultAsync(d => d.DepartmentID == departmentID);
            if (department == null)
            {
                return RedirectToAction("Index", "Departments");
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
                return RedirectToAction("DeleteEmployee", new { id = departmentID, saveChangesError = true });
            }
        }
        #endregion

        #region Delete Title
        public async Task<IActionResult> DeleteTitle(int titleID)
        {
            var department = await _departmentRepository.SearchTitleAsync(titleID);
            if (department == null)
            {
                return RedirectToAction("EmployeesManagement", "Main");
            }

            try
            {
                _context.DepartmentTitles.Remove(department);
                await _context.SaveChangesAsync();

                return RedirectToAction("Index", "Departments");
            }
            catch (DbUpdateException /* ex */)
            {
                //Log the error (uncomment ex variable name and write a log.)
                return RedirectToAction("DeleteTitle", new { id = titleID, saveChangesError = true });
            }
        }
        #endregion

        #region Methods
        private void ListOfTitles()
        {
            var departmentTitles = _context.DepartmentTitles.Select(t => t.Title).Distinct().ToList();
            ViewData["DepartmentTitles"] = new SelectList(departmentTitles);
        }

        private void ListOfDepartments()
        {
            var departments = _context.Departments.Select(t => t).ToList();
            ViewData["Departments"] = new SelectList(departments, "DepartmentID", "DepartmentName");
        }
        #endregion

        #region Employee Null

        public IActionResult EmployeeNull()
        {
            return View();
        }

        #endregion

    }
}
