using KEPSAS.TTS.Data;
using KEPSAS.TTS.Models;
using KEPSAS.TTS.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace KEPSAS.TTS.Controllers
{
    [Authorize]
    public class DonanimController : Controller
    {
        private readonly ApplicationDbContext _db;
        private readonly UserManager<ApplicationUser> _userManager;

        // Genişletilmiş kategori/durum kaynakları (veritabanıyla birleştiriyoruz)
        private static readonly List<string> _kategoriKaynak = new()
        {
            "Bilgisayar","Dizüstü","Masaüstü","All in One","Sunucu","Yazıcı","Tarayıcı",
            "Ağ Cihazı (Switch)","Ağ Cihazı (Router)","Erişim Noktası (AP)","Firewall",
            "Telefon","IP Telefon","Tablet","Ekran/Monitör","Projeksiyon","UPS",
            "Depolama (NAS)","Harici Disk","Kamera","Yazılım Lisansı",
            "Aksesuar (Klavye)","Aksesuar (Mouse)","Aksesuar (Kulaklık)","Dock/Hub",
            "Kablolama","Diğer"
        };

        private static readonly List<string> _durumKaynak = new()
        {
            "Stokta","Kullanımda","Boşta","Arızalı","Hurda"
        };

        public DonanimController(ApplicationDbContext db, UserManager<ApplicationUser> um)
        {
            _db = db;
            _userManager = um;
        }

        // ENVANTER YÖNETİMİ (Admin)
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Index(string? q, string? kategori, string? durum)
        {
            var query = _db.Donanimlar
                .Include(x => x.AtananKullanici)
                .AsNoTracking()
                .AsQueryable();

            if (!string.IsNullOrWhiteSpace(q))
                query = query.Where(x =>
                    (x.SeriNo ?? "").Contains(q) ||
                    (x.Model ?? "").Contains(q));

            if (!string.IsNullOrWhiteSpace(kategori))
                query = query.Where(x => x.Kategori == kategori);

            if (!string.IsNullOrWhiteSpace(durum))
                query = query.Where(x => x.Durum == durum);

            var list = await query
                .OrderBy(x => x.Kategori)
                .ThenBy(x => x.Model)
                .ToListAsync();

            // Filtre drop-down’ları: statik + veritabanındaki farklı değerler
            var dbKategoriler = await _db.Donanimlar
                .Select(x => x.Kategori!)
                .Where(x => x != null && x != "")
                .Distinct()
                .ToListAsync();

            ViewBag.Kategoriler = _kategoriKaynak
                .Union(dbKategoriler)
                .OrderBy(x => x)
                .ToList();

            ViewBag.Durumlar = _durumKaynak;

            return View(list);
        }

        // KULLANICININ KENDİ DONANIMLARI
        public async Task<IActionResult> Kullandiklarim()
        {
            var currentUserId = _userManager.GetUserId(User);
            var list = await _db.Donanimlar
                .AsNoTracking()
                .Where(d => d.AtananKullaniciId == currentUserId) // gerçekten bana atanmış olanlar
                .OrderBy(d => d.Model)
                .ToListAsync();

            return View(list);
        }

        // YENİ DONANIM FORMU
        [Authorize(Roles = "Admin")]
        [HttpGet]
        public async Task<IActionResult> Create()
        {
            // DB’deki kategorilerle birleştir
            var dbKategoriler = await _db.Donanimlar
                .Select(x => x.Kategori!)
                .Where(x => x != null && x != "")
                .Distinct()
                .ToListAsync();

            var vm = new DonanimEkleViewModel
            {
                Durum = "Boşta",
                Kategoriler = _kategoriKaynak.Union(dbKategoriler).OrderBy(x => x).ToList(),
                Durumlar = _durumKaynak,
                KullaniciSecenekleri = await _userManager.Users
                    .OrderBy(x => x.UserName)
                    .Select(u => new KeyValuePair<string, string>(u.Id, u.UserName ?? u.Email ?? u.Id))
                    .ToListAsync()
            };
            return View(vm);
        }

        // YENİ DONANIM KAYDI
        [Authorize(Roles = "Admin")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(DonanimEkleViewModel vm)
        {
            if (!ModelState.IsValid)
            {
                // listeleri yeniden yükle
                var dbKategoriler = await _db.Donanimlar
                    .Select(x => x.Kategori!)
                    .Where(x => x != null && x != "")
                    .Distinct()
                    .ToListAsync();

                vm.Kategoriler = _kategoriKaynak.Union(dbKategoriler).OrderBy(x => x).ToList();
                vm.Durumlar = _durumKaynak;
                vm.KullaniciSecenekleri = await _userManager.Users
                    .OrderBy(x => x.UserName)
                    .Select(u => new KeyValuePair<string, string>(u.Id, u.UserName ?? u.Email ?? u.Id))
                    .ToListAsync();

                return View(vm);
            }

            var entity = new Donanim
            {
                SeriNo = vm.SeriNo.Trim(),
                Model = vm.Model.Trim(),
                Kategori = vm.Kategori?.Trim(),
                AlinmaTarihi = vm.AlinmaTarihi,
                AtananKullaniciId = string.IsNullOrWhiteSpace(vm.AtanacakKullaniciId) ? null : vm.AtanacakKullaniciId,
                Durum = string.IsNullOrWhiteSpace(vm.AtanacakKullaniciId)
                            ? (vm.Durum ?? "Boşta")
                            : "Kullanımda"
            };

            _db.Donanimlar.Add(entity);
            await _db.SaveChangesAsync();

            TempData["SuccessMessage"] = "Donanım envantere eklendi.";
            return RedirectToAction(nameof(Index));
        }

        // DÜZENLE – GET
        [Authorize(Roles = "Admin")]
        [HttpGet]
        public async Task<IActionResult> Edit(int id)
        {
            var d = await _db.Donanimlar
                .Include(x => x.AtananKullanici)
                .FirstOrDefaultAsync(x => x.Id == id);

            if (d == null) return NotFound();

            var dbKategoriler = await _db.Donanimlar
                .Select(x => x.Kategori!)
                .Where(x => x != null && x != "")
                .Distinct()
                .ToListAsync();

            var vm = new DonanimDuzenleViewModel
            {
                Id = d.Id,
                SeriNo = d.SeriNo ?? string.Empty,
                Model = d.Model ?? string.Empty,
                Kategori = d.Kategori,
                AlinmaTarihi = d.AlinmaTarihi,
                AtanacakKullaniciId = d.AtananKullaniciId,
                Durum = d.Durum ?? "Boşta",
                Kategoriler = _kategoriKaynak.Union(dbKategoriler).OrderBy(x => x).ToList(),
                Durumlar = _durumKaynak,
                KullaniciSecenekleri = await _userManager.Users
                    .OrderBy(u => u.UserName)
                    .Select(u => new KeyValuePair<string, string>(u.Id, u.UserName ?? u.Email ?? u.Id))
                    .ToListAsync()
            };

            return View(vm);
        }

        // DÜZENLE – POST
        [Authorize(Roles = "Admin")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(DonanimDuzenleViewModel vm)
        {
            var d = await _db.Donanimlar.FirstOrDefaultAsync(x => x.Id == vm.Id);
            if (d == null) return NotFound();

            if (!ModelState.IsValid)
            {
                var dbKategoriler = await _db.Donanimlar
                    .Select(x => x.Kategori!)
                    .Where(x => x != null && x != "")
                    .Distinct()
                    .ToListAsync();

                vm.Kategoriler = _kategoriKaynak.Union(dbKategoriler).OrderBy(x => x).ToList();
                vm.Durumlar = _durumKaynak;
                vm.KullaniciSecenekleri = await _userManager.Users
                    .OrderBy(u => u.UserName)
                    .Select(u => new KeyValuePair<string, string>(u.Id, u.UserName ?? u.Email ?? u.Id))
                    .ToListAsync();

                return View(vm);
            }

            d.SeriNo = vm.SeriNo.Trim();
            d.Model = vm.Model.Trim();
            d.Kategori = vm.Kategori;
            d.AlinmaTarihi = vm.AlinmaTarihi;

            if (string.IsNullOrWhiteSpace(vm.AtanacakKullaniciId))
            {
                d.AtananKullaniciId = null;
                d.Durum = string.IsNullOrWhiteSpace(vm.Durum) ? "Boşta" : vm.Durum;
            }
            else
            {
                d.AtananKullaniciId = vm.AtanacakKullaniciId;
                d.Durum = "Kullanımda";
            }

            await _db.SaveChangesAsync();
            TempData["SuccessMessage"] = "Donanım güncellendi.";
            return RedirectToAction(nameof(Index));
        }
    }
}
