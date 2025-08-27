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

        // El método Compose tiene la estructura del PDF (Header, Content, Footer).
        public void Compose(IDocumentContainer container)
        {
            container.Page(page =>
            {
                page.Margin(50);
                page.Header().Element(ComposeHeader);
                // El método ComposeContent es abstracto y debe ser implementado por las clases hijas.
                page.Content().Element(ComposeContent); 
                page.Footer().Element(ComposeFooter);
            });
        }

        // Métodos privados para el encabezado y pie de página, que no cambian.
        private void ComposeHeader(IContainer container)
        {
            container.Row(row =>
            {
                row.RelativeItem().Column(column =>
                {
                    // Puedes usar un placeholder o una propiedad para el título
                    column.Item().Text(GetTitle()).FontSize(20).Bold().FontColor(Colors.Blue.Medium);
                    column.Item().Text($"Fecha de Reporte: {DateTime.Now:MM/dd/yyyy}").FontSize(14);
                });
                row.ConstantItem(100).Height(50).Placeholder(); // Placeholder para un logo
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

        // Métodos abstractos que deben ser implementados.
        // GetTitle() nos permite personalizar el título desde la clase hija.
        protected abstract string GetTitle();
        // ComposeContent es donde va la lógica de la tabla o el cuerpo.
        protected abstract void ComposeContent(IContainer container);
    }
}