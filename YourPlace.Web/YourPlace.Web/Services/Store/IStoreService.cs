
using System.Collections.Generic;
using YourPlace.Models.Models;
using YourPlace.Web.Models.Store;
using YourPlace.Web.Services.Store.Models;

namespace YourPlace.Web.Services.Store
{
    public interface IStoreService
    {
        public AllStoreServiceModel All(
            string searchTerm,
            string townName,
            string districtName,
            int currentPage,
            int carsPerPage);

        public List<string> AllTownName();

        public List<string> AllDistrictName();

        public Town CreateTown(string name);

        public District CreateDistrict(string name);

        public string CreateStore(string name,
                string type,
                string description,
                int openHour,
                int closeHour,
                string pictureUrl,
                string town,
                string district,
                YourPlace.Models.Models.User user);

        public ListStoreViewModel ListStore(string storeId);

        public StoreBookedHoursViewModel StoreBookedHours(string storeId);


    }
}
