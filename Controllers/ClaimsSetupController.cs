//using Hotel_Management.Database;
//using Microsoft.AspNetCore.Authorization;
//using Microsoft.AspNetCore.Identity;
//using Microsoft.AspNetCore.Mvc;
//using System.Security.Claims;

//namespace Hotel_Management.Controllers
//{
//    [Authorize(Roles = "Admin")]

//    [Route("api/[controller]")]
//    [ApiController]
//    public class ClaimsSetupController : ControllerBase
//    {
//        private readonly ApplicationDbContext _context;
//        private readonly UserManager<IdentityUser> _userManager;
//        private readonly RoleManager<IdentityRole> _roleManager;
//        private readonly ILogger<ClaimsSetupController> _logger;

//        public ClaimsSetupController(
//            ApplicationDbContext context,
//            UserManager<IdentityUser> userManager,
//            RoleManager<IdentityRole> roleManager,
//            ILogger<ClaimsSetupController> logger
//        )
//        {
//            _logger = logger;
//            _context = context;
//            _userManager = userManager;
//            _roleManager = roleManager;
//        }

//        [HttpGet]
//        [Route("GetAllClaims")]

//        public async Task<IActionResult> GetAllClaims(string email)
//        {
//            // Check if the user exist
//            var user = await _userManager.FindByEmailAsync(email);

//            if (user == null) // User does not exist
//            {
//                _logger.LogInformation($"The user with the {email} does not exist");
//                return BadRequest(new
//                {
//                    error = "User does not exist"
//                });
//            }

//            var userClaims = await _userManager.GetClaimsAsync(user);
//            return Ok(userClaims);
//        }

//        [HttpPost]
//        [Route("AddClaimsToUser")]
//        public async Task<IActionResult> AddClaimsToUser(string email, string claimName, string claimValue)
//        {
//            // Check if the user exist
//            var user = await _userManager.FindByEmailAsync(email);

//            if (user == null) // User does not exist
//            {
//                _logger.LogInformation($"The user with the {email} does not exist");
//                return BadRequest(new
//                {
//                    error = "User does not exist"
//                });
//            }
//            var userClaim = new Claim(claimName, claimValue);
//            var result = await _userManager.AddClaimAsync(user, userClaim);
//            if (result.Succeeded)
//            {
//                return Ok(new
//                {
//                    result = $"User {user.Email} has a claim {claimName} added to them"
//                });
//            }

//            return BadRequest(new
//            {
//                error = $"Unable to add claim {claimName} to the user {user.Email}"
//            });

//        }
//    }
//}
