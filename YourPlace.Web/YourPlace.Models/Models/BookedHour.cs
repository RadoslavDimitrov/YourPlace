

using System;
using System.ComponentModel.DataAnnotations;

namespace YourPlace.Models.Models
{
    public class BookedHour
    {
        public BookedHour()
        {
            this.Id = Guid.NewGuid().ToString();
        }

        [Key]
        public string Id { get; set; }

        public string StoreServiceId { get; set; }

        [Required]
        public StoreServices StoreService { get; set; }

        [Required]
        public string StoreServiceName { get; set; }
        public int StartFrom { get; set; }

        [Required]
        public string StoreName { get; set; }

        public DateTime Date { get; set; }

        public string StoreId { get; set; }

        public string UserId { get; set; }

        public User User { get; set; }

    }
}
