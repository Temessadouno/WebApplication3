using System.Diagnostics;
using WebApplication3.Models;

namespace WebApplication3.Middlewares;

public class PerformanceMiddleware
{
    //recuperation de la requette exécutée
    private readonly RequestDelegate _next;

    public PerformanceMiddleware(RequestDelegate next)
    {
        _next = next;
    }

    public async Task Invoke(HttpContext context, ApplicationDbContext db)
    {
        var sw = Stopwatch.StartNew();

        await _next(context); // exécution page/action

        sw.Stop();

        var page = context.Request.Path.ToString(); //recuperation du chemin de la requette
        var methode = context.Request.Method;      //recuperation de la methode (GET, POST, etc)

        //  Chercher si la page existe déjà
        var existingLog = db.PerformanceLogs
            .FirstOrDefault(p => p.Page == page && p.Methode == methode);

        if (existingLog != null)
        {
            //  UPDATE
            existingLog.TempsMs = sw.ElapsedMilliseconds;
            existingLog.Date = DateTime.Now;
        }
        else
        {
            //  INSERT (première fois seulement)
            db.PerformanceLogs.Add(new performanceLog
            {
                Page = page,
                Methode = methode,
                TempsMs = sw.ElapsedMilliseconds,
                Date = DateTime.Now
            });
        }

        await db.SaveChangesAsync();
    }
}
