namespace WebApplication3.Models
{
    public class PanierItem
    {
        public int ProduitId { get; set; }
        public string Nom { get; set; }
        public decimal Prix { get; set; }
        public string Image { get; set; }
        public int Quantite { get; set; }

        public int StockDisponible { get; set; }
    }
}
