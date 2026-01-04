using Microsoft.EntityFrameworkCore;
using WebApplication3.Middlewares;
using WebApplication3.Models;

var builder = WebApplication.CreateBuilder(args);

// Razor Pages
builder.Services.AddRazorPages();

// DbContext
builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

// Session dans un cookie pour une duré de 30 jours pour stocker le panier
builder.Services.AddSession(options =>
{
    options.IdleTimeout = TimeSpan.FromDays(30); // durée panier
    options.Cookie.Name = ".VenteStore.Session";
    options.Cookie.HttpOnly = true;
    options.Cookie.IsEssential = true;
    options.Cookie.MaxAge = TimeSpan.FromDays(30); //  persistant
});


var app = builder.Build();

//  Initialisation de la TABLE DES PERFORMANCES à chaque chargement par l'administrateur on met les nouvelle performances dans la table des performances
using (var scope = app.Services.CreateScope())
{
    var db = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();

    // 🔥 Effacer toutes les anciennes performances
    db.PerformanceLogs.RemoveRange(db.PerformanceLogs);
    db.SaveChanges();
}


// Pipeline HTTP
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error");
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseMiddleware<PerformanceMiddleware>();

//  Session 
app.UseSession();

app.UseAuthorization();

app.MapRazorPages();

app.Run();
