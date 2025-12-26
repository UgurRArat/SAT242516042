namespace SAT242516042.Services;

public interface ILoggerService
{
    // Loglama metodu: TabloAdı, İşlemTürü, KayıtID, DetayMesaj
    Task LogAsync(string tableName, string operation, int recordId, string details);
}