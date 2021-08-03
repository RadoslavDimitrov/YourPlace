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
using YourPlace.Web.Services.Store;
using YourPlace.Web.Services.Store.Models;

namespace YourPlace.Web.Controllers
{
    [Authorize]
    public class StoreController : Controller
    {
        private readonly ApplicationDbContext data;
        private readonly UserManager<User> userManager;
        private readonly IStoreService storeService;

        public StoreController(ApplicationDbContext data, UserManager<User> userManager, IStoreService storeService)
        {
            this.data = data;
            this.userManager = userManager;
            this.storeService = storeService;
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
            var userStoreId = GetCurrentUserStoreId();

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
                    Raiting = s.Raitings.Select(r => r.StoreRaiting).Average()
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
                    Raiting = s.Raitings.Select(r => r.StoreRaiting).Average()
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

            var storeComments = this.data.Comments.Where(c => c.StoreId == store.Id).ToList();

            foreach (var storeComment in storeComments)
            {
                store.Comments.Add(storeComment);
            }

            return this.View(store);
        }

        public IActionResult MyStoreBookedHours()
        {
            var userStoreId = GetCurrentUserStoreId();

            var bookedHours = this.data.BookedHours
                .Where(b => b.StoreId == userStoreId)
                .Select(b => new ListStoreBookedHoursViewModel() 
                {
                    Id = b.Id,
                    Date = b.Date,
                    StartFrom = b.StartFrom,
                    StoreId = b.StoreId,
                    StoreName = b.StoreName,
                    StoreServiceId = b.StoreServiceId,
                    StoreServiceName = b.StoreServiceName,
                    UserId = b.UserId,
                    Username = b.User.UserName
                })
                .ToList();

            return this.View(bookedHours);
        }

        //TODO place it in UserService
        private string GetCurrentUserStoreId()
        {
            var user = this.GetCurrentUser();

            return user.StoreId;
        }
        private User GetCurrentUser()
        {
            var userId = this.User.GetId();

            return this.data.Users.Where(u => u.Id == userId).FirstOrDefault();
        }

    }
}
