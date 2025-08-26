namespace KEPSAS.TTS.Models
{
    public class Donanim
    {
        public int Id { get; set; }
        public string Ad { get; set; } = "";
        public string? Tip { get; set; }
        public string? MarkaModel { get; set; }
        public string? SeriNo { get; set; }
        public string? Durum { get; set; }
        public DateTime OlusturmaTarihi { get; set; } = DateTime.UtcNow;
    }
}