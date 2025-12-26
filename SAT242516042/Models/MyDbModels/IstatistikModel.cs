namespace MyDbModels;

public class IstatistikModel
{
    // SP'den dönen kolon adlarıyla aynı olmalı
    public string KategoriAdi { get; set; } = "";
    public int KitapSayisi { get; set; }
}