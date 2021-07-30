
using System.Collections.Generic;
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
    }
}
