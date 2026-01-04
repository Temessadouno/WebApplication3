using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using WebApplication3.Models;
using WebApplication3.Helpers;
using System.Diagnostics;

public class PanierModel : PageModel
{
    private readonly ApplicationDbContext _context;
    public long tempsChargementMs { get; set; }

    public List<PanierItem> Panier { get; set; } = new();

    public PanierModel(ApplicationDbContext context)
    {
        _context = context;
    }

    public void OnGet()
    {
        //var debut = Stopwatch.StartNew(); // Début mesure
        var panierSession =
            HttpContext.Session.GetObject<List<PanierItem>>("PANIER")
            ?? new List<PanierItem>();

        Panier.Clear();

        foreach (var item in panierSession)
        {
            var produit = _context.Produits.FirstOrDefault(p => p.Id == item.ProduitId);
            if (produit == null) continue;

            Panier.Add(new PanierItem
            {
                ProduitId = produit.Id,
                Nom = produit.Nom,
                Image = produit.Image,
                Prix = produit.Prix,
                StockDisponible = produit.Quantite,
                Quantite = Math.Min(item.Quantite, produit.Quantite) //  sécurité
            });
        }

        HttpContext.Session.SetObject("PANIER", Panier);
        HttpContext.Session.SetInt32("CartCount", Panier.Sum(p => p.Quantite));
        //debut.Stop(); // Fin mesure
        //tempsChargementMs = debut.ElapsedMilliseconds;

    }

    public IActionResult OnPost(int ProduitId, int Quantite, string action)
    {
        var panier =
            HttpContext.Session.GetObject<List<PanierItem>>("PANIER")
            ?? new List<PanierItem>();

        //  PRIORITÉ : vider le panier
        if (action == "clear")
        {
            panier.Clear();

            HttpContext.Session.SetObject("PANIER", panier);
            HttpContext.Session.SetInt32("CartCount", 0);

            return RedirectToPage();
        }

        //  Actions qui nécessitent un produit
        var item = panier.FirstOrDefault(p => p.ProduitId == ProduitId);
        if (item == null)
            return RedirectToPage();

        var produit = _context.Produits.FirstOrDefault(p => p.Id == ProduitId);
        if (produit == null)
            return RedirectToPage();

        if (action == "update")
        {
            if (Quantite <= 0)
            {
                panier.Remove(item);
            }
            else
            {
                item.Quantite = Math.Clamp(Quantite, 1, produit.Quantite);
            }
        }
        else if (action == "delete")
        {
            panier.Remove(item);
        }

        HttpContext.Session.SetObject("PANIER", panier);
        HttpContext.Session.SetInt32("CartCount", panier.Sum(p => p.Quantite));

        return RedirectToPage();
    }

}
