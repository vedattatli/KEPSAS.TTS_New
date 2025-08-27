using KEPSAS.TTS.Data;
using KEPSAS.TTS.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;

namespace KEPSAS.TTS.Controllers
{
    [Authorize]
    public class TalepController : Controller
    {
        private readonly ApplicationDbContext _db;
        private readonly UserManager<ApplicationUser> _userManager;

        public TalepController(ApplicationDbContext db, UserManager<ApplicationUser> userManager)
        {
            _db = db;
            _userManager = userManager;
        }
        // Raporlar: Kullanıcı kendi tüm geçmiş taleplerini görür (Tamamlananlar dahil)
        [Authorize(Roles = "User")]
        public async Task<IActionResult> Raporlar()
        {
            var meId = _userManager.GetUserId(User);

            // Kendi tüm taleplerini, tamamlananlar da dahil
            var list = await _db.Talepler
                .Include(t => t.OlusturanKullanici)
                .Include(t => t.AtananKullanici)
                .Where(t => t.OlusturanKullaniciId == meId)
                .OrderByDescending(t => t.OlusturmaTarihi)
                .ToListAsync();

            return View(list);
        }
        // Liste: Admin tüm talepleri görür; normal kullanıcı sadece kendi açtıklarını görür
        public async Task<IActionResult> Index(string? q)
        {
            var meId = _userManager.GetUserId(User);

            IQueryable<Talep> baseQ = _db.Talepler
                .Include(t => t.OlusturanKullanici)  // <-- DÜZELTİLDİ
                .Include(t => t.AtananKullanici)     // <-- DÜZELTİLDİ
                .AsNoTracking();

            if (!User.IsInRole("Admin"))
                baseQ = baseQ.Where(t => t.OlusturanKullaniciId == meId);

            if (!string.IsNullOrWhiteSpace(q))
                baseQ = baseQ.Where(t => t.Baslik.Contains(q) || (t.Aciklama ?? "").Contains(q));

            // Tamamlananları normal listeden düş
            baseQ = baseQ.Where(t => (t.Durum ?? "") != "Tamamlandı");

            var list = await baseQ.OrderByDescending(t => t.OlusturmaTarihi).ToListAsync();
            return View(list);
        }

        // Detay: normal kullanıcı sadece kendi talebini görebilir; Admin herkesi
        public async Task<IActionResult> Details(int id)
        {
            var talep = await _db.Talepler
                .Include(t => t.OlusturanKullanici)  // <-- DÜZELTİLDİ
                .Include(t => t.AtananKullanici)     // <-- DÜZELTİLDİ
                .FirstOrDefaultAsync(t => t.Id == id);

            if (talep == null) return NotFound();

            var meId = _userManager.GetUserId(User);
            if (!User.IsInRole("Admin") && talep.OlusturanKullaniciId != meId)
                return Forbid();

            // Sadece admin atama/durum yönetebilir → atanabilir kullanıcılar listesi
            if (User.IsInRole("Admin"))
            {
                ViewBag.AssignableUsers = await _userManager.Users
                    .OrderBy(u => u.UserName)
                    .Select(u => new SelectListItem { Value = u.Id, Text = u.UserName! })
                    .ToListAsync();
            }

            return View(talep);
        }

        [HttpGet]
        public IActionResult Create() => View();

        [HttpPost, ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Talep model)
        {
            if (!ModelState.IsValid) return View(model);

            var meId = _userManager.GetUserId(User);
            model.OlusturanKullaniciId = meId;
            model.OlusturmaTarihi = DateTime.UtcNow;
            model.SonIslemTarihi = DateTime.UtcNow;
            model.Durum = "Yeni";
            model.AtananKullaniciId = null;   // kullanıcı atama yapamaz

            _db.Talepler.Add(model);
            await _db.SaveChangesAsync();

            TempData["SuccessMessage"] = "Talebiniz oluşturuldu.";
            return RedirectToAction(nameof(Index));
        }

        [Authorize(Roles = "Admin")]
        [HttpPost, ValidateAntiForgeryToken]
        public async Task<IActionResult> Assign(int id, string? userId)
        {
            var talep = await _db.Talepler.FindAsync(id);
            if (talep == null) return NotFound();

            talep.AtananKullaniciId = string.IsNullOrWhiteSpace(userId) ? null : userId;
            talep.Durum = string.IsNullOrWhiteSpace(userId) ? "Yeni" : "Üzerimde";
            talep.SonIslemTarihi = DateTime.UtcNow;

            await _db.SaveChangesAsync();
            TempData["SuccessMessage"] = "Atama güncellendi.";
            return RedirectToAction(nameof(Details), new { id });
        }

        [Authorize(Roles = "Admin")]
        [HttpPost, ValidateAntiForgeryToken]
        public async Task<IActionResult> SetStatus(int id, string durum)
        {
            var talep = await _db.Talepler.FindAsync(id);
            if (talep == null) return NotFound();

            talep.Durum = durum;
            talep.SonIslemTarihi = DateTime.UtcNow;

            await _db.SaveChangesAsync();
            TempData["SuccessMessage"] = "Talep durumu güncellendi.";
            return RedirectToAction(nameof(Details), new { id });
        }
    }
}
