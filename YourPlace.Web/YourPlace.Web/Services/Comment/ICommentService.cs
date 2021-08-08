using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using YourPlace.Web.Models.Comment;

namespace YourPlace.Web.Services.Comment
{
    public interface ICommentService
    {
        string Create(string description, string storeId, string userId);

        List<MineCommentsViewModel> CommentsByUser(string userId);

        bool Delete(string id, string userId);
    }
}
