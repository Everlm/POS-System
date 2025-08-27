using POS.Application.Documents.Static;
using QuestPDF.Fluent;
using QuestPDF.Helpers;
using QuestPDF.Infrastructure;

namespace POS.Application.Documents.Bases
{
    public abstract class BaseDocument<T> : IDocument
    {
        protected readonly T _data;

        public BaseDocument(T data)
        {
            _data = data;
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
            container.Row(row =>
            {
                row.RelativeItem().Column(column =>
                {
                    column.Item().Text(GetTitle()).Style(PdfStyles.Title);
                    column.Item().Text($"Fecha de Reporte: {DateTime.Now:MM/dd/yyyy}").Style(PdfStyles.Header);
                });
                row.ConstantItem(100).Height(50).Placeholder();
            });
        }

        private void ComposeFooter(IContainer container)
        {
            container.AlignCenter().Text(text =>
            {
                text.Span("Página ").Style(PdfStyles.Small);
                text.CurrentPageNumber().Style(PdfStyles.Small);
                text.Span(" de ").Style(PdfStyles.Small);
                text.TotalPages().Style(PdfStyles.Small);
            });
        }

        protected abstract string GetTitle();
        protected abstract void ComposeContent(IContainer container);
    }
}