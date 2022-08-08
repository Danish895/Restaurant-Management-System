using Microsoft.AspNetCore.Identity;

namespace Hotel_Management.Models
{
    public class SessionModel
    {
        public int Id { get; set; }
        public DateTime createdAt { get; set; }
        public DateTime currentTime { get; set; } = DateTime.UtcNow;

        public string UserId { get; set; }
        public IdentityUser User { get; set; }

    }
}
