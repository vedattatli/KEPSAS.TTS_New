
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
Tarayıcıdan http://localhost:7237 aç

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

🤝 KATKI
Pull request ve issue açabilirsiniz.

📄 LİSANS
MIT

# İLGİLİ FOTOĞRAFLAR :





<img width="577" height="146" alt="Screenshot 2025-08-27 150034" src="https://github.com/user-attachments/assets/c241e22b-efe5-4921-8f36-ccb3bcc0bbe5" />
<img width="1674" height="831" alt="Screenshot 2025-08-27 145938" src="https://github.com/user-attachments/assets/d686bace-aa7c-4704-a876-0e0304dcd06e" />
<img width="1684" height="835" alt="Screenshot 2025-08-27 145931" src="https://github.com/user-attachments/assets/19737ffb-061c-4c00-9837-8a5662fca01b" />
<img width="1656" height="832" alt="Screenshot 2025-08-27 145924" src="https://github.com/user-attachments/assets/c2f3dd38-5b3b-4a5f-8d26-d7d66cce32a0" />
<img width="1675" height="832" alt="Screenshot 2025-08-27 145910" src="https://github.com/user-attachments/assets/45c53303-265a-4c65-b5f5-f3c2fd8c6e11" />
<img width="1672" height="828" alt="Screenshot 2025-08-27 145903" src="https://github.com/user-attachments/assets/e1bb08b9-c170-4184-a457-d9a36a82c873" />
<img width="1649" height="820" alt="Screenshot 2025-08-27 145856" src="https://github.com/user-attachments/assets/2fa8081d-aabb-44d5-9e45-1f8dae701495" />
<img width="1653" height="822" alt="Screenshot 2025-08-27 145840" src="https://github.com/user-attachments/assets/2ce5c6f7-33f5-4875-99a7-a287f57e3ee1" />
<img width="1628" height="836" alt="Screenshot 2025-08-27 145830" src="https://github.com/user-attachments/assets/c0eaa572-e431-4060-a892-8423e744e1bb" />
<img width="1027" height="522" alt="Screenshot 2025-08-27 145023" src="https://github.com/user-attachments/assets/e5266120-aee6-4076-a463-90c5143f1553" />
<img width="1664" height="853" alt="Screenshot 2025-08-27 144837" src="https://github.com/user-attachments/assets/a4b0e20e-ef0e-44c6-a961-5cac16244929" />
<img width="1671" height="862" alt="Screenshot 2025-08-27 144824" src="https://github.com/user-attachments/assets/6a42dcd7-6e42-4fcd-af79-2b471f01b99d" />
<img width="1682" height="831" alt="Screenshot 2025-08-27 144816" src="https://github.com/user-attachments/assets/1c060334-90f4-4398-961f-786b5a4ea27c" />
<img width="1679" height="861" alt="Screenshot 2025-08-27 144808" src="https://github.com/user-attachments/assets/ce621e7b-75f1-4bcf-9d33-5e2ba04d2ab5" />
<img width="1668" height="830" alt="Screenshot 2025-08-27 144756" src="https://github.com/user-attachments/assets/92b3b26d-c56a-46b2-b3ca-5e140477b8c1" />
<img width="1678" height="867" alt="Screenshot 2025-08-27 144747" src="https://github.com/user-attachments/assets/e3e5e60f-4f06-410b-a091-97173366ecfa" />
<img width="277" height="358" alt="Screenshot 2025-08-27 144030" src="https://github.com/user-attachments/assets/ce0a4aa5-7a83-4626-a7ab-bdcc09f58420" />
