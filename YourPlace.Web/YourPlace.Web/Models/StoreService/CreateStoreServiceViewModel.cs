﻿using System.ComponentModel.DataAnnotations;

using static YourPlace.Models.ModelConstants.ModelConstants.StoreServiceConstants;

namespace YourPlace.Web.Models.StoreService
{
    public class CreateStoreServiceViewModel
    {
        [Required]
        [MaxLength(StoreServiceNameMaxLenght)]
        public string Name { get; set; }

        [Required]
        [MaxLength(StoreServiceDescriptionMaxLenght)]
        public string Description { get; set; }

        [Range(StoreServicePriceMin, StoreServicePriceMax)]
        public decimal Price { get; set; }
    }
}
