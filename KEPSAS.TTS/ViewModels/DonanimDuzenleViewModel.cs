using System;
using System.Collections.Generic;

namespace KEPSAS.TTS.ViewModels
{
    public class DonanimDuzenleViewModel
    {
        public int Id { get; set; }

        public string SeriNo { get; set; } = string.Empty;
        public string Model { get; set; } = string.Empty;
        public string? Kategori { get; set; }
        public DateTime? AlinmaTarihi { get; set; }

        public string? AtanacakKullaniciId { get; set; } // null/empty => Boşta
        public string? Durum { get; set; }               // Stokta/Kullanımda/Boşta/Arızalı/Hurda

        // Drop-down kaynakları
        public List<string> Kategoriler { get; set; } = new();
        public List<string> Durumlar { get; set; } = new();
        public List<KeyValuePair<string, string>> KullaniciSecenekleri { get; set; } = new();
    }
}
