using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace YourPlace.Web.Models.Comment
{
    public class MineCommentsViewModel
    {
        public string Id { get; set; }
        public string Description { get; set; }

        [Display(Name = "Store name")]
        public string StoreName { get; set; }
        public string StoreId { get; set; }
    }
}
