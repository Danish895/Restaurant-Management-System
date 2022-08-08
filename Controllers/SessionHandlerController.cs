using Hotel_Management.Database;
using Hotel_Management.Models;
using Hotel_Management.Service;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace Hotel_Management.Controllers
{
    public class SessionHandlerController : ControllerBase
    {
        private readonly ApplicationDbContext _db;
        private readonly IUserService _userService;
        private readonly UserManager<IdentityUser> _userManager;
        

        private readonly ILogger<SessionHandlerController> _logger;

        public SessionHandlerController(ILogger<SessionHandlerController> logger, ApplicationDbContext db, IUserService userService, UserManager<IdentityUser> userManager)
        {
            _logger = logger;
            _db = db;
            _userService = userService;
            _userManager = userManager;
        }

        public async Task<IActionResult> mySession([FromBody] SessionModel newSession)
        {
            string UserId = _userService.Id();
            IdentityUser user = await _userManager.FindByIdAsync(UserId);
            SessionModel session = new SessionModel() { createdAt = newSession.createdAt, currentTime = newSession.currentTime, UserId = UserId, User = user };
            await _db.SessionModels.AddAsync(session);
            await _db.SaveChangesAsync();
            return CreatedAtAction("Get", session.Id, session);
        }
    }
}
