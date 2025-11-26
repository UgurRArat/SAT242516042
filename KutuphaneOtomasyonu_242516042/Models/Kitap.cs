using System.ComponentModel.DataAnnotations;

namespace KutuphaneOtomasyonu_242516042.Models
{
    public class Kitap
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "ISBN alanı zorunludur.")]
        public string ISBN { get; set; } = "";

        [Required(ErrorMessage = "Kitap adı zorunludur.")]
        public string Ad { get; set; } = "";

        public string YayinEvi { get; set; } = "";

        public int? YayinYili { get; set; }

        public string Durum { get; set; } = "Mevcut";

        // NOT: EklemeTarihi'ni SQL prosedüründe kullanmadığımız için sildim.
        // Eğer veritabanında bu sütun varsa ve otomatik doluyorsa burada durabilir ama mapping hatası olmasın diye kaldırdım.

        [Required(ErrorMessage = "Yazar seçimi zorunludur.")]
        [Range(1, int.MaxValue, ErrorMessage = "Lütfen bir yazar seçin.")]
        public int? YazarId { get; set; }

        [Required(ErrorMessage = "Kategori seçimi zorunludur.")]
        [Range(1, int.MaxValue, ErrorMessage = "Lütfen bir kategori seçin.")]
        public int? KategoriId { get; set; }

        // --- SQL PROSEDÜRÜNDEN GELEN EKSTRA BİLGİLER ---

        // SQL'de "YazarTamAdi" olarak gönderdiğimiz için burada set edilebilir özellik yaptık.
        public string? YazarTamAdi { get; set; }

        // SQL'den gelen kategori ismi
        public string? KategoriAdi { get; set; }

        // YENİ: Hoca'nın istediği Parçalı Fonksiyon sonucu (Yeni Çıkan, Klasik vb.)
        public string? BasimTuru { get; set; }
    }
}