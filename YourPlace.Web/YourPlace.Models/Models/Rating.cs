﻿using System;
using System.ComponentModel.DataAnnotations;

using static YourPlace.Models.ModelConstants.ModelConstants.RaitingConstants;

namespace YourPlace.Models.Models
{
    public class Rating
    {
        public Rating()
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

        [Required]
        public string UserId { get; set; }
    }
}
