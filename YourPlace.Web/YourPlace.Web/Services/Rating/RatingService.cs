using System;
using System.Linq;

using YourPlace.Data.Data;
using YourPlace.Models.Models;

namespace YourPlace.Web.Services.Raiting
{
    public class RatingService : IRatingService
    {
        private readonly ApplicationDbContext data;

        public RatingService(ApplicationDbContext data)
        {
            this.data = data;
        }

        public bool IsUserRate(string userId, string storeId)
        {
            //TODO CHeck logic
            if(this.data.Raitings.Any(r => r.StoreId == storeId && r.UserId == userId))
            {
                return false;
            }

            return true;
        }

        public void Rate(int rate, string storeId)
        {
            var store = this.Store(storeId);

            store.Raitings.Add(new YourPlace.Models.Models.Rating()
            {
                Id = Guid.NewGuid().ToString(),
                StoreId = store.Id,
                StoreRaiting = rate
            });

            this.data.SaveChanges();
        }

        public Store Store(string storeId)
        {
            return this.data.Stores.Where(s => s.Id == storeId).FirstOrDefault();
        }
    }
}
