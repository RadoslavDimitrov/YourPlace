using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using YourPlace.Data.Data;
using YourPlace.Models.Models;
using YourPlace.Web.Areas.Admin.Services.Models;

namespace YourPlace.Web.Areas.Admin.Services
{
    public class AdminService : IAdminService
    {
        private readonly ApplicationDbContext data;
        private readonly UserManager<User> userManager;

        public AdminService(ApplicationDbContext data, UserManager<User> userManager)
        {
            this.data = data;
            this.userManager = userManager;
        }

        public async Task<List<UserServiceModel>> AllUsers()
        {
            var users = this.data.Users.AsQueryable();

            var allUsers = new List<UserServiceModel>();

            foreach (var user in users)
            {
                var newUser = new UserServiceModel
                {
                    Id = user.Id,
                    Email = user.Email,
                    Username = user.UserName,
                    Rolename = await userManager.GetRolesAsync(user)
                };

                allUsers.Add(newUser);
            }

            return allUsers;
        }
    }
}
