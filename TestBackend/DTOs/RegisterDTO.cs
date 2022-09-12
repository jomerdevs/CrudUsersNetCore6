using System.ComponentModel.DataAnnotations;
using System.ComponentModel;

namespace TestBackend.DTOs
{
    public class RegisterDTO
    {
        [Required]
        public string Username { get; set; }
        
        [Required]
        [PasswordPropertyText]
        public string Password { get; set; }
        
        [PasswordPropertyText]
        [Compare("Password", ErrorMessage = "Las contraseñas deben ser iguales")]
        public string ConfirmPassword { get; set; }
        [Required]
        public string Name { get; set; }
        public string Lastname { get; set; }
        [Required]
        [EmailAddress]
        public string Email { get; set; }
        public int Active { get; set; }        
        public string Role { get; set; }
        public string? CreatedBy { get; set; }
        public DateTime? CreatedDate { get; set; }
    }
}
