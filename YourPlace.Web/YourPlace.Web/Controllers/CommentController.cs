using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using YourPlace.Data.Data;
using YourPlace.Models.Models;
using YourPlace.Web.Infrastructure;
using YourPlace.Web.Models.Comment;

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
    }
}
