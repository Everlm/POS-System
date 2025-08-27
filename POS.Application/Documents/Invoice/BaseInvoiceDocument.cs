using QuestPDF.Fluent;
using QuestPDF.Infrastructure;

namespace POS.Application.Documents.Invoice
{
    public abstract class BaseInvoiceDocument<T> : IDocument
    {
        protected readonly T _data;

        protected BaseInvoiceDocument(T data)
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

        protected abstract void ComposeHeader(IContainer container);
        protected abstract void ComposeContent(IContainer container);
        protected abstract void ComposeFooter(IContainer container);
    }
}