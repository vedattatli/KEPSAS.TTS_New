using KEPSAS.TTS.Data;

namespace KEPSAS.TTS.Models
{
    public class Talep
    {
        public int Id { get; set; }

        // Talep adı = Baslik (mevcut)
        public string Baslik { get; set; } = "";
        public string Aciklama { get; set; } = "";

        // Yeni alanlar
        public TalepTipi Tip { get; set; } = TalepTipi.Diger;

        // Donanım ilişkisi (Tip=Donanim ise)
        public int? DonanimId { get; set; }
        public Donanim? Donanim { get; set; }

        // Yazılım adı (Tip=Yazilim ise)
        public string? YazilimAdi { get; set; }

        // Talebi etkileyen kişi (kimin siciline/kaydı adına açıldı)
        public string? HedefKullaniciId { get; set; }
        public ApplicationUser? HedefKullanici { get; set; }

        // İsteğe bağlı: harici sicil numarası
        public string? HedefSicilNo { get; set; }

        // Kaydı açanın IP adresi
        public string? IpAdresi { get; set; }

        // Durumlar: "Yeni", "Üzerimde", "Onaylı", "Tamamlandı", "İptal" vb.
        public string Durum { get; set; } = "Beklemede";

        public DateTime OlusturmaTarihi { get; set; } = DateTime.UtcNow;

        // Açan / Atanan
        public string? OlusturanKullaniciId { get; set; }
        public ApplicationUser? OlusturanKullanici { get; set; }

        public string? AtananKullaniciId { get; set; }
        public ApplicationUser? AtananKullanici { get; set; }
        public DateTime? AtamaTarihi { get; set; }
        public DateTime? SonIslemTarihi { get; set; }

        // İptal bilgisi
        public string? IptalNedeni { get; set; }

        public ICollection<TalepEk>? Ekler { get; set; }
        public ICollection<TalepLog>? Loglar { get; set; }
    }
}
