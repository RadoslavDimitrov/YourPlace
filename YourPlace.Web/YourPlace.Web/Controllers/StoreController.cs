using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

using YourPlace.Data.Data;
using YourPlace.Models.Models;
using YourPlace.Web.Models.Store;

namespace YourPlace.Web.Controllers
{
    [Authorize]
    public class StoreController : Controller
    {
        private readonly ApplicationDbContext data;
        private readonly UserManager<User> userManager;

        public StoreController(ApplicationDbContext data, UserManager<User> userManager)
        {
            this.data = data;
            this.userManager = userManager;
        }


        public IActionResult Create()
        {
            var user = this.GetCurrentUser();

            var userStoreId = user.StoreId;

            if(userStoreId != null)
            {
                return this.RedirectToAction("MyStore");
            }

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

            Town town;

            if(!this.data.Towns.Any(t => t.Name == model.Town))
            {
                town = new Town
                {
                    Id = Guid.NewGuid().ToString(),
                    Name = model.Town
                };
            }
            else
            {
                town = this.data.Towns.Where(t => t.Name == model.Town).FirstOrDefault();
            }

            District district;
            //TODO check in DB for existing districts and town names
            if(!data.Districts.Any(d => d.Name == model.District))
            {
                district = new District
                {
                    Id = Guid.NewGuid().ToString(),
                    Name = model.District
                };
            }
            else
            {
                district = this.data.Districts.Where(d => d.Name == model.District).FirstOrDefault();
            }

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
                District = district,
            };

            var user = GetCurrentUser();

            user.Store = store;

            this.data.Stores.Add(store);
            this.data.SaveChanges();


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
            var user = GetCurrentUser();

            var userStoreId = user.StoreId;

            var store = this.data.Stores
                .Where(s => s.Id == userStoreId)
                .Select(s => new ListStoreViewModel
                {
                    Name = s.Name,
                    Description = s.Description,
                    Type = s.Type,
                    OpenHour = s.OpenHour,
                    CloseHour = s.CloseHour,
                    District = s.District.Name,
                    PictureUrl = s.PictureUrl,
                    Town = s.Town.Name,
                    Id = s.Id
                })
                .FirstOrDefault();

            var storeServices = this.data.StoreServices.Where(st => st.StoreId == store.Id).ToList();

            foreach (var storeService in storeServices)
            {
                store.StoreServices.Add(storeService);
            }

            return View(store);
        }

        private User GetCurrentUser()
        {
            var userId = GetCurrentUserId();

            return this.data.Users.Where(u => u.Id == userId).FirstOrDefault();
        }

        private string GetCurrentUserId()
        {
            return User.FindFirstValue(ClaimTypes.NameIdentifier);
        }
    }
}
