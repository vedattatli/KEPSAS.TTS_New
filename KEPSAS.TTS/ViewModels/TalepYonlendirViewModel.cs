using Microsoft.AspNetCore.Mvc.Rendering;
using System.Collections.Generic;

// HATA DÜZELTMESİ: Ad alanı projenizin adıyla güncellendi.
namespace KEPSAS.TTS.ViewModels
{
    public class TalepYonlendirViewModel
    {
        public int TalepId { get; set; }

        public string? AtanacakKullaniciId { get; set; }

        public string? YonlendirmeNotu { get; set; }
        public List<SelectListItem> KullanicilarListesi { get; set; } = new();
    }
}