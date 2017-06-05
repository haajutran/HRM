using System.Linq;
using System.ComponentModel.DataAnnotations;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.AspNetCore.Mvc;
using HRM.Models;
using System.Collections.Generic;
using Microsoft.AspNetCore.Authorization;

namespace HRM.Controllers
{
    [Authorize(Roles = "Master, HRDepartmentManager")]
    public class RoleController : Controller
    {
        #region Requires

        private RoleManager<IdentityRole> roleManager;
        private UserManager<AppUser> userManager;
        public RoleController(RoleManager<IdentityRole> roleMgr, UserManager<AppUser> userMrg)
        {
            roleManager = roleMgr;
            userManager = userMrg;
        }

        #endregion

        #region Actions
        public ViewResult Index() {
            return View(roleManager.Roles.OrderBy(r => r.Name));
        }
        public IActionResult Create() => View();
        [HttpPost]
        public async Task<IActionResult> Create([Required]string name)
        {
            if (ModelState.IsValid)
            {
                IdentityResult result
                = await roleManager.CreateAsync(new IdentityRole(name));
                if (result.Succeeded)
                {
                    return RedirectToAction("Index");
                }
                else
                {
                    AddErrorsFromResult(result);
                }
            }
            return View(name);
        }
        [HttpPost]
        public async Task<IActionResult> Delete(string id)
        {
            IdentityRole role = await roleManager.FindByIdAsync(id);
            if (role != null)
            {
                IdentityResult result = await roleManager.DeleteAsync(role);
                if (result.Succeeded)
                {
                    return RedirectToAction("Index");
                }
                else
                {
                    AddErrorsFromResult(result);
                }
            }
            else
            {
                ModelState.AddModelError("", "Chưa có phân quyền nào!");
            }
            return View("Index", roleManager.Roles);
        }
        [Authorize(Roles = "Master, HRDepartmentManager")]
        public async Task<IActionResult> Edit(string id)
        {
            IdentityRole role = await roleManager.FindByIdAsync(id);
            List<AppUser> members = new List<AppUser>();
            List<AppUser> nonMembers = new List<AppUser>();
            foreach (AppUser user in userManager.Users)
            {                
                var list = await userManager.IsInRoleAsync(user, role.Name)
                ? members : nonMembers;
                list.Add(user);
            }
            return View(new RoleEditModel
            {
                Role = role,
                Members = members,
                NonMembers = nonMembers
            });
        }

        [HttpPost]
        [Authorize(Roles = "Master, HRDepartmentManager")]
        public async Task<IActionResult> Edit(RoleModificationModel model)
        {
            IdentityResult result;
            if (ModelState.IsValid)
            {
                foreach (string userId in model.IdsToAdd ?? new string[] { })
                {
                    AppUser user = await userManager.FindByIdAsync(userId);

                    if (user != null)
                    {
                        var roles = await userManager.GetRolesAsync(user);
                        foreach(var role in roles)
                        {
                            if(role == "Master")
                            {
                                var currentUser = await GetCurrentUserAsync();
                                var count = 0;
                                var currentUserRoles = await userManager.GetRolesAsync(currentUser);
                                foreach(var currentUserRole in currentUserRoles)
                                {
                                    if(currentUserRole == "Master")
                                    {
                                        count++;
                                        if(count == 1)
                                        {
                                            break;
                                        }
                                    }
                                }
                                if(count == 0)
                                {
                                    TempData["ErrorMessage"] = "Bạn không có quyền chỉnh sửa user Master.";
                                    return Redirect("Error");
                                }
                            }
                        }
                        result = await userManager.AddToRoleAsync(user,
                        model.RoleName);
                        if (!result.Succeeded)
                        {
                            AddErrorsFromResult(result);
                        }
                    }
                }
                foreach (string userId in model.IdsToDelete ?? new string[] { })
                {
                    AppUser user = await userManager.FindByIdAsync(userId);
                    if (user != null)
                    {
                        result = await userManager.RemoveFromRoleAsync(user,
                        model.RoleName);
                        if (!result.Succeeded)
                        {
                            AddErrorsFromResult(result);
                        }
                    }
                }
            }
            if (ModelState.IsValid)
            {
                return RedirectToAction(nameof(Index));
            }
            else
            {
                return await Edit(model.RoleId);
            }
        }

        public IActionResult Error()
        {
            return View();
        }

        #endregion

        #region Methods
        private void AddErrorsFromResult(IdentityResult result)
        {
            foreach (IdentityError error in result.Errors)
            {
                ModelState.AddModelError("", error.Description);
            }
        }

        private Task<AppUser> GetCurrentUserAsync()
        {
            return userManager.GetUserAsync(HttpContext.User);
        }
        #endregion
    }
}