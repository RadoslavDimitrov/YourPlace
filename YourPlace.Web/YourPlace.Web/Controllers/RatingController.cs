using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

using YourPlace.Web.Infrastructure;
using YourPlace.Web.Models.Rate;
using YourPlace.Web.Services.Raiting;

namespace YourPlace.Web.Controllers
{
    [Authorize]
    public class RatingController : Controller
    {
        private readonly IRatingService ratingService;

        public RatingController(IRatingService ratingService)
        {
            this.ratingService = ratingService;
        }

        public IActionResult Give(string storeId)
        {
            var isUserRate = this.ratingService.IsUserRate(this.User.GetId(), storeId);

            if (isUserRate)
            {
                return this.View("UserAlreadyRate", storeId);
            }

            return View();
        }
        [HttpPost]
        public IActionResult Give(AvalibleRaitingViewModel rating, string storeId)
        {
            var store = this.ratingService.Store(storeId);
            var userId = this.User.GetId();

            this.ratingService.Rate(rating.Raiting, storeId, userId);

            return RedirectToAction("Visit", "Store", storeId);
        }
    }
}
