namespace KutuphaneOtomasyonu_242516042.Models
{
    public class Kategori
    {
        public int Id { get; set; }
        public string Ad { get; set; } = "";

        // SQL tablosunda 'Durum' sütunu var, buraya ekledik.
        public string Durum { get; set; } = "Mevcut";

        // SQL tablomuzda 'Aciklama' sütunu olmadığı için kapattım.
        // public string Aciklama { get; set; } = "";
    }
}