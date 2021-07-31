using Microsoft.AspNetCore.Mvc;
using System.Linq;

using YourPlace.Web.Infrastructure;
using YourPlace.Data.Data;
using YourPlace.Web.Models.StoreService;
using YourPlace.Models.Models;
using System;
using System.Collections.Generic;

namespace YourPlace.Web.Controllers
{
    public class StoreServiceController : Controller
    {
        private readonly ApplicationDbContext data;

        public StoreServiceController(ApplicationDbContext data)
        {
            this.data = data;
        }

        public IActionResult Create(string storeId)
        {
            return this.View();
        }

        [HttpPost]
        public IActionResult Create(CreateStoreServiceViewModel model, string storeId)
        {

            var store = this.data.Stores.Where(s => s.Id == storeId).FirstOrDefault();

            if(store == null)
            {
                return View("NotFound", ApplicationMessages.Exception.StoreDoesNotExist);
            }

            var storeService = new StoreServices
            {
                Id = Guid.NewGuid().ToString(),
                Name = model.Name,
                Description = model.Description,
                Price = model.Price,
                Store = store
            };

            store.StoreServices.Add(storeService);

            this.data.StoreServices.Add(storeService);
            this.data.SaveChanges();

            var message = new List<string>
            {
                ApplicationMessages.Succsesfully.CreateStoreService,
                nameof(StoreServices)
            };

            return View("SuccessfullyCreate", message);
        }

        public IActionResult Details(string storeServiceId)
        {
            var storeService = this.data.StoreServices.Where(st => st.Id == storeServiceId).FirstOrDefault();

            if(storeService == null)
            {
                return this.View(storeServiceId);
            }

            var dto = new DetailsStoreServiceViewModel()
            {
                Id = storeService.Id,
                Name = storeService.Name,
                Description = storeService.Description,
                Price = storeService.Price
            };

            return this.View(dto);
        }

        public IActionResult Edit(string storeServiceId)
        {
            return this.View();
        }

        [HttpPost]
        public IActionResult Edit(DetailsStoreServiceViewModel model ,string storeServiceId)
        {
            if (!ModelState.IsValid)
            {
                return this.View(storeServiceId);
            }

            if(!this.data.StoreServices.Any(st => st.Id == storeServiceId))
            {
                return this.View(storeServiceId);
            }

            var storeService = this.data.StoreServices.Where(st => st.Id == storeServiceId).FirstOrDefault();

            storeService.Name = model.Name;
            storeService.Description = model.Description;
            storeService.Price = model.Price;

            this.data.SaveChanges();

            return this.RedirectToAction("MyStore", "Store");
        }

        public IActionResult Delete(string storeServiceId)
        {
            var storeService = this.data.StoreServices.Where(st => st.Id == storeServiceId).FirstOrDefault();

            var store = this.data.Stores.Where(s => s.StoreServices.Any(st => st.Id == storeServiceId)).FirstOrDefault();

            if(store == null)
            {
                return View("NotFound", ApplicationMessages.Exception.StoreDoesNotExist);
            }

            store.StoreServices.Remove(storeService);

            this.data.StoreServices.Remove(storeService);

            this.data.SaveChanges();

            return this.RedirectToAction("MyStore", "Store");
        }

        public IActionResult BookAnHour(string storeServiceId)
        {
            var storeService = this.data.StoreServices
                .Where(st => st.Id == storeServiceId)
                .FirstOrDefault();

            var bookedHours = this.data.BookedHours
                .Where(bh => bh.StoreServiceId == storeServiceId)
                .Select(b => b.StartFrom).ToList();

            var store = this.data.Stores.Where(s => s.Id == storeService.StoreId).FirstOrDefault();

            int storeOpen = store.OpenHour;
            int storeClose = store.CloseHour;

            List<int> freeHours = new List<int>();

            for (int currHour = storeOpen; currHour < storeClose; currHour++)
            {
                if (!bookedHours.Contains(currHour))
                {
                    freeHours.Add(currHour);
                }
            }

            var model = new BookAnHourViewModel
            {
                ShopName = storeService.Store.Name,
                Price = storeService.Price,
                StoreServiceName = storeService.Name,
                FreeHours = freeHours,
                StoreServiceId = storeService.Id,
                StoreId = store.Id
            };

            return this.View(model);
        }

        public IActionResult CreateAnHour(int hour, string storeName, string storeServiceName, string storeServiceId, string storeId)
        {
            var hourToBook = new BookedHour
            {
                Id = Guid.NewGuid().ToString(),
                StartFrom = hour,
                StoreName = storeName,
                StoreServiceName = storeServiceName,
                StoreServiceId = storeServiceId,
                StoreId = storeId
            };

            var storeService = this.data.StoreServices
                .Where(st => st.Id == storeServiceId)
                .FirstOrDefault();

            storeService.bookedHours.Add(hourToBook);

            var user = this.GetCurrentUser();

            user.BookedHours.Add(hourToBook);

            this.data.SaveChanges();

            return RedirectToAction("MyBookedHours", "User");
        }

        private User GetCurrentUser()
        {
            var userId = this.User.GetId();

            return this.data.Users.Where(u => u.Id == userId).FirstOrDefault();
        }
    }
}
