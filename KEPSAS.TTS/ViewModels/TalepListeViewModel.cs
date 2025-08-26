// --- ViewModels/TalepListeViewModel.cs ---
using System.Collections.Generic;

// HATA DÜZELTMESİ: Ad alanı projenizin adıyla güncellendi.
namespace KEPSAS.TTS.ViewModels
{
    public class TalepOzetViewModel
    {
        public int Id { get; set; }
        public string? Baslik { get; set; }
        public string? OlusturanKullanici { get; set; }
        public string? Durum { get; set; }
        public string? OlusturmaTarihi { get; set; }
    }

    public class TalepListeViewModel
    {
        public string? AramaTerimi { get; set; }
        public string? DurumFiltresi { get; set; }
        public List<TalepOzetViewModel> Talepler { get; set; } = new();
    }
}