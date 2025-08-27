using POS.Application.Dtos.Category.Response;
using QuestPDF.Fluent;
using QuestPDF.Helpers;
using QuestPDF.Infrastructure;

namespace POS.Application.Documents.QuestPDF
{
    public class InvoiceDocument : BaseInvoiceDocument<CategoryResponseDto>
    {
        public InvoiceDocument(CategoryResponseDto data) : base(data) { }
        protected override void ComposeHeader(IContainer container)
        {
            // Implementación del encabezado usando _data
            container.Row(row =>
            {
                row.RelativeItem().Column(column =>
                {
                    column.Item().Text("INVOICE").FontSize(20).Bold().FontColor(Colors.Blue.Medium);
                    // column.Item().Text($"Invoice #{_data.InvoiceNumber}").FontSize(14);
                    column.Item().Text($"Invoice #{"0000000"}").FontSize(14);
                    // column.Item().Text($"Date: {_data.InvoiceDate:MM/dd/yyyy}").FontSize(14);
                    column.Item().Text($"Date: {_data.AuditCreateDate:MM/dd/yyyy}").FontSize(14);
                });
                row.ConstantItem(100).Height(50).Placeholder();
            });
        }

        protected override void ComposeContent(IContainer container)
        {
            // Implementación del cuerpo usando _data
            container.PaddingVertical(40).Column(column =>
            {
                column.Spacing(20);
                column.Item().Text($"Name: {_data.Name}").FontSize(12).Bold();
                column.Item().Text($"Description: {_data.Description}").FontSize(12);

            });
        }

        protected override void ComposeFooter(IContainer container)
        {
            // Implementación del pie de página
            container.Column(column =>
            {
                column.Item().AlignCenter().Text(text =>
                {
                    text.Span("Page ").FontSize(10);
                    text.CurrentPageNumber().FontSize(10);
                    text.Span(" of ").FontSize(10);
                    text.TotalPages().FontSize(10);
                });
            });
        }
    }
}