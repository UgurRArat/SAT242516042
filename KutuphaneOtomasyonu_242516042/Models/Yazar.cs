namespace KutuphaneOtomasyonu_242516042.Models
{
    public class Yazar
    {
        public int Id { get; set; }
        public string Ad { get; set; } = "";
        public string Soyad { get; set; } = "";

        // SQL prosedüründe (Ad + ' ' + Soyad) birleştirilip 'TamAd' olarak gönderiliyor.
        // Veriyi karşılayabilmek için computed (=>) yerine property ({ get; set; }) yaptık.
        public string? TamAd { get; set; }

        // SQL tablosundaki Durum (Mevcut/Silindi) alanı
        public string Durum { get; set; } = "Mevcut";

        // SQL tablomuzda 'Ulke' sütunu oluşturmadığımız için hata vermesin diye kapatıyorum.
        // public string Ulke { get; set; } = "";
    }
}