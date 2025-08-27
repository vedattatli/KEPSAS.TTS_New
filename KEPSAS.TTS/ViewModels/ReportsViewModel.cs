// ViewModels/ReportsViewModels.cs
using System;
using System.Collections.Generic;

namespace KEPSAS.TTS.ViewModels
{
    public class ReportsFilterVm
    {
        public DateTime? From { get; set; }
        public DateTime? To { get; set; }

        public string? CreatorId { get; set; }
        public string? AssigneeId { get; set; }
        public string? Durum { get; set; }
    }

    public class StatusCountVm
    {
        public string Durum { get; set; } = "-";
        public int Adet { get; set; }
    }

    public class TrendPointVm
    {
        public DateTime Tarih { get; set; }
        public int Yeni { get; set; }          // o gün açılan
        public int Kapanan { get; set; }       // o gün tamamlanan
        public int Backlog { get; set; }       // o güne kadar açık kalan
    }

    public class TopItemVm
    {
        public string Ad { get; set; } = "-";
        public int Adet { get; set; }
    }

    public class AgingBucketVm
    {
        public string Aralik { get; set; } = "-";
        public int Adet { get; set; }
    }

    public class ReportsDashboardVm
    {
        public ReportsFilterVm Filters { get; set; } = new();

        // Özet kartlar
        public int Toplam { get; set; }
        public int Acik { get; set; }          // Durum != Tamamlandı/Onaylı
        public int Kapanan { get; set; }       // Durum == Tamamlandı/Onaylı aralığa göre
        public double? MTTR_Saat { get; set; } // Mean Time To Resolve (saat)

        // Grafik/tablolar
        public List<StatusCountVm> DurumDagilimi { get; set; } = new();
        public List<TrendPointVm> GunlukTrend { get; set; } = new();
        public List<TopItemVm> EnCokTalepAcanlar { get; set; } = new();
        public List<TopItemVm> EnCokAtananlar { get; set; } = new();
        public List<AgingBucketVm> YasDagilimi { get; set; } = new();
    }
}
