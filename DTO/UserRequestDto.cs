using System.ComponentModel.DataAnnotations;

namespace Hotel_Management.DTO
{
    public class UserRequestDto
    {
        [Required]
        [EmailAddress]
        public string email { get; set; }
        [Required]
        public string password { get; set; }
    }
}
