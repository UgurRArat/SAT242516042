using Microsoft.EntityFrameworkCore;
using DbContexts;

namespace SAT242516042.Services;

public class DbLogger : ILoggerService
{
    private readonly MyDbModel_DbContext _context;

    public DbLogger(MyDbModel_DbContext context)
    {
        _context = context;
    }

    public async Task LogAsync(string tableName, string operation, int recordId, string details)
    {
        try
        {
            // SQL sorgusu ile SP çağırıyoruz
            var query = $"EXEC sp_System_Add_Log @TableName={{0}}, @Operation={{1}}, @RecordId={{2}}, @Details={{3}}";
            await _context.Database.ExecuteSqlRawAsync(query, tableName, operation, recordId, details);
        }
        catch (Exception)
        {
            // DB loglama hatası olursa uygulamayı kırmasın
        }
    }
}