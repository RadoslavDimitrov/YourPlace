using AutoMapper;
using AutoMapper.QueryableExtensions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using YourPlace.Data.Data;
using YourPlace.Models.Models;
using YourPlace.Web.Models.Store;
using YourPlace.Web.Services.Store.Models;

namespace YourPlace.Web.Services.Store
{
    public class StoreService : IStoreService
    {
        private readonly ApplicationDbContext data;
        private readonly IConfigurationProvider mapper;

        public StoreService(ApplicationDbContext data, 
            IMapper mapper)
        {
            this.data = data;
            this.mapper = mapper.ConfigurationProvider;
        }

        public AllStoreServiceModel All(string searchTerm, string townName, string districtName, int currentPage, int storesPerPage)
        {
            var storesQuery = this.data.Stores.AsQueryable();

            if (!string.IsNullOrWhiteSpace(townName))
            {
                storesQuery = this.data.Stores.Where(s => s.Town.Name.ToLower() == townName.ToLower());
            }

            if (!string.IsNullOrWhiteSpace(searchTerm))
            {
                storesQuery = this.data.Stores.Where(s => s.Name.ToLower().Contains(searchTerm.ToLower()));
            }

            if (!string.IsNullOrWhiteSpace(districtName))
            {
                storesQuery = this.data.Stores.Where(s => s.District.Name.ToLower() == districtName.ToLower());
            }

            if(!string.IsNullOrWhiteSpace(townName) && !string.IsNullOrWhiteSpace(districtName))
            {
                storesQuery = this.data.Stores.Where(s => s.Town.Name.ToLower() == townName 
                && s.District.Name.ToLower() == districtName);
            }

            if (!string.IsNullOrWhiteSpace(townName) && !string.IsNullOrWhiteSpace(searchTerm))
            {
                storesQuery = this.data.Stores.Where(s => s.Town.Name.ToLower() == townName 
                && s.Name.ToLower().Contains(searchTerm.ToLower()));
            }

            if (!string.IsNullOrWhiteSpace(districtName) && !string.IsNullOrWhiteSpace(searchTerm))
            {
                storesQuery = this.data.Stores.Where(s => s.District.Name.ToLower() == districtName 
                && s.Name.ToLower().Contains(searchTerm.ToLower()));
            }

            if(!string.IsNullOrWhiteSpace(searchTerm) && !string.IsNullOrWhiteSpace(townName) 
                && !string.IsNullOrWhiteSpace(districtName))
            {
                storesQuery = this.data.Stores.Where(s => s.District.Name.ToLower() == districtName
                && s.Name.ToLower().Contains(searchTerm.ToLower())
                && s.Town.Name.ToLower() == townName.ToLower());
            }

            var totalStores = storesQuery.Count();

            var stores = GetStore(storesQuery
                .Skip((currentPage - 1) * storesPerPage)
                .Take(storesPerPage));

            return new AllStoreServiceModel()
            {
                TotalStores = totalStores,
                CurrentPage = currentPage,
                StoresPerPage = storesPerPage,
                Stores = stores
            };
        }

        private static List<StoreViewModel> GetStore(IQueryable<YourPlace.Models.Models.Store> storeQuery)
        {
           return storeQuery
                  .Select(s => new StoreViewModel
                  {
                      Id = s.Id,
                      Name = s.Name,
                      Type = s.Type,
                      Description = s.Description,
                      OpenHour = s.OpenHour,
                      CloseHour = s.CloseHour,
                      Town = s.Town.Name,
                      District = s.District.Name,
                      PictureUrl = s.PictureUrl,
                      Raiting = s.Raitings.Select(r => r.StoreRaiting).Average()
                  })
                  .ToList();
        }

        public List<string> AllTownName()
        {
            return this.data.Towns.Select(t => t.Name).Distinct().OrderBy(tName => tName).ToList();
        }

        public List<string> AllDistrictName()
        {
            return this.data.Districts.Select(d => d.Name).Distinct().OrderBy(dName => dName).ToList();
        }

        public Town CreateTown(string name)
        {
            Town town;

            if (!this.data.Towns.Any(t => t.Name == name))
            {
                town = new Town
                {
                    Id = Guid.NewGuid().ToString(),
                    Name = name
                };
            }
            else
            {
                town = this.data.Towns.Where(t => t.Name == name).FirstOrDefault();
            }

            return town;
        }

        public District CreateDistrict(string name)
        {
            District district;

            if (!this.data.Districts.Any(d => d.Name == name))
            {
                district = new District
                {
                    Id = Guid.NewGuid().ToString(),
                    Name = name
                };
            }
            else
            {
                district = this.data.Districts.Where(d => d.Name == name).FirstOrDefault();
            }

            return district;
        }

        public string CreateStore(string name, 
            string type, 
            string description, 
            int openHour, 
            int closeHour, 
            string pictureUrl, 
            string town, 
            string district,
            YourPlace.Models.Models.User user)
        {
            var storeTown = this.CreateTown(town);
            var storeDistrict = this.CreateDistrict(district);

            storeTown.Districts.Add(storeDistrict);

            var store = new YourPlace.Models.Models.Store 
            {
                Id = Guid.NewGuid().ToString(),
                Name = name,
                Type = type,
                Description = description,
                OpenHour = openHour,
                CloseHour = closeHour,
                PictureUrl = pictureUrl,
                Town = storeTown,
                District = storeDistrict,
            };

            user.Store = store;

            this.data.Stores.Add(store);
            this.data.SaveChanges();

            return store.Id;
        }

        public ListStoreViewModel ListStore(string storeId)
        {
            var model = this.data.Stores.Where(store => store.Id == storeId)
                .ProjectTo<ListStoreViewModel>(this.mapper)
                .FirstOrDefault();

            if (model != null)
            {
                var storeServices = this.data.StoreServices.Where(st => st.StoreId == model.Id).ToList();

                foreach (var storeService in storeServices)
                {
                    model.StoreServices.Add(storeService);
                }

                var storeComments = this.data.Comments.Where(c => c.StoreId == model.Id).ToList();

                foreach (var storeComment in storeComments)
                {
                    model.Comments.Add(storeComment);
                }
            }

            return model;
        }

        public StoreBookedHoursViewModel StoreBookedHours(string storeId)
        {
            var bookedHours = this.data.BookedHours
                .Where(b => b.StoreId == storeId)
                .ProjectTo<ListStoreBookedHoursViewModel>(this.mapper)
                .ToList();

            var result = new StoreBookedHoursViewModel();

            foreach (var hour in bookedHours)
            {
                if(hour.Date.Day < DateTime.UtcNow.Day)
                {
                    result.PastDays.Add(hour);
                }
                else if(hour.Date.Day > DateTime.UtcNow.Day)
                {
                    result.CommingDays.Add(hour);
                }
                else
                {
                    result.CurrDay.Add(hour);
                }
            }

            return result;
        }
    }
}
