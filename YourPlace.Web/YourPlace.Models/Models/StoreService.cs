
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

using static YourPlace.Models.ModelConstants.ModelConstants;

namespace YourPlace.Models.Models
{
    public class StoreService
    {
        public StoreService()
        {
            this.Id = Guid.NewGuid().ToString();
            this.bookedHours = new List<BookedHour>();
        }

        [Key]
        public string Id { get; set; }

        [Required]
        [MaxLength(StoreServiceNameMaxLenght)]
        public string Name { get; set; }

        [Required]
        [MaxLength(StoreServiceDescriptionMaxLenght)]
        public string Description { get; set; }

        [Range(StoreServicePriceMin, StoreServicePriceMax)]
        public decimal Price { get; set; }

        public string StoreId { get; set; }

        [Required]
        public Store Store { get; set; }

        public ICollection<BookedHour> bookedHours { get; set; }
    }
}
