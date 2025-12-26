using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace DbContexts; // Hocanın namespace'iyle uyumlu olsun diye buraya koyuyoruz ama dosya yeni.

// Bu sınıf SADECE Identity tablolarını (AspNetUsers vb.) yönetecek.
public class AuthDbContext : IdentityDbContext<IdentityUser>
{
    public AuthDbContext(DbContextOptions<AuthDbContext> options)
        : base(options)
    {
    }
}