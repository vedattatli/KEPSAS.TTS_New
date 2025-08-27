using KEPSAS.TTS.Data;
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
        // --- FK + Navigation (NULL olabilir) ---
        public string? OlusturanKullaniciId { get; set; }
        public ApplicationUser? OlusturanKullanici { get; set; }   // <-- navigation

        public string? AtananKullaniciId { get; set; }
        public ApplicationUser? AtananKullanici { get; set; }      // <-- navigation
        public DateTime? AtamaTarihi { get; set; }
        public DateTime? SonIslemTarihi { get; set; }

        // Navigations
        public ICollection<TalepEk>? Ekler { get; set; }
        public ICollection<TalepLog>? Loglar { get; set; }
    }
}
