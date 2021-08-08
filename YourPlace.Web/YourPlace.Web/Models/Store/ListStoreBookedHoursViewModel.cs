using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace YourPlace.Web.Models.Store
{
    public class ListStoreBookedHoursViewModel
    {
        public string Id { get; set; }
        public string UserId { get; set; }

        public string StoreId { get; set; }

        public string StoreName { get; set; }

        public string StoreServiceName { get; set; }

        public string StoreServiceId { get; set; }

        public int StartFrom { get; set; }

        public DateTime Date { get; set; }

        public string Username { get; set; }
    }
}
