using Microsoft.AspNetCore.Mvc;
using System.Linq;

using YourPlace.Web.Infrastructure;
using YourPlace.Data.Data;
using YourPlace.Web.Models.StoreService;
using System;
using System.Collections.Generic;
using Microsoft.AspNetCore.Authorization;

using YourPlace.Models.Models;
using YourPlace.Web.Services.StoreService;
using YourPlace.Web.Services.User;

namespace YourPlace.Web.Controllers
{
    [Authorize]
    public class StoreServiceController : Controller
    {
        private string today = DateTime.UtcNow.ToString("MM/dd/yyyy");

        private readonly IStoreServiceService storeServiceService;
        private readonly IUserService userService;

        public StoreServiceController(IStoreServiceService storeServiceService, 
            IUserService userService)
        {
            this.storeServiceService = storeServiceService;
            this.userService = userService;
        }

        public IActionResult Create(string storeId)
        {
            return this.View();
        }

        [HttpPost]
        public IActionResult Create(CreateStoreServiceViewModel model, string storeId)
        {
            var result = this.storeServiceService.Create(model.Name, model.Description, model.Price, storeId, this.User.GetId());

            if (!result)
            {
                return View("NotFound", ApplicationMessages.Exception.StoreDoesNotExist);
            }

            var message = new List<string>
            {
                ApplicationMessages.Succsesfully.CreateStoreService,
                model.Name
            };

            return View("SuccessfullyCreate", message);
        }

        public IActionResult Details(string storeServiceId)
        {
            var storeService = this.storeServiceService.ServiceById(storeServiceId);

            if(storeService == null)
            {
                return this.View(storeServiceId);
            }

            return this.View(storeService);
        }

        public IActionResult Edit(string storeServiceId)
        {
            return this.View();
        }

        [HttpPost]
        public IActionResult Edit(EditStoreServiceViewModel model ,string storeServiceId)
        {
            if (!ModelState.IsValid)
            {
                return this.View(storeServiceId);
            }

            if(!this.storeServiceService.Edit(model.Name, model.Description, model.Price, storeServiceId, this.User.GetId()))
            {
                return this.View(storeServiceId);
            }

            return this.RedirectToAction("MyStore", "Store");
        }

        public IActionResult Delete(string storeServiceId)
        {
            var result = this.storeServiceService.Delete(storeServiceId, this.User.GetId());

            if(!result)
            {
                return View("NotFound", ApplicationMessages.Exception.ServiceDoesNotExist);
            }

            return this.RedirectToAction("MyStore", "Store");
        }

        public IActionResult BookAnHour(string storeServiceId, string date)
        {
            DateTime currDate = this.storeServiceService.ParseDate(date);

            var model = this.storeServiceService.FreeHours(storeServiceId, currDate);

            return this.View(model);
        }

        public IActionResult CreateAnHour(int hour, string storeName, string storeServiceName, string storeServiceId, string storeId, string date)
        {
            DateTime currDate = this.storeServiceService.ParseDate(date);

            var user = this.userService.GetCurrentUser(this.User.GetId());

            var hourId = this.storeServiceService.BookHour(hour, storeName, storeServiceName, storeServiceId, storeId, currDate, user);

            if(hourId == null)
            {
                return this.RedirectToAction("bookAnHour");
            }

            return RedirectToAction("MyBookedHours", "User");
        }

    }
}
