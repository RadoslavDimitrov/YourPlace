using System.Collections.Generic;

using YourPlace.Models.Models;
using YourPlace.Web.Models.User;

namespace YourPlace.Web.Services.User
{
    public interface IUserService
    {
        List<UserBookHourViewModel> UserBookedHours(string userId);

        ProfileUserViewModel UserWithRole(string userId);

        YourPlace.Models.Models.User GetCurrentUser(string userId);

        string GetCurrentUserStoreId(string userId);
    }
}
