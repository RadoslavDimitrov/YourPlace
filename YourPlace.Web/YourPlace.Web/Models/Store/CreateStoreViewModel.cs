using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

using static YourPlace.Models.ModelConstants.ModelConstants;

namespace YourPlace.Web.Models.Store
{
    public class CreateStoreViewModel
    {

        [Required]
        [StringLength(StoreNameMaxLenght, MinimumLength = StoreNameMinLenght)]
        public string Name { get; set; }

        [Required]
        public string Type { get; set; }

        [Required]
        [StringLength(int.MaxValue,MinimumLength = StoreDescriptionMinLenght, ErrorMessage = "The field Description must be a string with a minimum length of {2}.")]
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
    }
}
