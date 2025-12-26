namespace SAT242516042.Services;

public class FileLogger : ILoggerService
{
    private readonly string _filePath;

    public FileLogger()
    {
        // Dosyayı projenin ana dizinine (wwwroot seviyesine) kaydeder
        _filePath = Path.Combine(Directory.GetCurrentDirectory(), "app_logs.txt");
    }

    public async Task LogAsync(string tableName, string operation, int recordId, string details)
    {
        try
        {
            var logMessage = $"[{DateTime.Now}] - Tablo: {tableName}, İşlem: {operation}, ID: {recordId}, Detay: {details}{Environment.NewLine}";

            // Asenkron olarak dosyaya ekle
            await File.AppendAllTextAsync(_filePath, logMessage);
        }
        catch (Exception)
        {
            // Dosya hatası yutulur
        }
    }
}