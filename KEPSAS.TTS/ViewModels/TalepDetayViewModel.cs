using System;
using System.Collections.Generic;

// HATA DÜZELTMESİ: Ad alanı projenizin adıyla güncellendi.
namespace KEPSAS.TTS.ViewModels
{
    public class TalepHareketViewModel
    {
        public string? IslemYapan { get; set; }
        public string? Aciklama { get; set; }
        public DateTime Tarih { get; set; }
        public string? Ikon { get; set; }
        public string? Renk { get; set; }
    }

    public class TalepDetayViewModel
    {
        public int Id { get; set; }
        public string? Baslik { get; set; }
        public string? Aciklama { get; set; }
        public string? OlusturanKullanici { get; set; }
        public string? AtananKullanici { get; set; }
        public string? Durum { get; set; }
        public string? Oncelik { get; set; }
        public DateTime OlusturmaTarihi { get; set; }
        public DateTime? GuncellemeTarihi { get; set; }
        public List<TalepHareketViewModel> HareketGecmisi { get; set; } = new();
    }
}