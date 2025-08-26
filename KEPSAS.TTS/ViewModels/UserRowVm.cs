namespace KEPSAS.TTS.ViewModels
{
    public class UserRowVm
    {
        public string Id { get; set; } = default!;
        public string UserName { get; set; } = default!;
        public string? Ad { get; set; }
        public string? Soyad { get; set; }
        public string? Email { get; set; }
        public bool IsAdmin { get; set; }
    }
}