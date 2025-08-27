using QuestPDF.Fluent;
using QuestPDF.Infrastructure;

namespace POS.Application.Documents
{
    public class DocumentGenerator : IDocumentGenerator
    {
        public byte[] GeneratePdf(IDocument document)
        => document.GeneratePdf();

        public string GeneratePdfBase64(IDocument document)
            => Convert.ToBase64String(document.GeneratePdf());
    }
}