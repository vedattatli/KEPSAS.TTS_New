using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using TalepTakipSistemi.ViewModels; // ViewModel'i eklemeyi unutmayın
[Authorize]
public class HomeController : Controller
{

    // Bu bölüm normalde veritabanından gelecek
    private DashboardViewModel GetDashboardData()
    {
        // Sahte (mock) veriler oluşturuyoruz
        var viewModel = new DashboardViewModel
        {
            UzerimdeTalepler = 3,
            OnaysizTalepler = 1,
            OnayliTalepler = 18,
            YoneticiOnayindaTalepler = 2,
            DisServisteTalepler = 5,
            GecikmisTalepler = 0,
            SonTalepler = new List<SonTalepViewModel>
            {
                new SonTalepViewModel { Id = 101, Baslik = "Yeni Yazıcı Kurulumu", Durum = "Uzerimde", OlusturmaTarihi = "26.08.2025 14:30" },
                new SonTalepViewModel { Id = 100, Baslik = "Mausum çalışmıyor", Durum = "Beklemede", OlusturmaTarihi = "26.08.2025 11:15" },
                new SonTalepViewModel { Id = 99, Baslik = "Ofis programları lisans talebi", Durum = "Onayli", OlusturmaTarihi = "25.08.2025 16:00" },
                new SonTalepViewModel { Id = 98, Baslik = "VPN bağlantı sorunu", Durum = "Beklemede", OlusturmaTarihi = "25.08.2025 09:05" }
            }
        };
        return viewModel;
    }

    public IActionResult Index()
    {
        // Verileri alıp View'e gönderiyoruz
        var model = GetDashboardData();
        return View(model);
    }
}
