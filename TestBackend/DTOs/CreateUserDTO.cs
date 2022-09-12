using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace TestBackend.DTOs
{
    public class CreateUserDTO
    {
        [Required]
        public string Username { get; set; }
        [Required(ErrorMessage = "La contraseña es requerida")]
        [PasswordPropertyText]
        public string Password { get; set; }
        [Required]
        public string Name { get; set; }
        public string Lastname { get; set; }
        [Required]
        [EmailAddress]
        public string Email { get; set; }
        public int Active { get; set; }
        [Required]
        public string Role { get; set; }        
        public string? CreatedBy { get; set; }
        public DateTime? CreatedDate { get; set; }
    }
}
