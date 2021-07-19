

using System;
using System.ComponentModel.DataAnnotations;

using static YourPlace.Models.ModelConstants.ModelConstants.DistrictConstants;

namespace YourPlace.Models.Models
{
    public class District
    {
        public District()
        {
            this.Id = Guid.NewGuid().ToString();
        }

        public string Id { get; set; }

        [Required]
        [MaxLength(DistrictNameMaxLenght)]
        public string Name { get; set; }
    }
}
