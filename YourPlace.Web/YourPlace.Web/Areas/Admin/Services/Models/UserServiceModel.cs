using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace YourPlace.Web.Areas.Admin.Services.Models
{
    public class UserServiceModel
    {
        public string Id { get; set; }
        public string Username { get; set; }
        public string Email { get; set; }
        public IList<string> Rolename { get; set; }
    }
}
