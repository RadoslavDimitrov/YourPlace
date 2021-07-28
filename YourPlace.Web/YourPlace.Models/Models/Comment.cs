

using System;
using System.ComponentModel.DataAnnotations;

using static YourPlace.Models.ModelConstants.ModelConstants.CommentConstants;

namespace YourPlace.Models.Models
{
    public class Comment
    {
        public Comment()
        {
            this.Id = Guid.NewGuid().ToString();
        }

        [Key]
        public string Id { get; set; }

        [Required]
        [MaxLength(CommentDescriptionMaxLenght)]
        public string Description { get; set; }

        public string UserId { get; set; }
        [Required]
        public User User { get; set; }


        public string StoreId { get; set; }

        [Required]
        public Store Store { get; set; }
    }
}
