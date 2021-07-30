using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using YourPlace.Models.Models;

namespace YourPlace.Web.Services.Raiting
{
    public interface IRatingService
    {
        void Rate(int rate, string storeId, string userId);
        YourPlace.Models.Models.Store Store(string storeId);

        bool IsUserRate(string userId ,string storeId);
    }
}
