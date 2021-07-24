using Microsoft.AspNetCore.Mvc;
using System;
using System.Linq;
using System.Security.Claims;
using YourPlace.Data.Data;
using YourPlace.Models.Models;
using YourPlace.Web.Models.Store;

namespace YourPlace.Web.Controllers
{
    public class StoreController : Controller
    {
        private readonly ApplicationDbContext data;

        public StoreController(ApplicationDbContext data)
        {
            this.data = data;
        }

        public IActionResult Create()
        {
            return this.View();
        }

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

            //TODO check in DB for existing districts and town names
            if(!data.Districts.Any(d => d.Name == model.District))
            {

            }

            var district = new District
            {
                Id = Guid.NewGuid().ToString(),
                Name = model.District
            };

            var town = new Town
            {
                Id = Guid.NewGuid().ToString(),
                Name = model.Town
            };

            town.Districts.Add(district);

            var store = new Store
            {
                Id = Guid.NewGuid().ToString(),
                Name = model.Name,
                Type = model.Type,
                Description = model.Description,
                OpenHour = model.OpenHour,
                CloseHour = model.CloseHour,
                PictureUrl = model.PictureUrl,
                Town = town,
                District = district
            };

            this.data.Stores.Add(store);
            this.data.SaveChanges();

            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);

            var storeDto = new ListStoreViewModel
            {
                Id = store.Id,
                Name = store.Name,
                Type = store.Type,
                Description = store.Description,
                OpenHour = store.OpenHour,
                CloseHour = store.CloseHour,
                Town = store.Town.Name,
                District = store.District.Name,
                PictureUrl = store.PictureUrl
            };

            //return this.RedirectToAction("MyStore", "Store", userId);
            return this.RedirectToAction("MyStore", "Store", storeDto);
        }


        public IActionResult MyStore(ListStoreViewModel model)
        {
            //TODO get user store from DB

            return View(model);
        }
    }
}
