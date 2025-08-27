using KEPSAS.TTS.Data;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace KEPSAS.TTS.Controllers
{
    [Authorize]
    public class MeController : Controller
    {
        private readonly ApplicationDbContext _db;
        public MeController(ApplicationDbContext db) => _db = db;

        public async Task<IActionResult> Index()
        {
            var meId = User.FindFirst(System.Security.Claims.ClaimTypes.NameIdentifier)?.Value;

            var tamamlanan = await _db.Talepler.CountAsync(t => t.AtananKullaniciId == meId && t.Durum == "Onaylı");
            var uzerimde = await _db.Talepler.CountAsync(t => t.AtananKullaniciId == meId && t.Durum == "Üzerimde");
            var toplam = await _db.Talepler.CountAsync(t => t.OlusturanKullaniciId == meId);

            ViewBag.Tamamlanan = tamamlanan;
            ViewBag.Uzerimde = uzerimde;
            ViewBag.Toplam = toplam;

            return View();
        }
    }
}
