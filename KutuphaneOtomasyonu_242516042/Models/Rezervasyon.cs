namespace KutuphaneOtomasyonu_242516042.Models
{
    public class Rezervasyon
    {
        public int Id { get; set; }
        public int UyeId { get; set; }
        public int KitapId { get; set; }
        public DateTime RezervasyonTarihi { get; set; }
        public string Durum { get; set; } = "";
        public string KitapAdi { get; set; } = "";
        public string UyeAdi { get; set; } = "";
        public string UyeSoyadi { get; set; } = "";
        public string UyeTamAdi => $"{UyeAdi} {UyeSoyadi}".Trim();
    }
}