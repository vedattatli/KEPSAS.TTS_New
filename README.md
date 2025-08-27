# KEPSAS TTS – Talep & Donanım Takip Sistemi

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

## 📦 Mimari ve Geliştirme Ortamı

Bu proje **çoklu VM mimarisi** ile çalışır.  
- **DB-VM (SQL Server)** → Veritabanı barındırır.  
- **APP-VM (ASP.NET Core)** → Uygulamayı barındırır.  
- **CLIENT (tarayıcı / başka VM)** → Web üzerinden uygulamaya bağlanır.  

**Örnek Topoloji:**
- DB-VM → `192.168.1.10`  
- APP-VM → `192.168.1.11`  
- Client → `http://192.168.1.11:5000`

---

## ⚙️ Kurulum

### Gereksinimler
- .NET 7/8 SDK
- Visual Studio 2022 veya Rider
- SQL Server (ayrı VM üzerinde de olabilir)

### 1. DB-VM (SQL Server)
1. SQL Server kur ve dış bağlantıya izin ver.  
2. Statik IP ata (örn. `192.168.1.10`).  
3. `KEPSAS_TTS` isminde boş bir DB oluştur.

### 2. APP-VM (Uygulama)
1. Projeyi klonla:  
   ```bash
   git clone https://github.com/kullanici/KEPSAS.TTS.git
   cd KEPSAS.TTS
appsettings.json içinde ConnectionString’i DB VM IP’sine göre güncelle:

json
Copy code
"DefaultConnection": "Server=192.168.1.10;Database=KEPSAS_TTS;User Id=sa;Password=Parola123;TrustServerCertificate=True"
Migration & DB update çalıştır:

bash
Copy code
dotnet ef database update
Uygulamayı başlat:

bash
Copy code
dotnet run
APP-VM IP’sinden eriş: http://192.168.1.11:5000

3. Client
Tarayıcıdan http://192.168.1.11:5000 adresine git.

Giriş yap:

Admin: admin@kepsas.com / Admin!234

User: demo@kepsas.com / Demo.1234

🐳 Docker (Opsiyonel – Daha Profesyonel)
VM’lerle uğraşmak istemezsen, uygulamayı ve SQL Server’ı Docker Compose ile çalıştırabilirsin.
Bu sayede tek komutla kurulur:

bash
Copy code
docker-compose up
Dockerfile ve docker-compose.yml daha sonra eklenecektir.

📊 Ekran Görselleri
Buraya proje çalışırken alınmış ekran görüntülerini ekleyin.

📄 Lisans
Bu proje MIT lisansı ile yayınlanmıştır.
