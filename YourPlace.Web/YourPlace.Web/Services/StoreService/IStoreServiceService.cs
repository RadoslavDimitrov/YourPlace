using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using YourPlace.Web.Models.StoreService;

namespace YourPlace.Web.Services.StoreService
{
    public interface IStoreServiceService
    {
        bool Create(string name, string description, decimal price, string storeId, string userId);

        DetailsStoreServiceViewModel ServiceById(string id);

        bool Edit(string name, string description, decimal price, string id, string userId);

        bool Delete(string id, string userId);

        DateTime ParseDate(string date);

        BookAnHourViewModel FreeHours(string storeServiceId, DateTime currDate);

        string BookHour(int hour, string storeName, string storeServiceName, string storeServiceId, string storeId, DateTime currDate, YourPlace.Models.Models.User user);
    }
}
