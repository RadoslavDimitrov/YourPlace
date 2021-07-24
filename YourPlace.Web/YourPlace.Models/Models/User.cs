
using Microsoft.AspNetCore.Identity;
using System.Collections.Generic;

namespace YourPlace.Models.Models
{
    public class User : IdentityUser
    {
        public string StoreId { get; set; }
        public Store Store { get; set; }

        public ICollection<BookedHour> bookedHours { get; set; } = new List<BookedHour>();
    }
}
