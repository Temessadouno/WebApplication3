using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using WebApplication3.Models;

namespace WebApplication3.Pages.Admins;
//la classe qui gère la page de connexion 
public class LoginModel : PageModel
{
    private readonly ApplicationDbContext  _context;
    public LoginModel(ApplicationDbContext context)
    {
        _context = context;
    }

    [BindProperty]
    public string Login { get; set; }
    [BindProperty]
    public string Password { get; set; }
    public string? ErrorMessage { get; set; }
    public void OnGet()
    {
          
    }

    public IActionResult OnPost()
    {
        if (string.IsNullOrEmpty(Login) || string.IsNullOrEmpty(Password))
        {
            ErrorMessage = " Veuillez remplir tous les champs.";
            return Page();
        }

        var admin = _context.Admins
            .FirstOrDefault(a => a.Login == Login);

        if (admin == null)
        {
            ErrorMessage = " Login ou mot de passe incorrect. ";
            return Page();
        }

        //  COMPARAISON DIRECTE (mot de passe en clair)
        if (admin.Password != Password)
        {
            ErrorMessage = " Login ou mot de passe incorrect. ";
            return Page();
        }

        //  Connexion réussie  Session
        HttpContext.Session.SetInt32("AdminId", admin.Id);
        HttpContext.Session.SetString("AdminLOGIN", admin.Login);

        return RedirectToPage("/Admins/Dashboard");
    }
}
