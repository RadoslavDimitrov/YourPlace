using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace YourPlace.Web.Models.User
{
    public class ListUserBookedHoursViewModel
    {
        public List<UserBookHourViewModel> CurrDay { get; set; } = new List<UserBookHourViewModel>();
        public List<UserBookHourViewModel> PastDays { get; set; } = new List<UserBookHourViewModel>();
        public List<UserBookHourViewModel> CommingDays { get; set; } = new List<UserBookHourViewModel>();
    }
}
