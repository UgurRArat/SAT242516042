using KutuphaneOtomasyonu_242516042.Models;
using System.Data;
using System.Data.SqlClient;
using Microsoft.Extensions.Configuration;

namespace KutuphaneOtomasyonu_242516042.Data
{
    public class KutuphaneServisi
    {
        private readonly string _connectionString;

        public KutuphaneServisi(IConfiguration configuration)
        {
            _connectionString = configuration.GetConnectionString("DefaultConnection")
                ?? throw new ArgumentNullException("Connection string 'DefaultConnection' not found.");
        }

        // --- GENEL YARDIMCI METOTLAR ---
        private async Task<List<T>> GetListFromSp<T>(string spName, List<SqlParameter>? parameters = null) where T : new()
        {
            var list = new List<T>();
            using (var con = new SqlConnection(_connectionString))
            {
                var cmd = new SqlCommand(spName, con) { CommandType = CommandType.StoredProcedure };
                if (parameters != null)
                {
                    cmd.Parameters.AddRange(parameters.ToArray());
                }
                await con.OpenAsync();
                using (var reader = await cmd.ExecuteReaderAsync())
                {
                    while (await reader.ReadAsync())
                    {
                        T item = new T();
                        for (int i = 0; i < reader.FieldCount; i++)
                        {
                            var prop = typeof(T).GetProperty(reader.GetName(i));
                            if (prop != null && !reader.IsDBNull(i))
                            {
                                prop.SetValue(item, reader.GetValue(i));
                            }
                        }
                        list.Add(item);
                    }
                }
            }
            return list;
        }

        private async Task ExecuteSp(string spName, List<SqlParameter> parameters)
        {
            using (var con = new SqlConnection(_connectionString))
            {
                var cmd = new SqlCommand(spName, con) { CommandType = CommandType.StoredProcedure };
                if (parameters != null)
                {
                    cmd.Parameters.AddRange(parameters.ToArray());
                }
                await con.OpenAsync();
                await cmd.ExecuteNonQueryAsync();
            }
        }

        // ============================================================
        // KİTAP İŞLEMLERİ
        // ============================================================
        public async Task<List<Kitap>> KitapListeleAsync()
        {
            var parameters = new List<SqlParameter> { new SqlParameter("@Islem", "select") };
            return await GetListFromSp<Kitap>("sp_Kitap_Yonet", parameters);
        }

        public async Task KitapKaydetAsync(Kitap kitap, string operation)
        {
            var parameters = new List<SqlParameter>
            {
                new SqlParameter("@Islem", operation),
                new SqlParameter("@Id", kitap.Id),
                new SqlParameter("@ISBN", kitap.ISBN),
                new SqlParameter("@Ad", kitap.Ad),
                new SqlParameter("@YayinEvi", (object)kitap.YayinEvi ?? DBNull.Value),
                new SqlParameter("@YayinYili", (object)kitap.YayinYili ?? DBNull.Value),
                new SqlParameter("@Durum", "Mevcut"),
                new SqlParameter("@YazarId", kitap.YazarId),
                new SqlParameter("@KategoriId", kitap.KategoriId)
            };
            await ExecuteSp("sp_Kitap_Yonet", parameters);
        }

        public async Task KitapSilAsync(int id)
        {
            var parameters = new List<SqlParameter> { new SqlParameter("@Islem", "delete"), new SqlParameter("@Id", id) };
            await ExecuteSp("sp_Kitap_Yonet", parameters);
        }

        // ============================================================
        // ÜYE İŞLEMLERİ
        // ============================================================
        public async Task<List<Uye>> UyeListeleAsync()
        {
            var parameters = new List<SqlParameter> { new SqlParameter("@Islem", "select") };
            return await GetListFromSp<Uye>("sp_Uye_Yonet", parameters);
        }

        public async Task UyeKaydetAsync(Uye uye, string operation)
        {
            var parameters = new List<SqlParameter>
            {
                new SqlParameter("@Islem", operation),
                new SqlParameter("@Id", uye.Id),
                new SqlParameter("@Ad", uye.Ad),
                new SqlParameter("@Soyad", uye.Soyad),
                new SqlParameter("@Eposta", uye.Email),
                new SqlParameter("@Telefon", (object)uye.Telefon ?? DBNull.Value),
                new SqlParameter("@CezaPuani", 0)
            };
            await ExecuteSp("sp_Uye_Yonet", parameters);
        }

        public async Task UyeSilAsync(int id)
        {
            var parameters = new List<SqlParameter> { new SqlParameter("@Islem", "delete"), new SqlParameter("@Id", id) };
            await ExecuteSp("sp_Uye_Yonet", parameters);
        }

        // ============================================================
        // ÖDÜNÇ / İADE İŞLEMLERİ (Eksik Olanlar Bunlardı)
        // ============================================================
        public async Task<List<OduncIslemi>> GetAktifOduncListesiAsync()
        {
            return await GetListFromSp<OduncIslemi>("sp_Odunc_Listele_Aktif");
        }

        public async Task<List<Kitap>> GetMusaitKitapListesiAsync()
        {
            return await GetListFromSp<Kitap>("sp_Kitap_Listele_Musait");
        }

        public async Task OduncVerAsync(int uyeId, int kitapId, DateTime iadeTarihi)
        {
            var parameters = new List<SqlParameter>
            {
                new SqlParameter("@UyeId", uyeId),
                new SqlParameter("@KitapId", kitapId),
                new SqlParameter("@IadeTarihi", iadeTarihi)
            };
            await ExecuteSp("sp_Odunc_Ver", parameters);
        }

        public async Task IadeAlAsync(int oduncId)
        {
            var parameters = new List<SqlParameter> { new SqlParameter("@OduncId", oduncId) };
            await ExecuteSp("sp_Iade_Al", parameters);
        }

        // ============================================================
        // CEZA İŞLEMLERİ (Eksik Olanlar)
        // ============================================================
        public async Task<List<Ceza>> GetOdenmemisCezalarAsync()
        {
            // Ceza listeleme için sp_Ceza_Yonet prosedürünü 'select' ile çağırıyoruz
            var parameters = new List<SqlParameter> { new SqlParameter("@Islem", "select") };
            // Not: İleride sadece ödenmemişleri getirmek istersen SQL'e WHERE Odendi=0 eklersin.
            // Şimdilik hepsi gelir.
            return await GetListFromSp<Ceza>("sp_Ceza_Yonet", parameters);
        }

        public async Task CezaOdeAsync(int cezaId)
        {
            var parameters = new List<SqlParameter>
            {
                new SqlParameter("@Islem", "ode"),
                new SqlParameter("@Id", cezaId)
            };
            await ExecuteSp("sp_Ceza_Yonet", parameters);
        }

        // ============================================================
        // REZERVASYON İŞLEMLERİ (Eksik Olanlar)
        // ============================================================
        public async Task<List<Rezervasyon>> GetAktifRezervasyonlarAsync()
        {
            return await GetListFromSp<Rezervasyon>("sp_Rezervasyon_Listele_Aktif");
        }

        public async Task RezervasyonYapAsync(int uyeId, int kitapId)
        {
            var parameters = new List<SqlParameter>
            {
                new SqlParameter("@UyeId", uyeId),
                new SqlParameter("@KitapId", kitapId)
            };
            await ExecuteSp("sp_Rezervasyon_Yap", parameters);
        }

        public async Task RezervasyonIptalAsync(int rezervasyonId)
        {
            var parameters = new List<SqlParameter> { new SqlParameter("@RezervasyonId", rezervasyonId) };
            await ExecuteSp("sp_Rezervasyon_Iptal", parameters);
        }

        // ============================================================
        // YAZAR VE KATEGORİ İŞLEMLERİ
        // ============================================================
        public async Task<List<Yazar>> YazarListeleAsync()
        {
            var parameters = new List<SqlParameter> { new SqlParameter("@Islem", "select") };
            return await GetListFromSp<Yazar>("sp_Yazar_Yonet", parameters);
        }

        public async Task YazarKaydetAsync(Yazar yazar, string operation)
        {
            var parameters = new List<SqlParameter>
            {
                new SqlParameter("@Islem", operation),
                new SqlParameter("@Id", yazar.Id),
                new SqlParameter("@Ad", yazar.Ad),
                new SqlParameter("@Soyad", yazar.Soyad)
            };
            await ExecuteSp("sp_Yazar_Yonet", parameters);
        }

        public async Task YazarSilAsync(int id)
        {
            var parameters = new List<SqlParameter> { new SqlParameter("@Islem", "delete"), new SqlParameter("@Id", id) };
            await ExecuteSp("sp_Yazar_Yonet", parameters);
        }

        public async Task<List<Kategori>> KategoriListeleAsync()
        {
            var parameters = new List<SqlParameter> { new SqlParameter("@Islem", "select") };
            return await GetListFromSp<Kategori>("sp_Kategori_Yonet", parameters);
        }

        public async Task KategoriKaydetAsync(Kategori kategori, string operation)
        {
            var parameters = new List<SqlParameter>
            {
                new SqlParameter("@Islem", operation),
                new SqlParameter("@Id", kategori.Id),
                new SqlParameter("@Ad", kategori.Ad)
            };
            await ExecuteSp("sp_Kategori_Yonet", parameters);
        }

        public async Task KategoriSilAsync(int id)
        {
            var parameters = new List<SqlParameter> { new SqlParameter("@Islem", "delete"), new SqlParameter("@Id", id) };
            await ExecuteSp("sp_Kategori_Yonet", parameters);
        }
    }
}