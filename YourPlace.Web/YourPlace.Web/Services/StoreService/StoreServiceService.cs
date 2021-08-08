using AutoMapper;
using AutoMapper.QueryableExtensions;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Threading.Tasks;
using YourPlace.Data.Data;
using YourPlace.Models.Models;
using YourPlace.Web.Models.StoreService;
using YourPlace.Web.Services.User;

namespace YourPlace.Web.Services.StoreService
{
    public class StoreServiceService : IStoreServiceService
    {
        private readonly ApplicationDbContext data;
        private readonly IUserService userService;
        private readonly IConfigurationProvider mapper;

        public StoreServiceService(ApplicationDbContext data,
            IUserService userService, 
            IMapper mapper)
        {
            this.data = data;
            this.userService = userService;
            this.mapper = mapper.ConfigurationProvider;
        }

        public string BookHour(int hour, 
            string storeName, 
            string storeServiceName, 
            string storeServiceId, 
            string storeId, 
            DateTime currDate, 
            YourPlace.Models.Models.User user)
        {
            var hourToBook = new BookedHour
            {
                Id = Guid.NewGuid().ToString(),
                StartFrom = hour,
                StoreName = storeName,
                StoreServiceName = storeServiceName,
                StoreServiceId = storeServiceId,
                StoreId = storeId,
                Date = currDate
            };

            var storeService = this.data.StoreServices
                .Where(st => st.Id == storeServiceId)
                .FirstOrDefault();

            storeService.bookedHours.Add(hourToBook);

            user.BookedHours.Add(hourToBook);

            this.data.SaveChanges();

            return hourToBook.Id;
        }

        public bool Create(string name, string description, decimal price, string storeId, string userId)
        {
            if(!this.userService.isUserOwner(userId, storeId))
            {
                return false;
            }

            var store = this.data.Stores.Where(s => s.Id == storeId).FirstOrDefault();

            if (store == null)
            {
                return false;
            }

            var storeService = new StoreServices
            {
                Id = Guid.NewGuid().ToString(),
                Name = name,
                Description = description,
                Price = price,
                Store = store,
            };

            store.StoreServices.Add(storeService);

            this.data.StoreServices.Add(storeService);
            this.data.SaveChanges();

            return true;
        }

        public bool Delete(string id, string userId)
        {

            var storeService = this.data.StoreServices.Where(st => st.Id == id).FirstOrDefault();

            if(storeService == null)
            {
                return false;
            }

            var store = this.data.Stores.Where(s => s.StoreServices.Any(st => st.Id == id)).FirstOrDefault();

            if (store == null)
            {
                return false;
            }

            if (!this.userService.isUserOwner(userId, store.Id))
            {
                return false;
            }

            store.StoreServices.Remove(storeService);

            this.data.StoreServices.Remove(storeService);

            this.data.SaveChanges();

            return true;
        }

        public bool Edit(string name, string description, decimal price, string id, string userId)
        {
            if (!this.data.StoreServices.Any(st => st.Id == id))
            {
                return false;
            }

            var storeService = this.data.StoreServices.Where(st => st.Id == id).FirstOrDefault();

            if (!this.userService.isUserOwner(userId, storeService.StoreId))
            {
                return false;
            }

            storeService.Name = name;
            storeService.Description = description;
            storeService.Price = price;

            this.data.SaveChanges();

            return true;
        }

        public BookAnHourViewModel FreeHours(string storeServiceId, DateTime currDate)
        {
            var storeService = this.data.StoreServices
                .Where(st => st.Id == storeServiceId)
                .FirstOrDefault();

            var bookedHours = this.data.BookedHours
                .Where(bh => bh.StoreServiceId == storeServiceId && bh.Date == currDate)
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

            return new BookAnHourViewModel
            {
                ShopName = store.Name,
                Price = storeService.Price,
                StoreServiceName = storeService.Name,
                FreeHours = freeHours,
                StoreServiceId = storeService.Id,
                StoreId = store.Id,
                CurrDate = currDate
            };
        }

        public DateTime ParseDate(string date)
        {
            DateTime currDate;

            if (date == null)
            {
                currDate = DateTime.UtcNow;
            }
            else
            {
                if (!DateTime.TryParseExact(date, "MM/dd/yyyy", CultureInfo.InvariantCulture, DateTimeStyles.None, out currDate))
                {
                    currDate = DateTime.UtcNow;
                }
            }

            return currDate;
        }

        DetailsStoreServiceViewModel IStoreServiceService.ServiceById(string id)
        {
            var service = this.data.StoreServices.Where(st => st.Id == id)
                .ProjectTo<DetailsStoreServiceViewModel>(this.mapper)
                .FirstOrDefault();

            return service;
        }
    }
}
