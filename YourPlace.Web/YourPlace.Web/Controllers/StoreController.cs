using Microsoft.AspNetCore.Mvc;
using System;
using YourPlace.Models.Models;
using YourPlace.Web.Models.Store;

namespace YourPlace.Web.Controllers
{
    public class StoreController : Controller
    {
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


            return this.View();
        }
    }
}
