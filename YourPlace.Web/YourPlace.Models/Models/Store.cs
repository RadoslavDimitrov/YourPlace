
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using YourPlace.Models.ModelConstants;
using YourPlace.Models.Models;

using static YourPlace.Models.ModelConstants.ModelConstants;

namespace YourPlace.Models.Models
{
    public class Store
    {
        public Store()
        {
            this.Id = Guid.NewGuid().ToString();

            this.Comments = new List<Comment>();
            this.StoreServices = new List<StoreService>();
            this.Raitings = new List<Raiting>();
        }

        [Key]
        public string Id { get; set; }
        [Required]
        [MaxLength(StoreNameMaxLenght)]
        public string Name { get; set; }
        [Required]
        public string Type { get; set; }

        public string TownId { get; set; }
        [Required]
        public Town Town { get; set; }
        public string DistrictId { get; set; }
        [Required]
        public District District { get; set; }
        [Range(StoreMinOpenHour, StoreMaxOpenHour)]
        public int OpenHour { get; set; }
        [Range(StoreMinOpenHour, StoreMaxOpenHour)]
        public int CloseHour { get; set; }

        public string PictureUrl { get; set; }

        public ICollection<StoreService> StoreServices { get; set; }
        public ICollection<Comment> Comments { get; set; }
        public ICollection<Raiting> Raitings { get; set; }


    }
}
