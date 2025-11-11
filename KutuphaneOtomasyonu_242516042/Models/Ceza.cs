namespace KutuphaneOtomasyonu_242516042.Models
{
    public class Ceza
    {
        public int Id { get; set; }
        public int UyeId { get; set; }
        public int OduncIslemiId { get; set; }
        public DateTime CezaBaslangicTarihi { get; set; }
        public DateTime CezaBitisTarihi { get; set; }
        public int GecikenGunSayisi { get; set; }
        public decimal CezaTutari { get; set; }
        public string? Aciklama { get; set; }
        public bool Odendi { get; set; }
        public string UyeAdi { get; set; } = "";
        public string UyeSoyadi { get; set; } = "";
        public string UyeTamAdi => $"{UyeAdi} {UyeSoyadi}".Trim();
    }
}