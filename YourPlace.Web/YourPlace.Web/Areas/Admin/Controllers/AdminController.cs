using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using YourPlace.Web.Areas.Admin.Models;
using YourPlace.Web.Areas.Admin.Services;
using static YourPlace.Web.Areas.Admin.AdminConstants;

namespace YourPlace.Web.Areas.Admin.Controllers
{
    [Area(AdminRoleName)]
    [Authorize(Roles = AdminRoleName)]
    public class AdminController : Controller
    {
        private readonly IAdminService adminService;

        public AdminController(IAdminService adminService)
        {
            this.adminService = adminService;
        }

        public IActionResult AllUsers()
        {
            var users = this.adminService.AllUsers();

            return this.View(users);
        }

        public IActionResult DeleteUser(string userId)
        {
            var model = this.adminService.DeleteUser(userId);

            if (!model.Result)
            {
                return this.RedirectToAction("AllUsers");
            }

            return this.View(model);
        }

        public IActionResult ChangeRole(string userId)
        {
            var allRoles = this.adminService.GetAllRoles();

            return this.View(new ChangeUserRoleViewModel()
            {
                AllRoles = allRoles
            });
        }

        [HttpPost]
        public IActionResult ChangeRole(ChangeUserRoleViewModel model ,string userId)
        {
            if (!ModelState.IsValid)
            {
                return this.View(new ChangeUserRoleViewModel { AllRoles = this.adminService.GetAllRoles() });
            }

            var result = this.adminService.ChangeRole(userId, model.Rolename);

            if (!result)
            {
                return this.View(userId);
            }

            return this.RedirectToAction("AllUsers");
        }
    }
}
