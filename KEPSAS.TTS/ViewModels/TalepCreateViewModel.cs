// ViewModels/TalepCreateViewModel.cs
using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Mvc.Rendering;
using KEPSAS.TTS.Models;

namespace KEPSAS.TTS.ViewModels
{
    public class TalepCreateViewModel
    {
        [Required, StringLength(128)]
        [Display(Name = "Talep Adı")]
        public string Baslik { get; set; } = string.Empty;

        [Display(Name = "Açıklama")]
        [DataType(DataType.MultilineText)]
        public string? Aciklama { get; set; }

        [Display(Name = "Talep Tipi")]
        [Required]
        public TalepTipi Tip { get; set; } = TalepTipi.Diger;

        [Display(Name = "Donanım")]
        public int? DonanimId { get; set; }

        [Display(Name = "Yazılım")]
        [StringLength(128)]
        public string? YazilimAdi { get; set; }

        [Display(Name = "Hedef Personel")]
        public string? HedefKullaniciId { get; set; }

        [Display(Name = "Hedef Sicil No")]
        [StringLength(64)]
        public string? HedefSicilNo { get; set; }

        // Dropdown kaynakları
        public List<SelectListItem> Donanimlar { get; set; } = new();
        public List<SelectListItem> Kullanicilar { get; set; } = new();
        public List<string> YazilimListe { get; set; } = new(); // basit string listesi
    }
}
