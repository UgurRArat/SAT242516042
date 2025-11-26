using System.ComponentModel.DataAnnotations;

namespace KutuphaneOtomasyonu_242516042.Models
{
    public class Uye
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "Ad alanı zorunludur.")]
        public string Ad { get; set; } = "";

        [Required(ErrorMessage = "Soyad alanı zorunludur.")]
        public string Soyad { get; set; } = "";

        [Required(ErrorMessage = "Email alanı zorunludur.")]
        [EmailAddress(ErrorMessage = "Geçerli bir email adresi girin.")]
        public string Email { get; set; } = "";

        public string? Telefon { get; set; }

        // --- SQL İLE EŞLEŞTİRMEK İÇİN EKLENENLER ---

        // SQL tablomuzda CezaPuani sütunu var, buraya ekledik.
        public int CezaPuani { get; set; } = 0;

        // SQL'de 'Durum' (Mevcut/Silindi) tutuyoruz. 'Aktif' bool yerine bunu kullanacağız.
        public string Durum { get; set; } = "Mevcut";

        // SQL prosedüründen (Ad + ' ' + Soyad) birleşmiş olarak gelecek.
        // Bu yüzden hesaplanan alan '=>' yerine veri taşıyan '{ get; set; }' yaptık.
        public string? TamAd { get; set; }

        // --- SQL TABLOSUNDA OLMAYANLAR (GEÇİCİ OLARAK KAPATILDI) ---
        // Not: Eğer bunları kullanmak istersen SQL tablosuna ve sp_Uye_Yonet'e bu sütunları eklememiz gerekir.
        // Şimdilik sistemin patlamaması için kapalı duruyorlar.

        // public string? TCKimlik { get; set; }
        // public string? Adres { get; set; }
        // public DateTime UyelikTarihi { get; set; }
    }
}