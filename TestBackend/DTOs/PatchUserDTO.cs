using System.ComponentModel.DataAnnotations;

namespace TestBackend.DTOs
{
    public class PatchUserDTO
    {        
        public string Username { get; set; }
        public string Name { get; set; }
        [EmailAddress]
        public string Email { get; set; }
    }
}
