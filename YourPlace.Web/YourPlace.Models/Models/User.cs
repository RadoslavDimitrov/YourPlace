
using Microsoft.AspNetCore.Identity;
using System.Collections.Generic;

namespace YourPlace.Models.Models
{
    public class User : IdentityUser
    {
        public User()
            :base()
        {
            this.BookedHours = new List<BookedHour>();
            this.Comments = new List<Comment>();
        }
        public string StoreId { get; set; }
        public Store Store { get; set; }

        public ICollection<Comment> Comments { get; set; }
        public ICollection<BookedHour> BookedHours { get; set; }
    }
}
