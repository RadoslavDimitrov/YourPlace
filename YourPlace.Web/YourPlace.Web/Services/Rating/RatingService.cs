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

            if(this.data.Raitings.Any(r => r.StoreId == storeId && r.UserId == userId))
            {
                return true;
            }

            return false;
        }

        public void Rate(int rate, string storeId, string userId)
        {
            var store = this.Store(storeId);

            store.Raitings.Add(new YourPlace.Models.Models.Rating()
            {
                Id = Guid.NewGuid().ToString(),
                StoreId = store.Id,
                StoreRaiting = rate,
                UserId = userId
            });

            this.data.SaveChanges();
        }

        public YourPlace.Models.Models.Store Store(string storeId)
        {
            return this.data.Stores.Where(s => s.Id == storeId).FirstOrDefault();
        }

    }
}
