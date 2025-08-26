using KEPSAS.TTS.Data;
using KEPSAS.TTS.Models; // ApplicationUser
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// ---------------- DB ----------------
builder.Services.AddDbContext<ApplicationDbContext>(opt =>
    opt.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

// -------------- Identity ------------
builder.Services.AddIdentity<ApplicationUser, IdentityRole>()
    .AddEntityFrameworkStores<ApplicationDbContext>()
    .AddDefaultTokenProviders();

// Cookie yönlendirmeleri
builder.Services.ConfigureApplicationCookie(opt =>
{
    opt.LoginPath = "/Account/Login";
    opt.AccessDeniedPath = "/Account/Denied";
    opt.SlidingExpiration = true;
    opt.ExpireTimeSpan = TimeSpan.FromHours(8);
});

builder.Services.AddControllersWithViews();

var app = builder.Build();

// ------- Ortam / güvenlik / static ------
if (!app.Environment.IsDevelopment())
{
    app.UseHsts();
}
app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();
app.UseAuthentication();
app.UseAuthorization();

// ------- Otomatik migrate + seed --------
using (var scope = app.Services.CreateScope())
{
    var sp = scope.ServiceProvider;
    try
    {
        var db = sp.GetRequiredService<ApplicationDbContext>();
        // Tüm bekleyen migration’ları uygula (tabloyu oluşturur)
        await db.Database.MigrateAsync();

        // Admin ve roller
        await SeedData.RunAsync(sp);
    }
    catch (Exception ex)
    {
        // burada bir logger varsa kullan, yoksa Console’a yaz
        Console.Error.WriteLine($"Startup migration/seed error: {ex}");
        throw; // kritikse patlatalım ki fark edilsin
    }
}

// ------- Rotalar ------------------------
app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();
