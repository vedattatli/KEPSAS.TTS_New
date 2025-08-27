using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace KEPSAS.TTS.ViewModels
{
    public class DonanimEkleViewModel
    {
        [Required, Display(Name = "Seri Numarası")]
        public string SeriNo { get; set; } = string.Empty;

        [Required, Display(Name = "Marka / Model")]
        public string Model { get; set; } = string.Empty;

        [Display(Name = "Kategori")]
        public string? Kategori { get; set; }

        [Display(Name = "Alınma Tarihi")]
        [DataType(DataType.Date)]
        public DateTime? AlinmaTarihi { get; set; }

        // Kullanıcı atama (boş bırakılabilir)
        [Display(Name = "Atanacak Kullanıcı")]
        public string? AtanacakKullaniciId { get; set; }

        // Durum seçimi (kullanıcı seçilmişse POST’ta otomatik Kullanımda yapılır)
        [Display(Name = "Durum")]
        public string? Durum { get; set; } = "Boşta";

        // Dropdown kaynakları
        public List<string> Kategoriler { get; set; } = new();
        public List<string> Durumlar { get; set; } = new();
        public List<KeyValuePair<string, string>> KullaniciSecenekleri { get; set; } = new();
    }
}
