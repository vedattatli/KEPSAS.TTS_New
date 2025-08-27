// Controllers/ReportsController.cs
using System.Text;
using KEPSAS.TTS.Data;
using KEPSAS.TTS.ViewModels;
using KEPSAS.TTS.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace KEPSAS.TTS.Controllers
{
    [Authorize]
    public class ReportsController : Controller
    {
        private readonly ApplicationDbContext _db;

        // Kapalı statüler burada sabit: EF bunu IN(...) olarak çevirir
        private static readonly string[] ClosedStatuses = new[] { "Tamamlandı", "Onaylı" };

        public ReportsController(ApplicationDbContext db) => _db = db;

        // ----------------------- Helpers -----------------------
        private IQueryable<Talep> BaseQuery(bool isAdmin, string? meId)
        {
            var q = _db.Talepler
                .Include(t => t.OlusturanKullanici)
                .Include(t => t.AtananKullanici)
                .AsNoTracking();

            if (!isAdmin && meId != null)
                q = q.Where(t => t.OlusturanKullaniciId == meId);

            return q;
        }

        private static IQueryable<Talep> ApplyFilters(IQueryable<Talep> q, ReportsFilterVm f)
        {
            if (f.From.HasValue) q = q.Where(t => t.OlusturmaTarihi >= f.From.Value);
            if (f.To.HasValue) q = q.Where(t => t.OlusturmaTarihi <= f.To.Value);
            if (!string.IsNullOrWhiteSpace(f.CreatorId)) q = q.Where(t => t.OlusturanKullaniciId == f.CreatorId);
            if (!string.IsNullOrWhiteSpace(f.AssigneeId)) q = q.Where(t => t.AtananKullaniciId == f.AssigneeId);
            if (!string.IsNullOrWhiteSpace(f.Durum)) q = q.Where(t => (t.Durum ?? "") == f.Durum);
            return q;
        }

        // ----------------------- Index -----------------------
        public async Task<IActionResult> Index(DateTime? from, DateTime? to, string? creatorId, string? assigneeId, string? durum)
        {
            var isAdmin = User.IsInRole("Admin");
            var meId = User.FindFirst(System.Security.Claims.ClaimTypes.NameIdentifier)?.Value;

            var filters = new ReportsFilterVm
            {
                From = from,
                To = to,
                CreatorId = creatorId,
                AssigneeId = assigneeId,
                Durum = durum
            };

            var q = ApplyFilters(BaseQuery(isAdmin, meId), filters);

            // Özet kartlar
            var toplam = await q.CountAsync();

            var kapananQ = q.Where(t => ClosedStatuses.Contains(t.Durum));
            var kapanan = await kapananQ.CountAsync();

            var acik = toplam - kapanan;

            double? mttrSaat = null;
            {
                // MTTR (dakika → saat)
                var kapaliSureler = await kapananQ
                    .Where(t => t.SonIslemTarihi.HasValue)
                    .Select(t => EF.Functions.DateDiffMinute(t.OlusturmaTarihi, t.SonIslemTarihi!.Value))
                    .ToListAsync();

                if (kapaliSureler.Count > 0)
                    mttrSaat = Math.Round(kapaliSureler.Average() / 60.0, 1);
            }

            // Durum dağılımı
            var durumDagilimi = await q
                .GroupBy(t => t.Durum ?? "-")
                .Select(g => new StatusCountVm { Durum = g.Key, Adet = g.Count() })
                .OrderByDescending(x => x.Adet)
                .ToListAsync();

            // Günlük trend (Açılan/Kapanan/Backlog)
            var basTar = filters.From ?? DateTime.UtcNow.Date.AddDays(-29);
            var bitTar = (filters.To ?? DateTime.UtcNow.Date).Date;
            var days = Enumerable.Range(0, (bitTar - basTar).Days + 1)
                                 .Select(i => basTar.AddDays(i))
                                 .ToList();

            // Açılan
            var opened = await q
                .GroupBy(t => t.OlusturmaTarihi.Date)
                .Select(g => new { Tarih = g.Key, Adet = g.Count() })
                .ToListAsync();

            // Kapanan (Contains ile)
            var closedDaily = await q
                .Where(t => ClosedStatuses.Contains(t.Durum) && t.SonIslemTarihi.HasValue)
                .GroupBy(t => t.SonIslemTarihi!.Value.Date)
                .Select(g => new { Tarih = g.Key, Adet = g.Count() })
                .ToListAsync();

            var openedDict = opened.ToDictionary(x => x.Tarih, x => x.Adet);
            var closedDict = closedDaily.ToDictionary(x => x.Tarih, x => x.Adet);

            var trend = new List<TrendPointVm>();
            int cumOpen = 0, cumClosed = 0;
            foreach (var d in days)
            {
                cumOpen += openedDict.TryGetValue(d, out var o) ? o : 0;
                cumClosed += closedDict.TryGetValue(d, out var c) ? c : 0;

                trend.Add(new TrendPointVm
                {
                    Tarih = d,
                    Yeni = openedDict.GetValueOrDefault(d, 0),
                    Kapanan = closedDict.GetValueOrDefault(d, 0),
                    Backlog = Math.Max(0, cumOpen - cumClosed)
                });
            }

            // Top talep açanlar
            var topCreators = await q
                .GroupBy(t => t.OlusturanKullanici!.UserName ?? t.OlusturanKullanici!.Email ?? "(?)")
                .Select(g => new TopItemVm { Ad = g.Key, Adet = g.Count() })
                .OrderByDescending(x => x.Adet)
                .Take(10)
                .ToListAsync();

            // Top atananlar
            var topAssignees = await q
                .Where(t => t.AtananKullanici != null)
                .GroupBy(t => t.AtananKullanici!.UserName ?? t.AtananKullanici!.Email ?? "(?)")
                .Select(g => new TopItemVm { Ad = g.Key, Adet = g.Count() })
                .OrderByDescending(x => x.Adet)
                .Take(10)
                .ToListAsync();

            // Yaş dağılımı (yalnız açık talepler)
            var now = DateTime.UtcNow;
            var acikQ = q.Where(t => !ClosedStatuses.Contains(t.Durum));
            var yaslar = await acikQ
                .Select(t => EF.Functions.DateDiffDay(t.OlusturmaTarihi, now))
                .ToListAsync();

            var aging = new List<AgingBucketVm>
            {
                new() { Aralik = "0–2 gün",   Adet = yaslar.Count(x => x is >= 0 and <= 2) },
                new() { Aralik = "3–7 gün",   Adet = yaslar.Count(x => x is >= 3 and <= 7) },
                new() { Aralik = "8–14 gün",  Adet = yaslar.Count(x => x is >= 8 and <= 14) },
                new() { Aralik = "15–30 gün", Adet = yaslar.Count(x => x is >= 15 and <= 30) },
                new() { Aralik = "30+ gün",   Adet = yaslar.Count(x => x > 30) },
            };

            var vm = new ReportsDashboardVm
            {
                Filters = filters,
                Toplam = toplam,
                Acik = acik,
                Kapanan = kapanan,
                MTTR_Saat = mttrSaat,
                DurumDagilimi = durumDagilimi,
                GunlukTrend = trend,
                EnCokTalepAcanlar = topCreators,
                EnCokAtananlar = topAssignees,
                YasDagilimi = aging
            };

            // Filtre dropdown’ları
            ViewBag.Users = await _db.Users
                .OrderBy(u => u.UserName)
                .Select(u => new KeyValuePair<string, string>(u.Id, u.UserName ?? u.Email ?? u.Id))
                .ToListAsync();

            ViewBag.Durumlar = await _db.Talepler
                .Select(t => t.Durum!)
                .Where(x => x != null && x != "")
                .Distinct()
                .OrderBy(x => x)
                .ToListAsync();

            return View(vm);
        }

        // ----------------------- CSV Export -----------------------
        public async Task<FileResult> ExportCsv(DateTime? from, DateTime? to, string? creatorId, string? assigneeId, string? durum)
        {
            var isAdmin = User.IsInRole("Admin");
            var meId = User.FindFirst(System.Security.Claims.ClaimTypes.NameIdentifier)?.Value;

            var filters = new ReportsFilterVm
            {
                From = from,
                To = to,
                CreatorId = creatorId,
                AssigneeId = assigneeId,
                Durum = durum
            };

            var q = ApplyFilters(BaseQuery(isAdmin, meId), filters)
                .OrderByDescending(t => t.OlusturmaTarihi)
                .Select(t => new
                {
                    t.Id,
                    t.Baslik,
                    t.Durum,
                    Olusturma = t.OlusturmaTarihi,
                    Olusturan = t.OlusturanKullanici!.UserName ?? t.OlusturanKullanici!.Email,
                    Atanan = t.AtananKullanici != null ? (t.AtananKullanici.UserName ?? t.AtananKullanici.Email) : ""
                });

            var rows = await q.ToListAsync();

            var sb = new StringBuilder();
            sb.AppendLine("Id;Baslik;Durum;Olusturma;Olusturan;Atanan");
            foreach (var r in rows)
                sb.AppendLine($"{r.Id};{r.Baslik};{r.Durum};{r.Olusturma:yyyy-MM-dd HH:mm};{r.Olusturan};{r.Atanan}");

            return File(Encoding.UTF8.GetBytes(sb.ToString()), "text/csv", "talepler.csv");
        }
    }
}
