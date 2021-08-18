using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System;

using YourPlace.Models.Models;
using YourPlace.Web.Infrastructure;
using YourPlace.Web.Models.Store;
using YourPlace.Web.Services.Store;
using YourPlace.Web.Services.Store.Models;
using YourPlace.Web.Services.User;

namespace YourPlace.Web.Controllers
{
    public class StoreController : Controller
    {
        private readonly UserManager<User> userManager;
        private readonly IStoreService storeService;
        private readonly IUserService userService;

        public StoreController(
            UserManager<User> userManager,
            IStoreService storeService,
            IUserService userService)
        {
            this.userManager = userManager;
            this.storeService = storeService;
            this.userService = userService;
        }

        [Authorize]
        public IActionResult Create()
        {

            var userStoreId = this.userService.GetCurrentUserStoreId(this.User.GetId());

            if (userStoreId != null)
            {
                return this.RedirectToAction("MyStore");
            }

            return this.View();
        }

        [Authorize]
        [HttpPost]
        public IActionResult Create(CreateStoreViewModel model)
        {

            if (!ModelState.IsValid)
            {
                return this.View(model);
            }

            if (String.IsNullOrWhiteSpace(model.District))
            {
                return this.View(model);
            }

            if (String.IsNullOrWhiteSpace(model.Town))
            {
                return this.View(model);
            }
            var user = this.userService.GetCurrentUser(this.User.GetId());

            var storeId = this.storeService.CreateStore(model.Name,
                model.Type,
                model.Description,
                model.OpenHour,
                model.CloseHour,
                model.PictureUrl,
                model.Town,
                model.District,
                user);

            return this.RedirectToAction("MyStore", "Store");
        }

        [Authorize]
        public IActionResult MyStore()
        {
            var userStoreId = this.userService.GetCurrentUserStoreId(this.User.GetId());

            var store = this.storeService.ListStore(userStoreId);
            
            return View(store);
        }

        public IActionResult All([FromQuery] AllStoresQueryModel query)
        {
            var queryResult = this.storeService.All(
                query.SearchTerm,
                query.TownName,
                query.DistrictName,
                query.CurrentPage,
                AllStoresQueryModel.StoresPerPage);

            var townNames = this.storeService.AllTownName();
            var districtNames = this.storeService.AllDistrictName();

            query.Towns = townNames;
            query.Districts = districtNames;
            query.TotalStores = queryResult.TotalStores;
            query.Stores = queryResult.Stores;

            return this.View(query);
        }

        public IActionResult Visit(string storeId)
        {
            var store = this.storeService.ListStore(storeId);

            var userStoreId = this.userService.GetCurrentUserStoreId(this.User.GetId());

            if(userStoreId == store.Id)
            {
                return RedirectToAction("MyStore");
            }

            if (store == null)
            {
                return RedirectToAction("All");
            }

            return this.View(store);
        }

        [Authorize]
        public IActionResult MyStoreBookedHours()
        {
            var userStoreId = this.userService.GetCurrentUserStoreId(this.User.GetId());

            var bookedHours = this.storeService.StoreBookedHours(userStoreId);

            return this.View(bookedHours);
        }
    }
}
