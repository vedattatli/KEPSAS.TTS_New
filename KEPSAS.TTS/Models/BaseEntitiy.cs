// KEPSAS.TTS/Models/BaseEntity.cs

using System.ComponentModel.DataAnnotations;

namespace KEPSAS.TTS.Models
{
    public abstract class BaseEntity
    {
        [Key]
        public int Id { get; set; }

        public DateTime OlusturmaTarihi { get; set; }

        // Hata veren 'GuncellemeTarihi' alanı, kodunuzla uyumlu olması için 'SonIslemTarihi' olarak değiştirildi.
        public DateTime? SonIslemTarihi { get; set; }

        public bool SilindiMi { get; set; } = false;

        public bool AktifMi { get; set; } = true;
    }
}