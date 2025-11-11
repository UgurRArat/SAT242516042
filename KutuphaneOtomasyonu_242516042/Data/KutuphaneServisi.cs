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
                var reader = await cmd.ExecuteReaderAsync();
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
            return list;
        }

        private async Task ExecuteSp(string spName, List<SqlParameter> parameters)
        {
            using (var con = new SqlConnection(_connectionString))
            {
                var cmd = new SqlCommand(spName, con) { CommandType = CommandType.StoredProcedure };
                cmd.Parameters.AddRange(parameters.ToArray());
                await con.OpenAsync();
                await cmd.ExecuteNonQueryAsync();
            }
        }

        // --- KİTAP METOTLARI ---
        public async Task<List<Kitap>> KitapListeleAsync()
        {
            var parameters = new List<SqlParameter> { new SqlParameter("@Operation", "list") };
            return await GetListFromSp<Kitap>("sp_Kitap_Yonet", parameters);
        }
        public async Task KitapKaydetAsync(Kitap kitap, string operation)
        {
            var parameters = new List<SqlParameter>
            {
                new SqlParameter("@Operation", operation), new SqlParameter("@Id", kitap.Id),
                new SqlParameter("@ISBN", kitap.ISBN), new SqlParameter("@Ad", kitap.Ad),
                new SqlParameter("@YayinEvi", (object)kitap.YayinEvi ?? DBNull.Value),
                new SqlParameter("@YayinYili", (object)kitap.YayinYili ?? DBNull.Value),
                new SqlParameter("@Durum", kitap.Durum), new SqlParameter("@YazarId", kitap.YazarId),
                new SqlParameter("@KategoriId", kitap.KategoriId)
            };
            await ExecuteSp("sp_Kitap_Yonet", parameters);
        }
        public async Task KitapSilAsync(int id)
        {
            var parameters = new List<SqlParameter> { new SqlParameter("@Operation", "delete"), new SqlParameter("@Id", id) };
            await ExecuteSp("sp_Kitap_Yonet", parameters);
        }

        // --- ÜYE METOTLARI ---
        public async Task<List<Uye>> UyeListeleAsync()
        {
            var parameters = new List<SqlParameter> { new SqlParameter("@Operation", "list") };
            return await GetListFromSp<Uye>("sp_Uye_Yonet", parameters);
        }
        public async Task UyeKaydetAsync(Uye uye, string operation)
        {
            var parameters = new List<SqlParameter>
            {
                new SqlParameter("@Operation", operation), new SqlParameter("@Id", uye.Id),
                new SqlParameter("@Ad", uye.Ad), new SqlParameter("@Soyad", uye.Soyad),
                new SqlParameter("@Email", uye.Email), new SqlParameter("@Telefon", (object)uye.Telefon ?? DBNull.Value),
                new SqlParameter("@TCKimlik", (object)uye.TCKimlik ?? DBNull.Value),
                new SqlParameter("@Adres", (object)uye.Adres ?? DBNull.Value), new SqlParameter("@Aktif", uye.Aktif)
            };
            await ExecuteSp("sp_Uye_Yonet", parameters);
        }
        public async Task UyeSilAsync(int id)
        {
            var parameters = new List<SqlParameter> { new SqlParameter("@Operation", "delete"), new SqlParameter("@Id", id) };
            await ExecuteSp("sp_Uye_Yonet", parameters);
        }

        // --- ÖDÜNÇ/İADE METOTLARI ---
        public async Task<List<OduncIslemi>> GetAktifOduncListesiAsync()
        {
            return await GetListFromSp<OduncIslemi>("sp_Odunc_Listele_Aktif", null);
        }
        public async Task<List<Kitap>> GetMusaitKitapListesiAsync()
        {
            return await GetListFromSp<Kitap>("sp_Kitap_Listele_Musait", null);
        }
        public async Task OduncVerAsync(int uyeId, int kitapId, DateTime iadeTarihi)
        {
            var parameters = new List<SqlParameter> { new SqlParameter("@UyeId", uyeId),
                new SqlParameter("@KitapId", kitapId), new SqlParameter("@IadeTarihi", iadeTarihi) };
            await ExecuteSp("sp_Odunc_Ver", parameters);
        }
        public async Task IadeAlAsync(int oduncId)
        {
            var parameters = new List<SqlParameter> { new SqlParameter("@OduncId", oduncId) };
            await ExecuteSp("sp_Iade_Al", parameters);
        }

        // --- CEZA METOTLARI ---
        public async Task<List<Ceza>> GetOdenmemisCezalarAsync()
        {
            return await GetListFromSp<Ceza>("sp_Ceza_Listele_Odenmemis", null);
        }
        public async Task CezaOdeAsync(int cezaId)
        {
            var parameters = new List<SqlParameter> { new SqlParameter("@CezaId", cezaId) };
            await ExecuteSp("sp_Ceza_Ode", parameters);
        }

        // --- REZERVASYON METOTLARI ---
        public async Task<List<Rezervasyon>> GetAktifRezervasyonlarAsync()
        {
            return await GetListFromSp<Rezervasyon>("sp_Rezervasyon_Listele_Aktif", null);
        }
        public async Task RezervasyonYapAsync(int uyeId, int kitapId)
        {
            var parameters = new List<SqlParameter> { new SqlParameter("@UyeId", uyeId), new SqlParameter("@KitapId", kitapId) };
            await ExecuteSp("sp_Rezervasyon_Yap", parameters);
        }
        public async Task RezervasyonIptalAsync(int rezervasyonId)
        {
            var parameters = new List<SqlParameter> { new SqlParameter("@RezervasyonId", rezervasyonId) };
            await ExecuteSp("sp_Rezervasyon_Iptal", parameters);
        }

        // --- DROPDOWN LİSTELERİ İÇİN ---
        public async Task<List<Yazar>> YazarListeleAsync()
        {
            return await GetListFromSp<Yazar>("sp_Yazar_Listele", null);
        }
        public async Task<List<Kategori>> KategoriListeleAsync()
        {
            return await GetListFromSp<Kategori>("sp_Kategori_Listele", null);
        }
    }
}