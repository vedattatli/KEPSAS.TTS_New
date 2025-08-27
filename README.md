# KEPSAS TTS – Talep ve Donanım Takip Sistemi

Kurumsal IT destek süreçlerini yönetmek için geliştirilmiş **Talep ve Donanım Takip Sistemi**.  
ASP.NET Core MVC, Entity Framework Core ve Identity tabanlı kullanıcı/rol yönetimi içerir.

---

## 🚀 Özellikler
- **Kullanıcı Yönetimi**
  - Identity tabanlı kimlik doğrulama
  - Admin / User rol yönetimi
  - Kullanıcı oluşturma, rol değiştirme

- **Talep Yönetimi**
  - Talep oluşturma (başlık, açıklama, tip, donanım/yazılım seçimi)
  - Atama (Admin tarafından kullanıcıya yönlendirme)
  - Durum güncellemeleri (Yeni, Üzerimde, Onaylı, Tamamlandı, İptal vb.)
  - Talep logları (durum/atama geçmişi)
  - Dosya ekleri (TalepEk tablosu)

- **Raporlama**
  - Açık / Kapalı talepler dağılımı
  - Günlük trend (açılan, kapanan, backlog)
  - MTTR (ortalama çözüm süresi)
  - En çok talep açan / atanan kullanıcılar
  - CSV export özelliği

- **Donanım Yönetimi**
  - Envanter kaydı (bilgisayar, yazıcı, switch, vb.)
  - Kullanıcıya atama
  - Durum yönetimi (Stokta, Kullanımda, Arızalı, Hurda)
  - Kullanıcının kendi donanımlarını görmesi

- **Dashboard**
  - Kullanıcıya özel talep özetleri
  - Admin için ayrı dashboard paneli

---

## 🛠️ Teknolojiler
- .NET 7 / ASP.NET Core MVC
- Entity Framework Core (Code First, Migrations)
- Identity (Kullanıcı ve Rol yönetimi)
- MS SQL Server
- Bootstrap 5 + Razor Views

---

## 📦 Kurulum
1. Projeyi klonla:
   ```bash
   git clone https://github.com/kullanici/KEPSAS.TTS.git
   cd KEPSAS.TTS
appsettings.json içinde ConnectionString ayarını kendi SQL Server bilgine göre düzenle.
👉 Öneri: appsettings.json yerine User Secrets veya Environment Variables kullan.

Migration ve database update yap:

bash
Copy code
dotnet ef database update
Projeyi çalıştır:

bash
Copy code
dotnet run
Varsayılan kullanıcılar:

Admin: admin@kepsas.com / Admin!234

Demo User: demo@kepsas.com / Demo.1234

📊 Ekran Görselleri
Buraya proje çalışırken alınmış ekran görüntülerini ekleyebilirsiniz.

📄 Lisans
Bu proje MIT lisansı ile yayınlanmıştır.
