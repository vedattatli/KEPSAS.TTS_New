namespace KEPSAS.TTS.ViewModels
{
    public class DashboardItemVm
    {
        public int Id { get; set; }
        public string Baslik { get; set; } = "";
        public string Durum { get; set; } = "";
        public DateTime? OlusturmaTarihi { get; set; }
    }

    public class DashboardViewModel
    {
        // Kartlardaki sayılar (gerçek DB’den)
        public int Uzerimde { get; set; }
        public int Onaysiz { get; set; }
        public int Onayli { get; set; }
        public int YoneticiOnayinda { get; set; }
        public int DisServiste { get; set; }
        public int Gecikmis { get; set; }

        // Son N talep listesi
        public List<DashboardItemVm> SonTalepler { get; set; } = new();
    }
}
