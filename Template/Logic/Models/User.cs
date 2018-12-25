using System.ComponentModel.DataAnnotations;

namespace Logic.Models
{
    public class User : Entity
    {
        [MaxLength(150)]
        public string FirstName { get; set; }
        
        [MaxLength(150)]
        public string LastName { get; set; }
        
        [MaxLength(150)]
        public string Username { get; set; }

        [Required]
        [MaxLength(100)]
        public string Email { get; set; }

        [Required]
        public byte[] PasswordHash { get; set; }

        [Required]
        public byte[] PasswordSalt { get; set; }
    }
}
