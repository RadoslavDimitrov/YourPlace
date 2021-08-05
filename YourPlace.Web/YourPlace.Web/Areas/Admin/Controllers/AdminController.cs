using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
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
    }
}
