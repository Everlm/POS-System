using POS.Application.Documents.Bases;
using POS.Application.Documents.Static;
using POS.Application.Dtos.Category.Response;
using QuestPDF.Fluent;
using QuestPDF.Helpers;
using QuestPDF.Infrastructure;

namespace POS.Application.Documents.Category
{
    public class CategoryDocument : BaseDocument<IEnumerable<CategoryResponseDto>>
    {
        public CategoryDocument(IEnumerable<CategoryResponseDto> categories) : base(categories)
        {
        }

        protected override string GetTitle() => "REPORTE DE CATEGORÍAS";

        protected override void ComposeContent(IContainer container)
        {
            container.PaddingVertical(40).Table(table =>
            {
                table.ColumnsDefinition(columns =>
                {
                    columns.RelativeColumn(1);
                    columns.RelativeColumn(3);
                    columns.RelativeColumn(2);
                    columns.RelativeColumn(1);
                });

                table.Header(header =>
                {
                    header.Cell().Text("ID").Style(PdfStyles.Header);
                    header.Cell().Text("Nombre").Style(PdfStyles.Header);
                    header.Cell().Text("Descripción").Style(PdfStyles.Header);
                    header.Cell().Text("Estado").Style(PdfStyles.Header);
                });

                if (_data == null || !_data.Any())
                {
                    table.Cell().ColumnSpan(4).Element(CellStyle).AlignCenter()
                        .Text("No hay categorías registradas").Italic().FontColor(Colors.Grey.Darken2);
                    return;
                }

                foreach (var category in _data)
                {
                    table.Cell().Text(category.CategoryId.ToString());
                    table.Cell().Text(category.Name);
                    table.Cell().Text(category.Description);
                    table.Cell().Text(category.State == 1 ? "Activo" : "Inactivo");
                }
            });

        }
        private static IContainer CellStyle(IContainer container) =>
          container.Padding(5).BorderBottom(1).BorderColor(Colors.Grey.Lighten2);
    }
}
