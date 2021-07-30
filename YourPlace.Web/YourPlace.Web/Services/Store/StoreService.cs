using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using YourPlace.Data.Data;
using YourPlace.Web.Models.Store;
using YourPlace.Web.Services.Store.Models;

namespace YourPlace.Web.Services.Store
{
    public class StoreService : IStoreService
    {
        private readonly ApplicationDbContext data;

        public StoreService(ApplicationDbContext data)
        {
            this.data = data;
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
    }
}
