using POS.Application.Dtos.Category.Response;
using QuestPDF.Fluent;
using QuestPDF.Helpers;
using QuestPDF.Infrastructure;

namespace POS.Application.Documents.Category
{
    public class CategoryDocument : IDocument
    {
        private readonly IEnumerable<CategoryResponseDto> _categories;
        public CategoryDocument(IEnumerable<CategoryResponseDto> categories)
        {
            _categories = categories;
        }

        public DocumentMetadata GetMetadata() => DocumentMetadata.Default;

        public void Compose(IDocumentContainer container)
        {
            container.Page(page =>
            {
                page.Margin(50);
                page.Header().Element(ComposeHeader);
                page.Content().Element(ComposeContent);
                page.Footer().Element(ComposeFooter);
            });
        }

        private void ComposeHeader(IContainer container)
        {
            // Lógica del encabezado del documento
            container.Row(row =>
            {
                row.RelativeItem().Column(column =>
                {
                    column.Item().Text("REPORTE DE CATEGORÍAS").FontSize(20).Bold().FontColor(Colors.Blue.Medium);
                    column.Item().Text($"Fecha de Reporte: {DateTime.Now:MM/dd/yyyy}").FontSize(14);
                });
                row.ConstantItem(100).Height(50).Placeholder(); // Placeholder para un logo
            });
        }

        private void ComposeContent(IContainer container)
        {
            // Lógica de composición del cuerpo con los datos de las categorías
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
                    header.Cell().Text("ID").Bold();
                    header.Cell().Text("Nombre").Bold();
                    header.Cell().Text("Descripción").Bold();
                    header.Cell().Text("Estado").Bold();
                });

                foreach (var category in _categories)
                {
                    table.Cell().Text(category.CategoryId.ToString());
                    table.Cell().Text(category.Name);
                    table.Cell().Text(category.Description);
                    table.Cell().Text(category.State == 1 ? "Activo" : "Inactivo");
                }
            });
        }

        private void ComposeFooter(IContainer container)
        {
            container.AlignCenter().Text(text =>
            {
                text.Span("Página ").FontSize(10);
                text.CurrentPageNumber().FontSize(10);
                text.Span(" de ").FontSize(10);
                text.TotalPages().FontSize(10);
            });
        }
    }
}
