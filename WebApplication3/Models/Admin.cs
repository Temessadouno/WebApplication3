using System.ComponentModel.DataAnnotations;

namespace WebApplication3.Models
{
    public class Admin
    {
        public int Id { get; set; }

        [Required]
        [StringLength(100)]
        public string Nom { get; set; }

        [Required]
        [StringLength(50)]
        public string Login { get; set; }

        [Required]
        public string Password { get; set; }

        public string? Photo { get; set; }

        public string Role { get; set; } = "Client";
    }
}
