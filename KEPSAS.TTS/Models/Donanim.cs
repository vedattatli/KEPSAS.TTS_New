// Models/Donanim.cs
using KEPSAS.TTS.Data; // ApplicationUser için

namespace KEPSAS.TTS.Models
{
    public class Donanim
    {
        public int Id { get; set; }

        public string SeriNo { get; set; } = string.Empty;
        public string Model { get; set; } = string.Empty;
        public string Kategori { get; set; } = string.Empty;

        public string? Durum { get; set; }           // "Kullanımda", "Stokta" vb.
        public DateTime? AlinmaTarihi { get; set; }

        // ---> Kullanıcı ilişkisi (Include için gerekli)
        public string? AtananKullaniciId { get; set; }
        public ApplicationUser? AtananKullanici { get; set; }

        public DateTime OlusturmaTarihi { get; set; } = DateTime.UtcNow;
    }
}
