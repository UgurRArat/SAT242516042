using Microsoft.AspNetCore.Identity;
using SAT242516042.Data; // Kendi namespace'ini kontrol et

namespace SAT242516042.Data;

public static class DbSeeder
{
    public static async Task Initialize(IServiceProvider serviceProvider)
    {
        // Rol Yöneticisi ve Kullanıcı Yöneticisi servislerini çağırıyoruz
        var roleManager = serviceProvider.GetRequiredService<RoleManager<IdentityRole>>();
        var userManager = serviceProvider.GetRequiredService<UserManager<ApplicationUser>>();

        // 1. ROLLERİ TANIMLIYORUZ (Diyagramına uygun olarak)
        string[] roleNames = { "Yonetici", "Kutuphaneci", "Uye" };

        foreach (var roleName in roleNames)
        {
            // Eğer rol veritabanında yoksa oluştur
            var roleExist = await roleManager.RoleExistsAsync(roleName);
            if (!roleExist)
            {
                await roleManager.CreateAsync(new IdentityRole(roleName));
            }
        }

        // 2. VARSAYILAN YÖNETİCİ (ADMIN) OLUŞTURMA
        // Projeyi açtığında giriş yapabilmen için hazır bir kullanıcı
        var adminUser = new ApplicationUser
        {
            UserName = "admin@admin.com",
            Email = "admin@admin.com",
            EmailConfirmed = true,
            Ad = "Sistem",
            Soyad = "Yöneticisi",
            AktifMi = true
        };

        // Eğer bu emailde biri yoksa oluştur
        var userCheck = await userManager.FindByEmailAsync("admin@admin.com");
        if (userCheck == null)
        {
            var createPowerUser = await userManager.CreateAsync(adminUser, "123"); // Şifre: 123
            if (createPowerUser.Succeeded)
            {
                // Kullanıcıya 'Yonetici' rolünü ata
                await userManager.AddToRoleAsync(adminUser, "Yonetici");
            }
        }
    }
}