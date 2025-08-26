using KEPSAS.TTS.Data;
using KEPSAS.TTS.Models;
using KEPSAS.TTS.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.Threading.Tasks;

namespace KEPSAS.TTS.Controllers
{
    [Authorize] // Bu controller'a sadece giriş yapmış kullanıcılar erişebilir.
    public class DonanimController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly UserManager<ApplicationUser> _userManager;

        // Constructor: Gerekli servisleri (veritabanı ve kullanıcı yönetimi) enjekte ediyoruz.
        public DonanimController(ApplicationDbContext context, UserManager<ApplicationUser> userManager)
        {
            _context = context;
            _userManager = userManager;
        }

        // Adminler için Envanter Listesi (Tüm Donanımlar)
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Index()
        {
            // İlişkili kullanıcı verisini de (AtananKullanici.UserName) verimli bir şekilde çekmek için .Include() kullanıyoruz.
            // AsNoTracking() sorguyu hızlandırır çünkü veriler sadece okunacaktır.
            var donanimlar = await _context.Donanimlar
                                           .Include(d => d.AtananKullanici)
                                           .AsNoTracking()
                                           .OrderBy(d => d.Kategori)
                                           .ThenBy(d => d.Model)
                                           .ToListAsync();
            return View(donanimlar);
        }

        // Standart kullanıcılar için "Donanımlarım" sayfası
        public async Task<IActionResult> Kullandiklarim()
        {
            // O an giriş yapmış olan kullanıcının kimliğini (ID) alıyoruz.
            var currentUserId = _userManager.GetUserId(User);

            // Veritabanından sadece bu kullanıcıya atanmış donanımları çekiyoruz.
            var kullaniciDonanimlari = await _context.Donanimlar
                                                     .Where(d => d.AtananKullaniciId == currentUserId)
                                                     .AsNoTracking()
                                                     .ToListAsync();

            return View(kullaniciDonanimlari);
        }

        // Adminler için "Yeni Donanım Ekle" formunu gösterir.
        [Authorize(Roles = "Admin")]
        [HttpGet]
        public IActionResult Create()
        {
            return View();
        }

        // "Yeni Donanım Ekle" formundan gelen veriyi işler ve veritabanına kaydeder.
        [Authorize(Roles = "Admin")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(DonanimEkleViewModel model)
        {
            if (ModelState.IsValid)
            {
                var yeniDonanim = new Donanim
                {
                    SeriNo = model.SeriNo,
                    Model = model.Model,
                    Kategori = model.Kategori,
                    AlinmaTarihi = model.AlinmaTarihi,
                    Durum = "Stokta" // Yeni eklenen her donanım başlangıçta stoktadır.
                };

                _context.Donanimlar.Add(yeniDonanim);
                await _context.SaveChangesAsync();

                // Başarılı işlem sonrası kullanıcıya geri bildirim vermek için TempData kullanıyoruz.
                TempData["SuccessMessage"] = $"'{yeniDonanim.Model}' modeli başarıyla envantere eklendi.";

                return RedirectToAction("Index");
            }

            // Model geçerli değilse, formu hatalarla birlikte tekrar göster.
            return View(model);
        }
    }
}
