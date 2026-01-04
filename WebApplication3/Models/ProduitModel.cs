using System.ComponentModel.DataAnnotations;

namespace WebApplication3.Models
{
    public class Produit
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "Le nom est obligatoire")]
        public string Nom { get; set; }

        [Required(ErrorMessage = "La description est obligatoire")]
        public string? Description { get; set; }

        [Range(0.01, 999999, ErrorMessage = "Prix invalide")]
        public decimal Prix { get; set; }

        [Range(0, int.MaxValue, ErrorMessage = "Quantité invalide")]
        public int Quantite { get; set; }

        [Required]
        public string Image { get; set; }
    }

}
