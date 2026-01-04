using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using WebApplication3.Models;

namespace WebApplication3.Pages.Admins
{
    public class gestionProduitsModel : PageModel
    {
        private readonly ApplicationDbContext _context;

        public gestionProduitsModel(ApplicationDbContext context)
        {
            _context = context;
        }

        public List<Produit> Produits { get; set; } = new();

        [BindProperty]
        public Produit Produit { get; set; }

        [BindProperty] //  OBLIGATOIRE
        public IFormFile ImageUpload { get; set; }

        public bool ShowAddModal { get; set; }

        [BindProperty(SupportsGet = true)]
        public string? SearchQuery { get; set; }


        public void OnGet()
        {
            if (HttpContext.Session.GetInt32("AdminId") == null)
            {
                Response.Redirect("/Admins/Login");
                return;
            }
            var query = _context.Produits.AsQueryable();

            if (!string.IsNullOrEmpty(SearchQuery))
            {
                query = query.Where(p => p.Nom.Contains(SearchQuery));
            }

            Produits = query.ToList();

        }

        // ================= AJOUTER =================
        public IActionResult OnPostAjouter()
        {
            ModelState.Remove("Produit.Image");

            if (string.IsNullOrWhiteSpace(Produit.Description))
                Produit.Description = "Aucune description";

            if (ImageUpload == null)
            {
                ModelState.AddModelError("ImageUpload", "Veuillez sélectionner une image.");
            }

            if (!ModelState.IsValid)
            {
                ShowAddModal = true;
                Produits = _context.Produits.ToList();
                return Page();
            }

            string uploadsFolder = Path.Combine(
                Directory.GetCurrentDirectory(), "wwwroot", "Images");

            Directory.CreateDirectory(uploadsFolder);

            string fileName = Guid.NewGuid() + Path.GetExtension(ImageUpload.FileName);
            string filePath = Path.Combine(uploadsFolder, fileName);

            using var stream = new FileStream(filePath, FileMode.Create);
            ImageUpload.CopyTo(stream);

            Produit.Image = fileName;

            _context.Produits.Add(Produit);
            _context.SaveChanges();

            return RedirectToPage();
        }


        // ================= MODIFIER =================
        public IActionResult OnPostModifier()
        {
            var p = _context.Produits.Find(Produit.Id);

            if (p == null) return RedirectToPage();

            p.Nom = Produit.Nom;
            p.Description = Produit.Description;
            p.Prix = Produit.Prix;
            p.Quantite = Produit.Quantite;

            _context.SaveChanges();
            return RedirectToPage();
        }

        // ================= SUPPRIMER =================
        public IActionResult OnPostSupprimer(int id)
        {
            var p = _context.Produits.Find(id);
            if (p != null)
            {
                _context.Produits.Remove(p);
                _context.SaveChanges();
            }
            return RedirectToPage();
        }

        // ================= MODIFIER IMAGE =================
        public IActionResult OnPostModifierImage(IFormFile image, int id)
        {
            if (image == null) return RedirectToPage();

            var p = _context.Produits.Find(id);
            if (p == null) return RedirectToPage();

            string folder = Path.Combine(
                Directory.GetCurrentDirectory(),
                "wwwroot",
                "Images"
            );

            string fileName = Guid.NewGuid() + Path.GetExtension(image.FileName);
            string path = Path.Combine(folder, fileName);

            using var stream = new FileStream(path, FileMode.Create);
            image.CopyTo(stream);

            p.Image = fileName;
            _context.SaveChanges();

            return RedirectToPage();
        }
    }
}
