using System.Text;
using KEPSAS.TTS.Data;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace KEPSAS.TTS.Controllers
{
    [Authorize]
    public class ReportsController : Controller
    {
        private readonly ApplicationDbContext _db;
        public ReportsController(ApplicationDbContext db) => _db = db;

        public async Task<IActionResult> Index(DateTime? from, DateTime? to)
        {
            var q = _db.Talepler.AsNoTracking().AsQueryable();
            if (from.HasValue) q = q.Where(t => t.OlusturmaTarihi >= from.Value);
            if (to.HasValue) q = q.Where(t => t.OlusturmaTarihi <= to.Value);

            var toplam = await q.CountAsync();
            var durumDagilimi = await q.GroupBy(t => t.Durum)
                                       .Select(g => new { Durum = g.Key!, Adet = g.Count() })
                                       .OrderByDescending(x => x.Adet).ToListAsync();

            ViewBag.Toplam = toplam;
            return View(durumDagilimi);
        }

        // CSV export (tarih filtresine saygılı)
        public async Task<FileResult> ExportCsv(DateTime? from, DateTime? to)
        {
            var q = _db.Talepler.AsNoTracking().AsQueryable();
            if (from.HasValue) q = q.Where(t => t.OlusturmaTarihi >= from.Value);
            if (to.HasValue) q = q.Where(t => t.OlusturmaTarihi <= to.Value);

            var rows = await q.OrderByDescending(t => t.OlusturmaTarihi)
                              .Select(t => new { t.Id, t.Baslik, t.Durum, t.OlusturmaTarihi })
                              .ToListAsync();

            var sb = new StringBuilder();
            sb.AppendLine("Id;Baslik;Durum;Olusturma");
            foreach (var r in rows)
                sb.AppendLine($"{r.Id};{r.Baslik};{r.Durum};{r.OlusturmaTarihi:yyyy-MM-dd HH:mm}");

            return File(Encoding.UTF8.GetBytes(sb.ToString()), "text/csv", "talepler.csv");
        }
    }
}
