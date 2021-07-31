using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Linq;

using YourPlace.Data.Data;
using YourPlace.Models.Models;
using YourPlace.Web.Infrastructure;
using YourPlace.Web.Models.Comment;
using static YourPlace.Web.Infrastructure.ApplicationMessages.Exception;

namespace YourPlace.Web.Controllers
{
    [Authorize]
    public class CommentController : Controller
    {
        private readonly ApplicationDbContext data;

        public CommentController(ApplicationDbContext data)
        {
            this.data = data;
        }

        public IActionResult Create(string storeId)
        {
            return View();
        }

        [HttpPost]
        public IActionResult Create(CreateCommentViewModel model ,string storeId)
        {
            var userId = this.User.GetId();

            if (!ModelState.IsValid)
            {
                return this.View(storeId);
            }

            if(storeId == null)
            {
                return this.BadRequest();
            }

            var comment = new Comment
            {
                Id = Guid.NewGuid().ToString(),
                Description = model.Description,
                StoreId = model.StoreId,
                UserId = userId
            };

            this.data.Comments.Add(comment);
            this.data.SaveChanges();

            return RedirectToAction("Visit", "Store", storeId);
        }

        public IActionResult Mine()
        {
            var userId = this.User.GetId();

            var comments = this.data.Comments
                .Where(c => c.UserId == userId)
                .Select(c => new MineCommentsViewModel()
                {
                    Id = c.Id,
                    Description = c.Description,
                    StoreName = c.Store.Name,
                    StoreId = c.StoreId
                })
                .ToList();

            return this.View(comments);
        }

        public IActionResult Delete(string id)
        {
            var userId = this.User.GetId();

            var comment = this.data.Comments.Where(c => c.Id == id && c.UserId == userId).FirstOrDefault();

            if(comment == null)
            {
                return this.View("NotFound", CommentDoesNotExist);
            }

            var store = this.data.Stores.Where(s => s.Id == comment.StoreId).FirstOrDefault();

            if(store == null)
            {
                return this.View("NotFound", StoreDoesNotExist);
            }

            store.Comments.Remove(comment);
            this.data.Comments.Remove(comment);
            this.data.SaveChanges();

            return this.RedirectToAction("Mine");
        }
    }
}
