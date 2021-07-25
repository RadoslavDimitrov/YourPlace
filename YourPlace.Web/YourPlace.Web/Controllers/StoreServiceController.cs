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

            var storeService = new StoreService
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
                nameof(StoreService)
            };

            return View("SuccessfullyCreate", message);
        }
    }
}
