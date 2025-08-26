using System;

namespace KEPSAS.TTS.Models
{
    public class TalepLog
    {
        public int Id { get; set; }
        public int TalepId { get; set; }

        public DateTime Tarih { get; set; } = DateTime.UtcNow;
        public string? KullaniciId { get; set; } // işlemi yapan
        public string Tip { get; set; } = "";    // "Durum", "Atama", "EkYukleme", "EkSilme" vs.
        public string? Eski { get; set; }
        public string? Yeni { get; set; }
        public string? Not { get; set; }

        public Talep? Talep { get; set; }
    }
}
