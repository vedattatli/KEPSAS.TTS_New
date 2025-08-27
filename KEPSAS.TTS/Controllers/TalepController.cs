using KEPSAS.TTS.Data;
using KEPSAS.TTS.Models;
using KEPSAS.TTS.ViewModels; // <-- eklendi
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

        // --- helper: form kaynakları ---
        private async Task<TalepCreateViewModel> BuildCreateVmAsync(TalepCreateViewModel? vm = null)
        {
            vm ??= new TalepCreateViewModel();

            vm.Donanimlar = await _db.Donanimlar
                .OrderBy(d => d.Model)
                .Select(d => new SelectListItem { Value = d.Id.ToString(), Text = $"{d.Kategori} - {d.Model} ({d.SeriNo})" })
                .ToListAsync();

            vm.Kullanicilar = await _userManager.Users
                .OrderBy(u => u.UserName)
                .Select(u => new SelectListItem { Value = u.Id, Text = u.UserName ?? u.Email ?? u.Id })
                .ToListAsync();

            // Basit yazılım listesi: var olan taleplerden distinct + örnek seed
            var existing = await _db.Talepler
                .Select(t => t.YazilimAdi!)
                .Where(s => s != null && s != "")
                .Distinct()
                .OrderBy(s => s)
                .ToListAsync();

            vm.YazilimListe = existing
                .Union(new[] { "Windows", "Office", "Antivirüs", "Muhasebe", "İK", "ERP", "CRM" })
                .Distinct()
                .OrderBy(x => x)
                .ToList();

            return vm;
        }

        // ----------------- CREATE -----------------
        [HttpGet]
        public async Task<IActionResult> Create() => View(await BuildCreateVmAsync());

        [HttpPost, ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(TalepCreateViewModel vm)
        {
            if (!ModelState.IsValid)
                return View(await BuildCreateVmAsync(vm));

            // Tip validasyonu
            if (vm.Tip == TalepTipi.Donanim && vm.DonanimId is null)
                ModelState.AddModelError(nameof(vm.DonanimId), "Donanım seçiniz.");
            if (vm.Tip == TalepTipi.Yazilim && string.IsNullOrWhiteSpace(vm.YazilimAdi))
                ModelState.AddModelError(nameof(vm.YazilimAdi), "Yazılım adı giriniz.");

            if (!ModelState.IsValid)
                return View(await BuildCreateVmAsync(vm));

            var meId = _userManager.GetUserId(User);
            var ip = HttpContext.Connection.RemoteIpAddress?.ToString();

            var talep = new Talep
            {
                Baslik = vm.Baslik.Trim(),
                Aciklama = vm.Aciklama?.Trim(),
                Tip = vm.Tip,
                DonanimId = vm.Tip == TalepTipi.Donanim ? vm.DonanimId : null,
                YazilimAdi = vm.Tip == TalepTipi.Yazilim ? vm.YazilimAdi?.Trim() : null,
                HedefKullaniciId = vm.HedefKullaniciId,
                HedefSicilNo = vm.HedefSicilNo?.Trim(),
                IpAdresi = ip,

                OlusturanKullaniciId = meId,
                OlusturmaTarihi = DateTime.UtcNow,
                SonIslemTarihi = DateTime.UtcNow,
                Durum = "Yeni",
                AtananKullaniciId = null
            };

            _db.Talepler.Add(talep);
            await _db.SaveChangesAsync();

            // Log
            _db.TalepLoglar.Add(new TalepLog
            {
                TalepId = talep.Id,
                Tip = "Olusturma",
                Yeni = $"Tip={talep.Tip}; DonanimId={talep.DonanimId}; Yazilim={talep.YazilimAdi}; Hedef={talep.HedefKullaniciId ?? talep.HedefSicilNo}; IP={ip}",
                KullaniciId = meId
            });
            await _db.SaveChangesAsync();

            TempData["SuccessMessage"] = "Talep oluşturuldu.";
            return RedirectToAction(nameof(Details), new { id = talep.Id });
        }

        // ----------------- LIST / DETAILS -----------------
        public async Task<IActionResult> Index(string? q)
        {
            var meId = _userManager.GetUserId(User);

            IQueryable<Talep> baseQ = _db.Talepler
                .Include(t => t.OlusturanKullanici)
                .Include(t => t.AtananKullanici)
                .Include(t => t.HedefKullanici)
                .Include(t => t.Donanim)
                .AsNoTracking();

            if (!User.IsInRole("Admin"))
                baseQ = baseQ.Where(t => t.OlusturanKullaniciId == meId);

            if (!string.IsNullOrWhiteSpace(q))
                baseQ = baseQ.Where(t => t.Baslik.Contains(q) || (t.Aciklama ?? "").Contains(q));

            baseQ = baseQ.Where(t => (t.Durum ?? "") != "Tamamlandı" && (t.Durum ?? "") != "İptal");

            var list = await baseQ.OrderByDescending(t => t.OlusturmaTarihi).ToListAsync();
            return View(list);
        }

        public async Task<IActionResult> Details(int id)
        {
            var talep = await _db.Talepler
                .Include(t => t.OlusturanKullanici)
                .Include(t => t.AtananKullanici)
                .Include(t => t.HedefKullanici)
                .Include(t => t.Donanim)
                .FirstOrDefaultAsync(t => t.Id == id);

            if (talep == null) return NotFound();

            var meId = _userManager.GetUserId(User);
            if (!User.IsInRole("Admin") && talep.OlusturanKullaniciId != meId)
                return Forbid();

            if (User.IsInRole("Admin"))
            {
                ViewBag.AssignableUsers = await _userManager.Users
                    .OrderBy(u => u.UserName)
                    .Select(u => new SelectListItem { Value = u.Id, Text = u.UserName! })
                    .ToListAsync();
            }

            return View(talep);
        }

        // ----------------- ASSIGN / STATUS -----------------
        [Authorize(Roles = "Admin")]
        [HttpPost, ValidateAntiForgeryToken]
        public async Task<IActionResult> Assign(int id, string? userId, string? note)
        {
            var talep = await _db.Talepler.FindAsync(id);
            if (talep == null) return NotFound();

            var oldAssignee = talep.AtananKullaniciId;

            talep.AtananKullaniciId = string.IsNullOrWhiteSpace(userId) ? null : userId;
            talep.Durum = string.IsNullOrWhiteSpace(userId) ? "Yeni" : "Üzerimde";
            talep.AtamaTarihi = DateTime.UtcNow;
            talep.SonIslemTarihi = DateTime.UtcNow;

            await _db.SaveChangesAsync();

            _db.TalepLoglar.Add(new TalepLog
            {
                TalepId = talep.Id,
                Tip = "Atama",
                Eski = oldAssignee,
                Yeni = talep.AtananKullaniciId,
                Not = note,
                KullaniciId = _userManager.GetUserId(User)
            });
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

            _db.TalepLoglar.Add(new TalepLog
            {
                TalepId = talep.Id,
                Tip = "Durum",
                Eski = null,
                Yeni = durum,
                KullaniciId = _userManager.GetUserId(User)
            });
            await _db.SaveChangesAsync();

            TempData["SuccessMessage"] = "Talep durumu güncellendi.";
            return RedirectToAction(nameof(Details), new { id });
        }

        // İPTAL (nedenli)
        [Authorize(Roles = "Admin")]
        [HttpPost, ValidateAntiForgeryToken]
        public async Task<IActionResult> Cancel(int id, string? reason)
        {
            var talep = await _db.Talepler.FindAsync(id);
            if (talep == null) return NotFound();

            talep.Durum = "İptal";
            talep.IptalNedeni = reason;
            talep.SonIslemTarihi = DateTime.UtcNow;

            await _db.SaveChangesAsync();

            _db.TalepLoglar.Add(new TalepLog
            {
                TalepId = talep.Id,
                Tip = "Iptal",
                Yeni = reason,
                KullaniciId = _userManager.GetUserId(User)
            });
            await _db.SaveChangesAsync();

            TempData["SuccessMessage"] = "Talep iptal edildi.";
            return RedirectToAction(nameof(Details), new { id });
        }
    }
}
