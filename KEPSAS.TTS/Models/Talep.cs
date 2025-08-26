using System;
using System.Collections.Generic;

namespace KEPSAS.TTS.Models
{
    public class Talep
    {
        public int Id { get; set; }
        public string Baslik { get; set; } = "";
        public string Aciklama { get; set; } = "";
        public string Durum { get; set; } = "Beklemede";
        public DateTime OlusturmaTarihi { get; set; } = DateTime.UtcNow;

        // Atama / akış
        public string? OlusturanKullaniciId { get; set; }
        public string? AtananKullaniciId { get; set; }
        public DateTime? AtamaTarihi { get; set; }
        public DateTime? SonIslemTarihi { get; set; }

        // Navigations
        public ICollection<TalepEk>? Ekler { get; set; }
        public ICollection<TalepLog>? Loglar { get; set; }
    }
}
