using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

using YourPlace.Data.Data;
using YourPlace.Models.Models;
using YourPlace.Web.Infrastructure;
using YourPlace.Web.Models.Store;
using YourPlace.Web.Models.StoreService;

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
                    Id = s.Id,
                    Raiting = s.Raitings.Select(r => r.StoreRaiting).Sum()
                })
                .FirstOrDefault();

            if(store != null)
            {
                var storeServices = this.data.StoreServices.Where(st => st.StoreId == store.Id).ToList();

                foreach (var storeService in storeServices)
                {
                    store.StoreServices.Add(storeService);
                }
            }
            

            return View(store);
        }

        public IActionResult All()
        {
            List<StoreViewModel> stores = this.data.Stores
                .Select(s => new StoreViewModel
                {
                    Id = s.Id,
                    Name = s.Name,
                    Type = s.Type,
                    Description = s.Description,
                    PictureUrl = s.PictureUrl,
                    OpenHour = s.OpenHour,
                    CloseHour = s.CloseHour,
                    Town = s.Town.Name,
                    District = s.District.Name,
                    Raiting = s.Raitings.Select(r => r.StoreRaiting).Sum()
                })
                .ToList();

            return this.View(stores);
        }

        public IActionResult Visit(string storeId)
        {
            var store = this.data.Stores
                .Where(s => s.Id == storeId)
                .Select(s => new ListStoreViewModel
                {
                    Id = s.Id,
                    Name = s.Name,
                    Type = s.Type,
                    Description = s.Description,
                    PictureUrl = s.PictureUrl,
                    OpenHour = s.OpenHour,
                    CloseHour = s.CloseHour,
                    Town = s.Town.Name,
                    District = s.District.Name,
                    Raiting = s.Raitings.Select(r => r.StoreRaiting).Sum()
                })
                .FirstOrDefault();

            if(store == null)
            {
                return RedirectToAction("All");
            }

            var storeServices = this.data.StoreServices.Where(st => st.StoreId == store.Id).ToList();

            foreach (var storeService in storeServices)
            {
                store.StoreServices.Add(storeService);
            }

            return this.View(store);
        }

        //TODO place it in UserService
        private User GetCurrentUser()
        {
            var userId = this.User.GetId();

            return this.data.Users.Where(u => u.Id == userId).FirstOrDefault();
        }

    }
}
