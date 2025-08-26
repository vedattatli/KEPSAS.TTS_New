using System.Collections.Generic;

// Projenizin ad alanına göre güncelleyin
namespace TalepTakipSistemi.ViewModels
{
    // Anasayfadaki talepleri temsil eden basit bir sınıf
    public class SonTalepViewModel
    {
        public int Id { get; set; }
        public string Baslik { get; set; }
        public string Durum { get; set; }
        public string OlusturmaTarihi { get; set; }
    }

    // Gösterge panelinin ihtiyaç duyduğu tüm verileri içeren ana model
    public class DashboardViewModel
    {
        public int UzerimdeTalepler { get; set; }
        public int OnaysizTalepler { get; set; }
        public int OnayliTalepler { get; set; }
        public int YoneticiOnayindaTalepler { get; set; }
        public int DisServisteTalepler { get; set; }
        public int GecikmisTalepler { get; set; }

        public List<SonTalepViewModel> SonTalepler { get; set; }

        public DashboardViewModel()
        {
            SonTalepler = new List<SonTalepViewModel>();
        }
    }
}
