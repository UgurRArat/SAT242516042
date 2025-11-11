using System.ComponentModel.DataAnnotations;
namespace KutuphaneOtomasyonu_242516042.Models
{
    public class OduncIslemi
    {
        public int Id { get; set; }
        [Required(ErrorMessage = "Üye seçimi zorunludur.")]
        [Range(1, int.MaxValue, ErrorMessage = "Lütfen bir üye seçin.")]
        public int UyeId { get; set; }
        [Required(ErrorMessage = "Kitap seçimi zorunludur.")]
        [Range(1, int.MaxValue, ErrorMessage = "Lütfen bir kitap seçin.")]
        public int KitapId { get; set; }
        public DateTime OduncAlmaTarihi { get; set; }
        [Required(ErrorMessage = "İade tarihi zorunludur.")]
        public DateTime PlanlananIadeTarihi { get; set; } = DateTime.Now.AddDays(14);
        public DateTime? GeriGetirmeTarihi { get; set; }
        public string Durum { get; set; } = "";
        public string KitapAdi { get; set; } = "";
        public string UyeAdi { get; set; } = "";
        public string UyeSoyadi { get; set; } = "";
        public string UyeTamAdi => $"{UyeAdi} {UyeSoyadi}".Trim();
    }
}