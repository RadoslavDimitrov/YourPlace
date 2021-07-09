using System;
using System.ComponentModel.DataAnnotations;

using static YourPlace.Models.ModelConstants.ModelConstants;

namespace YourPlace.Models.Models
{
    public class Raiting
    {
        public Raiting()
        {
            this.Id = Guid.NewGuid().ToString();
        }

        [Key]
        public string Id { get; set; }

        [Range(RaitingRateMinValue, RaitingRateMaxValue)]
        public int StoreRaiting { get; set; }

        public string StoreId { get; set; }

        [Required]
        public Store Store { get; set; }
    }
}
