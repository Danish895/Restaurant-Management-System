using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations;

namespace Hotel_Management.Models
{
    public class DishModel
    {
        [Key]
        public int Id { get; set; }
        public string dishName { get; set; }
        public string category { get; set; }
        public string description { get; set; }
        public int price { get; set; }
        public DateTime createdAt { get; set; }
        public DateTime updatedAt { get; set; }
        public bool isDeleted { get; set; } = false;

        public string UserId { get; set; }
        public IdentityUser User { get; set; }

    }
}
