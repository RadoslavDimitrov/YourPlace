using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using YourPlace.Models.Models;

namespace YourPlace.Web.Models.User
{
    public class UserBookHourViewModel
    {
        public string Id { get; set; }

        public string StoreServiceId { get; set; }

        [Required]
        public StoreServices StoreService { get; set; }

        [Required]
        public string StoreServiceName { get; set; }
        public int StartFrom { get; set; }

        [Required]
        public string StoreName { get; set; }

        public DateTime Date { get; set; }
    }
}
