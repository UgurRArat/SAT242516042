namespace MyDbModels;

public class Kitap
{
    public int Id { get; set; }
    public string ISBN { get; set; } = "";
    public string Baslik { get; set; } = "";

    // İlişkiler
    public int YazarId { get; set; }
    public int KategoriId { get; set; }

    // Listeleme Ekranında Görünmesi İçin (Veritabanında yok ama SP'den dönecek)
    public string YazarAdi { get; set; } = "";
    public string KategoriAdi { get; set; } = "";

    public string YayinEvi { get; set; } = "";
    public int SayfaSayisi { get; set; }
    public int StokAdeti { get; set; }
    public bool AktifMi { get; set; } = true;
    public int TotalRecordCount { get; set; }
}