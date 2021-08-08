using Microsoft.AspNetCore.Identity;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using YourPlace.Data.Data;
using YourPlace.Web.Models.User;

namespace YourPlace.Web.Services.User
{
    public class UserService : IUserService
    {
        private readonly ApplicationDbContext data;
        private readonly RoleManager<IdentityRole> roleManager;
        private readonly UserManager<YourPlace.Models.Models.User> userManager;

        public UserService(ApplicationDbContext data,
            RoleManager<IdentityRole> roleManager,
            UserManager<YourPlace.Models.Models.User> userManager)
        {
            this.data = data;
            this.roleManager = roleManager;
            this.userManager = userManager;
        }

        public YourPlace.Models.Models.User GetCurrentUser(string userId)
        {
            return this.data.Users.Where(u => u.Id == userId).FirstOrDefault();
        }

        public string GetCurrentUserStoreId(string userId)
        {
            var user = this.GetCurrentUser(userId);

            return user.StoreId;
        }

        public bool isUserOwner(string userId, string storeId)
        {
            return this.data.Users.Any(u => u.Id == userId && u.StoreId == storeId);
        }

        public List<UserBookHourViewModel> UserBookedHours(string userId)
        {
            var bookHour = this.data.BookedHours
                .Where(b => b.UserId == userId)
                .Select(b => new UserBookHourViewModel
                {
                    Id = b.Id,
                    StartFrom = b.StartFrom,
                    StoreName = b.StoreName,
                    StoreServiceId = b.StoreServiceId,
                    StoreServiceName = b.StoreServiceName,
                    Date = b.Date
                })
                .ToList();


            return bookHour;
        }

        public ProfileUserViewModel UserWithRole(string userId)
        {
            var user = this.data.Users.Where(u => u.Id == userId).FirstOrDefault();

            ProfileUserViewModel model = new ProfileUserViewModel()
            {
                UserName = user.UserName,
                Email = user.Email,
                RoleName = this.GetUserRoleAsync(user).GetAwaiter().GetResult()
            }; 

            return model;
        }

        private async Task<string> GetUserRoleAsync(YourPlace.Models.Models.User user)
        {
            var roles = await this.userManager.GetRolesAsync(user);

            if (!roles.Any())
            {
                return null;
            }

            return roles[0];
        } 
    }
}
