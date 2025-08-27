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
    public class HomeController : Controller
    {
        private readonly ApplicationDbContext _db;
        private readonly UserManager<ApplicationUser> _userManager;

        public HomeController(ApplicationDbContext db, UserManager<ApplicationUser> userManager)
        {
            _db = db;
            _userManager = userManager;
        }

        public async Task<IActionResult> Index()
        {
            // temel sorgu: admin => tüm talepler, user => kendi talepleri
            var meId = _userManager.GetUserId(User);
            var isAdmin = User.IsInRole("Admin");

            IQueryable<Talep> baseQ = _db.Talepler.AsNoTracking();

            baseQ = baseQ.Where(t => (t.Durum ?? "") != "Tamamlandı");

            if (!isAdmin)
            {
                // DB şeman hangi alanı destekliyorsa onu kullan:
                // - oluşturana göre
                baseQ = baseQ.Where(t => t.OlusturanKullaniciId == meId
                                      || t.AtananKullaniciId == meId); 
            }

            // Güvenli sayım helper’ı
            async Task<int> CountBy(string durumAd)
                => await baseQ.CountAsync(t => (t.Durum ?? "") == durumAd);

            var vm = new DashboardViewModel
            {
                Uzerimde = await CountBy("Üzerimde"),
                Onaysiz = await CountBy("Onaysız"),
                Onayli = await CountBy("Onaylı"),
                YoneticiOnayinda = await CountBy("Yönetici Onayında"),
                DisServiste = await CountBy("Dış Serviste"),
                Gecikmis = await CountBy("Gecikmiş"),

                SonTalepler = await baseQ
                    .OrderByDescending(t => t.OlusturmaTarihi)
                    .Take(20)
                    .Select(t => new DashboardItemVm
                    {
                        Id = t.Id,
                        Baslik = t.Baslik ?? "(başlıksız)",
                        Durum = t.Durum ?? "-",
                        OlusturmaTarihi = t.OlusturmaTarihi
                    })
                    .ToListAsync()
            };

            return View(vm);
        }
    }
}
