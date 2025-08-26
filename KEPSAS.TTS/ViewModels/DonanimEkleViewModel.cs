using System.ComponentModel.DataAnnotations;

namespace KEPSAS.TTS.ViewModels
{
    public class DonanimEkleViewModel
    {
        [Required(ErrorMessage = "Seri numarası zorunludur.")]
        [Display(Name = "Seri Numarası")]
        public string? SeriNo { get; set; }

        [Required(ErrorMessage = "Model bilgisi zorunludur.")]
        [Display(Name = "Marka ve Model")]
        public string? Model { get; set; }

        [Required(ErrorMessage = "Kategori seçimi zorunludur.")]
        [Display(Name = "Kategori")]
        public string? Kategori { get; set; }

        [Display(Name = "Alınma Tarihi")]
        [DataType(DataType.Date)]
        public DateTime? AlinmaTarihi { get; set; }
    }
}
