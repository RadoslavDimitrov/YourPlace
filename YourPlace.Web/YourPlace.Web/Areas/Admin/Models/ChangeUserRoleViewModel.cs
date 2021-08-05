using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace YourPlace.Web.Areas.Admin.Models
{
    public class ChangeUserRoleViewModel
    {
        public string Id { get; set; }

        public string Rolename { get; set; }

        public List<string> AllRoles { get; set; }
    }
}
