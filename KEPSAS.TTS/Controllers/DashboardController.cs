using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
[Authorize]
public class DashboardController : Controller
{

    public IActionResult Index() => View(new { Uzerimde = 10, Onaysiz = 2, Onayli = 29, Bekleyen = 6, Haric = 19 });
}
