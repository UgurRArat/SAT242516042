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
        [StringLength(11, MinimumLength = 11, ErrorMessage = "TC Kimlik 11 haneli olmalıdır.")]
        public string? TCKimlik { get; set; }
        public string? Adres { get; set; }
        public DateTime UyelikTarihi { get; set; }
        public bool Aktif { get; set; } = true;
        public string TamAd => $"{Ad} {Soyad}";
    }
}