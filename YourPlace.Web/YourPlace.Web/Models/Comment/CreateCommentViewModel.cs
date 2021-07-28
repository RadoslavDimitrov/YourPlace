using System.ComponentModel.DataAnnotations;
using static YourPlace.Models.ModelConstants.ModelConstants.CommentConstants;

namespace YourPlace.Web.Models.Comment
{
    public class CreateCommentViewModel
    {
        public string Id { get; set; }

        [Required]
        [StringLength(CommentDescriptionMaxLenght, MinimumLength = CommentDescriptionMinLenght)]
        public string Description { get; set; }
        
        [Required]
        public string StoreId { get; set; }

    }
}
