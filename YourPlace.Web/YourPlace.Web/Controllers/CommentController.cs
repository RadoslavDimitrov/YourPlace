using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Linq;

using YourPlace.Data.Data;
using YourPlace.Models.Models;
using YourPlace.Web.Infrastructure;
using YourPlace.Web.Models.Comment;
using YourPlace.Web.Services.Comment;

using static YourPlace.Web.Infrastructure.ApplicationMessages.Exception;

namespace YourPlace.Web.Controllers
{
    [Authorize]
    public class CommentController : Controller
    {
        private readonly ApplicationDbContext data;
        private readonly ICommentService commentService;

        public CommentController(ApplicationDbContext data, 
            ICommentService commentService)
        {
            this.data = data;
            this.commentService = commentService;
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

            var comment = this.commentService.Create(model.Description, storeId, userId);

            return RedirectToAction("Visit", "Store", storeId);
        }

        public IActionResult Mine()
        {
            var userId = this.User.GetId();

            var comments = this.commentService.CommentsByUser(userId);

            return this.View(comments);
        }

        public IActionResult Delete(string id)
        {
            var userId = this.User.GetId();

            var comment = this.commentService.Delete(id, userId);

            if(!comment)
            {
                return this.View("NotFound", CommentDoesNotExist);
            }

            return this.RedirectToAction("Mine");
        }
    }
}
