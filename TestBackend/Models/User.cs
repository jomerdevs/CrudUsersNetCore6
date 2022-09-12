using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace TestBackend.Models
{
    public partial class User
    {
        [Key]
        public int Id { get; set; }
        [Required]
        public string Username { get; set; } = null!;
        [Required]
        public string Password { get; set; } = null!;
        [Required]
        public string Name { get; set; } = null!;
        public string Lastname { get; set; } = null!;
        [Required]
        public string Email { get; set; } = null!;
        public int Active { get; set; }
        public string Role { get; set; } = null!;
        public string? CreatedBy { get; set; }
        public DateTime? CreatedDate { get; set; }
    }
}
