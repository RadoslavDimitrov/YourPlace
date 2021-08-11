using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace YourPlace.Web.Models.Store
{
    public class StoreBookedHoursViewModel
    {

        public List<ListStoreBookedHoursViewModel> CurrDay { get; set; } = new List<ListStoreBookedHoursViewModel>();
        public List<ListStoreBookedHoursViewModel> PastDays { get; set; } = new List<ListStoreBookedHoursViewModel>();
        public List<ListStoreBookedHoursViewModel> CommingDays { get; set; } = new List<ListStoreBookedHoursViewModel>();
    }
}
