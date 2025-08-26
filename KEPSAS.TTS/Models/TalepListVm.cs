namespace KEPSAS.TTS.Models
{
    public class TalepListVm
    {
        public string? Search { get; set; }
        public IEnumerable<Talep> Liste { get; set; } = Enumerable.Empty<Talep>();
        public int Uzerimde { get; set; }
        public int Onaysiz { get; set; }
        public int Onayli { get; set; }
        public int Bekleyen { get; set; }
        public int Haricen { get; set; }
    }
}
