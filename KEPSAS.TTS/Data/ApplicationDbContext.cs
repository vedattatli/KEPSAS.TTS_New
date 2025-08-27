using KEPSAS.TTS.Models;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace KEPSAS.TTS.Data
{
    public class ApplicationDbContext : IdentityDbContext<ApplicationUser>
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options) { }

        public DbSet<Talep> Talepler => Set<Talep>();
        public DbSet<TalepEk> TalepEkler => Set<TalepEk>();
        public DbSet<TalepLog> TalepLoglar => Set<TalepLog>();
        public DbSet<Donanim> Donanimlar => Set<Donanim>();

        protected override void OnModelCreating(ModelBuilder b)
        {
            base.OnModelCreating(b);

            b.Entity<TalepEk>()
             .HasOne(e => e.Talep)
             .WithMany(t => t.Ekler)
             .HasForeignKey(e => e.TalepId)
             .OnDelete(DeleteBehavior.Cascade);

            b.Entity<TalepLog>()
             .HasOne(l => l.Talep)
             .WithMany(t => t.Loglar)
             .HasForeignKey(l => l.TalepId)
             .OnDelete(DeleteBehavior.Cascade);
            b.Entity<Donanim>()
             .HasOne(d => d.AtananKullanici)
             .WithMany()
             .HasForeignKey(d => d.AtananKullaniciId)
             .OnDelete(DeleteBehavior.SetNull);

            // performans indexleri
            b.Entity<Talep>().HasIndex(t => t.Durum);
            b.Entity<Talep>().HasIndex(t => t.OlusturmaTarihi);
            b.Entity<Talep>().HasIndex(t => t.AtananKullaniciId);
            b.Entity<Donanim>().ToTable("Donanimlar");
        }
    }
}
