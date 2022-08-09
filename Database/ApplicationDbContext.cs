using Hotel_Management.DTO;
using Hotel_Management.Models;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace Hotel_Management.Database
{
    public class ApplicationDbContext : IdentityDbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options) { }

        //public DbSet<UserModel> UserModels { get; set; }
        public DbSet<DishModel> DishModels { get; set; }
        public DbSet<OrderModel> OrderModels { get; set; }
        
    }
}
