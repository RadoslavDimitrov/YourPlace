using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using YourPlace.Models.Models;
using static YourPlace.Models.ModelConstants.ModelConstants.StoreConstants;

namespace YourPlace.Web.Models.Store
{
    public class ListStoreViewModel
    {
        [Required]
        public string Id { get; set; }
        [Required]
        [StringLength(StoreNameMaxLenght, MinimumLength = StoreNameMinLenght)]
        public string Name { get; set; }

        [Required]
        public string Type { get; set; }

        [Required]
        [StringLength(int.MaxValue, MinimumLength = StoreDescriptionMinLenght, ErrorMessage = "The field Description must be a string with a minimum length of {2}.")]
        public string Description { get; set; }

        [Range(StoreMinOpenHour, StoreMaxOpenHour)]
        public int OpenHour { get; set; }

        [Range(StoreMinOpenHour, StoreMaxOpenHour)]
        public int CloseHour { get; set; }

        public string PictureUrl { get; set; }

        [Required]
        public string Town { get; set; }

        [Required]
        public string District { get; set; }

        public double? Raiting { get; set; }

        public ICollection<StoreServices> StoreServices { get; set; } = new List<StoreServices>();
        public ICollection<YourPlace.Models.Models.Comment> Comments { get; set; } = new List<YourPlace.Models.Models.Comment>();
    }
}
