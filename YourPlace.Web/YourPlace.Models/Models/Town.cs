using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

using static YourPlace.Models.ModelConstants.ModelConstants;

namespace YourPlace.Models.Models
{
    public class Town
    {
        public Town()
        {
            this.Id = Guid.NewGuid().ToString();
            this.Districts = new List<District>();
        }

        [Key]
        public string Id { get; set; }

        [Required]
        [MaxLength(DistrictNameMaxLenght)]
        public string Name { get; set; }

        public ICollection<District> Districts { get; set; }
    }
}
