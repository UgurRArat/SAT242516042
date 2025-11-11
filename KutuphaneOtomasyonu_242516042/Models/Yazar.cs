namespace KutuphaneOtomasyonu_242516042.Models
{
    public class Yazar
    {
        public int Id { get; set; }
        public string Ad { get; set; } = "";
        public string Soyad { get; set; } = "";
        public string Ulke { get; set; } = "";
        public string TamAd => $"{Ad} {Soyad}";
    }
}