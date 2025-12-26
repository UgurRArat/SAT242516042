namespace MyDbModels;

public class Yazar
{
    public int Id { get; set; }
    public string AdSoyad { get; set; } = "";
    public string Biyografi { get; set; } = "";
    public bool AktifMi { get; set; } = true;
    public int TotalRecordCount { get; set; } // Sayfalama için
}