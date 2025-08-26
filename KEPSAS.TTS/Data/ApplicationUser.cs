using Microsoft.AspNetCore.Identity;

namespace KEPSAS.TTS.Data
{
    public class ApplicationUser : IdentityUser
    {
        // SeedData ve UsersController'ın beklediği alanlar:
        public string? Ad { get; set; }
        public string? Soyad { get; set; }

        // Daha önce kullandığımız isteğe bağlı alan:
        public string? DisplayName { get; set; }
    }
}
