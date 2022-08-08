using System.ComponentModel.DataAnnotations;

namespace Hotel_Management.Models
{
    public class UserModel
    {

        public int Id { get; set; }
        [Required]
        public string username { get; set; }
        [Required]
        public string address { get; set; }
        [Required]
        public long phone { get; set; }
        [Required]
        [EmailAddress]
        public string email { get; set; }
        [Required]
        public string password { get; set; }
    }
}
