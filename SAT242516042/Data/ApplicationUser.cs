using Microsoft.AspNetCore.Identity;

namespace SAT242516042.Data; // Buradaki namespace senin projendekiyle ayný kalmalý

// Ad, Soyad ve AktifMi özelliklerini buraya ekliyoruz
public class ApplicationUser : IdentityUser
{
    public string Ad { get; set; } = "";
    public string Soyad { get; set; } = "";
    public bool AktifMi { get; set; } = true;
    public DateTime KayitTarihi { get; set; } = DateTime.Now;
}