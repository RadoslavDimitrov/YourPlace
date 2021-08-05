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
        private readonly RoleManager<IdentityRole> roleManager;

        public AdminService(ApplicationDbContext data, UserManager<User> userManager, RoleManager<IdentityRole> roleManager)
        {
            this.data = data;
            this.userManager = userManager;
            this.roleManager = roleManager;
        }

        public List<UserServiceModel> AllUsers()
        {
            var users = this.data.Users.AsQueryable();

            var allUsers = new List<UserServiceModel>();

            Task.Run(async () =>
            {
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
            })
                .GetAwaiter()
                .GetResult();
            

            return allUsers;
        }

        public bool ChangeRole(string userId, string rolename)
        {
            var user = this.GetUser(userId);

            if(user == null)
            {
                return false;
            }

            Task.Run(async () =>
            {
                var userRoles = await userManager.GetRolesAsync(user);

                if (!userRoles.Any())
                {
                    return;
                }

                await userManager.RemoveFromRoleAsync(user, userRoles[0]);

                await userManager.AddToRoleAsync(user, rolename);
            })
                .GetAwaiter()
                .GetResult();

            this.data.SaveChanges();

            return true;
        }

        public DeleteUserServiceModel DeleteUser(string userId)
        {
            var model = new DeleteUserServiceModel{Id = userId };

            var user = this.GetUser(userId);

            if(user == null)
            {
                model.Result = false;
            }
            else
            {
                var bookHours = this.data.BookedHours.Where(b => b.UserId == user.Id).ToList();

                foreach (var hour in bookHours)
                {
                    hour.UserId = null;
                }

                this.data.Users.Remove(user);
                this.data.SaveChanges();

                model.Result = true;
            }         

            return model;
        }

        public List<string> GetAllRoles()
        {
            return this.data.Roles.Select(r => r.Name).ToList();
        }

        private User GetUser(string id)
        {
            return this.data.Users.Where(u => u.Id == id).FirstOrDefault();
        }
    }
}
