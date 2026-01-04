using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace WebApplication3.Pages.Admins;

public class LogoutModel : PageModel
{
    public IActionResult OnGet()
    {
        HttpContext.Session.Remove("AdminId");
        HttpContext.Session.Remove("AdminLOGIN");

        return RedirectToPage("/Index");
    }
}
