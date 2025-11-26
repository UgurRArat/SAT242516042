using System.ComponentModel.DataAnnotations;

namespace KutuphaneOtomasyonu_242516042.Models
{
    public class Ceza
    {
        public int Id { get; set; }

        public int UyeId { get; set; }

        public int OduncIslemiId { get; set; }

        public DateTime CezaBaslangicTarihi { get; set; }

        public DateTime CezaBitisTarihi { get; set; } // Bitiş tarihi genelde iade tarihidir

        public int GecikenGunSayisi { get; set; }

        public decimal CezaTutari { get; set; }

        public string? Aciklama { get; set; }

        public bool Odendi { get; set; }

        // --- SQL Tarafından Doldurulacak Alanlar ---

        public string? UyeTamAdi { get; set; } // SQL'de Ad + Soyad birleşip gelecek

        public string? KitapAdi { get; set; } // Hangi kitaptan ceza yemiş?
    }
}