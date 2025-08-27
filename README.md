# 🚀 KEPSAS TTS – Kurumsal Talep ve Donanım Takip Sistemi

<p align="center">
<img src="https://img.shields.io/badge/.NET-8-blueviolet.svg" alt=".NET 8">
<img src="https://img.shields.io/badge/Platform-ASP.NET%20Core%20MVC-blue.svg" alt="ASP.NET Core MVC">
<img src="https://img.shields.io/badge/License-MIT-green.svg" alt="License">
<img src="https://img.shields.io/badge/Status-Geliştiriliyor-orange.svg" alt="Status">
</p>

Bu proje, **modern bir ASP.NET Core uygulaması** ile bu uygulamanın üzerinde çalıştığı **kurumsal ağ altyapısının birleşiminden** oluşur.  
Hem yazılım geliştiriciler hem de sistem & ağ yöneticileri için kapsamlı bir kaynak niteliğindedir.  

---

# 🖥️ Yazılım Katmanı (KEPSAS TTS Uygulaması)

### 🚀 TEMEL ÖZELLİKLER
- **Kimlik ve Yetki Yönetimi**
  - ASP.NET Core Identity ile güvenli giriş/çıkış
  - Admin & User rolleri
- **Talep Yönetimi**
  - Donanım / Yazılım talepleri
  - Atama ve durum akışı (Yeni → Onay → Tamamlandı → İptal)
- **Donanım Envanteri**
  - Donanım kayıt/zimmet
  - Durumlar: Stokta, Kullanımda, Arızalı, Hurda
- **Raporlama**
  - Açık/Kapalı talep dağılımı
  - MTTR (ortalama çözüm süresi)
  - Günlük trend ve CSV export
- **Dashboard**
  - Kullanıcıya özel panel
  - Admin için genişletilmiş rapor ekranı

---

### 🛠️ KULLANILAN TEKNOLOJİLER

| Kategori          | Teknoloji                 | Açıklama                          |
|-------------------|---------------------------|-----------------------------------|
| Backend           | .NET 8 / ASP.NET Core MVC | Güçlü, modern web altyapısı       |
| ORM               | Entity Framework Core 8   | Code-First, migration yönetimi    |
| Database          | MS SQL Server             | Güvenilir ilişkisel veritabanı    |
| Kimlik Yönetimi   | ASP.NET Identity          | Kullanıcı/rol yönetimi            |
| Frontend          | Razor Views + Bootstrap 5 | Responsive arayüzler              |

---

### ⚙️ HIZLI KURULUM (YAZILIM)

1. Projeyi klonla  
   ```bash
   git clone https://github.com/kullanici/KEPSAS.TTS.git
   cd KEPSAS.TTS
appsettings.json → kendi SQL Server bilgini yaz

Migration uygula

bash
Copy code
dotnet ef database update
Çalıştır

bash
Copy code
dotnet run
Tarayıcıdan http://localhost:5000 aç

Varsayılan Kullanıcılar:

Admin → admin@kepsas.com / Admin!234

User → demo@kepsas.com / Demo.1234

🔒 Donanım & Ağ Katmanı (Laboratuvar)
Yazılımın gerçekçi ortamda denenmesi için pfSense firewall ve çoklu VM mimarisi kullanılmıştır.

🌐 PFsense IP PLANLAMASI
Interface	IP/Subnet	Açıklama
WAN	192.168.100.128/24	Dış ağ
LAN	172.16.22.1/24	Genel LAN
DC_AGI	172.16.20.1/24	Domain Controller
DHCP_AGI	172.16.21.1/24	DHCP
APP_AGI	172.16.23.1/24	Uygulama Sunucusu
DB_AGI	172.16.24.1/24	SQL Server
BACKUP_AGI	172.16.25.1/24	Backup Sunucusu

🖥️ SUNUCU ROLLERİ
Rol	Hostname	Ağ/Subnet	Görev
Domain Controller	dc01	172.16.20.x	AD DS + DNS
DHCP Sunucusu	dhcp01	172.16.21.x	DHCP dağıtımı
Uygulama Sunucusu	app01	172.16.23.x	ASP.NET Core uygulama
DB Sunucusu	db01	172.16.24.x	SQL Server
Backup Sunucusu	backup01	172.16.25.x	Yedekleme
Client Admin	client-admin	DHCP (LAN)	Yönetici istemci
Client Standart	client-std	DHCP (LAN)	Kullanıcı istemci

🛡️ FIREWALL KURALLARI
✅ İzinli Trafik

Client → APP: TCP 80/443/5000

APP → DB: TCP 1433

Tüm ağlar → DC: DNS (53)

Backup → APP/DB: yedekleme

❌ Engelli Trafik

Client → DB: Direkt erişim yok

Gereksiz internet çıkışları kısıtlı

🔄 TRAFİK AKIŞI
Client → APP → DB (talep işlemleri)

APP ↔ DC (kimlik doğrulama + DNS)

Backup → APP/DB (yedekleme)

🗺️ TOPOLOJİ (Mermaid)
mermaid
Copy code
graph TD
    subgraph "WAN"
        W[🌐 WAN 192.168.100.128/24]
    end
    
    subgraph "pfSense Firewall"
        F[🔥 pfSense]
    end
    
    subgraph "Ağlar"
        L[💻 LAN 172.16.22.1/24]
        D[🆔 DC_AGI 172.16.20.1/24]
        H[🔢 DHCP_AGI 172.16.21.1/24]
        A[🚀 APP_AGI 172.16.23.1/24]
        B[🗃️ DB_AGI 172.16.24.1/24]
        K[💾 BACKUP_AGI 172.16.25.1/24]
    end
    
    subgraph "Sunucular"
        D1[DC Server]
        H1[DHCP Server]
        A1[APP Server]
        B1[DB Server]
        K1[Backup Server]
    end
    
    subgraph "İstemciler"
        L1[Client Admin]
        L2[Client Standart]
    end
    
    W --> F
    F --> L & D & H & A & B & K
    D --> D1
    H --> H1
    A --> A1
    B --> B1
    K --> K1
    L --> L1 & L2
    L1 & L2 --> A1
    A1 --> B1
    A1 --> D1
    K1 --> A1 & B1
📊 GELECEK YOL HARİTASI
 Docker Compose desteği (App + DB tek komutla)

 Bildirim sistemi (e-posta)

 Birim ve entegrasyon testleri

 SLA ve gelişmiş raporlar

🤝 KATKI
Pull request ve issue açabilirsiniz.

📄 LİSANS
MIT
