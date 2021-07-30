
using System.Collections.Generic;
using YourPlace.Web.Models.Store;

namespace YourPlace.Web.Services.Store.Models
{
    public class AllStoreServiceModel
    {
        public int CurrentPage { get; init; }

        public int StoresPerPage { get; init; }

        public int TotalStores { get; init; }

        public List<StoreViewModel> Stores { get; set; }
    }
}
