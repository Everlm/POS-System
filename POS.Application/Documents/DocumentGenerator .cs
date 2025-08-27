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

        public async Task<byte[]> GeneratePdfAsync(IDocument document, CancellationToken ct = default)
        {
            return await Task.Run(() => GeneratePdf(document), ct);
        }

        public async Task<string> GeneratePdfBase64Async(IDocument document, CancellationToken ct = default)
        {
            var bytes = await GeneratePdfAsync(document, ct);
            return Convert.ToBase64String(bytes);
        }
    }
}