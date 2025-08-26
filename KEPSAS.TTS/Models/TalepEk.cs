namespace KEPSAS.TTS.Models
{
    public class TalepEk
    {
        public int Id { get; set; }
        public int TalepId { get; set; }
        public string DosyaAdi { get; set; } = "";
        public string DosyaYolu { get; set; } = "";
        public string? ContentType { get; set; }
        public long Boyut { get; set; }
        public DateTime YuklemeTarihi { get; set; }

        public Talep? Talep { get; set; }
    }
}
