using Microsoft.AspNetCore.Identity;

namespace MyDbModels;

// IdentityUser sınıfından miras alarak standart Identity özelliklerini (UserName, Email, PasswordHash vb.) kazanıyoruz.
public class ApplicationUser : IdentityUser
{
    // Standart Identity tablolarında olmayan ama senin projen için gereken ekstra alanlar:
    public string Ad { get; set; } = string.Empty;
    public string Soyad { get; set; } = string.Empty;

    // SQL'deki 'AktifMi' alanına karşılık
    public bool AktifMi { get; set; } = true;

    // Kayıt tarihi
    public DateTime KayitTarihi { get; set; } = DateTime.Now;
}