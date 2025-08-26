using KEPSAS.TTS.Data;
using KEPSAS.TTS.Models;
using KEPSAS.TTS.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.Threading.Tasks;

[Authorize]
public class TalepController : Controller
{
    
    private readonly ApplicationDbContext _context;
    private readonly UserManager<ApplicationUser> _userManager;

    public TalepController(ApplicationDbContext context, UserManager<ApplicationUser> userManager)
    {
        _context = context;
        _userManager = userManager;
    }

    // Index ve diğer temel metotlarınızı buraya ekleyebilirsiniz.
    // Örnek Index metodu:
    public async Task<IActionResult> Index()
    {
        var talepler = await _context.Talepler.ToListAsync();
        // Bu talepleri bir ViewModel'e dönüştürüp View'e göndermeniz en doğrusu olacaktır.
        return View(talepler);
    }


    public async Task<IActionResult> Details(int id)
    {
        var talep = await _context.Talepler.FindAsync(id);

        if (talep == null)
        {
            return NotFound();
        }

        // Kullanıcıyı, talebin içindeki 'OlusturanKullaniciId' ile ayrı olarak buluyoruz.
        // Bu alanın Talep modelinizde string tipinde olduğundan emin olun.
        var olusturanKullanici = await _userManager.FindByIdAsync(talep.OlusturanKullaniciId);

        var model = new TalepDetayViewModel
        {
            Id = talep.Id,
            Baslik = talep.Baslik,
            Aciklama = talep.Aciklama,
            OlusturanKullanici = olusturanKullanici?.UserName
        };

        return View(model);
    }

    [HttpGet]
    public async Task<IActionResult> Yonlendir(int id)
    {
        var kullanicilar = await _userManager.Users
                                             .Select(u => new SelectListItem
                                             {
                                                 Value = u.Id.ToString(),
                                                 Text = u.UserName
                                             })
                                             .ToListAsync();

        var model = new TalepYonlendirViewModel
        {
            TalepId = id,
            KullanicilarListesi = kullanicilar
        };

        return PartialView("_TalepYonlendirPartial", model);
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Yonlendir(TalepYonlendirViewModel model)
    {
        if (ModelState.IsValid)
        {
            var talep = await _context.Talepler.FindAsync(model.TalepId);
            if (talep == null)
            {
                return NotFound();
            }

            // Talep modelinizdeki 'AtananKullaniciId' alanını güncelliyoruz.
            // Bu alanın Talep modelinizde string? (nullable string) tipinde olduğundan emin olun.
            talep.AtananKullaniciId = model.AtanacakKullaniciId;

            await _context.SaveChangesAsync();
            return RedirectToAction("Details", new { id = model.TalepId });
        }

        // Model geçerli değilse, kullanıcı listesini tekrar doldurup partial view'i döndürürüz.
        model.KullanicilarListesi = await _userManager.Users
                                             .Select(u => new SelectListItem { Value = u.Id.ToString(), Text = u.UserName })
                                             .ToListAsync();
        return PartialView("_TalepYonlendirPartial", model);
    }
}
