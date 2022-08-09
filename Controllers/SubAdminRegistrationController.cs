using Hotel_Management.Configurations;
using Hotel_Management.Database;
using Hotel_Management.Models;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;

namespace Hotel_Management.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    public class SubAdminRegistrationController : ControllerBase
    {
        private readonly ApplicationDbContext _db;
        private readonly IConfiguration _configuration;
        private readonly UserManager<IdentityUser> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly ILogger<SubAdminRegistrationController> _logger;

        public SubAdminRegistrationController(ApplicationDbContext db, RoleManager<IdentityRole> roleManager, ILogger<SubAdminRegistrationController> logger, UserManager<IdentityUser> userManager, IConfiguration configuration)
        {
            _logger = logger;
            _roleManager = roleManager;
            _db = db;
            _userManager = userManager;
            _configuration = configuration;
        }

        [HttpPost]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> SubAdminRegistration(UserModel user)
        {
            //var roleResult = await _roleManager.CreateAsync(new IdentityRole(department));
            var SubAdmins = await SubAdminRegister(user);
            return Ok(new { SubAdmins });
        }
        private async Task<IActionResult> SubAdminRegister(UserModel user)
        {
            if (ModelState.IsValid)
            {
                var existingUser = await _userManager.FindByEmailAsync(user.email);

                if (existingUser != null)
                {
                    return BadRequest(new
                    {
                        Errors = new List<string>() {
                                "Email already in use"
                            },
                        Success = false
                    });
                }

                var newUser = new IdentityUser() { Email = user.email, UserName = user.email };
                var isCreated = await _userManager.CreateAsync(newUser, user.password);
                if (isCreated.Succeeded)
                {
                    await _userManager.AddToRoleAsync(newUser, "SubAdmin");

                    var jwtToken = CreateToken(newUser);

                    return Ok(new
                    {
                        Success = true,
                        Token = jwtToken
                    });
                }
                else
                {
                    return BadRequest(new
                    {
                        Errors = isCreated.Errors.Select(x => x.Description).ToList(),
                        Success = false
                    });
                }
            }
            return BadRequest(new
            {
                Errors = new List<string>() {
                        "Invalid payload"
                    },
                Success = false
            });
        }

        private async Task<AuthResult> CreateToken(IdentityUser user)
        {
            var claims = await GetAllValidClaims(user);
            //List<Claim> claims = new List<Claim>
            //{
            //    new Claim("email", user.Email),
            //    new Claim("id", user.Id)

            //};
            Console.WriteLine(claims);
            var key = new SymmetricSecurityKey(System.Text.Encoding.UTF8.GetBytes(
                _configuration.GetSection("Jwt:Secret-Key").Value));

            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha512Signature);

            var token = new JwtSecurityToken(
                claims: claims,
                expires: DateTime.Now.AddDays(1),
                signingCredentials: creds);

            var jwt = new JwtSecurityTokenHandler().WriteToken(token);

            return new AuthResult()
            {
                Token = jwt,
                Success = true,
            };
        }

        private async Task<List<Claim>> GetAllValidClaims(IdentityUser user)
        {
            var claims = new List<Claim>
            {
                new Claim("email", user.Email),
                new Claim("id", user.Id)
            };

            // Getting the claims that we have assigned to the user
            var userClaims = await _userManager.GetClaimsAsync(user);
            claims.AddRange(userClaims);

            // Get the user role and add it to the claims
            var userRoles = await _userManager.GetRolesAsync(user);

            foreach (var userRole in userRoles)
            {
                var role = await _roleManager.FindByNameAsync(userRole);

                if (role != null)
                {
                    // Adding roles to the user
                    claims.Add(new Claim(ClaimTypes.Role, userRole));

                    var roleClaims = await _roleManager.GetClaimsAsync(role);
                    foreach (var roleClaim in roleClaims)
                    {
                        claims.Add(roleClaim);
                    }
                }
            }
            return claims;
        }
    }
}
