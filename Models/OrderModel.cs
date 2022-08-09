using Microsoft.AspNetCore.Identity;

namespace Hotel_Management.Models
{
    public class OrderModel 
    {
        public int Id { get; set; }
        public string dishName { get; set; }
        public string category { get; set; }
        public string description { get; set; }
        public int price { get; set; }
        public string UserId { get; set; }
        public IdentityUser User { get; set; }
        public string SubAdminId { get; set; }
        
    }
}
