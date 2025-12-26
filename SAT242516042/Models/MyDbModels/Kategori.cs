namespace MyDbModels; // Senin namespace'ine dikkat et

public class Kategori
{
    public int Id { get; set; }
    public string Ad { get; set; } = "";
    public string Aciklama { get; set; } = "";
    public bool AktifMi { get; set; } = true;

    // Grid'de göstermek için toplam kayıt sayısını tutacak (SP'den gelecek)
    public int TotalRecordCount { get; set; }
}