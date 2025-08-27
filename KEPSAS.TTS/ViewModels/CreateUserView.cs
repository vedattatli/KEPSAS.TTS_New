using System.ComponentModel.DataAnnotations;

namespace KEPSAS.TTS.ViewModels
{
    public class CreateUserVm
    {
        [Required, StringLength(64)]
        public string UserName { get; set; } = string.Empty;

        [Required, EmailAddress, StringLength(128)]
        public string Email { get; set; } = string.Empty;

        // Identity default policy: en az 6 karakter (senin projende ekstra kurallar varsa ona göre yaz)
        [Required, DataType(DataType.Password), StringLength(64, MinimumLength = 6)]
        public string Password { get; set; } = string.Empty;

        [Required]  // "Admin" veya "User"
        public string Role { get; set; } = "User";

        // (Opsiyonel) Ad/Soyad istiyorsan:
        [StringLength(64)]
        public string? Ad { get; set; }

        [StringLength(64)]
        public string? Soyad { get; set; }
    }
}
