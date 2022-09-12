using System.ComponentModel.DataAnnotations;
using TestBackend.Models;

namespace TestBackend.DTOs
{    
    public class UpdateUserDTO
    {        
        public string Username { get; set; }        
        public string Password { get; set; }
        public string Name { get; set; }
        public string Lastname { get; set; }
        [EmailAddress]
        public string Email { get; set; }
        public int Active { get; set; }
        public string Role { get; set; }
        
    }
}
