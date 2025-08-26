using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace KEPSAS.TTS.Controllers
{
    [Authorize(Roles = "Admin")] // Bu sayfanın SADECE Admin rolündekiler tarafından görülmesini sağlar.
    public class AdminDashboardController : Controller
    {
        public IActionResult Index()
        {
            // Admin paneli için özel verileri burada çekip View'e gönderebilirsiniz.
            return View();
        }
    }
}
