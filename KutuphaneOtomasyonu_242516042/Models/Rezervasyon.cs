using System.ComponentModel.DataAnnotations;

namespace KutuphaneOtomasyonu_242516042.Models
{
    public class Rezervasyon
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "Üye seçimi zorunludur.")]
        public int UyeId { get; set; }

        [Required(ErrorMessage = "Kitap seçimi zorunludur.")]
        public int KitapId { get; set; }

        public DateTime RezervasyonTarihi { get; set; } = DateTime.Now;

        public string Durum { get; set; } = "Aktif"; // Aktif, Tamamlandı, İptal

        // --- SQL Tarafından Doldurulacak Alanlar ---

        public string? KitapAdi { get; set; }

        public string? KitapISBN { get; set; } // Hangi kitap olduğunu anlamak için ekledik

        // SQL prosedüründe (Ad + ' ' + Soyad) yapıp buraya göndereceğiz
        public string? UyeTamAdi { get; set; }
    }
}