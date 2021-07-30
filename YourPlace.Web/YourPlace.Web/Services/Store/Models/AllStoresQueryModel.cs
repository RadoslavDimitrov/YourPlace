using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using YourPlace.Web.Models.Store;

namespace YourPlace.Web.Services.Store.Models
{
    public class AllStoresQueryModel
    {
        public const int StoresPerPage = 3;

        [Display(Name = "Search by text")]
        public string SearchTerm { get; set; }
        public string TownName { get; set; }

        public string DistrictName { get; set; }

        public int CurrentPage { get; set; } = 1;

        public int TotalStores { get; set; }

        public List<string> Towns { get; set; }

        public List<string> Districts { get; set; }

        public List<StoreViewModel> Stores { get; set; }
    }
}
