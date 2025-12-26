using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using SAT242516042.Components;
using SAT242516042.Components.Account;
using SAT242516042.Data;

using DbContexts;
using UnitOfWorks;
using Providers;

using SAT242516042.Services;
using QuestPDF.Infrastructure; // Namespace ekle

// Lisans ayarý (Ücretsiz sürüm)
QuestPDF.Settings.License = LicenseType.Community;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddRazorComponents()
    .AddInteractiveServerComponents();

builder.Services.AddCascadingAuthenticationState();
builder.Services.AddScoped<IdentityUserAccessor>();
builder.Services.AddScoped<IdentityRedirectManager>();
builder.Services.AddScoped<AuthenticationStateProvider, IdentityRevalidatingAuthenticationStateProvider>();

builder.Services.AddAuthentication(options =>
{
    options.DefaultScheme = IdentityConstants.ApplicationScheme;
    options.DefaultSignInScheme = IdentityConstants.ExternalScheme;
})
    .AddIdentityCookies();

// Veritabaný Baðlantý Cümlesi
var connectionString = builder.Configuration.GetConnectionString("DefaultConnection") ?? throw new InvalidOperationException("Connection string 'DefaultConnection' not found.");

// 1. IDENTITY DBCONTEXT (Login/Register iþlemleri için)
builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseSqlServer(connectionString));

// 2. HOCANIN DBCONTEXT'Ý (Stored Procedure iþlemleri için)
builder.Services.AddDbContext<MyDbModel_DbContext>(options =>
    options.UseSqlServer(connectionString));

// 3. HOCANIN SERVÝSLERÝ (UnitOfWork ve Provider)
builder.Services.AddScoped<IMyDbModel_UnitOfWork, MyDbModel_UnitOfWork<MyDbModel_DbContext>>();
builder.Services.AddScoped<IMyDbModel_Provider, MyDbModel_Provider>();

// 4. LOGLAMA SERVÝSLERÝ
builder.Services.AddScoped<DbLogger>();
builder.Services.AddScoped<FileLogger>();
builder.Services.AddScoped<PdfReportService>();

builder.Services.AddDatabaseDeveloperPageExceptionFilter();

// --- GÜNCELLENEN KISIM BURASI ---
builder.Services.AddIdentityCore<ApplicationUser>(options =>
{
    options.SignIn.RequireConfirmedAccount = false;
    options.Password.RequireDigit = false;
    options.Password.RequiredLength = 3;
    options.Password.RequireNonAlphanumeric = false;
    options.Password.RequireUppercase = false;
    options.Password.RequireLowercase = false;
})
    .AddRoles<IdentityRole>() // <--- KRÝTÝK EKLEME: Rol Servisini Aktif Ettik!
    .AddEntityFrameworkStores<ApplicationDbContext>()
    .AddSignInManager()
    .AddDefaultTokenProviders();
// --------------------------------

builder.Services.AddSingleton<IEmailSender<ApplicationUser>, IdentityNoOpEmailSender>();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseMigrationsEndPoint();
}
else
{
    app.UseExceptionHandler("/Error", createScopeForErrors: true);
    app.UseHsts();
}

app.UseHttpsRedirection();

app.UseStaticFiles();
app.UseAntiforgery();

app.MapRazorComponents<App>()
    .AddInteractiveServerRenderMode();

app.MapAdditionalIdentityEndpoints();

// --- VERÝTABANI BAÞLATICI (SEEDER) ---
using (var scope = app.Services.CreateScope())
{
    var services = scope.ServiceProvider;
    try
    {
        // DbSeeder artýk hata vermeden çalýþacak
        await DbSeeder.Initialize(services);
    }
    catch (Exception ex)
    {
        var logger = services.GetRequiredService<ILogger<Program>>();
        logger.LogError(ex, "Veritabaný baþlatýlýrken (Seeding) bir hata oluþtu.");
    }
}
// ---------------------------------------------

app.Run();