using Hotel_Management.Database;
using Hotel_Management.DTO;
using Hotel_Management.Models;
using Hotel_Management.Service;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Hotel_Management.Controllers
{
    [ApiController]
    [Route("api/[controller]")]

    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]

    public class DishController : ControllerBase
    {
        private readonly ApplicationDbContext _db;
        private readonly IUserService _userService;
        private readonly UserManager<IdentityUser> _userManager;
        //private readonly RoleManager<IdentityUser> _roleManager;


        public DishController(
                ApplicationDbContext db,
                IUserService userService,
                UserManager<IdentityUser> userManager
                //RoleManager<IdentityUser> roleManager
            )
        {
            _db = db;
            _userService = userService;
            _userManager = userManager;
            //_roleManager = roleManager;
        }

        [HttpGet]
        [Authorize(Roles = "Admin,SubAdmin, User")]
        public async Task<IActionResult> Get()
        {
            //string userId = _userService.Id();
            List<DishModel> dishes = await _db.DishModels.ToListAsync();
            return Ok(new { dishes });
        }

        [HttpPost]
        [Authorize(Roles = "Admin,SubAdmin")]
        
        public async Task<IActionResult> Post([FromBody] DishDto item)
        {
            string UserId = _userService.Id();
            IdentityUser user = await _userManager.FindByIdAsync(UserId);
            DishModel dishes = new DishModel() { dishName = item.dishName, description = item.description,category= item.category,price = item.price, UserId = UserId, User = user};
            await _db.DishModels.AddAsync(dishes);
            await _db.SaveChangesAsync();
            return CreatedAtAction("Get", dishes.Id , dishes);
        }

        [HttpGet("{id}")]
        [Authorize(Roles = "Admin, SubAdmin, User")]
        public async Task<IActionResult> Get(int id)
        {
            string userId = _userService.Id();
            DishModel dish = await _db.DishModels.Where(t => t.Id == id).FirstOrDefaultAsync();
            if (dish == null)
            {
                return BadRequest(new
                {
                    Errors = new List<string>() {
                        "Dish with this ID does not exist, Enter valid ID"
                    },
                    Success = false
                });
            }
            return Ok(dish);
        }

        [HttpPut("{id}")]
        [Authorize(Roles = "Admin,SubAdmin")]
        public async Task<IActionResult> Put(int id, [FromBody] DishDto item)
        {
            string userId = _userService.Id();
            IdentityUser user = await _userManager.FindByIdAsync(userId);
            DishModel dishes = await _db.DishModels.Where(t => t.UserId== userId && t.Id ==id).FirstOrDefaultAsync();
            if(dishes == null)
            {
                return BadRequest(new
                {
                    Errors = new List<string>() {
                        "No such dish with this ID is present "
                    },
                    Successful = false
                });
            }
            if (item.dishName != null) { dishes.dishName = item.dishName.Trim(); }
            if (item.category != null) { dishes.category = item.category.Trim(); }
            if (item.description != null) { dishes.description = item.description.Trim(); }
            if (item.price != null) { dishes.price = item.price; }
            await _db.SaveChangesAsync();
            return Ok("updated successfully");
        }

        [HttpDelete("{id}")]
        [Authorize(Roles = "Admin,SubAdmin")]
        public async Task <IActionResult> delete(int id)
        {
            string userId = _userService.Id();
            
            DishModel dishes = await _db.DishModels.Where(t => t.UserId == userId && t.Id == id).FirstOrDefaultAsync();
            
            if (dishes != null)
            {
                _db.Remove(dishes);
                await _db.SaveChangesAsync();
                return Ok(new { msg = "removed" });
            }
            return BadRequest("bad request");
        }

        //[HttpPost("OrderYourDish")]
        //[Authorize(Roles = "User")]
        //public async Task<IActionResult> PostOrder([FromBody] DishDto item)
        //{
        //    string UserId = _userService.Id();
        //    IdentityUser user = await _userManager.FindByIdAsync(UserId);

        //    if (await _db.DishModels.FirstOrDefaultAsync(t => t.dishName == item.dishName && t.price == item.price) != null)
        //    {
        //        OrderModel OrderedDishes = new OrderModel()
        //        {
        //            dishName = item.dishName,
        //            price = item.price,
        //            UserId = UserId,
        //            User = user
        //        };
        //        await _db.OrderModels.AddAsync(OrderedDishes);
        //        await _db.SaveChangesAsync();
        //        return CreatedAtAction("Get", OrderedDishes);
        //    }
        //    return BadRequest(new
        //    {
        //        Errors = "Please enter valid order"
        //    });
        //}
        [HttpPost("OrderYourDish")]
        [Authorize(Roles = "User")]
        public async Task<IActionResult> PostOrder([FromBody] OrderDto[] orderId)
        {
            string UserId = _userService.Id();
            IdentityUser user = await _userManager.FindByIdAsync(UserId);
            foreach (var item in orderId)
            {
                DishModel dish = await _db.DishModels.FirstOrDefaultAsync(t => t.Id == item.myOrderId);
                if (dish != null)
                {
                    OrderModel OrderedDishes = new OrderModel()
                    {
                        dishName = dish.dishName,
                        price = dish.price,
                        category = dish.category,
                        description = dish.description,
                        SubAdminId = dish.UserId,
                        UserId = UserId,
                        User = user
                    };
                    await _db.OrderModels.AddAsync(OrderedDishes);
                    await _db.SaveChangesAsync();
                }
            }
            return Ok(new { msg = "Your dish is ordered successfully" });
        }

        [HttpGet("MyOrders")]
        [Authorize(Roles = "User")]
        public async Task<IActionResult> GetOrdrs()
        {
            string UserId = _userService.Id();
            IdentityUser user = await _userManager.FindByIdAsync(UserId);
            List<OrderModel> orders = await _db.OrderModels.Where(t => t.UserId == UserId).ToListAsync();
            return Ok(new { orders });
        }

        [HttpDelete("deleteMyOrder")]
        [Authorize(Roles = "User")]
        public async Task<IActionResult> deleteMyOrder([FromBody] OrderDto[] orderId)
        {
            string UserId = _userService.Id();
            IdentityUser user = await _userManager.FindByIdAsync(UserId);
            foreach (var item in orderId)
            {
                OrderModel dish = await _db.OrderModels.FirstOrDefaultAsync(t => t.Id == item.myOrderId);

                if (dish != null)
                {
                    _db.OrderModels.Remove(dish);
                    await _db.SaveChangesAsync();
                    //return Ok(new { msg = "Your dish is deleted successfully" });
                }
            }
            return Ok(new { msg = "Your dish is deleted successfully" });
        }
    }
}
