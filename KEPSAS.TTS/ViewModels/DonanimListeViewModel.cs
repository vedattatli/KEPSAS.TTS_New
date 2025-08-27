using System.Collections.Generic;

// Projenizin ad alanına göre güncelleyin
namespace KEPSAS.TTS.ViewModels
{
    // Listede gösterilecek tek bir donanımın modeli
    public class DonanimOzetViewModel
    {
        public int Id { get; set; }
        public string SeriNo { get; set; }
        public string Model { get; set; }
        public string Kategori { get; set; }
        public string AtananKullanici { get; set; }
        public string Durum { get; set; } // Kullanımda, Stokta vb.
    }

    // Donanım listesi sayfasının ana modeli
    public class DonanimListeViewModel
    {
        public List<DonanimOzetViewModel> Donanimlar { get; set; }

        public DonanimListeViewModel()
        {
            Donanimlar = new List<DonanimOzetViewModel>();
        }
    }
}
