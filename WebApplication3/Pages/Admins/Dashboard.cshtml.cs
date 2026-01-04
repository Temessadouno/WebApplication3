using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Http;
using WebApplication3.Models;

namespace WebApplication3.Pages.Admins;
//Classe de gestion de la page de Tableau de bord
public class DashboardModel : PageModel
{
    private readonly ApplicationDbContext _context;

    public List<performanceLog> Logs { get; set; } = new List<performanceLog>();

    public DashboardModel(ApplicationDbContext context)
    {
        _context = context;
    }

    public void OnGet()
    {
        // Protection admin
        if (HttpContext.Session.GetInt32("AdminId") == null)
        {
            Response.Redirect("/Admins/Login");
            return;
        }

        // Charger les performances
        Logs = _context.PerformanceLogs
                       .OrderByDescending(p => p.Date)
                       .Take(50)
                       .ToList();
    }
}
