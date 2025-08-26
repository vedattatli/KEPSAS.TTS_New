namespace KEPSAS.TTS.Models
{
    public class HomeVm
    {
        public string? Search { get; set; }
        public IEnumerable<Talep> Liste { get; set; } = Enumerable.Empty<Talep>();

        public int Uzerimde { get; set; }
        public int Onaysiz { get; set; }
        public int Onayli { get; set; }
        public int YoneticiOnayinda { get; set; }
        public int DisServiste { get; set; }
        public int Gecikmis { get; set; }
    }
}
