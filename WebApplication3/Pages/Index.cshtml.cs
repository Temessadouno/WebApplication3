using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Diagnostics;

namespace WebApplication3.Pages;

public class IndexModel : PageModel

{
    public long timer { get; set; }
    public void OnGet()
    {
        var stopwatch = Stopwatch.StartNew(); // Début mesure
        // Simuler un travail
        //System.Threading.Thread.Sleep(100);
        stopwatch.Stop(); // Fin mesure
        timer = stopwatch.ElapsedMilliseconds;

    }
}
