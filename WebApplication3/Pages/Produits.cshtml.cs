using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using WebApplication3.Models;
using WebApplication3.Helpers;
using System.Diagnostics;

public class ProduitsModel : PageModel
{
    private readonly ApplicationDbContext _context;

    public long TempsChargementMs { get; set; }


    public List<Produit> Produits { get; set; }

    // Propriété publique pour le compteur
    public int CartCount { get; set; }

    public ProduitsModel(ApplicationDbContext context)
    {
        _context = context;
    }

    [BindProperty(SupportsGet = true)]
    public string? SearchQuery { get; set; }
    public void OnGet()
    {
        //var stopwatch = Stopwatch.StartNew(); // Début mesure

        var query = _context.Produits.AsQueryable();

        if (!string.IsNullOrEmpty(SearchQuery))
        {
            query = query.Where(p => p.Nom.Contains(SearchQuery));
        }

        Produits = query.ToList(); // Requête Entity Framework
        LoadCartCount();

        //stopwatch.Stop(); // Fin mesure
        //TempsChargementMs = stopwatch.ElapsedMilliseconds;
    }


    public IActionResult OnPostAddToCart(int id, int quantite = 1)
    {
        var produit = _context.Produits.FirstOrDefault(p => p.Id == id);

        if (produit == null || produit.Quantite <= 0)
            return RedirectToPage();

        var panier = HttpContext.Session.GetObject<List<PanierItem>>("PANIER") ?? new List<PanierItem>();

        var item = panier.FirstOrDefault(p => p.ProduitId == id);

        if (item != null)
        {
            // Ajouter seulement si la quantité totale ne dépasse pas le stock
            int nouvelleQuantite = item.Quantite + quantite;
            item.Quantite = nouvelleQuantite > produit.Quantite ? produit.Quantite : nouvelleQuantite;
        }
        else
        {
            panier.Add(new PanierItem
            {
                ProduitId = produit.Id,
                Nom = produit.Nom,
                Prix = produit.Prix,
                Image = produit.Image,
                Quantite = quantite > produit.Quantite ? produit.Quantite : quantite
            });
        }

        HttpContext.Session.SetObject("PANIER", panier);
        HttpContext.Session.SetInt32("CartCount", panier.Sum(p => p.Quantite));

        return RedirectToPage();
    }

    private void LoadCartCount()
    {
        CartCount = HttpContext.Session.GetInt32("CartCount") ?? 0;
    }
}

