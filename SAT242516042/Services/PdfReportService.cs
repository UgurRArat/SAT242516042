using QuestPDF.Fluent;
using QuestPDF.Helpers;
using QuestPDF.Infrastructure;
using MyDbModels; // Kategori ve diğer modellerin olduğu yer

namespace SAT242516042.Services;

public class PdfReportService
{
    // 1. KATEGORİ RAPORU
    public byte[] GenerateKategoriRaporu(List<Kategori> kategoriler)
    {
        return Document.Create(container =>
        {
            container.Page(page =>
            {
                page.Size(PageSizes.A4);
                page.Margin(2, Unit.Centimetre);
                page.PageColor(Colors.White);
                page.DefaultTextStyle(x => x.FontSize(12));

                page.Header()
                    .Text("Kategori Listesi Raporu")
                    .SemiBold().FontSize(20).FontColor(Colors.Blue.Medium);

                page.Content()
                    .PaddingVertical(1, Unit.Centimetre)
                    .Table(table =>
                    {
                        // Tablo Kolonları
                        table.ColumnsDefinition(columns =>
                        {
                            columns.ConstantColumn(50); // ID
                            columns.RelativeColumn();   // Ad
                            columns.RelativeColumn();   // Açıklama
                            columns.ConstantColumn(80); // Durum
                        });

                        // Başlıklar
                        table.Header(header =>
                        {
                            header.Cell().Element(CellStyle).Text("ID");
                            header.Cell().Element(CellStyle).Text("Kategori Adı");
                            header.Cell().Element(CellStyle).Text("Açıklama");
                            header.Cell().Element(CellStyle).Text("Durum");

                            static IContainer CellStyle(IContainer container)
                            {
                                return container.DefaultTextStyle(x => x.SemiBold()).PaddingVertical(5).BorderBottom(1).BorderColor(Colors.Black);
                            }
                        });

                        // Veriler
                        foreach (var item in kategoriler)
                        {
                            table.Cell().Element(CellStyle).Text(item.Id.ToString());
                            table.Cell().Element(CellStyle).Text(item.Ad);
                            table.Cell().Element(CellStyle).Text(item.Aciklama);
                            table.Cell().Element(CellStyle).Text(item.AktifMi ? "Aktif" : "Pasif");

                            static IContainer CellStyle(IContainer container)
                            {
                                return container.BorderBottom(1).BorderColor(Colors.Grey.Lighten2).PaddingVertical(5);
                            }
                        }
                    });

                page.Footer()
                    .AlignCenter()
                    .Text(x =>
                    {
                        x.Span("Sayfa ");
                        x.CurrentPageNumber();
                    });
            });
        })
        .GeneratePdf();
    }

    // 2. YAZAR RAPORU (Madde 24-a: İkinci tablo zorunluluğu için)
    // Şimdilik model olarak 'dynamic' veya Yazar sınıfını kullanabilirsin.
    // Mantık yukarıdakiyle aynıdır.
}