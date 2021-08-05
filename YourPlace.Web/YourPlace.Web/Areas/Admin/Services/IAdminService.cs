using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using YourPlace.Web.Areas.Admin.Services.Models;

namespace YourPlace.Web.Areas.Admin.Services
{
    public interface IAdminService
    {
        Task<List<UserServiceModel>> AllUsers();
    }
}
