
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace YourPlace.Web.Models.StoreService
{
    public class BookAnHourViewModel
    {
        [Required]
        public string StoreServiceId { get; set; }
        [Required]
        public string ShopName { get; set; }
        [Required]
        public string StoreServiceName { get; set; }
        public decimal Price { get; set; }
        public List<int> FreeHours { get; set; }
    }
}
